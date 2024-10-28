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
        public async Task<IActionResult> Index([FromQuery] int ps = 8, [FromQuery] int page = 1, [FromQuery] string q = null)
        {
            var produtos = await _catalogoService.ObterTodos(ps, page, q);
            ViewBag.pesquisa = q;
            produtos.ReferenceAction = "Index";
            return View(produtos);
        }


        [HttpGet("produto-detalhe/{id:guid}")]
        public async Task<IActionResult> ProdutoDetalhe(Guid id)
        {
            var response = await _catalogoService.ObterPorId(id);
            return View(response.Data);
        }
    }
}
