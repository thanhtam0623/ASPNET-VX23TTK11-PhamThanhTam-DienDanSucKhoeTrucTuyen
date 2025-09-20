# PHÂN TÍCH HỆ THỐNG NGƯỜI DÙNG DIỄN ĐÀN Y TẾ

## TỔNG QUAN HỆ THỐNG

Hệ thống người dùng diễn đàn y tế là nền tảng chính cho cộng đồng thảo luận y tế trực tuyến, cung cấp không gian tương tác giữa người dùng thông thường, chuyên gia y tế và các thành viên khác. Hệ thống tập trung vào trải nghiệm người dùng, tính năng thảo luận đa dạng và khả năng truy cập thông tin y tế một cách an toàn và hiệu quả.

---

## TÍNH NĂNG CHI TIẾT HỆ THỐNG

### 1. HỆ THỐNG XÁC THỰC VÀ PHÂN QUYỀN

- **Đăng Ký và Đăng Nhập**: Đăng ký tài khoản với username, email, mật khẩu. Xác thực email qua link kích hoạt. JWT Token cho phiên làm việc.

- **Quản Lý Hồ Sơ**: Cập nhật thông tin cá nhân (tên, avatar, bio). Thiết lập quyền riêng tư. Xem lịch sử hoạt động cá nhân.

### 2. HỆ THỐNG THẢO LUẬN

- **Quản Lý Chủ Đề**: Tạo chủ đề mới với tiêu đề, nội dung, danh mục. Chỉnh sửa và xóa chủ đề của mình. Đánh dấu câu trả lời đúng.

- **Hệ Thống Bài Viết**: Trả lời chủ đề với nội dung HTML được sanitized. Trả lời lồng nhau. Like/unlike bài viết. Báo cáo nội dung vi phạm.

### 3. HỆ THỐNG CHUYÊN GIA

- **Quản Lý Chuyên Gia**: Danh sách chuyên gia theo chuyên khoa. Hồ sơ chi tiết chuyên gia. Thống kê hoạt động và đóng góp.

- **Tương Tác**: Đặt câu hỏi cho chuyên gia cụ thể. Theo dõi chuyên gia yêu thích. Nhận thông báo từ chuyên gia.

### 4. HỖ TRỢ KHẨN CẤP

- **Trang Khẩn Cấp 24/7**: Hướng dẫn sơ cứu cơ bản. Danh sách hotline y tế khẩn cấp. Tìm bệnh viện gần nhất.

---

## SƠ ĐỒ MÔ TẢ BÀI TOÁN HỆ THỐNG

```text
┌─────────────────────────────────────────────────────────────────────────┐
│                    DIỄN ĐÀN Y TẾ - HỆ THỐNG NGƯỜI DÙNG                  │
│                                                                         │
│  ┌─────────────────┐    ┌─────────────────┐    ┌─────────────────────┐  │
│  │   NGƯỜI DÙNG    │    │   CHUYÊN GIA    │    │    KHÁCH TRUY CẬP   │  │
│  │   THÀNH VIÊN    │    │      Y TẾ       │    │     (ANONYMOUS)     │  │
│  └─────────────────┘    └─────────────────┘    └─────────────────────┘  │
│           │                       │                          │          │
│           │                       │                          │          │
│           ▼                       ▼                          ▼          │
│  ┌─────────────────────────────────────────────────────────────────────┐│
│  │                        NỀN TẢNG TƯƠNG TÁC                           ││
│  │                                                                     ││
│  │  • Đăng ký/Đăng nhập thành viên    • Tạo và quản lý chủ đề          ││
│  │  • Quản lý hồ sơ cá nhân          • Trả lời và thảo luận            ││
│  │  • Tìm kiếm thông tin y tế         • Tương tác xã hội (Like/Share)  ││
│  │  • Theo dõi chuyên gia            • Báo cáo nội dung vi phạm        ││
│  │  • Đặt câu hỏi sức khỏe           • Xem thống kê cá nhân            ││
│  └─────────────────────────────────────────────────────────────────────┘│
│                                     │                                   │
│                                     ▼                                   │
│  ┌─────────────────────────────────────────────────────────────────────┐│
│  │                     HỆ THỐNG HỖ TRỢ VÀ TÍNH NĂNG                    ││
│  │                                                                     ││
│  │  • Phân loại theo chuyên khoa      • Tìm kiếm thông minh            ││
│  │  • Đánh giá độ tin cậy             • Lọc nội dung phù hợp           ││
│  │  • Thông báo theo thời gian thực   • Hỗ trợ khẩn cấp 24/7           ││
│  │  • Bảo mật thông tin cá nhân       • Responsive trên mọi thiết bị   ││
│  └─────────────────────────────────────────────────────────────────────┘│
└─────────────────────────────────────────────────────────────────────────┘
```

