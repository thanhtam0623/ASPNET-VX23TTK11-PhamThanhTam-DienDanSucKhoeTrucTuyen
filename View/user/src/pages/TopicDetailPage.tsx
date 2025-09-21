import React, { useState, useEffect } from 'react';
import { useParams, useNavigate, Link } from 'react-router-dom';
import {
  HeartIcon,
  ChatBubbleLeftRightIcon,
  EyeIcon,
  CheckCircleIcon,
  UserIcon,
} from '@heroicons/react/24/outline';
import { HeartIcon as HeartSolidIcon } from '@heroicons/react/24/solid';
import { topicApi } from '../services/api';
import { useAuth } from '../context/AuthContext';
import type { Topic, Post, CreatePostRequest } from '../types';
import {
  formatRelativeTime,
  formatNumber,
  getRoleBadgeColor,
  cn,
} from '../utils';
import { usePageTitle } from '../utils';

const TopicDetailPage: React.FC = () => {
  usePageTitle('Chi tiết chủ đề');
  const { slug } = useParams<{ slug: string }>();
  const navigate = useNavigate();
  const { isAuthenticated } = useAuth();
  
  const [topic, setTopic] = useState<Topic | null>(null);
  const [posts, setPosts] = useState<Post[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [postsLoading, setPostsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [newPostContent, setNewPostContent] = useState('');
  const [isSubmittingPost, setIsSubmittingPost] = useState(false);

  useEffect(() => {
    if (slug) {
      fetchTopicDetail();
    }
  }, [slug]);

  useEffect(() => {
    if (topic?.id) {
      fetchTopicPosts();
      recordTopicView();
    }
  }, [topic?.id]);

  const fetchTopicDetail = async () => {
    try {
      setIsLoading(true);
      const response = await topicApi.getTopicBySlug(slug!);
      if (response.success) {
        setTopic(response.data);
      } else {
        setError(response.message);
      }
    } catch (err: any) {
      setError(err.message || 'Không thể tải chủ đề');
    } finally {
      setIsLoading(false);
    }
  };

  const fetchTopicPosts = async () => {
    try {
      setPostsLoading(true);
      let allPosts: Post[] = [];
      let page = 1;
      let hasMore = true;
      
      // Load tất cả posts bằng cách loop qua các trang với pageSize = 100
      while (hasMore) {
        console.log(`Fetching posts page ${page} for topic ${topic!.id}`);
        const response = await topicApi.getTopicPosts(topic!.id, page, 100);
        
        if (response.success && response.data && response.data.items) {
          allPosts = [...allPosts, ...response.data.items];
          hasMore = response.data.hasNext;
          page++;
          
          console.log(`Page ${page - 1}: loaded ${response.data.items.length} posts, hasNext: ${response.data.hasNext}`);
          
          // Giới hạn tối đa 10 trang để tránh vòng lặp vô hạn
          if (page > 10) break;
        } else {
          console.error('API response error:', response);
          hasMore = false;
        }
      }
      
      setPosts(allPosts);
      console.log(`Total loaded posts: ${allPosts.length} for topic ${topic!.id}`);
    } catch (err) {
      console.error('Error loading posts:', err);
      setPosts([]);
    } finally {
      setPostsLoading(false);
    }
  };

  const recordTopicView = async () => {
    try {
      await fetch(`/api/user/topics/${topic!.id}/view`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          ...(localStorage.getItem('token') && {
            Authorization: `Bearer ${localStorage.getItem('token')}`,
          }),
        },
      });
    } catch (err) {
      // Silent fail for view tracking
    }
  };

  const handleLikeTopic = async () => {
    if (!isAuthenticated || !topic) {
      navigate('/login');
      return;
    }

    try {
      if (topic.isLikedByCurrentUser) {
        await topicApi.unlikeTopic(topic.id);
        setTopic({
          ...topic,
          isLikedByCurrentUser: false,
          likeCount: topic.likeCount - 1,
        });
      } else {
        await topicApi.likeTopic(topic.id);
        setTopic({
          ...topic,
          isLikedByCurrentUser: true,
          likeCount: topic.likeCount + 1,
        });
      }
    } catch (err) {
    }
  };

  const handleCreatePost = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!isAuthenticated || !topic || !newPostContent.trim()) {
      return;
    }

    try {
      setIsSubmittingPost(true);
      const request: CreatePostRequest = {
        content: newPostContent.trim(),
      };
      
      const response = await topicApi.createPost(topic.id, request);
      if (response.success) {
        // Thêm post mới vào đầu danh sách
        setPosts([...posts, response.data]);
        setNewPostContent('');
        
        // Cập nhật topic count
        setTopic({
          ...topic,
          postCount: topic.postCount + 1,
        });
        
        // Scroll xuống post mới
        setTimeout(() => {
          window.scrollTo({
            top: document.body.scrollHeight,
            behavior: 'smooth'
          });
        }, 100);
      }
    } catch (err) {
      console.error('Error creating post:', err);
    } finally {
      setIsSubmittingPost(false);
    }
  };

  if (isLoading) {
    return (
      <div className="flex min-h-screen items-center justify-center">
        <div className="h-12 w-12 animate-spin rounded-full border-b-2 border-primary-600"></div>
      </div>
    );
  }

  if (error || !topic) {
    return (
      <div className="flex min-h-screen items-center justify-center">
        <div className="text-center">
          <h2 className="mb-2 text-xl font-semibold text-gray-900">
            Không tìm thấy chủ đề
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
    <div className="mx-auto max-w-4xl px-4 py-8 sm:px-6 lg:px-8">
      {/* Breadcrumb */}
      <nav className="mb-6 text-sm text-gray-500">
        <Link to="/" className="hover:text-primary-600">
          Trang chủ
        </Link>
        <span className="mx-2">›</span>
        <Link
          to={`/categories/${topic.category.slug}`}
          className="hover:text-primary-600"
        >
          {topic.category.name}
        </Link>
        <span className="mx-2">›</span>
        <span className="text-gray-900">{topic.title}</span>
      </nav>

      {/* Topic Header */}
      <div className="mb-8 rounded-xl border border-gray-200 bg-white p-6 shadow-sm">
        <div className="mb-4 flex items-start justify-between">
          <div className="flex-1">
            <h1 className="mb-3 text-2xl font-bold text-gray-900 md:text-3xl">
              {topic.title}
            </h1>
            <div className="flex flex-wrap items-center gap-4 text-sm text-gray-600">
              <div className="flex items-center space-x-2">
                {topic.author.avatar ? (
                  <img
                    src={topic.author.avatar}
                    alt={topic.author.fullName}
                    className="h-8 w-8 rounded-full object-cover"
                  />
                ) : (
                  <div className="flex h-8 w-8 items-center justify-center rounded-full bg-gray-100">
                    <UserIcon className="h-5 w-5 text-gray-600" />
                  </div>
                )}
                <Link
                  to={`/users/${topic.author.username}`}
                  className="font-medium hover:text-primary-600"
                >
                  {topic.author.fullName}
                </Link>
                <span
                  className={cn(
                    'inline-flex items-center rounded px-2 py-0.5 text-xs font-medium',
                    getRoleBadgeColor(topic.author.role)
                  )}
                >
                  {topic.author.role}
                </span>
              </div>
              <span>•</span>
              <span>{formatRelativeTime(topic.createdAt)}</span>
              <span>•</span>
              <Link
                to={`/categories/${topic.category.slug}`}
                className="text-primary-600 hover:text-primary-700"
              >
                {topic.category.name}
              </Link>
            </div>
          </div>

          {/* Topic Actions */}
          <div className="flex items-center space-x-2">
            <button
              onClick={handleLikeTopic}
              className={cn(
                'flex items-center space-x-1 rounded-lg px-3 py-2 text-sm font-medium transition-colors',
                topic.isLikedByCurrentUser
                  ? 'bg-red-50 text-red-600 hover:bg-red-100'
                  : 'bg-gray-100 text-gray-600 hover:bg-gray-200'
              )}
            >
              {topic.isLikedByCurrentUser ? (
                <HeartSolidIcon className="h-4 w-4" />
              ) : (
                <HeartIcon className="h-4 w-4" />
              )}
              <span>{formatNumber(topic.likeCount)}</span>
            </button>
          </div>
        </div>

        {/* Tags */}
        {topic.tags.length > 0 && (
          <div className="mb-4 flex flex-wrap gap-2">
            {topic.tags.map(tag => (
              <Link
                key={tag.id}
                to={`/search?tags=${tag.slug}`}
                className="inline-flex items-center rounded-full bg-primary-100 px-3 py-1 text-sm font-medium text-primary-800 hover:bg-primary-200"
              >
                {tag.name}
              </Link>
            ))}
          </div>
        )}

        {/* Topic Content */}
        <div
          className="prose max-w-none text-gray-900"
          dangerouslySetInnerHTML={{ __html: topic.content }}
        />

        {/* Topic Stats */}
        <div className="mt-6 flex items-center space-x-6 border-t border-gray-200 pt-4 text-sm text-gray-500">
          <div className="flex items-center space-x-1">
            <EyeIcon className="h-4 w-4" />
            <span>{formatNumber(topic.viewCount)} views</span>
          </div>
          <div className="flex items-center space-x-1">
            <ChatBubbleLeftRightIcon className="h-4 w-4" />
            <span>{formatNumber(topic.postCount)} replies</span>
          </div>
          {topic.hasAnswer && (
            <div className="flex items-center space-x-1 text-green-600">
              <CheckCircleIcon className="h-4 w-4" />
              <span>Đã trả lời</span>
            </div>
          )}
        </div>
      </div>

      {/* Posts Section */}
      <div className="space-y-6">
        <h2 className="text-xl font-semibold text-gray-900">
          Phản hồi ({posts.length})
        </h2>

        {postsLoading ? (
          <div className="flex justify-center py-8">
            <div className="h-8 w-8 animate-spin rounded-full border-b-2 border-primary-600"></div>
          </div>
        ) : posts.length === 0 ? (
          <div className="rounded-xl border border-gray-200 bg-gray-50 p-6 text-center">
            <ChatBubbleLeftRightIcon className="mx-auto h-12 w-12 text-gray-400" />
            <p className="mt-2 text-gray-600">
              Chưa có phản hồi nào. Hãy là người đầu tiên chia sẻ suy nghĩ của bạn!
            </p>
          </div>
        ) : (
          posts.map(post => (
            <div
              key={post.id}
              className={cn(
                'rounded-xl border bg-white p-6 shadow-sm',
                post.isAnswer
                  ? 'border-green-200 bg-green-50'
                  : 'border-gray-200'
              )}
            >
              {post.isAnswer && (
                <div className="mb-4 flex items-center space-x-2 text-green-600">
                  <CheckCircleIcon className="h-5 w-5" />
                  <span className="text-sm font-medium">Câu trả lời được chấp nhận</span>
                </div>
              )}

              <div className="mb-4 flex items-start justify-between">
                <div className="flex items-center space-x-3">
                  {post.author.avatar ? (
                    <img
                      src={post.author.avatar}
                      alt={post.author.fullName}
                      className="h-10 w-10 rounded-full object-cover"
                    />
                  ) : (
                    <div className="flex h-10 w-10 items-center justify-center rounded-full bg-gray-100">
                      <UserIcon className="h-6 w-6 text-gray-600" />
                    </div>
                  )}
                  <div>
                    <Link
                      to={`/users/${post.author.username}`}
                      className="font-medium text-gray-900 hover:text-primary-600"
                    >
                      {post.author.fullName}
                    </Link>
                    <div className="flex items-center space-x-2 text-sm text-gray-500">
                      <span
                        className={cn(
                          'inline-flex items-center rounded px-2 py-0.5 text-xs font-medium',
                          getRoleBadgeColor(post.author.role)
                        )}
                      >
                        {post.author.role}
                      </span>
                      <span>•</span>
                      <span>{formatRelativeTime(post.createdAt)}</span>
                    </div>
                  </div>
                </div>

                <div className="flex items-center space-x-2">
                  <button
                    className={cn(
                      'flex items-center space-x-1 rounded-lg px-3 py-1 text-sm font-medium transition-colors',
                      post.isLikedByCurrentUser
                        ? 'bg-red-50 text-red-600 hover:bg-red-100'
                        : 'bg-gray-100 text-gray-600 hover:bg-gray-200'
                    )}
                  >
                    {post.isLikedByCurrentUser ? (
                      <HeartSolidIcon className="h-4 w-4" />
                    ) : (
                      <HeartIcon className="h-4 w-4" />
                    )}
                    <span>{formatNumber(post.likeCount)}</span>
                  </button>
                </div>
              </div>

              <div
                className="prose max-w-none text-gray-900"
                dangerouslySetInnerHTML={{ __html: post.content }}
              />
            </div>
          ))
        )}

        {/* Create New Post */}
        {isAuthenticated && !topic.isLocked && (
          <div className="rounded-xl border border-gray-200 bg-white p-6 shadow-sm">
            <h3 className="mb-4 text-lg font-semibold text-gray-900">
              <ChatBubbleLeftRightIcon className="h-5 w-5 inline mr-2" />
              Tham gia thảo luận
            </h3>
            <form onSubmit={handleCreatePost}>
              <textarea
                value={newPostContent}
                onChange={e => setNewPostContent(e.target.value)}
                placeholder="Chia sẻ suy nghĩ, kinh nghiệm của bạn hoặc đặt câu hỏi tiếp theo..."
                className="w-full rounded-lg border border-gray-300 p-3 focus:border-primary-500 focus:outline-none focus:ring-1 focus:ring-primary-500"
                rows={4}
                required
              />
              <div className="mt-4 flex justify-between">
                <p className="text-sm text-gray-500">
                  <CheckCircleIcon className="h-4 w-4 inline mr-1" />
                  Vui lòng tôn trọng và tuân thủ các nguyên tắc cộng đồng của chúng tôi.
                </p>
                <button
                  type="submit"
                  disabled={isSubmittingPost || !newPostContent.trim()}
                  className="rounded-lg bg-primary-600 px-4 py-2 text-white hover:bg-primary-700 disabled:opacity-50"
                >
                  {isSubmittingPost ? 'Đang đăng...' : 'Đăng phản hồi'}
                </button>
              </div>
            </form>
          </div>
        )}

        {!isAuthenticated && (
          <div className="rounded-xl border border-gray-200 bg-gray-50 p-6 text-center">
            <p className="mb-4 text-gray-600">
              Vui lòng đăng nhập để tham gia thảo luận và giúp đỡ những người khác trong cộng đồng chăm sóc sức khỏe của chúng tôi.
            </p>
            <Link
              to="/login"
              className="inline-block rounded-lg bg-primary-600 px-4 py-2 text-white hover:bg-primary-700"
            >
              Đăng nhập để phản hồi
            </Link>
          </div>
        )}
      </div>
    </div>
  );
};

export default TopicDetailPage;
