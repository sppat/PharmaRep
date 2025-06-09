using Appointments.Application.Dtos;
using Appointments.Domain.Entities;
using Appointments.WebApi.Mappings;
using Appointments.WebApi.Requests;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Application.Mediator;
using Shared.WebApi.EndpointMappings;
using Shared.WebApi.Responses;

namespace Appointments.WebApi.Endpoints;

public static class AppointmentEndpoints
{
    public static IEndpointRouteBuilder MapAppointmentEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(AppointmentModuleUrls.Appointment.GetAll, GetAllAsync)
            .RequireAuthorization()
            .Produces<PaginatedResponse<AppointmentDto>>()
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithDescription("Retrieves a list of users.")
            .WithTags(nameof(Appointment));
        
        endpoints.MapPost(AppointmentModuleUrls.Appointment.Create, CreateAsync)
            .RequireAuthorization()
            .Produces<Guid>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithDescription("Creates a new appointment.")
            .WithTags(nameof(Appointment));

        return endpoints;
    }
    
    private static async Task<IResult> GetAllAsync(IDispatcher dispatcher, [AsParameters] GetAppointmentsRequest request, CancellationToken cancellationToken)
    {
        var query = request.ToQuery();
        var result = await dispatcher.SendAsync(query, cancellationToken);
        
        return result.ToHttpResult(AppointmentResponseMappings.ToGetAllUsersResponse);
    }

    private static async Task<IResult> CreateAsync(IDispatcher dispatcher, CreateAppointmentRequest request, CancellationToken cancellationToken)
    {
        var command = request.ToCommand();
        var result = await dispatcher.SendAsync(command, cancellationToken);
        
        return result.ToHttpResult(AppointmentResponseMappings.ToResponse, createdAt: "GetByIdAsync");
    } 
}