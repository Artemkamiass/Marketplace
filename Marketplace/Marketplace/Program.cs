using Marketplace.Models;
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

            var app = builder.Build();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.UseStaticFiles();



            app.Run();
        }
    }
}
