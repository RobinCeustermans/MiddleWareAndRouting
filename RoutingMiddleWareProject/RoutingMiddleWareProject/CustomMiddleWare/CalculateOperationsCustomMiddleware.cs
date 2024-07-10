using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Net.Http;
using System.Threading.Tasks;

namespace RoutingMiddleWareProject.CustomMiddleWare
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class CalculateOperationsCustomMiddleware
    {
        private readonly RequestDelegate _next;

        public CalculateOperationsCustomMiddleware(RequestDelegate next)
        {
            _next = next;
        }


        //use this if the passed route looks like this ?operation=sum&num1=5&num2=6
        public async Task InvokeAsync(HttpContext context)
        {
            StreamReader reader = new StreamReader(context.Request.Body);
            string body = await reader.ReadToEndAsync();

            Dictionary<string, StringValues> queryDict = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(body);

            if (queryDict != null &&
                queryDict.TryGetValue("operation", out var operationValues) &&
                queryDict.TryGetValue("num1", out var num1Values) &&
                queryDict.TryGetValue("num2", out var num2Values) &&
                int.TryParse(num1Values, out int num1) &&
                int.TryParse(num2Values, out int num2))
            {
                var operation = operationValues.ToString().ToLowerInvariant();

                switch (operation)
                {
                    case "multiply":
                        var multiplyResult = num1 * num2;
                        await context.Response.WriteAsync($"Result of multiplication is: {multiplyResult}");
                        break;
                    case "divide":
                        if (num2 != 0)
                        {
                            var divideResult = num1 / num2;
                            await context.Response.WriteAsync($"Result of division is: {divideResult}");
                        }
                        else
                            await context.Response.WriteAsync("Cannot divide by zero.");
                        break;
                    case "modulo":
                        if (num2 != 0)
                        {
                            var moduloResult = num1 % num2;
                            await context.Response.WriteAsync($"Result of modulo is: {moduloResult}");
                        }
                        else
                            await context.Response.WriteAsync("Cannot modulo by zero.");
                        break;
                    default:
                        await context.Response.WriteAsync("Only multiply, divide and modulo are supported in this Middleware.");
                        break;
                }
                return;
            }

            await _next(context);
        }

        //use this if the passed route looks like this /sum/5/6
        //public async Task InvokeAsync(HttpContext context)
        //{
        //    var path = context.Request.Path;
        //    if (path.HasValue)
        //    {
        //        var segments = path.Value.Split('/');
        //        if (segments.Length == 4 && int.TryParse(segments[2], out int num1) && int.TryParse(segments[3], out int num2))
        //        {
        //            var operation = segments[1];
        //            int result = 0;
        //            bool validOperation = true;

        //            switch (operation)
        //            {
        //                case "multiply":
        //                    result = num1 * num2;
        //                    break;
        //                case "divide":
        //                    if (num2 != 0)
        //                    {
        //                        result = num1 / num2;
        //                    }
        //                    else
        //                    {
        //                        await context.Response.WriteAsync("Cannot divide by zero.");
        //                        return;
        //                    }
        //                    break;
        //                case "modulo":
        //                    if (num2 != 0)
        //                    {
        //                        result = num1 % num2;
        //                    }
        //                    else
        //                    {
        //                        await context.Response.WriteAsync("Cannot modulo by zero.");
        //                        return;
        //                    }
        //                    break;
        //                default:
        //                    validOperation = false;
        //                    break;
        //            }

        //            if (validOperation)
        //            {
        //                await context.Response.WriteAsync($"{operation} of {num1} and {num2} is: {result}");
        //            }
        //            else
        //            {
        //                await context.Response.WriteAsync("Invalid operation.");
        //            }
        //            return;
        //        }
        //    }

        //    await _next(context);
        //}
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class CalculateOperationsCustomMiddlewareExtensions
    {
        public static IApplicationBuilder UseCalculateOperationsCustomMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CalculateOperationsCustomMiddleware>();
        }
    }
}
