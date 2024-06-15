using Microsoft.AspNetCore.Mvc;
using NSE.WebApp.MVC.Models;

namespace NSE.WebApp.MVC.Controllers
{
    public abstract class MainController : Controller
    {
        protected bool PossuiErrosResponse(DefaultResponseVM<UsuarioResponstaLoginVM> defaultResponseVM)
        {
            if (defaultResponseVM.Errors != null)
                foreach (var erro in defaultResponseVM.Errors)
                {
                    ModelState.AddModelError(string.Empty, erro);
                }

            return !defaultResponseVM.Success;
        }
    }
}
