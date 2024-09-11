using Polly.CircuitBreaker;
using Refit;
using System.Net;

namespace NSE.WebApp.MVC.Extensions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
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
