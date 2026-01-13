namespace Shared.Application.Validation;

public class ValidationResult
{
	public ICollection<string> Errors { get; }
	public bool IsValid => Errors is null || Errors.Count == 0;

	private ValidationResult()
	{
		Errors = [];
	}

	private ValidationResult(ICollection<string> errors)
	{
		Errors = errors;
	}

	public static ValidationResult Valid => new();
	public static ValidationResult Failure(ICollection<string> errors) => new(errors);
}
