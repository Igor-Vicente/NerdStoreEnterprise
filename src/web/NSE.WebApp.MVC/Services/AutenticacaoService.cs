using NSE.Core.Models;
using NSE.WebApp.MVC.Models;

namespace NSE.WebApp.MVC.Services
{
    public interface IAutenticacaoService
    {
        Task<ResponseResult<UsuarioResponstaLoginViewModel>> Login(UsuarioLoginViewModel loginVM);
        Task<ResponseResult<UsuarioResponstaLoginViewModel>> Register(UsuarioRegistroViewModel usuarioRegistroVM);
    }
    public class AutenticacaoService : Service, IAutenticacaoService
    {
        private readonly HttpClient _httpClient;

        public AutenticacaoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
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
    }
}
