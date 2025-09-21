import React, { useState, useEffect } from 'react';
import { Link, useNavigate, useSearchParams } from 'react-router-dom';
import { UserIcon, PencilIcon, ChatBubbleLeftRightIcon, EyeIcon, HeartIcon } from '@heroicons/react/24/outline';
import { profileApi } from '../services/api';
import { useAuth } from '../context/AuthContext';
import { usePageTitle } from '../utils';
import type { User, TopicSummary, UpdateProfileRequest } from '../types';
import { formatRelativeTime, formatNumber, getRoleBadgeColor, cn } from '../utils';

const ProfilePage: React.FC = () => {
  usePageTitle('Hồ sơ cá nhân'); // Add usePageTitle hook
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();
  const {isAuthenticated } = useAuth();
  const [profile, setProfile] = useState<User | null>(null);
  const [userTopics, setUserTopics] = useState<TopicSummary[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [activeTab, setActiveTab] = useState<'profile' | 'topics'>('profile');
  const [isEditing, setIsEditing] = useState(false);
  const [editForm, setEditForm] = useState<UpdateProfileRequest>({
    fullName: '',
    bio: '',
    showEmail: false,
    showBio: false,
  });

  useEffect(() => {
    if (!isAuthenticated) {
      navigate('/login');
      return;
    }
    
    const tab = searchParams.get('tab');
    if (tab === 'topics') {
      setActiveTab('topics');
    } else {
      setActiveTab('profile');
    }
    
    fetchProfile();
    fetchUserTopics();
  }, [isAuthenticated, navigate, searchParams]);

  const fetchProfile = async () => {
    try {
      setIsLoading(true);
      const response = await profileApi.getProfile();
      if (response.success) {
        setProfile(response.data);
        setEditForm({
          fullName: response.data.fullName,
          bio: response.data.bio || '',
          showEmail: response.data.showEmail,
          showBio: response.data.showBio,
        });
      }
    } catch (err) {
      // Handle profile loading error
    } finally {
      setIsLoading(false);
    }
  };

  const fetchUserTopics = async () => {
    try {
      console.log('Fetching user topics...');
      const response = await profileApi.getUserTopics(1, 10);
      console.log('User topics API response:', response);
      
      if (response.success && response.data && response.data.items) {
        console.log('Setting user topics:', response.data.items);
        setUserTopics(response.data.items);
      } else {
        console.log('No topics data or API failed, setting empty array');
        setUserTopics([]);
      }
    } catch (err) {
      console.error('Error fetching user topics:', err);
      setUserTopics([]);
    }
  };

  const handleSaveProfile = async () => {
    try {
      const response = await profileApi.updateProfile(editForm);
      if (response.success) {
        setProfile(response.data);
        setIsEditing(false);
      }
    } catch (err) {
      // Handle profile update error
    }
  };

  if (isLoading || !profile) {
    return (
      <div className="flex min-h-screen items-center justify-center">
        <div className="h-12 w-12 animate-spin rounded-full border-b-2 border-primary-600"></div>
      </div>
    );
  }

  return (
    <div className="mx-auto max-w-6xl px-4 py-8 sm:px-6 lg:px-8">
      <div className="mb-8">
        <h1 className="text-3xl font-bold text-gray-900 flex items-center gap-3">
          <UserIcon className="h-8 w-8 text-primary-600" />
          Hồ sơ của tôi
        </h1>
        <p className="mt-2 text-gray-600">Quản lý hồ sơ cộng đồng y tế của bạn.</p>
      </div>

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
              <h2 className="mt-4 text-xl font-semibold text-gray-900">{profile.fullName}</h2>
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

            <div className="mt-6 grid grid-cols-2 gap-4 text-center">
              <div className="rounded-lg bg-gray-50 p-3">
                <div className="text-lg font-semibold text-gray-900">{formatNumber(profile.topicCount)}</div>
                <div className="text-sm text-gray-600">Chủ đề</div>
              </div>
              <div className="rounded-lg bg-gray-50 p-3">
                <div className="text-lg font-semibold text-gray-900">{formatNumber(profile.postCount)}</div>
                <div className="text-sm text-gray-600">Bài viết</div>
              </div>
            </div>

            <nav className="mt-6 space-y-1">
              <button
                onClick={() => setActiveTab('profile')}
                className={cn(
                  'flex w-full items-center rounded-lg px-3 py-2 text-sm font-medium',
                  activeTab === 'profile'
                    ? 'bg-primary-100 text-primary-700'
                    : 'text-gray-700 hover:bg-gray-100'
                )}
              >
                <UserIcon className="mr-3 h-4 w-4" />
                Thông tin hồ sơ
              </button>
              <button
                onClick={() => setActiveTab('topics')}
                className={cn(
                  'flex w-full items-center rounded-lg px-3 py-2 text-sm font-medium',
                  activeTab === 'topics'
                    ? 'bg-primary-100 text-primary-700'
                    : 'text-gray-700 hover:bg-gray-100'
                )}
              >
                <ChatBubbleLeftRightIcon className="mr-3 h-4 w-4" />
                Chủ đề của tôi
              </button>
            </nav>
          </div>
        </div>

        <div className="lg:col-span-2">
          {activeTab === 'profile' && (
            <div className="rounded-xl border border-gray-200 bg-white p-6 shadow-sm">
              <div className="mb-6 flex items-center justify-between">
                <h3 className="text-lg font-semibold text-gray-900">Thông tin hồ sơ</h3>
                <button
                  onClick={() => setIsEditing(!isEditing)}
                  className="flex items-center rounded-lg border border-gray-300 bg-white px-3 py-2 text-sm font-medium text-gray-700 hover:bg-gray-50"
                >
                  <PencilIcon className="mr-2 h-4 w-4" />
                  {isEditing ? 'Hủy' : 'Chỉnh sửa'}
                </button>
              </div>

              {isEditing ? (
                <div className="space-y-6">
                  <div>
                    <label className="block text-sm font-medium text-gray-700">Họ và tên</label>
                    <input
                      type="text"
                      value={editForm.fullName}
                      onChange={e => setEditForm((prev: UpdateProfileRequest) => ({ ...prev, fullName: e.target.value }))}
                      className="mt-1 block w-full rounded-lg border border-gray-300 px-3 py-2 focus:border-primary-500 focus:outline-none focus:ring-1 focus:ring-primary-500"
                    />
                  </div>

                  <div>
                    <label className="block text-sm font-medium text-gray-700">Bio</label>
                    <textarea
                      value={editForm.bio}
                      onChange={e => setEditForm((prev: UpdateProfileRequest) => ({ ...prev, bio: e.target.value }))}
                      rows={4}
                      placeholder="Hãy giới thiệu bản thân với cộng đồng..."
                      className="mt-1 block w-full rounded-lg border border-gray-300 px-3 py-2 focus:border-primary-500 focus:outline-none focus:ring-1 focus:ring-primary-500"
                    />
                  </div>

                  <div className="space-y-3">
                    <div className="flex items-center">
                      <input
                        type="checkbox"
                        id="showEmail"
                        checked={editForm.showEmail}
                        onChange={e => setEditForm((prev: UpdateProfileRequest) => ({ ...prev, showEmail: e.target.checked }))}
                        className="h-4 w-4 rounded border-gray-300 text-primary-600 focus:ring-primary-500"
                      />
                      <label htmlFor="showEmail" className="ml-2 text-sm text-gray-900">
                        Hiển thị địa chỉ email công khai
                      </label>
                    </div>

                    <div className="flex items-center">
                      <input
                        type="checkbox"
                        id="showBio"
                        checked={editForm.showBio}
                        onChange={e => setEditForm((prev: UpdateProfileRequest) => ({ ...prev, showBio: e.target.checked }))}
                        className="h-4 w-4 rounded border-gray-300 text-primary-600 focus:ring-primary-500"
                      />
                      <label htmlFor="showBio" className="ml-2 text-sm text-gray-900">
                        Hiển thị giới thiệu công khai
                      </label>
                    </div>
                  </div>

                  <div className="flex justify-end space-x-3">
                    <button
                      onClick={() => setIsEditing(false)}
                      className="rounded-lg border border-gray-300 px-4 py-2 text-sm font-medium text-gray-700 hover:bg-gray-50"
                    >
                      Hủy
                    </button>
                    <button
                      onClick={handleSaveProfile}
                      className="rounded-lg bg-primary-600 px-4 py-2 text-sm font-medium text-white hover:bg-primary-700"
                    >
                      Lưu thay đổi
                    </button>
                  </div>
                </div>
              ) : (
                <div className="space-y-6">
                  <div>
                    <label className="block text-sm font-medium text-gray-700">Họ và tên</label>
                    <p className="mt-1 text-sm text-gray-900">{profile.fullName}</p>
                  </div>

                  <div>
                    <label className="block text-sm font-medium text-gray-700">Tên đăng nhập</label>
                    <p className="mt-1 text-sm text-gray-900">@{profile.username}</p>
                  </div>

                  <div>
                    <label className="block text-sm font-medium text-gray-700">Email</label>
                    <p className="mt-1 text-sm text-gray-900">{profile.email}</p>
                    <p className="mt-1 text-xs text-gray-500">
                      {profile.showEmail ? 'Hiển thị với người dùng khác' : 'Riêng tư'}
                    </p>
                  </div>

                  <div>
                    <label className="block text-sm font-medium text-gray-700">Giới thiệu</label>
                    <p className="mt-1 text-sm text-gray-900">
                      {profile.bio || 'Chưa có thông tin giới thiệu.'}
                    </p>
                    <p className="mt-1 text-xs text-gray-500">
                      {profile.showBio ? 'Hiển thị với người dùng khác' : 'Riêng tư'}
                    </p>
                  </div>

                  <div>
                    <label className="block text-sm font-medium text-gray-700">Tham gia</label>
                    <p className="mt-1 text-sm text-gray-900">
                      {formatRelativeTime(profile.createdAt)}
                    </p>
                  </div>
                </div>
              )}
            </div>
          )}

          {activeTab === 'topics' && (
            <div className="rounded-xl border border-gray-200 bg-white p-6 shadow-sm">
              <div className="mb-6 flex items-center justify-between">
                <h3 className="text-lg font-semibold text-gray-900">
                  Chủ đề của tôi ({formatNumber(profile.topicCount)})
                </h3>
                <Link
                  to="/topics/create"
                  className="flex items-center rounded-lg bg-primary-600 px-3 py-2 text-sm font-medium text-white hover:bg-primary-700"
                >
                  <ChatBubbleLeftRightIcon className="mr-2 h-4 w-4" />
                  Đặt câu hỏi
                </Link>
              </div>

              <div className="space-y-4">
                {userTopics.length === 0 ? (
                  <div className="rounded-lg bg-gray-50 p-8 text-center">
                    <ChatBubbleLeftRightIcon className="mx-auto h-12 w-12 text-gray-400" />
                    <h3 className="mt-4 text-lg font-medium text-gray-900">Chưa có chủ đề nào</h3>
                    <p className="mt-2 text-gray-600">
                      Bắt đầu bằng cách đặt câu hỏi sức khỏe đầu tiên cho cộng đồng.
                    </p>
                    <Link
                      to="/topics/create"
                      className="mt-4 inline-flex items-center rounded-lg bg-primary-600 px-4 py-2 text-white hover:bg-primary-700"
                    >
                      <ChatBubbleLeftRightIcon className="mr-2 h-4 w-4" />
                      Đặt câu hỏi đầu tiên
                    </Link>
                  </div>
                ) : (
                  userTopics.map(topic => (
                    <div
                      key={topic.id}
                      className="rounded-lg border border-gray-200 p-4 hover:border-gray-300"
                    >
                      <Link
                        to={`/topics/${topic.slug}`}
                        className="text-lg font-medium text-gray-900 hover:text-primary-600"
                      >
                        {topic.title}
                      </Link>
                      <div className="mt-2 flex items-center space-x-4 text-sm text-gray-500">
                        <span>{formatRelativeTime(topic.createdAt)}</span>
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
          )}
        </div>
      </div>
    </div>
  );
};

export default ProfilePage;
