import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import {
  ChatBubbleLeftRightIcon,
  ClockIcon,
  UserIcon,
  EyeIcon,
  HeartIcon,
  TagIcon,
  MagnifyingGlassIcon,
} from '@heroicons/react/24/outline';
import { formatRelativeTime, formatNumber } from '../utils';
import { topicApi, categoryApi } from '../services/api';
import type { TopicSummary, Category, TopicsListRequest } from '../types';
import { usePageTitle } from '../utils';

const TopicsPage: React.FC = () => {
  usePageTitle('Thảo luận gần đây');
  const [topics, setTopics] = useState<TopicSummary[]>([]);
  const [categories, setCategories] = useState<Category[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [searchQuery, setSearchQuery] = useState('');
  const [selectedCategory, setSelectedCategory] = useState('all');
  const [sortBy, setSortBy] = useState('latest');
  const [currentPage, setCurrentPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const [isLoadingMore, setIsLoadingMore] = useState(false);

  // Load initial data
  useEffect(() => {
    loadData();
  }, []);

  // Reload data when filters change (NOT searchQuery!)
  useEffect(() => {
    if (!isLoading) {
      setCurrentPage(1);
      loadData();
    }
  }, [selectedCategory, sortBy]);

  const loadData = async (page = 1, append = false) => {
    try {
      if (!append) {
        setIsLoading(true);
        setError(null);
      }

      // Build request parameters
      const params: Partial<TopicsListRequest> = {
        page,
        pageSize: 20,
        sortBy: sortBy === 'latest' ? 'createdAt' : sortBy === 'popular' ? 'viewCount' : 'postCount',
        sortOrder: 'desc',
      };

      if (searchQuery.trim()) {
        params.query = searchQuery.trim();
      }

      if (selectedCategory !== 'all') {
        const category = categories.find(c => c.slug === selectedCategory);
        if (category) {
          params.categoryId = category.id;
        }
      }

      // Load topics
      const topicsResponse = await topicApi.getTopics(params);

      if (topicsResponse.success) {
        if (append) {
          setTopics(prev => [...prev, ...topicsResponse.data.items]);
        } else {
          setTopics(topicsResponse.data.items);
        }
        setCurrentPage(topicsResponse.data.page);
        setTotalPages(topicsResponse.data.totalPages);
      }

      // Load categories on first page load
      if (page === 1) {
        const categoriesResponse = await categoryApi.getAllCategories();
        if (categoriesResponse.success) {
          setCategories(categoriesResponse.data);
        }
      }
    } catch (err) {
      setError('Không thể tải các cuộc thảo luận. Vui lòng thử lại.');
    } finally {
      setIsLoading(false);
    }
  };

  const loadMore = () => {
    if (currentPage < totalPages && !isLoading) {
      setIsLoadingMore(true);
      loadData(currentPage + 1, true).finally(() => setIsLoadingMore(false));
    }
  };

  const handleSearch = (e: React.FormEvent) => {
    e.preventDefault();
    setCurrentPage(1);
    loadData();
  };

  // Topics are already filtered and sorted by the API
  const displayTopics = topics;

  if (isLoading) {
    return (
      <div className="min-h-screen bg-gray-50">
        <div className="mx-auto max-w-7xl px-4 py-8 sm:px-6 lg:px-8">
          <div className="animate-pulse space-y-6">
            <div className="h-8 bg-gray-200 rounded w-1/4"></div>
            <div className="space-y-4">
              {[...Array(5)].map((_, i) => (
                <div key={i} className="bg-white p-6 rounded-lg shadow-sm">
                  <div className="h-6 bg-gray-200 rounded w-3/4 mb-4"></div>
                  <div className="h-4 bg-gray-200 rounded w-full mb-2"></div>
                  <div className="h-4 bg-gray-200 rounded w-2/3"></div>
                </div>
              ))}
            </div>
          </div>
        </div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="min-h-screen bg-gray-50 flex items-center justify-center">
        <div className="text-center">
          <h2 className="text-2xl font-bold text-gray-900 mb-4">Lỗi khi tải chủ đề</h2>
          <p className="text-gray-600">{error}</p>
        </div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gray-50">
      <div className="mx-auto max-w-7xl px-4 py-8 sm:px-6 lg:px-8">
        {/* Header */}
        <div className="mb-8">
          <h1 className="text-3xl font-bold text-gray-900 mb-4">Các cuộc thảo luận gần đây</h1>
          <p className="text-gray-600">
            Tham gia cuộc trò chuyện với các chuyên gia y tế và thành viên cộng đồng
          </p>
        </div>

        {/* Filters */}
        <div className="mb-8 space-y-4 lg:flex lg:items-center lg:justify-between lg:space-y-0">
          {/* Search */}
          <div className="relative max-w-md">
            <div className="flex">
              <button
                type="button"
                onClick={handleSearch}
                className="flex items-center justify-center px-3 py-2 bg-blue-50 hover:bg-blue-100 border border-r-0 border-gray-300 rounded-l-lg transition-colors duration-200"
              >
                <MagnifyingGlassIcon className="h-5 w-5 text-blue-600 hover:text-blue-700 transition-colors duration-200" />
              </button>
              <input
                type="text"
                value={searchQuery}
                onChange={(e) => setSearchQuery(e.target.value)}
                onKeyDown={(e) => e.key === 'Enter' && handleSearch(e as any)}
                className="flex-1 block w-full rounded-r-lg border border-gray-300 bg-white py-2 px-3 text-sm focus:border-primary-500 focus:outline-none focus:ring-1 focus:ring-primary-500"
                placeholder="Tìm kiếm các cuộc thảo luận..."
              />
            </div>
          </div>

          {/* Category Filter & Sort */}
          <div className="flex space-x-4">
            <select
              value={selectedCategory}
              onChange={(e) => setSelectedCategory(e.target.value)}
              className="rounded-lg border border-gray-300 bg-white px-3 py-2 text-sm focus:border-primary-500 focus:outline-none focus:ring-1 focus:ring-primary-500"
            >
              <option value="all">Tất cả danh mục</option>
              {categories.map((category) => (
                <option key={category.id} value={category.slug}>
                  {category.name}
                </option>
              ))}
            </select>

            <select
              value={sortBy}
              onChange={(e) => setSortBy(e.target.value)}
              className="rounded-lg border border-gray-300 bg-white px-3 py-2 text-sm focus:border-primary-500 focus:outline-none focus:ring-1 focus:ring-primary-500"
            >
              <option value="latest">Mới nhất</option>
              <option value="popular">Phổ biến nhất</option>
              <option value="discussed">Thảo luận nhiều nhất</option>
            </select>
          </div>
        </div>

        {/* Topics List */}
        <div className="space-y-6">
          {displayTopics.map((topic: TopicSummary) => (
            <div key={topic.id} className="bg-white rounded-lg shadow-sm border border-gray-200 p-6 hover:shadow-md transition-shadow">
              <div className="flex items-start justify-between">
                <div className="flex-1">
                  {/* Topic Title */}
                  <Link
                    to={`/topics/${topic.slug}`}
                    className="block text-xl font-semibold text-gray-900 hover:text-primary-600 transition-colors"
                  >
                    {topic.title}
                  </Link>

                  {/* Topic Category */}
                  <p className="mt-2 text-gray-600">
                    <Link
                      to={`/categories/${topic.categorySlug}`}
                      className="text-primary-600 hover:text-primary-800"
                    >
                      {topic.categoryName}
                    </Link>
                  </p>

                  {/* Tags */}
                  {topic.tags.length > 0 && (
                    <div className="mt-3 flex flex-wrap gap-2">
                      {topic.tags.map((tag: string) => (
                        <span
                          key={tag}
                          className="inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium bg-primary-100 text-primary-800"
                        >
                          <TagIcon className="w-3 h-3 mr-1" />
                          {tag}
                        </span>
                      ))}
                    </div>
                  )}

                  {/* Meta Info */}
                  <div className="mt-4 flex items-center space-x-6 text-sm text-gray-500">
                    {/* Author */}
                    <div className="flex items-center space-x-2">
                      {topic.authorAvatar ? (
                        <img
                          className="h-6 w-6 rounded-full"
                          src={topic.authorAvatar}
                          alt={topic.authorName}
                        />
                      ) : (
                        <div className="h-6 w-6 rounded-full bg-gray-300 flex items-center justify-center">
                          <UserIcon className="h-4 w-4 text-gray-600" />
                        </div>
                      )}
                      <span className="font-medium">{topic.authorName}</span>
                      {topic.authorRole === 'Doctor' && (
                        <span className="inline-flex items-center px-2 py-0.5 rounded text-xs font-medium bg-blue-100 text-blue-800">
                          Chuyên gia
                        </span>
                      )}
                    </div>

                    {/* Status */}
                    <div className="flex items-center space-x-2">
                      {topic.isPinned && (
                        <span className="inline-flex items-center px-2 py-0.5 rounded text-xs font-medium bg-yellow-100 text-yellow-800">
                          Đã ghim
                        </span>
                      )}
                      {topic.hasAnswer && (
                        <span className="inline-flex items-center px-2 py-0.5 rounded text-xs font-medium bg-green-100 text-green-800">
                          Đã trả lời
                        </span>
                      )}
                    </div>

                    {/* Time */}
                    <div className="flex items-center space-x-1">
                      <ClockIcon className="h-4 w-4" />
                      <span>{formatRelativeTime(topic.createdAt)}</span>
                    </div>
                  </div>
                </div>

                {/* Stats */}
                <div className="ml-6 flex flex-col items-center space-y-2 text-sm text-gray-500">
                  <div className="flex items-center space-x-1">
                    <EyeIcon className="h-4 w-4" />
                    <span>{formatNumber(topic.viewCount)}</span>
                  </div>
                  <div className="flex items-center space-x-1">
                    <HeartIcon className="h-4 w-4" />
                    <span>{formatNumber(topic.likeCount)}</span>
                  </div>
                  <div className="flex items-center space-x-1">
                    <ChatBubbleLeftRightIcon className="h-4 w-4" />
                    <span>{formatNumber(topic.postCount)}</span>
                  </div>
                </div>
              </div>
            </div>
          ))}
        </div>

        {/* Empty State */}
        {displayTopics.length === 0 && !isLoading && (
          <div className="text-center py-12">
            <ChatBubbleLeftRightIcon className="mx-auto h-12 w-12 text-gray-400" />
            <h3 className="mt-4 text-lg font-medium text-gray-900">Không tìm thấy cuộc thảo luận nào</h3>
            <p className="mt-2 text-gray-500">
              {searchQuery || selectedCategory !== 'all' 
                ? 'Hãy thử điều chỉnh tìm kiếm hoặc bộ lọc'
                : 'Hãy là người đầu tiên bắt đầu cuộc thảo luận!'
              }
            </p>
            <div className="mt-6">
              <Link
                to="/topics/create"
                className="inline-flex items-center px-4 py-2 border border-transparent text-sm font-medium rounded-md shadow-sm text-white bg-primary-600 hover:bg-primary-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-primary-500"
              >
                Đặt câu hỏi
              </Link>
            </div>
          </div>
        )}

        {/* Load More Button (for pagination) */}
        {displayTopics.length > 0 && currentPage < totalPages && (
          <div className="mt-8 text-center">
            <button 
              onClick={loadMore}
              disabled={isLoadingMore}
              className="inline-flex items-center px-6 py-3 border border-gray-300 shadow-sm text-base font-medium rounded-md text-gray-700 bg-white hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-primary-500 disabled:opacity-50"
            >
              {isLoadingMore ? 'Đang tải...' : 'Tải thêm cuộc thảo luận'}
            </button>
          </div>
        )}
      </div>
    </div>
  );
};

export default TopicsPage;
