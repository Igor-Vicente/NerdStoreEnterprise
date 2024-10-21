
using NSE.Catalogo.Api.Data.Repository;
using NSE.Catalogo.Api.Models;
using NSE.Core.Messages.Integration;
using NSE.Core.ModelObjects;
using NSE.MessageBus;

namespace NSE.Catalogo.Api.Services
{
    public class CatalogoIntegrationHandler : BackgroundService
    {
        private readonly IMessageBus _bus;
        private readonly IServiceProvider _serviceProvider;
        private ILogger<CatalogoIntegrationHandler> _logger;

        public CatalogoIntegrationHandler(IServiceProvider serviceProvider, IMessageBus bus, ILogger<CatalogoIntegrationHandler> logger)
        {
            _serviceProvider = serviceProvider;
            _bus = bus;
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            SetSubscribers();
            return Task.CompletedTask;
        }

        private void SetSubscribers()
        {
            _bus.SubscribeAsync<PedidoAutorizadoIntegrationEvent>("PedidoAutorizado", async request =>
                await BaixarEstoque(request));
        }

        private async Task BaixarEstoque(PedidoAutorizadoIntegrationEvent message)
        {
            using var scope = _serviceProvider.CreateScope();

            var produtosComEstoque = new List<Produto>();
            var produtoRepository = scope.ServiceProvider.GetRequiredService<IProdutoRepository>();

            var idsProdutos = string.Join(",", message.Itens.Select(c => c.Key));
            var produtos = await produtoRepository.ObterProdutosPorId(idsProdutos);

            if (produtos.Count != message.Itens.Count)
            {
                CancelarPedidoSemEstoque(message);
                return;
            }

            foreach (var produto in produtos)
            {
                var quantidadeProduto = message.Itens.FirstOrDefault(p => p.Key == produto.Id).Value;

                if (produto.EstaDisponivel(quantidadeProduto))
                {
                    produto.RetirarEstoque(quantidadeProduto);
                    produtosComEstoque.Add(produto);
                }
            }

            if (produtosComEstoque.Count != message.Itens.Count)
            {
                CancelarPedidoSemEstoque(message);
                return;
            }

            foreach (var produto in produtosComEstoque)
            {
                produtoRepository.Atualizar(produto);
            }

            if (!await produtoRepository.UnitOfWork.CommitAsync())
            {
                throw new DomainException($"Problemas ao atualizar estoque do pedido {message.PedidoId}");
            }

            var pedidoBaixado = new PedidoBaixadoEstoqueIntegrationEvent(message.ClienteId, message.PedidoId);
            await _bus.PublishAsync(pedidoBaixado);
        }

        public async void CancelarPedidoSemEstoque(PedidoAutorizadoIntegrationEvent message)
        {
            var pedidoCancelado = new PedidoCanceladoIntegrationEvent(message.ClienteId, message.PedidoId);
            _logger.LogInformation($"Lançando PedidoCanceladoIntegrationEvent:  ClientId: {pedidoCancelado.ClienteId}, PedidoId: {pedidoCancelado.PedidoId}");
            await _bus.PublishAsync(pedidoCancelado);
        }
    }
}
