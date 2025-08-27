using DataAccess;
using Domain.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using Service;
using System.Net;
using View.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddHttpClient("LocalAPI", client =>
{
    client.BaseAddress = new Uri("https://localhost:7190/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
}).ConfigurePrimaryHttpMessageHandler(() =>
    new HttpClientHandler
    {
        UseCookies = true,
        CookieContainer = new CookieContainer()
    } as HttpMessageHandler
);

builder.Services.AddScoped<Domain.Interfaces.IDbConnectionFactory, DataAccess.SqlConnectionFactory>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<IPreferenceRepository, PreferenceRepository>();
builder.Services.AddScoped<ILocationOfficeRepository, LocationOfficeRepository>();
builder.Services.AddScoped<IMajorRepository, MajorRepository>();
builder.Services.AddScoped<IDegreeRepository, DegreeRepository>();
builder.Services.AddScoped<IUniversityRepository, UniversityRepository>();
builder.Services.AddScoped<IEducationRepository, EducationRepository>();
builder.Services.AddScoped<CityService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<PreferenceService>();
builder.Services.AddScoped<LocationOfficeService>();
builder.Services.AddScoped<CookieService>();
builder.Services.AddScoped<SessionService>();
builder.Services.AddScoped<MajorService>();
builder.Services.AddScoped<DegreeService>();
builder.Services.AddScoped<UniversityService>();
builder.Services.AddScoped<EducationService>();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddAuthorization();
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.Use(async (context, next) =>
{
    context.Response.Headers["Content-Security-Policy"] =
        "default-src 'self'; " +
        "script-src 'self' 'nonce-RbwVv3QnnEuEoBuXLjA7Xg=='; " +
        "img-src 'self' data:; " +
        "object-src 'none'; " +
        "upgrade-insecure-requests; " +
        "block-all-mixed-content;";

    await next();
});
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.UseAntiforgery();
app.UseStaticFiles(); 
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
app.Run();
