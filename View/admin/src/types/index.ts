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

// Admin Auth Types
export interface AdminLoginRequest {
  username: string;
  password: string;
}

export interface AdminUser {
  id: number;
  username: string;
  email: string;
  fullName: string;
  avatar?: string;
  role: string;
  isActive: boolean;
  createdAt: string;
  lastLoginAt?: string;
}

export interface AdminAuthResponse {
  token: string;
  admin: AdminUser;
}

export interface ChangePasswordRequest {
  currentPassword: string;
  newPassword: string;
  confirmPassword: string;
}

// Dashboard Types
export interface DashboardStats {
  totalUsers: number;
  totalTopics: number;
  totalPosts: number;
  totalReports: number;
  totalCategories: number;
  newUsersToday: number;
  newTopicsToday: number;
  newPostsToday: number;
  pendingReports: number;
}

export interface ChartDataPoint {
  date: string;
  count: number;
}

export interface ChartData {
  usersChart: ChartDataPoint[];
  topicsChart: ChartDataPoint[];
  reportsChart: ChartDataPoint[];
}

export interface QuickAction {
  title: string;
  description: string;
  icon: string;
  url: string;
  color: string;
}

export interface DashboardData {
  stats: DashboardStats;
  chartData: ChartData;
  quickActions: QuickAction[];
}

export interface RecentActivity {
  id: number;
  type: 'UserRegistered' | 'TopicCreated' | 'PostCreated';
  description: string;
  userInfo: string;
  timestamp: string;
}

// User Management Types
export interface UserSummary {
  id: number;
  username: string;
  email: string;
  fullName: string;
  role: 'Member' | 'Doctor' | 'Moderator';
  isActive: boolean;
  isEmailVerified: boolean;
  createdAt: string;
  lastLoginAt?: string;
  topicCount: number;
  postCount: number;
  likeCount: number;
}

export interface CreateUserRequest {
  username: string;
  email: string;
  fullName: string;
  password: string;
  role: string;
}

export interface UpdateUserRequest {
  fullName?: string;
  email?: string;
  role?: string;
  isActive?: boolean;
}


export interface UserDetailResponse {
  user: UserSummary;
  recentTopics: TopicSummary[];
  recentPosts: PostSummary[];
}

// Category Management Types
export interface Category {
  id: number;
  name: string;
  slug: string;
  description: string;
  icon?: string;
  color?: string;
  displayOrder: number;
  isActive: boolean;
  topicCount: number;
  postCount: number;
  createdAt: string;
  updatedAt: string;
}

export interface CreateCategoryRequest {
  name: string;
  description: string;
  icon?: string;
  color?: string;
  displayOrder?: number;
}

export interface UpdateCategoryRequest {
  name?: string;
  description?: string;
  icon?: string;
  color?: string;
  displayOrder?: number;
  isActive?: boolean;
}

// Topic Management Types
export interface TopicSummary {
  id: number;
  title: string;
  slug: string;
  categoryName: string;
  categorySlug: string;
  authorName: string;
  authorRole: string;
  isPinned: boolean;
  isLocked: boolean;
  hasAnswer: boolean;
  viewCount: number;
  likeCount: number;
  postCount: number;
  createdAt: string;
  lastActivityAt?: string;
  tags: string[];
}

export interface TopicDetail {
  id: number;
  title: string;
  slug: string;
  content: string;
  categoryId: number;
  categoryName: string;
  authorId: number;
  authorName: string;
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

export interface UpdateTopicRequest {
  title?: string;
  content?: string;
  categoryId?: number;
  isPinned?: boolean;
  isLocked?: boolean;
}

export interface PostSummary {
  id: number;
  content: string;
  topicId: number;
  topicTitle: string;
  authorName: string;
  authorRole: string;
  isAnswer: boolean;
  likeCount: number;
  createdAt: string;
}


// Search and Filter Types
export interface UserSearchRequest {
  role?: string;
  status?: string;
  page: number;
  pageSize: number;
}

export interface TopicSearchRequest {
  categoryId?: number;
  authorId?: number;
  isPinned?: boolean;
  isLocked?: boolean;
  hasAnswer?: boolean;
  page: number;
  pageSize: number;
  sortBy?: 'newest' | 'oldest' | 'popular' | 'title';
}

// Expert Management Types
export interface ExpertSummary {
  id: number;
  username: string;
  email: string;
  fullName: string;
  avatar?: string;
  bio?: string;
  specialty?: string;
  rating: number;
  reviewCount: number;
  isVerified: boolean;
  isOnline: boolean;
  isActive: boolean;
  createdAt: string;
  lastLoginAt?: string;
  topicCount: number;
  postCount: number;
}

export interface ExpertDetail extends ExpertSummary {
  bio: string;
  experience: string;
  education: string;
  certifications: string[];
  workplaces: string[];
  recentTopics: TopicSummary[];
  recentPosts: PostSummary[];
  reviews: ExpertReview[];
}

export interface ExpertReview {
  id: number;
  patientName: string;
  rating: number;
  comment: string;
  createdAt: string;
}

export interface ExpertSpecialty {
  id: number;
  name: string;
  description: string;
  expertCount: number;
  isActive: boolean;
  createdAt: string;
}

export interface ExpertSearchRequest {
  specialty?: string;
  isVerified?: boolean;
  isActive?: boolean;
  verificationStatus?: string;
  page: number;
  pageSize: number;
  sortBy?: 'newest' | 'oldest' | 'name' | 'rating' | 'topics';
}

export interface UpdateExpertRequest {
  isVerified?: boolean;
  isActive?: boolean;
  verificationStatus?: 'Pending' | 'Verified' | 'Rejected';
}

export interface CreateExpertSpecialtyRequest {
  name: string;
  description: string;
}

export interface UpdateExpertSpecialtyRequest {
  name?: string;
  description?: string;
  isActive?: boolean;
}

// Search Types
export interface GlobalSearchRequest {
  query: string;
  type?: 'all' | 'users' | 'topics' | 'experts' | 'categories';
  page?: number;
  pageSize?: number;
}

export interface SearchResults {
  users: UserSummary[];
  topics: TopicSummary[];
  experts: ExpertSummary[];
  categories: Category[];
  totalResults: number;
}

export interface SearchSuggestion {
  text: string;
  type: 'user' | 'topic' | 'expert' | 'category';
  count: number;
}

// Report Types
export interface Report {
  id: number;
  userName: string;
  topicTitle?: string;
  postContent?: string;
  category: string;
  reason: string;
  status: 'Chờ xử lý' | 'Đã xem xét' | 'Đã giải quyết' | 'Đã từ chối';
  createdAt: string;
  reviewedAt?: string;
  reviewedByAdmin?: string;
}

export interface UpdateReportStatusRequest {
  status: 'Đã xem xét' | 'Đã giải quyết' | 'Đã từ chối';
}

export interface ReportSearchRequest {
  status?: string;
  category?: string;
  page: number;
  pageSize: number;
  sortBy?: 'newest' | 'oldest' | 'status';
}

// Table Types
export interface TableColumn<T> {
  key: keyof T;
  header: string;
  sortable?: boolean;
  noWrap?: boolean;
  width?: string; 
  render?: (value: any, item: T) => React.ReactNode;
}

export interface TableProps<T> {
  data: T[];
  columns: TableColumn<T>[];
  loading?: boolean;
  pagination?: {
    currentPage: number;
    totalPages: number;
    onPageChange: (page: number) => void;
  };
  onSort?: (key: keyof T, direction: 'asc' | 'desc') => void;
}
