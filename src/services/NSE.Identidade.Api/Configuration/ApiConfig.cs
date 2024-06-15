using Microsoft.AspNetCore.Mvc;

namespace NSE.Identidade.Api.Configuration
{
    public static class ApiConfig
    {
        public static IServiceCollection ConfigureApi(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);

            services.AddControllers();
            return services;
        }
    }
}