---

## LƯỢC ĐỒ USE CASE HỆ THỐNG NGƯỜI DÙNG

```
                    BIỂU ĐỒ USE CASE USER SYSTEM

    ┌─────────────────┐                                    ┌─────────────────┐
    │                 │                                    │                 │
    │  KHÁCH TRUY CẬP │                                    │   THÀNH VIÊN    │
    │   (ANONYMOUS)   │                                    │    (MEMBER)     │
    │                 │                                    │                 │
    └─────────────────┘                                    └─────────────────┘
            │                                                       │
            │                                                       │
    ┌───────▼───────────────────────────────────────────────────────▼───────┐
    │                                                                       │
    │                      HỆ THỐNG NGƯỜI DÙNG                              │
    │                                                                       │
    │  ┌─────────────────────────────────────────────────────────────────┐  │
    │  │                    CHỨC NĂNG CÔNG KHAI                          │  │
    │  │                                                                 │  │
    │  │  ◉ Xem trang chủ diễn đàn                                      │  │
    │  │  ◉ Duyệt danh sách chủ đề thảo luận                            │  │
    │  │  ◉ Xem chi tiết chủ đề và bài viết                             │  │
    │  │  ◉ Tìm kiếm thông tin theo từ khóa                             │  │
    │  │  ◉ Duyệt danh mục chuyên khoa y tế                             │  │
    │  │  ◉ Xem danh sách chuyên gia y tế                               │  │
    │  │  ◉ Truy cập trang hỗ trợ khẩn cấp                              │  │
    │  │  ◉ Đăng ký tài khoản mới                                       │  │
    │  │  ◉ Đăng nhập vào hệ thống                                      │  │
    │  └─────────────────────────────────────────────────────────────────┘  │
    │                                                                       │
    │  ┌─────────────────────────────────────────────────────────────────┐  │
    │  │                  QUẢN LÝ TÀI KHOẢN THÀNH VIÊN                   │  │
    │  │                                                                 │  │
    │  │  ◉ Đăng nhập và đăng xuất an toàn                              │  │
    │  │  ◉ Quản lý thông tin hồ sơ cá nhân                             │  │
    │  │  ◉ Cập nhật ảnh đại diện và tiểu sử                            │  │
    │  │  ◉ Thay đổi mật khẩu bảo mật                                   │  │
    │  │  ◉ Cài đặt quyền riêng tư thông tin                            │  │
    │  │  ◉ Quản lý thông báo và tùy chọn                               │  │
    │  │  ◉ Xem lịch sử hoạt động cá nhân                               │  │
    │  │  ◉ Khôi phục mật khẩu qua email                                │  │
    │  └─────────────────────────────────────────────────────────────────┘  │
    │                                                                       │
    │  ┌─────────────────────────────────────────────────────────────────┐  │
    │  │                    TƯƠNG TÁC NỘI DUNG                           │  │
    │  │                                                                 │  │
    │  │  ◉ Tạo chủ đề thảo luận mới                                     │  │
    │  │  ◉ Viết và gửi bài trả lời                                      │  │
    │  │  ◉ Chỉnh sửa bài viết của mình                                  │  │
    │  │  ◉ Xóa bài viết không phù hợp                                   │  │
    │  │  ◉ Like/Unlike chủ đề và bài viết                               │  │
    │  │  ◉ Đánh dấu bài trả lời là câu trả lời đúng                     │  │
    │  │  ◉ Báo cáo nội dung vi phạm quy định                            │  │
    │  │  ◉ Chia sẻ chủ đề trên mạng xã hội                              │  │
    │  │  ◉ Theo dõi chủ đề yêu thích                                    │  │
    │  └─────────────────────────────────────────────────────────────────┘  │
    │                                                                       │
    │  ┌─────────────────────────────────────────────────────────────────┐  │
    │  │                   TÌM KIẾM VÀ KHÁM PHÁ                          │  │
    │  │                                                                 │  │
    │  │  ◉ Tìm kiếm chủ đề theo từ khóa                                │  │
    │  │  ◉ Lọc theo danh mục chuyên khoa                               │  │
    │  │  ◉ Lọc theo thẻ tag và chủ đề                                  │  │
    │  │  ◉ Tìm kiếm nâng cao với nhiều tiêu chí                        │  │
    │  │  ◉ Sắp xếp kết quả theo relevance                              │  │
    │  │  ◉ Lưu tìm kiếm yêu thích                                      │  │
    │  │  ◉ Gợi ý tìm kiếm thông minh                                   │  │
    │  │  ◉ Xem lịch sử tìm kiếm                                        │  │
    │  └─────────────────────────────────────────────────────────────────┘  │
    │                                                                       │
    │  ┌─────────────────────────────────────────────────────────────────┐  │
    │  │                 TƯƠNG TÁC VỚI CHUYÊN GIA                        │  │
    │  │                                                                 │  │
    │  │  ◉ Xem danh sách chuyên gia theo chuyên khoa                   │  │
    │  │  ◉ Xem hồ sơ chi tiết của chuyên gia                           │  │
    │  │  ◉ Đặt câu hỏi trực tiếp cho chuyên gia                        │  │
    │  │  ◉ Đánh giá và review chuyên gia                               │  │
    │  │  ◉ Theo dõi chuyên gia yêu thích                               │  │
    │  │  ◉ Nhận thông báo từ chuyên gia                                │  │
    │  │  ◉ Xem thống kê hoạt động chuyên gia                           │  │
    │  │  ◉ Tìm kiếm chuyên gia theo địa điểm                           │  │
    │  └─────────────────────────────────────────────────────────────────┘  │
    │                                                                       │
    │  ┌─────────────────────────────────────────────────────────────────┐  │
    │  │                    HỖ TRỢ VÀ KHẨN CẤP                           │  │
    │  │                                                                 │  │
    │  │  ◉ Truy cập trang hỗ trợ khẩn cấp 24/7                         │  │
    │  │  ◉ Xem hướng dẫn sơ cứu cơ bản                                 │  │
    │  │  ◉ Liên hệ hotline y tế khẩn cấp                               │  │
    │  │  ◉ Tìm bệnh viện gần nhất                                      │  │
    │  │  ◉ Báo cáo tình huống khẩn cấp                                 │  │
    │  │  ◉ Nhận hướng dẫn xử lý khẩn cấp                               │  │
    │  │  ◉ Truy cập tài liệu y tế cấp cứu                              │  │
    │  └─────────────────────────────────────────────────────────────────┘  │
    │                                                                       │
    └───────────────────────────────────────────────────────────────────────┘

                    ┌─────────────────┐
                    │                 │
                    │   CHUYÊN GIA    │
                    │      Y TẾ       │
                    │                 │
                    └─────────────────┘
                            │
                ┌───────────▼───────────┐
                │                       │
                │   CHỨC NĂNG CHUYÊN    │
                │        GIA            │
                │                       │
                │  ◉ Trả lời chuyên    │
                │    môn cho thành viên │
                │  ◉ Tạo nội dung       │
                │    giáo dục y tế      │
                │  ◉ Xác thực thông     │
                │    tin y tế           │
                │  ◉ Tư vấn trực tuyến │
                │                       │
                └───────────────────────┘
```

