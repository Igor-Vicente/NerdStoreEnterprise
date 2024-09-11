using NSE.WebApp.MVC.Extensions;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NSE.WebApp.MVC.Models
{
    public class UsuarioRegistroVM
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DisplayName("Nome Completo")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DisplayName("CPF")]
        [Cpf]
        public string Cpf { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [EmailAddress(ErrorMessage = "O campo {0} está em formato inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 8)]
        public string Senha { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 8)]
        [Compare("Senha", ErrorMessage = "Senhas não correspondem")]
        public string SenhaConfirmacao { get; set; }
    }
    public class UsuarioLoginVM
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [EmailAddress(ErrorMessage = "O campo {0} está em formato inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 8)]
        public string Senha { get; set; }
    }

    public class UsuarioResponstaLoginVM
    {
        public string AccessToken { get; set; }
        public double ExpiresIn { get; set; }
        public string ExpiresPeriod { get; set; }
        public UsuarioTokenVM UsuarioToken { get; set; }
    }

    public class UsuarioTokenVM
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public IEnumerable<UsuarioClaimVM> Claims { get; set; }
    }

    public class UsuarioClaimVM
    {
        public string Value { get; set; }
        public string Type { get; set; }
    }
}
