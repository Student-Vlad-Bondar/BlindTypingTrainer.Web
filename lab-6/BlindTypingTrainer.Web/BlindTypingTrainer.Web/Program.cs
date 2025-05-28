using BlindTypingTrainer.Web.Data;
using BlindTypingTrainer.Web.Models;
using BlindTypingTrainer.Web.Repositories;
using BlindTypingTrainer.Web.Services;
using BlindTypingTrainer.Web.Services.AchievementHandlers.Handlers.ConcreteHandlers;
using BlindTypingTrainer.Web.Services.AchievementHandlers.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Реєстрація сервісів через ServiceRegistration
builder.Services.AddApplicationServices(builder.Configuration);

builder.Services.AddControllersWithViews();

var app = builder.Build();

// 9) Migrations + data + admin seeding
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var db = services.GetRequiredService<AppDbContext>();
    db.Database.Migrate();

    SeedData.Initialize(services);

    AchievementSeedData.Initialize(services);

    var roles = services.GetRequiredService<RoleManager<IdentityRole>>();
    if (!await roles.RoleExistsAsync("Admin"))
        await roles.CreateAsync(new IdentityRole("Admin"));

    var users = services.GetRequiredService<UserManager<ApplicationUser>>();
    if (await users.FindByNameAsync("admin") == null)
    {
        var admin = new ApplicationUser
        {
            UserName = "admin",
            Email = "admin@example.com",
            EmailConfirmed = true
        };
        await users.CreateAsync(admin, "Admin123!");
        await users.AddToRoleAsync(admin, "Admin");
    }
}

// 10) Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// 11) Routing
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();

public static class ServiceRegistration
{
    public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // 1) DbContext
        services.AddDbContext<AppDbContext>(opts =>
            opts.UseMySql(
                configuration.GetConnectionString("DefaultConnection"),
                new MySqlServerVersion(new Version(9, 1, 0))
            )
        );

        // 2) Identity
        services.AddIdentity<ApplicationUser, IdentityRole>(opts =>
        {
            opts.Password.RequiredLength = 6;
            opts.Password.RequireDigit = false;
            opts.Password.RequireLowercase = false;
            opts.Password.RequireUppercase = false;
            opts.Password.RequireNonAlphanumeric = false;
        })
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();

        // 3) Cookie paths
        services.ConfigureApplicationCookie(opts =>
        {
            opts.LoginPath = "/Account/Login";
            opts.AccessDeniedPath = "/Account/AccessDenied";
        });

        // 4) Repositories
        services.AddScoped<IReadRepository<Lesson>, LessonRepository>();
        services.AddScoped<IWriteRepository<Lesson>, LessonRepository>();
        services.AddScoped<IReadRepository<TypingSession>, TypingSessionRepository>();
        services.AddScoped<IWriteRepository<TypingSession>, TypingSessionRepository>();

        // 5) Repositories for achievements
        services.AddScoped<IAchievementRepository, AchievementRepository>();
        services.AddScoped<IUserAchievementRepository, UserAchievementRepository>();

        // 6) Filtering strategies
        services.AddScoped<ILessonFilterStrategy, EasyStrategy>();
        services.AddScoped<ILessonFilterStrategy, MediumStrategy>();
        services.AddScoped<ILessonFilterStrategy, HardStrategy>();
        services.AddScoped<ILessonFilterStrategy, VeryHardStrategy>();

        // 7) Handlers for COR
        services.AddScoped<IAchievementHandler, FirstLessonHandler>();
        services.AddScoped<IAchievementHandler, HundredWordsHandler>();
        services.AddScoped<IAchievementHandler, Accuracy95Handler>();
        services.AddScoped<IAchievementHandler, Speed50WpmHandler>();
        services.AddScoped<IAchievementHandler, MarathonHandler>();

        // 8) Application services
        services.AddHttpContextAccessor();
        services.AddScoped<TypingService>();
        services.AddScoped<AchievementService>();
        services.AddScoped<IUserProfileService, UserProfileService>();
    }
}