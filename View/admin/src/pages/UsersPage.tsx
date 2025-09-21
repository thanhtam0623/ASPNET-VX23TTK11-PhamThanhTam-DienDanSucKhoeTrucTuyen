import React, { useState, useEffect } from 'react';
import {
  FunnelIcon,
  PlusIcon,
  PencilIcon,
  TrashIcon,
  KeyIcon,
  UserIcon,
} from '@heroicons/react/24/outline';
import { usersApi } from '../services/api';
import type { UserSummary, UserSearchRequest } from '../types';
import {
  formatRelativeTime,
  formatNumber,
  getRoleBadgeColor,
  getStatusText,
  usePageTitle,
} from '../utils';
import Table from '../components/UI/Table';
import Modal from '../components/UI/Modal';
import Badge from '../components/UI/Badge';

const UsersPage: React.FC = () => {
  usePageTitle('Quản lý người dùng');
  
  const [users, setUsers] = useState<UserSummary[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [selectedRole, setSelectedRole] = useState<string>('');
  const [selectedStatus, setSelectedStatus] = useState<string>('');
  const [currentPage, setCurrentPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const [showFilters, setShowFilters] = useState(false);
  const [selectedUser, setSelectedUser] = useState<UserSummary | null>(null);
  const [showUserModal, setShowUserModal] = useState(false);
  const [showCreateModal, setShowCreateModal] = useState(false);
  const [showEditModal, setShowEditModal] = useState(false);
  const [showResetPasswordModal, setShowResetPasswordModal] = useState(false);
  const [actionLoading, setActionLoading] = useState<number | null>(null);
  const [resetPasswordData, setResetPasswordData] = useState({
    userId: 0,
    newPassword: '',
    confirmPassword: ''
  });
  const [formData, setFormData] = useState({
    username: '',
    email: '',
    fullName: '',
    role: 'Member',
    password: ''
  });
  const [editFormData, setEditFormData] = useState({
    fullName: '',
    email: '',
    role: 'Member'
  });

  const pageSize = 20;

  useEffect(() => {
    fetchUsers();
  }, [currentPage, selectedRole, selectedStatus]);


  const fetchUsers = async () => {
    try {
      setIsLoading(true);
      setError(''); // Clear previous errors

      const params: UserSearchRequest = {
        role: selectedRole || undefined,
        status: selectedStatus || undefined,
        page: currentPage,
        pageSize,
      };

      console.log('Fetching users with params:', params); // Debug log

      const response = await usersApi.searchUsers(params);
      if (response.success) {
        setUsers(response.data.items);
        setTotalPages(response.data.totalPages);
      } else {
        setError(response.message || 'Failed to load users');
      }
    } catch (err: any) {
      console.error('Error fetching users:', err);
      setError(err.message || 'Failed to load users');
    } finally {
      setIsLoading(false);
    }
  };

  const handleToggleStatus = async (userId: number) => {
    try {
      setActionLoading(userId);
      await usersApi.toggleUserStatus(userId);
      await fetchUsers();
    } catch (err: any) {
      setError(err.message || 'Failed to toggle user status');
    } finally {
      setActionLoading(null);
    }
  };

  const handleResetPassword = (userId: number) => {
    setResetPasswordData({
      userId: userId,
      newPassword: '',
      confirmPassword: ''
    });
    setShowResetPasswordModal(true);
  };

  const handleSubmitResetPassword = async () => {
    if (resetPasswordData.newPassword !== resetPasswordData.confirmPassword) {
      setError('Passwords do not match');
      return;
    }
    
    if (resetPasswordData.newPassword.length < 6) {
      setError('Password must be at least 6 characters long');
      return;
    }

    try {
      setActionLoading(resetPasswordData.userId);
      await usersApi.resetUserPassword(resetPasswordData.userId, resetPasswordData.newPassword);
      alert('Password reset successfully');
      setShowResetPasswordModal(false);
      setResetPasswordData({ userId: 0, newPassword: '', confirmPassword: '' });
    } catch (err: any) {
      setError(err.message || 'Failed to reset password');
    } finally {
      setActionLoading(null);
    }
  };

  const handleDeleteUser = async (userId: number) => {
    if (!confirm('Bạn có chắc chắn muốn xóa người dùng này? Hành động này không thể hoàn tác.')) {
      return;
    }
    try {
      setActionLoading(userId);
      const response = await usersApi.deleteUser(userId);
      if (response.success) {
        await fetchUsers();
      } else {
        setError(response.message || 'Không thể xóa người dùng');
      }
    } catch (err: any) {
      // Extract error message from response
      const errorMessage = err.response?.data?.message || err.message || 'Không thể xóa người dùng';
      setError(errorMessage);
    } finally {
      setActionLoading(null);
    }
  };

  const columns = [
    {
      key: 'fullName' as keyof UserSummary,
      header: 'Người dùng',
      sortable: true,
      width: 'w-1/5',
      render: (_: any, user: UserSummary) => (
        <div className="flex items-center">
          <div className="flex h-10 w-10 items-center justify-center rounded-full bg-gray-100">
            <UserIcon className="h-5 w-5 text-gray-600" />
          </div>
          <div className="ml-4">
            <div className="text-sm font-medium text-gray-900">{user.fullName}</div>
            <div className="text-sm text-gray-500">@{user.username}</div>
          </div>
        </div>
      ),
    },
    {
      key: 'email' as keyof UserSummary,
      header: 'Email',
      sortable: true,
      width: 'w-1/6',
    },
    {
      key: 'role' as keyof UserSummary,
      header: 'Vai trò',
      width: 'w-1/6',
      render: (value: string) => (
        <Badge variant="secondary" className={getRoleBadgeColor(value)}>
          {value}
        </Badge>
      ),
    },
    {
      key: 'isActive' as keyof UserSummary,
      header: 'Trạng thái',
      width: 'w-1/6',
      render: (value: boolean) => (
        <Badge variant={value ? 'success' : 'danger'}>
          {getStatusText(value)}
        </Badge>
      ),
    },
    {
      key: 'topicCount' as keyof UserSummary,
      header: 'Chủ đề',
      width: 'w-16',
      render: (value: number) => formatNumber(value),
    },
    {
      key: 'postCount' as keyof UserSummary,
      header: 'Bài viết',
      width: 'w-20',
      render: (value: number) => formatNumber(value),
    },
    {
      key: 'createdAt' as keyof UserSummary,
      header: 'Tham gia',
      width: 'w-32',
      render: (value: string) => formatRelativeTime(value),
    },
    {
      key: 'id' as keyof UserSummary,
      header: 'Hành động',
      width: 'w-32',
      noWrap: false,
      render: (_: any, user: UserSummary) => (
        <div className="flex space-x-1 min-w-[120px]">
          <button
            onClick={() => {
              setSelectedUser(user);
              setShowUserModal(true);
            }}
            className="text-blue-600 hover:text-blue-900 p-1 rounded hover:bg-blue-50"
            title="Xem chi tiết"
          >
            <PencilIcon className="h-4 w-4" />
          </button>
          <button
            onClick={() => handleToggleStatus(user.id)}
            disabled={actionLoading === user.id}
            className={`p-1 rounded hover:bg-gray-50 ${
              user.isActive ? 'text-yellow-600 hover:text-yellow-900' : 'text-green-600 hover:text-green-900'
            }`}
            title={user.isActive ? 'Tắt hoạt động' : 'Kích hoạt'}
          >
            {actionLoading === user.id ? (
              <div className="h-4 w-4 animate-spin rounded-full border-b-2 border-current"></div>
            ) : (
              <UserIcon className="h-4 w-4" />
            )}
          </button>
          <button
            onClick={() => handleResetPassword(user.id)}
            disabled={actionLoading === user.id}
            className="text-blue-600 hover:text-blue-900 p-1 rounded hover:bg-blue-50"
            title="Đặt lại mật khẩu"
          >
            <KeyIcon className="h-4 w-4" />
          </button>
          <button
            onClick={() => handleDeleteUser(user.id)}
            disabled={actionLoading === user.id}
            className="text-red-600 hover:text-red-900 p-1 rounded hover:bg-red-50"
            title="Xóa người dùng"
          >
            {actionLoading === user.id ? (
              <div className="h-4 w-4 animate-spin rounded-full border-b-2 border-current"></div>
            ) : (
              <TrashIcon className="h-4 w-4" />
            )}
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
          <h1 className="text-2xl font-bold text-gray-900">Người dùng</h1>
          <p className="mt-1 text-sm text-gray-500">
            Quản lý tài khoản và quyền người dùng
          </p>
        </div>
        <button 
          onClick={() => setShowCreateModal(true)}
          className="btn-primary flex items-center"
        >
          <PlusIcon className="mr-2 h-4 w-4" />
Thêm người dùng
        </button>
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
              <label className="block text-sm font-medium text-gray-700">Vai trò</label>
              <select
                value={selectedRole}
                onChange={e => setSelectedRole(e.target.value)}
                className="input-field mt-1"
              >
                <option value="">Tất cả vai trò</option>
                <option value="Member">Thành viên</option>
                <option value="Doctor">Bác sĩ</option>
                <option value="Moderator">Người kiểm duyệt</option>
              </select>
            </div>
            <div>
              <label className="block text-sm font-medium text-gray-700">Trạng thái</label>
              <select
                value={selectedStatus}
                onChange={e => setSelectedStatus(e.target.value)}
                className="input-field mt-1"
              >
                <option value="">Tất cả trạng thái</option>
                <option value="true">Hoạt động</option>
                <option value="false">Không hoạt động</option>
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

      {/* Users Table */}
      <div className="card">
        <div className="table-container">
          <Table
            data={users}
            columns={columns}
            loading={isLoading}
            pagination={{
              currentPage,
              totalPages,
              onPageChange: setCurrentPage,
            }}
          />
        </div>
      </div>

      {/* Create User Modal */}
      <Modal
        isOpen={showCreateModal}
        onClose={() => setShowCreateModal(false)}
        title="Thêm người dùng mới"
        size="lg"
      >
        <form onSubmit={async (e) => {
          e.preventDefault();
          try {
            const result = await usersApi.createUser({
              username: formData.username,
              email: formData.email,
              fullName: formData.fullName,
              password: formData.password,
              role: formData.role
            });

            if (result.success) {
              alert('User created successfully');
              setShowCreateModal(false);
              setFormData({ username: '', email: '', fullName: '', role: 'Member', password: '' });
              // Refresh the users list
              await fetchUsers();
            } else {
              alert(result.message || 'Failed to create user');
            }
          } catch (error) {
            console.error('Error creating user:', error);
            alert('Failed to create user');
          }
        }} className="space-y-6">
          <div className="grid grid-cols-1 gap-6 sm:grid-cols-2">
            <div>
              <label className="block text-sm font-medium text-gray-700">Tên đăng nhập</label>
              <input
                type="text"
                value={formData.username}
                onChange={(e) => setFormData({...formData, username: e.target.value})}
                className="input-field mt-1"
                required
              />
            </div>
            <div>
              <label className="block text-sm font-medium text-gray-700">Email</label>
              <input
                type="email"
                value={formData.email}
                onChange={(e) => setFormData({...formData, email: e.target.value})}
                className="input-field mt-1"
                required
              />
            </div>
            <div>
              <label className="block text-sm font-medium text-gray-700">Họ và tên</label>
              <input
                type="text"
                value={formData.fullName}
                onChange={(e) => setFormData({...formData, fullName: e.target.value})}
                className="input-field mt-1"
                required
              />
            </div>
            <div>
              <label className="block text-sm font-medium text-gray-700">Vai trò</label>
              <select
                value={formData.role}
                onChange={(e) => setFormData({...formData, role: e.target.value})}
                className="input-field mt-1"
              >
                <option value="Member">Thành viên</option>
                <option value="Doctor">Bác sĩ</option>
                <option value="Moderator">Người kiểm duyệt</option>
              </select>
            </div>
            <div className="sm:col-span-2">
              <label className="block text-sm font-medium text-gray-700">Mật khẩu</label>
              <input
                type="password"
                value={formData.password}
                onChange={(e) => setFormData({...formData, password: e.target.value})}
                className="input-field mt-1"
                required
              />
            </div>
          </div>
          <div className="flex justify-end space-x-3 border-t border-gray-200 pt-4">
            <button
              type="button"
              onClick={() => {
                setShowCreateModal(false);
                setFormData({ username: '', email: '', fullName: '', role: 'Member', password: '' });
              }}
              className="btn-outline"
            >
              Hủy
            </button>
            <button
              type="submit"
              className="btn-primary"
            >
              Tạo người dùng
            </button>
          </div>
        </form>
      </Modal>

      {/* User Detail Modal */}
      {selectedUser && (
        <Modal
          isOpen={showUserModal}
          onClose={() => {
            setShowUserModal(false);
            setSelectedUser(null);
          }}
          title="User Details"
          size="lg"
        >
          <div className="space-y-6">
            <div className="flex items-center space-x-4">
              <div className="flex h-16 w-16 items-center justify-center rounded-full bg-gray-100">
                <UserIcon className="h-8 w-8 text-gray-600" />
              </div>
              <div>
                <h3 className="text-lg font-medium text-gray-900">
                  {selectedUser.fullName}
                </h3>
                <p className="text-sm text-gray-500">@{selectedUser.username}</p>
                <Badge className={getRoleBadgeColor(selectedUser.role)}>
                  {selectedUser.role}
                </Badge>
              </div>
            </div>

            <div className="grid grid-cols-2 gap-4">
              <div>
                <dt className="text-sm font-medium text-gray-500">Email</dt>
                <dd className="mt-1 text-sm text-gray-900">{selectedUser.email}</dd>
              </div>
              <div>
                <dt className="text-sm font-medium text-gray-500">Status</dt>
                <dd className="mt-1">
                  <Badge variant={selectedUser.isActive ? 'success' : 'danger'}>
                    {getStatusText(selectedUser.isActive)}
                  </Badge>
                </dd>
              </div>
              <div>
                <dt className="text-sm font-medium text-gray-500">Joined</dt>
                <dd className="mt-1 text-sm text-gray-900">
                  {formatRelativeTime(selectedUser.createdAt)}
                </dd>
              </div>
              <div>
                <dt className="text-sm font-medium text-gray-500">Last Login</dt>
                <dd className="mt-1 text-sm text-gray-900">
                  {selectedUser.lastLoginAt
                    ? formatRelativeTime(selectedUser.lastLoginAt)
                    : 'Never'}
                </dd>
              </div>
              <div>
                <dt className="text-sm font-medium text-gray-500">Topics</dt>
                <dd className="mt-1 text-sm text-gray-900">
                  {formatNumber(selectedUser.topicCount)}
                </dd>
              </div>
              <div>
                <dt className="text-sm font-medium text-gray-500">Posts</dt>
                <dd className="mt-1 text-sm text-gray-900">
                  {formatNumber(selectedUser.postCount)}
                </dd>
              </div>
            </div>

            <div className="flex justify-end space-x-3 border-t border-gray-200 pt-4">
              <button
                onClick={() => {
                  setShowUserModal(false);
                  setSelectedUser(null);
                }}
                className="btn-outline"
              >
                Close
              </button>
              <button
                onClick={() => {
                  setEditFormData({
                    fullName: selectedUser.fullName,
                    email: selectedUser.email,
                    role: selectedUser.role
                  });
                  setShowUserModal(false);
                  setShowEditModal(true);
                }}
                className="btn-primary"
              >
                Edit User
              </button>
            </div>
          </div>
        </Modal>
      )}

      {/* Edit User Modal */}
      <Modal
        isOpen={showEditModal}
        onClose={() => {
          setShowEditModal(false);
          setEditFormData({ fullName: '', email: '', role: 'Member' });
        }}
        title="Edit User"
        size="lg"
      >
        <form onSubmit={async (e) => {
          e.preventDefault();
          try {
            if (selectedUser) {
              await usersApi.updateUser(selectedUser.id, editFormData);
              setShowEditModal(false);
              setEditFormData({ fullName: '', email: '', role: 'Member' });
              await fetchUsers();
            }
          } catch (err: any) {
            setError(err.message || 'Failed to update user');
          }
        }} className="space-y-6">
          <div className="grid grid-cols-1 gap-6 sm:grid-cols-2">
            <div>
              <label className="block text-sm font-medium text-gray-700">Full Name</label>
              <input
                type="text"
                value={editFormData.fullName}
                onChange={(e) => setEditFormData({...editFormData, fullName: e.target.value})}
                className="input-field mt-1"
                required
              />
            </div>
            <div>
              <label className="block text-sm font-medium text-gray-700">Email</label>
              <input
                type="email"
                value={editFormData.email}
                onChange={(e) => setEditFormData({...editFormData, email: e.target.value})}
                className="input-field mt-1"
                required
              />
            </div>
            <div>
              <label className="block text-sm font-medium text-gray-700">Role</label>
              <select
                value={editFormData.role}
                onChange={(e) => setEditFormData({...editFormData, role: e.target.value})}
                className="input-field mt-1"
              >
                <option value="Member">Member</option>
                <option value="Doctor">Doctor</option>
                <option value="Moderator">Moderator</option>
              </select>
            </div>
          </div>
          <div className="flex justify-end space-x-3 border-t border-gray-200 pt-4">
            <button
              type="button"
              onClick={() => {
                setShowEditModal(false);
                setEditFormData({ fullName: '', email: '', role: 'Member' });
              }}
              className="btn-outline"
            >
              Cancel
            </button>
            <button
              type="submit"
              className="btn-primary"
            >
              Update User
            </button>
          </div>
        </form>
      </Modal>

      {/* Reset Password Modal */}
      <Modal
        isOpen={showResetPasswordModal}
        onClose={() => {
          setShowResetPasswordModal(false);
          setResetPasswordData({ userId: 0, newPassword: '', confirmPassword: '' });
        }}
        title="Reset User Password"
        size="md"
      >
        <form onSubmit={async (e) => {
          e.preventDefault();
          await handleSubmitResetPassword();
        }} className="space-y-6">
          <div>
            <label className="block text-sm font-medium text-gray-700">New Password</label>
            <input
              type="password"
              value={resetPasswordData.newPassword}
              onChange={(e) => setResetPasswordData({...resetPasswordData, newPassword: e.target.value})}
              className="input-field mt-1"
              required
              minLength={6}
              placeholder="Enter new password (min 6 characters)"
            />
          </div>
          <div>
            <label className="block text-sm font-medium text-gray-700">Confirm Password</label>
            <input
              type="password"
              value={resetPasswordData.confirmPassword}
              onChange={(e) => setResetPasswordData({...resetPasswordData, confirmPassword: e.target.value})}
              className="input-field mt-1"
              required
              minLength={6}
              placeholder="Confirm new password"
            />
          </div>
          <div className="flex justify-end space-x-3 border-t border-gray-200 pt-4">
            <button
              type="button"
              onClick={() => {
                setShowResetPasswordModal(false);
                setResetPasswordData({ userId: 0, newPassword: '', confirmPassword: '' });
              }}
              className="btn-outline"
            >
              Cancel
            </button>
            <button
              type="submit"
              disabled={actionLoading === resetPasswordData.userId}
              className="btn-primary"
            >
              {actionLoading === resetPasswordData.userId ? 'Resetting...' : 'Reset Password'}
            </button>
          </div>
        </form>
      </Modal>
    </div>
  );
};

export default UsersPage;
