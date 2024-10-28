using Microsoft.AspNetCore.Mvc;
using NSE.WebApp.MVC.Models;
using NSE.WebApp.MVC.Services;

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
        public async Task<IActionResult> Registro(UsuarioRegistroViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var resposta = await _autenticacaoService.Register(model);
            if (PossuiErrosResponse(resposta)) return View(model);

            await _autenticacaoService.RealizarLogin(resposta.Data);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet("login")]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UsuarioLoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (!ModelState.IsValid) return BadRequest(model);

            var resposta = await _autenticacaoService.Login(model);
            if (PossuiErrosResponse(resposta)) return View(model);

            await _autenticacaoService.RealizarLogin(resposta.Data);

            if (string.IsNullOrEmpty(returnUrl)) return RedirectToAction("Index", "Home");
            return LocalRedirect(returnUrl);
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await _autenticacaoService.Logout();
            return RedirectToAction("Index", "Catalogo");
        }
    }
}
