using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetDevPack.Security.Jwt.Core.Jwa;
using NSE.Identidade.Api.Data;
using NSE.Identidade.Api.Extensions;

namespace NSE.Identidade.Api.Configuration
{
    public static class IdentityConfig
    {
        public static IServiceCollection ConfigureIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddJwksManager(options => options.Jws = Algorithm.Create(DigitalSignaturesAlgorithm.EcdsaSha256))
              .PersistKeysToDatabaseStore<AppDbContext>()
              .UseJwtValidation();

            services.AddDbContext<AppDbContext>(ctxOptsBuilder =>
               ctxOptsBuilder.UseSqlServer(configuration.GetConnectionString("Default")));

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()//trabalhar com ORM EntityFramework no <context>
                .AddRoles<IdentityRole>()
                .AddErrorDescriber<IdentityResponsePtBr>();

            services.AddMemoryCache();

            return services;
        }
    }
}
