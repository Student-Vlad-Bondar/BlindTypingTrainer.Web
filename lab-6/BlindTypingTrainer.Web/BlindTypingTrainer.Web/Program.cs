using BlindTypingTrainer.Web.Data;
using BlindTypingTrainer.Web.Models;
using BlindTypingTrainer.Web.Repositories;
using BlindTypingTrainer.Web.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// 1) Налаштування DbContext з Pomelo MySQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(9, 1, 0))
    )
);

// 2) Реєструємо DI для репозиторіїв і сервісу
builder.Services.AddScoped<IRepository<Lesson>, LessonRepository>();
builder.Services.AddScoped<IRepository<TypingSession>, TypingSessionRepository>();
builder.Services.AddScoped<TypingService>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// 3) Сідуємо початкові дані
SeedData.Initialize(app.Services);

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();
