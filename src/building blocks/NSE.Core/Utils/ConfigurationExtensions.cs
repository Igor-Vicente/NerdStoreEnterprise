using Microsoft.Extensions.Configuration;

namespace NSE.Core.Utils
{
    public static class ConfigurationExtensions
    {
        public static string GetMessageQueueConnection(this IConfiguration configuraiton, string name)
        {
            return configuraiton?.GetSection("MessageQueueConnection")?[name];
        }
    }
}
