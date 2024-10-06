using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace NSE.WebApi.Core.Identidade
{
    public static class JwtConfig
    {
        public static IServiceCollection AddJwtConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var secretsSection = configuration.GetSection("IdentidadeSecrets");
            services.Configure<IdentidadeSecrets>(secretsSection);
            var identidadeSecrets = secretsSection.Get<IdentidadeSecrets>();

            services.AddAuthentication(authOpts =>
            {
                authOpts.DefaultAuthenticateScheme = "Bearer";
                authOpts.DefaultChallengeScheme = "Bearer";
            }).AddJwtBearer(jwtBearerOpts =>
            {
                jwtBearerOpts.RequireHttpsMetadata = true;
                jwtBearerOpts.SaveToken = true;
                jwtBearerOpts.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(identidadeSecrets!.Secret)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = identidadeSecrets!.Emissor,
                    ValidAudience = identidadeSecrets!.Audiencia,
                };
                jwtBearerOpts.MapInboundClaims = false;
            });

            return services;
        }

        public static void UseAuthConfiguration(this IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
        }
    }
}
