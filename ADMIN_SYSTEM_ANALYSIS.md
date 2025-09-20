# PHÂN TÍCH HỆ THỐNG QUẢN TRỊ DIỄN ĐÀN Y TẾ

## TỔNG QUAN HỆ THỐNG

Hệ thống quản trị diễn đàn y tế là một ứng dụng web toàn diện được xây dựng để quản lý và điều hành một nền tảng thảo luận y tế trực tuyến. Hệ thống bao gồm các chức năng quản lý người dùng, chuyên gia y tế, nội dung thảo luận, báo cáo vi phạm và thống kê tổng quan.

---

## SƠ ĐỒ MÔ TẢ BÀI TOÁN HỆ THỐNG

```
┌─────────────────────────────────────────────────────────────────┐
│                    DIỄN ĐÀN Y TẾ TRỰC TUYẾN                     │
│                                                                 │
│  ┌─────────────────┐    ┌─────────────────┐    ┌──────────────┐ │
│  │   NGƯỜI DÙNG    │    │   CHUYÊN GIA    │    │  QUẢN TRỊ    │ │
│  │   THÔNG THƯỜNG  │    │      Y TẾ       │    │    VIÊN      │ │
│  └─────────────────┘    └─────────────────┘    └──────────────┘ │
│           │                       │                     │        │
│           │                       │                     │        │
│           ▼                       ▼                     ▼        │
│  ┌─────────────────────────────────────────────────────────────┐ │
│  │                    PLATFORM CHÍNH                          │ │
│  │                                                             │ │
│  │  • Đăng ký/Đăng nhập          • Tạo chủ đề thảo luận       │ │
│  │  • Quản lý hồ sơ cá nhân      • Trả lời bài viết           │ │
│  │  • Tìm kiếm thông tin         • Đánh giá chuyên gia        │ │
│  │  • Báo cáo vi phạm           • Theo dõi chủ đề yêu thích   │ │
│  │  • Tương tác xã hội          • Nhận tư vấn từ chuyên gia   │ │
│  └─────────────────────────────────────────────────────────────┘ │
│                                   │                             │
│                                   ▼                             │
│  ┌─────────────────────────────────────────────────────────────┐ │
│  │                  HỆ THỐNG QUẢN TRỊ                         │ │
│  │                                                             │ │
│  │  • Kiểm soát người dùng       • Kiểm duyệt nội dung        │ │
│  │  • Xác thực chuyên gia        • Xử lý báo cáo vi phạm      │ │
│  │  │  Quản lý danh mục          • Thống kê và phân tích      │ │
│  │  • Cấu hình hệ thống          • Giám sát hoạt động         │ │
│  └─────────────────────────────────────────────────────────────┘ │
└─────────────────────────────────────────────────────────────────┘

                        VẤN ĐỀ CẦN GIẢI QUYẾT:

┌─────────────────────────────────────────────────────────────────┐
│                     THÁCH THỨC QUẢN LÝ                         │
│                                                                 │
│ 🎯 QUẢN LÝ NGƯỜI DÙNG:                                         │
│    • Xác thực danh tính chuyên gia y tế                        │
│    • Phân quyền và kiểm soát truy cập                          │
│    • Theo dõi hoạt động và hành vi người dùng                  │
│                                                                 │
│ 🎯 KIỂM DUYỆT NỘI DUNG:                                        │
│    • Đảm bảo thông tin y tế chính xác và an toàn               │
│    • Ngăn chặn thông tin sai lệch hoặc có hại                  │
│    • Duy trì chất lượng thảo luận chuyên nghiệp                │
│                                                                 │
│ 🎯 BẢO MẬT VÀ AN TOÀN:                                         │
│    • Bảo vệ thông tin sức khỏe cá nhân                         │
│    • Tuân thủ quy định về quyền riêng tư y tế                  │
│    • Ngăn chặn spam và lạm dụng hệ thống                       │
│                                                                 │
│ 🎯 THỐNG KÊ VÀ PHÂN TÍCH:                                       │
│    • Theo dõi xu hướng sức khỏe cộng đồng                      │
│    • Đánh giá hiệu quả hoạt động của nền tảng                  │
│    • Cung cấp báo cáo quản lý chi tiết                         │
└─────────────────────────────────────────────────────────────────┘
```

---

## LƯỢC ĐỒ USE CASE HỆ THỐNG QUẢN TRỊ

