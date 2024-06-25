using NSE.Identidade.Api.Configuration;
using NSE.WebApi.Core.Identidade;
using System.Reflection;

namespace NSE.Identidade.Api
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

            builder.Services.ConfigureIdentity(builder.Configuration);
            builder.Services.AddCors(op =>
            {
                op.AddPolicy("Development", o =>
                {
                    o.AllowAnyMethod();
                    o.AllowAnyHeader();
                    o.AllowAnyOrigin();
                });
            });

            builder.Services.ConfigureApi();
            builder.Services.ConfigureSwagger();

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseCors("Development");
            app.UseHsts();
            app.UseHttpsRedirection();
            app.UseAuthConfiguration();
            app.MapControllers();
            app.Run();
        }
    }
}
