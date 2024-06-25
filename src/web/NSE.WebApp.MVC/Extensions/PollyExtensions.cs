using Polly;
using Polly.Extensions.Http;
using Polly.Retry;

namespace NSE.WebApp.MVC.Extensions
{
    public static class PollyExtensions
    {
        public static AsyncRetryPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            var retry = HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                (outcome, timespan, retryCount, context) =>
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"Tentando pela {retryCount} vez!");
                    Console.ForegroundColor = ConsoleColor.White;
                });

            return retry;
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