```
                    BIỂU ĐỒ USE CASE ADMIN SYSTEM

    ┌─────────────────┐                                    ┌─────────────────┐
    │                 │                                    │                 │
    │   SUPER ADMIN   │                                    │      ADMIN      │
    │                 │                                    │                 │
    └─────────────────┘                                    └─────────────────┘
            │                                                       │
            │                                                       │
    ┌───────▼───────────────────────────────────────────────────────▼───────┐
    │                                                                       │
    │                        HỆ THỐNG QUẢN TRỊ                             │
    │                                                                       │
    │  ┌─────────────────────────────────────────────────────────────────┐  │
    │  │                    QUẢN LÝ XÁC THỰC                            │  │
    │  │                                                                 │  │
    │  │  ◉ Đăng nhập hệ thống quản trị                                  │  │
    │  │  ◉ Quản lý hồ sơ quản trị viên                                  │  │
    │  │  ◉ Thay đổi mật khẩu bảo mật                                    │  │
    │  │  ◉ Kiểm soát phiên đăng nhập                                    │  │
    │  └─────────────────────────────────────────────────────────────────┘  │
    │                                                                       │
    │  ┌─────────────────────────────────────────────────────────────────┐  │
    │  │                     QUẢN LÝ NGƯỜI DÙNG                         │  │
    │  │                                                                 │  │
    │  │  ◉ Xem danh sách tất cả người dùng                             │  │
    │  │  ◉ Tìm kiếm và lọc người dùng theo tiêu chí                    │  │
    │  │  ◉ Tạo tài khoản người dùng mới                                │  │
    │  │  ◉ Cập nhật thông tin người dùng                               │  │
    │  │  ◉ Phân quyền vai trò (Thành viên/Bác sĩ/Kiểm duyệt viên)      │  │
    │  │  ◉ Kích hoạt/Vô hiệu hóa tài khoản                             │  │
    │  │  ◉ Đặt lại mật khẩu cho người dùng                             │  │
    │  │  ◉ Xóa tài khoản người dùng                                    │  │
    │  │  ◉ Xem thống kê hoạt động người dùng                           │  │
    │  └─────────────────────────────────────────────────────────────────┘  │
    │                                                                       │
    │  ┌─────────────────────────────────────────────────────────────────┐  │
    │  │                   QUẢN LÝ CHUYÊN GIA Y TẾ                       │  │
    │  │                                                                 │  │
    │  │  ◉ Xem danh sách chuyên gia đã đăng ký                         │  │
    │  │  ◉ Tìm kiếm chuyên gia theo chuyên khoa                        │  │
    │  │  ◉ Xác thực và phê duyệt chuyên gia                            │  │
    │  │  ◉ Từ chối xác thực với lý do cụ thể                           │  │
    │  │  ◉ Cập nhật thông tin chuyên khoa                              │  │
    │  │  ◉ Quản lý danh sách chuyên khoa y tế                          │  │
    │  │  ◉ Xem đánh giá và phản hồi về chuyên gia                      │  │
    │  │  ◉ Xóa đánh giá không phù hợp                                  │  │
    │  │  ◉ Thống kê hiệu suất chuyên gia                               │  │
    │  └─────────────────────────────────────────────────────────────────┘  │
    │                                                                       │
    │  ┌─────────────────────────────────────────────────────────────────┐  │
    │  │                    QUẢN LÝ NỘI DUNG                             │  │
    │  │                                                                 │  │
    │  │  ◉ Xem tất cả chủ đề thảo luận                                  │  │
    │  │  ◉ Tìm kiếm chủ đề theo danh mục và từ khóa                     │  │
    │  │  ◉ Kiểm duyệt và chỉnh sửa nội dung chủ đề                     │  │
    │  │  ◉ Ghim chủ đề quan trọng                                       │  │
    │  │  ◉ Khóa chủ đề không cho phép thảo luận                        │  │
    │  │  ◉ Xóa chủ đề vi phạm quy định                                  │  │
    │  │  ◉ Quản lý bài trả lời và bình luận                            │  │
    │  │  ◉ Đánh dấu câu trả lời chính xác                              │  │
    │  │  ◉ Xóa bài viết không phù hợp                                   │  │
    │  └─────────────────────────────────────────────────────────────────┘  │
    │                                                                       │
    │  ┌─────────────────────────────────────────────────────────────────┐  │
    │  │                   QUẢN LÝ DANH MỤC                              │  │
    │  │                                                                 │  │
    │  │  ◉ Tạo danh mục thảo luận mới                                   │  │
    │  │  ◉ Chỉnh sửa thông tin danh mục                                 │  │
    │  │  ◉ Sắp xếp thứ tự hiển thị danh mục                            │  │
    │  │  ◉ Kích hoạt/Vô hiệu hóa danh mục                              │  │
    │  │  ◉ Xóa danh mục (chuyển chủ đề sang danh mục khác)             │  │
    │  │  ◉ Quản lý thẻ tag cho chủ đề                                   │  │
    │  │  ◉ Gộp các thẻ tag trùng lặp                                    │  │
    │  │  ◉ Cập nhật số lượng thống kê thẻ tag                          │  │
    │  └─────────────────────────────────────────────────────────────────┘  │
    │                                                                       │
    │  ┌─────────────────────────────────────────────────────────────────┐  │
    │  │                   XỬ LÝ BÁO CÁO VI PHẠM                         │  │
    │  │                                                                 │  │
    │  │  ◉ Xem danh sách tất cả báo cáo                                 │  │
    │  │  ◉ Lọc báo cáo theo trạng thái và loại vi phạm                 │  │
    │  │  ◉ Xem chi tiết nội dung bị báo cáo                            │  │
    │  │  ◉ Phân loại mức độ nghiêm trọng                               │  │
    │  │  ◉ Xử lý báo cáo (Chấp nhận/Từ chối/Cần xem xét)              │  │
    │  │  ◉ Ghi chú lý do xử lý                                          │  │
    │  │  ◉ Thông báo kết quả xử lý cho người báo cáo                   │  │
    │  └─────────────────────────────────────────────────────────────────┘  │
    │                                                                       │
    │  ┌─────────────────────────────────────────────────────────────────┐  │
    │  │                 TÌM KIẾM VÀ PHÂN TÍCH                           │  │
    │  │                                                                 │  │
    │  │  ◉ Tìm kiếm toàn cục trong hệ thống                            │  │
    │  │  ◉ Tìm kiếm người dùng theo nhiều tiêu chí                     │  │
    │  │  ◉ Tìm kiếm chuyên gia theo chuyên môn                         │  │
    │  │  ◉ Tìm kiếm nội dung theo từ khóa                               │  │
    │  │  ◉ Gợi ý tìm kiếm thông minh                                    │  │
    │  │  ◉ Lưu lịch sử tìm kiếm                                         │  │
    │  └─────────────────────────────────────────────────────────────────┘  │
    │                                                                       │
    │  ┌─────────────────────────────────────────────────────────────────┐  │
    │  │                  DASHBOARD VÀ THỐNG KÊ                          │  │
    │  │                                                                 │  │
    │  │  ◉ Xem tổng quan thống kê hệ thống                              │  │
    │  │  ◉ Biểu đồ tăng trưởng người dùng                               │  │
    │  │  ◉ Thống kê hoạt động hàng ngày                                 │  │
    │  │  ◉ Báo cáo xu hướng thảo luận                                   │  │
    │  │  ◉ Phân tích hiệu suất chuyên gia                               │  │
    │  │  ◉ Thống kê báo cáo vi phạm                                     │  │
    │  │  ◉ Xuất báo cáo dữ liệu                                         │  │
    │  │  ◉ Cấu hình thông báo cảnh báo                                  │  │
    │  └─────────────────────────────────────────────────────────────────┘  │
    │                                                                       │
    └───────────────────────────────────────────────────────────────────────┘

                    ┌─────────────────┐
                    │                 │
                    │   MODERATOR     │
                    │                 │
                    └─────────────────┘
                            │
                ┌───────────▼───────────┐
                │                       │
                │   CHỨC NĂNG GIỚI HẠN  │
                │                       │
                │  ◉ Kiểm duyệt chủ đề  │
                │  ◉ Xử lý báo cáo      │
                │  ◉ Quản lý bình luận  │
                │                       │
                └───────────────────────┘
```

