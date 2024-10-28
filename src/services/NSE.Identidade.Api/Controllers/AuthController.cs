using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NSE.Core.Messages.Integration;
using NSE.Identidade.Api.Models;
using NSE.Identidade.Api.Services;
using NSE.MessageBus;
using NSE.WebApi.Core.Controllers;

namespace NSE.Identidade.Api.Controllers
{
    [Route("api/v1/identidade")]
    public class AuthController : MainController
    {
        private readonly AuthenticationService _authenticationService;
        private readonly IMessageBus _bus;

        public AuthController(AuthenticationService authenticationService,
                              IMessageBus bus)
        {
            _authenticationService = authenticationService;
            _bus = bus;
        }


        [HttpPost("nova-conta")]
        public async Task<IActionResult> Registrar(UsuarioRegistro usuarioRegistro)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var usuario = new IdentityUser { Email = usuarioRegistro.Email, UserName = usuarioRegistro.Email };
            var result = await _authenticationService.UserManager.CreateAsync(usuario, usuarioRegistro.Senha);

            if (result.Succeeded)
            {
                var clienteResult = await RegistrarCliente(usuarioRegistro);

                if (!clienteResult.ValidationResult.IsValid)
                {
                    await _authenticationService.UserManager.DeleteAsync(usuario);
                    return CustomResponse(clienteResult.ValidationResult);
                }

                return CustomResponse(await _authenticationService.GerarJwtAsync(usuario.Email));
            }

            foreach (var erro in result.Errors)
            {
                AdicionarErroProcessamento(erro.Description);
            }

            return CustomResponse();
        }

        private async Task<ResponseMessage> RegistrarCliente(UsuarioRegistro usuarioRegistro)
        {
            var usuario = await _authenticationService.UserManager.FindByEmailAsync(usuarioRegistro.Email);
            var usuarioRegistrado = new UsuarioRegistradoIntegrationEvent(Guid.Parse(usuario.Id), usuarioRegistro.Nome, usuarioRegistro.Email, usuarioRegistro.Cpf);

            try
            {
                return await _bus.RequestAsync<UsuarioRegistradoIntegrationEvent, ResponseMessage>(usuarioRegistrado);
            }
            catch
            {
                await _authenticationService.UserManager.DeleteAsync(usuario);
                throw;
            }
        }

        [HttpPost("autenticar")]
        public async Task<IActionResult> Login(Login loginDto)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var result = await _authenticationService.SignInManager.PasswordSignInAsync(loginDto.Email, loginDto.Senha, false, true);
            if (result.Succeeded)
                return CustomResponse(await _authenticationService.GerarJwtAsync(loginDto.Email));

            if (result.IsLockedOut)
            {
                AdicionarErroProcessamento("Usuário temporariamente bloaqueado por tentativas inválidas");
                return CustomResponse();
            }

            AdicionarErroProcessamento("Usuário ou Senha inválido");
            return CustomResponse();
        }


        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                AdicionarErroProcessamento("Refresh Token inválido");
                return CustomResponse();
            }

            var token = await _authenticationService.ObterRefreshToken(Guid.Parse(refreshToken));

            if (token is null)
            {
                AdicionarErroProcessamento("Refresh Token expirado");
                return CustomResponse();
            }

            return CustomResponse(await _authenticationService.GerarJwtAsync(token.Username));
        }
    }
}
