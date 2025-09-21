import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import {
  UserIcon,
  StarIcon,
  ChatBubbleLeftRightIcon,
  CheckBadgeIcon,
  MagnifyingGlassIcon,
  MapPinIcon,
  ClockIcon,
} from '@heroicons/react/24/outline';
import { formatNumber } from '../utils';
import { expertApi } from '../services/api';
import type { ExpertSummary, ExpertSearchRequest } from '../types';
import { usePageTitle } from '../utils';

const ExpertsPage: React.FC = () => {
  usePageTitle('Tư vấn chuyên gia');
  const [experts, setExperts] = useState<ExpertSummary[]>([]);
  const [specialties, setSpecialties] = useState<{ id: number; name: string; }[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [searchQuery, setSearchQuery] = useState('');
  const [selectedSpecialty, setSelectedSpecialty] = useState('all');
  const [sortBy, setSortBy] = useState('rating');
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
  }, [selectedSpecialty, sortBy]);

  const loadData = async (page = 1, append = false) => {
    try {
      if (!append) {
        setIsLoading(true);
        setError(null);
      } else {
        setIsLoadingMore(true);
      }

      // Build request parameters
      const params: Partial<ExpertSearchRequest> = {
        page,
        pageSize: 20,
        sortBy: sortBy === 'rating' ? 'rating' : sortBy === 'experience' ? 'createdAt' : sortBy,
        sortOrder: 'desc',
      };

      if (searchQuery.trim()) {
        params.query = searchQuery.trim();
      }

      if (selectedSpecialty !== 'all') {
        params.specialty = selectedSpecialty;
      }

      // Load experts
      const expertsResponse = await expertApi.getExperts(params);

      if (expertsResponse.success) {
        if (append) {
          setExperts(prev => [...prev, ...expertsResponse.data.items]);
        } else {
          setExperts(expertsResponse.data.items);
        }
        setCurrentPage(expertsResponse.data.page);
        setTotalPages(expertsResponse.data.totalPages);
      }

      // Load specialties on first page load
      if (page === 1) {
        const specialtiesResponse = await expertApi.getSpecialties();
        if (specialtiesResponse.success) {
          setSpecialties(specialtiesResponse.data);
        }
      }
    } catch (err) {
      setError('Không thể tải danh sách chuyên gia. Vui lòng thử lại.');
    } finally {
      setIsLoading(false);
      setIsLoadingMore(false);
    }
  };

  const loadMore = () => {
    if (currentPage < totalPages && !isLoadingMore) {
      loadData(currentPage + 1, true);
    }
  };

  const handleSearch = (e: React.FormEvent) => {
    e.preventDefault();
    setCurrentPage(1);
    loadData();
  };

  // Experts are already filtered and sorted by the API
  const displayExperts = experts;

  if (isLoading) {
    return (
      <div className="min-h-screen bg-gray-50">
        <div className="mx-auto max-w-7xl px-4 py-8 sm:px-6 lg:px-8">
          <div className="animate-pulse space-y-6">
            <div className="h-8 bg-gray-200 rounded w-1/4"></div>
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
              {[...Array(6)].map((_, i) => (
                <div key={i} className="bg-white p-6 rounded-lg shadow-sm">
                  <div className="h-16 w-16 bg-gray-200 rounded-full mb-4"></div>
                  <div className="h-6 bg-gray-200 rounded w-3/4 mb-2"></div>
                  <div className="h-4 bg-gray-200 rounded w-full mb-4"></div>
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
          <h2 className="text-2xl font-bold text-gray-900 mb-4">Lỗi tải danh sách chuyên gia</h2>
          <p className="text-gray-600">{error}</p>
        </div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gray-50">
      <div className="mx-auto max-w-7xl px-4 py-8 sm:px-6 lg:px-8">
        {/* Header */}
        <div className="mb-8 text-center">
          <h1 className="text-3xl font-bold text-gray-900 mb-4">Chuyên gia y tế</h1>
          <p className="text-gray-600 max-w-2xl mx-auto">
            Kết nối với các chuyên gia y tế đã được xác minh và nhận lời khuyên chuyên môn về các vấn đề sức khỏe của bạn
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
                placeholder="Tìm kiếm chuyên gia, chuyên khoa..."
              />
            </div>
          </div>

          {/* Specialty Filter & Sort */}
          <div className="flex space-x-4">
            <select
              value={selectedSpecialty}
              onChange={(e) => setSelectedSpecialty(e.target.value)}
              className="rounded-lg border border-gray-300 bg-white px-3 py-2 text-sm focus:border-primary-500 focus:outline-none focus:ring-1 focus:ring-primary-500"
            >
              <option value="all">Tất cả chuyên khoa</option>
              {specialties.map((specialty) => (
                <option key={specialty.id} value={specialty.id}>{specialty.name}</option>
              ))}
            </select>

            <select
              value={sortBy}
              onChange={(e) => setSortBy(e.target.value)}
              className="rounded-lg border border-gray-300 bg-white px-3 py-2 text-sm focus:border-primary-500 focus:outline-none focus:ring-1 focus:ring-primary-500"
            >
              <option value="rating">Đánh giá cao nhất</option>
              <option value="experience">Kinh nghiệm nhiều nhất</option>
              <option value="answers">Nhiều câu trả lời nhất</option>
              <option value="reviews">Nhiều đánh giá nhất</option>
            </select>
          </div>
        </div>

        {/* Experts Grid */}
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {displayExperts.map((expert: ExpertSummary) => (
            <div key={expert.id} className="bg-white rounded-lg shadow-sm border border-gray-200 p-6 hover:shadow-md transition-shadow">
              {/* Expert Header */}
              <div className="flex items-center space-x-4 mb-4">
                <div className="relative">
                  {expert.avatar ? (
                    <img
                      className="h-16 w-16 rounded-full object-cover"
                      src={expert.avatar}
                      alt={expert.fullName}
                    />
                  ) : (
                    <div className="h-16 w-16 rounded-full bg-gray-300 flex items-center justify-center">
                      <UserIcon className="h-8 w-8 text-gray-600" />
                    </div>
                  )}
                  {expert.isOnline && (
                    <div className="absolute -bottom-1 -right-1 h-5 w-5 rounded-full bg-green-400 border-2 border-white"></div>
                  )}
                </div>

                <div className="flex-1">
                  <div className="flex items-center space-x-2">
                    <h3 className="text-lg font-semibold text-gray-900">{expert.fullName}</h3>
                    {expert.isVerified && (
                      <CheckBadgeIcon className="h-5 w-5 text-blue-500" />
                    )}
                  </div>
                  <p className="text-sm text-gray-500">@{expert.username}</p>
                  
                  {/* Status */}
                  <div className="flex items-center space-x-1 text-xs text-gray-500 mt-1">
                    <ClockIcon className="h-3 w-3" />
                    <span>
                      {expert.isOnline 
                        ? 'Đang trực tuyến' 
                        : `Lần cuối trực tuyến ${new Date(expert.lastSeen!).toLocaleDateString()}`
                      }
                    </span>
                  </div>
                </div>
              </div>

              {/* Specialties */}
              <div className="mb-4">
                <div className="flex flex-wrap gap-2">
                  {expert.specialties.map((specialty) => (
                    <span
                      key={specialty}
                      className="inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium bg-blue-100 text-blue-800"
                    >
                      {specialty}
                    </span>
                  ))}
                </div>
              </div>

              {/* Bio */}
              <p className="text-sm text-gray-600 mb-4 line-clamp-3">
                {expert.bio}
              </p>

              {/* Stats */}
              <div className="flex items-center justify-between text-sm text-gray-500 mb-4">
                <div className="flex items-center space-x-1">
                  <StarIcon className="h-4 w-4 text-yellow-400 fill-current" />
                  <span className="font-medium text-gray-900">{expert.rating}</span>
                  <span>({formatNumber(expert.reviewCount)} đánh giá)</span>
                </div>
                <div className="flex items-center space-x-1">
                  <ChatBubbleLeftRightIcon className="h-4 w-4" />
                  <span>{formatNumber(expert.answerCount)} câu trả lời</span>
                </div>
              </div>

              {/* Location & Experience */}
              <div className="flex items-center justify-between text-sm text-gray-500 mb-4">
                <div className="flex items-center space-x-1">
                  <MapPinIcon className="h-4 w-4" />
                  <span>{expert.location}</span>
                </div>
                <span className="font-medium">{expert.experience}</span>
              </div>

              {/* Action Buttons */}
              <div className="flex space-x-2">
                <Link
                  to={`/users/${expert.username}`}
                  className="flex-1 inline-flex justify-center items-center px-3 py-2 border border-gray-300 shadow-sm text-sm font-medium rounded-md text-gray-700 bg-white hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-primary-500"
                >
                  Xem hồ sơ
                </Link>
                <Link
                  to={`/topics/create?expert=${expert.username}`}
                  className="flex-1 inline-flex justify-center items-center px-3 py-2 border border-transparent text-sm font-medium rounded-md text-white bg-primary-600 hover:bg-primary-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-primary-500"
                >
                  Đặt câu hỏi
                </Link>
              </div>
            </div>
          ))}
        </div>

        {/* Empty State */}
        {displayExperts.length === 0 && !isLoading && (
          <div className="text-center py-12">
            <UserIcon className="mx-auto h-12 w-12 text-gray-400" />
            <h3 className="mt-4 text-lg font-medium text-gray-900">Không tìm thấy chuyên gia nào</h3>
            <p className="mt-2 text-gray-500">
              {searchQuery || selectedSpecialty !== 'all' 
                ? 'Thử điều chỉnh tìm kiếm hoặc bộ lọc'
                : 'Hiện tại không có chuyên gia y tế nào'
              }
            </p>
          </div>
        )}

        {/* Load More Button */}
        {displayExperts.length > 0 && currentPage < totalPages && (
          <div className="mt-8 text-center">
            <button 
              onClick={loadMore}
              disabled={isLoadingMore}
              className="inline-flex items-center px-6 py-3 border border-gray-300 shadow-sm text-base font-medium rounded-md text-gray-700 bg-white hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-primary-500 disabled:opacity-50"
            >
              {isLoadingMore ? 'Đang tải...' : 'Xem thêm chuyên gia'}
            </button>
          </div>
        )}
      </div>
    </div>
  );
};

export default ExpertsPage;
