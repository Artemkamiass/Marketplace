using Marketplace.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Marketplace
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

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

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.UseStaticFiles();



            app.Run();
        }
    }
}
