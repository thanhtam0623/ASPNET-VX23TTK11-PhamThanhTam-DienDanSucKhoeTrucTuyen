import React, { useState, useEffect } from 'react';
import { useParams, Link } from 'react-router-dom';
import {
  EyeIcon,
  ChatBubbleLeftRightIcon,
  StarIcon,
  HeartIcon,
  UserIcon,
  CheckCircleIcon,
} from '@heroicons/react/24/outline';
import { categoryApi } from '../services/api';
import type { Category, TopicSummary } from '../types';
import {
  formatRelativeTime,
  formatNumber,
  getRoleBadgeColor,
  cn,
} from '../utils';

interface CategoryTopicsRequest {
  Page: number;
  PageSize: number;
  SortBy?: string;
  Filter?: string;
  TagIds?: number[];
}

const CategoryPage: React.FC = () => {
  const { slug } = useParams<{ slug: string }>();
  const [category, setCategory] = useState<Category | null>(null);
  const [topics, setTopics] = useState<TopicSummary[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [isLoadingTopics, setIsLoadingTopics] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [filters, setFilters] = useState<CategoryTopicsRequest>({
    Page: 1,
    PageSize: 20,
    SortBy: 'created_at',
  });

  useEffect(() => {
    if (slug) {
      fetchCategoryData();
    }
  }, [slug]);

  useEffect(() => {
    if (category) {
      fetchCategoryTopics();
    }
  }, [category, filters]);

  const fetchCategoryData = async () => {
    try {
      setIsLoading(true);
      const response = await categoryApi.getCategoryBySlug(slug!);
      if (response.success) {
        setCategory(response.data);
      } else {
        setError(response.message);
      }
    } catch (err: any) {
      setError(err.message || 'Không thể tải danh mục');
    } finally {
      setIsLoading(false);
    }
  };

  const fetchCategoryTopics = async () => {
    try {
      setIsLoadingTopics(true);
      console.log('Fetching category topics for slug:', slug, 'with filters:', filters);
      
      const response = await categoryApi.getCategoryTopics(slug!, filters);
      console.log('Category topics API response:', response);
      
      if (response.success && response.data) {
        const categoryPageData = response.data;
        console.log('Category page data:', categoryPageData);
        
        // Check both uppercase and lowercase variants (cast to any to avoid TS error)
        const topicsArray = categoryPageData?.Topics || (categoryPageData as any)?.topics;
        
        if (topicsArray && Array.isArray(topicsArray)) {
          console.log('Setting topics:', topicsArray);
          console.log('Topics detail:', topicsArray.map(t => ({ id: t.id, title: t.title, slug: t.slug })));
          setTopics(topicsArray);
        } else {
          console.log('No topics found or invalid Topics array, setting empty');
          console.log('Available keys in categoryPageData:', Object.keys(categoryPageData));
          setTopics([]);
        }
      } else {
        console.log('API call failed or no data, setting empty topics');
        setTopics([]);
      }
    } catch (err) {
      console.error('Error fetching category topics:', err);
      setTopics([]);
    } finally {
      setIsLoadingTopics(false);
    }
  };

  const handleFilterChange = (key: keyof CategoryTopicsRequest, value: any) => {
    setFilters(prev => ({
      ...prev,
      [key]: value,
      Page: key !== 'Page' ? 1 : value,
    }));
  };

  if (isLoading) {
    return (
      <div className="flex min-h-screen items-center justify-center">
        <div className="h-12 w-12 animate-spin rounded-full border-b-2 border-primary-600"></div>
      </div>
    );
  }

  if (error || !category) {
    return (
      <div className="flex min-h-screen items-center justify-center">
        <div className="text-center">
          <h2 className="mb-2 text-xl font-semibold text-gray-900">
            Không tìm thấy danh mục
          </h2>
          <p className="text-gray-600">{error}</p>
          <Link
            to="/"
            className="mt-4 inline-block rounded-lg bg-primary-600 px-4 py-2 text-white hover:bg-primary-700"
          >
            Về trang chủ
          </Link>
        </div>
      </div>
    );
  }

  return (
    <div className="mx-auto max-w-6xl px-4 py-8 sm:px-6 lg:px-8">
      {/* Breadcrumb */}
      <nav className="mb-6 text-sm text-gray-500">
        <Link to="/" className="hover:text-primary-600">Trang chủ</Link>
        <span className="mx-2">›</span>
        <Link to="/categories" className="hover:text-primary-600">Danh mục sức khỏe</Link>
        <span className="mx-2">›</span>
        <span className="text-gray-900">{category.name}</span>
      </nav>

      {/* Category Header */}
      <div className="mb-8 rounded-xl border border-gray-200 bg-white p-6 shadow-sm">
        <div className="flex items-start space-x-4">
          {category.icon && (
            <div
              className="flex h-16 w-16 items-center justify-center rounded-xl text-2xl text-white"
              style={{ backgroundColor: category.color || '#3B82F6' }}
            >
              {category.icon}
            </div>
          )}
          <div className="flex-1">
            <h1 className="mb-2 text-3xl font-bold text-gray-900">{category.name}</h1>
            <p className="mb-4 text-gray-600">{category.description}</p>
            
            <div className="flex flex-wrap items-center gap-6 text-sm text-gray-500">
              <div className="flex items-center space-x-1">
                <ChatBubbleLeftRightIcon className="h-4 w-4" />
                <span>{formatNumber(category.topicCount)} chủ đề</span>
              </div>
              <div className="flex items-center space-x-1">
                <ChatBubbleLeftRightIcon className="h-4 w-4" />
                <span>{formatNumber(category.postCount)} bài viết</span>
              </div>
            </div>
          </div>
          
          <Link
            to="/topics/create"
            className="flex items-center rounded-lg bg-primary-600 px-4 py-2 text-white hover:bg-primary-700"
          >
            <ChatBubbleLeftRightIcon className="mr-2 h-4 w-4" />
            Đặt câu hỏi
          </Link>
        </div>
      </div>

      {/* Sort Controls */}
      <div className="mb-6 flex items-center justify-between">
        <h2 className="text-xl font-semibold text-gray-900">Thảo luận</h2>
        <div className="flex items-center space-x-2">
          <span className="text-sm text-gray-500">Sắp xếp theo:</span>
          <select
            value={filters.SortBy}
            onChange={e => handleFilterChange('SortBy', e.target.value)}
            className="rounded-lg border border-gray-300 bg-white px-3 py-2 text-sm focus:border-primary-500 focus:outline-none focus:ring-1 focus:ring-primary-500"
          >
            <option value="created_at">Mới nhất</option>
            <option value="view_count">Phổ biến nhất</option>
            <option value="last_activity">Vừa được trả lời</option>
            <option value="like_count">Nhiều phản hồi nhất</option>
          </select>
        </div>
      </div>

      {/* Topics List */}
      <div className="space-y-4">
        {isLoadingTopics ? (
          <div className="flex justify-center py-12">
            <div className="h-8 w-8 animate-spin rounded-full border-b-2 border-primary-600"></div>
          </div>
        ) : topics.length === 0 ? (
          <div className="rounded-lg bg-gray-50 p-12 text-center">
            <ChatBubbleLeftRightIcon className="mx-auto h-12 w-12 text-gray-400" />
            <h3 className="mt-4 text-lg font-medium text-gray-900">Không tìm thấy chủ đề nào</h3>
            <p className="mt-2 text-gray-600">Hãy là người đầu tiên bắt đầu thảo luận trong danh mục này.</p>
            <Link
              to="/topics/create"
              className="mt-4 inline-flex items-center rounded-lg bg-primary-600 px-4 py-2 text-white hover:bg-primary-700"
            >
              <ChatBubbleLeftRightIcon className="mr-2 h-4 w-4" />
              Bắt đầu thảo luận đầu tiên
            </Link>
          </div>
        ) : (
          topics.map(topic => (
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
                    <span>{formatRelativeTime(topic.createdAt)}</span>
                  </div>

                  {/* Tags */}
                  {topic.tags.length > 0 && (
                    <div className="flex flex-wrap gap-1 mb-3">
                      {topic.tags.slice(0, 3).map((tag, index) => (
                        <span
                          key={index}
                          className="inline-flex items-center rounded bg-gray-100 px-2 py-0.5 text-xs font-medium text-gray-700"
                        >
                          {tag}
                        </span>
                      ))}
                      {topic.tags.length > 3 && (
                        <span className="text-xs text-gray-500">
                          +{topic.tags.length - 3} khác
                        </span>
                      )}
                    </div>
                  )}

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
          ))
        )}
      </div>
    </div>
  );
};

export default CategoryPage;
