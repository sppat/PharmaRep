using FluentValidation;
using Shared.Application.Results;

namespace Shared.Application.Mediator.Pipeline;

public class ValidationMiddleware<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IDispatcherMiddleware<TRequest, TResponse>
{
    public async Task<TResponse> HandleAsync(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var validationErrors = await GetValidationErrors(request, cancellationToken);
        if (validationErrors.Count == 0) return await next(cancellationToken); 

        var responseType = typeof(TResponse);
        if (responseType.IsGenericType)
        {
            var genericResponseType = typeof(Result<>).MakeGenericType(responseType.GenericTypeArguments);
            var genericFailureMethod = genericResponseType.GetMethod("Failure");

            return (TResponse)genericFailureMethod?.Invoke(null, [validationErrors, ResultType.ValidationError]);
        }
        
        var failureMethod = typeof(Result).GetMethod("Failure");

        return (TResponse)failureMethod?.Invoke(null, [validationErrors, ResultType.ValidationError]);
    }

    private async Task<ICollection<string>> GetValidationErrors(TRequest request, CancellationToken cancellationToken)
    {
        var validationErrors = new List<string>();
        foreach (var validator in validators)
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (validationResult.IsValid) continue;
            
            var errors = validationResult.Errors
                .Select(e => e.ErrorMessage)
                .ToList();
            validationErrors.AddRange(errors);
        }
        
        return validationErrors;
    }
}