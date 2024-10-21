using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSE.Catalogo.Api.Data;
using NSE.Catalogo.Api.Data.Repository;
using NSE.Catalogo.Api.Services;
using NSE.Core.Utils;
using NSE.MessageBus;

namespace NSE.Catalogo.Api.Configuration
{
    public static class ApiConfig
    {
        public static void AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);

            services.AddDbContext<CatalogoContext>(options => options.UseSqlServer(configuration.GetConnectionString("Default")));

            services.AddMessageBus(configuration.GetMessageQueueConnection("MessageBus"));
            services.AddHostedService<CatalogoIntegrationHandler>();

            services.AddScoped<IProdutoRepository, ProdutoRepository>();

            services.AddCors(options =>
            {
                options.AddPolicy("Development", policy =>
                {
                    policy.AllowAnyOrigin();
                    policy.AllowAnyMethod();
                    policy.AllowAnyHeader();
                });
            });
        }
    }
}
