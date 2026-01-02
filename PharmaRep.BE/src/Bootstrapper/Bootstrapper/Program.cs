using Appointments.WebApi;
using Bootstrapper;
using Bootstrapper.Configurations;
using Identity.WebApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi;
using Shared.Application;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "PharmaRep Web API",
        Version = "v1"
    });
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        In = ParameterLocation.Header,
        Description = "Enter your valid token in the text input below."
    });
    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
    });
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("PharmaRepClientPolicy", corsPolicyBuilder =>
    {
        var corsConfiguration = builder.Configuration.GetSection("Cors").Get<CorsConfiguration>();
        corsPolicyBuilder.WithOrigins(corsConfiguration.AllowedOrigins.ToArray())
            .WithMethods(corsConfiguration.AllowedMethods.ToArray())
            .WithHeaders(corsConfiguration.AllowedHeaders.ToArray());
    });
});
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddDispatcher();
builder.Services.AddIdentityWebApi(builder.Configuration)
    .AddAppointmentsWebApi(builder.Configuration);

var app = builder.Build();

if (!app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseExceptionHandler();

app.UseCors("PharmaRepClientPolicy");

await app.UseIdentityMiddleware();
await app.UseAppointmentMiddleware();

app.Run();

public partial class Program {}