using NSE.Core.Models;
using NSE.WebApp.MVC.Models;
using System.Net;

namespace NSE.WebApp.MVC.Services
{
    public interface IClienteService
    {
        Task<ResponseResult<EnderecoViewModel>> ObterEndereco();
        Task<ResponseResult> AdicionarEndereco(EnderecoViewModel endereco);
    }
    public class ClienteService : Service, IClienteService
    {
        private readonly HttpClient _httpClient;

        public ClienteService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ResponseResult> AdicionarEndereco(EnderecoViewModel endereco)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/v1/cliente/endereco", endereco);

            TratarErrosResponse(response);

            return await DeserializarDefaultResponseAsync(response);
        }

        public async Task<ResponseResult<EnderecoViewModel>> ObterEndereco()
        {
            var response = await _httpClient.GetAsync("/api/v1/cliente/endereco");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            TratarErrosResponse(response);

            return await DeserializarDefaultResponseAsync<EnderecoViewModel>(response);
        }
    }
}
