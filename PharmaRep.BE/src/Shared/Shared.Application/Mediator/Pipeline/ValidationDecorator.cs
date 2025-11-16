using System.Reflection;
using FluentValidation;
using Shared.Application.Results;

namespace Shared.Application.Mediator.Pipeline;

public class ValidationDecorator<TRequest, TResponse>(
    IRequestHandler<TRequest, TResponse> decorated,
    IEnumerable<IValidator<TRequest>> validators) : IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private delegate object FailedResultDelegate(ICollection<string> errors, ResultType resultType);
    
    public async Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken)
    {
        var validationErrors = await GetValidationErrors(request, cancellationToken);

        if (validationErrors.Count is 0) return await decorated.HandleAsync(request, cancellationToken);

        var failedResultDelegate = BuildFailedResultDelegate();

        return (TResponse)failedResultDelegate(validationErrors, ResultType.ValidationError);
    }

    private static FailedResultDelegate BuildFailedResultDelegate()
    {
        var failedResultMethodInfo = typeof(TResponse).GetMethod(
            name: "Failure",
            bindingAttr: BindingFlags.Public | BindingFlags.Static,
            types: [typeof(ICollection<string>), typeof(ResultType)]);
        
        ArgumentNullException.ThrowIfNull(failedResultMethodInfo);
        
        return (FailedResultDelegate)Delegate.CreateDelegate(typeof(FailedResultDelegate), failedResultMethodInfo);
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