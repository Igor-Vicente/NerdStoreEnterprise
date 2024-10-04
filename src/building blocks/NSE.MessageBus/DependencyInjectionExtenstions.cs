using Microsoft.Extensions.DependencyInjection;

namespace NSE.MessageBus
{
    public static class DependencyInjectionExtenstions
    {
        public static IServiceCollection AddMessageBus(this IServiceCollection service, string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString)) throw new ArgumentNullException(nameof(connectionString));

            service.AddSingleton<IMessageBus>(new MessageBus(connectionString));

            return service;
        }
    }
}
