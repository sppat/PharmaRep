namespace Shared.Application.Validation;

public interface IValidationOrchestrator<in TRequest>
{
    Task<ValidationResult> ValidateAsync(TRequest request, CancellationToken cancellationToken);
}