import React, { useState, useEffect } from 'react';
import {
  PlusIcon,
  PencilIcon,
  TrashIcon,
  FolderIcon,
  ArrowsUpDownIcon,
  CheckCircleIcon,
  XCircleIcon,
} from '@heroicons/react/24/outline';
import { categoriesApi } from '../services/api';
import type { Category, CreateCategoryRequest, UpdateCategoryRequest } from '../types';
import { usePageTitle } from '../utils';
import Table from '../components/UI/Table';
import Modal from '../components/UI/Modal';
import Badge from '../components/UI/Badge';
import { formatRelativeTime, formatNumber, getStatusText } from '../utils';

const CategoriesPage: React.FC = () => {
  usePageTitle('Quản lý danh mục');

  const [categories, setCategories] = useState<Category[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [showCreateModal, setShowCreateModal] = useState(false);
  const [showEditModal, setShowEditModal] = useState(false);
  const [selectedCategory, setSelectedCategory] = useState<Category | null>(null);
  const [isSubmitting, setIsSubmitting] = useState(false);

  const [formData, setFormData] = useState<CreateCategoryRequest>({
    name: '',
    description: '',
    icon: '',
    color: '#3B82F6',
    displayOrder: 0,
  });

  useEffect(() => {
    fetchCategories();
  }, []);

  const fetchCategories = async () => {
    try {
      setIsLoading(true);
      const response = await categoriesApi.getCategories();
      if (response.success) {
        setCategories(response.data);
      } else {
        setError(response.message);
      }
    } catch (err: any) {
      setError(err.message || 'Failed to load categories');
    } finally {
      setIsLoading(false);
    }
  };

  const handleCreate = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      setIsSubmitting(true);
      const response = await categoriesApi.createCategory(formData);
      if (response.success) {
        setShowCreateModal(false);
        setFormData({
          name: '',
          description: '',
          icon: '',
          color: '#3B82F6',
          displayOrder: 0,
        });
        await fetchCategories();
      }
    } catch (err: any) {
      setError(err.message || 'Failed to create category');
    } finally {
      setIsSubmitting(false);
    }
  };

  const handleEdit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!selectedCategory) return;
    
    try {
      setIsSubmitting(true);
      const updateData: UpdateCategoryRequest = {
        name: formData.name,
        description: formData.description,
        icon: formData.icon,
        color: formData.color,
        displayOrder: formData.displayOrder,
      };
      
      const response = await categoriesApi.updateCategory(selectedCategory.id, updateData);
      if (response.success) {
        setShowEditModal(false);
        setSelectedCategory(null);
        await fetchCategories();
      }
    } catch (err: any) {
      setError(err.message || 'Failed to update category');
    } finally {
      setIsSubmitting(false);
    }
  };

  const handleDelete = async (categoryId: number) => {
    if (!confirm('Bạn có chắc chắn muốn xóa danh mục này?')) return;
    
    try {
      const response = await categoriesApi.deleteCategory(categoryId);
      if (response.success) {
        await fetchCategories();
      } else {
        setError(response.message || 'Không thể xóa danh mục');
      }
    } catch (err: any) {
      // Extract error message from response
      const errorMessage = err.response?.data?.message || err.message || 'Không thể xóa danh mục';
      setError(errorMessage);
    }
  };

  const handleToggleStatus = async (categoryId: number) => {
    try {
      await categoriesApi.toggleCategoryStatus(categoryId);
      await fetchCategories();
    } catch (err: any) {
      setError(err.message || 'Failed to toggle category status');
    }
  };

  const openEditModal = (category: Category) => {
    setSelectedCategory(category);
    setFormData({
      name: category.name,
      description: category.description,
      icon: category.icon || '',
      color: category.color || '#3B82F6',
      displayOrder: category.displayOrder,
    });
    setShowEditModal(true);
  };

  const columns = [
    {
      key: 'name' as keyof Category,
      header: 'Danh mục',
      sortable: true,
      width: 'w-2/5',
      render: (_: any, category: Category) => (
        <div className="flex items-center">
          <div 
            className="flex h-10 w-10 items-center justify-center rounded-lg text-white"
            style={{ backgroundColor: category.color || '#3B82F6' }}
          >
            {category.icon ? (
              <span className="text-lg">{category.icon}</span>
            ) : (
              <FolderIcon className="h-5 w-5" />
            )}
          </div>
          <div className="ml-4">
            <div className="text-sm font-medium text-gray-900">{category.name}</div>
            <div className="text-sm text-gray-500">{category.description}</div>
          </div>
        </div>
      ),
    },
    {
      key: 'displayOrder' as keyof Category,
      header: 'Thứ tự',
      width: 'w-16', // Cố định 64px cho cột thứ tự
      render: (value: number) => (
        <span className="text-sm text-gray-900">{value}</span>
      ),
    },
    {
      key: 'isActive' as keyof Category,
      header: 'Trạng thái',
      width: 'w-24', // 96px cho trạng thái
      render: (value: boolean) => (
        <Badge variant={value ? 'success' : 'danger'}>
          {getStatusText(value)}
        </Badge>
      ),
    },
    {
      key: 'topicCount' as keyof Category,
      header: 'Chủ đề',
      width: 'w-20', // 80px cho số chủ đề
      render: (value: number) => formatNumber(value),
    },
    {
      key: 'postCount' as keyof Category,
      header: 'Bài viết',
      width: 'w-20', // 80px cho số bài viết
      render: (value: number) => formatNumber(value),
    },
    {
      key: 'createdAt' as keyof Category,
      header: 'Tạo lúc',
      width: 'w-28', // 112px cho thời gian
      render: (value: string) => formatRelativeTime(value),
    },
    {
      key: 'id' as keyof Category,
      header: 'Thao tác',
      width: 'w-28', // 112px cho các nút thao tác
      noWrap: false,
      render: (_: any, category: Category) => (
        <div className="flex space-x-1 justify-center">
          <button
            onClick={() => openEditModal(category)}
            className="text-blue-600 hover:text-blue-900 p-1 rounded hover:bg-blue-50"
            title="Edit"
          >
            <PencilIcon className="h-4 w-4" />
          </button>
          <button
            onClick={() => handleToggleStatus(category.id)}
            className={`p-1 rounded hover:bg-gray-50 ${
              category.isActive ? 'text-yellow-600 hover:text-yellow-900' : 'text-green-600 hover:text-green-900'
            }`}
            title={category.isActive ? 'Deactivate' : 'Activate'}
          >
            {category.isActive ? (
              <XCircleIcon className="h-4 w-4" />
            ) : (
              <CheckCircleIcon className="h-4 w-4" />
            )}
          </button>
          <button
            onClick={() => handleDelete(category.id)}
            className="text-red-600 hover:text-red-900 p-1 rounded hover:bg-red-50"
            title="Delete"
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
          <h1 className="text-2xl font-bold text-gray-900">Danh mục</h1>
          <p className="mt-1 text-sm text-gray-500">
            Quản lý danh mục chủ đề và tổ chức
          </p>
        </div>
        <div className="flex space-x-3">
          <button className="btn-outline flex items-center">
            <ArrowsUpDownIcon className="mr-2 h-4 w-4" />
Sắp xếp lại
          </button>
          <button 
            onClick={() => setShowCreateModal(true)}
            className="btn-primary flex items-center"
          >
            <PlusIcon className="mr-2 h-4 w-4" />
Thêm danh mục
          </button>
        </div>
      </div>

      {/* Error Message */}
      {error && (
        <div className="rounded-md bg-red-50 p-4">
          <div className="text-sm text-red-700">{error}</div>
        </div>
      )}

      {/* Categories Table */}
      <div className="card">
        <Table
          data={categories}
          columns={columns}
          loading={isLoading}
        />
      </div>

      {/* Create Category Modal */}
      <Modal
        isOpen={showCreateModal}
        onClose={() => setShowCreateModal(false)}
        title="Create Category"
      >
        <form onSubmit={handleCreate} className="space-y-4">
          <div>
            <label className="block text-sm font-medium text-gray-700">Tên</label>
            <input
              type="text"
              value={formData.name}
              onChange={e => setFormData({ ...formData, name: e.target.value })}
              className="input-field mt-1"
              required
            />
          </div>
          
          <div>
            <label className="block text-sm font-medium text-gray-700">Mô tả</label>
            <textarea
              value={formData.description}
              onChange={e => setFormData({ ...formData, description: e.target.value })}
              className="input-field mt-1"
              rows={3}
              required
            />
          </div>
          
          <div className="grid grid-cols-2 gap-4">
            <div>
              <label className="block text-sm font-medium text-gray-700">Icon (Emoji)</label>
              <input
                type="text"
                value={formData.icon}
                onChange={e => setFormData({ ...formData, icon: e.target.value })}
                className="input-field mt-1"
                placeholder="🏥"
              />
            </div>
            
            <div>
              <label className="block text-sm font-medium text-gray-700">Color</label>
              <input
                type="color"
                value={formData.color}
                onChange={e => setFormData({ ...formData, color: e.target.value })}
                className="mt-1 h-10 w-full rounded-lg border border-gray-300"
              />
            </div>
          </div>
          
          <div>
            <label className="block text-sm font-medium text-gray-700">Display Order</label>
            <input
              type="number"
              value={formData.displayOrder}
              onChange={e => setFormData({ ...formData, displayOrder: parseInt(e.target.value) })}
              className="input-field mt-1"
              min="0"
            />
          </div>

          <div className="flex justify-end space-x-3 border-t border-gray-200 pt-4">
            <button
              type="button"
              onClick={() => setShowCreateModal(false)}
              className="btn-outline"
            >
Hủy
            </button>
            <button
              type="submit"
              disabled={isSubmitting}
              className="btn-primary disabled:opacity-50"
            >
{isSubmitting ? 'Đang tạo...' : 'Tạo danh mục'}
            </button>
          </div>
        </form>
      </Modal>

      {/* Edit Category Modal */}
      <Modal
        isOpen={showEditModal}
        onClose={() => {
          setShowEditModal(false);
          setSelectedCategory(null);
        }}
        title="Sửa danh mục"
      >
        <form onSubmit={handleEdit} className="space-y-4">
          <div>
            <label className="block text-sm font-medium text-gray-700">Tên</label>
            <input
              type="text"
              value={formData.name}
              onChange={e => setFormData({ ...formData, name: e.target.value })}
              className="input-field mt-1"
              required
            />
          </div>
          
          <div>
            <label className="block text-sm font-medium text-gray-700">Mô tả</label>
            <textarea
              value={formData.description}
              onChange={e => setFormData({ ...formData, description: e.target.value })}
              className="input-field mt-1"
              rows={3}
              required
            />
          </div>
          
          <div className="grid grid-cols-2 gap-4">
            <div>
              <label className="block text-sm font-medium text-gray-700">Icon (Emoji)</label>
              <input
                type="text"
                value={formData.icon}
                onChange={e => setFormData({ ...formData, icon: e.target.value })}
                className="input-field mt-1"
                placeholder="🏥"
              />
            </div>
            
            <div>
              <label className="block text-sm font-medium text-gray-700">Color</label>
              <input
                type="color"
                value={formData.color}
                onChange={e => setFormData({ ...formData, color: e.target.value })}
                className="mt-1 h-10 w-full rounded-lg border border-gray-300"
              />
            </div>
          </div>
          
          <div>
            <label className="block text-sm font-medium text-gray-700">Display Order</label>
            <input
              type="number"
              value={formData.displayOrder}
              onChange={e => setFormData({ ...formData, displayOrder: parseInt(e.target.value) })}
              className="input-field mt-1"
              min="0"
            />
          </div>

          <div className="flex justify-end space-x-3 border-t border-gray-200 pt-4">
            <button
              type="button"
              onClick={() => {
                setShowEditModal(false);
                setSelectedCategory(null);
              }}
              className="btn-outline"
            >
              Hủy
            </button>
            <button
              type="submit"
              disabled={isSubmitting}
              className="btn-primary disabled:opacity-50"
            >
{isSubmitting ? 'Đang cập nhật...' : 'Cập nhật danh mục'}
            </button>
          </div>
        </form>
      </Modal>
    </div>
  );
};

export default CategoriesPage;
