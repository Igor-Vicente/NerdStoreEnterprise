using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NSE.Identidade.Api.Data;
using NSE.Identidade.Api.Extensions;
using NSE.WebApi.Core.Identidade;

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

            services.AddJwtConfiguration(configuration);

            return services;
        }
    }
}
