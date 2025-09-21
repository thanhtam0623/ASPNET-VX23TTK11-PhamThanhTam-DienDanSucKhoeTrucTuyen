import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import {
  HeartIcon,
  AcademicCapIcon,
  EyeIcon,
  ShieldCheckIcon,
  HomeIcon,
  UserGroupIcon,
  DocumentTextIcon,
  ChartBarIcon,
  MagnifyingGlassIcon,
  TagIcon,
} from '@heroicons/react/24/outline';
import { formatNumber, usePageTitle } from '../utils';
import { categoryApi } from '../services/api';
import type { Category } from '../types';

const CategoriesPage: React.FC = () => {
  usePageTitle('Chủ đề sức khỏe');
  const [categories, setCategories] = useState<Category[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [searchQuery, setSearchQuery] = useState('');
  const [selectedFilter, setSelectedFilter] = useState('all');

  // Load categories data
  useEffect(() => {
    loadCategories();
  }, []);

  const loadCategories = async () => {
    try {
      setIsLoading(true);
      setError(null);
      
      const response = await categoryApi.getAllCategories();
      
      if (response.success) {
        setCategories(response.data);
      } else {
        setError('Không thể tải danh mục');
      }
    } catch (err) {
      setError('Không thể tải danh mục. Vui lòng thử lại.');
    } finally {
      setIsLoading(false);
    }
  };

  const getIconComponent = (iconName?: string) => {
    const iconMap: { [key: string]: React.ComponentType<any> } = {
      heart: HeartIcon,
      brain: AcademicCapIcon,
      eye: EyeIcon,
      shield: ShieldCheckIcon,
      home: HomeIcon,
      users: UserGroupIcon,
      document: DocumentTextIcon,
      chart: ChartBarIcon,
    };
    
    return iconMap[iconName || 'document'] || DocumentTextIcon;
  };

  const filteredCategories = categories.filter(category => {
    const matchesSearch = category.name.toLowerCase().includes(searchQuery.toLowerCase()) ||
                         category.description.toLowerCase().includes(searchQuery.toLowerCase());
    const matchesFilter = selectedFilter === 'all' || 
                         (selectedFilter === 'popular' && category.topicCount > 100);
    return matchesSearch && matchesFilter;
  });

  const sortedCategories = [...filteredCategories].sort((a, b) => {
    return b.topicCount - a.topicCount;
  });

  if (isLoading) {
    return (
      <div className="min-h-screen bg-gray-50">
        <div className="mx-auto max-w-7xl px-4 py-8 sm:px-6 lg:px-8">
          <div className="animate-pulse space-y-6">
            <div className="h-8 bg-gray-200 rounded w-1/4"></div>
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
              {[...Array(8)].map((_, i) => (
                <div key={i} className="bg-white rounded-lg shadow-sm p-6">
                  <div className="w-12 h-12 bg-gray-200 rounded-lg mb-4"></div>
                  <div className="h-6 bg-gray-200 rounded mb-2"></div>
                  <div className="h-4 bg-gray-200 rounded mb-4"></div>
                  <div className="h-4 bg-gray-200 rounded w-3/4"></div>
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
      <div className="min-h-screen bg-gray-50">
        <div className="mx-auto max-w-7xl px-4 py-8 sm:px-6 lg:px-8">
          <div className="text-center py-12">
            <div className="text-red-500 text-lg font-medium mb-4">{error}</div>
            <button
              onClick={loadCategories}
              className="inline-flex items-center px-4 py-2 border border-transparent text-sm font-medium rounded-md text-white bg-primary-600 hover:bg-primary-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-primary-500"
            >
              Thử lại
            </button>
          </div>
        </div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gray-50">
      <div className="mx-auto max-w-7xl px-4 py-8 sm:px-6 lg:px-8">
        {/* Header */}
        <div className="mb-8 text-center">
          <h1 className="text-3xl font-bold text-gray-900 mb-4">Chủ đề sức khỏe</h1>
          <p className="text-gray-600 max-w-2xl mx-auto">
            Duyệt các chủ đề sức khỏe theo danh mục để tìm lời khuyên chuyên gia và thảo luận cộng đồng
          </p>
        </div>

        {/* Filters */}
        <div className="mb-8 space-y-4 lg:flex lg:items-center lg:justify-between lg:space-y-0">
          {/* Search */}
          <div className="relative max-w-md">
            <div className="pointer-events-none absolute inset-y-0 left-0 flex items-center pl-3">
              <MagnifyingGlassIcon className="h-5 w-5 text-gray-400" />
            </div>
            <input
              type="text"
              value={searchQuery}
              onChange={(e) => setSearchQuery(e.target.value)}
              className="block w-full rounded-lg border border-gray-300 bg-white py-2 pl-10 pr-3 text-sm focus:border-primary-500 focus:outline-none focus:ring-1 focus:ring-primary-500"
              placeholder="Tìm kiếm chủ đề sức khỏe..."
            />
          </div>

          {/* Filter */}
          <div className="flex space-x-4">
            <select
              value={selectedFilter}
              onChange={(e) => setSelectedFilter(e.target.value)}
              className="rounded-lg border border-gray-300 bg-white px-3 py-2 text-sm focus:border-primary-500 focus:outline-none focus:ring-1 focus:ring-primary-500"
            >
              <option value="all">Tất cả danh mục</option>
              <option value="popular">Danh mục phổ biến</option>
            </select>
          </div>
        </div>

        {/* Stats */}
        <div className="mb-8 grid grid-cols-2 gap-4 sm:grid-cols-4">
          <div className="bg-white rounded-lg p-4 text-center shadow-sm">
            <div className="text-2xl font-bold text-primary-600">
              {formatNumber(categories.length)}
            </div>
            <div className="text-sm text-gray-500">Danh mục</div>
          </div>
          <div className="bg-white rounded-lg p-4 text-center shadow-sm">
            <div className="text-2xl font-bold text-primary-600">
              {formatNumber(categories.reduce((sum, cat) => sum + cat.topicCount, 0))}
            </div>
            <div className="text-sm text-gray-500">Tổng chủ đề</div>
          </div>
          <div className="bg-white rounded-lg p-4 text-center shadow-sm">
            <div className="text-2xl font-bold text-primary-600">
              {categories.filter(cat => cat.topicCount > 100).length}
            </div>
            <div className="text-sm text-gray-500">Phổ biến</div>
          </div>
          <div className="bg-white rounded-lg p-4 text-center shadow-sm">
            <div className="text-2xl font-bold text-primary-600">24/7</div>
            <div className="text-sm text-gray-500">Hỗ trợ chuyên gia</div>
          </div>
        </div>

        {/* Categories Grid */}
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
          {sortedCategories.map((category) => {
            const IconComponent = getIconComponent(category.icon);
            
            return (
              <Link
                key={category.id}
                to={`/categories/${category.slug}`}
                className="group bg-white rounded-lg shadow-sm border border-gray-200 p-6 hover:shadow-md hover:border-primary-200 transition-all duration-200"
              >
                {/* Category Icon */}
                <div className={`inline-flex items-center justify-center w-12 h-12 rounded-lg ${category.color} text-white mb-4 group-hover:scale-110 transition-transform duration-200`}>
                  <IconComponent className="h-6 w-6" />
                </div>

                {/* Popular Badge */}
                {category.topicCount > 100 && (
                  <div className="mb-3">
                    <span className="inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium bg-yellow-100 text-yellow-800">
                      <TagIcon className="w-3 h-3 mr-1" />
                      Phổ biến
                    </span>
                  </div>
                )}

                {/* Category Info */}
                <h3 className="text-lg font-semibold text-gray-900 mb-2 group-hover:text-primary-600 transition-colors">
                  {category.name}
                </h3>
                
                <p className="text-sm text-gray-600 mb-4 line-clamp-2">
                  {category.description}
                </p>

                {/* Topic Count */}
                <div className="flex items-center justify-between text-sm">
                  <span className="text-gray-500">
                    {formatNumber(category.topicCount)} chủ đề
                  </span>
                  <span className="text-primary-600 group-hover:text-primary-700 font-medium">
                    Khám phá →
                  </span>
                </div>
              </Link>
            );
          })}
        </div>

        {/* Empty State */}
        {sortedCategories.length === 0 && !isLoading && (
          <div className="text-center py-12">
            <TagIcon className="mx-auto h-12 w-12 text-gray-400" />
            <h3 className="mt-4 text-lg font-medium text-gray-900">Không tìm thấy danh mục nào</h3>
            <p className="mt-2 text-gray-500">
              {searchQuery 
                ? 'Thử điều chỉnh từ khóa tìm kiếm'
                : 'Hiện tại không có danh mục sức khỏe nào'
              }
            </p>
          </div>
        )}

        {/* Call to Action */}
        <div className="mt-12 bg-primary-50 rounded-lg p-8 text-center">
          <h3 className="text-xl font-semibold text-gray-900 mb-4">
            Không tìm thấy thông tin bạn cần?
          </h3>
          <p className="text-gray-600 mb-6">
            Đặt câu hỏi cho cộng đồng các chuyên gia y tế và nhận lời khuyên chuyên môn
          </p>
          <div className="space-x-4">
            <Link
              to="/topics/create"
              className="inline-flex items-center px-6 py-3 border border-transparent text-base font-medium rounded-md text-white bg-primary-600 hover:bg-primary-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-primary-500"
            >
              Đặt câu hỏi
            </Link>
            <Link
              to="/experts"
              className="inline-flex items-center px-6 py-3 border border-gray-300 text-base font-medium rounded-md text-gray-700 bg-white hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-primary-500"
            >
              Duyệt chuyên gia
            </Link>
          </div>
        </div>
      </div>
    </div>
  );
};

export default CategoriesPage;
