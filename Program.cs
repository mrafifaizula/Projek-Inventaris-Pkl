using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using MySql.EntityFrameworkCore.Extensions;
using ProjekPklInventaris.Models;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<DataContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    new MySqlServerVersion(new Version(8, 0, 21))));

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<DataContext>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.MapRazorPages();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "dashboard",
    pattern: "admin/dashboard",
    defaults: new { controller = "Backend", action = "Index" }
);

app.MapControllerRoute(
    name: "kategori",
    pattern: "admin/kategori/{action=Index}/{id?}",
    defaults: new { controller = "Kategori", action = "Index" }
);

app.MapControllerRoute(
    name: "pemasok",
    pattern: "admin/pemasok/{action=Index}/{id?}",
    defaults: new { controller = "Pemasok", action = "Index" }
);

app.MapControllerRoute(
    name: "barang",
    pattern: "admin/barang/{action=Index}/{id?}",
    defaults: new { controller = "Barang", action = "Index" }
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
