using Shared.Application.Validation;

namespace Appointments.Application.Features;

public class AppointmentValidator<TRequest>(IEnumerable<IValidator<TRequest>> validators) : IValidationOrchestrator<TRequest>
{
	public async Task<ValidationResult> ValidateAsync(TRequest request, CancellationToken cancellationToken)
	{
		var validationErrors = new List<string>();
		foreach (var validator in validators)
		{
			var validationResult = await validator.ValidateAsync(request, cancellationToken);
			if (validationResult.IsValid)
			{
				continue;
			}

			validationErrors.AddRange(validationResult.Errors);
		}

		return validationErrors.Count == 0
			? ValidationResult.Valid
			: ValidationResult.Failure(validationErrors);
	}
}
