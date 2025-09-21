import React, { useState, useEffect } from 'react';
import { useParams, Link } from 'react-router-dom';
import {
  UserIcon,
  ChatBubbleLeftRightIcon,
  EyeIcon,
  HeartIcon,
  EnvelopeIcon,
  CalendarIcon,
} from '@heroicons/react/24/outline';
import { profileApi } from '../services/api';
import type { User, TopicSummary } from '../types';
import {
  formatRelativeTime,
  formatNumber,
  getRoleBadgeColor,
  cn,
} from '../utils';

const UserProfilePage: React.FC = () => {
  const { username } = useParams<{ username: string }>();
  
  const [profile, setProfile] = useState<User | null>(null);
  const [userTopics, setUserTopics] = useState<TopicSummary[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    if (username) {
      fetchPublicProfile();
      fetchUserTopics();
    }
  }, [username]);

  const fetchPublicProfile = async () => {
    try {
      setIsLoading(true);
      const response = await profileApi.getPublicProfile(username!);
      if (response.success) {
        setProfile(response.data);
      } else {
        setError(response.message);
      }
    } catch (err: any) {
      setError(err.message || 'Không thể tải hồ sơ người dùng');
    } finally {
      setIsLoading(false);
    }
  };

  const fetchUserTopics = async () => {
    try {
      if (!username) return;
      
      const response = await profileApi.getPublicUserTopics(username, 1, 10);
      if (response.success && response.data && response.data.items) {
        setUserTopics(response.data.items);
      } else {
        setUserTopics([]); // Set empty array if no data
      }
    } catch (err) {
      console.error('Error loading user topics:', err);
      setUserTopics([]); // Set empty array on error
    }
  };

  if (isLoading) {
    return (
      <div className="flex min-h-screen items-center justify-center">
        <div className="h-12 w-12 animate-spin rounded-full border-b-2 border-primary-600"></div>
      </div>
    );
  }

  if (error || !profile) {
    return (
      <div className="flex min-h-screen items-center justify-center">
        <div className="text-center">
          <h2 className="mb-2 text-xl font-semibold text-gray-900">Không tìm thấy người dùng</h2>
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
      <nav className="text-sm text-gray-500 mb-8">
        <Link to="/" className="hover:text-primary-600">Trang chủ</Link>
        <span className="mx-2">›</span>
        <span className="text-gray-900">@{profile.username}</span>
      </nav>

      <div className="grid grid-cols-1 gap-8 lg:grid-cols-3">
        <div className="lg:col-span-1">
          <div className="rounded-xl border border-gray-200 bg-white p-6 shadow-sm">
            <div className="text-center">
              {profile.avatar ? (
                <img
                  src={profile.avatar}
                  alt={profile.fullName}
                  className="mx-auto h-24 w-24 rounded-full object-cover"
                />
              ) : (
                <div className="mx-auto flex h-24 w-24 items-center justify-center rounded-full bg-gray-100">
                  <UserIcon className="h-12 w-12 text-gray-400" />
                </div>
              )}
              <h1 className="mt-4 text-2xl font-bold text-gray-900">{profile.fullName}</h1>
              <p className="text-gray-500">@{profile.username}</p>
              <span
                className={cn(
                  'mt-2 inline-flex items-center rounded-full px-3 py-1 text-sm font-medium',
                  getRoleBadgeColor(profile.role)
                )}
              >
                {profile.role}
              </span>
            </div>

            {profile.showBio && profile.bio && (
              <div className="mt-6">
                <h3 className="text-sm font-medium text-gray-700 mb-2">Giới thiệu</h3>
                <p className="text-sm text-gray-900">{profile.bio}</p>
              </div>
            )}

            <div className="mt-6 space-y-3">
              {profile.showEmail && profile.email && (
                <div className="flex items-center space-x-2 text-sm text-gray-600">
                  <EnvelopeIcon className="h-4 w-4" />
                  <span>{profile.email}</span>
                </div>
              )}
              
              <div className="flex items-center space-x-2 text-sm text-gray-600">
                <CalendarIcon className="h-4 w-4" />
                <span>Tham gia {formatRelativeTime(profile.createdAt)}</span>
              </div>
            </div>

            <div className="mt-6 grid grid-cols-2 gap-4 text-center">
              <div className="rounded-lg bg-gray-50 p-3">
                <div className="text-lg font-semibold text-gray-900">
                  {formatNumber(profile.topicCount)}
                </div>
                <div className="text-sm text-gray-600">Chủ đề</div>
              </div>
              <div className="rounded-lg bg-gray-50 p-3">
                <div className="text-lg font-semibold text-gray-900">
                  {formatNumber(profile.postCount)}
                </div>
                <div className="text-sm text-gray-600">Bài viết</div>
              </div>
            </div>
          </div>
        </div>

        <div className="lg:col-span-2">
          <div className="rounded-xl border border-gray-200 bg-white p-6 shadow-sm">
            <h2 className="mb-6 text-lg font-semibold text-gray-900">
              Chủ đề gần đây của {profile.fullName}
            </h2>

            <div className="space-y-4">
              {userTopics && userTopics.length === 0 ? (
                <div className="rounded-lg bg-gray-50 p-8 text-center">
                  <ChatBubbleLeftRightIcon className="mx-auto h-12 w-12 text-gray-400" />
                  <h3 className="mt-4 text-lg font-medium text-gray-900">
                    Chưa có chủ đề công khai
                  </h3>
                  <p className="mt-2 text-gray-600">
                    Người dùng này chưa tạo chủ đề sức khỏe công khai nào.
                  </p>
                </div>
              ) : (
                userTopics?.map(topic => (
                  <div
                    key={topic.id}
                    className="rounded-lg border border-gray-200 p-6 hover:border-gray-300 transition-all duration-200"
                  >
                    <Link
                      to={`/topics/${topic.slug}`}
                      className="text-lg font-semibold text-gray-900 hover:text-primary-600"
                    >
                      {topic.title}
                    </Link>
                    
                    <div className="mt-2 flex items-center space-x-2 text-sm text-gray-600">
                      <Link
                        to={`/categories/${topic.categorySlug}`}
                        className="text-primary-600 hover:text-primary-700"
                      >
                        {topic.categoryName}
                      </Link>
                      <span>•</span>
                      <span>{formatRelativeTime(topic.createdAt)}</span>
                    </div>

                    {topic.tags.length > 0 && (
                      <div className="mt-3 flex flex-wrap gap-1">
                        {topic.tags.slice(0, 4).map((tag, index) => (
                          <span
                            key={index}
                            className="inline-flex items-center rounded bg-gray-100 px-2 py-0.5 text-xs font-medium text-gray-700"
                          >
                            {tag}
                          </span>
                        ))}
                        {topic.tags.length > 4 && (
                          <span className="text-xs text-gray-500">
                            +{topic.tags.length - 4} thêm
                          </span>
                        )}
                      </div>
                    )}

                    <div className="mt-4 flex items-center space-x-6 text-sm text-gray-500">
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
                ))
              )}
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default UserProfilePage;
