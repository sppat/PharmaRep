using Bootstrapper;
using Identity.WebApi;
using Shared.Application;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddMediator();
builder.Services.AddIdentityWebApi(builder.Configuration);

var app = builder.Build();

app.UseHttpsRedirection();

app.UseExceptionHandler();

var scope = app.Services.CreateScope();
await app.UseIdentityMiddleware(scope, app.Environment.IsDevelopment());

app.Run();