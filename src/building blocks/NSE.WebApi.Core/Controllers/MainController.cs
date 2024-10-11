using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NSE.Core.Models;

namespace NSE.WebApi.Core.Controllers
{
    [ApiController]
    public abstract class MainController : ControllerBase
    {
        protected ICollection<string> Erros = new List<string>();

        protected IActionResult CustomResponse(object result = null)
        {
            if (OperacaoValida())
            {
                return Ok(new
                {
                    success = true,
                    data = result
                });
            }
            else
            {
                return BadRequest(new
                {
                    success = false,
                    errors = Erros.ToArray()
                });

                //return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]> { {"Mensagens", Errors.ToArray() } }));
            }
        }
        protected IActionResult CustomResponse(ModelStateDictionary modelState)
        {
            var erros = modelState.Values.SelectMany(e => e.Errors);
            foreach (var erro in erros)
            {
                AdicionarErroProcessamento(erro.ErrorMessage);
            }
            return CustomResponse();
        }

        protected IActionResult CustomResponse(ValidationResult result)
        {
            foreach (var erro in result.Errors)
            {
                AdicionarErroProcessamento(erro.ErrorMessage);
            }
            return CustomResponse();
        }

        protected bool OperacaoValida()
        {
            return !Erros.Any();
        }
        protected bool PossuiErrosResponse<T>(ResponseResult<T> defaultResponse)
        {
            return PossuiErrosResponse(defaultResponse);
        }

        protected bool PossuiErrosResponse(ResponseResult defaultResponse)
        {
            if (defaultResponse.Errors == null) return false;

            foreach (var mensagem in defaultResponse.Errors)
            {
                AdicionarErroProcessamento(mensagem);
            }

            return true;
        }

        protected void AdicionarErroProcessamento(string erro)
        {
            Erros.Add(erro);
        }
        protected void LimparErrosProcessamento()
        {
            Erros.Clear();
        }
    }
}
