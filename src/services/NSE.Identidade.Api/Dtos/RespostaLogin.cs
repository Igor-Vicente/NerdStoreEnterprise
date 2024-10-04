namespace NSE.Identidade.Api.Dtos
{
    public class RespostaLogin
    {
        public string AccessToken { get; set; }
        public double ExpiresIn { get; set; }
        public string ExpiresPeriod { get; set; }
        public UsuarioToken UsuarioToken { get; set; }
    }

    public class UsuarioToken
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public IEnumerable<UsuarioClaim> Claims { get; set; }
    }

    public class UsuarioClaim
    {
        public string Value { get; set; }
        public string Type { get; set; }
    }
}
