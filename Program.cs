using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<MongoDBContext>();
builder.Services.AddControllers()
    .ConfigureApplicationPartManager(manager =>
    {
        manager.FeatureProviders.Add(new ControllerFeatureProvider());
    });

var app = builder.Build();

app.UseExceptionHandler("/error");
app.Map("/error", HandleError);

static Task HandleError(HttpContext context)
{
    var error = context.Features.Get<IExceptionHandlerFeature>()?.Error;
    if (error != null)
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        return context.Response.WriteAsync(JsonSerializer.Serialize(new { error = error.Message }));
    }

    return Task.CompletedTask;
}

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
