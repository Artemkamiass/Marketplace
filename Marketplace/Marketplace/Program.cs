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
