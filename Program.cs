using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using MySql.EntityFrameworkCore.Extensions;
using ProjekPklInventaris.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<DataContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    new MySqlServerVersion(new Version(8, 0, 21))));

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "dashboard",
    pattern: "dashboard",
    defaults: new { controller = "Backend", action = "Index" }
);

app.MapControllerRoute(
    name: "kategori",
    pattern: "kategori/{action=Index}/{id?}",
    defaults: new { controller = "Kategori", action = "Index" }
);

app.MapControllerRoute(
    name: "pemasok",
    pattern: "pemasok/{action=Index}/{id?}",
    defaults: new { controller = "Pemasok", action = "Index" }
);

app.MapControllerRoute(
    name: "barang",
    pattern: "barang/{action=Index}/{id?}",
    defaults: new { controller = "Barang", action = "Index" }
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
