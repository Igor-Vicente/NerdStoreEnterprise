using NSE.Catalogo.Api.Configuration;
using NSE.WebApi.Core.Identidade;

namespace NSE.Catalogo.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddApiConfiguration(builder.Configuration);
            builder.Services.AddJwtConfiguration(builder.Configuration);
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerConfiguration();

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
