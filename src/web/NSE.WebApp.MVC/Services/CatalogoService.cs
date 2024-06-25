using NSE.WebApp.MVC.Models;

namespace NSE.WebApp.MVC.Services
{
    public class CatalogoService : Service, ICatalogoService
    {
        private readonly HttpClient _httpClient;

        public CatalogoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<DefaultResponseVM<ProdutoViewModel>> ObterPorId(Guid id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/catalogo/produtos/{id}");
            TratarErrosResponse(response);
            return await DeserializarDefaultResponseAsync<ProdutoViewModel>(response);
        }

        public async Task<DefaultResponseVM<ProdutoViewModel[]>> ObterTodos()
        {
            var response = await _httpClient.GetAsync("/api/v1/catalogo/produtos");
            TratarErrosResponse(response);
            return await DeserializarDefaultResponseAsync<ProdutoViewModel[]>(response);
        }
    }
}
