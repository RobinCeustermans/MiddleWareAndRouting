using RoutingMiddleWareProject.CustomMiddleWare;
using RoutingMiddleWareProject.HelperClasses;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//enable routing
app.UseRouting();


//use something like postman
//in body: ?operation=multiply&num1=5&num2=6
app.UseWhen(
    context => context.Request.Method == "POST",
    app =>
    {
        app.UseCalculateOperationsCustomMiddleware();
    }
);

// -> /sum/6/5
app.MapGet("/sum/{num1:int}/{num2:int}", async context =>
{
    if (!context.Request.RouteValues.ContainsKey("num1") || !context.Request.RouteValues.ContainsKey("num2"))
    {
        context.Response.StatusCode = 400;
        await context.Response.WriteAsync("Invalid numbers");
    }

    var result = Sum.CalculateObject(context.Request.RouteValues["num1"], context.Request.RouteValues["num2"]);
    if (result != null)
        await context.Response.WriteAsync($"Result of the sum is: {result}");
    else
        await context.Response.WriteAsync($"Could not calculate the sum");
});

app.MapGet("/subtract/{num1:int}/{num2:int}", async (HttpContext context, int num1, int num2) =>
{
    if (!context.Request.RouteValues.ContainsKey("num1") || !context.Request.RouteValues.ContainsKey("num2"))
    {
        context.Response.StatusCode = 400;
        await context.Response.WriteAsync("Invalid numbers");
    }
    var result = Difference.CalculateObject(context.Request.RouteValues["num1"], context.Request.RouteValues["num2"]);
    if (result != null)
        await context.Response.WriteAsync($"Difference is: {result}");
    else
        await context.Response.WriteAsync($"Could not calculate the difference");
});

////this is the default route, this causes the other routes to skip and this will execute instead
///use app.UseEndpoints if you want to use this

//app.Run(async context =>
//{
//    await context.Response.WriteAsync("No response");
//});


////if you want a default, use this
app.MapFallback(async context =>
{
    await context.Response.WriteAsync("No response");
});

app.Run();
