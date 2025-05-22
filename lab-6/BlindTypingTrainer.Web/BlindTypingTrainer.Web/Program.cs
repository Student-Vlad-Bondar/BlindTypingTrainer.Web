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

// 1) DbContext
builder.Services.AddDbContext<AppDbContext>(opts =>
    opts.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(9, 1, 0))
    )
);

// 2) Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(opts =>
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
builder.Services.ConfigureApplicationCookie(opts =>
{
    opts.LoginPath = "/Account/Login";
    opts.AccessDeniedPath = "/Account/AccessDenied";
});

// 4) Repositories
builder.Services.AddScoped<IReadRepository<Lesson>, LessonRepository>();
builder.Services.AddScoped<IWriteRepository<Lesson>, LessonRepository>();
builder.Services.AddScoped<IReadRepository<TypingSession>, TypingSessionRepository>();
builder.Services.AddScoped<IWriteRepository<TypingSession>, TypingSessionRepository>();

// 5) Repositories for achievements
builder.Services.AddScoped<IAchievementRepository, AchievementRepository>();
builder.Services.AddScoped<IUserAchievementRepository, UserAchievementRepository>();

// 6) Filtering strategies
builder.Services.AddScoped<ILessonFilterStrategy, EasyStrategy>();
builder.Services.AddScoped<ILessonFilterStrategy, MediumStrategy>();
builder.Services.AddScoped<ILessonFilterStrategy, HardStrategy>();
builder.Services.AddScoped<ILessonFilterStrategy, VeryHardStrategy>();

// 6) Handlers for COR
builder.Services.AddScoped<IAchievementHandler, FirstLessonHandler>();
builder.Services.AddScoped<IAchievementHandler, HundredWordsHandler>();
builder.Services.AddScoped<IAchievementHandler, Accuracy95Handler>();
builder.Services.AddScoped<IAchievementHandler, Speed50WpmHandler>();
builder.Services.AddScoped<IAchievementHandler, MarathonHandler>();

// 8) Application services
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<TypingService>();
builder.Services.AddScoped<AchievementService>();

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