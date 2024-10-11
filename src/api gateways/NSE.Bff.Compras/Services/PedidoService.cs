using NSE.Bff.Compras.Models;
using NSE.Core.Models;
using System.Net;

namespace NSE.Bff.Compras.Services
{
    public interface IPedidoService
    {
        Task<ResponseResult> FinalizarPedido(PedidoDTO pedido);
        Task<ResponseResult<PedidoDTO>> ObterUltimoPedido();
        Task<ResponseResult<IEnumerable<PedidoDTO>>> ObterListaPorClienteId();

        Task<ResponseResult<VoucherDTO>> ObterVoucherPorCodigo(string codigo);
    }
    public class PedidoService : Service, IPedidoService
    {
        private readonly HttpClient _httpClient;

        public PedidoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ResponseResult> FinalizarPedido(PedidoDTO pedido)
        {
            var response = await _httpClient.PostAsJsonAsync($"/api/v1/pedido", pedido);

            TratarErrosResponse(response);

            return await DeserializarDefaultResponseAsync(response);
        }

        public async Task<ResponseResult<IEnumerable<PedidoDTO>>> ObterListaPorClienteId()
        {
            var response = await _httpClient.GetAsync("/api/v1/pedido/lista-cliente");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            TratarErrosResponse(response);

            return await DeserializarDefaultResponseAsync<IEnumerable<PedidoDTO>>(response);
        }

        public async Task<ResponseResult<PedidoDTO>> ObterUltimoPedido()
        {
            var response = await _httpClient.GetAsync("/api/v1/pedido/ultimo");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            TratarErrosResponse(response);

            return await DeserializarDefaultResponseAsync<PedidoDTO>(response);
        }

        public async Task<ResponseResult<VoucherDTO>> ObterVoucherPorCodigo(string codigo)
        {
            var response = await _httpClient.GetAsync($"/api/v1/voucher/{codigo}");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            TratarErrosResponse(response);

            return await DeserializarDefaultResponseAsync<VoucherDTO>(response);
        }
    }
}