---

## KIẾN TRÚC HỆ THỐNG ADMIN

### 1. KIẾN TRÚC TỔNG QUAN

```
┌─────────────────────────────────────────────────────────────────────────┐
│                          KIẾN TRÚC HỆ THỐNG 3 TẦNG                      │
│                                                                         │
│  ┌─────────────────────────────────────────────────────────────────┐    │
│  │                     TẦNG GIAO DIỆN (FRONTEND)                  │    │
│  │                                                                 │    │
│  │  ┌─────────────────┐    ┌──────────────────┐    ┌─────────────┐ │    │
│  │  │   React Admin   │    │  TypeScript      │    │ Tailwind    │ │    │
│  │  │   Dashboard     │    │  Type Safety     │    │    CSS      │ │    │
│  │  └─────────────────┘    └──────────────────┘    └─────────────┘ │    │
│  │                                                                 │    │
│  │  ┌─────────────────┐    ┌──────────────────┐    ┌─────────────┐ │    │
│  │  │   React Router  │    │   Axios HTTP     │    │  Recharts   │ │    │
│  │  │   Navigation    │    │    Client        │    │  Analytics  │ │    │
│  │  └─────────────────┘    └──────────────────┘    └─────────────┘ │    │
│  └─────────────────────────────────────────────────────────────────┘    │
│                                    │                                     │
│                              HTTP/HTTPS                                  │
│                                    │                                     │
│  ┌─────────────────────────────────▼───────────────────────────────────┐ │
│  │                        TẦNG ỨNG DỤNG (BACKEND)                     │ │
│  │                                                                     │ │
│  │  ┌─────────────────────────────────────────────────────────────┐    │ │
│  │  │                    ASP.NET CORE WEB API                     │    │ │
│  │  │                                                             │    │ │
│  │  │  ┌─────────────┐  ┌─────────────┐  ┌─────────────────────┐  │    │ │
│  │  │  │Controllers  │  │ Services    │  │    JWT Security     │  │    │ │
│  │  │  │  - Admin    │  │ - Business  │  │  - Authentication   │  │    │ │
│  │  │  │  - Auth     │  │   Logic     │  │  - Authorization    │  │    │ │
│  │  │  │  - Users    │  │ - Data      │  │  - Policy Based     │  │    │ │
│  │  │  │  - Experts  │  │   Access    │  │                     │  │    │ │
│  │  │  │  - Topics   │  │             │  │                     │  │    │ │
│  │  │  └─────────────┘  └─────────────┘  └─────────────────────┘  │    │ │
│  │  └─────────────────────────────────────────────────────────────┘    │ │
│  │                                                                     │ │
│  │  ┌─────────────────────────────────────────────────────────────┐    │ │
│  │  │                      DEPENDENCY INJECTION                   │    │ │
│  │  │                                                             │    │ │
│  │  │  ┌───────────────┐  ┌───────────────┐  ┌─────────────────┐  │    │ │
│  │  │  │   Logging     │  │   Caching     │  │   Background    │  │    │ │
│  │  │  │   Service     │  │   Service     │  │    Services     │  │    │ │
│  │  │  └───────────────┘  └───────────────┘  └─────────────────┘  │    │ │
│  │  └─────────────────────────────────────────────────────────────┘    │ │
│  └─────────────────────────────────────────────────────────────────────┘ │
│                                    │                                     │
│                                EF Core                                   │
│                                    │                                     │
│  ┌─────────────────────────────────▼───────────────────────────────────┐ │
│  │                          TẦNG DỮ LIỆU                              │ │
│  │                                                                     │ │
│  │  ┌─────────────────────────────────────────────────────────────┐    │ │
│  │  │                    SQL SERVER DATABASE                     │    │ │
│  │  │                                                             │    │ │
│  │  │  ┌─────────────┐  ┌─────────────┐  ┌─────────────────────┐  │    │ │
│  │  │  │   Users     │  │  Topics     │  │     Categories      │  │    │ │
│  │  │  │   Experts   │  │  Posts      │  │       Tags          │  │    │ │
│  │  │  │   Admins    │  │  Reports    │  │    Specialties      │  │    │ │
│  │  │  │             │  │  Likes      │  │                     │  │    │ │
│  │  │  └─────────────┘  └─────────────┘  └─────────────────────┘  │    │ │
│  │  └─────────────────────────────────────────────────────────────┘    │ │
│  └─────────────────────────────────────────────────────────────────────┘ │
└─────────────────────────────────────────────────────────────────────────┘
```

