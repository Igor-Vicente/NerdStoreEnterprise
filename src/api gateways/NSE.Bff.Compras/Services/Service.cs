using NSE.Core.Models;
using System.Net;
using System.Text.Json;

namespace NSE.Bff.Compras.Services
{
    public abstract class Service
    {
        protected bool TratarErrosResponse(HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.BadRequest) return false;

            response.EnsureSuccessStatusCode();
            return true;
        }

        protected async Task<ResponseResult<T>> DeserializarDefaultResponseAsync<T>(HttpResponseMessage response)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            var result = JsonSerializer.Deserialize<ResponseResult<T>>(await response.Content.ReadAsStringAsync(), options);
            return result;
        }

        protected async Task<ResponseResult> DeserializarDefaultResponseAsync(HttpResponseMessage response)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            var result = JsonSerializer.Deserialize<ResponseResult>(await response.Content.ReadAsStringAsync(), options);
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
