
using Microsoft.EntityFrameworkCore;
using NSE.Catalogo.Api.Configuration;
using NSE.Catalogo.Api.Data;
using NSE.Catalogo.Api.Data.Repository;
using NSE.Catalogo.Api.Models;
using NSE.WebApi.Core.Identidade;

namespace NSE.Catalogo.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<CatalogoContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

            builder.Services.AddControllers();
            builder.Services.AddJwtConfiguration(builder.Configuration);
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerConfiguration();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("Development", policy =>
                {
                    policy.AllowAnyOrigin();
                    policy.AllowAnyMethod();
                    policy.AllowAnyHeader();
                });
            });

            builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();

            var app = builder.Build();

            app.UseCors("Development");
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseHsts();
            app.UseHttpsRedirection();
            app.UseAuthConfiguration();
            app.MapControllers();

            app.Run();
        }
    }
}
