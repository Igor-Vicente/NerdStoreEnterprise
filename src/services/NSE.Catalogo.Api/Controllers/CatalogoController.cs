using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSE.Catalogo.Api.Models;
using NSE.WebApi.Core.Identidade;

namespace NSE.Catalogo.Api.Controllers
{
    [Authorize]
    [Route("api/v1/catalogo")]
    public class CatalogoController : MainController
    {
        private readonly IProdutoRepository _produtoRepository;

        public CatalogoController(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        [AllowAnonymous]
        [HttpGet("produtos")]
        public async Task<IActionResult> TodosProdutos()
        {
            var produtos = await _produtoRepository.ObterTodosAsync();
            return CustomResponse(produtos);
        }

        [ClaimsAuthorize("Catalogo", "Ler")]
        [HttpGet("produtos/{id:guid}")]
        public async Task<IActionResult> ProdutoDetalhes(Guid id)
        {
            //throw new Exception("Error");
            var produto = await _produtoRepository.ObterPorId(id);
            return CustomResponse(produto);
        }
    }
}
