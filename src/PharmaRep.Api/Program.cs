using PharmaRep.Api.Endpoints;
using PharmaRep.Application;
using PharmaRep.Infrastructure;
using PharmaRep.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddApplication(builder.Configuration)
    .AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAccountEndpoints();

AppDbContext.ApplyMigrations(app);

app.Run();
