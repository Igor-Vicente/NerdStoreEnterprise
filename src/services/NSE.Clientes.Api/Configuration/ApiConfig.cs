using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSE.Clientes.Api.Application.Commands;
using NSE.Clientes.Api.Application.Events;
using NSE.Clientes.Api.Data;
using NSE.Clientes.Api.Data.Repository;
using NSE.Clientes.Api.Services;
using NSE.Core.Mediator;
using NSE.Core.Utils;
using NSE.MessageBus;
using System.Reflection;

namespace NSE.Clientes.Api.Configuration
{
    public static class ApiConfig
    {
        public static void AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);

            services.AddDbContext<ClientesContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("Default")));

            services.AddMediatR(c => c.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
            services.AddScoped<IMediatorHandler, MediatorHandler>();
            services.AddScoped<IRequestHandler<RegistrarClienteCommand, ValidationResult>, ClienteCommandHandler>();
            services.AddScoped<IClienteRepository, ClienteRepository>();
            services.AddScoped<INotificationHandler<ClienteRegistradoEvent>, ClinteEventHandler>();

            services.AddMessageBus(configuration.GetMessageQueueConnection("MessageBus"));
            services.AddHostedService<RegistroClienteIntegrationHandler>(); //life cicle of a background service is Singleton

            services.AddControllers();

            services.AddCors(options =>
            {
                options.AddPolicy("Development",
                    builder =>
                        builder
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader());
            });
        }
    }
}
