using ApiApplication;
using ApiApplication.Data;
using ApiApplication.Services.Admin;
using ApiApplication.Services.User;
using ApiApplication.Services.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("AdminScheme", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:AdminKey"]!)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    })
    .AddJwtBearer("UserScheme", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:UserKey"]!)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

// Add Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.AddAuthenticationSchemes("AdminScheme").RequireAuthenticatedUser());
    options.AddPolicy("UserPolicy", policy => policy.AddAuthenticationSchemes("UserScheme").RequireAuthenticatedUser());
});

// Register Services
// Common Services
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IHtmlSanitizerService, HtmlSanitizerService>();

// Admin Services
builder.Services.AddScoped<IAdminAuthService, AdminAuthService>();
builder.Services.AddScoped<IAdminDashboardService, AdminDashboardService>();
builder.Services.AddScoped<IAdminUserService, AdminUserService>();
builder.Services.AddScoped<IAdminCategoryService, AdminCategoryService>();
builder.Services.AddScoped<IAdminTopicService, AdminTopicService>();
builder.Services.AddScoped<IAdminExpertService, AdminExpertService>();
builder.Services.AddScoped<IAdminSearchService, AdminSearchService>();

// User Services
builder.Services.AddScoped<IUserAuthService, UserAuthService>();
builder.Services.AddScoped<IUserHomeService, UserHomeService>();
builder.Services.AddScoped<IUserCategoryService, UserCategoryService>();
builder.Services.AddScoped<IUserTopicService, UserTopicService>();
builder.Services.AddScoped<IUserProfileService, UserProfileService>();
builder.Services.AddScoped<IUserExpertService, UserExpertService>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure URL
app.Urls.Add("http://localhost:5002");
app.Urls.Add("https://localhost:7002");

// app.UseHttpsRedirection(); // Disabled for testing
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Ensure database is created
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();
}

app.Run();