### 2. KIẾN TRÚC BACKEND CHI TIẾT

```
                    KIẾN TRÚC ASP.NET CORE BACKEND

┌─────────────────────────────────────────────────────────────────────────┐
│                         CONTROLLER LAYER                               │
│                                                                         │
│  AdminAuthController      AdminUserController                          │
│  • POST /login             • GET /users                                │
│  • GET /profile            • POST /users/search                        │
│  • PUT /profile            • POST /users                               │
│  • POST /change-password   • PUT /users/{id}                           │
│                            • DELETE /users/{id}                        │
│                                                                         │
│  AdminExpertController     AdminTopicController                        │
│  • GET /experts            • GET /topics                               │
│  • POST /experts/search    • POST /topics/search                       │
│  • PUT /experts/{id}/verify • PUT /topics/{id}                         │
│  • GET /experts/specialties • DELETE /topics/{id}                      │
│                                                                         │
│  AdminCategoryController   AdminDashboardController                    │
│  • GET /categories         • GET /dashboard                            │
│  • POST /categories        • GET /dashboard/stats                      │
│  • PUT /categories/{id}    • GET /dashboard/chart-data                 │
│  • POST /categories/reorder                                            │
└─────────────────────────────────────────────────────────────────────────┘
                                    │
                                    ▼
┌─────────────────────────────────────────────────────────────────────────┐
│                           SERVICE LAYER                                │
│                                                                         │
│  IAdminAuthService         IAdminUserService                           │
│  • LoginAsync()            • GetUsersAsync()                           │
│  • GetProfileAsync()       • CreateUserAsync()                         │
│  • UpdateProfileAsync()    • UpdateUserAsync()                         │
│  • ChangePasswordAsync()   • DeleteUserAsync()                         │
│                                                                         │
│  IAdminExpertService       IAdminTopicService                          │
│  • GetExpertsAsync()       • GetTopicsAsync()                          │
│  • VerifyExpertAsync()     • UpdateTopicAsync()                        │
│  • GetSpecialtiesAsync()   • DeleteTopicAsync()                        │
│                                                                         │
│  IAdminCategoryService     IAdminDashboardService                      │
│  • GetCategoriesAsync()    • GetStatsAsync()                           │
│  • CreateCategoryAsync()   • GetChartDataAsync()                       │
│  • UpdateCategoryAsync()   • GetQuickActionsAsync()                    │
└─────────────────────────────────────────────────────────────────────────┘
                                    │
                                    ▼
┌─────────────────────────────────────────────────────────────────────────┐
│                         COMMON SERVICES                                │
│                                                                         │
│  IJwtService              IHtmlSanitizerService                        │
│  • GenerateAdminToken()   • SanitizeHtml()                             │
│  • GenerateUserToken()    • CleanContent()                             │
│  • ValidateToken()        • StripDangerousTags()                       │
│  • GetUserIdFromToken()                                                │
└─────────────────────────────────────────────────────────────────────────┘
                                    │
                                    ▼
┌─────────────────────────────────────────────────────────────────────────┐
│                            DATA LAYER                                  │
│                                                                         │
│                          AppDbContext                                  │
│                                                                         │
│  DbSet<AdminAccount>      DbSet<UserAccount>                           │
│  DbSet<Category>          DbSet<Topic>                                  │
│  DbSet<Post>              DbSet<Report>                                 │
│  DbSet<Like>              DbSet<Tag>                                    │
│  DbSet<Specialty>         DbSet<TopicView>                             │
│  DbSet<StaticPage>        DbSet<TopicTag>                              │
└─────────────────────────────────────────────────────────────────────────┘
```

