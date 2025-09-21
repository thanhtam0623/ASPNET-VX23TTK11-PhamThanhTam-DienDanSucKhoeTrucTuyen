import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import {
  ChatBubbleLeftRightIcon,
  UserGroupIcon,
  DocumentTextIcon,
  ChartBarIcon,
  ArrowTrendingUpIcon,
  ClockIcon,
  StarIcon,
  UserIcon,
  LockClosedIcon,
  GlobeAltIcon,
  MagnifyingGlassIcon,
  BookOpenIcon,
  ExclamationTriangleIcon,
} from '@heroicons/react/24/outline';
import { homeApi } from '../services/api';
import { useAuth } from '../context/AuthContext';
import type { HomePageData, TopicSummary } from '../types';
import {
  formatRelativeTime,
  formatNumber,
  getRoleBadgeColor,
  cn,
  usePageTitle
} from '../utils';

const HomePage: React.FC = () => {
  usePageTitle('Trang chủ');
  const { user, isAuthenticated } = useAuth();
  const [homeData, setHomeData] = useState<HomePageData | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchHomeData = async () => {
      try {
        setIsLoading(true);
        const response = await homeApi.getHomePage();
        if (response.success) {
          setHomeData(response.data);
        } else {
          setError(response.message);
        }
      } catch (err: any) {
        setError(err.message || 'Failed to load homepage data');
      } finally {
        setIsLoading(false);
      }
    };

    fetchHomeData();
  }, []);

  if (isLoading) {
    return (
      <div className="flex min-h-screen items-center justify-center">
        <div className="h-12 w-12 animate-spin rounded-full border-b-2 border-primary-600"></div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="flex min-h-screen items-center justify-center">
        <div className="text-center">
          <h2 className="mb-2 text-xl font-semibold text-gray-900">
            Đã xảy ra lỗi
          </h2>
          <p className="text-gray-600">{error}</p>
        </div>
      </div>
    );
  }

  if (!homeData) {
    return null;
  }

  return (
    <div className="mx-auto max-w-7xl px-4 py-8 sm:px-6 lg:px-8">
      {/* Modern Hero Section */}
      <div className="relative overflow-hidden">
        {/* Background Elements */}
        <div className="absolute inset-0 bg-gradient-to-br from-primary-50 via-white to-secondary-50">
          <div className="absolute top-0 -left-4 w-72 h-72 bg-primary-300 rounded-full mix-blend-multiply filter blur-xl opacity-20 animate-blob"></div>
          <div className="absolute top-0 -right-4 w-72 h-72 bg-secondary-300 rounded-full mix-blend-multiply filter blur-xl opacity-20 animate-blob animation-delay-2000"></div>
          <div className="absolute -bottom-8 left-20 w-72 h-72 bg-accent-orange rounded-full mix-blend-multiply filter blur-xl opacity-20 animate-blob animation-delay-4000"></div>
        </div>
        
        <div className="relative px-6 py-16 sm:py-20 lg:px-8">
          <div className="mx-auto max-w-4xl text-center">
            {/* Main Heading */}
            <div className="mb-8">
              {isAuthenticated && user ? (
                <>
                  <p className="text-2xl font-medium text-transparent bg-clip-text bg-gradient-to-r from-primary-600 to-secondary-600 mb-4 sm:text-3xl">
                    Chào mừng trở lại,
                  </p>
                  <h1 className="text-4xl font-bold tracking-tight text-gray-900 sm:text-5xl lg:text-6xl">
                    {user.fullName}!
                  </h1>
                </>
              ) : (
                <h1 className="text-4xl font-bold tracking-tight text-gray-900 sm:text-5xl lg:text-6xl leading-tight">
                  <span className="block text-transparent bg-clip-text bg-gradient-to-r from-primary-600 to-secondary-600 mb-3">
                    Cộng đồng Y tế
                  </span>
                  <span className="block text-gray-900">Đáng tin cậy</span>
                </h1>
              )}
            </div>

            {/* Subtitle */}
            <p className="mx-auto max-w-3xl text-xl leading-8 text-gray-600 sm:text-2xl">
              {isAuthenticated && user ? (
                `Chúng tôi rất vui mừng được chào đón bạn trong cộng đồng y tế của chúng tôi. Hãy tiếp tục khám phá và chia sẻ kiến thức về sức khỏe.`
              ) : (
                'Kết nối với bệnh nhân, người chăm sóc và các chuyên gia y tế. Chia sẻ kinh nghiệm, tìm kiếm lời khuyên và nhận hỗ trợ trong hành trình sức khỏe của bạn.'
              )}
            </p>

            {/* CTA Buttons */}
            <div className="mt-10 flex items-center justify-center gap-x-6">
              <Link
                to={isAuthenticated ? "/topics/create" : "/register"}
                className="group relative inline-flex items-center justify-center rounded-full bg-gradient-to-r from-primary-600 to-secondary-600 px-8 py-4 text-lg font-semibold text-white shadow-lg transition-all duration-300 hover:shadow-xl hover:scale-105 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-primary-600"
              >
                <ChatBubbleLeftRightIcon className="mr-2 h-5 w-5" />
                {isAuthenticated ? "Đặt câu hỏi về Sức khỏe" : "Tham gia Cộng đồng"}
                <div className="absolute inset-0 rounded-full bg-white opacity-0 transition-opacity duration-300 group-hover:opacity-10"></div>
              </Link>
              <Link
                to="/topics"
                className="inline-flex items-center justify-center rounded-full border-2 border-gray-300 bg-white px-8 py-4 text-lg font-semibold text-gray-900 shadow-sm transition-all duration-300 hover:border-gray-400 hover:shadow-lg hover:scale-105 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-gray-600"
              >
                <MagnifyingGlassIcon className="mr-2 h-5 w-5" />
                Duyệt Thảo luận
              </Link>
            </div>

            {/* Trust Indicators */}
            <div className="mt-12 grid grid-cols-1 gap-4 sm:grid-cols-3">
              <div className="group rounded-2xl border border-gray-200 bg-white/50 backdrop-blur-sm p-6 shadow-sm transition-all duration-300 hover:shadow-lg hover:border-primary-200">
                <div className="flex items-center justify-center w-12 h-12 mx-auto mb-4 rounded-xl bg-gradient-to-br from-green-500 to-emerald-600 text-white shadow-lg group-hover:scale-110 transition-transform duration-300">
                  <UserIcon className="h-6 w-6" />
                </div>
                <h3 className="text-lg font-semibold text-gray-900 mb-2">Chuyên gia Y tế</h3>
                <p className="text-sm text-gray-600">Các chuyên gia y tế được xác minh cung cấp lời khuyên đáng tin cậy</p>
              </div>
              
              <div className="group rounded-2xl border border-gray-200 bg-white/50 backdrop-blur-sm p-6 shadow-sm transition-all duration-300 hover:shadow-lg hover:border-primary-200">
                <div className="flex items-center justify-center w-12 h-12 mx-auto mb-4 rounded-xl bg-gradient-to-br from-blue-500 to-primary-600 text-white shadow-lg group-hover:scale-110 transition-transform duration-300">
                  <LockClosedIcon className="h-6 w-6" />
                </div>
                <h3 className="text-lg font-semibold text-gray-900 mb-2">Bảo vệ Quyền riêng tư</h3>
                <p className="text-sm text-gray-600">Nền tảng tuân thủ HIPAA đảm bảo bảo mật dữ liệu của bạn</p>
              </div>
              
              <div className="group rounded-2xl border border-gray-200 bg-white/50 backdrop-blur-sm p-6 shadow-sm transition-all duration-300 hover:shadow-lg hover:border-primary-200">
                <div className="flex items-center justify-center w-12 h-12 mx-auto mb-4 rounded-xl bg-gradient-to-br from-orange-500 to-red-600 text-white shadow-lg group-hover:scale-110 transition-transform duration-300">
                  <GlobeAltIcon className="h-6 w-6" />
                </div>
                <h3 className="text-lg font-semibold text-gray-900 mb-2">Hỗ trợ Toàn cầu</h3>
                <p className="text-sm text-gray-600">Hỗ trợ cộng đồng 24/7 từ khắp nơi trên thế giới</p>
              </div>
            </div>
          </div>
        </div>
      </div>

      {/* Stats Section */}
      <div className="mb-12 grid grid-cols-2 gap-6 md:grid-cols-4">
        <div className="rounded-xl border border-gray-200 bg-white p-6 text-center shadow-sm">
          <div className="mb-4 inline-flex h-12 w-12 items-center justify-center rounded-lg bg-blue-100">
            <DocumentTextIcon className="h-6 w-6 text-blue-600" />
          </div>
          <div className="mb-1 text-2xl font-bold text-gray-900">
            {formatNumber(homeData.siteStats.totalTopics)}
          </div>
          <div className="text-sm text-gray-600">Chủ đề Sức khỏe</div>
        </div>

        <div className="rounded-xl border border-gray-200 bg-white p-6 text-center shadow-sm">
          <div className="mb-4 inline-flex h-12 w-12 items-center justify-center rounded-lg bg-green-100">
            <ChatBubbleLeftRightIcon className="h-6 w-6 text-green-600" />
          </div>
          <div className="mb-1 text-2xl font-bold text-gray-900">
            {formatNumber(homeData.siteStats.totalPosts)}
          </div>
          <div className="text-sm text-gray-600">Thảo luận</div>
        </div>

        <div className="rounded-xl border border-gray-200 bg-white p-6 text-center shadow-sm">
          <div className="mb-4 inline-flex h-12 w-12 items-center justify-center rounded-lg bg-purple-100">
            <UserGroupIcon className="h-6 w-6 text-purple-600" />
          </div>
          <div className="mb-1 text-2xl font-bold text-gray-900">
            {formatNumber(homeData.siteStats.totalUsers)}
          </div>
          <div className="text-sm text-gray-600">Thành viên Cộng đồng</div>
        </div>

        <div className="rounded-xl border border-gray-200 bg-white p-6 text-center shadow-sm">
          <div className="mb-4 inline-flex h-12 w-12 items-center justify-center rounded-lg bg-orange-100">
            <ChartBarIcon className="h-6 w-6 text-orange-600" />
          </div>
          <div className="mb-1 text-2xl font-bold text-gray-900">
            {formatNumber(homeData.siteStats.onlineUsers)}
          </div>
          <div className="text-sm text-gray-600">Trực tuyến</div>
        </div>
      </div>

      <div className="grid grid-cols-1 gap-8 lg:grid-cols-3">
        {/* Main Content */}
        <div className="space-y-8 lg:col-span-2">
          {/* Categories Section */}
          <div className="rounded-xl border border-gray-200 bg-white p-6 shadow-sm">
            <div className="mb-6 flex items-center justify-between">
              <h2 className="text-xl font-semibold text-gray-900">
                Danh mục Sức khỏe
              </h2>
              <Link
                to="/categories"
                className="text-sm font-medium text-primary-600 hover:text-primary-700"
              >
                Xem tất cả
              </Link>
            </div>
            <div className="grid grid-cols-1 gap-4 md:grid-cols-2">
              {homeData.categories.slice(0, 6).map(category => (
                <Link
                  key={category.id}
                  to={`/categories/${category.slug}`}
                  className="block rounded-lg border border-gray-200 p-4 transition-all duration-200 hover:border-primary-300 hover:bg-primary-50"
                >
                  <div className="flex items-start space-x-3">
                    {category.icon ? (
                      <div
                        className="flex h-10 w-10 items-center justify-center rounded-lg text-white"
                        style={{ backgroundColor: category.color || '#3B82F6' }}
                      >
                        <span className="text-lg">{category.icon}</span>
                      </div>
                    ) : (
                      <div className="flex h-10 w-10 items-center justify-center rounded-lg bg-gray-100">
                        <DocumentTextIcon className="h-5 w-5 text-gray-600" />
                      </div>
                    )}
                    <div className="min-w-0 flex-1">
                      <h3 className="truncate font-medium text-gray-900">
                        {category.name}
                      </h3>
                      <p className="mb-2 line-clamp-2 text-sm text-gray-600">
                        {category.description}
                      </p>
                      <div className="flex items-center space-x-4 text-xs text-gray-500">
                        <span>{formatNumber(category.topicCount)} chủ đề</span>
                        <span>{formatNumber(category.postCount)} bài viết</span>
                      </div>
                      {category.lastTopicTitle && (
                        <div className="mt-2 text-xs text-gray-500">
                          Mới nhất:{' '}
                          <span className="text-gray-700">
                            {category.lastTopicTitle}
                          </span>
                          {category.lastAuthorName && (
                            <span> bởi {category.lastAuthorName}</span>
                          )}
                        </div>
                      )}
                    </div>
                  </div>
                </Link>
              ))}
            </div>
          </div>

          {/* Pinned Topics */}
          {homeData.pinnedTopics.length > 0 && (
            <div className="rounded-xl border border-gray-200 bg-white p-6 shadow-sm">
              <div className="mb-6 flex items-center space-x-2">
                <StarIcon className="h-5 w-5 text-yellow-500" />
                <h2 className="text-xl font-semibold text-gray-900">
                  Chủ đề Sức khỏe Nổi bật
                </h2>
              </div>
              <div className="space-y-4">
                {homeData.pinnedTopics.map(topic => (
                  <TopicCard key={topic.id} topic={topic} />
                ))}
              </div>
            </div>
          )}

          {/* Latest Topics */}
          <div className="rounded-xl border border-gray-200 bg-white p-6 shadow-sm">
            <div className="mb-6 flex items-center justify-between">
              <div className="flex items-center space-x-2">
                <ClockIcon className="h-5 w-5 text-gray-500" />
                <h2 className="text-xl font-semibold text-gray-900">
                  Thảo luận Sức khỏe Gần đây
                </h2>
              </div>
              <Link
                to="/topics"
                className="text-sm font-medium text-primary-600 hover:text-primary-700"
              >
                Xem tất cả
              </Link>
            </div>
            <div className="space-y-4">
              {homeData.latestTopics.slice(0, 10).map(topic => (
                <TopicCard key={topic.id} topic={topic} />
              ))}
            </div>
          </div>
        </div>

        {/* Sidebar */}
        <div className="space-y-6">
          {/* Popular Tags */}
          <div className="rounded-xl border border-gray-200 bg-white p-6 shadow-sm">
            <div className="mb-4 flex items-center space-x-2">
              <ArrowTrendingUpIcon className="h-5 w-5 text-green-500" />
              <h3 className="font-semibold text-gray-900">
                Chủ đề Sức khỏe Phổ biến
              </h3>
            </div>
            <div className="flex flex-wrap gap-2">
              {homeData.popularTags.slice(0, 15).map(tag => (
                <Link
                  key={tag.id}
                  to={`/search?tags=${tag.slug}`}
                  className="inline-flex items-center rounded-full bg-gray-100 px-3 py-1 text-sm font-medium text-gray-800 transition-colors duration-200 hover:bg-gray-200"
                  style={{
                    backgroundColor: tag.color ? `${tag.color}20` : undefined,
                    color: tag.color || undefined,
                  }}
                >
                  {tag.name}
                  <span className="ml-1 text-xs opacity-75">
                    {formatNumber(tag.topicCount)}
                  </span>
                </Link>
              ))}
            </div>
          </div>

          {/* Quick Actions */}
          <div className="rounded-xl border border-gray-200 bg-white p-6 shadow-sm">
            <h3 className="mb-4 font-semibold text-gray-900">
              Tài nguyên Sức khỏe
            </h3>
            <div className="space-y-3">
              <Link
                to="/topics/create"
                className="block w-full rounded-lg bg-primary-600 px-4 py-2 text-center font-medium text-white transition-colors duration-200 hover:bg-primary-700"
              >
                <ChatBubbleLeftRightIcon className="h-4 w-4 inline mr-1" />
                Đặt câu hỏi Sức khỏe
              </Link>
              <Link
                to="/search"
                className="block w-full rounded-lg bg-secondary-100 px-4 py-2 text-center font-medium text-secondary-900 transition-colors duration-200 hover:bg-secondary-200"
              >
                <MagnifyingGlassIcon className="h-4 w-4 inline mr-1" />
                Tìm kiếm Chủ đề Sức khỏe
              </Link>
              <Link
                to="/categories"
                className="block w-full rounded-lg bg-gray-100 px-4 py-2 text-center font-medium text-gray-900 transition-colors duration-200 hover:bg-gray-200"
              >
                <BookOpenIcon className="h-4 w-4 inline mr-1" />
                Duyệt Danh mục Sức khỏe
              </Link>
              <Link
                to="/emergency"
                className="block w-full rounded-lg bg-red-100 px-4 py-2 text-center font-medium text-red-900 transition-colors duration-200 hover:bg-red-200"
              >
                <ExclamationTriangleIcon className="h-4 w-4 inline mr-1" />
                Tài nguyên Cấp cứu
              </Link>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

// Topic Card Component
const TopicCard: React.FC<{ topic: TopicSummary }> = ({ topic }) => {
  return (
    <div className="flex items-start space-x-3 rounded-lg p-4 transition-colors duration-200 hover:bg-gray-50">
      <div className="min-w-0 flex-1">
        <div className="flex items-start justify-between">
          <div className="flex-1">
            <Link
              to={`/topics/${topic.slug}`}
              className="text-lg font-medium text-gray-900 transition-colors duration-200 hover:text-primary-600"
            >
              {topic.title}
              {topic.isPinned && (
                <StarIcon className="ml-1 inline-block h-4 w-4 text-yellow-500" />
              )}
            </Link>
            <div className="mt-1 flex items-center space-x-2">
              <Link
                to={`/categories/${topic.categorySlug}`}
                className="text-sm text-primary-600 hover:text-primary-700"
              >
                {topic.categoryName}
              </Link>
              <span className="text-gray-300">•</span>
              <span className="text-sm text-gray-600">
                by{' '}
                <Link
                  to={`/users/${topic.authorName}`}
                  className="font-medium hover:text-primary-600"
                >
                  {topic.authorName}
                </Link>
              </span>
              <span
                className={cn(
                  'inline-flex items-center rounded px-2 py-0.5 text-xs font-medium',
                  getRoleBadgeColor(topic.authorRole)
                )}
              >
                {topic.authorRole}
              </span>
            </div>
          </div>
          <div className="text-right text-sm text-gray-500">
            {formatRelativeTime(topic.createdAt)}
          </div>
        </div>

        {/* Tags */}
        {topic.tags.length > 0 && (
          <div className="mt-2 flex flex-wrap gap-1">
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
                +{topic.tags.length - 3} more
              </span>
            )}
          </div>
        )}

        {/* Stats */}
        <div className="mt-3 flex items-center space-x-4 text-sm text-gray-500">
          <div className="flex items-center space-x-1">
            <ChatBubbleLeftRightIcon className="h-4 w-4" />
            <span>{formatNumber(topic.postCount)} phản hồi</span>
          </div>
          <div className="flex items-center space-x-1">
            <svg
              className="h-4 w-4"
              fill="none"
              stroke="currentColor"
              viewBox="0 0 24 24"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth="2"
                d="M4.318 6.318a4.5 4.5 0 000 6.364L12 20.364l7.682-7.682a4.5 4.5 0 00-6.364-6.364L12 7.636l-1.318-1.318a4.5 4.5 0 00-6.364 0z"
              />
            </svg>
            <span>{formatNumber(topic.likeCount)} lượt thích</span>
          </div>
          <div className="flex items-center space-x-1">
            <svg
              className="h-4 w-4"
              fill="none"
              stroke="currentColor"
              viewBox="0 0 24 24"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth="2"
                d="M15 12a3 3 0 11-6 0 3 3 0 016 0z"
              />
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth="2"
                d="M2.458 12C3.732 7.943 7.523 5 12 5c4.478 0 8.268 2.943 9.542 7-1.274 4.057-5.064 7-9.542 7-4.477 0-8.268-2.943-9.542-7z"
              />
            </svg>
            <span>{formatNumber(topic.viewCount)} lượt xem</span>
          </div>
        </div>
      </div>
    </div>
  );
};

export default HomePage;
