using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSE.WebApp.MVC.Models;
using NSE.WebApp.MVC.Services;

namespace NSE.WebApp.MVC.Controllers
{
    [Authorize]
    public class CarrinhoController : MainController
    {
        private readonly IComprasBffService _comprasBffService;

        public CarrinhoController(IComprasBffService bffCompras)
        {
            _comprasBffService = bffCompras;
        }


        [Route("carrinho")]
        public async Task<IActionResult> Index()
        {
            var carrinho = await _comprasBffService.ObterCarrinho();
            return View(carrinho.Data);
        }

        [HttpPost]
        [Route("carrinho/adicionar-item")]
        public async Task<IActionResult> AdicionarItemCarrinho(ItemCarrinhoViewModel model)
        {
            var resposta = await _comprasBffService.AdicionarItemCarrinho(model);

            if (PossuiErrosResponse(resposta)) return View("Index", _comprasBffService.ObterCarrinho().Result.Data);

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("carrinho/atualizar-item")]
        public async Task<IActionResult> AtualizarItemCarrinho(Guid produtoId, int quantidade)
        {
            var itemCarrinho = new ItemCarrinhoViewModel { ProdutoId = produtoId, Quantidade = quantidade };
            var resposta = await _comprasBffService.AtualizarItemCarrinho(produtoId, itemCarrinho);

            if (PossuiErrosResponse(resposta)) return View("Index", _comprasBffService.ObterCarrinho().Result.Data);

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("carrinho/remover-item")]
        public async Task<IActionResult> RemoverItemCarrinho(Guid produtoId)
        {
            var resposta = await _comprasBffService.RemoverItemCarrinho(produtoId);

            if (PossuiErrosResponse(resposta)) return View("Index", _comprasBffService.ObterCarrinho().Result.Data);

            return RedirectToAction("Index");
        }

        [HttpPost("carrinho/aplicar-voucher")]
        public async Task<IActionResult> AplicarVoucher(string voucherCodigo)
        {
            var resposta = await _comprasBffService.AplicarVoucherCarrinho(voucherCodigo);

            if (PossuiErrosResponse(resposta)) return View("Index", _comprasBffService.ObterCarrinho().Result.Data);

            return RedirectToAction("Index");
        }
    }
}
