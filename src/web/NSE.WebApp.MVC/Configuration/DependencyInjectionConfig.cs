using NSE.WebApp.MVC.Extensions;
using NSE.WebApp.MVC.Services;

namespace NSE.WebApp.MVC.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient<IAutenticacaoService, AutenticacaoService>(o => AuthHttpClientOptions(o, configuration));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUser, AspNetUser>();
            return services;
        }

        private static void AuthHttpClientOptions(HttpClient clientConfig, IConfiguration configuration)
        {
            clientConfig.BaseAddress = new Uri(configuration.GetValue<string>("AutenticacaoUrl") ?? "");
        }
    }
}
