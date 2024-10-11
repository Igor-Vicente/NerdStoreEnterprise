using NSE.Core.Models;
using NSE.WebApp.MVC.Models;

namespace NSE.WebApp.MVC.Services
{
    public interface IComprasBffService
    {
        // Carrinho
        Task<ResponseResult<CarrinhoViewModel>> ObterCarrinho();
        Task<int> ObterQuantidadeCarrinho();
        Task<ResponseResult> AdicionarItemCarrinho(ItemCarrinhoViewModel produto);
        Task<ResponseResult> AtualizarItemCarrinho(Guid produtoId, ItemCarrinhoViewModel produto);
        Task<ResponseResult> RemoverItemCarrinho(Guid produtoId);
        Task<ResponseResult> AplicarVoucherCarrinho(string voucher);

        // Pedido
        Task<ResponseResult> FinalizarPedido(PedidoTransacaoViewModel pedidoTransacao);
        Task<ResponseResult<PedidoViewModel>> ObterUltimoPedido();
        Task<ResponseResult<IEnumerable<PedidoViewModel>>> ObterListaPorClienteId();
        PedidoTransacaoViewModel MapearParaPedido(CarrinhoViewModel carrinho, EnderecoViewModel endereco);
    }

    public class ComprasBffService : Service, IComprasBffService
    {
        private readonly HttpClient _httpClient;

        public ComprasBffService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        #region Carrinho

        public async Task<ResponseResult<CarrinhoViewModel>> ObterCarrinho()
        {
            var response = await _httpClient.GetAsync($"/api/v1/compras/carrinho");

            TratarErrosResponse(response);

            return await DeserializarDefaultResponseAsync<CarrinhoViewModel>(response);
        }

        public async Task<ResponseResult> AdicionarItemCarrinho(ItemCarrinhoViewModel produto)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/v1/compras/carrinho/items", produto);

            TratarErrosResponse(response);

            return await DeserializarDefaultResponseAsync(response);
        }

        public async Task<ResponseResult> AtualizarItemCarrinho(Guid produtoId, ItemCarrinhoViewModel produto)
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/v1/compras/carrinho/items/{produtoId}", produto);

            TratarErrosResponse(response);

            return await DeserializarDefaultResponseAsync(response);
        }

        public async Task<ResponseResult> RemoverItemCarrinho(Guid produtoId)
        {
            var response = await _httpClient.DeleteAsync($"/api/v1/compras/carrinho/items/{produtoId}");

            TratarErrosResponse(response);

            return await DeserializarDefaultResponseAsync(response);
        }

        public async Task<int> ObterQuantidadeCarrinho()
        {
            var response = await _httpClient.GetAsync("/api/v1/compras/carrinho-quantidade");

            TratarErrosResponse(response);

            return await DeserializarResponseAsync<int>(response);
        }

        public async Task<ResponseResult> AplicarVoucherCarrinho(string voucher)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/v1/compras/carrinho/aplicar-voucher", voucher);

            TratarErrosResponse(response);

            return await DeserializarDefaultResponseAsync(response);
        }

        #endregion

        #region Pedido

        public async Task<ResponseResult> FinalizarPedido(PedidoTransacaoViewModel pedidoTransacao)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/v1/compras/pedido", pedidoTransacao);

            TratarErrosResponse(response);

            return await DeserializarDefaultResponseAsync(response);
        }

        public async Task<ResponseResult<PedidoViewModel>> ObterUltimoPedido()
        {
            var response = await _httpClient.GetAsync("/api/v1/compras/pedido/ultimo");

            TratarErrosResponse(response);

            return await DeserializarDefaultResponseAsync<PedidoViewModel>(response);
        }

        public async Task<ResponseResult<IEnumerable<PedidoViewModel>>> ObterListaPorClienteId()
        {
            var response = await _httpClient.GetAsync("/api/v1/compras/pedido/lista-cliente");

            TratarErrosResponse(response);

            return await DeserializarDefaultResponseAsync<IEnumerable<PedidoViewModel>>(response);
        }

        public PedidoTransacaoViewModel MapearParaPedido(CarrinhoViewModel carrinho, EnderecoViewModel endereco)
        {
            var pedido = new PedidoTransacaoViewModel
            {
                ValorTotal = carrinho.ValorTotal,
                Itens = carrinho.Itens,
                Desconto = carrinho.Desconto,
                VoucherUtilizado = carrinho.VoucherUtilizado,
                VoucherCodigo = carrinho.Voucher?.Codigo
            };

            if (endereco != null)
            {
                pedido.Endereco = new EnderecoViewModel
                {
                    Logradouro = endereco.Logradouro,
                    Numero = endereco.Numero,
                    Bairro = endereco.Bairro,
                    Cep = endereco.Cep,
                    Complemento = endereco.Complemento,
                    Cidade = endereco.Cidade,
                    Estado = endereco.Estado
                };
            }

            return pedido;
        }

        #endregion
    }
}
