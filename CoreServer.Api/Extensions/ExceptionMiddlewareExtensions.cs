using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace CoreServer.Api.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigurationExceptionHanlder(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                    {
                        context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                        context.Response.ContentType = "application/json";

                        var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                        if (contextFeature != null)
                        {
                            await context.Response.WriteAsync("对不起，服务器发生了错误。");
                        }
                    });
            });
        }
    }
}