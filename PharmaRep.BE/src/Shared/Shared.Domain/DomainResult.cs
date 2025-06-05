namespace Shared.Domain;

public class DomainResult<T> where T : class
{
    public T? Value { get; }
    public string ErrorMessage { get; }
    public bool IsSuccess => string.IsNullOrEmpty(ErrorMessage);

    private DomainResult(T value)
    {
        ArgumentNullException.ThrowIfNull(value);
        
        Value = value;
        ErrorMessage = string.Empty;
    }
    
    private DomainResult(string errorMessage)
    {
        Value = null;
        ErrorMessage = errorMessage;
    }
    
    public static implicit operator DomainResult<T>(T value) => new(value: value);
    public static implicit operator DomainResult<T>(string errorMessage) => new(errorMessage: errorMessage);
}