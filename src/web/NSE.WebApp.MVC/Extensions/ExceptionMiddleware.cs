using NSE.WebApp.MVC.Services;
using Polly.CircuitBreaker;
using Refit;
using System.Net;

namespace NSE.WebApp.MVC.Extensions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private static IAutenticacaoService _autenticacaoService;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, IAutenticacaoService autenticacaoService)
        {
            _autenticacaoService = autenticacaoService;

            try
            {
                await _next(httpContext);
            }
            catch (CustomHttpRequestException exception)
            {
                HandleRequestException(httpContext, exception.StatusCode);
            }
            catch (ValidationApiException exception)
            {
                HandleRequestException(httpContext, exception.StatusCode);
            }
            catch (ApiException exception)
            {
                HandleRequestException(httpContext, exception.StatusCode);
            }
            catch (BrokenCircuitException)
            {
                HandleBrokenCircuitException(httpContext);
            }
        }

        private void HandleRequestException(HttpContext context, HttpStatusCode statusCode)
        {
            if (statusCode == HttpStatusCode.Unauthorized)
            {
                if (_autenticacaoService.TokenExpirado())
                {
                    if (_autenticacaoService.RefreshTokenValido().Result)
                    {
                        context.Response.Redirect(context.Request.Path);
                        return;
                    }
                }

                _autenticacaoService.Logout();
                context.Response.Redirect($"/login?ReturnUrl={context.Request.Path}");
                return;
            }

            context.Response.StatusCode = (int)statusCode;
        }

        private void HandleBrokenCircuitException(HttpContext context)
        {
            context.Response.Redirect("/sistema-indisponivel");
        }
    }
}
