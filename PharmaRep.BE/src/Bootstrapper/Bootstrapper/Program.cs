using Identity.WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentityWebApi(builder.Configuration);

var app = builder.Build();

app.UseHttpsRedirection();

var scope = app.Services.CreateScope();
await app.UseIdentityMiddleware(scope, app.Environment.IsDevelopment());

app.Run();