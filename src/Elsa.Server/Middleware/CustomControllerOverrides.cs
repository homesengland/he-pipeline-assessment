using System.Globalization;

namespace Elsa.Server.Middleware
{
    public class ControllerRoutingMiddleware
    {
        private readonly RequestDelegate _next;

        public ControllerRoutingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.ToString().Contains("/history"))
            {
                PathString newRoute = new PathString(context.Request.Path.ToString().Replace("history", "customHistory"));
                context.Request.Path = newRoute;
                context.Request.RouteValues.Remove("controller");
                context.Request.RouteValues.Add("controller", "CustomHistory");
                context.Response.Redirect(newRoute);
                return;
            }

            if (context.Request.Path.ToString().EndsWith("/workflow-definitions") && context.Request.Method == "GET")
            {
                PathString newRoute = new PathString(context.Request.Path.ToString().Replace("workflow-definitions", "custom-workflow-definitions"));
                context.Request.Path = newRoute;
                context.Request.RouteValues.Remove("controller");
                context.Request.RouteValues.Add("controller", "CustomList");
                context.Response.Redirect(newRoute + context.Request.QueryString);
                return;
            }

            // Call the next delegate/middleware in the pipeline.
            await _next(context);
        }
    }

    public static class ControllerRoutingMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomControllerOverrides(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ControllerRoutingMiddleware>();
        }
    }
}