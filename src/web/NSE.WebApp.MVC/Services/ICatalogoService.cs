using NSE.WebApp.MVC.Models;
using Refit;

namespace NSE.WebApp.MVC.Services
{
    public interface ICatalogoService
    {
        Task<DefaultResponseVM<ProdutoViewModel>> ObterPorId(Guid id);
        Task<DefaultResponseVM<ProdutoViewModel[]>> ObterTodos();
        // Task<ProdutoViewModel> ObterPorId(Guid id);
        //Task<IEnumerable<ProdutoViewModel>> ObterTodos();
    }

    public interface ICatalogoServiceRefit
    {
        [Get("/api/v1/catalogo/produtos/{id}")]
        Task<DefaultResponseVM<ProdutoViewModel>> ObterPorId(Guid id);

        [Get("/api/v1/catalogo/produtos")]
        Task<DefaultResponseVM<ProdutoViewModel[]>> ObterTodos();
    }
}
