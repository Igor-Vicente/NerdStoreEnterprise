using NSE.Bff.Compras.Models;
using NSE.Core.Models;
using System.Net;

namespace NSE.Bff.Compras.Services
{
    public interface IClienteService
    {
        Task<ResponseResult<EnderecoDTO>> ObterEndereco();
    }
    public class ClienteService : Service, IClienteService
    {
        private readonly HttpClient _httpClient;

        public ClienteService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ResponseResult<EnderecoDTO>> ObterEndereco()
        {
            var response = await _httpClient.GetAsync("/api/v1/cliente/endereco");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            TratarErrosResponse(response);

            return await DeserializarDefaultResponseAsync<EnderecoDTO>(response);
        }
    }
}
