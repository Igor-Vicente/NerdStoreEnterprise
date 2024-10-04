using NSE.Clientes.Api.Application.Commands;
using NSE.Core.Mediator;
using NSE.Core.Messages.Integration;
using NSE.MessageBus;

namespace NSE.Clientes.Api.Services
{
    public class RegistroClienteIntegrationHandler : BackgroundService
    {
        private readonly IMessageBus _bus;
        private readonly IServiceProvider _serviceProvider;
        public RegistroClienteIntegrationHandler(IServiceProvider serviceProvider, IMessageBus bus)
        {
            _serviceProvider = serviceProvider;
            _bus = bus;
        }

        private async Task SetResponder()
        {
            await _bus.RespondAsync<UsuarioRegistradoIntegrationEvent, ResponseMessage>(async request =>
                await RegistrarCliente(request));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await SetResponder();
            _bus.AdvancedBus.Connected += OnConnect;
        }

        private async void OnConnect(object s, EventArgs e)
        {
            await SetResponder();
        }

        private async Task<ResponseMessage> RegistrarCliente(UsuarioRegistradoIntegrationEvent message)
        {
            var clienteCommand = new RegistrarClienteCommand(message.Id, message.Nome, message.Email, message.Cpf);

            using var scope = _serviceProvider.CreateScope();

            var mediator = scope.ServiceProvider.GetRequiredService<IMediatorHandler>();
            var sucesso = await mediator.EnviarComando(clienteCommand);

            return new ResponseMessage(sucesso);
        }
    }
}
