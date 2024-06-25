using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NSE.Identidade.Api.Dtos;
using NSE.WebApi.Core.Identidade;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NSE.Identidade.Api.Controllers
{
    [Route("api/v1/identidade")]
    public class AuthController : MainController
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IdentidadeSecrets _secrets;

        public AuthController(SignInManager<IdentityUser> signInManager,
                            UserManager<IdentityUser> userManager,
                            IOptions<IdentidadeSecrets> secrets)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _secrets = secrets.Value;
        }

        [HttpPost("nova-conta")]
        public async Task<IActionResult> Registrar(AddUsuarioDto usuarioDto)
        {
            // return new StatusCodeResult(403);

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var usuario = new IdentityUser { Email = usuarioDto.Email, UserName = usuarioDto.Email };
            var result = await _userManager.CreateAsync(usuario, usuarioDto.Senha);

            if (result.Succeeded)
                return CustomResponse(await GerarJwtAsync(usuario.Email));
            foreach (var erro in result.Errors)
            {
                AdicionarErroProcessamento(erro.Description);
            }

            return CustomResponse();
        }

        [HttpPost("autenticar")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var result = await _signInManager.PasswordSignInAsync(loginDto.Email, loginDto.Senha, false, true);
            if (result.Succeeded)
                return CustomResponse(await GerarJwtAsync(loginDto.Email));

            if (result.IsLockedOut)
            {
                AdicionarErroProcessamento("Usuário temporariamente bloaqueado por tentativas inválidas");
                return CustomResponse();
            }

            AdicionarErroProcessamento("Usuário ou Senha inválido");
            return CustomResponse();
        }

        private async Task<RespostaLoginDto> GerarJwtAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var claims = await _userManager.GetClaimsAsync(user);

            var identityClaims = await GerarIdentityClaimsUsuario(user, claims);
            string encodedToken = CodificarToken(identityClaims);

            return GerarRespostaLogin(user, claims, encodedToken);
        }

        private RespostaLoginDto GerarRespostaLogin(IdentityUser? user, IList<Claim> claims, string encodedToken)
        {
            return new RespostaLoginDto
            {
                AccessToken = encodedToken,
                ExpiresIn = TimeSpan.FromHours(_secrets.ExpiracaoHoras).TotalSeconds,
                ExpiresPeriod = "Seconds",
                UsuarioToken = new UsuarioToken
                {
                    Id = user.Id,
                    Email = user.Email,
                    Claims = claims.Select(claim => new UsuarioClaim
                    {
                        Type = claim.Type,
                        Value = claim.Value
                    })
                },
            };
        }

        private string CodificarToken(ClaimsIdentity identityClaims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secrets.Secret);
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _secrets.Emissor,
                Audience = _secrets.Audiencia,
                Subject = identityClaims,
                Expires = DateTime.UtcNow.AddHours(_secrets.ExpiracaoHoras),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });
            return tokenHandler.WriteToken(token);
        }

        private async Task<ClaimsIdentity> GerarIdentityClaimsUsuario(IdentityUser? user, IList<Claim> claims)
        {
            var roles = await _userManager.GetRolesAsync(user);
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id)); //subject - identifies the principal
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())); //jwtId
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString())); //notValidBefore
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));//IssuedAt
            foreach (var role in roles)
            {
                claims.Add(new Claim("role", role));
            }
            return new ClaimsIdentity(claims);
        }

        private static long ToUnixEpochDate(DateTime date)
            => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
    }
}
