using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using NSE.WebApp.MVC.Models;
using NSE.WebApp.MVC.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace NSE.WebApp.MVC.Controllers
{
    public class IdentidadeController : MainController
    {
        private readonly IAutenticacaoService _autenticacaoService;

        public IdentidadeController(IAutenticacaoService autenticacaoService)
        {
            _autenticacaoService = autenticacaoService;
        }


        [HttpGet("nova-conta")]
        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost("nova-conta")]
        public async Task<IActionResult> Registro(UsuarioRegistroVM usuarioRegistroVM)
        {
            if (!ModelState.IsValid) return View(usuarioRegistroVM);

            var resp = await _autenticacaoService.Register(usuarioRegistroVM);
            if (PossuiErrosResponse(resp)) return View(usuarioRegistroVM);

            await RealizarLogin(resp.Data);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet("login")]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UsuarioLoginVM usuarioLoginVM, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (!ModelState.IsValid) return BadRequest(usuarioLoginVM);

            var resp = await _autenticacaoService.Login(usuarioLoginVM);
            if (PossuiErrosResponse(resp)) return View(usuarioLoginVM);

            await RealizarLogin(resp.Data);

            if (string.IsNullOrEmpty(returnUrl)) return RedirectToAction("Index", "Home");
            return LocalRedirect(returnUrl);
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        private async Task RealizarLogin(UsuarioResponstaLoginVM usuarioResponstaLoginVM)
        {
            var jwt = ObterTokenFormatado(usuarioResponstaLoginVM.AccessToken);
            var claims = new List<Claim>();
            claims.Add(new Claim("JWT", usuarioResponstaLoginVM.AccessToken));
            claims.AddRange(jwt.Claims);

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60),
                IsPersistent = true,
            };

            //metodo abaixo é do proprio aspnet, e não do identity (até pq identity não está sendo usado como uma dependencia nesse assembly)
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

        }

        private JwtSecurityToken ObterTokenFormatado(string jwt)
        {
            return new JwtSecurityTokenHandler().ReadJwtToken(jwt);
        }
    }
}
