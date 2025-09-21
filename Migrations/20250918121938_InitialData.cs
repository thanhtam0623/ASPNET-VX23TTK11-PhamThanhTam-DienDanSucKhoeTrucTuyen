using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ApiApplication.Migrations
{
    /// <inheritdoc />
    public partial class InitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdminAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: false),
                    FullName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Avatar = table.Column<string>(type: "TEXT", nullable: true),
                    Role = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastLoginAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminAccounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Slug = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Icon = table.Column<string>(type: "TEXT", nullable: true),
                    Color = table.Column<string>(type: "TEXT", nullable: true),
                    DisplayOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TopicCount = table.Column<int>(type: "INTEGER", nullable: false),
                    PostCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Specialties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specialties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StaticPages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Slug = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Content = table.Column<string>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedByAdminId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaticPages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Slug = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Color = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TopicCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: false),
                    FullName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Avatar = table.Column<string>(type: "TEXT", nullable: true),
                    Bio = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Role = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsEmailVerified = table.Column<bool>(type: "INTEGER", nullable: false),
                    ShowEmail = table.Column<bool>(type: "INTEGER", nullable: false),
                    ShowBio = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastLoginAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ResetPasswordToken = table.Column<string>(type: "TEXT", nullable: true),
                    ResetPasswordExpires = table.Column<DateTime>(type: "TEXT", nullable: true),
                    SpecialtyId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAccounts_Specialties_SpecialtyId",
                        column: x => x.SpecialtyId,
                        principalTable: "Specialties",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Topics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Slug = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Content = table.Column<string>(type: "TEXT", nullable: false),
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsPinned = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsLocked = table.Column<bool>(type: "INTEGER", nullable: false),
                    HasAnswer = table.Column<bool>(type: "INTEGER", nullable: false),
                    ViewCount = table.Column<int>(type: "INTEGER", nullable: false),
                    LikeCount = table.Column<int>(type: "INTEGER", nullable: false),
                    PostCount = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastActivityAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Topics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Topics_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Topics_UserAccounts_UserId",
                        column: x => x.UserId,
                        principalTable: "UserAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Content = table.Column<string>(type: "TEXT", nullable: false),
                    TopicId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    ParentPostId = table.Column<int>(type: "INTEGER", nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsAnswer = table.Column<bool>(type: "INTEGER", nullable: false),
                    LikeCount = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Posts_Posts_ParentPostId",
                        column: x => x.ParentPostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Posts_Topics_TopicId",
                        column: x => x.TopicId,
                        principalTable: "Topics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Posts_UserAccounts_UserId",
                        column: x => x.UserId,
                        principalTable: "UserAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TopicTags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TopicId = table.Column<int>(type: "INTEGER", nullable: false),
                    TagId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TopicTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TopicTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TopicTags_Topics_TopicId",
                        column: x => x.TopicId,
                        principalTable: "Topics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TopicViews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TopicId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    IpAddress = table.Column<string>(type: "TEXT", nullable: true),
                    ViewedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TopicViews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TopicViews_Topics_TopicId",
                        column: x => x.TopicId,
                        principalTable: "Topics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TopicViews_UserAccounts_UserId",
                        column: x => x.UserId,
                        principalTable: "UserAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Likes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    TopicId = table.Column<int>(type: "INTEGER", nullable: true),
                    PostId = table.Column<int>(type: "INTEGER", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Likes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Likes_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Likes_Topics_TopicId",
                        column: x => x.TopicId,
                        principalTable: "Topics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Likes_UserAccounts_UserId",
                        column: x => x.UserId,
                        principalTable: "UserAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    TopicId = table.Column<int>(type: "INTEGER", nullable: true),
                    PostId = table.Column<int>(type: "INTEGER", nullable: true),
                    Category = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Reason = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Status = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ReviewedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ReviewedByAdminId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reports_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reports_Topics_TopicId",
                        column: x => x.TopicId,
                        principalTable: "Topics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reports_UserAccounts_UserId",
                        column: x => x.UserId,
                        principalTable: "UserAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AdminAccounts",
                columns: new[] { "Id", "Avatar", "CreatedAt", "Email", "FullName", "IsActive", "LastLoginAt", "PasswordHash", "Role", "UpdatedAt", "Username" },
                values: new object[] { 1, null, new DateTime(2025, 9, 13, 0, 37, 18, 507, DateTimeKind.Utc).AddTicks(6791), "admin@example.com", "Quản trị viên hệ thống", true, null, "$2a$11$eZAkSS9FRi7fhwk3l0RQ5.efyvoQc3imPo2QbBdd5GYI66qeRnR.m", "SuperAdmin", new DateTime(2025, 9, 3, 12, 52, 18, 889, DateTimeKind.Utc).AddTicks(5231), "admin" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Color", "CreatedAt", "Description", "DisplayOrder", "Icon", "IsActive", "Name", "PostCount", "Slug", "TopicCount", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "#85D3E1", new DateTime(2025, 9, 3, 6, 13, 25, 918, DateTimeKind.Utc).AddTicks(2392), "Thông tin và thảo luận về sức khỏe tổng quát", 1, "icon-1", true, "Sức khỏe tổng quát", 0, "danh-muc-1", 0, new DateTime(2025, 9, 3, 6, 13, 25, 918, DateTimeKind.Utc).AddTicks(2392) },
                    { 2, "#433946", new DateTime(2025, 9, 4, 0, 45, 48, 736, DateTimeKind.Utc).AddTicks(9948), "Thông tin và thảo luận về tim mạch", 2, "icon-2", true, "Tim mạch", 0, "danh-muc-2", 0, new DateTime(2025, 9, 4, 0, 45, 48, 736, DateTimeKind.Utc).AddTicks(9948) },
                    { 3, "#834EE8", new DateTime(2025, 9, 14, 0, 56, 39, 164, DateTimeKind.Utc).AddTicks(4141), "Thông tin và thảo luận về da liễu", 3, "icon-3", true, "Da liễu", 0, "danh-muc-3", 0, new DateTime(2025, 9, 14, 0, 56, 39, 164, DateTimeKind.Utc).AddTicks(4141) },
                    { 4, "#C2E151", new DateTime(2025, 9, 4, 3, 1, 2, 126, DateTimeKind.Utc).AddTicks(7602), "Thông tin và thảo luận về xương khớp", 4, "icon-4", true, "Xương khớp", 0, "danh-muc-4", 0, new DateTime(2025, 9, 4, 3, 1, 2, 126, DateTimeKind.Utc).AddTicks(7602) },
                    { 5, "#41DFD0", new DateTime(2025, 9, 5, 5, 20, 31, 735, DateTimeKind.Utc).AddTicks(7783), "Thông tin và thảo luận về nhi khoa", 5, "icon-5", true, "Nhi khoa", 0, "danh-muc-5", 0, new DateTime(2025, 9, 5, 5, 20, 31, 735, DateTimeKind.Utc).AddTicks(7783) },
                    { 6, "#51FACD", new DateTime(2025, 9, 10, 2, 25, 14, 127, DateTimeKind.Utc).AddTicks(353), "Thông tin và thảo luận về thần kinh", 6, "icon-6", true, "Thần kinh", 0, "danh-muc-6", 0, new DateTime(2025, 9, 10, 2, 25, 14, 127, DateTimeKind.Utc).AddTicks(353) },
                    { 7, "#429F64", new DateTime(2025, 9, 7, 20, 34, 43, 561, DateTimeKind.Utc).AddTicks(9490), "Thông tin và thảo luận về tâm thần", 7, "icon-7", true, "Tâm thần", 0, "danh-muc-7", 0, new DateTime(2025, 9, 7, 20, 34, 43, 561, DateTimeKind.Utc).AddTicks(9490) },
                    { 8, "#090AA7", new DateTime(2025, 9, 10, 7, 32, 19, 432, DateTimeKind.Utc).AddTicks(9993), "Thông tin và thảo luận về ung thư", 8, "icon-8", true, "Ung thư", 0, "danh-muc-8", 0, new DateTime(2025, 9, 10, 7, 32, 19, 432, DateTimeKind.Utc).AddTicks(9993) },
                    { 9, "#93C32C", new DateTime(2025, 9, 15, 15, 42, 25, 632, DateTimeKind.Utc).AddTicks(7487), "Thông tin và thảo luận về phụ khoa", 9, "icon-9", true, "Phụ khoa", 0, "danh-muc-9", 0, new DateTime(2025, 9, 15, 15, 42, 25, 632, DateTimeKind.Utc).AddTicks(7487) },
                    { 10, "#26F620", new DateTime(2025, 9, 8, 3, 42, 25, 745, DateTimeKind.Utc).AddTicks(9169), "Thông tin và thảo luận về mắt", 10, "icon-10", true, "Mắt", 0, "danh-muc-10", 0, new DateTime(2025, 9, 8, 3, 42, 25, 745, DateTimeKind.Utc).AddTicks(9169) },
                    { 11, "#B484D9", new DateTime(2025, 9, 2, 14, 55, 43, 41, DateTimeKind.Utc).AddTicks(4203), "Thông tin và thảo luận về tai mũi họng", 11, "icon-11", true, "Tai mũi họng", 0, "danh-muc-11", 0, new DateTime(2025, 9, 2, 14, 55, 43, 41, DateTimeKind.Utc).AddTicks(4203) },
                    { 12, "#8A3B83", new DateTime(2025, 9, 15, 16, 35, 25, 352, DateTimeKind.Utc).AddTicks(223), "Thông tin và thảo luận về nha khoa", 12, "icon-12", true, "Nha khoa", 0, "danh-muc-12", 0, new DateTime(2025, 9, 15, 16, 35, 25, 352, DateTimeKind.Utc).AddTicks(223) },
                    { 13, "#B60E3A", new DateTime(2025, 9, 1, 19, 4, 17, 584, DateTimeKind.Utc).AddTicks(9306), "Thông tin và thảo luận về dinh dưỡng", 13, "icon-13", true, "Dinh dưỡng", 0, "danh-muc-13", 0, new DateTime(2025, 9, 1, 19, 4, 17, 584, DateTimeKind.Utc).AddTicks(9306) },
                    { 14, "#E7FD8F", new DateTime(2025, 9, 3, 15, 49, 58, 173, DateTimeKind.Utc).AddTicks(5600), "Thông tin và thảo luận về tập luyện", 14, "icon-14", true, "Tập luyện", 0, "danh-muc-14", 0, new DateTime(2025, 9, 3, 15, 49, 58, 173, DateTimeKind.Utc).AddTicks(5600) },
                    { 15, "#8433B0", new DateTime(2025, 9, 13, 11, 11, 54, 844, DateTimeKind.Utc).AddTicks(4585), "Thông tin và thảo luận về y học cổ truyền", 15, "icon-15", true, "Y học cổ truyền", 0, "danh-muc-15", 0, new DateTime(2025, 9, 13, 11, 11, 54, 844, DateTimeKind.Utc).AddTicks(4585) },
                    { 16, "#0AA065", new DateTime(2025, 9, 4, 8, 30, 55, 588, DateTimeKind.Utc).AddTicks(8177), "Thông tin và thảo luận về dược phẩm", 16, "icon-16", true, "Dược phẩm", 0, "danh-muc-16", 0, new DateTime(2025, 9, 4, 8, 30, 55, 588, DateTimeKind.Utc).AddTicks(8177) },
                    { 17, "#9549FE", new DateTime(2025, 9, 6, 13, 8, 31, 807, DateTimeKind.Utc).AddTicks(4782), "Thông tin và thảo luận về cấp cứu", 17, "icon-17", true, "Cấp cứu", 0, "danh-muc-17", 0, new DateTime(2025, 9, 6, 13, 8, 31, 807, DateTimeKind.Utc).AddTicks(4782) },
                    { 18, "#01032F", new DateTime(2025, 9, 15, 18, 41, 59, 703, DateTimeKind.Utc).AddTicks(2951), "Thông tin và thảo luận về phục hồi chức năng", 18, "icon-18", true, "Phục hồi chức năng", 0, "danh-muc-18", 0, new DateTime(2025, 9, 15, 18, 41, 59, 703, DateTimeKind.Utc).AddTicks(2951) },
                    { 19, "#CE2091", new DateTime(2025, 9, 10, 15, 25, 54, 351, DateTimeKind.Utc).AddTicks(219), "Thông tin và thảo luận về lão khoa", 19, "icon-19", true, "Lão khoa", 0, "danh-muc-19", 0, new DateTime(2025, 9, 10, 15, 25, 54, 351, DateTimeKind.Utc).AddTicks(219) },
                    { 20, "#62736B", new DateTime(2025, 9, 14, 11, 17, 27, 55, DateTimeKind.Utc).AddTicks(425), "Thông tin và thảo luận về y học gia đình", 20, "icon-20", true, "Y học gia đình", 0, "danh-muc-20", 0, new DateTime(2025, 9, 14, 11, 17, 27, 55, DateTimeKind.Utc).AddTicks(425) },
                    { 21, "#06BE2F", new DateTime(2025, 9, 3, 16, 25, 9, 831, DateTimeKind.Utc).AddTicks(2383), "Thông tin và thảo luận về hô hấp", 21, "icon-21", true, "Hô hấp", 0, "danh-muc-21", 0, new DateTime(2025, 9, 3, 16, 25, 9, 831, DateTimeKind.Utc).AddTicks(2383) },
                    { 22, "#C819C0", new DateTime(2025, 9, 1, 22, 2, 9, 160, DateTimeKind.Utc).AddTicks(215), "Thông tin và thảo luận về tiêu hóa", 22, "icon-22", true, "Tiêu hóa", 0, "danh-muc-22", 0, new DateTime(2025, 9, 1, 22, 2, 9, 160, DateTimeKind.Utc).AddTicks(215) },
                    { 23, "#1D7C32", new DateTime(2025, 9, 12, 6, 37, 44, 342, DateTimeKind.Utc).AddTicks(5206), "Thông tin và thảo luận về nội tiết", 23, "icon-23", true, "Nội tiết", 0, "danh-muc-23", 0, new DateTime(2025, 9, 12, 6, 37, 44, 342, DateTimeKind.Utc).AddTicks(5206) },
                    { 24, "#C5BDDC", new DateTime(2025, 9, 5, 11, 58, 58, 830, DateTimeKind.Utc).AddTicks(919), "Thông tin và thảo luận về miễn dịch", 24, "icon-24", true, "Miễn dịch", 0, "danh-muc-24", 0, new DateTime(2025, 9, 5, 11, 58, 58, 830, DateTimeKind.Utc).AddTicks(919) },
                    { 25, "#A5ECE3", new DateTime(2025, 9, 6, 5, 6, 36, 17, DateTimeKind.Utc).AddTicks(4187), "Thông tin và thảo luận về huyết học", 25, "icon-25", true, "Huyết học", 0, "danh-muc-25", 0, new DateTime(2025, 9, 6, 5, 6, 36, 17, DateTimeKind.Utc).AddTicks(4187) },
                    { 26, "#0B643E", new DateTime(2025, 9, 3, 14, 3, 18, 869, DateTimeKind.Utc).AddTicks(4731), "Thông tin và thảo luận về thận - tiết niệu", 26, "icon-26", true, "Thận - Tiết niệu", 0, "danh-muc-26", 0, new DateTime(2025, 9, 3, 14, 3, 18, 869, DateTimeKind.Utc).AddTicks(4731) },
                    { 27, "#8412E6", new DateTime(2025, 9, 11, 21, 10, 9, 993, DateTimeKind.Utc).AddTicks(8943), "Thông tin và thảo luận về gan mật", 27, "icon-27", true, "Gan mật", 0, "danh-muc-27", 0, new DateTime(2025, 9, 11, 21, 10, 9, 993, DateTimeKind.Utc).AddTicks(8943) },
                    { 28, "#6B9D1D", new DateTime(2025, 9, 1, 21, 56, 35, 848, DateTimeKind.Utc).AddTicks(1027), "Thông tin và thảo luận về chấn thương chỉnh hình", 28, "icon-28", true, "Chấn thương chỉnh hình", 0, "danh-muc-28", 0, new DateTime(2025, 9, 1, 21, 56, 35, 848, DateTimeKind.Utc).AddTicks(1027) },
                    { 29, "#A0E2FE", new DateTime(2025, 9, 13, 17, 14, 36, 127, DateTimeKind.Utc).AddTicks(2877), "Thông tin và thảo luận về phẫu thuật", 29, "icon-29", true, "Phẫu thuật", 0, "danh-muc-29", 0, new DateTime(2025, 9, 13, 17, 14, 36, 127, DateTimeKind.Utc).AddTicks(2877) },
                    { 30, "#036499", new DateTime(2025, 9, 4, 22, 22, 6, 281, DateTimeKind.Utc).AddTicks(824), "Thông tin và thảo luận về gây mê hồi sức", 30, "icon-30", true, "Gây mê hồi sức", 0, "danh-muc-30", 0, new DateTime(2025, 9, 4, 22, 22, 6, 281, DateTimeKind.Utc).AddTicks(824) }
                });

            migrationBuilder.InsertData(
                table: "Specialties",
                columns: new[] { "Id", "CreatedAt", "Description", "IsActive", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 9, 6, 8, 27, 53, 468, DateTimeKind.Utc).AddTicks(3501), "Chuyên khoa bác sĩ đa khoa - Cung cấp dịch vụ chăm sóc y tế chuyên nghiệp", true, "Bác sĩ đa khoa", new DateTime(2025, 9, 6, 8, 27, 53, 468, DateTimeKind.Utc).AddTicks(3501) },
                    { 2, new DateTime(2025, 9, 17, 7, 56, 7, 323, DateTimeKind.Utc).AddTicks(3958), "Chuyên khoa tim mạch - Cung cấp dịch vụ chăm sóc y tế chuyên nghiệp", true, "Tim mạch", new DateTime(2025, 9, 17, 7, 56, 7, 323, DateTimeKind.Utc).AddTicks(3958) },
                    { 3, new DateTime(2025, 9, 11, 13, 28, 15, 796, DateTimeKind.Utc).AddTicks(3039), "Chuyên khoa da liễu - Cung cấp dịch vụ chăm sóc y tế chuyên nghiệp", true, "Da liễu", new DateTime(2025, 9, 11, 13, 28, 15, 796, DateTimeKind.Utc).AddTicks(3039) },
                    { 4, new DateTime(2025, 9, 14, 5, 33, 54, 797, DateTimeKind.Utc).AddTicks(4695), "Chuyên khoa xương khớp - Cung cấp dịch vụ chăm sóc y tế chuyên nghiệp", true, "Xương khớp", new DateTime(2025, 9, 14, 5, 33, 54, 797, DateTimeKind.Utc).AddTicks(4695) },
                    { 5, new DateTime(2025, 9, 1, 8, 23, 9, 781, DateTimeKind.Utc).AddTicks(8478), "Chuyên khoa nhi khoa - Cung cấp dịch vụ chăm sóc y tế chuyên nghiệp", true, "Nhi khoa", new DateTime(2025, 9, 1, 8, 23, 9, 781, DateTimeKind.Utc).AddTicks(8478) },
                    { 6, new DateTime(2025, 9, 16, 20, 53, 8, 882, DateTimeKind.Utc).AddTicks(359), "Chuyên khoa thần kinh - Cung cấp dịch vụ chăm sóc y tế chuyên nghiệp", true, "Thần kinh", new DateTime(2025, 9, 16, 20, 53, 8, 882, DateTimeKind.Utc).AddTicks(359) },
                    { 7, new DateTime(2025, 9, 18, 5, 43, 0, 923, DateTimeKind.Utc).AddTicks(91), "Chuyên khoa tâm thần - Cung cấp dịch vụ chăm sóc y tế chuyên nghiệp", true, "Tâm thần", new DateTime(2025, 9, 18, 5, 43, 0, 923, DateTimeKind.Utc).AddTicks(91) },
                    { 8, new DateTime(2025, 9, 6, 23, 47, 3, 515, DateTimeKind.Utc).AddTicks(9135), "Chuyên khoa ung bướu - Cung cấp dịch vụ chăm sóc y tế chuyên nghiệp", true, "Ung bướu", new DateTime(2025, 9, 6, 23, 47, 3, 515, DateTimeKind.Utc).AddTicks(9135) },
                    { 9, new DateTime(2025, 9, 13, 19, 1, 19, 890, DateTimeKind.Utc).AddTicks(2492), "Chuyên khoa phụ sản - Cung cấp dịch vụ chăm sóc y tế chuyên nghiệp", true, "Phụ sản", new DateTime(2025, 9, 13, 19, 1, 19, 890, DateTimeKind.Utc).AddTicks(2492) },
                    { 10, new DateTime(2025, 9, 3, 22, 8, 26, 473, DateTimeKind.Utc).AddTicks(6978), "Chuyên khoa mắt - Cung cấp dịch vụ chăm sóc y tế chuyên nghiệp", true, "Mắt", new DateTime(2025, 9, 3, 22, 8, 26, 473, DateTimeKind.Utc).AddTicks(6978) },
                    { 11, new DateTime(2025, 9, 2, 20, 33, 27, 652, DateTimeKind.Utc).AddTicks(3395), "Chuyên khoa tai mũi họng - Cung cấp dịch vụ chăm sóc y tế chuyên nghiệp", true, "Tai mũi họng", new DateTime(2025, 9, 2, 20, 33, 27, 652, DateTimeKind.Utc).AddTicks(3395) },
                    { 12, new DateTime(2025, 9, 9, 3, 19, 45, 562, DateTimeKind.Utc).AddTicks(6642), "Chuyên khoa chẩn đoán hình ảnh - Cung cấp dịch vụ chăm sóc y tế chuyên nghiệp", true, "Chẩn đoán hình ảnh", new DateTime(2025, 9, 9, 3, 19, 45, 562, DateTimeKind.Utc).AddTicks(6642) },
                    { 13, new DateTime(2025, 9, 6, 23, 57, 48, 264, DateTimeKind.Utc).AddTicks(6877), "Chuyên khoa giải phẫu bệnh - Cung cấp dịch vụ chăm sóc y tế chuyên nghiệp", true, "Giải phẫu bệnh", new DateTime(2025, 9, 6, 23, 57, 48, 264, DateTimeKind.Utc).AddTicks(6877) },
                    { 14, new DateTime(2025, 9, 14, 23, 16, 46, 180, DateTimeKind.Utc).AddTicks(573), "Chuyên khoa gây mê hồi sức - Cung cấp dịch vụ chăm sóc y tế chuyên nghiệp", true, "Gây mê hồi sức", new DateTime(2025, 9, 14, 23, 16, 46, 180, DateTimeKind.Utc).AddTicks(573) },
                    { 15, new DateTime(2025, 9, 3, 16, 10, 23, 428, DateTimeKind.Utc).AddTicks(4081), "Chuyên khoa phẫu thuật - Cung cấp dịch vụ chăm sóc y tế chuyên nghiệp", true, "Phẫu thuật", new DateTime(2025, 9, 3, 16, 10, 23, 428, DateTimeKind.Utc).AddTicks(4081) },
                    { 16, new DateTime(2025, 9, 3, 10, 11, 15, 314, DateTimeKind.Utc).AddTicks(1107), "Chuyên khoa cấp cứu - Cung cấp dịch vụ chăm sóc y tế chuyên nghiệp", true, "Cấp cứu", new DateTime(2025, 9, 3, 10, 11, 15, 314, DateTimeKind.Utc).AddTicks(1107) },
                    { 17, new DateTime(2025, 9, 4, 21, 9, 54, 885, DateTimeKind.Utc).AddTicks(2711), "Chuyên khoa y học gia đình - Cung cấp dịch vụ chăm sóc y tế chuyên nghiệp", true, "Y học gia đình", new DateTime(2025, 9, 4, 21, 9, 54, 885, DateTimeKind.Utc).AddTicks(2711) },
                    { 18, new DateTime(2025, 9, 11, 15, 35, 52, 868, DateTimeKind.Utc).AddTicks(7411), "Chuyên khoa nội khoa - Cung cấp dịch vụ chăm sóc y tế chuyên nghiệp", true, "Nội khoa", new DateTime(2025, 9, 11, 15, 35, 52, 868, DateTimeKind.Utc).AddTicks(7411) },
                    { 19, new DateTime(2025, 9, 10, 5, 32, 47, 286, DateTimeKind.Utc).AddTicks(5279), "Chuyên khoa nội tiết - Cung cấp dịch vụ chăm sóc y tế chuyên nghiệp", true, "Nội tiết", new DateTime(2025, 9, 10, 5, 32, 47, 286, DateTimeKind.Utc).AddTicks(5279) },
                    { 20, new DateTime(2025, 9, 18, 18, 23, 34, 992, DateTimeKind.Utc).AddTicks(8163), "Chuyên khoa tiêu hóa - Cung cấp dịch vụ chăm sóc y tế chuyên nghiệp", true, "Tiêu hóa", new DateTime(2025, 9, 18, 18, 23, 34, 992, DateTimeKind.Utc).AddTicks(8163) },
                    { 21, new DateTime(2025, 9, 14, 19, 0, 29, 303, DateTimeKind.Utc).AddTicks(1555), "Chuyên khoa hô hấp - Cung cấp dịch vụ chăm sóc y tế chuyên nghiệp", true, "Hô hấp", new DateTime(2025, 9, 14, 19, 0, 29, 303, DateTimeKind.Utc).AddTicks(1555) },
                    { 22, new DateTime(2025, 9, 13, 2, 19, 21, 57, DateTimeKind.Utc).AddTicks(3798), "Chuyên khoa thận - tiết niệu - Cung cấp dịch vụ chăm sóc y tế chuyên nghiệp", true, "Thận - Tiết niệu", new DateTime(2025, 9, 13, 2, 19, 21, 57, DateTimeKind.Utc).AddTicks(3798) },
                    { 23, new DateTime(2025, 9, 13, 20, 51, 28, 660, DateTimeKind.Utc).AddTicks(7781), "Chuyên khoa gan mật - Cung cấp dịch vụ chăm sóc y tế chuyên nghiệp", true, "Gan mật", new DateTime(2025, 9, 13, 20, 51, 28, 660, DateTimeKind.Utc).AddTicks(7781) },
                    { 24, new DateTime(2025, 9, 6, 5, 3, 10, 873, DateTimeKind.Utc).AddTicks(7025), "Chuyên khoa huyết học - Cung cấp dịch vụ chăm sóc y tế chuyên nghiệp", true, "Huyết học", new DateTime(2025, 9, 6, 5, 3, 10, 873, DateTimeKind.Utc).AddTicks(7025) },
                    { 25, new DateTime(2025, 9, 10, 22, 29, 23, 180, DateTimeKind.Utc).AddTicks(8237), "Chuyên khoa miễn dịch - Cung cấp dịch vụ chăm sóc y tế chuyên nghiệp", true, "Miễn dịch", new DateTime(2025, 9, 10, 22, 29, 23, 180, DateTimeKind.Utc).AddTicks(8237) },
                    { 26, new DateTime(2025, 9, 18, 19, 36, 20, 422, DateTimeKind.Utc).AddTicks(9413), "Chuyên khoa lão khoa - Cung cấp dịch vụ chăm sóc y tế chuyên nghiệp", true, "Lão khoa", new DateTime(2025, 9, 18, 19, 36, 20, 422, DateTimeKind.Utc).AddTicks(9413) },
                    { 27, new DateTime(2025, 9, 12, 17, 9, 33, 789, DateTimeKind.Utc).AddTicks(632), "Chuyên khoa y học cổ truyền - Cung cấp dịch vụ chăm sóc y tế chuyên nghiệp", true, "Y học cổ truyền", new DateTime(2025, 9, 12, 17, 9, 33, 789, DateTimeKind.Utc).AddTicks(632) },
                    { 28, new DateTime(2025, 9, 1, 10, 27, 42, 285, DateTimeKind.Utc).AddTicks(7004), "Chuyên khoa dinh dưỡng - Cung cấp dịch vụ chăm sóc y tế chuyên nghiệp", true, "Dinh dưỡng", new DateTime(2025, 9, 1, 10, 27, 42, 285, DateTimeKind.Utc).AddTicks(7004) },
                    { 29, new DateTime(2025, 9, 7, 5, 12, 57, 421, DateTimeKind.Utc).AddTicks(9107), "Chuyên khoa phục hồi chức năng - Cung cấp dịch vụ chăm sóc y tế chuyên nghiệp", true, "Phục hồi chức năng", new DateTime(2025, 9, 7, 5, 12, 57, 421, DateTimeKind.Utc).AddTicks(9107) },
                    { 30, new DateTime(2025, 9, 17, 17, 21, 17, 603, DateTimeKind.Utc).AddTicks(3729), "Chuyên khoa y học thể thao - Cung cấp dịch vụ chăm sóc y tế chuyên nghiệp", true, "Y học thể thao", new DateTime(2025, 9, 17, 17, 21, 17, 603, DateTimeKind.Utc).AddTicks(3729) }
                });

            migrationBuilder.InsertData(
                table: "StaticPages",
                columns: new[] { "Id", "Content", "CreatedAt", "IsActive", "Slug", "Title", "UpdatedAt", "UpdatedByAdminId" },
                values: new object[,]
                {
                    { 1, "<h1>Điều khoản sử dụng</h1><p>Nội dung chi tiết về điều khoản sử dụng. Đây là thông tin quan trọng mà người dùng cần biết khi sử dụng hệ thống.</p>", new DateTime(2025, 9, 1, 11, 43, 32, 604, DateTimeKind.Utc).AddTicks(5046), true, "trang-tinh-1", "Điều khoản sử dụng", new DateTime(2025, 9, 1, 11, 43, 32, 604, DateTimeKind.Utc).AddTicks(5046), 1 },
                    { 2, "<h1>Chính sách bảo mật</h1><p>Nội dung chi tiết về chính sách bảo mật. Đây là thông tin quan trọng mà người dùng cần biết khi sử dụng hệ thống.</p>", new DateTime(2025, 9, 4, 22, 55, 50, 699, DateTimeKind.Utc).AddTicks(2174), true, "trang-tinh-2", "Chính sách bảo mật", new DateTime(2025, 9, 4, 22, 55, 50, 699, DateTimeKind.Utc).AddTicks(2174), 1 },
                    { 3, "<h1>Về chúng tôi</h1><p>Nội dung chi tiết về về chúng tôi. Đây là thông tin quan trọng mà người dùng cần biết khi sử dụng hệ thống.</p>", new DateTime(2025, 9, 5, 18, 40, 17, 821, DateTimeKind.Utc).AddTicks(90), true, "trang-tinh-3", "Về chúng tôi", new DateTime(2025, 9, 5, 18, 40, 17, 821, DateTimeKind.Utc).AddTicks(90), 1 },
                    { 4, "<h1>Liên hệ</h1><p>Nội dung chi tiết về liên hệ. Đây là thông tin quan trọng mà người dùng cần biết khi sử dụng hệ thống.</p>", new DateTime(2025, 9, 1, 16, 37, 24, 820, DateTimeKind.Utc).AddTicks(2891), true, "trang-tinh-4", "Liên hệ", new DateTime(2025, 9, 1, 16, 37, 24, 820, DateTimeKind.Utc).AddTicks(2891), 1 },
                    { 5, "<h1>Câu hỏi thường gặp</h1><p>Nội dung chi tiết về câu hỏi thường gặp. Đây là thông tin quan trọng mà người dùng cần biết khi sử dụng hệ thống.</p>", new DateTime(2025, 9, 4, 21, 21, 55, 369, DateTimeKind.Utc).AddTicks(112), true, "trang-tinh-5", "Câu hỏi thường gặp", new DateTime(2025, 9, 4, 21, 21, 55, 369, DateTimeKind.Utc).AddTicks(112), 1 },
                    { 6, "<h1>Hướng dẫn sử dụng</h1><p>Nội dung chi tiết về hướng dẫn sử dụng. Đây là thông tin quan trọng mà người dùng cần biết khi sử dụng hệ thống.</p>", new DateTime(2025, 9, 11, 15, 53, 24, 115, DateTimeKind.Utc).AddTicks(2302), true, "trang-tinh-6", "Hướng dẫn sử dụng", new DateTime(2025, 9, 11, 15, 53, 24, 115, DateTimeKind.Utc).AddTicks(2302), 1 },
                    { 7, "<h1>Quy tắc cộng đồng</h1><p>Nội dung chi tiết về quy tắc cộng đồng. Đây là thông tin quan trọng mà người dùng cần biết khi sử dụng hệ thống.</p>", new DateTime(2025, 9, 8, 14, 23, 46, 759, DateTimeKind.Utc).AddTicks(585), true, "trang-tinh-7", "Quy tắc cộng đồng", new DateTime(2025, 9, 8, 14, 23, 46, 759, DateTimeKind.Utc).AddTicks(585), 1 },
                    { 8, "<h1>Chính sách cookie</h1><p>Nội dung chi tiết về chính sách cookie. Đây là thông tin quan trọng mà người dùng cần biết khi sử dụng hệ thống.</p>", new DateTime(2025, 9, 2, 16, 29, 33, 117, DateTimeKind.Utc).AddTicks(5848), true, "trang-tinh-8", "Chính sách cookie", new DateTime(2025, 9, 2, 16, 29, 33, 117, DateTimeKind.Utc).AddTicks(5848), 1 },
                    { 9, "<h1>Tuyên bố pháp lý</h1><p>Nội dung chi tiết về tuyên bố pháp lý. Đây là thông tin quan trọng mà người dùng cần biết khi sử dụng hệ thống.</p>", new DateTime(2025, 9, 18, 21, 11, 6, 343, DateTimeKind.Utc).AddTicks(3053), true, "trang-tinh-9", "Tuyên bố pháp lý", new DateTime(2025, 9, 18, 21, 11, 6, 343, DateTimeKind.Utc).AddTicks(3053), 1 },
                    { 10, "<h1>Bảo mật thông tin</h1><p>Nội dung chi tiết về bảo mật thông tin. Đây là thông tin quan trọng mà người dùng cần biết khi sử dụng hệ thống.</p>", new DateTime(2025, 9, 3, 4, 26, 30, 812, DateTimeKind.Utc).AddTicks(9204), true, "trang-tinh-10", "Bảo mật thông tin", new DateTime(2025, 9, 3, 4, 26, 30, 812, DateTimeKind.Utc).AddTicks(9204), 1 },
                    { 11, "<h1>Hỗ trợ khách hàng</h1><p>Nội dung chi tiết về hỗ trợ khách hàng. Đây là thông tin quan trọng mà người dùng cần biết khi sử dụng hệ thống.</p>", new DateTime(2025, 9, 2, 21, 49, 57, 932, DateTimeKind.Utc).AddTicks(2773), true, "trang-tinh-11", "Hỗ trợ khách hàng", new DateTime(2025, 9, 2, 21, 49, 57, 932, DateTimeKind.Utc).AddTicks(2773), 1 },
                    { 12, "<h1>Điều khoản dịch vụ</h1><p>Nội dung chi tiết về điều khoản dịch vụ. Đây là thông tin quan trọng mà người dùng cần biết khi sử dụng hệ thống.</p>", new DateTime(2025, 9, 6, 18, 48, 19, 211, DateTimeKind.Utc).AddTicks(6174), true, "trang-tinh-12", "Điều khoản dịch vụ", new DateTime(2025, 9, 6, 18, 48, 19, 211, DateTimeKind.Utc).AddTicks(6174), 1 },
                    { 13, "<h1>Quyền riêng tư</h1><p>Nội dung chi tiết về quyền riêng tư. Đây là thông tin quan trọng mà người dùng cần biết khi sử dụng hệ thống.</p>", new DateTime(2025, 9, 6, 19, 53, 54, 179, DateTimeKind.Utc).AddTicks(6586), true, "trang-tinh-13", "Quyền riêng tư", new DateTime(2025, 9, 6, 19, 53, 54, 179, DateTimeKind.Utc).AddTicks(6586), 1 },
                    { 14, "<h1>Báo cáo lỗi</h1><p>Nội dung chi tiết về báo cáo lỗi. Đây là thông tin quan trọng mà người dùng cần biết khi sử dụng hệ thống.</p>", new DateTime(2025, 9, 14, 12, 29, 49, 893, DateTimeKind.Utc).AddTicks(1501), true, "trang-tinh-14", "Báo cáo lỗi", new DateTime(2025, 9, 14, 12, 29, 49, 893, DateTimeKind.Utc).AddTicks(1501), 1 },
                    { 15, "<h1>Góp ý</h1><p>Nội dung chi tiết về góp ý. Đây là thông tin quan trọng mà người dùng cần biết khi sử dụng hệ thống.</p>", new DateTime(2025, 9, 15, 20, 45, 26, 755, DateTimeKind.Utc).AddTicks(403), true, "trang-tinh-15", "Góp ý", new DateTime(2025, 9, 15, 20, 45, 26, 755, DateTimeKind.Utc).AddTicks(403), 1 },
                    { 16, "<h1>Tài liệu hướng dẫn</h1><p>Nội dung chi tiết về tài liệu hướng dẫn. Đây là thông tin quan trọng mà người dùng cần biết khi sử dụng hệ thống.</p>", new DateTime(2025, 9, 1, 3, 54, 22, 514, DateTimeKind.Utc).AddTicks(849), true, "trang-tinh-16", "Tài liệu hướng dẫn", new DateTime(2025, 9, 1, 3, 54, 22, 514, DateTimeKind.Utc).AddTicks(849), 1 },
                    { 17, "<h1>Chính sách hoàn tiền</h1><p>Nội dung chi tiết về chính sách hoàn tiền. Đây là thông tin quan trọng mà người dùng cần biết khi sử dụng hệ thống.</p>", new DateTime(2025, 9, 1, 18, 10, 49, 151, DateTimeKind.Utc).AddTicks(2869), true, "trang-tinh-17", "Chính sách hoàn tiền", new DateTime(2025, 9, 1, 18, 10, 49, 151, DateTimeKind.Utc).AddTicks(2869), 1 },
                    { 18, "<h1>Điều kiện sử dụng</h1><p>Nội dung chi tiết về điều kiện sử dụng. Đây là thông tin quan trọng mà người dùng cần biết khi sử dụng hệ thống.</p>", new DateTime(2025, 9, 16, 3, 5, 37, 980, DateTimeKind.Utc).AddTicks(817), true, "trang-tinh-18", "Điều kiện sử dụng", new DateTime(2025, 9, 16, 3, 5, 37, 980, DateTimeKind.Utc).AddTicks(817), 1 },
                    { 19, "<h1>Thông tin pháp lý</h1><p>Nội dung chi tiết về thông tin pháp lý. Đây là thông tin quan trọng mà người dùng cần biết khi sử dụng hệ thống.</p>", new DateTime(2025, 9, 17, 3, 1, 34, 549, DateTimeKind.Utc).AddTicks(161), true, "trang-tinh-19", "Thông tin pháp lý", new DateTime(2025, 9, 17, 3, 1, 34, 549, DateTimeKind.Utc).AddTicks(161), 1 },
                    { 20, "<h1>Cam kết chất lượng</h1><p>Nội dung chi tiết về cam kết chất lượng. Đây là thông tin quan trọng mà người dùng cần biết khi sử dụng hệ thống.</p>", new DateTime(2025, 9, 5, 6, 50, 51, 908, DateTimeKind.Utc).AddTicks(865), true, "trang-tinh-20", "Cam kết chất lượng", new DateTime(2025, 9, 5, 6, 50, 51, 908, DateTimeKind.Utc).AddTicks(865), 1 },
                    { 21, "<h1>Hướng dẫn đăng ký</h1><p>Nội dung chi tiết về hướng dẫn đăng ký. Đây là thông tin quan trọng mà người dùng cần biết khi sử dụng hệ thống.</p>", new DateTime(2025, 9, 17, 16, 7, 35, 903, DateTimeKind.Utc).AddTicks(2211), true, "trang-tinh-21", "Hướng dẫn đăng ký", new DateTime(2025, 9, 17, 16, 7, 35, 903, DateTimeKind.Utc).AddTicks(2211), 1 },
                    { 22, "<h1>Cách thức hoạt động</h1><p>Nội dung chi tiết về cách thức hoạt động. Đây là thông tin quan trọng mà người dùng cần biết khi sử dụng hệ thống.</p>", new DateTime(2025, 9, 16, 5, 9, 51, 395, DateTimeKind.Utc).AddTicks(5770), true, "trang-tinh-22", "Cách thức hoạt động", new DateTime(2025, 9, 16, 5, 9, 51, 395, DateTimeKind.Utc).AddTicks(5770), 1 },
                    { 23, "<h1>Quy định đăng bài</h1><p>Nội dung chi tiết về quy định đăng bài. Đây là thông tin quan trọng mà người dùng cần biết khi sử dụng hệ thống.</p>", new DateTime(2025, 9, 6, 0, 43, 15, 539, DateTimeKind.Utc).AddTicks(4658), true, "trang-tinh-23", "Quy định đăng bài", new DateTime(2025, 9, 6, 0, 43, 15, 539, DateTimeKind.Utc).AddTicks(4658), 1 },
                    { 24, "<h1>Chính sách kiểm duyệt</h1><p>Nội dung chi tiết về chính sách kiểm duyệt. Đây là thông tin quan trọng mà người dùng cần biết khi sử dụng hệ thống.</p>", new DateTime(2025, 9, 10, 19, 35, 18, 453, DateTimeKind.Utc).AddTicks(6022), true, "trang-tinh-24", "Chính sách kiểm duyệt", new DateTime(2025, 9, 10, 19, 35, 18, 453, DateTimeKind.Utc).AddTicks(6022), 1 },
                    { 25, "<h1>Hướng dẫn bảo mật tài khoản</h1><p>Nội dung chi tiết về hướng dẫn bảo mật tài khoản. Đây là thông tin quan trọng mà người dùng cần biết khi sử dụng hệ thống.</p>", new DateTime(2025, 9, 12, 10, 5, 36, 369, DateTimeKind.Utc).AddTicks(4529), true, "trang-tinh-25", "Hướng dẫn bảo mật tài khoản", new DateTime(2025, 9, 12, 10, 5, 36, 369, DateTimeKind.Utc).AddTicks(4529), 1 },
                    { 26, "<h1>Quy trình xử lý khiếu nại</h1><p>Nội dung chi tiết về quy trình xử lý khiếu nại. Đây là thông tin quan trọng mà người dùng cần biết khi sử dụng hệ thống.</p>", new DateTime(2025, 9, 12, 6, 8, 31, 410, DateTimeKind.Utc).AddTicks(9703), true, "trang-tinh-26", "Quy trình xử lý khiếu nại", new DateTime(2025, 9, 12, 6, 8, 31, 410, DateTimeKind.Utc).AddTicks(9703), 1 },
                    { 27, "<h1>Chính sách sử dụng dữ liệu</h1><p>Nội dung chi tiết về chính sách sử dụng dữ liệu. Đây là thông tin quan trọng mà người dùng cần biết khi sử dụng hệ thống.</p>", new DateTime(2025, 9, 4, 11, 4, 38, 662, DateTimeKind.Utc).AddTicks(6176), true, "trang-tinh-27", "Chính sách sử dụng dữ liệu", new DateTime(2025, 9, 4, 11, 4, 38, 662, DateTimeKind.Utc).AddTicks(6176), 1 },
                    { 28, "<h1>Hướng dẫn tương tác</h1><p>Nội dung chi tiết về hướng dẫn tương tác. Đây là thông tin quan trọng mà người dùng cần biết khi sử dụng hệ thống.</p>", new DateTime(2025, 9, 6, 8, 25, 4, 922, DateTimeKind.Utc).AddTicks(6013), true, "trang-tinh-28", "Hướng dẫn tương tác", new DateTime(2025, 9, 6, 8, 25, 4, 922, DateTimeKind.Utc).AddTicks(6013), 1 },
                    { 29, "<h1>Nguyên tắc cộng đồng</h1><p>Nội dung chi tiết về nguyên tắc cộng đồng. Đây là thông tin quan trọng mà người dùng cần biết khi sử dụng hệ thống.</p>", new DateTime(2025, 9, 4, 7, 4, 31, 616, DateTimeKind.Utc).AddTicks(6458), true, "trang-tinh-29", "Nguyên tắc cộng đồng", new DateTime(2025, 9, 4, 7, 4, 31, 616, DateTimeKind.Utc).AddTicks(6458), 1 },
                    { 30, "<h1>Chính sách nội dung</h1><p>Nội dung chi tiết về chính sách nội dung. Đây là thông tin quan trọng mà người dùng cần biết khi sử dụng hệ thống.</p>", new DateTime(2025, 9, 1, 12, 46, 48, 358, DateTimeKind.Utc).AddTicks(9064), true, "trang-tinh-30", "Chính sách nội dung", new DateTime(2025, 9, 1, 12, 46, 48, 358, DateTimeKind.Utc).AddTicks(9064), 1 }
                });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "Color", "CreatedAt", "Name", "Slug", "TopicCount" },
                values: new object[,]
                {
                    { 1, "#28a745", new DateTime(2025, 9, 7, 13, 44, 58, 838, DateTimeKind.Utc).AddTicks(9159), "Câu hỏi", "the-1", 0 },
                    { 2, "#ffc107", new DateTime(2025, 9, 5, 19, 31, 59, 949, DateTimeKind.Utc).AddTicks(2023), "Thảo luận", "the-2", 0 },
                    { 3, "#dc3545", new DateTime(2025, 9, 2, 5, 39, 41, 263, DateTimeKind.Utc).AddTicks(1247), "Trợ giúp", "the-3", 0 },
                    { 4, "#6f42c1", new DateTime(2025, 9, 13, 22, 15, 4, 644, DateTimeKind.Utc).AddTicks(1079), "Thông báo", "the-4", 0 },
                    { 5, "#fd7e14", new DateTime(2025, 9, 2, 6, 38, 50, 391, DateTimeKind.Utc).AddTicks(8651), "Khẩn cấp", "the-5", 0 },
                    { 6, "#20c997", new DateTime(2025, 9, 9, 8, 29, 17, 868, DateTimeKind.Utc).AddTicks(8228), "Tư vấn", "the-6", 0 },
                    { 7, "#17a2b8", new DateTime(2025, 9, 1, 5, 11, 54, 525, DateTimeKind.Utc).AddTicks(2112), "Chia sẻ", "the-7", 0 },
                    { 8, "#6c757d", new DateTime(2025, 9, 15, 8, 39, 12, 267, DateTimeKind.Utc).AddTicks(4276), "Kinh nghiệm", "the-8", 0 },
                    { 9, "#343a40", new DateTime(2025, 9, 8, 21, 43, 31, 492, DateTimeKind.Utc).AddTicks(8976), "Phòng bệnh", "the-9", 0 },
                    { 10, "#007bff", new DateTime(2025, 9, 10, 5, 49, 48, 837, DateTimeKind.Utc).AddTicks(3614), "Điều trị", "the-10", 0 },
                    { 11, "#28a745", new DateTime(2025, 9, 9, 23, 49, 32, 438, DateTimeKind.Utc).AddTicks(6260), "Thuốc men", "the-11", 0 },
                    { 12, "#ffc107", new DateTime(2025, 9, 1, 3, 52, 3, 555, DateTimeKind.Utc).AddTicks(3624), "Dinh dưỡng", "the-12", 0 },
                    { 13, "#dc3545", new DateTime(2025, 9, 15, 22, 3, 27, 915, DateTimeKind.Utc).AddTicks(544), "Tập luyện", "the-13", 0 },
                    { 14, "#6f42c1", new DateTime(2025, 9, 1, 5, 34, 17, 125, DateTimeKind.Utc).AddTicks(1531), "Sức khỏe tâm thần", "the-14", 0 },
                    { 15, "#fd7e14", new DateTime(2025, 9, 1, 1, 19, 40, 694, DateTimeKind.Utc).AddTicks(9564), "Mẹ và bé", "the-15", 0 },
                    { 16, "#20c997", new DateTime(2025, 9, 2, 3, 32, 59, 855, DateTimeKind.Utc).AddTicks(2864), "Người cao tuổi", "the-16", 0 },
                    { 17, "#17a2b8", new DateTime(2025, 9, 12, 18, 35, 22, 727, DateTimeKind.Utc).AddTicks(5310), "Y học cổ truyền", "the-17", 0 },
                    { 18, "#6c757d", new DateTime(2025, 9, 1, 14, 55, 15, 69, DateTimeKind.Utc).AddTicks(9079), "Cấp cứu", "the-18", 0 },
                    { 19, "#343a40", new DateTime(2025, 9, 17, 10, 38, 16, 366, DateTimeKind.Utc).AddTicks(8480), "Bảo hiểm y tế", "the-19", 0 },
                    { 20, "#007bff", new DateTime(2025, 9, 17, 17, 18, 54, 407, DateTimeKind.Utc).AddTicks(3798), "Khám sức khỏe", "the-20", 0 },
                    { 21, "#28a745", new DateTime(2025, 9, 9, 7, 14, 8, 76, DateTimeKind.Utc).AddTicks(7492), "Lời khuyên", "the-21", 0 },
                    { 22, "#ffc107", new DateTime(2025, 9, 7, 18, 8, 37, 415, DateTimeKind.Utc).AddTicks(9746), "Góp ý", "the-22", 0 },
                    { 23, "#dc3545", new DateTime(2025, 9, 12, 2, 22, 25, 160, DateTimeKind.Utc).AddTicks(1391), "Hướng dẫn", "the-23", 0 },
                    { 24, "#6f42c1", new DateTime(2025, 9, 1, 19, 11, 58, 977, DateTimeKind.Utc).AddTicks(4242), "Mẹo hay", "the-24", 0 },
                    { 25, "#fd7e14", new DateTime(2025, 9, 7, 17, 2, 7, 222, DateTimeKind.Utc).AddTicks(1972), "Lưu ý", "the-25", 0 },
                    { 26, "#20c997", new DateTime(2025, 9, 3, 13, 17, 31, 788, DateTimeKind.Utc).AddTicks(4922), "Cảnh báo", "the-26", 0 },
                    { 27, "#17a2b8", new DateTime(2025, 9, 8, 20, 45, 45, 264, DateTimeKind.Utc).AddTicks(3181), "An toàn", "the-27", 0 },
                    { 28, "#6c757d", new DateTime(2025, 9, 15, 6, 16, 2, 825, DateTimeKind.Utc).AddTicks(9937), "Hiệu quả", "the-28", 0 },
                    { 29, "#343a40", new DateTime(2025, 9, 10, 13, 58, 54, 446, DateTimeKind.Utc).AddTicks(4186), "Tự nhiên", "the-29", 0 },
                    { 30, "#007bff", new DateTime(2025, 9, 8, 5, 6, 6, 672, DateTimeKind.Utc).AddTicks(6085), "Khoa học", "the-30", 0 }
                });

            migrationBuilder.InsertData(
                table: "UserAccounts",
                columns: new[] { "Id", "Avatar", "Bio", "CreatedAt", "Email", "FullName", "IsActive", "IsEmailVerified", "LastLoginAt", "PasswordHash", "ResetPasswordExpires", "ResetPasswordToken", "Role", "ShowBio", "ShowEmail", "SpecialtyId", "UpdatedAt", "Username" },
                values: new object[,]
                {
                    { 1, null, "Xin chào, tôi là Nguyễn Văn An. Rất vui được tham gia cộng đồng sức khỏe để học hỏi và chia sẻ kiến thức.", new DateTime(2025, 9, 5, 3, 43, 35, 92, DateTimeKind.Utc).AddTicks(9726), "nguoidung1@email.com", "Nguyễn Văn An", true, false, new DateTime(2025, 9, 17, 19, 15, 18, 637, DateTimeKind.Utc).AddTicks(3086), "$2a$11$ZJN7y.5DKhZzYN2R5ZHE8O9Kb8kzGzV8jGqZ9v1E8z.8K.8h8Z8h8Z", null, null, "Bác sĩ", true, false, null, new DateTime(2025, 9, 5, 3, 43, 35, 92, DateTimeKind.Utc).AddTicks(9726), "nguoidung1" },
                    { 2, null, "Xin chào, tôi là Trần Thị Bình. Rất vui được tham gia cộng đồng sức khỏe để học hỏi và chia sẻ kiến thức.", new DateTime(2025, 9, 18, 18, 34, 5, 94, DateTimeKind.Utc).AddTicks(28), "nguoidung2@email.com", "Trần Thị Bình", true, true, new DateTime(2025, 9, 19, 4, 34, 5, 94, DateTimeKind.Utc).AddTicks(28), "$2a$11$ZJN7y.5DKhZzYN2R5ZHE8O9Kb8kzGzV8jGqZ9v1E8z.8K.8h8Z8h8Z", null, null, "Kiểm duyệt viên", true, false, null, new DateTime(2025, 9, 18, 18, 34, 5, 94, DateTimeKind.Utc).AddTicks(28), "nguoidung2" },
                    { 3, null, "Xin chào, tôi là Lê Văn Cường. Rất vui được tham gia cộng đồng sức khỏe để học hỏi và chia sẻ kiến thức.", new DateTime(2025, 9, 16, 21, 37, 35, 953, DateTimeKind.Utc).AddTicks(1220), "nguoidung3@email.com", "Lê Văn Cường", true, false, new DateTime(2025, 9, 17, 18, 37, 35, 953, DateTimeKind.Utc).AddTicks(1220), "$2a$11$ZJN7y.5DKhZzYN2R5ZHE8O9Kb8kzGzV8jGqZ9v1E8z.8K.8h8Z8h8Z", null, null, "Thành viên", true, true, null, new DateTime(2025, 9, 16, 21, 37, 35, 953, DateTimeKind.Utc).AddTicks(1220), "nguoidung3" },
                    { 4, null, "Xin chào, tôi là Phạm Thị Dung. Rất vui được tham gia cộng đồng sức khỏe để học hỏi và chia sẻ kiến thức.", new DateTime(2025, 9, 11, 1, 54, 50, 811, DateTimeKind.Utc).AddTicks(460), "nguoidung4@email.com", "Phạm Thị Dung", true, true, new DateTime(2025, 9, 13, 4, 54, 50, 811, DateTimeKind.Utc).AddTicks(460), "$2a$11$ZJN7y.5DKhZzYN2R5ZHE8O9Kb8kzGzV8jGqZ9v1E8z.8K.8h8Z8h8Z", null, null, "Bác sĩ", true, false, null, new DateTime(2025, 9, 11, 1, 54, 50, 811, DateTimeKind.Utc).AddTicks(460), "nguoidung4" },
                    { 5, null, "Xin chào, tôi là Hoàng Văn Em. Rất vui được tham gia cộng đồng sức khỏe để học hỏi và chia sẻ kiến thức.", new DateTime(2025, 9, 3, 4, 40, 21, 767, DateTimeKind.Utc).AddTicks(6862), "nguoidung5@email.com", "Hoàng Văn Em", true, false, new DateTime(2025, 9, 11, 18, 24, 45, 525, DateTimeKind.Utc).AddTicks(4357), "$2a$11$ZJN7y.5DKhZzYN2R5ZHE8O9Kb8kzGzV8jGqZ9v1E8z.8K.8h8Z8h8Z", null, null, "Kiểm duyệt viên", true, false, null, new DateTime(2025, 9, 3, 4, 40, 21, 767, DateTimeKind.Utc).AddTicks(6862), "nguoidung5" },
                    { 6, null, "Xin chào, tôi là Vũ Thị Phương. Rất vui được tham gia cộng đồng sức khỏe để học hỏi và chia sẻ kiến thức.", new DateTime(2025, 9, 8, 1, 6, 16, 473, DateTimeKind.Utc).AddTicks(6075), "nguoidung6@email.com", "Vũ Thị Phương", true, true, new DateTime(2025, 9, 16, 20, 33, 37, 60, DateTimeKind.Utc).AddTicks(5389), "$2a$11$ZJN7y.5DKhZzYN2R5ZHE8O9Kb8kzGzV8jGqZ9v1E8z.8K.8h8Z8h8Z", null, null, "Thành viên", true, true, null, new DateTime(2025, 9, 8, 1, 6, 16, 473, DateTimeKind.Utc).AddTicks(6075), "nguoidung6" },
                    { 7, null, "Xin chào, tôi là Đặng Văn Giang. Rất vui được tham gia cộng đồng sức khỏe để học hỏi và chia sẻ kiến thức.", new DateTime(2025, 9, 9, 0, 12, 10, 91, DateTimeKind.Utc).AddTicks(5380), "nguoidung7@email.com", "Đặng Văn Giang", true, false, new DateTime(2025, 9, 11, 21, 12, 10, 91, DateTimeKind.Utc).AddTicks(5380), "$2a$11$ZJN7y.5DKhZzYN2R5ZHE8O9Kb8kzGzV8jGqZ9v1E8z.8K.8h8Z8h8Z", null, null, "Bác sĩ", true, false, null, new DateTime(2025, 9, 9, 0, 12, 10, 91, DateTimeKind.Utc).AddTicks(5380), "nguoidung7" },
                    { 8, null, "Xin chào, tôi là Bùi Thị Hoa. Rất vui được tham gia cộng đồng sức khỏe để học hỏi và chia sẻ kiến thức.", new DateTime(2025, 9, 9, 8, 17, 43, 118, DateTimeKind.Utc).AddTicks(6622), "nguoidung8@email.com", "Bùi Thị Hoa", true, true, new DateTime(2025, 9, 10, 18, 35, 0, 666, DateTimeKind.Utc).AddTicks(7388), "$2a$11$ZJN7y.5DKhZzYN2R5ZHE8O9Kb8kzGzV8jGqZ9v1E8z.8K.8h8Z8h8Z", null, null, "Kiểm duyệt viên", true, false, null, new DateTime(2025, 9, 9, 8, 17, 43, 118, DateTimeKind.Utc).AddTicks(6622), "nguoidung8" },
                    { 9, null, "Xin chào, tôi là Dương Văn Inh. Rất vui được tham gia cộng đồng sức khỏe để học hỏi và chia sẻ kiến thức.", new DateTime(2025, 9, 18, 1, 25, 31, 271, DateTimeKind.Utc).AddTicks(6544), "nguoidung9@email.com", "Dương Văn Inh", true, false, new DateTime(2025, 9, 18, 11, 25, 31, 271, DateTimeKind.Utc).AddTicks(6544), "$2a$11$ZJN7y.5DKhZzYN2R5ZHE8O9Kb8kzGzV8jGqZ9v1E8z.8K.8h8Z8h8Z", null, null, "Thành viên", true, true, null, new DateTime(2025, 9, 18, 1, 25, 31, 271, DateTimeKind.Utc).AddTicks(6544), "nguoidung9" },
                    { 10, null, "Xin chào, tôi là Đinh Thị Kim. Rất vui được tham gia cộng đồng sức khỏe để học hỏi và chia sẻ kiến thức.", new DateTime(2025, 9, 2, 22, 22, 5, 513, DateTimeKind.Utc).AddTicks(8242), "nguoidung10@email.com", "Đinh Thị Kim", true, true, new DateTime(2025, 9, 3, 11, 22, 5, 513, DateTimeKind.Utc).AddTicks(8242), "$2a$11$ZJN7y.5DKhZzYN2R5ZHE8O9Kb8kzGzV8jGqZ9v1E8z.8K.8h8Z8h8Z", null, null, "Bác sĩ", true, false, null, new DateTime(2025, 9, 2, 22, 22, 5, 513, DateTimeKind.Utc).AddTicks(8242), "nguoidung10" },
                    { 11, null, "Xin chào, tôi là Phan Văn Long. Rất vui được tham gia cộng đồng sức khỏe để học hỏi và chia sẻ kiến thức.", new DateTime(2025, 9, 17, 22, 12, 34, 159, DateTimeKind.Utc).AddTicks(7516), "nguoidung11@email.com", "Phan Văn Long", true, false, new DateTime(2025, 9, 19, 3, 12, 34, 159, DateTimeKind.Utc).AddTicks(7516), "$2a$11$ZJN7y.5DKhZzYN2R5ZHE8O9Kb8kzGzV8jGqZ9v1E8z.8K.8h8Z8h8Z", null, null, "Kiểm duyệt viên", true, false, null, new DateTime(2025, 9, 17, 22, 12, 34, 159, DateTimeKind.Utc).AddTicks(7516), "nguoidung11" },
                    { 12, null, "Xin chào, tôi là Võ Thị Mai. Rất vui được tham gia cộng đồng sức khỏe để học hỏi và chia sẻ kiến thức.", new DateTime(2025, 9, 11, 15, 13, 22, 958, DateTimeKind.Utc).AddTicks(409), "nguoidung12@email.com", "Võ Thị Mai", true, true, new DateTime(2025, 9, 12, 0, 13, 22, 958, DateTimeKind.Utc).AddTicks(409), "$2a$11$ZJN7y.5DKhZzYN2R5ZHE8O9Kb8kzGzV8jGqZ9v1E8z.8K.8h8Z8h8Z", null, null, "Thành viên", true, true, null, new DateTime(2025, 9, 11, 15, 13, 22, 958, DateTimeKind.Utc).AddTicks(409), "nguoidung12" },
                    { 13, null, "Xin chào, tôi là Đỗ Văn Nam. Rất vui được tham gia cộng đồng sức khỏe để học hỏi và chia sẻ kiến thức.", new DateTime(2025, 9, 17, 9, 25, 0, 500, DateTimeKind.Utc).AddTicks(9389), "nguoidung13@email.com", "Đỗ Văn Nam", true, false, new DateTime(2025, 9, 18, 8, 24, 17, 984, DateTimeKind.Utc).AddTicks(3988), "$2a$11$ZJN7y.5DKhZzYN2R5ZHE8O9Kb8kzGzV8jGqZ9v1E8z.8K.8h8Z8h8Z", null, null, "Bác sĩ", true, false, null, new DateTime(2025, 9, 17, 9, 25, 0, 500, DateTimeKind.Utc).AddTicks(9389), "nguoidung13" },
                    { 14, null, "Xin chào, tôi là Chu Thị Oanh. Rất vui được tham gia cộng đồng sức khỏe để học hỏi và chia sẻ kiến thức.", new DateTime(2025, 9, 17, 0, 42, 54, 719, DateTimeKind.Utc).AddTicks(9478), "nguoidung14@email.com", "Chu Thị Oanh", true, true, new DateTime(2025, 9, 18, 0, 42, 54, 719, DateTimeKind.Utc).AddTicks(9478), "$2a$11$ZJN7y.5DKhZzYN2R5ZHE8O9Kb8kzGzV8jGqZ9v1E8z.8K.8h8Z8h8Z", null, null, "Kiểm duyệt viên", true, false, null, new DateTime(2025, 9, 17, 0, 42, 54, 719, DateTimeKind.Utc).AddTicks(9478), "nguoidung14" },
                    { 15, null, "Xin chào, tôi là Triệu Văn Phúc. Rất vui được tham gia cộng đồng sức khỏe để học hỏi và chia sẻ kiến thức.", new DateTime(2025, 9, 10, 18, 13, 51, 892, DateTimeKind.Utc).AddTicks(7823), "nguoidung15@email.com", "Triệu Văn Phúc", true, false, new DateTime(2025, 9, 12, 2, 4, 56, 23, DateTimeKind.Utc).AddTicks(9570), "$2a$11$ZJN7y.5DKhZzYN2R5ZHE8O9Kb8kzGzV8jGqZ9v1E8z.8K.8h8Z8h8Z", null, null, "Thành viên", true, true, null, new DateTime(2025, 9, 10, 18, 13, 51, 892, DateTimeKind.Utc).AddTicks(7823), "nguoidung15" },
                    { 16, null, "Xin chào, tôi là Lý Thị Quỳnh. Rất vui được tham gia cộng đồng sức khỏe để học hỏi và chia sẻ kiến thức.", new DateTime(2025, 9, 17, 23, 7, 49, 801, DateTimeKind.Utc).AddTicks(9182), "nguoidung16@email.com", "Lý Thị Quỳnh", true, true, new DateTime(2025, 9, 20, 5, 7, 49, 801, DateTimeKind.Utc).AddTicks(9182), "$2a$11$ZJN7y.5DKhZzYN2R5ZHE8O9Kb8kzGzV8jGqZ9v1E8z.8K.8h8Z8h8Z", null, null, "Bác sĩ", true, false, null, new DateTime(2025, 9, 17, 23, 7, 49, 801, DateTimeKind.Utc).AddTicks(9182), "nguoidung16" },
                    { 17, null, "Xin chào, tôi là Tô Văn Rực. Rất vui được tham gia cộng đồng sức khỏe để học hỏi và chia sẻ kiến thức.", new DateTime(2025, 9, 6, 1, 10, 47, 169, DateTimeKind.Utc).AddTicks(356), "nguoidung17@email.com", "Tô Văn Rực", true, false, new DateTime(2025, 9, 7, 2, 44, 36, 235, DateTimeKind.Utc).AddTicks(8382), "$2a$11$ZJN7y.5DKhZzYN2R5ZHE8O9Kb8kzGzV8jGqZ9v1E8z.8K.8h8Z8h8Z", null, null, "Kiểm duyệt viên", true, false, null, new DateTime(2025, 9, 6, 1, 10, 47, 169, DateTimeKind.Utc).AddTicks(356), "nguoidung17" },
                    { 18, null, "Xin chào, tôi là Ngô Thị Sương. Rất vui được tham gia cộng đồng sức khỏe để học hỏi và chia sẻ kiến thức.", new DateTime(2025, 9, 10, 11, 3, 23, 353, DateTimeKind.Utc).AddTicks(343), "nguoidung18@email.com", "Ngô Thị Sương", true, true, new DateTime(2025, 9, 15, 11, 27, 14, 135, DateTimeKind.Utc).AddTicks(5348), "$2a$11$ZJN7y.5DKhZzYN2R5ZHE8O9Kb8kzGzV8jGqZ9v1E8z.8K.8h8Z8h8Z", null, null, "Thành viên", true, true, null, new DateTime(2025, 9, 10, 11, 3, 23, 353, DateTimeKind.Utc).AddTicks(343), "nguoidung18" },
                    { 19, null, "Xin chào, tôi là Lương Văn Tuấn. Rất vui được tham gia cộng đồng sức khỏe để học hỏi và chia sẻ kiến thức.", new DateTime(2025, 9, 5, 10, 45, 5, 870, DateTimeKind.Utc).AddTicks(1412), "nguoidung19@email.com", "Lương Văn Tuấn", true, false, new DateTime(2025, 9, 16, 23, 36, 58, 65, DateTimeKind.Utc).AddTicks(8582), "$2a$11$ZJN7y.5DKhZzYN2R5ZHE8O9Kb8kzGzV8jGqZ9v1E8z.8K.8h8Z8h8Z", null, null, "Bác sĩ", true, false, null, new DateTime(2025, 9, 5, 10, 45, 5, 870, DateTimeKind.Utc).AddTicks(1412), "nguoidung19" },
                    { 20, null, "Xin chào, tôi là Tạ Thị Uyên. Rất vui được tham gia cộng đồng sức khỏe để học hỏi và chia sẻ kiến thức.", new DateTime(2025, 9, 12, 23, 1, 40, 393, DateTimeKind.Utc).AddTicks(633), "nguoidung20@email.com", "Tạ Thị Uyên", true, true, new DateTime(2025, 9, 13, 19, 1, 40, 393, DateTimeKind.Utc).AddTicks(633), "$2a$11$ZJN7y.5DKhZzYN2R5ZHE8O9Kb8kzGzV8jGqZ9v1E8z.8K.8h8Z8h8Z", null, null, "Kiểm duyệt viên", true, false, null, new DateTime(2025, 9, 12, 23, 1, 40, 393, DateTimeKind.Utc).AddTicks(633), "nguoidung20" },
                    { 21, null, "Xin chào, tôi là Cao Văn Bảo. Rất vui được tham gia cộng đồng sức khỏe để học hỏi và chia sẻ kiến thức.", new DateTime(2025, 9, 5, 2, 42, 59, 894, DateTimeKind.Utc).AddTicks(9897), "nguoidung21@email.com", "Cao Văn Bảo", true, false, new DateTime(2025, 9, 16, 0, 7, 20, 803, DateTimeKind.Utc).AddTicks(9391), "$2a$11$ZJN7y.5DKhZzYN2R5ZHE8O9Kb8kzGzV8jGqZ9v1E8z.8K.8h8Z8h8Z", null, null, "Thành viên", true, true, null, new DateTime(2025, 9, 5, 2, 42, 59, 894, DateTimeKind.Utc).AddTicks(9897), "nguoidung21" },
                    { 22, null, "Xin chào, tôi là Đào Thị Chi. Rất vui được tham gia cộng đồng sức khỏe để học hỏi và chia sẻ kiến thức.", new DateTime(2025, 9, 12, 12, 29, 29, 949, DateTimeKind.Utc).AddTicks(9069), "nguoidung22@email.com", "Đào Thị Chi", true, true, new DateTime(2025, 9, 13, 18, 29, 29, 949, DateTimeKind.Utc).AddTicks(9069), "$2a$11$ZJN7y.5DKhZzYN2R5ZHE8O9Kb8kzGzV8jGqZ9v1E8z.8K.8h8Z8h8Z", null, null, "Bác sĩ", true, false, null, new DateTime(2025, 9, 12, 12, 29, 29, 949, DateTimeKind.Utc).AddTicks(9069), "nguoidung22" },
                    { 23, null, "Xin chào, tôi là Lưu Văn Đức. Rất vui được tham gia cộng đồng sức khỏe để học hỏi và chia sẻ kiến thức.", new DateTime(2025, 9, 15, 11, 49, 11, 900, DateTimeKind.Utc).AddTicks(6243), "nguoidung23@email.com", "Lưu Văn Đức", true, false, new DateTime(2025, 9, 16, 20, 11, 59, 580, DateTimeKind.Utc).AddTicks(1785), "$2a$11$ZJN7y.5DKhZzYN2R5ZHE8O9Kb8kzGzV8jGqZ9v1E8z.8K.8h8Z8h8Z", null, null, "Kiểm duyệt viên", true, false, null, new DateTime(2025, 9, 15, 11, 49, 11, 900, DateTimeKind.Utc).AddTicks(6243), "nguoidung23" },
                    { 24, null, "Xin chào, tôi là Mai Thị Ế. Rất vui được tham gia cộng đồng sức khỏe để học hỏi và chia sẻ kiến thức.", new DateTime(2025, 9, 15, 0, 26, 19, 586, DateTimeKind.Utc).AddTicks(7245), "nguoidung24@email.com", "Mai Thị Ế", true, true, new DateTime(2025, 9, 18, 3, 51, 12, 660, DateTimeKind.Utc).AddTicks(2655), "$2a$11$ZJN7y.5DKhZzYN2R5ZHE8O9Kb8kzGzV8jGqZ9v1E8z.8K.8h8Z8h8Z", null, null, "Thành viên", true, true, null, new DateTime(2025, 9, 15, 0, 26, 19, 586, DateTimeKind.Utc).AddTicks(7245), "nguoidung24" },
                    { 25, null, "Xin chào, tôi là Nghiêm Văn Phát. Rất vui được tham gia cộng đồng sức khỏe để học hỏi và chia sẻ kiến thức.", new DateTime(2025, 9, 17, 23, 25, 0, 793, DateTimeKind.Utc).AddTicks(3703), "nguoidung25@email.com", "Nghiêm Văn Phát", true, false, new DateTime(2025, 9, 20, 15, 25, 0, 793, DateTimeKind.Utc).AddTicks(3703), "$2a$11$ZJN7y.5DKhZzYN2R5ZHE8O9Kb8kzGzV8jGqZ9v1E8z.8K.8h8Z8h8Z", null, null, "Bác sĩ", true, false, null, new DateTime(2025, 9, 17, 23, 25, 0, 793, DateTimeKind.Utc).AddTicks(3703), "nguoidung25" },
                    { 26, null, "Xin chào, tôi là Ông Thị Giang. Rất vui được tham gia cộng đồng sức khỏe để học hỏi và chia sẻ kiến thức.", new DateTime(2025, 9, 18, 10, 41, 26, 853, DateTimeKind.Utc).AddTicks(51), "nguoidung26@email.com", "Ông Thị Giang", true, true, new DateTime(2025, 9, 20, 4, 41, 26, 853, DateTimeKind.Utc).AddTicks(51), "$2a$11$ZJN7y.5DKhZzYN2R5ZHE8O9Kb8kzGzV8jGqZ9v1E8z.8K.8h8Z8h8Z", null, null, "Kiểm duyệt viên", true, false, null, new DateTime(2025, 9, 18, 10, 41, 26, 853, DateTimeKind.Utc).AddTicks(51), "nguoidung26" },
                    { 27, null, "Xin chào, tôi là Quách Văn Hùng. Rất vui được tham gia cộng đồng sức khỏe để học hỏi và chia sẻ kiến thức.", new DateTime(2025, 9, 4, 19, 15, 20, 266, DateTimeKind.Utc).AddTicks(7473), "nguoidung27@email.com", "Quách Văn Hùng", true, false, new DateTime(2025, 9, 12, 10, 0, 26, 541, DateTimeKind.Utc).AddTicks(368), "$2a$11$ZJN7y.5DKhZzYN2R5ZHE8O9Kb8kzGzV8jGqZ9v1E8z.8K.8h8Z8h8Z", null, null, "Thành viên", true, true, null, new DateTime(2025, 9, 4, 19, 15, 20, 266, DateTimeKind.Utc).AddTicks(7473), "nguoidung27" },
                    { 28, null, "Xin chào, tôi là Sử Thị Vy. Rất vui được tham gia cộng đồng sức khỏe để học hỏi và chia sẻ kiến thức.", new DateTime(2025, 9, 10, 0, 23, 20, 753, DateTimeKind.Utc).AddTicks(6596), "nguoidung28@email.com", "Sử Thị Vy", true, true, new DateTime(2025, 9, 13, 17, 58, 58, 765, DateTimeKind.Utc).AddTicks(5703), "$2a$11$ZJN7y.5DKhZzYN2R5ZHE8O9Kb8kzGzV8jGqZ9v1E8z.8K.8h8Z8h8Z", null, null, "Bác sĩ", true, false, null, new DateTime(2025, 9, 10, 0, 23, 20, 753, DateTimeKind.Utc).AddTicks(6596), "nguoidung28" },
                    { 29, null, "Xin chào, tôi là Thái Văn Khôi. Rất vui được tham gia cộng đồng sức khỏe để học hỏi và chia sẻ kiến thức.", new DateTime(2025, 9, 2, 23, 57, 56, 973, DateTimeKind.Utc).AddTicks(9234), "nguoidung29@email.com", "Thái Văn Khôi", true, false, new DateTime(2025, 9, 15, 12, 11, 33, 984, DateTimeKind.Utc).AddTicks(9387), "$2a$11$ZJN7y.5DKhZzYN2R5ZHE8O9Kb8kzGzV8jGqZ9v1E8z.8K.8h8Z8h8Z", null, null, "Kiểm duyệt viên", true, false, null, new DateTime(2025, 9, 2, 23, 57, 56, 973, DateTimeKind.Utc).AddTicks(9234), "nguoidung29" },
                    { 30, null, "Xin chào, tôi là Ứng Thị Linh. Rất vui được tham gia cộng đồng sức khỏe để học hỏi và chia sẻ kiến thức.", new DateTime(2025, 9, 7, 6, 16, 41, 743, DateTimeKind.Utc).AddTicks(6709), "nguoidung30@email.com", "Ứng Thị Linh", true, true, new DateTime(2025, 9, 10, 9, 9, 52, 316, DateTimeKind.Utc).AddTicks(7439), "$2a$11$ZJN7y.5DKhZzYN2R5ZHE8O9Kb8kzGzV8jGqZ9v1E8z.8K.8h8Z8h8Z", null, null, "Thành viên", true, true, null, new DateTime(2025, 9, 7, 6, 16, 41, 743, DateTimeKind.Utc).AddTicks(6709), "nguoidung30" }
                });

            migrationBuilder.InsertData(
                table: "Topics",
                columns: new[] { "Id", "CategoryId", "Content", "CreatedAt", "HasAnswer", "IsActive", "IsLocked", "IsPinned", "LastActivityAt", "LikeCount", "PostCount", "Slug", "Title", "UpdatedAt", "UserId", "ViewCount" },
                values: new object[,]
                {
                    { 1, 2, "<p>Đây là nội dung chi tiết về chủ đề 'Cách phòng ngừa cảm cúm mùa đông hiệu quả'. Bài viết cung cấp thông tin hữu ích và kiến thức y học dựa trên khoa học để giúp mọi người hiểu rõ hơn về vấn đề sức khỏe quan trọng này. Nội dung được biên soạn bởi các chuyên gia y tế có kinh nghiệm.</p>", new DateTime(2025, 9, 13, 23, 20, 5, 894, DateTimeKind.Utc).AddTicks(3091), false, true, false, true, new DateTime(2025, 9, 15, 19, 20, 5, 894, DateTimeKind.Utc).AddTicks(3091), 29, 3, "chu-de-1-thao-luan-suc-khoe", "Cách phòng ngừa cảm cúm mùa đông hiệu quả", new DateTime(2025, 9, 13, 23, 20, 5, 894, DateTimeKind.Utc).AddTicks(3091), 2, 396 },
                    { 2, 3, "<p>Đây là nội dung chi tiết về chủ đề 'Chế độ ăn uống lành mạnh cho người tiểu đường'. Bài viết cung cấp thông tin hữu ích và kiến thức y học dựa trên khoa học để giúp mọi người hiểu rõ hơn về vấn đề sức khỏe quan trọng này. Nội dung được biên soạn bởi các chuyên gia y tế có kinh nghiệm.</p>", new DateTime(2025, 9, 1, 22, 35, 36, 93, DateTimeKind.Utc).AddTicks(8933), false, true, false, true, new DateTime(2025, 9, 4, 20, 38, 21, 172, DateTimeKind.Utc).AddTicks(9212), 9, 5, "chu-de-2-thao-luan-suc-khoe", "Chế độ ăn uống lành mạnh cho người tiểu đường", new DateTime(2025, 9, 2, 0, 3, 17, 974, DateTimeKind.Utc).AddTicks(3434), 3, 163 },
                    { 3, 4, "<p>Đây là nội dung chi tiết về chủ đề 'Bài tập thể dục phù hợp cho người cao tuổi'. Bài viết cung cấp thông tin hữu ích và kiến thức y học dựa trên khoa học để giúp mọi người hiểu rõ hơn về vấn đề sức khỏe quan trọng này. Nội dung được biên soạn bởi các chuyên gia y tế có kinh nghiệm.</p>", new DateTime(2025, 9, 5, 20, 55, 30, 551, DateTimeKind.Utc).AddTicks(320), false, true, false, true, new DateTime(2025, 9, 9, 11, 3, 15, 435, DateTimeKind.Utc).AddTicks(9566), 23, 15, "chu-de-3-thao-luan-suc-khoe", "Bài tập thể dục phù hợp cho người cao tuổi", new DateTime(2025, 9, 16, 21, 3, 1, 897, DateTimeKind.Utc).AddTicks(1468), 4, 190 },
                    { 4, 5, "<p>Đây là nội dung chi tiết về chủ đề 'Triệu chứng và cách điều trị đau đầu thường gặp'. Bài viết cung cấp thông tin hữu ích và kiến thức y học dựa trên khoa học để giúp mọi người hiểu rõ hơn về vấn đề sức khỏe quan trọng này. Nội dung được biên soạn bởi các chuyên gia y tế có kinh nghiệm.</p>", new DateTime(2025, 9, 12, 22, 13, 42, 363, DateTimeKind.Utc).AddTicks(6915), false, true, false, true, new DateTime(2025, 9, 14, 19, 13, 42, 363, DateTimeKind.Utc).AddTicks(6915), 16, 3, "chu-de-4-thao-luan-suc-khoe", "Triệu chứng và cách điều trị đau đầu thường gặp", new DateTime(2025, 9, 18, 23, 42, 48, 8, DateTimeKind.Utc).AddTicks(5479), 5, 429 },
                    { 5, 6, "<p>Đây là nội dung chi tiết về chủ đề 'Tầm quan trọng của việc khám sức khỏe định kỳ'. Bài viết cung cấp thông tin hữu ích và kiến thức y học dựa trên khoa học để giúp mọi người hiểu rõ hơn về vấn đề sức khỏe quan trọng này. Nội dung được biên soạn bởi các chuyên gia y tế có kinh nghiệm.</p>", new DateTime(2025, 9, 11, 16, 11, 53, 868, DateTimeKind.Utc).AddTicks(7875), false, true, false, true, new DateTime(2025, 9, 13, 10, 11, 53, 868, DateTimeKind.Utc).AddTicks(7875), 4, 9, "chu-de-5-thao-luan-suc-khoe", "Tầm quan trọng của việc khám sức khỏe định kỳ", new DateTime(2025, 9, 12, 0, 44, 38, 329, DateTimeKind.Utc).AddTicks(1043), 6, 478 },
                    { 6, 7, "<p>Đây là nội dung chi tiết về chủ đề 'Cách chăm sóc da trong mùa khô hanh'. Bài viết cung cấp thông tin hữu ích và kiến thức y học dựa trên khoa học để giúp mọi người hiểu rõ hơn về vấn đề sức khỏe quan trọng này. Nội dung được biên soạn bởi các chuyên gia y tế có kinh nghiệm.</p>", new DateTime(2025, 9, 6, 14, 57, 28, 487, DateTimeKind.Utc).AddTicks(1951), false, true, false, false, new DateTime(2025, 9, 16, 20, 46, 8, 522, DateTimeKind.Utc).AddTicks(4859), 1, 5, "chu-de-6-thao-luan-suc-khoe", "Cách chăm sóc da trong mùa khô hanh", new DateTime(2025, 9, 17, 13, 9, 23, 55, DateTimeKind.Utc).AddTicks(5978), 7, 446 },
                    { 7, 8, "<p>Đây là nội dung chi tiết về chủ đề 'Phương pháp giảm stress tự nhiên và hiệu quả'. Bài viết cung cấp thông tin hữu ích và kiến thức y học dựa trên khoa học để giúp mọi người hiểu rõ hơn về vấn đề sức khỏe quan trọng này. Nội dung được biên soạn bởi các chuyên gia y tế có kinh nghiệm.</p>", new DateTime(2025, 9, 11, 10, 39, 53, 22, DateTimeKind.Utc).AddTicks(1075), true, true, false, false, new DateTime(2025, 9, 17, 0, 49, 24, 699, DateTimeKind.Utc).AddTicks(4769), 34, 13, "chu-de-7-thao-luan-suc-khoe", "Phương pháp giảm stress tự nhiên và hiệu quả", new DateTime(2025, 9, 11, 10, 39, 53, 22, DateTimeKind.Utc).AddTicks(1075), 8, 211 },
                    { 8, 9, "<p>Đây là nội dung chi tiết về chủ đề 'Chế độ dinh dưỡng cân bằng cho trẻ em'. Bài viết cung cấp thông tin hữu ích và kiến thức y học dựa trên khoa học để giúp mọi người hiểu rõ hơn về vấn đề sức khỏe quan trọng này. Nội dung được biên soạn bởi các chuyên gia y tế có kinh nghiệm.</p>", new DateTime(2025, 9, 18, 17, 22, 26, 302, DateTimeKind.Utc).AddTicks(9424), false, true, false, false, new DateTime(2025, 9, 20, 0, 22, 26, 302, DateTimeKind.Utc).AddTicks(9424), 2, 6, "chu-de-8-thao-luan-suc-khoe", "Chế độ dinh dưỡng cân bằng cho trẻ em", new DateTime(2025, 9, 18, 17, 22, 26, 302, DateTimeKind.Utc).AddTicks(9424), 9, 378 },
                    { 9, 10, "<p>Đây là nội dung chi tiết về chủ đề 'Cách nhận biết và xử lý cơn đau tim'. Bài viết cung cấp thông tin hữu ích và kiến thức y học dựa trên khoa học để giúp mọi người hiểu rõ hơn về vấn đề sức khỏe quan trọng này. Nội dung được biên soạn bởi các chuyên gia y tế có kinh nghiệm.</p>", new DateTime(2025, 9, 1, 18, 24, 2, 678, DateTimeKind.Utc).AddTicks(8808), false, true, false, false, new DateTime(2025, 9, 10, 9, 27, 3, 308, DateTimeKind.Utc).AddTicks(1960), 17, 4, "chu-de-9-thao-luan-suc-khoe", "Cách nhận biết và xử lý cơn đau tim", new DateTime(2025, 9, 13, 8, 2, 58, 379, DateTimeKind.Utc).AddTicks(9794), 10, 188 },
                    { 10, 11, "<p>Đây là nội dung chi tiết về chủ đề 'Tác hại của việc thức khuya đối với sức khỏe'. Bài viết cung cấp thông tin hữu ích và kiến thức y học dựa trên khoa học để giúp mọi người hiểu rõ hơn về vấn đề sức khỏe quan trọng này. Nội dung được biên soạn bởi các chuyên gia y tế có kinh nghiệm.</p>", new DateTime(2025, 9, 11, 21, 25, 6, 125, DateTimeKind.Utc).AddTicks(5731), false, true, false, false, new DateTime(2025, 9, 11, 21, 34, 31, 695, DateTimeKind.Utc).AddTicks(8294), 33, 1, "chu-de-10-thao-luan-suc-khoe", "Tác hại của việc thức khuya đối với sức khỏe", new DateTime(2025, 9, 12, 14, 4, 3, 526, DateTimeKind.Utc).AddTicks(3507), 11, 167 },
                    { 11, 12, "<p>Đây là nội dung chi tiết về chủ đề 'Lợi ích của việc uống đủ nước mỗi ngày'. Bài viết cung cấp thông tin hữu ích và kiến thức y học dựa trên khoa học để giúp mọi người hiểu rõ hơn về vấn đề sức khỏe quan trọng này. Nội dung được biên soạn bởi các chuyên gia y tế có kinh nghiệm.</p>", new DateTime(2025, 9, 3, 10, 42, 51, 868, DateTimeKind.Utc).AddTicks(9297), false, true, false, false, new DateTime(2025, 9, 6, 5, 39, 27, 406, DateTimeKind.Utc).AddTicks(3040), 18, 0, "chu-de-11-thao-luan-suc-khoe", "Lợi ích của việc uống đủ nước mỗi ngày", new DateTime(2025, 9, 16, 3, 58, 47, 876, DateTimeKind.Utc).AddTicks(4812), 12, 489 },
                    { 12, 13, "<p>Đây là nội dung chi tiết về chủ đề 'Cách chăm sóc răng miệng đúng cách'. Bài viết cung cấp thông tin hữu ích và kiến thức y học dựa trên khoa học để giúp mọi người hiểu rõ hơn về vấn đề sức khỏe quan trọng này. Nội dung được biên soạn bởi các chuyên gia y tế có kinh nghiệm.</p>", new DateTime(2025, 9, 11, 10, 16, 43, 791, DateTimeKind.Utc).AddTicks(9114), false, true, false, false, new DateTime(2025, 9, 13, 7, 16, 43, 791, DateTimeKind.Utc).AddTicks(9114), 32, 2, "chu-de-12-thao-luan-suc-khoe", "Cách chăm sóc răng miệng đúng cách", new DateTime(2025, 9, 11, 10, 16, 43, 791, DateTimeKind.Utc).AddTicks(9114), 13, 99 },
                    { 13, 14, "<p>Đây là nội dung chi tiết về chủ đề 'Phòng ngừa bệnh cao huyết áp từ sớm'. Bài viết cung cấp thông tin hữu ích và kiến thức y học dựa trên khoa học để giúp mọi người hiểu rõ hơn về vấn đề sức khỏe quan trọng này. Nội dung được biên soạn bởi các chuyên gia y tế có kinh nghiệm.</p>", new DateTime(2025, 9, 4, 5, 51, 34, 892, DateTimeKind.Utc).AddTicks(6846), false, true, false, false, new DateTime(2025, 9, 4, 13, 53, 56, 72, DateTimeKind.Utc).AddTicks(3305), 32, 17, "chu-de-13-thao-luan-suc-khoe", "Phòng ngừa bệnh cao huyết áp từ sớm", new DateTime(2025, 9, 12, 19, 18, 9, 887, DateTimeKind.Utc).AddTicks(8171), 14, 324 },
                    { 14, 15, "<p>Đây là nội dung chi tiết về chủ đề 'Tác động nghiêm trọng của thuốc lá'. Bài viết cung cấp thông tin hữu ích và kiến thức y học dựa trên khoa học để giúp mọi người hiểu rõ hơn về vấn đề sức khỏe quan trọng này. Nội dung được biên soạn bởi các chuyên gia y tế có kinh nghiệm.</p>", new DateTime(2025, 9, 7, 9, 53, 31, 994, DateTimeKind.Utc).AddTicks(4955), true, true, false, false, new DateTime(2025, 9, 8, 13, 59, 44, 867, DateTimeKind.Utc).AddTicks(5041), 10, 15, "chu-de-14-thao-luan-suc-khoe", "Tác động nghiêm trọng của thuốc lá", new DateTime(2025, 9, 12, 1, 49, 7, 256, DateTimeKind.Utc).AddTicks(2668), 15, 219 },
                    { 15, 16, "<p>Đây là nội dung chi tiết về chủ đề 'Cách giữ gìn sức khỏe tim mạch'. Bài viết cung cấp thông tin hữu ích và kiến thức y học dựa trên khoa học để giúp mọi người hiểu rõ hơn về vấn đề sức khỏe quan trọng này. Nội dung được biên soạn bởi các chuyên gia y tế có kinh nghiệm.</p>", new DateTime(2025, 9, 10, 23, 29, 42, 559, DateTimeKind.Utc).AddTicks(4699), false, true, false, false, new DateTime(2025, 9, 13, 2, 22, 42, 320, DateTimeKind.Utc).AddTicks(1298), 33, 19, "chu-de-15-thao-luan-suc-khoe", "Cách giữ gìn sức khỏe tim mạch", new DateTime(2025, 9, 10, 23, 29, 42, 559, DateTimeKind.Utc).AddTicks(4699), 16, 211 },
                    { 16, 17, "<p>Đây là nội dung chi tiết về chủ đề 'Chế độ ăn dành cho người bị dạ dày'. Bài viết cung cấp thông tin hữu ích và kiến thức y học dựa trên khoa học để giúp mọi người hiểu rõ hơn về vấn đề sức khỏe quan trọng này. Nội dung được biên soạn bởi các chuyên gia y tế có kinh nghiệm.</p>", new DateTime(2025, 9, 13, 12, 58, 43, 311, DateTimeKind.Utc).AddTicks(7970), false, true, false, false, new DateTime(2025, 9, 14, 3, 58, 43, 311, DateTimeKind.Utc).AddTicks(7970), 40, 13, "chu-de-16-thao-luan-suc-khoe", "Chế độ ăn dành cho người bị dạ dày", new DateTime(2025, 9, 13, 12, 58, 43, 311, DateTimeKind.Utc).AddTicks(7970), 17, 274 },
                    { 17, 18, "<p>Đây là nội dung chi tiết về chủ đề 'Tầm quan trọng của giấc ngủ đối với sức khỏe'. Bài viết cung cấp thông tin hữu ích và kiến thức y học dựa trên khoa học để giúp mọi người hiểu rõ hơn về vấn đề sức khỏe quan trọng này. Nội dung được biên soạn bởi các chuyên gia y tế có kinh nghiệm.</p>", new DateTime(2025, 9, 11, 14, 33, 42, 220, DateTimeKind.Utc).AddTicks(2811), false, true, false, false, new DateTime(2025, 9, 11, 16, 33, 42, 220, DateTimeKind.Utc).AddTicks(2811), 25, 17, "chu-de-17-thao-luan-suc-khoe", "Tầm quan trọng của giấc ngủ đối với sức khỏe", new DateTime(2025, 9, 14, 12, 40, 30, 269, DateTimeKind.Utc).AddTicks(2290), 18, 495 },
                    { 18, 19, "<p>Đây là nội dung chi tiết về chủ đề 'Cách phòng ngừa bệnh ung thư hiệu quả'. Bài viết cung cấp thông tin hữu ích và kiến thức y học dựa trên khoa học để giúp mọi người hiểu rõ hơn về vấn đề sức khỏe quan trọng này. Nội dung được biên soạn bởi các chuyên gia y tế có kinh nghiệm.</p>", new DateTime(2025, 9, 8, 10, 33, 39, 999, DateTimeKind.Utc).AddTicks(1251), false, true, false, false, new DateTime(2025, 9, 10, 7, 33, 39, 999, DateTimeKind.Utc).AddTicks(1251), 20, 0, "chu-de-18-thao-luan-suc-khoe", "Cách phòng ngừa bệnh ung thư hiệu quả", new DateTime(2025, 9, 8, 13, 30, 25, 497, DateTimeKind.Utc).AddTicks(8516), 19, 285 },
                    { 19, 20, "<p>Đây là nội dung chi tiết về chủ đề 'Chăm sóc sức khỏe bà mẹ mang thai'. Bài viết cung cấp thông tin hữu ích và kiến thức y học dựa trên khoa học để giúp mọi người hiểu rõ hơn về vấn đề sức khỏe quan trọng này. Nội dung được biên soạn bởi các chuyên gia y tế có kinh nghiệm.</p>", new DateTime(2025, 9, 9, 7, 16, 40, 324, DateTimeKind.Utc).AddTicks(4677), false, true, false, false, new DateTime(2025, 9, 9, 17, 11, 28, 90, DateTimeKind.Utc).AddTicks(5837), 21, 1, "chu-de-19-thao-luan-suc-khoe", "Chăm sóc sức khỏe bà mẹ mang thai", new DateTime(2025, 9, 9, 7, 16, 40, 324, DateTimeKind.Utc).AddTicks(4677), 20, 360 },
                    { 20, 21, "<p>Đây là nội dung chi tiết về chủ đề 'Tác dụng của việc tập yoga đối với sức khỏe'. Bài viết cung cấp thông tin hữu ích và kiến thức y học dựa trên khoa học để giúp mọi người hiểu rõ hơn về vấn đề sức khỏe quan trọng này. Nội dung được biên soạn bởi các chuyên gia y tế có kinh nghiệm.</p>", new DateTime(2025, 9, 4, 9, 23, 26, 366, DateTimeKind.Utc).AddTicks(3236), false, true, false, false, new DateTime(2025, 9, 9, 8, 4, 0, 703, DateTimeKind.Utc).AddTicks(6152), 47, 10, "chu-de-20-thao-luan-suc-khoe", "Tác dụng của việc tập yoga đối với sức khỏe", new DateTime(2025, 9, 15, 8, 16, 22, 935, DateTimeKind.Utc).AddTicks(9641), 21, 454 },
                    { 21, 22, "<p>Đây là nội dung chi tiết về chủ đề 'Lợi ích của việc ăn rau xanh hàng ngày'. Bài viết cung cấp thông tin hữu ích và kiến thức y học dựa trên khoa học để giúp mọi người hiểu rõ hơn về vấn đề sức khỏe quan trọng này. Nội dung được biên soạn bởi các chuyên gia y tế có kinh nghiệm.</p>", new DateTime(2025, 9, 4, 18, 2, 16, 957, DateTimeKind.Utc).AddTicks(6721), true, true, false, false, new DateTime(2025, 9, 16, 21, 25, 55, 391, DateTimeKind.Utc).AddTicks(2887), 32, 16, "chu-de-21-thao-luan-suc-khoe", "Lợi ích của việc ăn rau xanh hàng ngày", new DateTime(2025, 9, 18, 1, 12, 18, 730, DateTimeKind.Utc).AddTicks(4153), 22, 320 },
                    { 22, 23, "<p>Đây là nội dung chi tiết về chủ đề 'Cách phòng ngừa bệnh tiểu đường tuýp 2'. Bài viết cung cấp thông tin hữu ích và kiến thức y học dựa trên khoa học để giúp mọi người hiểu rõ hơn về vấn đề sức khỏe quan trọng này. Nội dung được biên soạn bởi các chuyên gia y tế có kinh nghiệm.</p>", new DateTime(2025, 9, 10, 2, 10, 56, 175, DateTimeKind.Utc).AddTicks(6921), false, true, false, false, new DateTime(2025, 9, 11, 22, 10, 56, 175, DateTimeKind.Utc).AddTicks(6921), 45, 12, "chu-de-22-thao-luan-suc-khoe", "Cách phòng ngừa bệnh tiểu đường tuýp 2", new DateTime(2025, 9, 10, 2, 10, 56, 175, DateTimeKind.Utc).AddTicks(6921), 23, 63 },
                    { 23, 24, "<p>Đây là nội dung chi tiết về chủ đề 'Chăm sóc sức khỏe răng miệng cho trẻ em'. Bài viết cung cấp thông tin hữu ích và kiến thức y học dựa trên khoa học để giúp mọi người hiểu rõ hơn về vấn đề sức khỏe quan trọng này. Nội dung được biên soạn bởi các chuyên gia y tế có kinh nghiệm.</p>", new DateTime(2025, 9, 11, 5, 51, 2, 474, DateTimeKind.Utc).AddTicks(4990), false, true, false, false, new DateTime(2025, 9, 11, 6, 51, 2, 474, DateTimeKind.Utc).AddTicks(4990), 19, 2, "chu-de-23-thao-luan-suc-khoe", "Chăm sóc sức khỏe răng miệng cho trẻ em", new DateTime(2025, 9, 11, 5, 51, 2, 474, DateTimeKind.Utc).AddTicks(4990), 24, 75 },
                    { 24, 25, "<p>Đây là nội dung chi tiết về chủ đề 'Phương pháp tăng cường hệ miễn dịch'. Bài viết cung cấp thông tin hữu ích và kiến thức y học dựa trên khoa học để giúp mọi người hiểu rõ hơn về vấn đề sức khỏe quan trọng này. Nội dung được biên soạn bởi các chuyên gia y tế có kinh nghiệm.</p>", new DateTime(2025, 9, 12, 18, 2, 21, 315, DateTimeKind.Utc).AddTicks(493), false, true, false, false, new DateTime(2025, 9, 13, 13, 2, 21, 315, DateTimeKind.Utc).AddTicks(493), 41, 16, "chu-de-24-thao-luan-suc-khoe", "Phương pháp tăng cường hệ miễn dịch", new DateTime(2025, 9, 12, 18, 2, 21, 315, DateTimeKind.Utc).AddTicks(493), 25, 459 },
                    { 25, 26, "<p>Đây là nội dung chi tiết về chủ đề 'Cách điều trị và phòng ngừa đau lưng'. Bài viết cung cấp thông tin hữu ích và kiến thức y học dựa trên khoa học để giúp mọi người hiểu rõ hơn về vấn đề sức khỏe quan trọng này. Nội dung được biên soạn bởi các chuyên gia y tế có kinh nghiệm.</p>", new DateTime(2025, 9, 11, 18, 26, 50, 261, DateTimeKind.Utc).AddTicks(6736), false, true, false, false, new DateTime(2025, 9, 15, 6, 17, 18, 284, DateTimeKind.Utc).AddTicks(3169), 27, 1, "chu-de-25-thao-luan-suc-khoe", "Cách điều trị và phòng ngừa đau lưng", new DateTime(2025, 9, 11, 18, 26, 50, 261, DateTimeKind.Utc).AddTicks(6736), 26, 151 },
                    { 26, 27, "<p>Đây là nội dung chi tiết về chủ đề 'Lợi ích của việc đi bộ hàng ngày'. Bài viết cung cấp thông tin hữu ích và kiến thức y học dựa trên khoa học để giúp mọi người hiểu rõ hơn về vấn đề sức khỏe quan trọng này. Nội dung được biên soạn bởi các chuyên gia y tế có kinh nghiệm.</p>", new DateTime(2025, 9, 1, 0, 27, 10, 93, DateTimeKind.Utc).AddTicks(4301), false, true, false, false, new DateTime(2025, 9, 2, 16, 39, 35, 607, DateTimeKind.Utc).AddTicks(3516), 39, 10, "chu-de-26-thao-luan-suc-khoe", "Lợi ích của việc đi bộ hàng ngày", new DateTime(2025, 9, 12, 17, 14, 6, 437, DateTimeKind.Utc).AddTicks(2583), 27, 269 },
                    { 27, 28, "<p>Đây là nội dung chi tiết về chủ đề 'Chế độ ăn giúp giảm cholesterol'. Bài viết cung cấp thông tin hữu ích và kiến thức y học dựa trên khoa học để giúp mọi người hiểu rõ hơn về vấn đề sức khỏe quan trọng này. Nội dung được biên soạn bởi các chuyên gia y tế có kinh nghiệm.</p>", new DateTime(2025, 9, 13, 22, 59, 47, 100, DateTimeKind.Utc).AddTicks(132), false, true, false, false, new DateTime(2025, 9, 15, 15, 59, 47, 100, DateTimeKind.Utc).AddTicks(132), 18, 8, "chu-de-27-thao-luan-suc-khoe", "Chế độ ăn giúp giảm cholesterol", new DateTime(2025, 9, 17, 5, 20, 12, 877, DateTimeKind.Utc).AddTicks(5503), 28, 446 },
                    { 28, 29, "<p>Đây là nội dung chi tiết về chủ đề 'Cách nhận biết và xử lý chứng mất ngủ'. Bài viết cung cấp thông tin hữu ích và kiến thức y học dựa trên khoa học để giúp mọi người hiểu rõ hơn về vấn đề sức khỏe quan trọng này. Nội dung được biên soạn bởi các chuyên gia y tế có kinh nghiệm.</p>", new DateTime(2025, 9, 15, 7, 30, 10, 452, DateTimeKind.Utc).AddTicks(3612), true, true, false, false, new DateTime(2025, 9, 17, 22, 44, 44, 201, DateTimeKind.Utc).AddTicks(3923), 33, 1, "chu-de-28-thao-luan-suc-khoe", "Cách nhận biết và xử lý chứng mất ngủ", new DateTime(2025, 9, 16, 5, 0, 17, 532, DateTimeKind.Utc).AddTicks(5526), 29, 407 },
                    { 29, 30, "<p>Đây là nội dung chi tiết về chủ đề 'Phương pháp chăm sóc da mặt tự nhiên'. Bài viết cung cấp thông tin hữu ích và kiến thức y học dựa trên khoa học để giúp mọi người hiểu rõ hơn về vấn đề sức khỏe quan trọng này. Nội dung được biên soạn bởi các chuyên gia y tế có kinh nghiệm.</p>", new DateTime(2025, 9, 15, 8, 36, 55, 587, DateTimeKind.Utc).AddTicks(7499), false, true, false, false, new DateTime(2025, 9, 16, 6, 17, 16, 909, DateTimeKind.Utc).AddTicks(9602), 35, 19, "chu-de-29-thao-luan-suc-khoe", "Phương pháp chăm sóc da mặt tự nhiên", new DateTime(2025, 9, 17, 19, 43, 41, 499, DateTimeKind.Utc).AddTicks(3981), 30, 101 },
                    { 30, 1, "<p>Đây là nội dung chi tiết về chủ đề 'Tác dụng của vitamin D với sức khỏe'. Bài viết cung cấp thông tin hữu ích và kiến thức y học dựa trên khoa học để giúp mọi người hiểu rõ hơn về vấn đề sức khỏe quan trọng này. Nội dung được biên soạn bởi các chuyên gia y tế có kinh nghiệm.</p>", new DateTime(2025, 9, 15, 12, 19, 33, 982, DateTimeKind.Utc).AddTicks(7509), false, true, false, false, new DateTime(2025, 9, 17, 3, 19, 33, 982, DateTimeKind.Utc).AddTicks(7509), 10, 5, "chu-de-30-thao-luan-suc-khoe", "Tác dụng của vitamin D với sức khỏe", new DateTime(2025, 9, 15, 15, 39, 55, 990, DateTimeKind.Utc).AddTicks(6873), 1, 454 }
                });

            migrationBuilder.InsertData(
                table: "Likes",
                columns: new[] { "Id", "CreatedAt", "PostId", "TopicId", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 9, 15, 19, 50, 0, 921, DateTimeKind.Utc).AddTicks(2638), null, 1, 2 },
                    { 2, new DateTime(2025, 9, 3, 0, 8, 32, 724, DateTimeKind.Utc).AddTicks(5626), null, 2, 3 },
                    { 3, new DateTime(2025, 9, 9, 12, 21, 9, 721, DateTimeKind.Utc).AddTicks(1784), null, 3, 4 },
                    { 4, new DateTime(2025, 9, 4, 16, 10, 28, 413, DateTimeKind.Utc).AddTicks(2163), null, 4, 5 },
                    { 5, new DateTime(2025, 9, 13, 6, 29, 29, 389, DateTimeKind.Utc).AddTicks(5047), null, 5, 6 },
                    { 6, new DateTime(2025, 9, 18, 1, 34, 12, 426, DateTimeKind.Utc).AddTicks(2477), null, 6, 7 },
                    { 7, new DateTime(2025, 9, 2, 18, 43, 2, 629, DateTimeKind.Utc).AddTicks(4047), null, 7, 8 },
                    { 8, new DateTime(2025, 9, 6, 7, 17, 18, 328, DateTimeKind.Utc).AddTicks(3218), null, 8, 9 },
                    { 9, new DateTime(2025, 9, 13, 23, 37, 26, 776, DateTimeKind.Utc).AddTicks(1055), null, 9, 10 },
                    { 10, new DateTime(2025, 9, 7, 19, 18, 50, 557, DateTimeKind.Utc).AddTicks(8262), null, 10, 11 },
                    { 11, new DateTime(2025, 9, 10, 18, 4, 48, 788, DateTimeKind.Utc).AddTicks(5334), null, 11, 12 },
                    { 12, new DateTime(2025, 9, 9, 14, 5, 47, 643, DateTimeKind.Utc).AddTicks(5032), null, 12, 13 },
                    { 13, new DateTime(2025, 9, 18, 17, 42, 44, 540, DateTimeKind.Utc).AddTicks(3315), null, 13, 14 },
                    { 14, new DateTime(2025, 9, 3, 18, 3, 21, 467, DateTimeKind.Utc).AddTicks(4333), null, 14, 15 },
                    { 15, new DateTime(2025, 9, 12, 7, 56, 12, 991, DateTimeKind.Utc).AddTicks(5653), null, 15, 16 }
                });

            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "Id", "Content", "CreatedAt", "IsActive", "IsAnswer", "LikeCount", "ParentPostId", "TopicId", "UpdatedAt", "UserId" },
                values: new object[,]
                {
                    { 1, "<p>Cảm ơn bạn đã chia sẻ thông tin hữu ích này! Đây là bình luận chi tiết về chủ đề đang được thảo luận. Tôi hy vọng thông tin này sẽ hữu ích cho cộng đồng.</p>", new DateTime(2025, 9, 7, 11, 40, 51, 414, DateTimeKind.Utc).AddTicks(6939), true, false, 14, null, 2, new DateTime(2025, 9, 7, 11, 40, 51, 414, DateTimeKind.Utc).AddTicks(6939), 2 },
                    { 2, "<p>Tôi cũng gặp vấn đề tương tự, lời khuyên của bạn rất hay. Đây là bình luận chi tiết về chủ đề đang được thảo luận. Tôi hy vọng thông tin này sẽ hữu ích cho cộng đồng.</p>", new DateTime(2025, 9, 17, 12, 45, 44, 455, DateTimeKind.Utc).AddTicks(6342), true, false, 22, null, 3, new DateTime(2025, 9, 17, 12, 45, 44, 455, DateTimeKind.Utc).AddTicks(6342), 3 },
                    { 3, "<p>Bác sĩ có thể tư vấn thêm về vấn đề này không ạ? Đây là bình luận chi tiết về chủ đề đang được thảo luận. Tôi hy vọng thông tin này sẽ hữu ích cho cộng đồng.</p>", new DateTime(2025, 9, 2, 1, 51, 47, 826, DateTimeKind.Utc).AddTicks(2207), true, false, 21, null, 4, new DateTime(2025, 9, 2, 1, 51, 47, 826, DateTimeKind.Utc).AddTicks(2207), 4 },
                    { 4, "<p>Theo kinh nghiệm của tôi thì phương pháp này khá hiệu quả. Đây là bình luận chi tiết về chủ đề đang được thảo luận. Tôi hy vọng thông tin này sẽ hữu ích cho cộng đồng.</p>", new DateTime(2025, 9, 7, 6, 7, 31, 508, DateTimeKind.Utc).AddTicks(9884), true, false, 21, null, 5, new DateTime(2025, 9, 7, 6, 7, 31, 508, DateTimeKind.Utc).AddTicks(9884), 5 },
                    { 5, "<p>Xin chào, tôi muốn hỏi thêm về triệu chứng này. Đây là bình luận chi tiết về chủ đề đang được thảo luận. Tôi hy vọng thông tin này sẽ hữu ích cho cộng đồng.</p>", new DateTime(2025, 9, 8, 0, 17, 16, 784, DateTimeKind.Utc).AddTicks(183), true, false, 2, null, 6, new DateTime(2025, 9, 8, 0, 17, 16, 784, DateTimeKind.Utc).AddTicks(183), 6 },
                    { 6, "<p>Rất hữu ích, tôi sẽ áp dụng ngay. Cảm ơn bạn! Đây là bình luận chi tiết về chủ đề đang được thảo luận. Tôi hy vọng thông tin này sẽ hữu ích cho cộng đồng.</p>", new DateTime(2025, 9, 18, 13, 29, 5, 544, DateTimeKind.Utc).AddTicks(6687), true, false, 3, null, 7, new DateTime(2025, 9, 18, 13, 29, 5, 544, DateTimeKind.Utc).AddTicks(6687), 7 },
                    { 7, "<p>Tôi nghĩ nên tham khảo ý kiến bác sĩ chuyên khoa. Đây là bình luận chi tiết về chủ đề đang được thảo luận. Tôi hy vọng thông tin này sẽ hữu ích cho cộng đồng.</p>", new DateTime(2025, 9, 16, 21, 32, 5, 78, DateTimeKind.Utc).AddTicks(8468), true, false, 12, null, 8, new DateTime(2025, 9, 16, 21, 32, 5, 78, DateTimeKind.Utc).AddTicks(8468), 8 },
                    { 8, "<p>Điều này hoàn toàn đúng, tôi đã trải nghiệm rồi. Đây là bình luận chi tiết về chủ đề đang được thảo luận. Tôi hy vọng thông tin này sẽ hữu ích cho cộng đồng.</p>", new DateTime(2025, 9, 18, 10, 20, 53, 751, DateTimeKind.Utc).AddTicks(4937), true, false, 0, null, 9, new DateTime(2025, 9, 18, 10, 20, 53, 751, DateTimeKind.Utc).AddTicks(4937), 9 },
                    { 9, "<p>Có ai biết thêm thông tin về vấn đề này không? Đây là bình luận chi tiết về chủ đề đang được thảo luận. Tôi hy vọng thông tin này sẽ hữu ích cho cộng đồng.</p>", new DateTime(2025, 9, 10, 23, 20, 40, 12, DateTimeKind.Utc).AddTicks(1277), true, false, 16, null, 10, new DateTime(2025, 9, 10, 23, 20, 40, 12, DateTimeKind.Utc).AddTicks(1277), 10 },
                    { 10, "<p>Bài viết rất chi tiết và dễ hiểu. Cảm ơn tác giả! Đây là bình luận chi tiết về chủ đề đang được thảo luận. Tôi hy vọng thông tin này sẽ hữu ích cho cộng đồng.</p>", new DateTime(2025, 9, 6, 17, 13, 25, 587, DateTimeKind.Utc).AddTicks(9809), true, true, 3, null, 11, new DateTime(2025, 9, 6, 17, 13, 25, 587, DateTimeKind.Utc).AddTicks(9809), 11 },
                    { 11, "<p>Tôi sẽ thử áp dụng và chia sẻ kết quả sau. Đây là bình luận chi tiết về chủ đề đang được thảo luận. Tôi hy vọng thông tin này sẽ hữu ích cho cộng đồng.</p>", new DateTime(2025, 9, 4, 14, 3, 38, 574, DateTimeKind.Utc).AddTicks(2885), true, false, 18, null, 12, new DateTime(2025, 9, 4, 14, 3, 38, 574, DateTimeKind.Utc).AddTicks(2885), 12 },
                    { 12, "<p>Phương pháp này có phù hợp với trẻ em không ạ? Đây là bình luận chi tiết về chủ đề đang được thảo luận. Tôi hy vọng thông tin này sẽ hữu ích cho cộng đồng.</p>", new DateTime(2025, 9, 3, 1, 57, 0, 156, DateTimeKind.Utc).AddTicks(9012), true, false, 3, null, 13, new DateTime(2025, 9, 3, 1, 57, 0, 156, DateTimeKind.Utc).AddTicks(9012), 13 },
                    { 13, "<p>Cần lưu ý gì khi thực hiện theo hướng dẫn này? Đây là bình luận chi tiết về chủ đề đang được thảo luận. Tôi hy vọng thông tin này sẽ hữu ích cho cộng đồng.</p>", new DateTime(2025, 9, 9, 8, 34, 43, 263, DateTimeKind.Utc).AddTicks(1071), true, false, 9, null, 14, new DateTime(2025, 9, 9, 8, 34, 43, 263, DateTimeKind.Utc).AddTicks(1071), 14 },
                    { 14, "<p>Tôi có thể tìm hiểu thêm ở đâu về chủ đề này? Đây là bình luận chi tiết về chủ đề đang được thảo luận. Tôi hy vọng thông tin này sẽ hữu ích cho cộng đồng.</p>", new DateTime(2025, 9, 11, 3, 40, 48, 347, DateTimeKind.Utc).AddTicks(1304), true, false, 24, null, 15, new DateTime(2025, 9, 11, 3, 40, 48, 347, DateTimeKind.Utc).AddTicks(1304), 15 },
                    { 15, "<p>Rất cảm ơn những chia sẻ quý báu này. Đây là bình luận chi tiết về chủ đề đang được thảo luận. Tôi hy vọng thông tin này sẽ hữu ích cho cộng đồng.</p>", new DateTime(2025, 9, 13, 18, 37, 32, 27, DateTimeKind.Utc).AddTicks(949), true, false, 6, null, 16, new DateTime(2025, 9, 13, 18, 37, 32, 27, DateTimeKind.Utc).AddTicks(949), 16 }
                });

            migrationBuilder.InsertData(
                table: "Reports",
                columns: new[] { "Id", "Category", "CreatedAt", "PostId", "Reason", "ReviewedAt", "ReviewedByAdminId", "Status", "TopicId", "UserId" },
                values: new object[,]
                {
                    { 1, "Nội dung không phù hợp", new DateTime(2025, 9, 12, 4, 49, 41, 339, DateTimeKind.Utc).AddTicks(4847), null, "Nội dung thư rác quảng cáo", null, null, "Đã xem xét", 1, 2 },
                    { 2, "Quấy rối", new DateTime(2025, 9, 2, 3, 26, 57, 303, DateTimeKind.Utc).AddTicks(9554), null, "Sử dụng ngôn từ không phù hợp", null, null, "Đã giải quyết", 2, 3 },
                    { 3, "Vi phạm bản quyền", new DateTime(2025, 9, 4, 2, 8, 22, 708, DateTimeKind.Utc).AddTicks(8722), null, "Quấy rối người dùng khác", new DateTime(2025, 9, 7, 21, 27, 42, 880, DateTimeKind.Utc).AddTicks(7423), 1, "Đã từ chối", 3, 4 },
                    { 4, "Khác", new DateTime(2025, 9, 8, 10, 34, 40, 276, DateTimeKind.Utc).AddTicks(1281), null, "Sao chép nội dung từ nguồn khác", null, null, "Chờ xử lý", 4, 5 },
                    { 5, "Thư rác", new DateTime(2025, 9, 6, 17, 9, 45, 782, DateTimeKind.Utc).AddTicks(9173), null, "Thông tin sai lệch về y tế", null, null, "Đã xem xét", 5, 6 },
                    { 6, "Nội dung không phù hợp", new DateTime(2025, 9, 14, 14, 10, 36, 999, DateTimeKind.Utc).AddTicks(7357), null, "Bài viết không liên quan chủ đề", new DateTime(2025, 9, 14, 22, 10, 36, 999, DateTimeKind.Utc).AddTicks(7357), 1, "Đã giải quyết", 6, 7 },
                    { 7, "Quấy rối", new DateTime(2025, 9, 15, 16, 20, 39, 863, DateTimeKind.Utc).AddTicks(1275), null, "Ngôn từ thù địch", null, null, "Đã từ chối", 7, 8 },
                    { 8, "Vi phạm bản quyền", new DateTime(2025, 9, 16, 15, 57, 58, 197, DateTimeKind.Utc).AddTicks(4456), null, "Chia sẻ thông tin cá nhân", null, null, "Chờ xử lý", 8, 9 },
                    { 9, "Khác", new DateTime(2025, 9, 4, 17, 28, 41, 262, DateTimeKind.Utc).AddTicks(4775), null, "Nội dung có tính chất thương mại", new DateTime(2025, 9, 11, 14, 14, 31, 879, DateTimeKind.Utc).AddTicks(1608), 1, "Đã xem xét", 9, 10 },
                    { 10, "Thư rác", new DateTime(2025, 9, 14, 3, 32, 34, 284, DateTimeKind.Utc).AddTicks(4860), null, "Vi phạm quy tắc cộng đồng", null, null, "Đã giải quyết", 10, 11 },
                    { 11, "Nội dung không phù hợp", new DateTime(2025, 9, 4, 11, 43, 22, 6, DateTimeKind.Utc).AddTicks(3985), null, "Đăng nhiều bài trùng lặp", null, null, "Đã từ chối", 11, 12 },
                    { 12, "Quấy rối", new DateTime(2025, 9, 18, 6, 48, 27, 565, DateTimeKind.Utc).AddTicks(6935), null, "Sử dụng hình ảnh không phù hợp", new DateTime(2025, 9, 19, 0, 48, 27, 565, DateTimeKind.Utc).AddTicks(6935), 1, "Chờ xử lý", 12, 13 },
                    { 13, "Vi phạm bản quyền", new DateTime(2025, 9, 9, 12, 28, 38, 414, DateTimeKind.Utc).AddTicks(1715), null, "Tuyên truyền thuốc không rõ nguồn gốc", null, null, "Đã xem xét", 13, 14 },
                    { 14, "Khác", new DateTime(2025, 9, 13, 14, 55, 53, 426, DateTimeKind.Utc).AddTicks(582), null, "Phát tán tin đồn sai sự thật", null, null, "Đã giải quyết", 14, 15 },
                    { 15, "Thư rác", new DateTime(2025, 9, 1, 3, 33, 15, 245, DateTimeKind.Utc).AddTicks(7318), null, "Bình luận mang tính chất xúc phạm", new DateTime(2025, 9, 10, 5, 31, 38, 248, DateTimeKind.Utc).AddTicks(9262), 1, "Đã từ chối", 15, 16 }
                });

            migrationBuilder.InsertData(
                table: "TopicTags",
                columns: new[] { "Id", "CreatedAt", "TagId", "TopicId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 9, 6, 2, 8, 11, 224, DateTimeKind.Utc).AddTicks(9072), 2, 2 },
                    { 2, new DateTime(2025, 9, 7, 14, 35, 20, 939, DateTimeKind.Utc).AddTicks(1096), 3, 3 },
                    { 3, new DateTime(2025, 9, 3, 12, 36, 7, 667, DateTimeKind.Utc).AddTicks(4127), 4, 4 },
                    { 4, new DateTime(2025, 9, 3, 1, 2, 20, 475, DateTimeKind.Utc).AddTicks(4097), 5, 5 },
                    { 5, new DateTime(2025, 9, 2, 23, 15, 3, 379, DateTimeKind.Utc).AddTicks(8803), 6, 6 },
                    { 6, new DateTime(2025, 9, 9, 21, 29, 19, 807, DateTimeKind.Utc).AddTicks(4653), 7, 7 },
                    { 7, new DateTime(2025, 9, 10, 5, 35, 45, 212, DateTimeKind.Utc).AddTicks(629), 8, 8 },
                    { 8, new DateTime(2025, 9, 7, 7, 52, 22, 20, DateTimeKind.Utc).AddTicks(7359), 9, 9 },
                    { 9, new DateTime(2025, 9, 9, 10, 23, 22, 908, DateTimeKind.Utc).AddTicks(2865), 10, 10 },
                    { 10, new DateTime(2025, 9, 13, 7, 55, 17, 763, DateTimeKind.Utc).AddTicks(8238), 11, 11 },
                    { 11, new DateTime(2025, 9, 3, 18, 29, 38, 501, DateTimeKind.Utc).AddTicks(1974), 12, 12 },
                    { 12, new DateTime(2025, 9, 12, 6, 57, 17, 516, DateTimeKind.Utc).AddTicks(8156), 13, 13 },
                    { 13, new DateTime(2025, 9, 14, 22, 49, 7, 685, DateTimeKind.Utc).AddTicks(1097), 14, 14 },
                    { 14, new DateTime(2025, 9, 7, 8, 35, 43, 973, DateTimeKind.Utc).AddTicks(5689), 15, 15 },
                    { 15, new DateTime(2025, 9, 15, 15, 50, 37, 44, DateTimeKind.Utc).AddTicks(6137), 16, 16 },
                    { 16, new DateTime(2025, 9, 18, 6, 44, 27, 277, DateTimeKind.Utc).AddTicks(4711), 17, 17 },
                    { 17, new DateTime(2025, 9, 7, 2, 12, 43, 662, DateTimeKind.Utc).AddTicks(5285), 18, 18 },
                    { 18, new DateTime(2025, 9, 6, 3, 4, 29, 0, DateTimeKind.Utc).AddTicks(9153), 19, 19 },
                    { 19, new DateTime(2025, 9, 6, 1, 21, 40, 818, DateTimeKind.Utc).AddTicks(1782), 20, 20 },
                    { 20, new DateTime(2025, 9, 8, 8, 54, 8, 262, DateTimeKind.Utc).AddTicks(5107), 21, 21 },
                    { 21, new DateTime(2025, 9, 10, 23, 11, 8, 200, DateTimeKind.Utc).AddTicks(263), 22, 22 },
                    { 22, new DateTime(2025, 9, 2, 23, 34, 39, 898, DateTimeKind.Utc).AddTicks(5123), 23, 23 },
                    { 23, new DateTime(2025, 9, 3, 13, 33, 30, 338, DateTimeKind.Utc).AddTicks(6215), 24, 24 },
                    { 24, new DateTime(2025, 9, 17, 16, 11, 39, 534, DateTimeKind.Utc).AddTicks(3321), 25, 25 },
                    { 25, new DateTime(2025, 9, 7, 22, 50, 45, 529, DateTimeKind.Utc).AddTicks(3317), 26, 26 },
                    { 26, new DateTime(2025, 9, 14, 9, 34, 7, 109, DateTimeKind.Utc).AddTicks(7945), 27, 27 },
                    { 27, new DateTime(2025, 9, 11, 20, 39, 42, 292, DateTimeKind.Utc).AddTicks(69), 28, 28 },
                    { 28, new DateTime(2025, 9, 9, 14, 22, 11, 722, DateTimeKind.Utc).AddTicks(9888), 29, 29 },
                    { 29, new DateTime(2025, 9, 17, 9, 13, 7, 782, DateTimeKind.Utc).AddTicks(280), 30, 30 },
                    { 30, new DateTime(2025, 9, 8, 17, 48, 53, 955, DateTimeKind.Utc).AddTicks(3673), 1, 1 }
                });

            migrationBuilder.InsertData(
                table: "TopicViews",
                columns: new[] { "Id", "IpAddress", "TopicId", "UserId", "ViewedAt" },
                values: new object[,]
                {
                    { 1, "192.168.227.9", 2, 2, new DateTime(2025, 9, 8, 22, 39, 13, 238, DateTimeKind.Utc).AddTicks(7079) },
                    { 2, "192.168.10.219", 3, 3, new DateTime(2025, 9, 1, 18, 21, 5, 776, DateTimeKind.Utc).AddTicks(6348) },
                    { 3, "192.168.40.146", 4, 4, new DateTime(2025, 9, 13, 2, 6, 13, 584, DateTimeKind.Utc).AddTicks(641) },
                    { 4, "192.168.221.219", 5, 5, new DateTime(2025, 9, 1, 18, 55, 25, 334, DateTimeKind.Utc).AddTicks(2066) },
                    { 5, "192.168.144.230", 6, 6, new DateTime(2025, 9, 16, 14, 49, 18, 473, DateTimeKind.Utc).AddTicks(9773) },
                    { 6, "192.168.20.32", 7, 7, new DateTime(2025, 9, 18, 19, 30, 7, 871, DateTimeKind.Utc).AddTicks(1881) },
                    { 7, "192.168.76.32", 8, 8, new DateTime(2025, 9, 5, 20, 21, 15, 941, DateTimeKind.Utc).AddTicks(1669) },
                    { 8, "192.168.20.39", 9, 9, new DateTime(2025, 9, 5, 11, 31, 1, 783, DateTimeKind.Utc).AddTicks(4161) },
                    { 9, "192.168.40.165", 10, 10, new DateTime(2025, 9, 12, 6, 54, 20, 270, DateTimeKind.Utc).AddTicks(5765) },
                    { 10, "192.168.109.21", 11, 11, new DateTime(2025, 9, 10, 3, 55, 58, 63, DateTimeKind.Utc).AddTicks(1099) },
                    { 11, "192.168.184.225", 12, 12, new DateTime(2025, 9, 11, 5, 5, 43, 382, DateTimeKind.Utc).AddTicks(2439) },
                    { 12, "192.168.91.124", 13, 13, new DateTime(2025, 9, 10, 2, 51, 15, 299, DateTimeKind.Utc).AddTicks(4593) },
                    { 13, "192.168.10.241", 14, 14, new DateTime(2025, 9, 6, 5, 58, 4, 624, DateTimeKind.Utc).AddTicks(8733) },
                    { 14, "192.168.150.28", 15, 15, new DateTime(2025, 9, 17, 6, 34, 10, 896, DateTimeKind.Utc).AddTicks(7341) },
                    { 15, "192.168.217.117", 16, 16, new DateTime(2025, 9, 15, 9, 40, 40, 350, DateTimeKind.Utc).AddTicks(5454) },
                    { 16, "192.168.59.156", 17, 17, new DateTime(2025, 9, 11, 11, 11, 21, 193, DateTimeKind.Utc).AddTicks(6340) },
                    { 17, "192.168.45.13", 18, 18, new DateTime(2025, 9, 1, 20, 41, 36, 968, DateTimeKind.Utc).AddTicks(2140) },
                    { 18, "192.168.60.233", 19, 19, new DateTime(2025, 9, 11, 20, 47, 40, 679, DateTimeKind.Utc).AddTicks(2074) },
                    { 19, "192.168.25.207", 20, 20, new DateTime(2025, 9, 16, 22, 1, 32, 959, DateTimeKind.Utc).AddTicks(9746) },
                    { 20, "192.168.49.225", 21, 21, new DateTime(2025, 9, 4, 19, 46, 19, 986, DateTimeKind.Utc).AddTicks(7868) },
                    { 21, "192.168.106.185", 22, null, new DateTime(2025, 9, 9, 19, 59, 30, 620, DateTimeKind.Utc).AddTicks(6059) },
                    { 22, "192.168.42.37", 23, null, new DateTime(2025, 9, 18, 13, 42, 49, 151, DateTimeKind.Utc).AddTicks(7422) },
                    { 23, "192.168.122.53", 24, null, new DateTime(2025, 9, 8, 11, 33, 36, 217, DateTimeKind.Utc).AddTicks(9842) },
                    { 24, "192.168.92.11", 25, null, new DateTime(2025, 9, 4, 4, 43, 37, 770, DateTimeKind.Utc).AddTicks(7683) },
                    { 25, "192.168.178.181", 26, null, new DateTime(2025, 9, 1, 6, 33, 8, 800, DateTimeKind.Utc).AddTicks(465) },
                    { 26, "192.168.93.58", 27, null, new DateTime(2025, 9, 13, 10, 18, 10, 953, DateTimeKind.Utc).AddTicks(5760) },
                    { 27, "192.168.114.235", 28, null, new DateTime(2025, 9, 1, 16, 14, 53, 15, DateTimeKind.Utc).AddTicks(922) },
                    { 28, "192.168.12.65", 29, null, new DateTime(2025, 9, 1, 14, 13, 9, 445, DateTimeKind.Utc).AddTicks(8816) },
                    { 29, "192.168.118.125", 30, null, new DateTime(2025, 9, 18, 10, 9, 33, 906, DateTimeKind.Utc).AddTicks(103) },
                    { 30, "192.168.245.67", 1, null, new DateTime(2025, 9, 13, 3, 34, 59, 347, DateTimeKind.Utc).AddTicks(2890) }
                });

            migrationBuilder.InsertData(
                table: "Likes",
                columns: new[] { "Id", "CreatedAt", "PostId", "TopicId", "UserId" },
                values: new object[,]
                {
                    { 16, new DateTime(2025, 9, 5, 17, 26, 52, 262, DateTimeKind.Utc).AddTicks(4612), 1, null, 17 },
                    { 17, new DateTime(2025, 9, 9, 13, 12, 51, 168, DateTimeKind.Utc).AddTicks(6275), 2, null, 18 },
                    { 18, new DateTime(2025, 9, 10, 10, 44, 18, 471, DateTimeKind.Utc).AddTicks(6274), 3, null, 19 },
                    { 19, new DateTime(2025, 9, 6, 21, 10, 12, 193, DateTimeKind.Utc).AddTicks(8893), 4, null, 20 },
                    { 20, new DateTime(2025, 9, 12, 21, 42, 5, 998, DateTimeKind.Utc).AddTicks(6118), 5, null, 21 },
                    { 21, new DateTime(2025, 9, 5, 22, 55, 44, 285, DateTimeKind.Utc).AddTicks(9052), 6, null, 22 },
                    { 22, new DateTime(2025, 9, 4, 3, 31, 5, 65, DateTimeKind.Utc).AddTicks(4906), 7, null, 23 },
                    { 23, new DateTime(2025, 9, 9, 17, 14, 11, 453, DateTimeKind.Utc).AddTicks(9861), 8, null, 24 },
                    { 24, new DateTime(2025, 9, 1, 19, 8, 3, 511, DateTimeKind.Utc).AddTicks(6228), 9, null, 25 },
                    { 25, new DateTime(2025, 9, 3, 5, 31, 19, 761, DateTimeKind.Utc).AddTicks(9288), 10, null, 26 },
                    { 26, new DateTime(2025, 9, 4, 2, 33, 31, 326, DateTimeKind.Utc).AddTicks(3948), 11, null, 27 },
                    { 27, new DateTime(2025, 9, 5, 1, 1, 50, 600, DateTimeKind.Utc).AddTicks(4880), 12, null, 28 },
                    { 28, new DateTime(2025, 9, 4, 20, 24, 27, 133, DateTimeKind.Utc).AddTicks(805), 13, null, 29 },
                    { 29, new DateTime(2025, 9, 14, 2, 11, 33, 946, DateTimeKind.Utc).AddTicks(779), 14, null, 30 },
                    { 30, new DateTime(2025, 9, 7, 13, 40, 55, 270, DateTimeKind.Utc).AddTicks(858), 15, null, 1 }
                });

            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "Id", "Content", "CreatedAt", "IsActive", "IsAnswer", "LikeCount", "ParentPostId", "TopicId", "UpdatedAt", "UserId" },
                values: new object[,]
                {
                    { 16, "<p>Có tác dụng phụ nào cần chú ý không? Đây là bình luận chi tiết về chủ đề đang được thảo luận. Tôi hy vọng thông tin này sẽ hữu ích cho cộng đồng.</p>", new DateTime(2025, 9, 18, 21, 36, 6, 963, DateTimeKind.Utc).AddTicks(5318), true, false, 12, 2, 17, new DateTime(2025, 9, 18, 21, 36, 6, 963, DateTimeKind.Utc).AddTicks(5318), 17 },
                    { 17, "<p>Tôi đã thử và thấy có cải thiện đáng kể. Đây là bình luận chi tiết về chủ đề đang được thảo luận. Tôi hy vọng thông tin này sẽ hữu ích cho cộng đồng.</p>", new DateTime(2025, 9, 12, 3, 23, 1, 961, DateTimeKind.Utc).AddTicks(4919), true, false, 13, 3, 18, new DateTime(2025, 9, 12, 3, 23, 1, 961, DateTimeKind.Utc).AddTicks(4919), 18 },
                    { 18, "<p>Bác sĩ nào có thể tư vấn trực tiếp về vấn đề này? Đây là bình luận chi tiết về chủ đề đang được thảo luận. Tôi hy vọng thông tin này sẽ hữu ích cho cộng đồng.</p>", new DateTime(2025, 9, 1, 18, 57, 22, 903, DateTimeKind.Utc).AddTicks(8012), true, false, 7, 4, 19, new DateTime(2025, 9, 1, 18, 57, 22, 903, DateTimeKind.Utc).AddTicks(8012), 19 },
                    { 19, "<p>Thông tin này rất cần thiết cho mọi người. Đây là bình luận chi tiết về chủ đề đang được thảo luận. Tôi hy vọng thông tin này sẽ hữu ích cho cộng đồng.</p>", new DateTime(2025, 9, 16, 18, 44, 24, 996, DateTimeKind.Utc).AddTicks(7270), true, false, 23, 5, 20, new DateTime(2025, 9, 16, 18, 44, 24, 996, DateTimeKind.Utc).AddTicks(7270), 20 },
                    { 20, "<p>Tôi sẽ chia sẻ với gia đình và bạn bè. Đây là bình luận chi tiết về chủ đề đang được thảo luận. Tôi hy vọng thông tin này sẽ hữu ích cho cộng đồng.</p>", new DateTime(2025, 9, 16, 20, 52, 56, 375, DateTimeKind.Utc).AddTicks(1715), true, true, 24, 6, 21, new DateTime(2025, 9, 16, 20, 52, 56, 375, DateTimeKind.Utc).AddTicks(1715), 21 },
                    { 21, "<p>Cảm ơn bạn đã dành thời gian chia sẻ kiến thức. Đây là bình luận chi tiết về chủ đề đang được thảo luận. Tôi hy vọng thông tin này sẽ hữu ích cho cộng đồng.</p>", new DateTime(2025, 9, 6, 16, 9, 34, 365, DateTimeKind.Utc).AddTicks(3573), true, false, 5, 7, 22, new DateTime(2025, 9, 6, 16, 9, 34, 365, DateTimeKind.Utc).AddTicks(3573), 22 },
                    { 22, "<p>Tôi thấy cách này rất thực tế và dễ áp dụng. Đây là bình luận chi tiết về chủ đề đang được thảo luận. Tôi hy vọng thông tin này sẽ hữu ích cho cộng đồng.</p>", new DateTime(2025, 9, 8, 8, 19, 38, 803, DateTimeKind.Utc).AddTicks(7315), true, false, 20, 8, 23, new DateTime(2025, 9, 8, 8, 19, 38, 803, DateTimeKind.Utc).AddTicks(7315), 23 },
                    { 23, "<p>Bài viết này giải đáp được nhiều thắc mắc của tôi. Đây là bình luận chi tiết về chủ đề đang được thảo luận. Tôi hy vọng thông tin này sẽ hữu ích cho cộng đồng.</p>", new DateTime(2025, 9, 16, 16, 48, 10, 365, DateTimeKind.Utc).AddTicks(2915), true, false, 1, 9, 24, new DateTime(2025, 9, 16, 16, 48, 10, 365, DateTimeKind.Utc).AddTicks(2915), 24 },
                    { 24, "<p>Theo tôi nên kết hợp với phương pháp truyền thống. Đây là bình luận chi tiết về chủ đề đang được thảo luận. Tôi hy vọng thông tin này sẽ hữu ích cho cộng đồng.</p>", new DateTime(2025, 9, 15, 21, 58, 9, 664, DateTimeKind.Utc).AddTicks(2391), true, false, 11, 10, 25, new DateTime(2025, 9, 15, 21, 58, 9, 664, DateTimeKind.Utc).AddTicks(2391), 25 },
                    { 25, "<p>Có thể cho biết nguồn tham khảo để tôi tìm hiểu thêm? Đây là bình luận chi tiết về chủ đề đang được thảo luận. Tôi hy vọng thông tin này sẽ hữu ích cho cộng đồng.</p>", new DateTime(2025, 9, 16, 1, 58, 39, 231, DateTimeKind.Utc).AddTicks(2572), true, false, 19, 11, 26, new DateTime(2025, 9, 16, 1, 58, 39, 231, DateTimeKind.Utc).AddTicks(2572), 26 },
                    { 26, "<p>Kinh nghiệm của bạn rất bổ ích, cảm ơn nhé! Đây là bình luận chi tiết về chủ đề đang được thảo luận. Tôi hy vọng thông tin này sẽ hữu ích cho cộng đồng.</p>", new DateTime(2025, 9, 17, 0, 24, 9, 779, DateTimeKind.Utc).AddTicks(2769), true, false, 6, 12, 27, new DateTime(2025, 9, 17, 0, 24, 9, 779, DateTimeKind.Utc).AddTicks(2769), 27 },
                    { 27, "<p>Tôi đã thử và kết quả tốt hơn mong đợi. Đây là bình luận chi tiết về chủ đề đang được thảo luận. Tôi hy vọng thông tin này sẽ hữu ích cho cộng đồng.</p>", new DateTime(2025, 9, 16, 19, 20, 2, 445, DateTimeKind.Utc).AddTicks(1963), true, false, 3, 13, 28, new DateTime(2025, 9, 16, 19, 20, 2, 445, DateTimeKind.Utc).AddTicks(1963), 28 },
                    { 28, "<p>Phương pháp này có thể áp dụng lâu dài không? Đây là bình luận chi tiết về chủ đề đang được thảo luận. Tôi hy vọng thông tin này sẽ hữu ích cho cộng đồng.</p>", new DateTime(2025, 9, 12, 4, 26, 34, 276, DateTimeKind.Utc).AddTicks(829), true, false, 15, 14, 29, new DateTime(2025, 9, 12, 4, 26, 34, 276, DateTimeKind.Utc).AddTicks(829), 29 },
                    { 29, "<p>Cần bao lâu để thấy hiệu quả của phương pháp này? Đây là bình luận chi tiết về chủ đề đang được thảo luận. Tôi hy vọng thông tin này sẽ hữu ích cho cộng đồng.</p>", new DateTime(2025, 9, 9, 6, 35, 34, 66, DateTimeKind.Utc).AddTicks(4059), true, false, 19, 15, 30, new DateTime(2025, 9, 9, 6, 35, 34, 66, DateTimeKind.Utc).AddTicks(4059), 30 },
                    { 30, "<p>Có cần chế độ ăn đặc biệt kèm theo không? Đây là bình luận chi tiết về chủ đề đang được thảo luận. Tôi hy vọng thông tin này sẽ hữu ích cho cộng đồng.</p>", new DateTime(2025, 9, 9, 4, 2, 11, 774, DateTimeKind.Utc).AddTicks(1335), true, true, 16, 1, 1, new DateTime(2025, 9, 9, 4, 2, 11, 774, DateTimeKind.Utc).AddTicks(1335), 1 }
                });

            migrationBuilder.InsertData(
                table: "Reports",
                columns: new[] { "Id", "Category", "CreatedAt", "PostId", "Reason", "ReviewedAt", "ReviewedByAdminId", "Status", "TopicId", "UserId" },
                values: new object[,]
                {
                    { 16, "Nội dung không phù hợp", new DateTime(2025, 9, 18, 0, 53, 36, 541, DateTimeKind.Utc).AddTicks(6891), 1, "Chia sẻ liên kết độc hại", null, null, "Chờ xử lý", null, 17 },
                    { 17, "Quấy rối", new DateTime(2025, 9, 14, 7, 8, 20, 835, DateTimeKind.Utc).AddTicks(332), 2, "Giả mạo chuyên gia y tế", null, null, "Đã xem xét", null, 18 },
                    { 18, "Vi phạm bản quyền", new DateTime(2025, 9, 3, 1, 16, 40, 466, DateTimeKind.Utc).AddTicks(8128), 3, "Nội dung có thể gây hoang mang", new DateTime(2025, 9, 12, 3, 20, 18, 608, DateTimeKind.Utc).AddTicks(2282), 1, "Đã giải quyết", null, 19 },
                    { 19, "Khác", new DateTime(2025, 9, 10, 16, 31, 55, 618, DateTimeKind.Utc).AddTicks(9639), 4, "Vi phạm điều khoản sử dụng", null, null, "Đã từ chối", null, 20 },
                    { 20, "Thư rác", new DateTime(2025, 9, 18, 3, 12, 22, 768, DateTimeKind.Utc).AddTicks(3046), 5, "Báo cáo sai mục đích", null, null, "Chờ xử lý", null, 21 },
                    { 21, "Nội dung không phù hợp", new DateTime(2025, 9, 7, 17, 8, 25, 170, DateTimeKind.Utc).AddTicks(6587), 6, "Nội dung không đúng sự thật", new DateTime(2025, 9, 12, 5, 52, 29, 234, DateTimeKind.Utc).AddTicks(675), 1, "Đã xem xét", null, 22 },
                    { 22, "Quấy rối", new DateTime(2025, 9, 4, 0, 42, 12, 718, DateTimeKind.Utc).AddTicks(276), 7, "Thông tin y tế nguy hiểm", null, null, "Đã giải quyết", null, 23 },
                    { 23, "Vi phạm bản quyền", new DateTime(2025, 9, 16, 20, 5, 13, 101, DateTimeKind.Utc).AddTicks(2926), 8, "Quảng cáo thuốc trái phép", null, null, "Đã từ chối", null, 24 },
                    { 24, "Khác", new DateTime(2025, 9, 1, 11, 25, 56, 392, DateTimeKind.Utc).AddTicks(2037), 9, "Xúc phạm người khác", new DateTime(2025, 9, 11, 15, 16, 21, 897, DateTimeKind.Utc).AddTicks(4502), 1, "Chờ xử lý", null, 25 },
                    { 25, "Thư rác", new DateTime(2025, 9, 18, 3, 1, 1, 613, DateTimeKind.Utc).AddTicks(4749), 10, "Nội dung phân biệt đối xử", null, null, "Đã xem xét", null, 26 },
                    { 26, "Nội dung không phù hợp", new DateTime(2025, 9, 7, 18, 3, 29, 823, DateTimeKind.Utc).AddTicks(8756), 11, "Chia sẻ thông tin riêng tư", null, null, "Đã giải quyết", null, 27 },
                    { 27, "Quấy rối", new DateTime(2025, 9, 2, 13, 58, 5, 585, DateTimeKind.Utc).AddTicks(265), 12, "Bài viết lặp lại nhiều lần", new DateTime(2025, 9, 8, 17, 6, 19, 333, DateTimeKind.Utc).AddTicks(861), 1, "Đã từ chối", null, 28 },
                    { 28, "Vi phạm bản quyền", new DateTime(2025, 9, 2, 19, 0, 31, 927, DateTimeKind.Utc).AddTicks(825), 13, "Hình ảnh không phù hợp", null, null, "Chờ xử lý", null, 29 },
                    { 29, "Khác", new DateTime(2025, 9, 17, 6, 40, 16, 666, DateTimeKind.Utc).AddTicks(3920), 14, "Liên kết đáng ngờ", null, null, "Đã xem xét", null, 30 },
                    { 30, "Thư rác", new DateTime(2025, 9, 17, 8, 59, 3, 466, DateTimeKind.Utc).AddTicks(5707), 15, "Tư vấn y tế không có căn cứ", new DateTime(2025, 9, 18, 4, 48, 18, 101, DateTimeKind.Utc).AddTicks(4038), 1, "Đã giải quyết", null, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdminAccounts_Email",
                table: "AdminAccounts",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AdminAccounts_Username",
                table: "AdminAccounts",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_DisplayOrder",
                table: "Categories",
                column: "DisplayOrder");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Slug",
                table: "Categories",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Likes_PostId",
                table: "Likes",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_TopicId",
                table: "Likes",
                column: "TopicId");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_UserId",
                table: "Likes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_UserId_TopicId_PostId",
                table: "Likes",
                columns: new[] { "UserId", "TopicId", "PostId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_CreatedAt",
                table: "Posts",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_IsActive",
                table: "Posts",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_ParentPostId",
                table: "Posts",
                column: "ParentPostId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_TopicId",
                table: "Posts",
                column: "TopicId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_UserId",
                table: "Posts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_Category",
                table: "Reports",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_CreatedAt",
                table: "Reports",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_PostId",
                table: "Reports",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_Status",
                table: "Reports",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_TopicId",
                table: "Reports",
                column: "TopicId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_UserId",
                table: "Reports",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_StaticPages_Slug",
                table: "StaticPages",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Name",
                table: "Tags",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Slug",
                table: "Tags",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Topics_CategoryId",
                table: "Topics",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Topics_CreatedAt",
                table: "Topics",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Topics_IsActive",
                table: "Topics",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Topics_IsPinned",
                table: "Topics",
                column: "IsPinned");

            migrationBuilder.CreateIndex(
                name: "IX_Topics_Slug",
                table: "Topics",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Topics_UserId",
                table: "Topics",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TopicTags_TagId",
                table: "TopicTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_TopicTags_TopicId_TagId",
                table: "TopicTags",
                columns: new[] { "TopicId", "TagId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TopicViews_TopicId",
                table: "TopicViews",
                column: "TopicId");

            migrationBuilder.CreateIndex(
                name: "IX_TopicViews_UserId",
                table: "TopicViews",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TopicViews_ViewedAt",
                table: "TopicViews",
                column: "ViewedAt");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccounts_Email",
                table: "UserAccounts",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserAccounts_SpecialtyId",
                table: "UserAccounts",
                column: "SpecialtyId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccounts_Username",
                table: "UserAccounts",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminAccounts");

            migrationBuilder.DropTable(
                name: "Likes");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropTable(
                name: "StaticPages");

            migrationBuilder.DropTable(
                name: "TopicTags");

            migrationBuilder.DropTable(
                name: "TopicViews");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Topics");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "UserAccounts");

            migrationBuilder.DropTable(
                name: "Specialties");
        }
    }
}
