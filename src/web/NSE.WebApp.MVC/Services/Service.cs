using NSE.WebApp.MVC.Extensions;
using NSE.WebApp.MVC.Models;
using System.Text.Json;

namespace NSE.WebApp.MVC.Services
{
    public abstract class Service
    {
        protected bool TratarErrosResponse(HttpResponseMessage response)
        {
            switch ((int)response.StatusCode)
            {
                case 401:
                case 403:
                case 404:
                case 500:
                    throw new CustomHttpRequestException(response.StatusCode);

                case 400:
                    return false;
            }

            response.EnsureSuccessStatusCode();
            return true;
        }

        protected async Task<DefaultResponseViewModel<T>> DeserializarDefaultResponseAsync<T>(HttpResponseMessage response)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            var result = JsonSerializer.Deserialize<DefaultResponseViewModel<T>>(await response.Content.ReadAsStringAsync(), options);
            return result;
        }

        protected async Task<T> DeserializarResponseAsync<T>(HttpResponseMessage response)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            return JsonSerializer.Deserialize<T>(await response.Content.ReadAsStringAsync(), options);
        }
    }
}
