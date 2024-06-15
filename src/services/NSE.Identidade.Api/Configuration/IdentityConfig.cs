using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NSE.Identidade.Api.Data;
using NSE.Identidade.Api.Extensions;
using System.Text;

namespace NSE.Identidade.Api.Configuration
{
    public static class IdentityConfig
    {
        public static IServiceCollection ConfigureIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(ctxOptsBuilder =>
               ctxOptsBuilder.UseSqlServer(configuration.GetConnectionString("Default")));

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()//trabalhar com ORM EntityFramework no <context>
                .AddRoles<IdentityRole>()
                .AddErrorDescriber<IdentityResponsePtBr>();

            #region Jwt Bearer Authentication
            var identidadeSecretsSection = configuration.GetSection("IdentidadeSecrets");

            services.Configure<IdentidadeSecrets>(identidadeSecretsSection);
            var identidadeSecrets = identidadeSecretsSection.Get<IdentidadeSecrets>();

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
            });
            #endregion

            return services;
        }
    }
}
