using NSE.WebApp.MVC.Models;
using Refit;

namespace NSE.WebApp.MVC.Services
{
    public interface ICatalogoService
    {
        Task<DefaultResponseViewModel<ProdutoViewModel>> ObterPorId(Guid id);
        Task<DefaultResponseViewModel<ProdutoViewModel[]>> ObterTodos();
        // Task<ProdutoViewModel> ObterPorId(Guid id);
        //Task<IEnumerable<ProdutoViewModel>> ObterTodos();
    }

    public interface ICatalogoServiceRefit
    {
        [Get("/api/v1/catalogo/produtos/{id}")]
        Task<DefaultResponseViewModel<ProdutoViewModel>> ObterPorId(Guid id);

        [Get("/api/v1/catalogo/produtos")]
        Task<DefaultResponseViewModel<ProdutoViewModel[]>> ObterTodos();
    }
}
