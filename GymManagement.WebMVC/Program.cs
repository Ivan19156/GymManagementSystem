using GymManagement.Infrastructure;
using GymManagement.WebMVC.Data;
using GymManagement.WebMVC.Models;
using GymManagement.WebMVC.Services;
using GymManagement.WebMVC.Services.Excel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// ── EF Core contexts ──────────────────────────────────────────────────────────
builder.Services.AddDbContext<GymContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// ── Identity ──────────────────────────────────────────────────────────────────
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
    {
        options.Password.RequireDigit           = true;
        options.Password.RequiredLength         = 6;
        options.Password.RequireUppercase       = true;
        options.Password.RequireLowercase       = true;
        options.Password.RequireNonAlphanumeric = false;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath       = "/Account/Login";
    options.AccessDeniedPath = "/Account/Login";
});

// ── Data-port services ────────────────────────────────────────────────────────
builder.Services.AddScoped<IDataPortServiceFactory<ClientRowDto>,  ClientDataPortServiceFactory>();
builder.Services.AddScoped<IDataPortServiceFactory<TrainerRowDto>, TrainerDataPortServiceFactory>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// ── Seed ──────────────────────────────────────────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    // GymContext seed
    var db = scope.ServiceProvider.GetRequiredService<GymContext>();
    await DbSeeder.SeedAsync(db);

    // Identity migration + admin seed
    var identityDb   = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await identityDb.Database.MigrateAsync();

    var userManager  = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    var roleManager  = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    await IdentitySeeder.SeedAsync(userManager, roleManager);
}

app.Run();
