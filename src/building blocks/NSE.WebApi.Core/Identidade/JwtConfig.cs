using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetDevPack.Security.JwtExtensions;

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
                jwtBearerOpts.SetJwksOptions(new JwkOptions(identidadeSecrets.AutenticacaoJwksUrl));
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
