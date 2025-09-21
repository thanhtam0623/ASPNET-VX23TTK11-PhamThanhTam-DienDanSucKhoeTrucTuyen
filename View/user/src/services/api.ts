import axios from 'axios';
import type {
  ApiResponse,
  PaginatedResponse,
  Category,
  Topic,
  TopicSummary,
  Post,
  User,
  HomePageData,
  CategoryPageData,
  UpdateProfileRequest,
  ChangePasswordRequest,
  RegisterRequest,
  LoginRequest,
  AuthResponse,
  CreateTopicRequest,
  CreatePostRequest,
  Expert,
  ExpertSummary,
  ExpertStats,
  ExpertSearchRequest,
  TopicsListRequest,
} from '../types';

// Create axios instance
const api = axios.create({
  baseURL: 'http://localhost:5002/api',
  headers: {
    'Content-Type': 'application/json',
  },
});

// Add auth token to requests
api.interceptors.request.use(config => {
  const token = localStorage.getItem('token');
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
      localStorage.removeItem('token');
      localStorage.removeItem('user');
      window.location.href = '/login';
    }
    return Promise.reject(error);
  }
);

// Auth API
export const authApi = {
  register: (data: RegisterRequest): Promise<ApiResponse<AuthResponse>> =>
    api.post('/user/auth/register', data).then(res => res.data),

  login: (data: LoginRequest): Promise<ApiResponse<AuthResponse>> =>
    api.post('/user/auth/login', data).then(res => res.data),

  getProfile: (): Promise<ApiResponse<User>> =>
    api.get('/user/auth/profile').then(res => res.data),

  changePassword: (data: ChangePasswordRequest): Promise<ApiResponse<void>> =>
    api.post('/user/auth/change-password', data).then(res => res.data),

  forgotPassword: (email: string): Promise<ApiResponse<void>> =>
    api.post('/user/auth/forgot-password', { email }).then(res => res.data),

  resetPassword: (
    token: string,
    password: string
  ): Promise<ApiResponse<void>> =>
    api
      .post('/user/auth/reset-password', { token, password })
      .then(res => res.data),
};

// Home API
export const homeApi = {
  getHomePage: (): Promise<ApiResponse<HomePageData>> =>
    api.get('/user/home').then(res => res.data),
};

// Category API
export const categoryApi = {
  getAllCategories: (): Promise<ApiResponse<Category[]>> =>
    api.get('/user/categories').then(res => res.data),

  getCategoryBySlug: (slug: string): Promise<ApiResponse<Category>> =>
    api.get(`/user/categories/${slug}`).then(res => res.data),

  getCategoryTopics: (
    slug: string,
    data: any
  ): Promise<ApiResponse<CategoryPageData>> =>
    api.post(`/user/categories/${slug}/topics`, data).then(res => res.data),
};

