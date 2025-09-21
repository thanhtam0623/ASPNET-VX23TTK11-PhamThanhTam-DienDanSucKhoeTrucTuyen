import axios from 'axios';
import type {
  ApiResponse,
  ChangePasswordRequest,
  DashboardData,
  UserSearchRequest,
  UserSummary,
  PaginatedResponse,
  CreateUserRequest,
  UpdateUserRequest,
  UserDetailResponse,
  Category,
  TopicSearchRequest,
  UpdateTopicRequest,
  Report,
  AdminLoginRequest,
  AdminAuthResponse,
  AdminUser,
  UpdateReportStatusRequest,
  ExpertSummary,
  ExpertDetail,
  ExpertSpecialty,
  ExpertSearchRequest,
  UpdateExpertRequest,
  CreateExpertSpecialtyRequest,
  UpdateExpertSpecialtyRequest,
  ExpertReview,
  GlobalSearchRequest,
  SearchResults,
  SearchSuggestion,
  CreateCategoryRequest,
  UpdateCategoryRequest,
} from '../types';

// Create axios instance for admin
const api = axios.create({
  baseURL: 'http://localhost:5002/api/admin',
  headers: {
    'Content-Type': 'application/json',
  },
});

// Add auth token to requests
api.interceptors.request.use(config => {
  const token = localStorage.getItem('adminToken');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

// Handle auth errors
api.interceptors.response.use(
  response => response,
  error => {
    if (error.response?.status === 401) {
      localStorage.removeItem('adminToken');
      localStorage.removeItem('adminUser');
      window.location.href = '/login';
    }
    return Promise.reject(error);
  }
);

// Auth API
export const authApi = {
  login: (data: AdminLoginRequest): Promise<ApiResponse<AdminAuthResponse>> =>
    api.post('/auth/login', data).then(res => res.data),

  getProfile: (): Promise<ApiResponse<AdminUser>> =>
    api.get('/auth/profile').then(res => res.data),

  updateProfile: (data: AdminUser): Promise<ApiResponse<AdminUser>> =>
    api.put('/auth/profile', data).then(res => res.data),

  changePassword: (data: ChangePasswordRequest): Promise<ApiResponse<void>> =>
    api.post('/auth/change-password', data).then(res => res.data),
};

// Dashboard API
export const dashboardApi = {
  getDashboard: (): Promise<ApiResponse<DashboardData>> =>
    api.get('/dashboard').then(res => res.data),
};

// Users API
export const usersApi = {
  getUsers: (
    params: UserSearchRequest
  ): Promise<ApiResponse<PaginatedResponse<UserSummary>>> =>
    api.post('/users/search', params).then(res => res.data),

  searchUsers: (
    params: UserSearchRequest
  ): Promise<ApiResponse<PaginatedResponse<UserSummary>>> =>
    api.post('/users/search', params).then(res => res.data),

  createUser: (data: CreateUserRequest): Promise<ApiResponse<UserSummary>> =>
    api.post('/users', data).then(res => res.data),

  getUserById: (id: number): Promise<ApiResponse<UserDetailResponse>> =>
    api.get(`/users/${id}`).then(res => res.data),

  updateUser: (
    id: number,
    data: UpdateUserRequest
  ): Promise<ApiResponse<UserSummary>> =>
    api.put(`/users/${id}`, data).then(res => res.data),

  toggleUserStatus: (id: number): Promise<ApiResponse<void>> =>
    api.post(`/users/${id}/toggle-status`).then(res => res.data),

  resetUserPassword: (id: number, newPassword: string): Promise<ApiResponse<void>> =>
    api.post(`/users/${id}/reset-password`, { newPassword }).then(res => res.data),

  deleteUser: (id: number): Promise<ApiResponse<void>> =>
    api.delete(`/users/${id}`).then(res => res.data),

  sendVerificationEmail: (id: number): Promise<ApiResponse<void>> =>
    api.post(`/users/${id}/send-verification`).then(res => res.data),
};

// Categories API
export const categoriesApi = {
  getCategories: (): Promise<ApiResponse<Category[]>> =>
    api.get('/categories').then(res => res.data),

  getCategoryById: (id: number): Promise<ApiResponse<Category>> =>
    api.get(`/categories/${id}`).then(res => res.data),

  createCategory: (data: CreateCategoryRequest): Promise<ApiResponse<Category>> =>
    api.post('/categories', data).then(res => res.data),

  updateCategory: (
    id: number,
    data: UpdateCategoryRequest
  ): Promise<ApiResponse<Category>> =>
    api.put(`/categories/${id}`, data).then(res => res.data),

  deleteCategory: (id: number): Promise<ApiResponse<void>> =>
    api.delete(`/categories/${id}`).then(res => res.data),

  toggleCategoryStatus: (id: number): Promise<ApiResponse<void>> =>
    api.post(`/categories/${id}/toggle-status`).then(res => res.data),

  reorderCategories: (
    categoryIds: number[]
  ): Promise<ApiResponse<void>> =>
    api.post('/categories/reorder', { categoryIds }).then(res => res.data),
};

// Topics API
export const topicsApi = {
  getTopics: (
    params: TopicSearchRequest
  ): Promise<ApiResponse<PaginatedResponse<any>>> =>
    api.post('/topics/search', params).then(res => res.data),

  getTopicById: (id: number): Promise<ApiResponse<any>> =>
    api.get(`/topics/${id}`).then(res => res.data),

  updateTopic: (
    id: number,
    data: UpdateTopicRequest
  ): Promise<ApiResponse<any>> =>
    api.put(`/topics/${id}`, data).then(res => res.data),

  deleteTopic: (id: number): Promise<ApiResponse<void>> =>
    api.delete(`/topics/${id}`).then(res => res.data),

  togglePin: (id: number): Promise<ApiResponse<void>> =>
    api.post(`/topics/${id}/toggle-pin`).then(res => res.data),

  getTopicPosts: (
    topicId: number,
    page = 1,
    pageSize = 20
  ): Promise<ApiResponse<PaginatedResponse<any>>> =>
    api
      .get(`/topics/${topicId}/posts?page=${page}&pageSize=${pageSize}`)
      .then(res => res.data),

  deletePost: (postId: number): Promise<ApiResponse<void>> =>
    api.delete(`/posts/${postId}`).then(res => res.data),

  markPostAsAnswer: (postId: number): Promise<ApiResponse<void>> =>
    api.post(`/posts/${postId}/mark-answer`).then(res => res.data),
};


// Experts API
export const expertsApi = {
  getExperts: (
    params: ExpertSearchRequest
  ): Promise<ApiResponse<PaginatedResponse<ExpertSummary>>> =>
    api.post('/experts/search', params).then(res => res.data),

  getExpertById: (id: number): Promise<ApiResponse<ExpertDetail>> =>
    api.get(`/experts/${id}`).then(res => res.data),

  updateExpert: (
    id: number,
    data: UpdateExpertRequest
  ): Promise<ApiResponse<ExpertSummary>> =>
    api.put(`/experts/${id}`, data).then(res => res.data),

  verifyExpert: (id: number, isVerified: boolean = true): Promise<ApiResponse<void>> =>
    api.put(`/experts/${id}/verify`, { isVerified }).then(res => res.data),

  rejectExpert: (id: number): Promise<ApiResponse<void>> =>
    api.put(`/experts/${id}/verify`, { isVerified: false }).then(res => res.data),

  toggleExpertStatus: (id: number): Promise<ApiResponse<void>> =>
    api.put(`/experts/${id}/toggle-status`).then(res => res.data),

  getSpecialties: (): Promise<ApiResponse<ExpertSpecialty[]>> =>
    api.get('/experts/specialties').then(res => res.data),

  createSpecialty: (data: CreateExpertSpecialtyRequest): Promise<ApiResponse<ExpertSpecialty>> =>
    api.post('/experts/specialties', data).then(res => res.data),

  updateSpecialty: (
    id: number,
    data: UpdateExpertSpecialtyRequest
  ): Promise<ApiResponse<ExpertSpecialty>> =>
    api.put(`/experts/specialties/${id}`, data).then(res => res.data),

  deleteSpecialty: (id: number): Promise<ApiResponse<void>> =>
    api.delete(`/experts/specialties/${id}`).then(res => res.data),

  getExpertReviews: (
    expertId: number,
    page = 1,
    pageSize = 20
  ): Promise<ApiResponse<PaginatedResponse<ExpertReview>>> =>
    api
      .get(`/experts/${expertId}/reviews?page=${page}&pageSize=${pageSize}`)
      .then(res => res.data),

  deleteReview: (reviewId: number): Promise<ApiResponse<void>> =>
    api.delete(`/experts/reviews/${reviewId}`).then(res => res.data),
};

// Search API
export const searchApi = {
  globalSearch: (params: GlobalSearchRequest): Promise<ApiResponse<SearchResults>> =>
    api.get(`/search?query=${params.query}&type=${params.type || 'all'}&page=${params.page || 1}&pageSize=${params.pageSize || 10}`).then(res => res.data),

  searchUsers: (query: string, page = 1, pageSize = 10): Promise<ApiResponse<PaginatedResponse<UserSummary>>> =>
    api.get(`/search/users?query=${query}&page=${page}&pageSize=${pageSize}`).then(res => res.data),

  searchTopics: (query: string, page = 1, pageSize = 10): Promise<ApiResponse<PaginatedResponse<any>>> =>
    api.get(`/search/topics?query=${query}&page=${page}&pageSize=${pageSize}`).then(res => res.data),

  searchExperts: (query: string, page = 1, pageSize = 10): Promise<ApiResponse<PaginatedResponse<ExpertSummary>>> =>
    api.get(`/search/experts?query=${query}&page=${page}&pageSize=${pageSize}`).then(res => res.data),

  searchCategories: (query: string, page = 1, pageSize = 10): Promise<ApiResponse<PaginatedResponse<Category>>> =>
    api.get(`/search/categories?query=${query}&page=${page}&pageSize=${pageSize}`).then(res => res.data),

  getSearchSuggestions: (query: string): Promise<ApiResponse<SearchSuggestion[]>> =>
    api.get(`/search/suggestions?query=${query}`).then(res => res.data),
};

// Reports API
export const reportsApi = {
  getReports: (
    page = 1,
    pageSize = 20,
    status?: string
  ): Promise<ApiResponse<PaginatedResponse<Report>>> =>
    api
      .get(`/topics/reports?page=${page}&pageSize=${pageSize}${status ? `&status=${status}` : ''}`)
      .then(res => res.data),

  updateReportStatus: (
    reportId: number,
    data: UpdateReportStatusRequest
  ): Promise<ApiResponse<void>> =>
    api.put(`/topics/reports/${reportId}/status`, data).then(res => res.data),
};

export default api;