### 3. KIẾN TRÚC FRONTEND CHI TIẾT

```
                    KIẾN TRÚC REACT ADMIN FRONTEND

┌─────────────────────────────────────────────────────────────────────────┐
│                          REACT APPLICATION                             │
│                                                                         │
│   ┌─────────────────────────────────────────────────────────────────┐   │
│   │                        ROUTING LAYER                           │   │
│   │                                                                 │   │
│   │  React Router Dom v7.8.2                                       │   │
│   │  ┌─────────────────────────────────────────────────────────┐    │   │
│   │  │  /login          - LoginPage                            │    │   │
│   │  │  /               - DashboardPage                        │    │   │
│   │  │  /users          - UsersPage                            │    │   │
│   │  │  /experts        - ExpertsPage                          │    │   │
│   │  │  /categories     - CategoriesPage                       │    │   │
│   │  │  /topics         - TopicsPage                           │    │   │
│   │  │  /reports        - ReportsPage                          │    │   │
│   │  │  /profile        - ProfilePage                          │    │   │
│   │  └─────────────────────────────────────────────────────────┘    │   │
│   └─────────────────────────────────────────────────────────────────┘   │
│                                    │                                    │
│                                    ▼                                    │
│   ┌─────────────────────────────────────────────────────────────────┐   │
│   │                      COMPONENT LAYER                           │   │
│   │                                                                 │   │
│   │  ┌─────────────────┐  ┌─────────────────┐  ┌─────────────────┐  │   │
│   │  │     Layout      │  │       UI        │  │   Protected     │  │   │
│   │  │   Components    │  │   Components    │  │     Route       │  │   │
│   │  │                 │  │                 │  │                 │  │   │
│   │  │ • Header.tsx    │  │ • Button        │  │ • Auth Guard    │  │   │
│   │  │ • Sidebar.tsx   │  │ • Modal         │  │ • Role Check    │  │   │
│   │  │ • Layout.tsx    │  │ • Table         │  │ • Redirect      │  │   │
│   │  │                 │  │ • Form          │  │                 │  │   │
│   │  └─────────────────┘  └─────────────────┘  └─────────────────┘  │   │
│   └─────────────────────────────────────────────────────────────────┘   │
│                                    │                                    │
│                                    ▼                                    │
│   ┌─────────────────────────────────────────────────────────────────┐   │
│   │                       SERVICE LAYER                            │   │
│   │                                                                 │   │
│   │  ┌─────────────────────────────────────────────────────────┐    │   │
│   │  │                     API SERVICE                        │    │   │
│   │  │                                                         │    │   │
│   │  │  • authApi       - Xác thực admin                       │    │   │
│   │  │  • usersApi      - Quản lý người dùng                   │    │   │
│   │  │  • expertsApi    - Quản lý chuyên gia                   │    │   │
│   │  │  • topicsApi     - Quản lý chủ đề                       │    │   │
│   │  │  • categoriesApi - Quản lý danh mục                     │    │   │
│   │  │  • reportsApi    - Quản lý báo cáo                      │    │   │
│   │  │  • searchApi     - Tìm kiếm toàn cục                    │    │   │
│   │  │  • dashboardApi  - Thống kê dashboard                   │    │   │
│   │  └─────────────────────────────────────────────────────────┘    │   │
│   └─────────────────────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────────────────────┘
```

