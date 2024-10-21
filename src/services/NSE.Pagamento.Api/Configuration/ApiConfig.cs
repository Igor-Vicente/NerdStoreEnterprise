using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSE.Core.Utils;
using NSE.MessageBus;
using NSE.Pagamentos.Api.Data;
using NSE.Pagamentos.Api.Data.Repository;
using NSE.Pagamentos.Api.Facade;
using NSE.Pagamentos.Api.Services;

namespace NSE.Pagamentos.Api.Configuration
{
    public static class ApiConfig
    {
        public static void AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);

            services.Configure<PagamentoConfig>(configuration.GetSection("PagamentoConfig"));

            services.AddDbContext<PagamentosContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("Default")));

            services.AddMessageBus(configuration.GetMessageQueueConnection("MessageBus"));
            services.AddHostedService<PagamentoIntegrationHandler>();

            services.AddScoped<IPagamentoFacade, PagamentoCartaoCreditoFacade>();
            services.AddScoped<IPagamentoService, PagamentoService>();

            services.AddScoped<IPagamentoRepository, PagamentoRepository>();
        }
    }
}