---

## KIẾN TRÚC HỆ THỐNG

### 1. KIẾN TRÚC TỔNG QUAN

```text
┌─────────────────────────────────────────────────────────────────────────┐
│                      KIẾN TRÚC HỆ THỐNG NGƯỜI DÙNG                      │
│                                                                         │
│  ┌─────────────────────────────────────────────────────────────────┐    │
│  │                     TẦNG GIAO DIỆN NGƯỜI DÙNG                   │    │
│  │                                                                 │    │
│  │  ┌─────────────────┐    ┌──────────────────┐    ┌─────────────┐ │    │
│  │  │   React 19.1.1  │    │  TypeScript      │    │  Tailwind   │ │    │
│  │  │   User Frontend │    │  Type Safety     │    │     CSS     │ │    │
│  │  └─────────────────┘    └──────────────────┘    └─────────────┘ │    │
│  │                                                                 │    │
│  │  ┌─────────────────┐    ┌──────────────────┐    ┌─────────────┐ │    │
│  │  │   React Router  │    │   Axios HTTP     │    │ Responsive  │ │    │
│  │  │   Navigation    │    │    Client        │    │    Design   │ │    │
│  │  └─────────────────┘    └──────────────────┘    └─────────────┘ │    │
│  └─────────────────────────────────────────────────────────────────┘    │
│                                    │                                     │
│                              HTTP/HTTPS                                  │
│                                    │                                     │
│  ┌─────────────────────────────────▼───────────────────────────────────┐ │
│  │                        TẦNG ỨNG DỤNG BACKEND                        │ │
│  │                                                                     │ │
│  │  ┌─────────────────────────────────────────────────────────────┐    │ │
│  │  │                  ASP.NET CORE USER API                      │    │ │
│  │  │                                                             │    │ │
│  │  │  ┌─────────────┐  ┌─────────────┐  ┌─────────────────────┐  │    │ │
│  │  │  │Controllers  │  │ Services    │  │    JWT Security     │  │    │ │
│  │  │  │  - Auth     │  │ - User      │  │  - Authentication   │  │    │ │
│  │  │  │  - Home     │  │   Logic     │  │  - Authorization    │  │    │ │
│  │  │  │  - Topics   │  │ - Business  │  │  - User Policies    │  │    │ │
│  │  │  │  - Category │  │   Rules     │  │                     │  │    │ │
│  │  │  │  - Profile  │  │ - Validation│  │                     │  │    │ │
│  │  │  │  - Search   │  │             │  │                     │  │    │ │
│  │  │  │  - Expert   │  │             │  │                     │  │    │ │
│  │  │  └─────────────┘  └─────────────┘  └─────────────────────┘  │    │ │
│  │  └─────────────────────────────────────────────────────────────┘    │ │
│  └─────────────────────────────────────────────────────────────────────┘ │
│                                    │                                     │
│                                EF Core                                   │
│                                    │                                     │
│  ┌─────────────────────────────────▼───────────────────────────────────┐ │
│  │                          TẦNG DỮ LIỆU                               │ │
│  │                                                                     │ │
│  │  ┌─────────────────────────────────────────────────────────────┐    │ │
│  │  │                    SQL SERVER DATABASE                      │    │ │
│  │  │                                                             │    │ │
│  │  │  ┌─────────────┐  ┌─────────────┐  ┌─────────────────────┐  │    │ │
│  │  │  │   Users     │  │  Topics     │  │     Categories      │  │    │ │
│  │  │  │   Posts     │  │  Likes      │  │       Tags          │  │    │ │
│  │  │  │   Reports   │  │  Views      │  │    TopicTags        │  │    │ │
│  │  │  │   Experts   │  │  Profiles   │  │    Specialties      │  │    │ │
│  │  │  └─────────────┘  └─────────────┘  └─────────────────────┘  │    │ │
│  │  └─────────────────────────────────────────────────────────────┘    │ │
│  └─────────────────────────────────────────────────────────────────────┘ │
└─────────────────────────────────────────────────────────────────────────┘
```

