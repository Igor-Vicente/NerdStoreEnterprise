using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSE.WebApp.MVC.Models;
using NSE.WebApp.MVC.Services;

namespace NSE.WebApp.MVC.Controllers
{
    [Authorize]
    public class PedidoController : MainController
    {
        private readonly IClienteService _clienteService;
        private readonly IComprasBffService _comprasBffService;

        public PedidoController(IClienteService clienteService,
            IComprasBffService comprasBffService)
        {
            _clienteService = clienteService;
            _comprasBffService = comprasBffService;
        }


        [HttpGet("endereco-de-entrega")]
        public async Task<IActionResult> EnderecoEntrega()
        {
            var carrinho = await _comprasBffService.ObterCarrinho();
            if (carrinho.Data.Itens.Count == 0) return RedirectToAction("Index", "Carrinho");

            var endereco = await _clienteService.ObterEndereco();
            var pedido = _comprasBffService.MapearParaPedido(carrinho?.Data, endereco?.Data);

            return View(pedido);
        }

        [HttpGet("pagamento")]
        public async Task<IActionResult> Pagamento()
        {
            var carrinho = await _comprasBffService.ObterCarrinho();
            if (carrinho.Data.Itens.Count == 0) return RedirectToAction("Index", "Carrinho");

            var pedido = _comprasBffService.MapearParaPedido(carrinho.Data, null);

            return View(pedido);
        }


        [HttpPost("finalizar-pedido")]
        public async Task<IActionResult> FinalizarPedido(PedidoTransacaoViewModel pedidoTransacao)
        {
            var pedido = _comprasBffService.MapearParaPedido(_comprasBffService.ObterCarrinho().Result.Data, null);
            if (!ModelState.IsValid) return View("Pagamento", pedido);

            var retorno = await _comprasBffService.FinalizarPedido(pedidoTransacao);

            if (PossuiErrosResponse(retorno))
            {
                var carrinho = await _comprasBffService.ObterCarrinho();
                if (carrinho.Data.Itens.Count == 0) return RedirectToAction("Index", "Carrinho");

                var pedidoMap = _comprasBffService.MapearParaPedido(carrinho.Data, null);
                return View("Pagamento", pedidoMap);
            }

            return RedirectToAction("PedidoConcluido");
        }

        [HttpGet("pedido-concluido")]
        public async Task<IActionResult> PedidoConcluido()
        {
            var pedido = await _comprasBffService.ObterUltimoPedido();
            return View("ConfirmacaoPedido", pedido.Data);
        }

        [HttpGet("meus-pedidos")]
        public async Task<IActionResult> MeusPedidos()
        {
            var pedidos = await _comprasBffService.ObterListaPorClienteId();
            return View(pedidos.Data);
        }
    }
}
