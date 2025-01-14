using Marketplace.Mapping;
using Marketplace.Models;
using Marketplace.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Marketplace
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            builder.Services.AddAutoMapper(typeof(MappingProfile));

            builder.Services.AddSession();

            builder.Services.AddDbContext<MarketplaceContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("MarketplaceDataBase")));
            // Настройка Identity с требованиями к паролям
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                // Настройка требований к паролям
                options.Password.RequireDigit = true; // Требовать цифры
                options.Password.RequireLowercase = true; // Требовать строчные буквы
                options.Password.RequireUppercase = true; // Требовать заглавные буквы
                options.Password.RequireNonAlphanumeric = true; // Требовать специальные символы
                options.Password.RequiredLength = 6; // Минимальная длина пароля
            })

            .AddEntityFrameworkStores<MarketplaceContext>()
            .AddDefaultTokenProviders();
            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    await RoleInitializer.Initialize(userManager, roleManager);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }

            app.MapControllerRoute(
                name: "admin",
                pattern: "Admin/{action=Index}/{id?}",
                defaults: new { controller = "Admin", action = "Index" });

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");


            app.UseStaticFiles();
            app.UseCookiePolicy(new CookiePolicyOptions
            {
                Secure = CookieSecurePolicy.Always
            });

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();
            

            app.Run();
        }
    }
}
