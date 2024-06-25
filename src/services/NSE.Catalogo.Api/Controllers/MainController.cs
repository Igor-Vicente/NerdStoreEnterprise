using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace NSE.Catalogo.Api.Controllers
{
    [ApiController]
    public abstract class MainController : ControllerBase
    {
        protected ICollection<string> Errors = new List<string>();

        protected IActionResult CustomResponse(object result = null)
        {
            if (!Errors.Any())
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
                    errors = Errors.ToArray()
                });
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

        protected void AdicionarErroProcessamento(string erro)
        {
            Errors.Add(erro);
        }
    }
}
