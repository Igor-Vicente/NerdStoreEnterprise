using NSE.Bff.Compras.Models;
using NSE.Core.Models;

namespace NSE.Bff.Compras.Services
{
    public interface ICarrinhoService
    {
        Task<CarrinhoDTO> ObterCarrinho();
        Task<ResponseResult> AdicionarItemCarrinho(ItemCarrinhoDTO produto);
        Task<ResponseResult> AtualizarItemCarrinho(Guid produtoId, ItemCarrinhoDTO carrinho);
        Task<ResponseResult> RemoverItemCarrinho(Guid produtoId);
        Task<ResponseResult> AplicarVoucherCarrinho(VoucherDTO voucher);
    }

    public class CarrinhoService : Service, ICarrinhoService
    {
        private readonly HttpClient _httpClient;

        public CarrinhoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<CarrinhoDTO> ObterCarrinho()
        {
            var response = await _httpClient.GetAsync($"/api/v1/carrinho");

            TratarErrosResponse(response);

            return await DeserializarResponseAsync<CarrinhoDTO>(response);
        }

        public async Task<ResponseResult> AdicionarItemCarrinho(ItemCarrinhoDTO produto)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/v1/carrinho", produto);

            TratarErrosResponse(response);

            return await DeserializarDefaultResponseAsync(response);
        }

        public async Task<ResponseResult> AtualizarItemCarrinho(Guid produtoId, ItemCarrinhoDTO carrinho)
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/v1/carrinho/{produtoId}", carrinho);

            TratarErrosResponse(response);

            return await DeserializarDefaultResponseAsync(response);
        }

        public async Task<ResponseResult> RemoverItemCarrinho(Guid produtoId)
        {
            var response = await _httpClient.DeleteAsync($"/api/v1/carrinho/{produtoId}");

            TratarErrosResponse(response);

            return await DeserializarDefaultResponseAsync(response);
        }

        public async Task<ResponseResult> AplicarVoucherCarrinho(VoucherDTO voucher)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/v1/carrinho/aplicar-voucher", voucher);

            TratarErrosResponse(response);

            return await DeserializarDefaultResponseAsync(response);
        }
    }
}
