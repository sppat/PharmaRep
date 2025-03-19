using Identity.WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentityWebApi(builder.Configuration);

var app = builder.Build();

app.UseHttpsRedirection();

await app.UseIdentityMiddleware(app.Environment.IsDevelopment());

app.Run();