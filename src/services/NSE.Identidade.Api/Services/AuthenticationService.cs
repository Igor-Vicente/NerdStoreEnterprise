using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NetDevPack.Security.Jwt.Core.Interfaces;
using NSE.Identidade.Api.Data;
using NSE.Identidade.Api.Extensions;
using NSE.Identidade.Api.Models;
using NSE.WebApi.Core.Usuario;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace NSE.Identidade.Api.Services
{
    public class AuthenticationService
    {
        public readonly SignInManager<IdentityUser> SignInManager;
        public readonly UserManager<IdentityUser> UserManager;
        private readonly AppTokenSettings _appTokenSettings;
        private readonly AppDbContext _context;

        private readonly IAspNetUser _aspNetUser;
        private readonly IJwtService _jwtService;

        public AuthenticationService(SignInManager<IdentityUser> signInManager,
                                     UserManager<IdentityUser> userManager,
                                     IOptions<AppTokenSettings> appTokenSettings,
                                     IAspNetUser aspNetUser,
                                     IJwtService jwtService,
                                     AppDbContext context)
        {
            SignInManager = signInManager;
            UserManager = userManager;
            _appTokenSettings = appTokenSettings.Value;
            _aspNetUser = aspNetUser;
            _jwtService = jwtService;
            _context = context;
        }

        public async Task<RespostaLogin> GerarJwtAsync(string email)
        {
            var user = await UserManager.FindByEmailAsync(email);
            var claims = await UserManager.GetClaimsAsync(user);

            var identityClaims = await GerarIdentityClaimsUsuario(user, claims);
            string encodedToken = await CodificarToken(identityClaims);

            var refreshToken = await GerarRefreshToken(email);

            return GerarRespostaLogin(user, claims, encodedToken, refreshToken);
        }

        private RespostaLogin GerarRespostaLogin(IdentityUser? user, IList<Claim> claims, string encodedToken, RefreshToken refreshToken)
        {
            return new RespostaLogin
            {
                AccessToken = encodedToken,
                RefreshToken = refreshToken.Token,
                ExpiresIn = TimeSpan.FromHours(1).TotalSeconds,
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

        private async Task<string> CodificarToken(ClaimsIdentity identityClaims)
        {
            var issuer = $"{_aspNetUser.ObterHttpContext().Request.Scheme}://{_aspNetUser.ObterHttpContext().Request.Host}";
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = await _jwtService.GetCurrentSigningCredentials();
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = issuer,
                Subject = identityClaims,
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = key
            });
            return tokenHandler.WriteToken(token);
        }

        private async Task<ClaimsIdentity> GerarIdentityClaimsUsuario(IdentityUser? user, IList<Claim> claims)
        {
            var roles = await UserManager.GetRolesAsync(user);
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

        private async Task<RefreshToken> GerarRefreshToken(string email)
        {
            var refreshToken = new RefreshToken
            {
                Username = email,
                ExpirationDate = DateTime.UtcNow.AddHours(_appTokenSettings.RefreshTokenExpiration)
            };

            _context.RefreshTokens.RemoveRange(_context.RefreshTokens.Where(u => u.Username == email));
            await _context.RefreshTokens.AddAsync(refreshToken);

            await _context.SaveChangesAsync();

            return refreshToken;
        }

        public async Task<RefreshToken> ObterRefreshToken(Guid refreshToken)
        {
            var token = await _context.RefreshTokens.AsNoTracking()
                .FirstOrDefaultAsync(u => u.Token == refreshToken);

            return token != null && token.ExpirationDate.ToLocalTime() > DateTime.Now
                ? token
                : null;
        }
    }
}
