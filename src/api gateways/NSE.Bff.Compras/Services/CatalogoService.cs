using NSE.Bff.Compras.Models;
using NSE.Core.Models;

namespace NSE.Bff.Compras.Services
{
    public interface ICatalogoService
    {
        Task<ResponseResult<ItemProdutoDTO>> ObterPorId(Guid id);
        Task<ResponseResult<IEnumerable<ItemProdutoDTO>>> ObterItens(IEnumerable<Guid> ids);
    }

    public class CatalogoService : Service, ICatalogoService
    {
        private readonly HttpClient _httpClient;

        public CatalogoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }



        public async Task<ResponseResult<ItemProdutoDTO>> ObterPorId(Guid id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/catalogo/produtos/{id}");

            TratarErrosResponse(response);

            return await DeserializarDefaultResponseAsync<ItemProdutoDTO>(response);
        }

        public async Task<ResponseResult<IEnumerable<ItemProdutoDTO>>> ObterItens(IEnumerable<Guid> ids)
        {
            var idsRequest = string.Join(",", ids);

            var response = await _httpClient.GetAsync($"/api/v1/catalogo/produtos/lista/{idsRequest}");

            TratarErrosResponse(response);

            return await DeserializarDefaultResponseAsync<IEnumerable<ItemProdutoDTO>>(response);
        }
    }
}

