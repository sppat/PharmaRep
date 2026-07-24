using Appointments.WebApi;
using Bootstrapper;
using Bootstrapper.Configurations;
using Identity.WebApi;
using Scalar.AspNetCore;
using Shared.Application;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
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
	app.MapOpenApi();
	app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseExceptionHandler();

app.UseCors("PharmaRepClientPolicy");

await app.UseIdentityMiddleware();
await app.UseAppointmentMiddleware();

app.Run();

public partial class Program { }
