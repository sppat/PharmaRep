namespace Shared.Application.Results;

public class Result<T>
{
	public T? Value { get; }
	public IEnumerable<string> Errors { get; } = [];
	public ResultType Type { get; }
	public bool IsSuccess => !Errors.Any();

	private Result(T? value, ResultType type)
	{
		ArgumentNullException.ThrowIfNull(value);

		Value = value;
		Type = type;
	}

	private Result(ICollection<string> errors, ResultType type)
	{
		ArgumentNullException.ThrowIfNull(errors);
		if (errors.Count == 0)
		{
			throw new ArgumentException($"{nameof(errors)} is empty");
		}

		Value = default;
		Type = type;
		Errors = errors;
	}

	public static Result<T> Success(T value, ResultType type = ResultType.Success) => new(value, type);
	public static Result<T> Failure(ICollection<string> errors, ResultType type) => new(errors, type);
}

public enum ResultType
{
	ValidationError,
	Created,
	NotFound,
	Success,
	Updated,
	ServerError,
	Deleted
}
