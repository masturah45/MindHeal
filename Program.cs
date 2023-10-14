using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MindHeal.Data;
using MindHeal.FileManagers;
using MindHeal.Helper;
using MindHeal.Implementations.Repositories;
using MindHeal.Implementations.Services;
using MindHeal.Interfaces.IRepositories;
using MindHeal.Interfaces.IServices;
using MindHeal.Models.DTOs;
using MindHeal.Models.Entities;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IRoleService, RoleService>();

builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IClientService, ClientService>();

builder.Services.AddScoped<ITherapistRepository, TherapistRepository>();
builder.Services.AddScoped<ITherapistService, TherapistService>();

builder.Services.AddScoped<IIssuesRepository, IssuesRepository>();
builder.Services.AddScoped<IIssuesService, IssuesService>();

builder.Services.AddScoped<ITherapistIssuesRepository, TherapistIssuesRepository>();
builder.Services.AddScoped<ITherapistIssuesService, TherapistIssuesService>();

builder.Services.AddScoped<IChatRepository, ChatRepository>();
builder.Services.AddScoped<IChatService, ChatService>();

builder.Services.AddScoped<IFileManager, FileManager>();
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequiredLength = 9; // Password length requirements
                                         // Configure other password requirements here
}).AddClaimsPrincipalFactory<UserClaimsPrincipalFactory>()
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();
builder.Services.AddScoped<INotificationMessage, NotificationMessage>();
builder.Services.AddOptions<WhatsappMessageSettings>().BindConfiguration(nameof(WhatsappMessageSettings));
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(config =>
{
    config.LoginPath = "/User/Login";
    config.LogoutPath = "/Home/Index";
    config.ExpireTimeSpan = TimeSpan.FromHours(1);
    config.Cookie.Name = "MyFinalProject";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

await app.Seed();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
