using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using NSE.Core.Models;
using NSE.WebApi.Core.Usuario;
using NSE.WebApp.MVC.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace NSE.WebApp.MVC.Services
{
    public interface IAutenticacaoService
    {
        Task<ResponseResult<UsuarioResponstaLoginViewModel>> Login(UsuarioLoginViewModel loginVM);
        Task<ResponseResult<UsuarioResponstaLoginViewModel>> Register(UsuarioRegistroViewModel usuarioRegistroVM);
        Task<ResponseResult<UsuarioResponstaLoginViewModel>> UtilizarRefreshToken(string refreshToken);
        Task RealizarLogin(UsuarioResponstaLoginViewModel resposta);
        Task Logout();
        bool TokenExpirado();
        Task<bool> RefreshTokenValido();
    }
    public class AutenticacaoService : Service, IAutenticacaoService
    {
        private readonly HttpClient _httpClient;
        private readonly IAspNetUser _user;
        private readonly IAuthenticationService _authenticationService;

        public AutenticacaoService(HttpClient httpClient, IAspNetUser user, IAuthenticationService authenticationService)
        {
            _httpClient = httpClient;
            _user = user;
            _authenticationService = authenticationService;
        }

        public async Task<ResponseResult<UsuarioResponstaLoginViewModel>> Login(UsuarioLoginViewModel loginVM)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/v1/identidade/autenticar", loginVM);

            TratarErrosResponse(response);

            return await DeserializarDefaultResponseAsync<UsuarioResponstaLoginViewModel>(response);
        }

        public async Task<ResponseResult<UsuarioResponstaLoginViewModel>> Register(UsuarioRegistroViewModel usuarioRegistroVM)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/v1/identidade/nova-conta", usuarioRegistroVM);

            TratarErrosResponse(response);

            return await DeserializarDefaultResponseAsync<UsuarioResponstaLoginViewModel>(response);
        }


        public async Task<ResponseResult<UsuarioResponstaLoginViewModel>> UtilizarRefreshToken(string refreshToken)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/v1/identidade/refresh-token", refreshToken);

            TratarErrosResponse(response);

            return await DeserializarDefaultResponseAsync<UsuarioResponstaLoginViewModel>(response);
        }

        public async Task RealizarLogin(UsuarioResponstaLoginViewModel resposta)
        {
            var token = ObterTokenFormatado(resposta.AccessToken);

            var claims = new List<Claim>();
            claims.Add(new Claim("JWT", resposta.AccessToken));
            claims.Add(new Claim("RefreshToken", resposta.RefreshToken));
            claims.AddRange(token.Claims);

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8),
                IsPersistent = true
            };

            await _authenticationService.SignInAsync(
                _user.ObterHttpContext(),
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }

        public static JwtSecurityToken ObterTokenFormatado(string jwtToken)
        {
            return new JwtSecurityTokenHandler().ReadToken(jwtToken) as JwtSecurityToken;
        }

        public async Task Logout()
        {
            await _authenticationService.SignOutAsync(
                _user.ObterHttpContext(),
                CookieAuthenticationDefaults.AuthenticationScheme,
                null);
        }

        public bool TokenExpirado()
        {
            var jwt = _user.ObterUserToken();
            if (jwt is null) return true;

            var token = ObterTokenFormatado(jwt);
            return token.ValidTo.ToLocalTime() < DateTime.Now;
        }

        public async Task<bool> RefreshTokenValido()
        {
            var resposta = await UtilizarRefreshToken(_user.ObterUserRefreshToken());

            if (resposta.Data?.AccessToken != null && resposta.Success)
            {
                await RealizarLogin(resposta.Data);
                return true;
            }

            return false;
        }
    }
}
