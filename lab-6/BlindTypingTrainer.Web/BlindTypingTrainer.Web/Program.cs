using BlindTypingTrainer.Web.Data;
using BlindTypingTrainer.Web.Models;
using BlindTypingTrainer.Web.Repositories;
using BlindTypingTrainer.Web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// 1) ������������ DbContext � Pomelo MySQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(9, 1, 0))
    )
);

// 2) Identity: ����������� �� ���
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(opts =>
{
    opts.Password.RequireDigit = false;
    opts.Password.RequireLowercase = false;
    opts.Password.RequireUppercase = false;
    opts.Password.RequireNonAlphanumeric = false;
    opts.Password.RequiredLength = 6;
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// ������������ ��� ��� �����������
builder.Services.ConfigureApplicationCookie(opts =>
{
    opts.LoginPath = "/Account/Login";
    opts.AccessDeniedPath = "/Account/AccessDenied";
});

// 3) ���� ��������� �� ������
builder.Services.AddScoped<IRepository<Lesson>, LessonRepository>();
builder.Services.AddScoped<IRepository<TypingSession>, TypingSessionRepository>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<TypingService>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// 4) ̳������, �� ��� �� ��������� ���/�����
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    // ̳����� ��
    var db = services.GetRequiredService<AppDbContext>();
    db.Database.Migrate();

    // ѳ���� �����
    SeedData.Initialize(services);

    // ��������� ���� Admin �� ����������� admin, ���� �� ����
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        await roleManager.CreateAsync(new IdentityRole("Admin"));
    }

    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var adminUser = await userManager.FindByNameAsync("admin");
    if (adminUser == null)
    {
        adminUser = new ApplicationUser
        {
            UserName = "admin",
            Email = "admin@example.com",
            EmailConfirmed = true
        };
        await userManager.CreateAsync(adminUser, "Admin123!");
        await userManager.AddToRoleAsync(adminUser, "Admin");
    }
}

// 5) ������������ middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// ����������: �������� ��������������
app.UseAuthentication();
app.UseAuthorization();

// 6) ��������
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();