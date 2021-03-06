using Application.Common.FileStorage;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;

namespace Infrastructure.FileStorage;

internal static class Startup
{
    internal static IServiceCollection AddFileStorageService(this IServiceCollection services)
    {
        return services.AddTransient<IFileStorageService, LocalFileStorageService>();
    }
    internal static IApplicationBuilder UseFileStorage(this IApplicationBuilder app)
    {
        string path = "Files";
        CreateBaseDirectory(path);

        return app.UseStaticFiles(new StaticFileOptions()
        {
            FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), path)),
            RequestPath = new PathString("/Files"),
            // To make Files Unaccesible if not Loged In
            /*
            OnPrepareResponse = ctx =>
            {
                if (ctx.Context.User.Identity is null || !ctx.Context.User.Identity.IsAuthenticated)
                {
                    // respond HTTP 401 Unauthorized, and...
                    ctx.Context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

                    // Append following 2 lines to drop body from static files middleware!
                    ctx.Context.Response.ContentLength = 0;
                    ctx.Context.Response.Body = Stream.Null;
                }
            }
            */
        });
    }

    private static void CreateBaseDirectory(string relativePath)
    {

        var dir = Path.GetFullPath(relativePath);
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
    }
}