using NSE.Clientes.Api.Configuration;
using System.Reflection;

namespace NSE.Clientes.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Definindo appSettings.json
            //userSecrets tem preferência sobre qualquer appsettings.json
            builder.Configuration
                .SetBasePath(builder.Environment.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
                .AddEnvironmentVariables()
                .AddUserSecrets(Assembly.GetExecutingAssembly(), true);
            #endregion

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddApiConfiguration(builder.Configuration);

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("Development");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