// Topic API
export const topicApi = {
  getTopicBySlug: (slug: string): Promise<ApiResponse<Topic>> =>
    api.get(`/user/topics/${slug}`).then(res => res.data),
    
  getTopicById: (id: number): Promise<ApiResponse<Topic>> =>
    api.get(`/user/topics/${id}`).then(res => res.data),

  createTopic: (data: CreateTopicRequest): Promise<ApiResponse<Topic>> =>
    api.post('/user/topics', data).then(res => res.data),

  updateTopic: (
    id: number,
    data: Partial<CreateTopicRequest>
  ): Promise<ApiResponse<Topic>> =>
    api.put(`/user/topics/${id}`, data).then(res => res.data),

  deleteTopic: (id: number): Promise<ApiResponse<void>> =>
    api.delete(`/user/topics/${id}`).then(res => res.data),

  likeTopic: (id: number): Promise<ApiResponse<void>> =>
    api.post(`/user/topics/${id}/like`).then(res => res.data),

  unlikeTopic: (id: number): Promise<ApiResponse<void>> =>
    api.delete(`/user/topics/${id}/like`).then(res => res.data),

  getTopicPosts: (
    topicId: number,
    page = 1,
    pageSize = 20
  ): Promise<ApiResponse<PaginatedResponse<Post>>> =>
    api
      .get(`/user/topics/${topicId}/posts?page=${page}&pageSize=${pageSize}`)
      .then(res => res.data),

  createPost: (
    topicId: number,
    data: CreatePostRequest
  ): Promise<ApiResponse<Post>> =>
    api.post(`/user/topics/${topicId}/posts`, data).then(res => res.data),

  updatePost: (
    postId: number,
    data: Partial<CreatePostRequest>
  ): Promise<ApiResponse<Post>> =>
    api.put(`/user/posts/${postId}`, data).then(res => res.data),

  deletePost: (postId: number): Promise<ApiResponse<void>> =>
    api.delete(`/user/posts/${postId}`).then(res => res.data),

  likePost: (postId: number): Promise<ApiResponse<void>> =>
    api.post(`/user/posts/${postId}/like`).then(res => res.data),

  unlikePost: (postId: number): Promise<ApiResponse<void>> =>
    api.delete(`/user/posts/${postId}/like`).then(res => res.data),

  markPostAsAnswer: (postId: number): Promise<ApiResponse<void>> =>
    api.post(`/user/posts/${postId}/mark-answer`).then(res => res.data),

  reportTopic: (topicId: number, reason: string): Promise<ApiResponse<void>> =>
    api
      .post(`/user/topics/${topicId}/report`, { reason })
      .then(res => res.data),

  reportPost: (postId: number, reason: string): Promise<ApiResponse<void>> =>
    api.post(`/user/posts/${postId}/report`, { reason }).then(res => res.data),

  // Get topics list with filters
  getTopics: (params?: Partial<TopicsListRequest>): Promise<ApiResponse<PaginatedResponse<TopicSummary>>> =>
    api.get('/user/topics', { params }).then(res => res.data),

  // Use the correct search endpoint we implemented
  searchTopics: (params?: Partial<TopicsListRequest>): Promise<ApiResponse<PaginatedResponse<TopicSummary>>> =>
    api.get('/user/search', { params }).then(res => res.data),
};

// Profile API
export const profileApi = {
  getProfile: (): Promise<ApiResponse<User>> =>
    api.get('/user/profile').then(res => res.data),

  getPublicProfile: (username: string): Promise<ApiResponse<User>> =>
    api.get(`/user/profile/${username}`).then(res => res.data),

  getPublicUserTopics: (
    username: string,
    page = 1,
    pageSize = 20
  ): Promise<ApiResponse<PaginatedResponse<TopicSummary>>> =>
    api
      .get(`/user/profile/${username}/topics?page=${page}&pageSize=${pageSize}`)
      .then(res => res.data),

  updateProfile: (data: UpdateProfileRequest): Promise<ApiResponse<User>> =>
    api.put('/user/profile', data).then(res => res.data),

  getUserTopics: (
    page = 1,
    pageSize = 20
  ): Promise<ApiResponse<PaginatedResponse<TopicSummary>>> =>
    api
      .get(`/user/profile/topics?page=${page}&pageSize=${pageSize}`)
      .then(res => res.data),

  getUserPosts: (
    page = 1,
    pageSize = 20
  ): Promise<ApiResponse<PaginatedResponse<Post>>> =>
    api
      .get(`/user/profile/posts?page=${page}&pageSize=${pageSize}`)
      .then(res => res.data),

  getUserLikes: (
    page = 1,
    pageSize = 20
  ): Promise<ApiResponse<PaginatedResponse<TopicSummary>>> =>
    api
      .get(`/user/profile/likes?page=${page}&pageSize=${pageSize}`)
      .then(res => res.data),
};

// Expert API
export const expertApi = {
  getExperts: (params?: Partial<ExpertSearchRequest>): Promise<ApiResponse<PaginatedResponse<ExpertSummary>>> =>
    api.get('/user/experts', { params }).then(res => res.data),

  getSpecialties: (): Promise<ApiResponse<{ id: number; name: string; }[]>> =>
    api.get('/user/experts/specialties').then(res => res.data),

  getExpertByUsername: (username: string): Promise<ApiResponse<Expert>> =>
    api.get(`/user/experts/${username}`).then(res => res.data),

  getExpertSpecialties: (): Promise<ApiResponse<string[]>> =>
    api.get('/user/experts/specialties').then(res => res.data),

  getExpertTopics: (
    expertId: number,
    page = 1,
    pageSize = 20
  ): Promise<ApiResponse<PaginatedResponse<TopicSummary>>> =>
    api
      .get(`/user/experts/${expertId}/topics?page=${page}&pageSize=${pageSize}`)
      .then(res => res.data),

  getExpertStats: (expertId: number): Promise<ApiResponse<ExpertStats>> =>
    api.get(`/user/experts/${expertId}/stats`).then(res => res.data),
};

export default api;
