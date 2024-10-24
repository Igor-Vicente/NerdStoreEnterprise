﻿using Microsoft.AspNetCore.Mvc;
using NSE.Core.Models;

namespace NSE.WebApp.MVC.Controllers
{
    public abstract class MainController : Controller
    {
        protected bool PossuiErrosResponse<T>(ResponseResult<T> defaultResponse)
        {
            if (defaultResponse.Errors != null)
                foreach (var erro in defaultResponse.Errors) ModelState.AddModelError(string.Empty, erro);
            return !defaultResponse.Success;
        }

        protected bool PossuiErrosResponse(ResponseResult defaultResponse)
        {
            if (defaultResponse.Errors != null)
                foreach (var erro in defaultResponse.Errors) ModelState.AddModelError(string.Empty, erro);
            return !defaultResponse.Success;
        }

        protected void AdicionarErroValidacao(string mensagem)
        {
            ModelState.AddModelError("", mensagem);
        }

        protected bool OperacaoValida()
        {
            return ModelState.ErrorCount == 0;
        }
    }
}
