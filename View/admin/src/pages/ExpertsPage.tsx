import React, { useState, useEffect } from 'react';
import {
  UserIcon,
  EyeIcon,
  PlusIcon,
  StarIcon,
  FunnelIcon,
} from '@heroicons/react/24/outline';
import { CheckBadgeIcon as CheckBadgeIconSolid } from '@heroicons/react/24/solid';
import { expertsApi } from '../services/api';
import { usePageTitle } from '../utils';
import type { 
  ExpertSummary, 
  ExpertDetail, 
  ExpertSpecialty, 
  ExpertSearchRequest,
  PaginatedResponse,
  CreateExpertSpecialtyRequest 
} from '../types';
import { formatRelativeTime, getStatusColor } from '../utils';
import Modal from '../components/UI/Modal';

const ExpertsPage: React.FC = () => {
  usePageTitle('Quản lý chuyên gia');
  
  const [experts, setExperts] = useState<PaginatedResponse<ExpertSummary> | null>(null);
  const [specialties, setSpecialties] = useState<ExpertSpecialty[]>([]);
  const [selectedExpert, setSelectedExpert] = useState<ExpertDetail | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [searchParams, setSearchParams] = useState<ExpertSearchRequest>({
    page: 1,
    pageSize: 20,
    sortBy: 'newest',
  });
  
  // Modal states
  const [showExpertDetail, setShowExpertDetail] = useState(false);
  const [showSpecialtyModal, setShowSpecialtyModal] = useState(false);
  const [showFilters, setShowFilters] = useState(false);
  const [editingSpecialty, setEditingSpecialty] = useState<ExpertSpecialty | null>(null);
  const [specialtyForm, setSpecialtyForm] = useState<CreateExpertSpecialtyRequest>({
    name: '',
    description: '',
  });

  useEffect(() => {
    fetchExperts();
    fetchSpecialties();
  }, [searchParams]);


  const fetchExperts = async () => {
    try {
      setIsLoading(true);
      const response = await expertsApi.getExperts(searchParams);
      if (response.success) {
        setExperts(response.data);
      } else {
        setError(response.message);
      }
    } catch (err: any) {
      setError(err.message || 'Failed to load experts');
    } finally {
      setIsLoading(false);
    }
  };

  const fetchSpecialties = async () => {
    try {
      const response = await expertsApi.getSpecialties();
      if (response.success) {
        setSpecialties(response.data);
      }
    } catch (err: any) {
      console.error('Failed to load specialties:', err);
    }
  };


  const handleVerifyExpert = async (expertId: number) => {
    try {
      const response = await expertsApi.verifyExpert(expertId);
      if (response.success) {
        fetchExperts();
        if (selectedExpert && selectedExpert.id === expertId) {
          const updatedExpert = await expertsApi.getExpertById(expertId);
          if (updatedExpert.success) {
            setSelectedExpert(updatedExpert.data);
          }
        }
      }
    } catch (err: any) {
      setError(err.message || 'Failed to verify expert');
    }
  };

  const handleRejectExpert = async (expertId: number) => {
    try {
      const response = await expertsApi.rejectExpert(expertId);
      if (response.success) {
        fetchExperts();
        if (selectedExpert && selectedExpert.id === expertId) {
          const updatedExpert = await expertsApi.getExpertById(expertId);
          if (updatedExpert.success) {
            setSelectedExpert(updatedExpert.data);
          }
        }
      }
    } catch (err: any) {
      setError(err.message || 'Failed to reject expert');
    }
  };

  const handleToggleStatus = async (expertId: number) => {
    try {
      const response = await expertsApi.toggleExpertStatus(expertId);
      if (response.success) {
        fetchExperts();
        if (selectedExpert && selectedExpert.id === expertId) {
          const updatedExpert = await expertsApi.getExpertById(expertId);
          if (updatedExpert.success) {
            setSelectedExpert(updatedExpert.data);
          }
        }
      }
    } catch (err: any) {
      setError(err.message || 'Failed to toggle expert status');
    }
  };

  const viewExpertDetail = async (expertId: number) => {
    try {
      setError(null); // Clear previous errors
      console.log('Fetching expert details for ID:', expertId); // Debug log
      
      const response = await expertsApi.getExpertById(expertId);
      console.log('Expert detail response:', response); // Debug log
      
      if (response.success && response.data) {
        setSelectedExpert(response.data);
        setShowExpertDetail(true);
      } else {
        setError(response.message || 'Không thể tải thông tin chi tiết chuyên gia');
      }
    } catch (err: any) {
      console.error('Error loading expert details:', err); // Debug log
      setError(err.message || 'Có lỗi xảy ra khi tải thông tin chuyên gia');
    }
  };

  const handleCreateSpecialty = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      const response = await expertsApi.createSpecialty(specialtyForm);
      if (response.success) {
        fetchSpecialties();
        setShowSpecialtyModal(false);
        setSpecialtyForm({ name: '', description: '' });
      }
    } catch (err: any) {
      setError(err.message || 'Failed to create specialty');
    }
  };

  const handleUpdateSpecialty = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!editingSpecialty) return;
    
    try {
      const response = await expertsApi.updateSpecialty(editingSpecialty.id, specialtyForm);
      if (response.success) {
        fetchSpecialties();
        setShowSpecialtyModal(false);
        setEditingSpecialty(null);
        setSpecialtyForm({ name: '', description: '' });
      }
    } catch (err: any) {
      setError(err.message || 'Failed to update specialty');
    }
  };

  const openSpecialtyModal = (specialty?: ExpertSpecialty) => {
    if (specialty) {
      setEditingSpecialty(specialty);
      setSpecialtyForm({
        name: specialty.name,
        description: specialty.description,
      });
    } else {
      setEditingSpecialty(null);
      setSpecialtyForm({ name: '', description: '' });
    }
    setShowSpecialtyModal(true);
  };

  if (isLoading && !experts) {
    return (
      <div className="flex h-64 items-center justify-center">
        <div className="h-8 w-8 animate-spin rounded-full border-b-2 border-primary-600"></div>
      </div>
    );
  }

  return (
    <div className="space-y-6">
      {/* Header */}
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold text-gray-900">Quản lý chuyên gia</h1>
          <p className="mt-1 text-sm text-gray-500">
            Quản lý các chuyên gia và chuyên khoa y tế
          </p>
        </div>
        <button
          onClick={() => openSpecialtyModal()}
          className="btn-primary flex items-center"
        >
          <PlusIcon className="mr-2 h-4 w-4" />
Thêm chuyên khoa
        </button>
      </div>

      {error && (
        <div className="rounded-md bg-red-50 p-4">
          <div className="text-sm text-red-700">{error}</div>
        </div>
      )}

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
              <label className="block text-sm font-medium text-gray-700">Chuyên khoa</label>
              <select
                value={searchParams.specialty || ''}
                onChange={(e) => setSearchParams(prev => ({ ...prev, specialty: e.target.value }))}
                className="input-field mt-1"
              >
                <option value="">Tất cả chuyên khoa</option>
                {specialties.map(specialty => (
                  <option key={specialty.id} value={specialty.name}>
                    {specialty.name}
                  </option>
                ))}
              </select>
            </div>
            <div>
              <label className="block text-sm font-medium text-gray-700">Trạng thái xác thực</label>
              <select
                value={searchParams.verificationStatus || ''}
                onChange={(e) => setSearchParams(prev => ({ ...prev, verificationStatus: e.target.value }))}
                className="input-field mt-1"
              >
                <option value="">Tất cả trạng thái</option>
                <option value="Pending">Chờ xác thực</option>
                <option value="Verified">Đã xác thực</option>
                <option value="Rejected">Đã từ chối</option>
              </select>
            </div>
            <div>
              <label className="block text-sm font-medium text-gray-700">Sắp xếp theo</label>
              <select
                value={searchParams.sortBy || 'newest'}
                onChange={(e) => setSearchParams(prev => ({ ...prev, sortBy: e.target.value as any }))}
                className="input-field mt-1"
              >
                <option value="newest">Mới nhất</option>
                <option value="oldest">Cũ nhất</option>
                <option value="name">Tên</option>
                <option value="rating">Đánh giá</option>
                <option value="topics">Chủ đề</option>
              </select>
            </div>
          </div>
        )}
      </div>

      {/* Specialties Overview */}
      <div className="card">
        <h3 className="text-lg font-medium text-gray-900 mb-4">Chuyên khoa ({specialties.length})</h3>
        <div className="grid grid-cols-1 gap-4 sm:grid-cols-2 lg:grid-cols-3">
          {specialties.map(specialty => (
            <div key={specialty.id} className="border border-gray-200 rounded-lg p-4 hover:border-primary-300 transition-colors">
              <div className="flex items-center justify-between">
                <div className="flex-1">
                  <h4 className="font-medium text-gray-900">{specialty.name}</h4>
                  <p className="text-sm text-gray-500 mt-1">{specialty.description}</p>
                  <div className="mt-2 flex items-center space-x-4 text-xs text-gray-400">
                    <span>{specialty.expertCount} chuyên gia</span>
                    <span className={`px-2 py-1 rounded-full ${specialty.isActive ? 'bg-green-100 text-green-800' : 'bg-gray-100 text-gray-800'}`}>
                      {specialty.isActive ? 'Hoạt động' : 'Không hoạt động'}
                    </span>
                  </div>
                </div>
                <button
                  onClick={() => openSpecialtyModal(specialty)}
                  className="text-gray-400 hover:text-gray-600"
                >
                  <EyeIcon className="h-4 w-4" />
                </button>
              </div>
            </div>
          ))}
        </div>
      </div>

      {/* Experts Table */}
      <div className="card">
        <div className="flex items-center justify-between mb-6">
          <h3 className="text-lg font-medium text-gray-900">
