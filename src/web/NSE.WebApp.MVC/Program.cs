using NSE.WebApp.MVC.Configuration;
using NSE.WebApp.MVC.Extensions;

namespace NSE.WebApp.MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.Configure<AppSettings>(builder.Configuration);
            builder.Services.ConfigureAuthentication();
            builder.Services.AddControllersWithViews();
            builder.Services.RegisterServices(builder.Configuration);

            var app = builder.Build();

            app.UseAppConfiguration();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Catalogo}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
