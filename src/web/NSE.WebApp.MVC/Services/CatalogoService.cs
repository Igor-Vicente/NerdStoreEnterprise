using NSE.Core.Models;
using NSE.WebApp.MVC.Models;
using Refit;

namespace NSE.WebApp.MVC.Services
{
    public interface ICatalogoService
    {
        Task<ResponseResult<ProdutoViewModel>> ObterPorId(Guid id);
        Task<ResponseResult<ProdutoViewModel[]>> ObterTodos();
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

        public async Task<ResponseResult<ProdutoViewModel[]>> ObterTodos()
        {
            var response = await _httpClient.GetAsync("/api/v1/catalogo/produtos");
            TratarErrosResponse(response);
            return await DeserializarDefaultResponseAsync<ProdutoViewModel[]>(response);
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
