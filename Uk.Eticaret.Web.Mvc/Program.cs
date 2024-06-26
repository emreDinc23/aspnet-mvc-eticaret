using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using NETCore.MailKit.Extensions;
using NETCore.MailKit.Infrastructure.Internal;
using Uk.Eticaret.Business.Services;
using Uk.Eticaret.Business.Services.Abstract;
using Uk.Eticaret.Persistence;
using Uk.Eticaret.Persistence.Seeders;
using Uk.Eticaret.Web.Mvc.Services;
using Uk.Eticaret.Web.Mvc.Services.Email;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("Default");
    options.UseSqlServer(connectionString);
});

builder.Services.AddSession();

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<SettingsService>();
builder.Services.AddMailKit(config => config.UseMailKit(builder.Configuration.GetSection("Email").Get<MailKitOptions>()));
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("Email"));
builder.Services.AddTransient<IEmailServices, EmailServices>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(option =>
    {
        option.LoginPath = "/Auth/Login";
        option.LogoutPath = "/Auth/Logout";
        option.AccessDeniedPath = "/Auth/AccessDenied";
    }).AddTwitter(twitterOptions =>
    {
        twitterOptions.ConsumerKey = "VnZIQRxIaJ8Nmi607FUOCp3aR";
        twitterOptions.ConsumerSecret = "pRrlhDOkNFRZcixUoAowN60h9yRG93VsDAF3JdQrhX51P92rzP";
        twitterOptions.CallbackPath = new PathString("/signin-twitter");
    }); ;

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    //dbContext.Database.EnsureDeleted();
    dbContext.Database.EnsureCreated();
    DbSeeder.SeedAll(dbContext);
}

app.UseSession();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "category",
    pattern: "category/{slug}",
    defaults: new { controller = "Category", action = "Index" });

app.MapControllerRoute(
    name: "product",
    pattern: "product/{slug}",
    defaults: new { controller = "Product", action = "Detail" });

app.MapControllerRoute(
    name: "page",
    pattern: "page/{slug}",
    defaults: new { controller = "Page", action = "Detail" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();