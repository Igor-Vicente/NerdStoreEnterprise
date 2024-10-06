using NSE.WebApp.MVC.Models;

namespace NSE.WebApp.MVC.Services
{
    public interface ICarrinhoService
    {
        Task<CarrinhoViewModel> ObterCarrinho();
        Task<DefaultResponseViewModel<CarrinhoViewModel>> AdicionarItemCarrinho(ItemProdutoViewModel produto);
        Task<DefaultResponseViewModel<CarrinhoViewModel>> AtualizarItemCarrinho(Guid produtoId, ItemProdutoViewModel produto);
        Task<DefaultResponseViewModel<CarrinhoViewModel>> RemoverItemCarrinho(Guid produtoId);
    }

    public class CarrinhoService : Service, ICarrinhoService
    {
        private readonly HttpClient _httpClient;

        public CarrinhoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CarrinhoViewModel> ObterCarrinho()
        {
            var response = await _httpClient.GetAsync($"/api/v1/carrinho");

            TratarErrosResponse(response);

            return await DeserializarResponseAsync<CarrinhoViewModel>(response);
        }

        public async Task<DefaultResponseViewModel<CarrinhoViewModel>> AdicionarItemCarrinho(ItemProdutoViewModel produto)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/v1/carrinho", produto);

            TratarErrosResponse(response);

            return await DeserializarDefaultResponseAsync<CarrinhoViewModel>(response);
        }

        public async Task<DefaultResponseViewModel<CarrinhoViewModel>> AtualizarItemCarrinho(Guid produtoId, ItemProdutoViewModel produto)
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/v1/carrinho/{produtoId}", produto);

            TratarErrosResponse(response);

            return await DeserializarDefaultResponseAsync<CarrinhoViewModel>(response);
        }

        public async Task<DefaultResponseViewModel<CarrinhoViewModel>> RemoverItemCarrinho(Guid produtoId)
        {
            var response = await _httpClient.DeleteAsync($"/api/v1/carrinho/{produtoId}");

            TratarErrosResponse(response);

            return await DeserializarDefaultResponseAsync<CarrinhoViewModel>(response);
        }
    }
}
