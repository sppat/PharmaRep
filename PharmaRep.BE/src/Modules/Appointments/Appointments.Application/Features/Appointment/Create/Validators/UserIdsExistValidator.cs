using Identity.Public.Features.UsersExist;

using Shared.Application.Mediator;
using Shared.Application.Validation;

namespace Appointments.Application.Features.Appointment.Create.Validators;

public class UserIdsExistValidator(IDispatcher dispatcher) : IValidator<CreateAppointmentCommand>
{
	public async Task<ValidationResult> ValidateAsync(CreateAppointmentCommand request, CancellationToken cancellationToken)
	{
		List<Guid> userIdsToSearch = [request.OrganizerId, .. request.AttendeeIds];

		var query = new UsersExistQuery(userIdsToSearch);
		var queryResult = await dispatcher.SendAsync(query, cancellationToken);

		return queryResult.IsSuccess
			? ValidationResult.Valid
			: ValidationResult.Failure(queryResult.Errors.ToList());
	}
}
