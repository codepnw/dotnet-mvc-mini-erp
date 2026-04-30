using Microsoft.EntityFrameworkCore;
using MiniERP.Mvc.Data;

var builder = WebApplication.CreateBuilder(args);

// Connect SQL Server
builder.Services.AddSqlServer<AppDbContext>(
    builder.Configuration.GetConnectionString("DefaultConnection")
);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Database Migrations
await app.Services.CreateScope().ServiceProvider
    .GetRequiredService<AppDbContext>()
    .Database.MigrateAsync();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
