using Microsoft.AspNetCore.Mvc.DataAnnotations;
using NSE.WebApi.Core.Usuario;
using NSE.WebApp.MVC.Extensions;
using NSE.WebApp.MVC.Services;
using NSE.WebApp.MVC.Services.Handlers;
using Polly;

namespace NSE.WebApp.MVC.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IValidationAttributeAdapterProvider, CpfValidationAttributeAdapterProvider>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAspNetUser, AspNetUser>();

            #region HttpServices
            services.AddTransient<HttpClientAuthorizationDelegatingHandler>();

            services.AddHttpClient<IAutenticacaoService, AutenticacaoService>(o => o.BaseAddress = new Uri(configuration.GetValue<string>("AutenticacaoUrl") ?? ""))
                .AddPolicyHandler(PollyExtensions.GetRetryPolicy())
                .AddTransientHttpErrorPolicy(p => p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));

            services.AddHttpClient<ICatalogoService, CatalogoService>(o => o.BaseAddress = new Uri(configuration.GetValue<string>("CatalogoUrl") ?? ""))
                .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
                .AddPolicyHandler(PollyExtensions.GetRetryPolicy())
                .AddTransientHttpErrorPolicy(p => p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30))); //após 5 falhas consecutivas de requisição, impede a comunição na api por 30 seg

            services.AddHttpClient<ICarrinhoService, CarrinhoService>(o => o.BaseAddress = new Uri(configuration.GetValue<string>("CarrinhoUrl") ?? ""))
                .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
                .AddPolicyHandler(PollyExtensions.GetRetryPolicy())
                .AddTransientHttpErrorPolicy(p => p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));
            #endregion

            return services;
        }
    }
}

#region How Add Refit
/* YOU CAN USE REFIT INSTEAD OF YOUR OWN PERSONAL CLASS DO DEAL WITH REST RESQUEST */
/*
 services.AddHttpClient<ICatalogoService, CatalogoService>("Refit", o =>
                o.BaseAddress = new Uri(configuration.GetValue<string>("CatalogoUrl") ?? ""))
                .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
                .AddTypedClient(Refit.RestService.For<ICatalogoServiceRefit>);
*/
#endregion
#region What is Refit ?
/*
 Refit is a REST library for .NET developers available on NuGet. It allows you to define your REST API as an interface in C# 
and automatically generates the implementation for you. This can simplify calling RESTful services by using strongly-typed interfaces, 
making the code more maintainable and easier to work with. Essentially, Refit helps you create API clients with less boilerplate code.

It is very easy to use and provides a faster way to implement Rest Request especially for GET methods.
 */
#endregion