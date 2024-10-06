using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSE.Carrinho.Api.Data;
using NSE.Carrinho.Api.Model;
using NSE.WebApi.Core.Controllers;
using NSE.WebApi.Core.Usuario;

namespace NSE.Carrinho.Api.Controllers
{
    [Authorize]
    [Route("api/v1/carrinho")]
    public class CarrinhoController : MainController
    {
        private readonly IAspNetUser _aspNetUser;
        private readonly CarrinhoContext _context;

        public CarrinhoController(IAspNetUser aspNetUser, CarrinhoContext context)
        {
            _aspNetUser = aspNetUser;
            _context = context;
        }


        [HttpGet]
        public async Task<CarrinhoCliente> ObterCarrinho()
        {
            return await ObterCarrinhoCliente() ?? new CarrinhoCliente();
        }

        [HttpPost]
        public async Task<IActionResult> AdicionarItemCarrinho(CarrinhoItem item)
        {
            var carrinho = await ObterCarrinhoCliente();
            if (carrinho == null)
            {
                ManipularNovoCarrinho(item);
            }
            else
            {
                ManipularCarrinhoExistente(carrinho, item);
            }

            if (!OperacaoValida()) return CustomResponse();

            await PersistirDados();
            return CustomResponse();
        }

        [HttpPut("{produtoId}")]
        public async Task<IActionResult> AtualizarItemCarrinho(Guid produtoId, CarrinhoItem item)
        {
            var carrinho = await ObterCarrinhoCliente();
            var itemCarrinho = await ObterItemCarrinhoValidado(produtoId, carrinho, item);
            if (itemCarrinho == null) return CustomResponse();

            carrinho.AtualizarUnidades(itemCarrinho, item.Quantidade);

            if (!OperacaoValida()) return CustomResponse();

            _context.CarrinhoItem.Update(itemCarrinho);
            _context.CarrinhoCliente.Update(carrinho);

            await PersistirDados();
            return CustomResponse();
        }



        [HttpDelete("{produtoId:guid}")]
        public async Task<IActionResult> RemoverItemCarrinho(Guid produtoId)
        {
            var carrinho = await ObterCarrinhoCliente();
            var itemCarrinho = await ObterItemCarrinhoValidado(produtoId, carrinho);
            if (itemCarrinho == null) return CustomResponse();

            ValidarCarrinho(carrinho);
            if (!OperacaoValida()) return CustomResponse();

            carrinho.RemoverItem(itemCarrinho);

            _context.CarrinhoItem.Remove(itemCarrinho);
            _context.CarrinhoCliente.Update(carrinho);

            await PersistirDados();
            return CustomResponse();
        }

        private async Task<CarrinhoCliente> ObterCarrinhoCliente()
        {
            return await _context.CarrinhoCliente
                 .Include(c => c.Itens)
                 .FirstOrDefaultAsync(c => c.ClienteId == _aspNetUser.ObterUserId());
        }

        private void ManipularNovoCarrinho(CarrinhoItem item)
        {
            var carrinho = new CarrinhoCliente(_aspNetUser.ObterUserId());
            carrinho.AdicionarItem(item);
            ValidarCarrinho(carrinho);
            _context.CarrinhoCliente.Add(carrinho);
        }

        private void ManipularCarrinhoExistente(CarrinhoCliente carrinho, CarrinhoItem item)
        {
            var produtoExistente = carrinho.ObterPorProdutoId(item.ProdutoId);
            carrinho.AdicionarItem(item);
            ValidarCarrinho(carrinho);

            if (produtoExistente != null)
            {
                _context.CarrinhoItem.Update(produtoExistente);
            }
            else
            {
                _context.CarrinhoItem.Add(item);
            }

            _context.CarrinhoCliente.Update(carrinho);
        }

        private async Task<CarrinhoItem> ObterItemCarrinhoValidado(Guid produtoId, CarrinhoCliente carrinhoCliente, CarrinhoItem item = null)
        {
            if (item != null && produtoId != item.ProdutoId)
            {
                AdicionarErroProcessamento("O item não corresponde ao informado");
                return null;
            }
            if (carrinhoCliente == null)
            {
                AdicionarErroProcessamento("Carrinho não encontrado");
                return null;
            }
            var itemCarrinho = await _context.CarrinhoItem.FirstOrDefaultAsync(i => i.CarrinhoId == carrinhoCliente.Id && i.ProdutoId == produtoId);
            if (itemCarrinho == null)
            {
                AdicionarErroProcessamento("O item não está no carrinho");
                return null;
            }

            return itemCarrinho;
        }

        private async Task PersistirDados()
        {
            var result = await _context.SaveChangesAsync();
            if (result < 0) AdicionarErroProcessamento("Não foi possível persistir os dados no banco");
        }

        private bool ValidarCarrinho(CarrinhoCliente carrinho)
        {
            if (carrinho.EhValido()) return true;
            carrinho.ValidationResult.Errors.ForEach(e => AdicionarErroProcessamento(e.ErrorMessage));
            return false;
        }
    }
}
