namespace Shared.Application.Validation;

public interface IValidator<in T>
{
    Task<ValidationResult> ValidateAsync(T request, CancellationToken cancellationToken);
}