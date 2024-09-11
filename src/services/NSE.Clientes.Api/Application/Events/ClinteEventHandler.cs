using MediatR;

namespace NSE.Clientes.Api.Application.Events
{
    public class ClinteEventHandler : INotificationHandler<ClienteRegistradoEvent>
    {
        public Task Handle(ClienteRegistradoEvent notification, CancellationToken cancellationToken)
        {
            //Enviar Evento de Confirmação
            return Task.CompletedTask;
        }
    }
}
