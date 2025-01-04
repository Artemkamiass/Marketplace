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
            // ��������� Identity � ������������ � �������
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                // ��������� ���������� � �������
                options.Password.RequireDigit = true; // ��������� �����
                options.Password.RequireLowercase = true; // ��������� �������� �����
                options.Password.RequireUppercase = true; // ��������� ��������� �����
                options.Password.RequireNonAlphanumeric = true; // ��������� ����������� �������
                options.Password.RequiredLength = 6; // ����������� ����� ������
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