---

## PHÂN TÍCH CƠ SỞ DỮ LIỆU

### 1. BẢNG DỮ LIỆU CHÍNH

```
┌─────────────────────────────────────────────────────────────────────────┐
│                          SCHEMA CƠ SỞ DỮ LIỆU                          │
│                                                                         │
│  ┌─────────────────┐    ┌─────────────────┐    ┌─────────────────────┐  │
│  │   AdminAccount  │    │   UserAccount   │    │      Category       │  │
│  │                 │    │                 │    │                     │  │
│  │ • Id (PK)       │    │ • Id (PK)       │    │ • Id (PK)           │  │
│  │ • Username      │    │ • Username      │    │ • Name              │  │
│  │ • Email         │    │ • Email         │    │ • Slug              │  │
│  │ • PasswordHash  │    │ • PasswordHash  │    │ • Description       │  │
│  │ • FullName      │    │ • FullName      │    │ • Icon              │  │
│  │ • Avatar        │    │ • Avatar        │    │ • Color             │  │
│  │ • Role          │    │ • Bio           │    │ • DisplayOrder      │  │
│  │ • IsActive      │    │ • Role          │    │ • IsActive          │  │
│  │ • CreatedAt     │    │ • IsActive      │    │ • TopicCount        │  │
│  │ • UpdatedAt     │    │ • IsEmailVerified │   │ • PostCount         │  │
│  │ • LastLoginAt   │    │ • ShowEmail     │    │ • CreatedAt         │  │
│  └─────────────────┘    │ • ShowBio       │    │ • UpdatedAt         │  │
│                         │ • CreatedAt     │    └─────────────────────┘  │
│                         │ • UpdatedAt     │                            │
│                         │ • LastLoginAt   │                            │
│                         │ • ResetToken    │                            │
│                         └─────────────────┘                            │
│                                                                         │
│  ┌─────────────────┐    ┌─────────────────┐    ┌─────────────────────┐  │
│  │      Topic      │    │      Post       │    │       Report        │  │
│  │                 │    │                 │    │                     │  │
│  │ • Id (PK)       │    │ • Id (PK)       │    │ • Id (PK)           │  │
│  │ • Title         │    │ • Content       │    │ • UserId (FK)       │  │
│  │ • Slug          │    │ • TopicId (FK)  │    │ • TopicId (FK)      │  │
│  │ • Content       │    │ • UserId (FK)   │    │ • PostId (FK)       │  │
│  │ • CategoryId FK │    │ • IsActive      │    │ • Category          │  │
│  │ • UserId (FK)   │    │ • IsAnswer      │    │ • Reason            │  │
│  │ • IsActive      │    │ • LikeCount     │    │ • Status            │  │
│  │ • IsPinned      │    │ • CreatedAt     │    │ • CreatedAt         │  │
│  │ • IsLocked      │    │ • UpdatedAt     │    │ • ReviewedAt        │  │
│  │ • HasAnswer     │    └─────────────────┘    │ • ReviewedBy        │  │
│  │ • ViewCount     │                           └─────────────────────┘  │
│  │ • LikeCount     │                                                   │
│  │ • PostCount     │    ┌─────────────────┐    ┌─────────────────────┐  │
│  │ • CreatedAt     │    │       Tag       │    │      Specialty      │  │
│  │ • UpdatedAt     │    │                 │    │                     │  │
│  │ • LastActivityAt │   │ • Id (PK)       │    │ • Id (PK)           │  │
│  └─────────────────┘    │ • Name          │    │ • Name              │  │
│                         │ • Slug          │    │ • Description       │  │
│                         │ • Color         │    │ • ExpertCount       │  │
│                         │ • TopicCount    │    │ • IsActive          │  │
│                         │ • CreatedAt     │    │ • CreatedAt         │  │
│                         └─────────────────┘    └─────────────────────┘  │
└─────────────────────────────────────────────────────────────────────────┘
```

