using Microsoft.AspNetCore.Mvc;
using NSE.Core.Mediator;
using NSE.WebApi.Core.Controllers;

namespace NSE.Clientes.Api.Controllers
{
    public class ClientesController : MainController
    {
        private readonly IMediatorHandler _mediatorHandler;

        public ClientesController(IMediatorHandler mediatorHandler)
        {
            _mediatorHandler = mediatorHandler;
        }

        [HttpGet("cliente")]
        public async Task<IActionResult> Teste()
        {
            //var result = await _mediatorHandler.EnviarComando(new RegistrarClienteCommand(Guid.NewGuid(), "Maria", "maria@gmail.com", "25747656058"));
            return CustomResponse();
        }
    }
}
