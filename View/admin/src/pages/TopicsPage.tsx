import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import {
  FunnelIcon,
  PencilIcon,
  TrashIcon,
  DocumentTextIcon,
  StarIcon,
  EyeIcon,
  HeartIcon,
  ChatBubbleLeftRightIcon,
  CheckIcon,
} from '@heroicons/react/24/outline';
import { topicsApi, categoriesApi } from '../services/api';
import {
  formatRelativeTime,
  formatNumber,
  truncateText,
  usePageTitle,
} from '../utils';
import type { TopicSummary, TopicSearchRequest, Category } from '../types';
import Table from '../components/UI/Table';
import Modal from '../components/UI/Modal';
import Badge from '../components/UI/Badge';

const TopicsPage: React.FC = () => {
  usePageTitle('Quản lý chủ đề');
  
  const [topics, setTopics] = useState<TopicSummary[]>([]);
  const [categories, setCategories] = useState<Category[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [selectedCategory, setSelectedCategory] = useState<string>('');
  const [selectedStatus, setSelectedStatus] = useState<string>('');
  const [currentPage, setCurrentPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const [showFilters, setShowFilters] = useState(false);
  const [selectedTopic, setSelectedTopic] = useState<TopicSummary | null>(null);
  const [showTopicModal, setShowTopicModal] = useState(false);
  const [showEditModal, setShowEditModal] = useState(false);
  const [editingTopic, setEditingTopic] = useState<TopicSummary | null>(null);
  const [actionLoading, setActionLoading] = useState<number | null>(null);

  const pageSize = 20;

  useEffect(() => {
    fetchCategories();
    fetchTopics();
  }, [currentPage, selectedCategory, selectedStatus]);


  const fetchCategories = async () => {
    try {
      const response = await categoriesApi.getCategories();
      if (response.success) {
        setCategories(response.data);
      }
    } catch (err) {
      console.error('Failed to load categories:', err);
    }
  };

  const fetchTopics = async () => {
    try {
      setIsLoading(true);
      const params: TopicSearchRequest = {
        categoryId: selectedCategory ? parseInt(selectedCategory) : undefined,
        isPinned: selectedStatus === 'pinned' ? true : undefined,
        isLocked: selectedStatus === 'locked' ? true : undefined,
        hasAnswer: selectedStatus === 'answered' ? true : undefined,
        page: currentPage,
        pageSize,
        sortBy: 'newest',
      };

      const response = await topicsApi.getTopics(params);
      if (response.success) {
        setTopics(response.data.items);
        setTotalPages(response.data.totalPages);
      } else {
        setError(response.message);
      }
    } catch (err: any) {
      setError(err.message || 'Failed to load topics');
    } finally {
      setIsLoading(false);
    }
  };

  const handleTogglePin = async (topicId: number) => {
    try {
      setActionLoading(topicId);
      await topicsApi.togglePin(topicId);
      setTopics(prev => prev.map(topic => 
        topic.id === topicId 
          ? { ...topic, isPinned: !topic.isPinned }
          : topic
      ));
    } catch (err: any) {
      setError(err.message || 'Failed to toggle pin status');
    } finally {
      setActionLoading(null);
    }
  };


  const handleDelete = async (topicId: number) => {
    if (!confirm('Bạn có chắc chắn muốn xóa chủ đề này?')) return;
    
    try {
      setActionLoading(topicId);
      const response = await topicsApi.deleteTopic(topicId);
      if (response.success) {
        await fetchTopics();
      } else {
        setError(response.message || 'Không thể xóa chủ đề');
      }
    } catch (err: any) {
      // Extract error message from response
      const errorMessage = err.response?.data?.message || err.message || 'Không thể xóa chủ đề';
      setError(errorMessage);
    } finally {
      setActionLoading(null);
    }
  };

  const columns = [
    {
      key: 'title' as keyof TopicSummary,
      header: 'Chủ đề',
      sortable: true,
      render: (_: any, topic: TopicSummary) => (
        <div className="max-w-md">
          <div className="flex items-center space-x-2">
            {topic.isPinned && <StarIcon className="h-4 w-4 text-yellow-500" />}
            {topic.hasAnswer && <CheckIcon className="h-4 w-4 text-green-500" />}
            <DocumentTextIcon className="h-4 w-4 text-gray-400" />
          </div>
          <div className="mt-1">
            <Link
              to={`/topics/${topic.id}`}
              className="text-sm font-medium text-gray-900 hover:text-primary-600"
            >
              {truncateText(topic.title, 50)}
            </Link>
            <div className="text-xs text-gray-500 mt-1">
              {topic.categoryName} • by {topic.authorName}
            </div>
          </div>
        </div>
      ),
    },
    {
      key: 'authorRole' as keyof TopicSummary,
      header: 'Vai trò tác giả',
      render: (value: string) => (
        <Badge variant="secondary" size="sm">
          {value}
        </Badge>
      ),
    },
    {
      key: 'viewCount' as keyof TopicSummary,
      header: 'Lượt xem',
      render: (value: number) => (
        <div className="flex items-center space-x-1 text-sm text-gray-600">
          <EyeIcon className="h-4 w-4" />
          <span>{formatNumber(value)}</span>
        </div>
      ),
    },
    {
      key: 'likeCount' as keyof TopicSummary,
      header: 'Lượt thích',
      render: (value: number) => (
        <div className="flex items-center space-x-1 text-sm text-gray-600">
          <HeartIcon className="h-4 w-4" />
          <span>{formatNumber(value)}</span>
        </div>
      ),
    },
    {
      key: 'postCount' as keyof TopicSummary,
      header: 'Trả lời',
      render: (value: number) => (
        <div className="flex items-center space-x-1 text-sm text-gray-600">
          <ChatBubbleLeftRightIcon className="h-4 w-4" />
          <span>{formatNumber(value)}</span>
        </div>
      ),
    },
    {
      key: 'createdAt' as keyof TopicSummary,
      header: 'Đã tạo',
      render: (value: string) => (
        <div className="text-sm text-gray-900">
          {formatRelativeTime(value)}
        </div>
      ),
    },
    {
      key: 'id' as keyof TopicSummary,
      header: 'Hành động',
      render: (_: any, topic: TopicSummary) => (
        <div className="flex space-x-2">
          <button
            onClick={() => {
              setSelectedTopic(topic);
              setShowTopicModal(true);
            }}
            className="text-blue-600 hover:text-blue-900"
            title="Xem chi tiết"
          >
            <EyeIcon className="h-4 w-4" />
          </button>
          <button
            onClick={() => handleTogglePin(topic.id)}
            disabled={actionLoading === topic.id}
            className={`${
              topic.isPinned ? 'text-yellow-600 hover:text-yellow-900' : 'text-gray-600 hover:text-gray-900'
            }`}
            title={topic.isPinned ? 'Bỏ ghim' : 'Ghim'}
          >
            <StarIcon className="h-4 w-4" />
          </button>
          <button
            onClick={() => {
              setSelectedTopic(topic);
              setShowTopicModal(true);
            }}
            className="text-green-600 hover:text-green-900"
            title="Sửa"
          >
            <PencilIcon className="h-4 w-4" />
          </button>
          <button
            onClick={() => handleDelete(topic.id)}
            disabled={actionLoading === topic.id}
            className="text-red-600 hover:text-red-900"
            title="Xóa"
          >
            <TrashIcon className="h-4 w-4" />
          </button>
        </div>
      ),
    },
  ];

  return (
    <div className="space-y-6">
      {/* Header */}
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold text-gray-900">Chủ đề</h1>
          <p className="mt-1 text-sm text-gray-500">
            Quản lý chủ đề và thảo luận cộng đồng
          </p>
        </div>
      </div>

      {/* Filters */}
      <div className="card">
        <div className="flex items-center space-x-4">
          <button
            onClick={() => setShowFilters(!showFilters)}
            className="btn-outline flex items-center"
          >
            <FunnelIcon className="mr-2 h-4 w-4" />
Lọc
          </button>
        </div>

        {showFilters && (
          <div className="mt-4 grid grid-cols-1 gap-4 border-t border-gray-200 pt-4 sm:grid-cols-3">
            <div>
              <label className="block text-sm font-medium text-gray-700">Danh mục</label>
              <select
                value={selectedCategory}
                onChange={e => setSelectedCategory(e.target.value)}
                className="input-field mt-1"
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
              <label className="block text-sm font-medium text-gray-700">Trạng thái</label>
              <select
                value={selectedStatus}
                onChange={e => setSelectedStatus(e.target.value)}
                className="input-field mt-1"
              >
                <option value="">Tất cả chủ đề</option>
                <option value="pinned">Đã ghim</option>
                <option value="locked">Đã khóa</option>
                <option value="answered">Có câu trả lời</option>
              </select>
            </div>
          </div>
        )}
      </div>

      {/* Error Message */}
      {error && (
        <div className="rounded-md bg-red-50 p-4">
          <div className="text-sm text-red-700">{error}</div>
        </div>
      )}

      {/* Topics Table */}
      <div className="card">
        <Table
          data={topics}
          columns={columns}
          loading={isLoading}
          pagination={{
            currentPage,
            totalPages,
            onPageChange: setCurrentPage,
          }}
        />
      </div>

      {/* Topic Detail Modal */}
      {selectedTopic && (
        <Modal
          isOpen={showTopicModal}
          onClose={() => {
            setShowTopicModal(false);
            setSelectedTopic(null);
          }}
          title="Chi tiết chủ đề"
          size="lg"
        >
          <div className="space-y-6">
            <div>
              <h3 className="text-lg font-medium text-gray-900 mb-2">
                {selectedTopic.title}
              </h3>
              <div className="flex flex-wrap gap-2 mb-4">
                {selectedTopic.isPinned && (
                  <Badge variant="warning">Đã ghim</Badge>
                )}
                {selectedTopic.isLocked && (
                  <Badge variant="danger">Đã khóa</Badge>
                )}
                {selectedTopic.hasAnswer && (
                  <Badge variant="success">Có câu trả lời</Badge>
                )}
              </div>
            </div>

            <div className="grid grid-cols-2 gap-4">
              <div>
                <dt className="text-sm font-medium text-gray-500">Danh mục</dt>
                <dd className="mt-1 text-sm text-gray-900">{selectedTopic.categoryName}</dd>
              </div>
              <div>
                <dt className="text-sm font-medium text-gray-500">Tác giả</dt>
                <dd className="mt-1 text-sm text-gray-900">
                  {selectedTopic.authorName} ({selectedTopic.authorRole})
                </dd>
              </div>
              <div>
                <dt className="text-sm font-medium text-gray-500">Đã tạo</dt>
                <dd className="mt-1 text-sm text-gray-900">
                  {formatRelativeTime(selectedTopic.createdAt)}
                </dd>
              </div>
              <div>
                <dt className="text-sm font-medium text-gray-500">Hoạt động gần nhất</dt>
                <dd className="mt-1 text-sm text-gray-900">
                  {selectedTopic.lastActivityAt
                    ? formatRelativeTime(selectedTopic.lastActivityAt)
                    : 'Không có hoạt động'}
                </dd>
              </div>
            </div>

            <div className="grid grid-cols-3 gap-4">
              <div className="text-center">
                <div className="text-2xl font-bold text-gray-900">
                  {formatNumber(selectedTopic.viewCount)}
                </div>
                <div className="text-sm text-gray-500">Lượt xem</div>
              </div>
              <div className="text-center">
                <div className="text-2xl font-bold text-gray-900">
                  {formatNumber(selectedTopic.likeCount)}
                </div>
                <div className="text-sm text-gray-500">Lượt thích</div>
              </div>
              <div className="text-center">
                <div className="text-2xl font-bold text-gray-900">
                  {formatNumber(selectedTopic.postCount)}
                </div>
                <div className="text-sm text-gray-500">Trả lời</div>
              </div>
            </div>

            {selectedTopic.tags.length > 0 && (
              <div>
                <dt className="text-sm font-medium text-gray-500 mb-2">Thẻ</dt>
                <div className="flex flex-wrap gap-1">
                  {selectedTopic.tags.map((tag, index) => (
                    <Badge key={index} variant="secondary" size="sm">
                      {tag}
                    </Badge>
                  ))}
                </div>
              </div>
            )}

            <div className="flex justify-end space-x-3 border-t border-gray-200 pt-4">
              <button
                onClick={() => {
                  setShowTopicModal(false);
                  setSelectedTopic(null);
                }}
                className="btn-outline"
              >
                Đóng
              </button>
              <button
                onClick={() => {
                  setEditingTopic(selectedTopic);
                  setShowEditModal(true);
                  setShowTopicModal(false);
                }}
                className="btn-primary"
              >
                Sửa chủ đề
              </button>
            </div>
          </div>
        </Modal>
      )}

      {/* Edit Topic Modal */}
      {showEditModal && editingTopic && (
        <Modal
          isOpen={true}
          title="Sửa chủ đề"
          onClose={() => {
            setShowEditModal(false);
            setEditingTopic(null);
          }}
        >
          <div className="space-y-6">
            <form
              onSubmit={async (e) => {
                e.preventDefault();
                const formData = new FormData(e.currentTarget);
                const title = formData.get('title') as string;
                const content = formData.get('content') as string;
                const categoryId = parseInt(formData.get('categoryId') as string);

                try {
                  setActionLoading(editingTopic.id);
                  await topicsApi.updateTopic(editingTopic.id, {
                    title,
                    content,
                    categoryId,
                  });
                  
                  setShowEditModal(false);
                  setEditingTopic(null);
                  fetchTopics();
                } catch (error) {
                  console.error('Failed to update topic:', error);
                  setError('Failed to update topic. Please try again.');
                } finally {
                  setActionLoading(null);
                }
              }}
              className="space-y-4"
            >
              <div>
                <label htmlFor="title" className="block text-sm font-medium text-gray-700 mb-1">
                  Tiêu đề
                </label>
                <input
                  type="text"
                  id="title"
                  name="title"
                  defaultValue={editingTopic.title}
                  required
                  className="w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                />
              </div>

              <div>
                <label htmlFor="content" className="block text-sm font-medium text-gray-700 mb-1">
                  Nội dung
                </label>
                <textarea
                  id="content"
                  name="content"
                  rows={4}
                  defaultValue=""
                  required
                  className="w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                />
              </div>

              <div>
                <label htmlFor="categoryId" className="block text-sm font-medium text-gray-700 mb-1">
                  Danh mục
                </label>
                <select
                  id="categoryId"
                  name="categoryId"
                  defaultValue={categories[0]?.id || ''}
                  required
                  className="w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                >
                  {categories.map((category) => (
                    <option key={category.id} value={category.id}>
                      {category.name}
                    </option>
                  ))}
                </select>
              </div>

              <div>
                <label htmlFor="tags" className="block text-sm font-medium text-gray-700 mb-1">
                  Thẻ (phân cách bằng dấu phẩy)
                </label>
                <input
                  type="text"
                  id="tags"
                  name="tags"
                  defaultValue={editingTopic.tags.join(', ')}
                  className="w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                  placeholder="tag1, tag2, tag3"
                />
              </div>

              <div className="flex justify-end space-x-3 border-t border-gray-200 pt-4">
                <button
                  type="button"
                  onClick={() => {
                    setShowEditModal(false);
                    setEditingTopic(null);
                  }}
                  className="btn-outline"
                  disabled={actionLoading === editingTopic.id}
                >
                  Hủy
                </button>
                <button
                  type="submit"
                  className="btn-primary"
                  disabled={actionLoading === editingTopic.id}
                >
                  {actionLoading === editingTopic.id ? 'Đang cập nhật...' : 'Cập nhật chủ đề'}
                </button>
              </div>
            </form>
          </div>
        </Modal>
      )}
    </div>
  );
};

export default TopicsPage;