---

## TÍNH NĂNG CHI TIẾT HỆ THỐNG

### 1. XÁC THỰC VÀ PHÂN QUYỀN

#### A. Hệ thống đăng nhập Admin
- **Endpoint**: `POST /api/admin/auth/login`
- **Xác thực**: Username/Password với BCrypt hashing  
- **JWT Token**: Phân biệt admin và user token
- **Session Management**: Theo dõi thời gian đăng nhập cuối
- **Security**: Policy-based authorization

#### B. Phân quyền nhiều cấp
```
SuperAdmin  ─┐
             ├── Toàn quyền hệ thống
             └── Quản lý admin khác

Admin       ─┐
             ├── Quản lý người dùng
             ├── Quản lý nội dung  
             ├── Xử lý báo cáo
             └── Xem thống kê

Moderator   ─┐
             ├── Kiểm duyệt chủ đề
             ├── Xử lý báo cáo
             └── Quản lý bình luận
```

### 2. QUẢN LÝ NGƯỜI DÙNG

#### A. Tìm kiếm và lọc nâng cao
- **Endpoint**: `POST /api/admin/users/search`
- **Tiêu chí lọc**:
  - Vai trò (Member/Doctor/Moderator)
  - Trạng thái (Active/Inactive)
  - Ngày đăng ký
  - Trạng thái xác thực email
- **Phân trang**: Hỗ trợ pagination với page/pageSize
- **Sắp xếp**: Theo tên, ngày tạo, hoạt động cuối

#### B. CRUD người dùng hoàn chỉnh
- **Tạo mới**: Tạo tài khoản với validation đầy đủ
- **Cập nhật**: Sửa thông tin cá nhân và vai trò
- **Xóa**: Soft delete với kiểm tra ràng buộc
- **Reset mật khẩu**: Đặt lại mật khẩu an toàn
- **Toggle status**: Kích hoạt/vô hiệu hóa tài khoản

### 3. QUẢN LÝ CHUYÊN GIA Y TẾ

#### A. Xác thực chuyên gia
- **Endpoint**: `PUT /api/admin/experts/{id}/verify`
- **Quy trình**: Pending → Verified/Rejected
- **Thông tin kiểm tra**:
  - Bằng cấp chuyên môn
  - Kinh nghiệm làm việc
  - Chứng chỉ hành nghề
  - Đánh giá từ cộng đồng

