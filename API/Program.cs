using DataAccess;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Service;
using System.Data.Entity.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<Domain.Interfaces.IDbConnectionFactory, DataAccess.SqlConnectionFactory>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<IPreferenceRepository, PreferenceRepository>();
builder.Services.AddScoped<ILocationOfficeRepository, LocationOfficeRepository>();
builder.Services.AddScoped< CityService>();
builder.Services.AddScoped< UserService>();
builder.Services.AddScoped< PreferenceService>();
builder.Services.AddScoped<LocationOfficeService>();
builder.Services.AddScoped<CookieService>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<AdminService>();
builder.Services.AddAuthentication()
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.Cookie.Name = "user_auth";
        options.LoginPath = "/api/User/Login";
        options.LogoutPath = "/api/User/Logout";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.Cookie.SameSite = SameSiteMode.None;
        options.Cookie.SecurePolicy = CookieSecurePolicy.None;
        options.SlidingExpiration = true;
    })
    .AddCookie("AdminAuth", options =>
    {
        options.Cookie.Name = "admin_auth";
        options.LoginPath = "/api/Admin/Login";
        options.LogoutPath = "/api/Admin/Logout";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
        options.Cookie.SameSite = SameSiteMode.None;
        options.Cookie.SecurePolicy = CookieSecurePolicy.None;
        options.SlidingExpiration = true;
    });
builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "RequestVerificationToken";
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient", policy =>
    {
        policy.WithOrigins("https://localhost:7159") 
              .AllowCredentials()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
builder.Services.AddAuthorization();
builder.Services.AddControllersWithViews();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowBlazorClient");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();
app.MapControllers();

app.Run();
