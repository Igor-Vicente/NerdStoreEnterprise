using NSE.WebApp.MVC.Models;

namespace NSE.WebApp.MVC.Services
{
    public interface IAutenticacaoService
    {
        Task<DefaultResponseVM<UsuarioResponstaLoginVM>> Login(UsuarioLoginVM loginVM);
        Task<DefaultResponseVM<UsuarioResponstaLoginVM>> Register(UsuarioRegistroVM usuarioRegistroVM);
    }
}