#### B. Quản lý chuyên khoa
- **CRUD chuyên khoa**: Tạo, sửa, xóa chuyên khoa y tế
- **Phân loại chuyên gia**: Gắn chuyên gia vào chuyên khoa
- **Thống kê**: Số lượng chuyên gia theo từng chuyên khoa

### 4. DASHBOARD VÀ THỐNG KÊ

#### A. Thống kê tổng quan
- **Số liệu chính**:
  - Tổng số người dùng: `TotalUsers`
  - Tổng số chủ đề: `TotalTopics`
  - Tổng số bài viết: `TotalPosts`
  - Số báo cáo chờ xử lý: `PendingReports`
  - Người dùng mới hôm nay: `NewUsersToday`

#### B. Biểu đồ phân tích
- **Biểu đồ tăng trưởng**: Người dùng và chủ đề theo thời gian
- **Phân tích báo cáo**: Thống kê theo loại vi phạm
- **Hoạt động hàng ngày**: Xu hướng tương tác

---

## CÔNG NGHỆ VÀ THƯ VIỆN

### 1. BACKEND (.NET)

#### A. Framework chính
- **ASP.NET Core**: Framework web API chính
- **Entity Framework Core**: ORM cho truy cập cơ sở dữ liệu
- **SQL Server**: Hệ quản trị cơ sở dữ liệu

#### B. Thư viện bảo mật
- **JWT Bearer**: Xác thực token-based
- **BCrypt.Net**: Hash mật khẩu an toàn
- **Microsoft.AspNetCore.Authorization**: Phân quyền

### 2. FRONTEND (React)

#### A. Framework và thư viện chính
- **React 19.1.1**: Framework UI chính
- **TypeScript**: Type safety
- **React Router Dom 7.8.2**: Routing
- **Tailwind CSS**: Styling framework

#### B. Thư viện UI và UX
- **@headlessui/react**: UI components
- **@heroicons/react**: Icon library
- **@tanstack/react-table**: Data table
- **Recharts**: Charts và analytics

---

## KẾT LUẬN VÀ ĐÁNH GIÁ

### 1. ĐIỂM MẠNH HỆ THỐNG

#### A. Kiến trúc rõ ràng
- **Phân tầng rõ ràng**: Controllers → Services → Data Layer
- **Dependency Injection**: Quản lý phụ thuộc tốt
- **Separation of Concerns**: Tách biệt trách nhiệm

#### B. Bảo mật tốt
- **JWT Authentication**: Xác thực token-based
- **Role-based Authorization**: Phân quyền theo vai trò
- **Password Hashing**: Mã hóa mật khẩu BCrypt

#### C. Giao diện hiện đại
- **React 19**: Framework UI mới nhất
- **TypeScript**: Type safety đầy đủ
- **Tailwind CSS**: Thiết kế hiện đại, responsive

### 2. TÍNH NĂNG ĐÁNG CHÚ Ý

#### A. Quản lý chuyên gia y tế
- Hệ thống xác thực chuyên gia chuyên nghiệp
- Quản lý chuyên khoa chi tiết
- Theo dõi đánh giá và phản hồi

#### B. Dashboard thống kê
- Biểu đồ trực quan với Recharts
- Thống kê real-time
- Quick actions tiện lợi

#### C. Tìm kiếm toàn cục
- Search API cho tất cả module
- Gợi ý tìm kiếm thông minh
- Lọc và phân trang nâng cao

### 3. KẾT LUẬN TỔNG QUAN

Hệ thống quản trị diễn đàn y tế được xây dựng với kiến trúc hiện đại, bảo mật tốt và tính năng đầy đủ. Sử dụng công nghệ .NET Core cho backend và React TypeScript cho frontend tạo nên một ứng dụng ổn định, dễ bảo trì và mở rộng.

Hệ thống đáp ứng được các yêu cầu cơ bản của một nền tảng quản trị diễn đàn y tế, bao gồm quản lý người dùng, kiểm duyệt nội dung, xử lý báo cáo và thống kê phân tích. Đặc biệt, tính năng quản lý chuyên gia y tế và hệ thống xác thực chuyên nghiệp là những điểm mạnh nổi bật của hệ thống.

---
