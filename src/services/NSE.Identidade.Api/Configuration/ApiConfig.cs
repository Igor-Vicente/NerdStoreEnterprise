using Microsoft.AspNetCore.Mvc;
using NSE.Core.Utils;
using NSE.Identidade.Api.Extensions;
using NSE.Identidade.Api.Services;
using NSE.MessageBus;
using NSE.WebApi.Core.Usuario;

namespace NSE.Identidade.Api.Configuration
{
    public static class ApiConfig
    {
        public static IServiceCollection ConfigureApi(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
            services.Configure<AppTokenSettings>(configuration.GetSection("AppTokenSettings"));
            services.AddMessageBus(configuration.GetMessageQueueConnection("MessageBus"));

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAspNetUser, AspNetUser>();

            services.AddScoped<AuthenticationService>();

            services.AddControllers();
            return services;
        }
    }
}
