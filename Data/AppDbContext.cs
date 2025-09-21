using ApiApplication.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApiApplication.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Admin Tables
        public DbSet<AdminAccount> AdminAccounts { get; set; }

        // User Tables
        public DbSet<UserAccount> UserAccounts { get; set; }

        // Content Tables
        public DbSet<Category> Categories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<TopicTag> TopicTags { get; set; }

        // Interaction Tables
        public DbSet<Like> Likes { get; set; }
        public DbSet<TopicView> TopicViews { get; set; }
        public DbSet<Report> Reports { get; set; }

        // System Tables
        public DbSet<StaticPage> StaticPages { get; set; }
        
        // Expert Tables
        public DbSet<Specialty> Specialties { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Admin Accounts
            modelBuilder.Entity<AdminAccount>(entity =>
            {
                entity.HasIndex(e => e.Username).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
            });

            // Configure User Accounts
            modelBuilder.Entity<UserAccount>(entity =>
            {
                entity.HasIndex(e => e.Username).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
            });

            // Configure Categories
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasIndex(e => e.Slug).IsUnique();
                entity.HasIndex(e => e.DisplayOrder);
            });

            // Configure Tags
            modelBuilder.Entity<Tag>(entity =>
            {
                entity.HasIndex(e => e.Slug).IsUnique();
                entity.HasIndex(e => e.Name).IsUnique();
            });

            // Configure Topics
            modelBuilder.Entity<Topic>(entity =>
            {
                entity.HasIndex(e => e.Slug).IsUnique();
                entity.HasIndex(e => e.CategoryId);
                entity.HasIndex(e => e.UserId);
                entity.HasIndex(e => e.CreatedAt);
                entity.HasIndex(e => e.IsPinned);
                entity.HasIndex(e => e.IsActive);

                entity.HasOne(t => t.Category)
                    .WithMany(c => c.Topics)
                    .HasForeignKey(t => t.CategoryId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(t => t.User)
                    .WithMany(u => u.Topics)
                    .HasForeignKey(t => t.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure Posts
            modelBuilder.Entity<Post>(entity =>
            {
                entity.HasIndex(e => e.TopicId);
                entity.HasIndex(e => e.UserId);
                entity.HasIndex(e => e.ParentPostId);
                entity.HasIndex(e => e.CreatedAt);
                entity.HasIndex(e => e.IsActive);

                entity.HasOne(p => p.Topic)
                    .WithMany(t => t.Posts)
                    .HasForeignKey(p => p.TopicId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(p => p.User)
                    .WithMany(u => u.Posts)
                    .HasForeignKey(p => p.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(p => p.ParentPost)
                    .WithMany(p => p.Replies)
                    .HasForeignKey(p => p.ParentPostId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure TopicTags
            modelBuilder.Entity<TopicTag>(entity =>
            {
                entity.HasIndex(e => new { e.TopicId, e.TagId }).IsUnique();

                entity.HasOne(tt => tt.Topic)
                    .WithMany(t => t.TopicTags)
                    .HasForeignKey(tt => tt.TopicId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(tt => tt.Tag)
                    .WithMany(t => t.TopicTags)
                    .HasForeignKey(tt => tt.TagId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure Likes
            modelBuilder.Entity<Like>(entity =>
            {
                entity.HasIndex(e => new { e.UserId, e.TopicId, e.PostId }).IsUnique();
                entity.HasIndex(e => e.UserId);
                entity.HasIndex(e => e.TopicId);
                entity.HasIndex(e => e.PostId);

                entity.HasOne(l => l.User)
                    .WithMany(u => u.Likes)
                    .HasForeignKey(l => l.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(l => l.Topic)
                    .WithMany(t => t.Likes)
                    .HasForeignKey(l => l.TopicId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(l => l.Post)
                    .WithMany(p => p.Likes)
                    .HasForeignKey(l => l.PostId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure TopicViews
            modelBuilder.Entity<TopicView>(entity =>
            {
                entity.HasIndex(e => e.TopicId);
                entity.HasIndex(e => e.UserId);
                entity.HasIndex(e => e.ViewedAt);

                entity.HasOne(tv => tv.Topic)
                    .WithMany(t => t.Views)
                    .HasForeignKey(tv => tv.TopicId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(tv => tv.User)
                    .WithMany()
                    .HasForeignKey(tv => tv.UserId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Configure Reports
            modelBuilder.Entity<Report>(entity =>
            {
                entity.HasIndex(e => e.UserId);
                entity.HasIndex(e => e.TopicId);
                entity.HasIndex(e => e.PostId);
                entity.HasIndex(e => e.Status);
                entity.HasIndex(e => e.Category);
                entity.HasIndex(e => e.CreatedAt);

                entity.HasOne(r => r.User)
                    .WithMany(u => u.Reports)
                    .HasForeignKey(r => r.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(r => r.Topic)
                    .WithMany(t => t.Reports)
                    .HasForeignKey(r => r.TopicId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(r => r.Post)
                    .WithMany(p => p.Reports)
                    .HasForeignKey(r => r.PostId)
                    .OnDelete(DeleteBehavior.Cascade);
            });


            // Configure StaticPages
            modelBuilder.Entity<StaticPage>(entity =>
            {
                entity.HasIndex(e => e.Slug).IsUnique();
            });

            // Seed Default Data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            var random = new Random(42); // Fixed seed for consistent data
            var startDate = new DateTime(2025, 9, 1, 0, 0, 0, DateTimeKind.Utc);
            var endDate = new DateTime(2025, 9, 18, 23, 59, 59, DateTimeKind.Utc);

            DateTime GetRandomDate()
            {
                var range = endDate - startDate;
                var randomDays = random.NextDouble() * range.TotalDays;
                return startDate.AddDays(randomDays);
            }

            // Seed Admin Account (only 1)
            modelBuilder.Entity<AdminAccount>().HasData(
                new AdminAccount
                {
                    Id = 1,
                    Username = "admin",
                    Email = "admin@example.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"), // admin123
                    FullName = "Quản trị viên hệ thống",
                    Role = "SuperAdmin",
                    IsActive = true,
                    CreatedAt = GetRandomDate(),
                    UpdatedAt = GetRandomDate()
                }
            );

            // Seed 30 Categories
            var categories = new List<Category>();
            var categoryNames = new[] { 
                "Sức khỏe tổng quát", "Tim mạch", "Da liễu", "Xương khớp", "Nhi khoa", 
                "Thần kinh", "Tâm thần", "Ung thư", "Phụ khoa", "Mắt", 
                "Tai mũi họng", "Nha khoa", "Dinh dưỡng", "Tập luyện", "Y học cổ truyền", 
                "Dược phẩm", "Cấp cứu", "Phục hồi chức năng", "Lão khoa", "Y học gia đình",
                "Hô hấp", "Tiêu hóa", "Nội tiết", "Miễn dịch", "Huyết học",
                "Thận - Tiết niệu", "Gan mật", "Chấn thương chỉnh hình", "Phẫu thuật", "Gây mê hồi sức"
            };
            for (int i = 1; i <= 30; i++)
            {
                var createdDate = GetRandomDate();
                categories.Add(new Category
                {
                    Id = i,
                    Name = categoryNames[i - 1],
                    Slug = $"danh-muc-{i}",
                    Description = $"Thông tin và thảo luận về {categoryNames[i - 1].ToLower()}",
                    Icon = $"icon-{i}",
                    Color = $"#{random.Next(0x1000000):X6}",
                    DisplayOrder = i,
                    IsActive = true,
                    CreatedAt = createdDate,
                    UpdatedAt = createdDate,
                    TopicCount = 0,
                    PostCount = 0
                });
            }
            modelBuilder.Entity<Category>().HasData(categories);

            // Seed 30 Tags
            var tags = new List<Tag>();
            var tagColors = new[] { "#007bff", "#28a745", "#ffc107", "#dc3545", "#6f42c1", "#fd7e14", "#20c997", "#17a2b8", "#6c757d", "#343a40" };
            var tagNames = new[] { 
                "Câu hỏi", "Thảo luận", "Trợ giúp", "Thông báo", "Khẩn cấp", 
                "Tư vấn", "Chia sẻ", "Kinh nghiệm", "Phòng bệnh", "Điều trị", 
                "Thuốc men", "Dinh dưỡng", "Tập luyện", "Sức khỏe tâm thần", "Mẹ và bé", 
                "Người cao tuổi", "Y học cổ truyền", "Cấp cứu", "Bảo hiểm y tế", "Khám sức khỏe",
                "Lời khuyên", "Góp ý", "Hướng dẫn", "Mẹo hay", "Lưu ý",
                "Cảnh báo", "An toàn", "Hiệu quả", "Tự nhiên", "Khoa học"
            };
            for (int i = 1; i <= 30; i++)
            {
                tags.Add(new Tag
                {
                    Id = i,
                    Name = tagNames[i - 1],
                    Slug = $"the-{i}",
                    Color = tagColors[i % tagColors.Length],
                    CreatedAt = GetRandomDate(),
                    TopicCount = 0
                });
            }
            modelBuilder.Entity<Tag>().HasData(tags);

            // Seed 30 Specialties
            var specialties = new List<Specialty>();
            var specialtyNames = new[] { 
                "Bác sĩ đa khoa", "Tim mạch", "Da liễu", "Xương khớp", "Nhi khoa", 
                "Thần kinh", "Tâm thần", "Ung bướu", "Phụ sản", "Mắt", 
                "Tai mũi họng", "Chẩn đoán hình ảnh", "Giải phẫu bệnh", "Gây mê hồi sức", "Phẫu thuật", 
                "Cấp cứu", "Y học gia đình", "Nội khoa", "Nội tiết", "Tiêu hóa",
                "Hô hấp", "Thận - Tiết niệu", "Gan mật", "Huyết học", "Miễn dịch",
                "Lão khoa", "Y học cổ truyền", "Dinh dưỡng", "Phục hồi chức năng", "Y học thể thao"
            };
            for (int i = 1; i <= 30; i++)
            {
                var createdDate = GetRandomDate();
                specialties.Add(new Specialty
                {
                    Id = i,
                    Name = specialtyNames[i - 1],
                    Description = $"Chuyên khoa {specialtyNames[i - 1].ToLower()} - Cung cấp dịch vụ chăm sóc y tế chuyên nghiệp",
                    IsActive = true,
                    CreatedAt = createdDate,
                    UpdatedAt = createdDate
                });
            }
            modelBuilder.Entity<Specialty>().HasData(specialties);

            // Seed 30 Static Pages
            var staticPages = new List<StaticPage>();
            var pageNames = new[] { 
                "Điều khoản sử dụng", "Chính sách bảo mật", "Về chúng tôi", "Liên hệ", "Câu hỏi thường gặp", 
                "Hướng dẫn sử dụng", "Quy tắc cộng đồng", "Chính sách cookie", "Tuyên bố pháp lý", "Bảo mật thông tin", 
                "Hỗ trợ khách hàng", "Điều khoản dịch vụ", "Quyền riêng tư", "Báo cáo lỗi", "Góp ý", 
                "Tài liệu hướng dẫn", "Chính sách hoàn tiền", "Điều kiện sử dụng", "Thông tin pháp lý", "Cam kết chất lượng",
                "Hướng dẫn đăng ký", "Cách thức hoạt động", "Quy định đăng bài", "Chính sách kiểm duyệt", "Hướng dẫn bảo mật tài khoản",
                "Quy trình xử lý khiếu nại", "Chính sách sử dụng dữ liệu", "Hướng dẫn tương tác", "Nguyên tắc cộng đồng", "Chính sách nội dung"
            };
            for (int i = 1; i <= 30; i++)
            {
                var createdDate = GetRandomDate();
                staticPages.Add(new StaticPage
                {
                    Id = i,
                    Title = pageNames[i - 1],
                    Slug = $"trang-tinh-{i}",
                    Content = $"<h1>{pageNames[i - 1]}</h1><p>Nội dung chi tiết về {pageNames[i - 1].ToLower()}. Đây là thông tin quan trọng mà người dùng cần biết khi sử dụng hệ thống.</p>",
                    IsActive = true,
                    CreatedAt = createdDate,
                    UpdatedAt = createdDate,
                    UpdatedByAdminId = 1
                });
            }
            modelBuilder.Entity<StaticPage>().HasData(staticPages);

            // Seed 30 User Accounts
            var userAccounts = new List<UserAccount>();
            var roles = new[] { "Thành viên", "Bác sĩ", "Kiểm duyệt viên" };
            var vietnameseNames = new[] { 
                "Nguyễn Văn An", "Trần Thị Bình", "Lê Văn Cường", "Phạm Thị Dung", "Hoàng Văn Em", 
                "Vũ Thị Phương", "Đặng Văn Giang", "Bùi Thị Hoa", "Dương Văn Inh", "Đinh Thị Kim", 
                "Phan Văn Long", "Võ Thị Mai", "Đỗ Văn Nam", "Chu Thị Oanh", "Triệu Văn Phúc", 
                "Lý Thị Quỳnh", "Tô Văn Rực", "Ngô Thị Sương", "Lương Văn Tuấn", "Tạ Thị Uyên",
                "Cao Văn Bảo", "Đào Thị Chi", "Lưu Văn Đức", "Mai Thị Ế", "Nghiêm Văn Phát",
                "Ông Thị Giang", "Quách Văn Hùng", "Sử Thị Vy", "Thái Văn Khôi", "Ứng Thị Linh"
            };
            for (int i = 1; i <= 30; i++)
            {
                var createdDate = GetRandomDate();
                var lastLogin = GetRandomDate();
                userAccounts.Add(new UserAccount
                {
                    Id = i,
                    Username = $"nguoidung{i}",
                    Email = $"nguoidung{i}@email.com",
                    PasswordHash = "$2a$11$ZJN7y.5DKhZzYN2R5ZHE8O9Kb8kzGzV8jGqZ9v1E8z.8K.8h8Z8h8Z",
                    FullName = vietnameseNames[i - 1],
                    Avatar = null,
                    Bio = $"Xin chào, tôi là {vietnameseNames[i - 1]}. Rất vui được tham gia cộng đồng sức khỏe để học hỏi và chia sẻ kiến thức.",
                    Role = roles[i % roles.Length],
                    IsActive = true,
                    IsEmailVerified = i % 2 == 0,
                    ShowEmail = i % 3 == 0,
                    ShowBio = true,
                    CreatedAt = createdDate,
                    UpdatedAt = createdDate,
                    LastLoginAt = lastLogin > createdDate ? lastLogin : createdDate.AddHours(random.Next(1, 72)),
                    ResetPasswordToken = null,
                    ResetPasswordExpires = null
                });
            }
            modelBuilder.Entity<UserAccount>().HasData(userAccounts);

            // Seed 30 Topics
            var topics = new List<Topic>();
            var topicTitles = new[] { 
                "Cách phòng ngừa cảm cúm mùa đông hiệu quả", "Chế độ ăn uống lành mạnh cho người tiểu đường", "Bài tập thể dục phù hợp cho người cao tuổi", 
                "Triệu chứng và cách điều trị đau đầu thường gặp", "Tầm quan trọng của việc khám sức khỏe định kỳ", "Cách chăm sóc da trong mùa khô hanh", 
                "Phương pháp giảm stress tự nhiên và hiệu quả", "Chế độ dinh dưỡng cân bằng cho trẻ em", "Cách nhận biết và xử lý cơn đau tim", 
                "Tác hại của việc thức khuya đối với sức khỏe", "Lợi ích của việc uống đủ nước mỗi ngày", "Cách chăm sóc răng miệng đúng cách", 
                "Phòng ngừa bệnh cao huyết áp từ sớm", "Tác động nghiêm trọng của thuốc lá", "Cách giữ gìn sức khỏe tim mạch", 
                "Chế độ ăn dành cho người bị dạ dày", "Tầm quan trọng của giấc ngủ đối với sức khỏe", "Cách phòng ngừa bệnh ung thư hiệu quả", 
                "Chăm sóc sức khỏe bà mẹ mang thai", "Tác dụng của việc tập yoga đối với sức khỏe", "Lợi ích của việc ăn rau xanh hàng ngày",
                "Cách phòng ngừa bệnh tiểu đường tuýp 2", "Chăm sóc sức khỏe răng miệng cho trẻ em", "Phương pháp tăng cường hệ miễn dịch", 
                "Cách điều trị và phòng ngừa đau lưng", "Lợi ích của việc đi bộ hàng ngày", "Chế độ ăn giúp giảm cholesterol",
                "Cách nhận biết và xử lý chứng mất ngủ", "Phương pháp chăm sóc da mặt tự nhiên", "Tác dụng của vitamin D với sức khỏe"
            };
            for (int i = 1; i <= 30; i++)
            {
                var createdDate = GetRandomDate();
                var updatedDate = GetRandomDate();
                var lastActivityDate = GetRandomDate();
                topics.Add(new Topic
                {
                    Id = i,
                    Title = topicTitles[i - 1],
                    Slug = $"chu-de-{i}-thao-luan-suc-khoe",
                    Content = $"<p>Đây là nội dung chi tiết về chủ đề '{topicTitles[i - 1]}'. Bài viết cung cấp thông tin hữu ích và kiến thức y học dựa trên khoa học để giúp mọi người hiểu rõ hơn về vấn đề sức khỏe quan trọng này. Nội dung được biên soạn bởi các chuyên gia y tế có kinh nghiệm.</p>",
                    CategoryId = (i % 30) + 1,
                    UserId = (i % 30) + 1,
                    IsActive = true,
                    IsPinned = i <= 5,
                    IsLocked = false,
                    HasAnswer = i % 7 == 0,
                    ViewCount = random.Next(10, 500),
                    LikeCount = random.Next(1, 50),
                    PostCount = random.Next(0, 20),
                    CreatedAt = createdDate,
                    UpdatedAt = updatedDate > createdDate ? updatedDate : createdDate,
                    LastActivityAt = lastActivityDate > createdDate ? lastActivityDate : createdDate.AddHours(random.Next(1, 48))
                });
            }
            modelBuilder.Entity<Topic>().HasData(topics);

            // Seed 30 Posts
            var posts = new List<Post>();
            var postContents = new[] { 
                "Cảm ơn bạn đã chia sẻ thông tin hữu ích này!", "Tôi cũng gặp vấn đề tương tự, lời khuyên của bạn rất hay.", 
                "Bác sĩ có thể tư vấn thêm về vấn đề này không ạ?", "Theo kinh nghiệm của tôi thì phương pháp này khá hiệu quả.", 
                "Xin chào, tôi muốn hỏi thêm về triệu chứng này.", "Rất hữu ích, tôi sẽ áp dụng ngay. Cảm ơn bạn!", 
                "Tôi nghĩ nên tham khảo ý kiến bác sĩ chuyên khoa.", "Điều này hoàn toàn đúng, tôi đã trải nghiệm rồi.", 
                "Có ai biết thêm thông tin về vấn đề này không?", "Bài viết rất chi tiết và dễ hiểu. Cảm ơn tác giả!", 
                "Tôi sẽ thử áp dụng và chia sẻ kết quả sau.", "Phương pháp này có phù hợp với trẻ em không ạ?", 
                "Cần lưu ý gì khi thực hiện theo hướng dẫn này?", "Tôi có thể tìm hiểu thêm ở đâu về chủ đề này?", 
                "Rất cảm ơn những chia sẻ quý báu này.", "Có tác dụng phụ nào cần chú ý không?", 
                "Tôi đã thử và thấy có cải thiện đáng kể.", "Bác sĩ nào có thể tư vấn trực tiếp về vấn đề này?", 
                "Thông tin này rất cần thiết cho mọi người.", "Tôi sẽ chia sẻ với gia đình và bạn bè.",
                "Cảm ơn bạn đã dành thời gian chia sẻ kiến thức.", "Tôi thấy cách này rất thực tế và dễ áp dụng.",
                "Bài viết này giải đáp được nhiều thắc mắc của tôi.", "Theo tôi nên kết hợp với phương pháp truyền thống.",
                "Có thể cho biết nguồn tham khảo để tôi tìm hiểu thêm?", "Kinh nghiệm của bạn rất bổ ích, cảm ơn nhé!",
                "Tôi đã thử và kết quả tốt hơn mong đợi.", "Phương pháp này có thể áp dụng lâu dài không?",
                "Cần bao lâu để thấy hiệu quả của phương pháp này?", "Có cần chế độ ăn đặc biệt kèm theo không?"
            };
            for (int i = 1; i <= 30; i++)
            {
                var createdDate = GetRandomDate();
                posts.Add(new Post
                {
                    Id = i,
                    Content = $"<p>{postContents[i - 1]} Đây là bình luận chi tiết về chủ đề đang được thảo luận. Tôi hy vọng thông tin này sẽ hữu ích cho cộng đồng.</p>",
                    TopicId = (i % 30) + 1,
                    UserId = (i % 30) + 1,
                    ParentPostId = i > 15 ? ((i - 15) % 15) + 1 : null,
                    IsActive = true,
                    IsAnswer = i % 10 == 0,
                    LikeCount = random.Next(0, 25),
                    CreatedAt = createdDate,
                    UpdatedAt = createdDate
                });
            }
            modelBuilder.Entity<Post>().HasData(posts);

            // Seed 30 Topic Tags
            var topicTags = new List<TopicTag>();
            for (int i = 1; i <= 30; i++)
            {
                topicTags.Add(new TopicTag
                {
                    Id = i,
                    TopicId = (i % 30) + 1,
                    TagId = (i % 30) + 1,
                    CreatedAt = GetRandomDate()
                });
            }
            modelBuilder.Entity<TopicTag>().HasData(topicTags);

            // Seed 30 Likes
            var likes = new List<Like>();
            for (int i = 1; i <= 30; i++)
            {
                likes.Add(new Like
                {
                    Id = i,
                    UserId = (i % 30) + 1,
                    TopicId = i <= 15 ? i : null,
                    PostId = i > 15 ? i - 15 : null,
                    CreatedAt = GetRandomDate()
                });
            }
            modelBuilder.Entity<Like>().HasData(likes);

            // Seed 30 Topic Views
            var topicViews = new List<TopicView>();
            for (int i = 1; i <= 30; i++)
            {
                topicViews.Add(new TopicView
                {
                    Id = i,
                    TopicId = (i % 30) + 1,
                    UserId = i <= 20 ? (i % 30) + 1 : null,
                    IpAddress = $"192.168.{random.Next(1, 255)}.{random.Next(1, 255)}",
                    ViewedAt = GetRandomDate()
                });
            }
            modelBuilder.Entity<TopicView>().HasData(topicViews);

            // Seed 30 Reports
            var reports = new List<Report>();
            var reportCategories = new[] { "Thư rác", "Nội dung không phù hợp", "Quấy rối", "Vi phạm bản quyền", "Khác" };
            var reportStatuses = new[] { "Chờ xử lý", "Đã xem xét", "Đã giải quyết", "Đã từ chối" };
            var reportReasons = new[] { 
                "Nội dung thư rác quảng cáo", "Sử dụng ngôn từ không phù hợp", "Quấy rối người dùng khác", "Sao chép nội dung từ nguồn khác", 
                "Thông tin sai lệch về y tế", "Bài viết không liên quan chủ đề", "Ngôn từ thù địch", "Chia sẻ thông tin cá nhân", 
                "Nội dung có tính chất thương mại", "Vi phạm quy tắc cộng đồng", "Đăng nhiều bài trùng lặp", "Sử dụng hình ảnh không phù hợp", 
                "Tuyên truyền thuốc không rõ nguồn gốc", "Phát tán tin đồn sai sự thật", "Bình luận mang tính chất xúc phạm", "Chia sẻ liên kết độc hại", 
                "Giả mạo chuyên gia y tế", "Nội dung có thể gây hoang mang", "Vi phạm điều khoản sử dụng", "Báo cáo sai mục đích",
                "Nội dung không đúng sự thật", "Thông tin y tế nguy hiểm", "Quảng cáo thuốc trái phép", "Xúc phạm người khác",
                "Nội dung phân biệt đối xử", "Chia sẻ thông tin riêng tư", "Bài viết lặp lại nhiều lần", "Hình ảnh không phù hợp",
                "Liên kết đáng ngờ", "Tư vấn y tế không có căn cứ"
            };
            for (int i = 1; i <= 30; i++)
            {
                var createdDate = GetRandomDate();
                var reviewedDate = GetRandomDate();
                reports.Add(new Report
                {
                    Id = i,
                    UserId = (i % 30) + 1,
                    TopicId = i <= 15 ? i : null,
                    PostId = i > 15 ? i - 15 : null,
                    Category = reportCategories[i % reportCategories.Length],
                    Reason = reportReasons[i - 1],
                    Status = reportStatuses[i % reportStatuses.Length],
                    CreatedAt = createdDate,
                    ReviewedAt = i % 3 == 0 ? (reviewedDate > createdDate ? reviewedDate : createdDate.AddHours(random.Next(1, 48))) : null,
                    ReviewedByAdminId = i % 3 == 0 ? 1 : null
                });
            }
            modelBuilder.Entity<Report>().HasData(reports);
        }
    }
}
