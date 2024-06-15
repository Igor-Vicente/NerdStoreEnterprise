using NSE.WebApp.MVC.Models;

namespace NSE.WebApp.MVC.Services
{
    public class AutenticacaoService : Service, IAutenticacaoService
    {
        private readonly HttpClient _httpClient;

        public AutenticacaoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<DefaultResponseVM<UsuarioResponstaLoginVM>> Login(UsuarioLoginVM loginVM)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/v1/identidade/autenticar", loginVM);

            TratarErrosResponse(response);

            return await DeserializarResponseAsync<UsuarioResponstaLoginVM>(response);
        }

        public async Task<DefaultResponseVM<UsuarioResponstaLoginVM>> Register(UsuarioRegistroVM usuarioRegistroVM)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/v1/identidade/nova-conta", usuarioRegistroVM);

            TratarErrosResponse(response);

            return await DeserializarResponseAsync<UsuarioResponstaLoginVM>(response);
        }
    }
}
