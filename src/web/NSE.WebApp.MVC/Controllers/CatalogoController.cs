using Microsoft.AspNetCore.Mvc;
using NSE.WebApp.MVC.Services;

namespace NSE.WebApp.MVC.Controllers
{
    public class CatalogoController : MainController
    {
        //private readonly ICatalogoServiceRefit _catalogoService;
        private readonly ICatalogoService _catalogoService;

        public CatalogoController(ICatalogoService catalogoService)
        {
            _catalogoService = catalogoService;
        }


        [Route("")]
        [HttpGet("vitrine")]
        public async Task<IActionResult> Index()
        {
            var response = await _catalogoService.ObterTodos();
            return View(response.Data);
        }


        [HttpGet("produto-detalhe/{id:guid}")]
        public async Task<IActionResult> ProdutoDetalhe(Guid id)
        {
            var response = await _catalogoService.ObterPorId(id);
            return View(response.Data);
        }
    }
}