Chuyên gia ({experts?.totalItems || 0})
          </h3>
        </div>

        {isLoading ? (
          <div className="flex justify-center py-8">
            <div className="h-6 w-6 animate-spin rounded-full border-b-2 border-primary-600"></div>
          </div>
        ) : experts?.items.length === 0 ? (
          <div className="text-center py-8 text-gray-500">Không tìm thấy chuyên gia</div>
        ) : (
          <div className="overflow-x-auto">
            <table className="min-w-full divide-y divide-gray-200">
              <thead className="bg-gray-50">
                <tr>
                  <th className="table-header">Chuyên gia</th>
                  <th className="table-header">Chuyên khoa</th>
                  <th className="table-header">Trạng thái</th>
                  <th className="table-header">Đánh giá</th>
                  <th className="table-header">Hoạt động</th>
                  <th className="table-header">Thao tác</th>
                </tr>
              </thead>
              <tbody className="bg-white divide-y divide-gray-200">
                {experts?.items.map((expert) => (
                  <tr key={expert.id} className="hover:bg-gray-50">
                    <td className="table-cell">
                      <div className="flex items-center">
                        <div className="flex-shrink-0 h-10 w-10">
                          <div className="h-10 w-10 rounded-full bg-gray-200 flex items-center justify-center">
                            <UserIcon className="h-5 w-5 text-gray-600" />
                          </div>
                        </div>
                        <div className="ml-4">
                          <div className="text-sm font-medium text-gray-900">{expert.fullName}</div>
                          <div className="text-sm text-gray-500">@{expert.username}</div>
                          <div className="text-xs text-gray-400">{expert.email}</div>
                        </div>
                      </div>
                    </td>
                    <td className="table-cell">
                      <div className="flex flex-wrap gap-1">
                        {expert.specialty ? (
                          <span className="inline-flex items-center px-2 py-1 rounded-full text-xs bg-blue-100 text-blue-800">
                            {[...Array(5)].map((_, i) => (
                              <StarIcon
                                key={i}
                                className={`h-4 w-4 ${
                                  i < Math.floor(expert.rating) ? 'text-yellow-400' : 'text-gray-300'
                                }`}
                              />
                            ))}
                            <span className="ml-1 text-sm text-gray-500">({expert.reviewCount})</span>
                          </span>
                        ) : (
                          <span className="text-xs text-gray-400">Không có chuyên khoa</span>
                        )}
                      </div>
                    </td>
                    <td className="table-cell">
                      <div className="space-y-1">
                        <span className={`inline-flex items-center px-2 py-1 rounded-full text-xs ${expert.isVerified ? 'bg-green-100 text-green-800' : 'bg-yellow-100 text-yellow-800'}`}>
                          {expert.isVerified && <CheckBadgeIconSolid className="h-3 w-3 mr-1" />}
                          {expert.isVerified ? 'Đã xác thực' : 'Chờ xác thực'}
                        </span>
                        <div>
                          <span className={`inline-flex items-center px-2 py-1 rounded-full text-xs ${getStatusColor(expert.isActive)}`}>
                            {expert.isActive ? 'Hoạt động' : 'Không hoạt động'}
                          </span>
                        </div>
                      </div>
                    </td>
                    <td className="table-cell">
                      <div className="flex items-center">
                        <StarIcon className="h-4 w-4 text-yellow-400 mr-1" />
                        <span className="text-sm text-gray-900">
                          {expert.rating.toFixed(1)}
                        </span>
                        <span className="text-xs text-gray-500 ml-1">
                          ({expert.reviewCount})
                        </span>
                      </div>
                    </td>
                    <td className="table-cell">
                      <div className="text-sm text-gray-900">
                        {expert.topicCount} chủ đề, {expert.postCount} bài viết
                      </div>
                      <div className="text-xs text-gray-500">
Tham gia {formatRelativeTime(expert.createdAt)}
                      </div>
                    </td>
                    <td className="table-cell">
                      <div className="flex items-center space-x-2">
                        <button
                          onClick={() => viewExpertDetail(expert.id)}
                          className="text-indigo-600 hover:text-indigo-900 p-1 rounded hover:bg-indigo-50 transition-colors"
                          title="Xem chi tiết"
                        >
                          <EyeIcon className="h-4 w-4" />
                        </button>
                        {!expert.isVerified && (
                          <button
                            onClick={() => handleVerifyExpert(expert.id)}
                            className="text-green-600 hover:text-green-900 text-xs px-2 py-1 rounded hover:bg-green-50 transition-colors"
                            title="Xác thực chuyên gia"
                          >
                            Xác thực
                          </button>
                        )}
                        {expert.isVerified && (
                          <button
                            onClick={() => handleRejectExpert(expert.id)}
                            className="text-red-600 hover:text-red-900 text-xs px-2 py-1 rounded hover:bg-red-50 transition-colors"
                            title="Từ chối xác thực"
                          >
                            Từ chối
                          </button>
                        )}
                        <button
                          onClick={() => handleToggleStatus(expert.id)}
                          className={`text-xs px-2 py-1 rounded transition-colors ${expert.isActive ? 'text-red-600 hover:text-red-900 hover:bg-red-50' : 'text-green-600 hover:text-green-900 hover:bg-green-50'}`}
                          title={expert.isActive ? 'Tắt hoạt động' : 'Kích hoạt'}
                        >
                          {expert.isActive ? 'Tắt' : 'Kích hoạt'}
                        </button>
                      </div>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}

        {/* Pagination */}
        {experts && experts.totalPages > 1 && (
          <div className="flex items-center justify-between px-4 py-3 bg-white border-t border-gray-200 sm:px-6">
            <div className="flex justify-between flex-1 sm:hidden">
              <button
                onClick={() => setSearchParams(prev => ({ ...prev, page: prev.page - 1 }))}
                disabled={!experts.hasPrevious}
                className="btn-secondary disabled:opacity-50"
              >
                Trước
              </button>
              <button
                onClick={() => setSearchParams(prev => ({ ...prev, page: prev.page + 1 }))}
                disabled={!experts.hasNext}
                className="btn-secondary disabled:opacity-50"
              >
                Tiếp theo
              </button>
            </div>
            <div className="hidden sm:flex sm:flex-1 sm:items-center sm:justify-between">
              <div>
                <p className="text-sm text-gray-700">
                  Showing <span className="font-medium">{((experts.page - 1) * experts.pageSize) + 1}</span> to{' '}
                  <span className="font-medium">
                    {Math.min(experts.page * experts.pageSize, experts.totalItems)}
                  </span>{' '}
                  của <span className="font-medium">{experts.totalItems}</span> kết quả
                </p>
              </div>
              <div className="flex space-x-2">
                <button
                  onClick={() => setSearchParams(prev => ({ ...prev, page: prev.page - 1 }))}
                  disabled={!experts.hasPrevious}
                  className="btn-secondary disabled:opacity-50"
                >
                  Previous
                </button>
                <button
                  onClick={() => setSearchParams(prev => ({ ...prev, page: prev.page + 1 }))}
                  disabled={!experts.hasNext}
                  className="btn-secondary disabled:opacity-50"
                >
                  Next
                </button>
              </div>
            </div>
          </div>
        )}
      </div>

      {/* Expert Detail Modal */}
      <Modal
        isOpen={showExpertDetail}
        onClose={() => {
          setShowExpertDetail(false);
          setSelectedExpert(null);
        }}
        title="Chi tiết chuyên gia"
        size="xl"
      >
        {selectedExpert && (
          <div className="space-y-6">
            <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
              {/* Expert Info */}
              <div className="lg:col-span-2 space-y-6">
                <div className="flex items-start space-x-4">
                  <div className="flex-shrink-0 h-20 w-20">
                    <div className="h-20 w-20 rounded-full bg-gray-200 flex items-center justify-center">
                      <UserIcon className="h-10 w-10 text-gray-600" />
                    </div>
                  </div>
                  <div className="flex-1">
                    <h4 className="text-xl font-semibold text-gray-900">{selectedExpert.fullName}</h4>
                    <p className="text-gray-600">@{selectedExpert.username}</p>
                    <p className="text-gray-600">{selectedExpert.email}</p>
                    <div className="mt-2 flex items-center space-x-4">
                      <span className={`px-3 py-1 rounded-full text-sm ${selectedExpert.isVerified ? 'bg-green-100 text-green-800' : 'bg-yellow-100 text-yellow-800'}`}>
                        {selectedExpert.isVerified ? 'Đã xác thực' : 'Chờ xác thực'}
                      </span>
                      <span className={`px-3 py-1 rounded-full text-sm ${getStatusColor(selectedExpert.isActive)}`}>
                        {selectedExpert.isActive ? 'Hoạt động' : 'Không hoạt động'}
                      </span>
                    </div>
                  </div>
                </div>

                <div>
                  <h5 className="font-medium text-gray-900 mb-2">Tiểu sử</h5>
                  <p className="text-gray-700">{selectedExpert.bio || 'Chưa có tiểu sử'}</p>
                </div>

                <div>
                  <h5 className="font-medium text-gray-900 mb-2">Kinh nghiệm</h5>
                  <p className="text-gray-700">{selectedExpert.experience || 'Chưa có thông tin kinh nghiệm'}</p>
                </div>

                <div>
                  <h5 className="font-medium text-gray-900 mb-2">Học vấn</h5>
                  <p className="text-gray-700">{selectedExpert.education || 'Chưa có thông tin học vấn'}</p>
                </div>

                {/* Action Buttons */}
                <div className="flex space-x-3 pt-4 border-t border-gray-200">
                  {!selectedExpert.isVerified && (
                    <button
                      onClick={() => {
                        handleVerifyExpert(selectedExpert.id);
                        setShowExpertDetail(false);
                      }}
                      className="btn-success"
                    >
                      Xác thực chuyên gia
                    </button>
                  )}
                  {selectedExpert.isVerified && (
                    <button
                      onClick={() => {
                        handleRejectExpert(selectedExpert.id);
                        setShowExpertDetail(false);
                      }}
                      className="btn-danger"
                    >
                      Từ chối xác thực
                    </button>
                  )}
                  <button
                    onClick={() => {
                      handleToggleStatus(selectedExpert.id);
                      setShowExpertDetail(false);
                    }}
                    className={selectedExpert.isActive ? 'btn-warning' : 'btn-success'}
                  >
                    {selectedExpert.isActive ? 'Tắt hoạt động' : 'Kích hoạt'}
                  </button>
                </div>
              </div>

              {/* Stats and Activity */}
              <div className="space-y-6">
                <div className="bg-gray-50 p-4 rounded-lg">
                  <h5 className="font-medium text-gray-900 mb-3">Thống kê</h5>
                  <div className="space-y-2 text-sm">
                    <div className="flex justify-between">
                      <span>Đánh giá:</span>
                      <span className="flex items-center">
                        <StarIcon className="h-4 w-4 text-yellow-400 mr-1" />
                        {selectedExpert.rating.toFixed(1)} ({selectedExpert.reviewCount} đánh giá)
                      </span>
                    </div>
                    <div className="flex justify-between">
                      <span>Chủ đề:</span>
                      <span>{selectedExpert.topicCount}</span>
                    </div>
                    <div className="flex justify-between">
                      <span>Bài viết:</span>
                      <span>{selectedExpert.postCount}</span>
                    </div>
                    <div className="flex justify-between">
                      <span>Tham gia:</span>
                      <span>{formatRelativeTime(selectedExpert.createdAt)}</span>
                    </div>
                  </div>
                </div>

                <div>
                  <h5 className="font-medium text-gray-900 mb-2">Chuyên khoa</h5>
                  <div className="flex flex-wrap gap-2">
                    {selectedExpert.specialty ? (
                      <span className="inline-flex items-center px-2 py-1 rounded-full text-xs bg-blue-100 text-blue-800">
                        {selectedExpert.specialty}
                      </span>
                    ) : (
                      <span className="text-xs text-gray-400">Chưa có chuyên khoa</span>
                    )}
                  </div>
                </div>

                <div>
                  <h5 className="font-medium text-gray-900 mb-2">Chứng chỉ</h5>
                  <ul className="text-sm text-gray-700 space-y-1">
                    {selectedExpert.certifications && selectedExpert.certifications.length > 0 ? 
                      selectedExpert.certifications.map((cert, index) => (
                        <li key={index}>• {cert}</li>
                      )) :
                      <li className="text-gray-500">Chưa có chứng chỉ</li>
                    }
                  </ul>
                </div>

                <div>
                  <h5 className="font-medium text-gray-900 mb-2">Nơi làm việc</h5>
                  <ul className="text-sm text-gray-700 space-y-1">
                    {selectedExpert.workplaces && selectedExpert.workplaces.length > 0 ? 
                      selectedExpert.workplaces.map((workplace, index) => (
                        <li key={index}>• {workplace}</li>
                      )) :
                      <li className="text-gray-500">Chưa có thông tin nơi làm việc</li>
                    }
                  </ul>
                </div>
              </div>
            </div>
          </div>
        )}
      </Modal>

      {/* Specialty Modal */}
      {showSpecialtyModal && (
        <Modal
          isOpen={true}
          title={editingSpecialty ? 'Sửa chuyên khoa' : 'Thêm chuyên khoa mới'}
          onClose={() => setShowSpecialtyModal(false)}
          size="md"
        >
          <form onSubmit={editingSpecialty ? handleUpdateSpecialty : handleCreateSpecialty} className="space-y-6">
            <div>
              <label className="block text-sm font-medium text-gray-700">Tên *</label>
              <input
                type="text"
                required
                className="input-field mt-1"
                value={specialtyForm.name}
                onChange={(e) => setSpecialtyForm(prev => ({ ...prev, name: e.target.value }))}
                placeholder="Nhập tên chuyên khoa"
              />
            </div>
            
            <div>
              <label className="block text-sm font-medium text-gray-700">Mô tả *</label>
              <textarea
                required
                rows={3}
                className="input-field mt-1"
                value={specialtyForm.description}
                onChange={(e) => setSpecialtyForm(prev => ({ ...prev, description: e.target.value }))}
                placeholder="Nhập mô tả chuyên khoa"
              />
            </div>

            <div className="flex justify-end space-x-3 border-t border-gray-200 pt-4">
              <button
                type="button"
                onClick={() => setShowSpecialtyModal(false)}
                className="btn-outline"
              >
            Hủy
              </button>
              <button type="submit" className="btn-primary">
{editingSpecialty ? 'Cập nhật' : 'Tạo'}
              </button>
            </div>
          </form>
        </Modal>
      )}
    </div>
  );
};

export default ExpertsPage;
