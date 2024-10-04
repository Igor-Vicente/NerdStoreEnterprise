using Microsoft.AspNetCore.Mvc;
using NSE.Core.Utils;
using NSE.MessageBus;

namespace NSE.Identidade.Api.Configuration
{
    public static class ApiConfig
    {
        public static IServiceCollection ConfigureApi(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
            services.AddMessageBus(configuration.GetMessageQueueConnection("MessageBus"));
            services.AddControllers();
            return services;
        }
    }
}
