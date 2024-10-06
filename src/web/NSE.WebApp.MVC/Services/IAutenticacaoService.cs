using NSE.WebApp.MVC.Models;

namespace NSE.WebApp.MVC.Services
{
    public interface IAutenticacaoService
    {
        Task<DefaultResponseViewModel<UsuarioResponstaLoginViewModel>> Login(UsuarioLoginViewModel loginVM);
        Task<DefaultResponseViewModel<UsuarioResponstaLoginViewModel>> Register(UsuarioRegistroViewModel usuarioRegistroVM);
    }
}