---

## CÔNG NGHỆ VÀ KIẾN TRÚC

### 1. BACKEND TECHNOLOGY STACK

- **Framework**: ASP.NET Core 8.0, Entity Framework Core
- **Database**: SQL Server
- **Authentication**: JWT Token-based
- **Libraries**: HtmlSanitizer, BCrypt.Net, FluentValidation, AutoMapper

### 2. FRONTEND TECHNOLOGY STACK

- **Core Framework**: React 19.1.1, TypeScript 5.7.2
- **Routing**: React Router Dom 7.8.2  
- **HTTP Client**: Axios 1.7.9
- **UI & Styling**: Tailwind CSS 4.0.0, Responsive Design
- **Build Tool**: Vite 6.0.7

### 3. KIẾN TRÚC BẢO MẬT

- **Authentication**: JWT Token-based với Role-based Authorization
- **Data Security**: HTML Sanitization, SQL Injection Prevention, XSS Protection
- **Privacy**: User Privacy Settings, GDPR Compliance Ready

---

## ĐÁNH GIÁ VÀ KẾT LUẬN

### 1. ĐIỂM MẠNH HỆ THỐNG

- **Kiến Trúc Vững Chắc**: Phân tách rõ ràng Frontend/Backend, Layered Architecture
- **Trải Nghiệm Người Dùng**: Giao diện hiện đại React 19, Responsive design
- **Tính Năng Đa Dạng**: Hỗ trợ đầy đủ tính năng diễn đàn y tế, hệ thống chuyên gia
- **Bảo Mật Cao**: JWT authentication, HTML sanitization, Role-based authorization

