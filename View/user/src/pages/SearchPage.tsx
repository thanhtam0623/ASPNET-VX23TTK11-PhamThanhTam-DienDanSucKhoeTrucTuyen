import React, { useState, useEffect } from 'react';
import { useSearchParams, Link } from 'react-router-dom';
import {
  MagnifyingGlassIcon,
  FunnelIcon,
  ChatBubbleLeftRightIcon,
  EyeIcon,
  HeartIcon,
  CheckCircleIcon,
  StarIcon,
  UserIcon,
} from '@heroicons/react/24/outline';
import { topicApi, categoryApi } from '../services/api';
import type { SearchRequest, TopicSummary, Category, PaginatedResponse } from '../types';
import {
  formatRelativeTime,
  formatNumber,
  getRoleBadgeColor,
  cn,
} from '../utils';
import { usePageTitle } from '../utils';

const SearchPage: React.FC = () => {
  usePageTitle('Tìm kiếm chủ đề sức khỏe');
  const [searchParams, setSearchParams] = useSearchParams();
  
  const [searchQuery, setSearchQuery] = useState(searchParams.get('q') || '');
  const [categories, setCategories] = useState<Category[]>([]);
  const [results, setResults] = useState<TopicSummary[]>([]);
  const [pagination, setPagination] = useState<PaginatedResponse<TopicSummary> | null>(null);
  const [isLoading, setIsLoading] = useState(false);
  const [showFilters, setShowFilters] = useState(false);
  
  const [filters, setFilters] = useState<SearchRequest>({
    query: searchParams.get('q') || '',
    page: parseInt(searchParams.get('page') || '1'),
    pageSize: 20,
    sortBy: (searchParams.get('sort') as any) || 'newest',
    categoryId: searchParams.get('category') ? parseInt(searchParams.get('category')!) : undefined,
    hasAnswer: searchParams.get('answered') === 'true' ? true : searchParams.get('answered') === 'false' ? false : undefined,
  });

  useEffect(() => {
    fetchCategories();
    if (filters.query || filters.categoryId) {
      performSearch();
    }
  }, []);

  useEffect(() => {
    updateURLParams();
    if (filters.query || filters.categoryId) {
      performSearch();
    }
  }, [filters]);

  const fetchCategories = async () => {
    try {
      const response = await categoryApi.getAllCategories();
      if (response.success) {
        setCategories(response.data);
      }
    } catch (err) {
      // Handle categories loading error
    }
  };

  const performSearch = async () => {
    if (!filters.query?.trim() && !filters.categoryId) return;
    
    try {
      setIsLoading(true);
      const response = await topicApi.searchTopics(filters);
      if (response.success) {
        setResults(response.data.items);
        setPagination(response.data);
      }
    } catch (err) {
      // Handle search error
    } finally {
      setIsLoading(false);
    }
  };

  const updateURLParams = () => {
    const params = new URLSearchParams();
    if (filters.query) params.set('q', filters.query);
    if (filters.categoryId) params.set('category', filters.categoryId.toString());
    if (filters.hasAnswer !== undefined) params.set('answered', filters.hasAnswer.toString());
    if (filters.sortBy !== 'newest') params.set('sort', filters.sortBy!);
    if (filters.page > 1) params.set('page', filters.page.toString());
    
    setSearchParams(params);
  };

  const handleSearch = (e: React.FormEvent) => {
    e.preventDefault();
    setFilters(prev => ({
      ...prev,
      query: searchQuery,
      page: 1,
    }));
  };

  const handleFilterChange = (key: keyof SearchRequest, value: any) => {
    setFilters(prev => ({
      ...prev,
      [key]: value,
      page: key !== 'page' ? 1 : value,
    }));
  };

  return (
    <div className="mx-auto max-w-6xl px-4 py-8 sm:px-6 lg:px-8">
      {/* Header */}
      <div className="mb-8">
        <h1 className="text-3xl font-bold text-gray-900 flex items-center gap-3">
          <MagnifyingGlassIcon className="h-8 w-8 text-primary-600" />
          Tìm kiếm chủ đề sức khỏe
        </h1>
        <p className="mt-2 text-gray-600">
          Tìm câu trả lời cho các câu hỏi sức khỏe của bạn từ cộng đồng của chúng tôi.
        </p>
      </div>

      {/* Search Form */}
      <div className="mb-6 rounded-xl border border-gray-200 bg-white p-6 shadow-sm">
        <form onSubmit={handleSearch} className="space-y-4">
          <div className="flex space-x-4">
            <div className="flex-1">
              <div className="relative">
                <MagnifyingGlassIcon className="absolute left-3 top-1/2 h-5 w-5 -translate-y-1/2 text-gray-400" />
                <input
                  type="text"
                  value={searchQuery}
                  onChange={(e) => setSearchQuery(e.target.value)}
                  placeholder="Tìm kiếm triệu chứng, bệnh trạng, phương pháp điều trị..."
                  className="w-full rounded-lg border border-gray-300 py-3 pl-10 pr-4 focus:border-primary-500 focus:outline-none focus:ring-1 focus:ring-primary-500"
                />
              </div>
            </div>
            <button
              type="submit"
              className="flex items-center rounded-lg bg-primary-600 px-6 py-3 text-white hover:bg-primary-700"
            >
              <MagnifyingGlassIcon className="mr-2 h-5 w-5" />
              Tìm kiếm
            </button>
          </div>

          <div className="flex items-center space-x-4">
            <button
              type="button"
              onClick={() => setShowFilters(!showFilters)}
              className="flex items-center space-x-2 rounded-lg border border-gray-300 bg-white px-3 py-2 text-sm font-medium text-gray-700 hover:bg-gray-50"
            >
              <FunnelIcon className="h-4 w-4" />
              <span>Bộ lọc nâng cao</span>
            </button>
          </div>
        </form>
      </div>

      {/* Advanced Filters */}
      {showFilters && (
        <div className="mb-6 rounded-lg border border-gray-200 bg-white p-4">
          <div className="grid grid-cols-1 gap-4 sm:grid-cols-3">
            <div>
              <label className="mb-2 block text-sm font-medium text-gray-700">
                Danh mục
              </label>
              <select
                value={filters.categoryId || ''}
                onChange={e => handleFilterChange('categoryId', e.target.value ? parseInt(e.target.value) : undefined)}
                className="w-full rounded-lg border border-gray-300 bg-white px-3 py-2 text-sm focus:border-primary-500 focus:outline-none focus:ring-1 focus:ring-primary-500"
              >
                <option value="">Tất cả danh mục</option>
                {categories.map(category => (
                  <option key={category.id} value={category.id}>
                    {category.name}
                  </option>
                ))}
              </select>
            </div>

            <div>
              <label className="mb-2 block text-sm font-medium text-gray-700">
                Trạng thái trả lời
              </label>
              <select
                value={filters.hasAnswer === undefined ? '' : filters.hasAnswer.toString()}
                onChange={e => handleFilterChange('hasAnswer', e.target.value === '' ? undefined : e.target.value === 'true')}
                className="w-full rounded-lg border border-gray-300 bg-white px-3 py-2 text-sm focus:border-primary-500 focus:outline-none focus:ring-1 focus:ring-primary-500"
              >
                <option value="">Tất cả chủ đề</option>
                <option value="true">Có trả lời</option>
                <option value="false">Chưa có trả lời</option>
              </select>
            </div>

            <div>
              <label className="mb-2 block text-sm font-medium text-gray-700">
                Sắp xếp theo
              </label>
              <select
                value={filters.sortBy || 'newest'}
                onChange={e => handleFilterChange('sortBy', e.target.value)}
                className="w-full rounded-lg border border-gray-300 bg-white px-3 py-2 text-sm focus:border-primary-500 focus:outline-none focus:ring-1 focus:ring-primary-500"
              >
                <option value="newest">Mới nhất</option>
                <option value="popular">Phổ biến nhất</option>
                <option value="answered">Trả lời gần đây</option>
              </select>
            </div>
          </div>
        </div>
      )}

      {/* Results */}
      <div className="space-y-4">
        {/* Results Header */}
        {pagination && (
          <div className="flex items-center justify-between">
            <h2 className="text-lg font-medium text-gray-900">
              {pagination.totalItems === 0 
                ? 'Không tìm thấy kết quả' 
                : `Tìm thấy ${formatNumber(pagination.totalItems)} kết quả`
              }
            </h2>
          </div>
        )}

        {/* Loading State */}
        {isLoading && (
          <div className="flex justify-center py-12">
            <div className="h-8 w-8 animate-spin rounded-full border-b-2 border-primary-600"></div>
          </div>
        )}

        {/* Empty State */}
        {!isLoading && results.length === 0 && filters.query && (
          <div className="rounded-lg bg-gray-50 p-12 text-center">
            <MagnifyingGlassIcon className="mx-auto h-12 w-12 text-gray-400" />
            <h3 className="mt-4 text-lg font-medium text-gray-900">Không tìm thấy kết quả</h3>
            <p className="mt-2 text-gray-600">
              Hãy thử điều chỉnh từ khóa tìm kiếm hoặc bộ lọc của bạn.
            </p>
            <Link
              to="/categories"
              className="mt-4 inline-flex items-center rounded-lg bg-primary-600 px-4 py-2 text-white hover:bg-primary-700"
            >
              Duyệt danh mục
            </Link>
          </div>
        )}

        {/* Search Results */}
        {!isLoading && results.length > 0 && (
          <>
            {results.map(topic => (
              <div
                key={topic.id}
                className="rounded-lg border border-gray-200 bg-white p-6 shadow-sm hover:border-gray-300 transition-all duration-200"
              >
                <div className="flex items-start justify-between">
                  <div className="flex-1 min-w-0">
                    <div className="flex items-center space-x-2 mb-2">
                      {topic.isPinned && <StarIcon className="h-4 w-4 text-yellow-500" />}
                      {topic.hasAnswer && <CheckCircleIcon className="h-4 w-4 text-green-500" />}
                      <Link
                        to={`/topics/${topic.slug}`}
                        className="text-lg font-semibold text-gray-900 hover:text-primary-600 transition-colors duration-200"
                      >
                        {topic.title}
                      </Link>
                    </div>
                    
                    <div className="flex flex-wrap items-center gap-4 mb-3 text-sm text-gray-600">
                      <div className="flex items-center space-x-2">
                        {topic.authorAvatar ? (
                          <img
                            src={topic.authorAvatar}
                            alt={topic.authorName}
                            className="h-6 w-6 rounded-full object-cover"
                          />
                        ) : (
                          <div className="flex h-6 w-6 items-center justify-center rounded-full bg-gray-100">
                            <UserIcon className="h-4 w-4 text-gray-600" />
                          </div>
                        )}
                        <Link
                          to={`/users/${topic.authorName}`}
                          className="font-medium hover:text-primary-600"
                        >
                          {topic.authorName}
                        </Link>
                        <span
                          className={cn(
                            'inline-flex items-center rounded px-2 py-0.5 text-xs font-medium',
                            getRoleBadgeColor(topic.authorRole)
                          )}
                        >
                          {topic.authorRole}
                        </span>
                      </div>
                      <span>•</span>
                      <Link
                        to={`/categories/${topic.categorySlug}`}
                        className="text-primary-600 hover:text-primary-700"
                      >
                        {topic.categoryName}
                      </Link>
                      <span>•</span>
                      <span>{formatRelativeTime(topic.createdAt)}</span>
                    </div>

                    {/* Stats */}
                    <div className="flex items-center space-x-6 text-sm text-gray-500">
                      <div className="flex items-center space-x-1">
                        <ChatBubbleLeftRightIcon className="h-4 w-4" />
                        <span>{formatNumber(topic.postCount)} phản hồi</span>
                      </div>
                      <div className="flex items-center space-x-1">
                        <HeartIcon className="h-4 w-4" />
                        <span>{formatNumber(topic.likeCount)} lượt thích</span>
                      </div>
                      <div className="flex items-center space-x-1">
                        <EyeIcon className="h-4 w-4" />
                        <span>{formatNumber(topic.viewCount)} lượt xem</span>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            ))}
          </>
        )}

        {/* Initial State */}
        {!isLoading && !filters.query && !filters.categoryId && (
          <div className="rounded-lg bg-gradient-to-br from-primary-50 to-secondary-50 p-12 text-center">
            <MagnifyingGlassIcon className="mx-auto h-16 w-16 text-primary-600" />
            <h3 className="mt-4 text-xl font-medium text-gray-900">Tìm kiếm cộng đồng sức khỏe của chúng tôi</h3>
            <p className="mt-2 text-gray-600 max-w-2xl mx-auto">
              Tìm câu trả lời cho các câu hỏi sức khỏe của bạn từ hàng nghìn cuộc thảo luận với bệnh nhân, 
              người chăm sóc và các chuyên gia y tế.
            </p>
            <div className="mt-6 flex justify-center space-x-4">
              <Link
                to="/categories"
                className="inline-flex items-center rounded-lg bg-primary-600 px-4 py-2 text-white hover:bg-primary-700"
              >
                Duyệt danh mục
              </Link>
              <Link
                to="/topics/create"
                className="inline-flex items-center rounded-lg border border-gray-300 bg-white px-4 py-2 text-gray-700 hover:bg-gray-50"
              >
                Đặt câu hỏi
              </Link>
            </div>
          </div>
        )}
      </div>
    </div>
  );
};

export default SearchPage;
