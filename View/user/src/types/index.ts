// API Response Types
export interface ApiResponse<T> {
  success: boolean;
  message: string;
  data: T;
  errors: any;
}

export interface PaginatedResponse<T> {
  page: number;
  pageSize: number;
  totalItems: number;
  totalPages: number;
  hasNext: boolean;
  hasPrevious: boolean;
  items: T[];
}

// User Types
export interface User {
  id: number;
  username: string;
  email: string;
  fullName: string;
  avatar?: string;
  bio?: string;
  role: 'Member' | 'Doctor' | 'Moderator';
  isActive: boolean;
  isEmailVerified: boolean;
  showEmail: boolean;
  showBio: boolean;
  createdAt: string;
  topicCount: number;
  postCount: number;
  likeCount: number;
}

export interface UserSummary {
  id: number;
  username: string;
  fullName: string;
  avatar?: string;
  role: string;
  createdAt: string;
  topicCount: number;
  postCount: number;
}

// Auth Types
export interface LoginRequest {
  username: string;
  password: string;
}

export interface RegisterRequest {
  username: string;
  email: string;
  password: string;
  fullName: string;
}

export interface AuthResponse {
  token: string;
  user: User;
}

// Category Types
export interface Category {
  id: number;
  name: string;
  slug: string;
  description: string;
  icon?: string;
  color?: string;
  topicCount: number;
  postCount: number;
  lastActivityAt?: string;
  lastTopicTitle?: string;
  lastAuthorName?: string;
}

// Tag Types
export interface Tag {
  id: number;
  name: string;
  slug: string;
  color?: string;
  topicCount: number;
}

// Topic Types
export interface Topic {
  id: number;
  title: string;
  slug: string;
  content: string;
  category: Category;
  author: UserSummary;
  isLocked: boolean;
  hasAnswer: boolean;
  viewCount: number;
  likeCount: number;
  postCount: number;
  createdAt: string;
  updatedAt: string;
  lastActivityAt?: string;
  tags: Tag[];
  isLikedByCurrentUser: boolean;
  canEdit: boolean;
  canDelete: boolean;
  isPinned?: boolean;
}

export interface TopicSummary {
  id: number;
  title: string;
  slug: string;
  categoryName: string;
  categorySlug: string;
  authorName: string;
  authorAvatar?: string;
  authorRole: string;
  isPinned: boolean;
  isLocked: boolean;
  hasAnswer: boolean;
  viewCount: number;
  likeCount: number;
  postCount: number;
  createdAt: string;
  updatedAt: string;
  lastActivityAt?: string;
  tags: string[];
}

export interface CreateTopicRequest {
  title: string;
  content: string;
  categoryId: number;
  tags: string[];
}

// Post Types
export interface Post {
  id: number;
  content: string;
  author: UserSummary;
  isAnswer: boolean;
  likeCount: number;
  createdAt: string;
  updatedAt: string;
  replies: Post[];
  isLikedByCurrentUser: boolean;
  canEdit: boolean;
  canDelete: boolean;
  canMarkAsAnswer: boolean;
}

export interface CreatePostRequest {
  content: string;
  parentPostId?: number;
}

// Search Types
export interface SearchRequest {
  query?: string;
  categoryId?: number;
  tagIds?: number[];
  hasAnswer?: boolean;
  authorId?: number;
  sortBy?: string;
  sortOrder?: string;
  page: number;
  pageSize: number;
}

// Homepage Types
export interface HomePageData {
  categories: Category[];
  pinnedTopics: TopicSummary[];
  latestTopics: TopicSummary[];
  popularTags: Tag[];
  siteStats: {
    totalTopics: number;
    totalPosts: number;
    totalUsers: number;
    onlineUsers: number;
  };
}

export interface CategoryPageData {
  Category: Category;
  Topics: TopicSummary[];
  TotalTopics: number;
  Page: number;
  PageSize: number;
  TotalPages: number;
  PopularTags: Tag[];
}

// Profile Types
export interface UpdateProfileRequest {
  fullName?: string;
  bio?: string;
  avatar?: string;
  showEmail?: boolean;
  showBio?: boolean;
}

export interface ChangePasswordRequest {
  currentPassword: string;
  newPassword: string;
  confirmPassword: string;
}

// Auth Types
export interface AuthResponse {
  token: string;
  user: User;
}

export interface CreatePostRequest {
  content: string;
  parentPostId?: number;
}

// Expert Types
export interface Expert {
  id: number;
  username: string;
  fullName: string;
  avatar?: string;
  bio?: string;
  specialties: string[];
  rating: number;
  ratingCount: number;
  topicCount: number;
  postCount: number;
  isOnline: boolean;
  yearsOfExperience?: number;
  createdAt: string;
}

export interface ExpertSummary {
  id: number;
  username: string;
  fullName: string;
  avatar?: string;
  bio?: string;
  specialties: string[];
  rating: number;
  ratingCount: number;
  topicCount: number;
  postCount: number;
  isOnline: boolean;
  isVerified?: boolean;
  lastSeen?: string;
  location?: string;
  experience?: string;
  reviewCount: number;
  answerCount: number;
}

export interface ExpertStats {
  totalTopics: number;
  totalPosts: number;
  totalLikes: number;
  averageRating: number;
  ratingCount: number;
  specialtyCount: number;
}

export interface ExpertSearchRequest {
  query?: string;
  specialty?: string;
  isOnline?: boolean;
  minRating?: number;
  sortBy?: string;
  sortOrder?: string;
  page: number;
  pageSize: number;
}

// Topics List Types
export interface TopicsListRequest {
  query?: string;
  categoryId?: number;
  tagIds?: number[];
  hasAnswer?: boolean;
  isAnswered?: boolean;
  authorRole?: string;
  sortBy?: string;
  sortOrder?: string;
  page: number;
  pageSize: number;
}
