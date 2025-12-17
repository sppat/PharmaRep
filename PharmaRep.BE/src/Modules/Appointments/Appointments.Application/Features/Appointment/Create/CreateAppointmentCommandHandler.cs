using Appointments.Application.Abstractions;
using Shared.Application.Mediator;
using Shared.Application.Results;
using Shared.Application.Validation;

namespace Appointments.Application.Features.Appointment.Create;

public class CreateAppointmentCommandHandler(IValidationOrchestrator<CreateAppointmentCommand> validator,
    IAppointmentRepository appointmentRepository, 
    IAppointmentUnitOfWork unitOfWork) : IRequestHandler<CreateAppointmentCommand, Result<Guid>>
{
    public async Task<Result<Guid>> HandleAsync(CreateAppointmentCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Result<Guid>.Failure(validationResult.Errors, ResultType.ValidationError);
        }
        
        var appointment = Domain.Entities.Appointment.Create(startDate: request.StartDate,
            endDate: request.EndDate,
            street: request.Street,
            number: request.Number,
            zipCode: request.ZipCode,
            organizerId: request.OrganizerId,
            attendeeIds: request.AttendeeIds);
        
        await appointmentRepository.AddAsync(appointment, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result<Guid>.Success(appointment.Id.Value, ResultType.Created);
    }
}