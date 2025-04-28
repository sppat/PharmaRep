namespace Shared.Application.Results;

public class Result
{
    public IEnumerable<string> Errors { get; init; }
    public ResultType Type { get; init; }
    public bool IsSuccess => !Errors.Any();
    
    protected Result(ResultType type)
    {
        Type = type;
        Errors = [];
    }

    protected Result(ICollection<string> errors, ResultType type)
    {
        ArgumentNullException.ThrowIfNull(errors);
        if (errors.Count is 0)
        {
            throw new ArgumentException("Errors must not be empty in failure result");
        }

        Errors = errors;
        Type = type;
    }
    
    public static Result Success(ResultType type = ResultType.Success) => new(type);
    public static Result Failure(ICollection<string> errors, ResultType type) => new(errors, type);
}

public class Result<T> : Result
{
    public T Value { get; init; }

    private Result(T value, ResultType type) : base(type)
    {
        ArgumentNullException.ThrowIfNull(value);
        
        Value = value;
    }

    private Result(ICollection<string> errors, ResultType type) : base(errors, type)
    {
        Value = default;
    }

    public static Result<T> Success(T value, ResultType type = ResultType.Success) => new(value, type);
    public new static Result<T> Failure(ICollection<string> errors, ResultType type) => new(errors, type);
}

public enum ResultType
{
    ValidationError,
    Created,
    NotFound,
    Success,
    Updated,
    BadRequest,
    ServerError
}