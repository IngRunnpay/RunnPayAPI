
using ApiPrincipal.Extensions;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace ApiPrincipal
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.ConfigureContext(builder.Configuration);
            builder.Services.ConfigureServices();
            builder.Services.CongifureAuthentication(builder.Configuration);
            builder.Services.AddAuthorization();
            builder.Services.CongifureSwagger();
            builder.Services.AddControllers();

            var app = builder.Build();

            //if (app.Environment.IsDevelopment() || app.Configuration.GetSection("UseSwagger").Value.ToString() == "1")
            //{

            //}
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.InjectStylesheet("dist/swagger-custom.css");
                c.InjectJavascript("dist/swagger-custom.js");
            });

            app.UseResponseCaching();
            app.UseCors("AllowOrigin");
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Use(async (context, next) =>
            {
                context.Response.OnStarting(state =>
                {
                    var httpContext = (HttpContext)state;
                    httpContext.Response.Headers["Date"] = DateTime.UtcNow.AddHours(-5).ToString("R");
                    return Task.CompletedTask;
                }, context);

                await next();
            });

            //app.UseStaticFiles(new StaticFileOptions
            //{
            //    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "Dist")),
            //    RequestPath = "/swagger/dist"
            //});

            app.Run();

        }

    }
}