using Bootstrapper;
using Identity.WebApi;
using Shared.Application;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddDispatcher();
builder.Services.AddIdentityWebApi(builder.Configuration);

var app = builder.Build();

app.UseHttpsRedirection();

app.UseExceptionHandler();

using var scope = app.Services.CreateScope();
await app.UseIdentityMiddleware(scope);

app.Run();

public partial class Program {}