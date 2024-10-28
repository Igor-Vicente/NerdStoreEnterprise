using NSE.Core.Models;
using NSE.WebApp.MVC.Models;
using Refit;

namespace NSE.WebApp.MVC.Services
{
    public interface ICatalogoService
    {
        Task<ResponseResult<ProdutoViewModel>> ObterPorId(Guid id);
        Task<PagedViewModel<ProdutoViewModel>> ObterTodos(int pageSize, int pageIndex, string query = null);
    }

    public class CatalogoService : Service, ICatalogoService
    {
        private readonly HttpClient _httpClient;

        public CatalogoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<ResponseResult<ProdutoViewModel>> ObterPorId(Guid id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/catalogo/produtos/{id}");
            TratarErrosResponse(response);
            return await DeserializarDefaultResponseAsync<ProdutoViewModel>(response);
        }

        public async Task<PagedViewModel<ProdutoViewModel>> ObterTodos(int pageSize, int pageIndex, string query = null)
        {
            var response = await _httpClient.GetAsync($"/api/v1/catalogo/produtos?ps={pageSize}&page={pageIndex}&q={query}");
            TratarErrosResponse(response);
            return await DeserializarResponseAsync<PagedViewModel<ProdutoViewModel>>(response);
        }
    }

    public interface ICatalogoServiceRefit
    {
        [Get("/api/v1/catalogo/produtos/{id}")]
        Task<ResponseResult<ProdutoViewModel>> ObterPorId(Guid id);

        [Get("/api/v1/catalogo/produtos")]
        Task<ResponseResult<ProdutoViewModel[]>> ObterTodos();
    }
}
