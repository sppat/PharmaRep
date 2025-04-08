using FluentValidation;
using Shared.Application.Results;

namespace Shared.Application.Mediator.Pipeline;

public class ValidationMiddleware<TRequest, TResponse>(IValidator<TRequest> validator) : IDispatcherMiddleware<TRequest, TResponse>
{
    public async Task<TResponse> HandleAsync(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        
        if (validationResult.IsValid) return await next(cancellationToken);
        
        var validationErrors = validationResult.Errors
            .Select(e => e.ErrorMessage)
            .ToList();

        var genericRequestType = request.GetType().GetInterfaces().FirstOrDefault(i => i.IsGenericType && typeof(IRequest<>) == i.GetGenericTypeDefinition());
        if (genericRequestType is not null)
        {
            var responseTypeGenericArgument = typeof(TResponse).GetGenericArguments()[0];
            var genericResponseType = typeof(Result<>).MakeGenericType(responseTypeGenericArgument);
            var genericFailureMethod = genericResponseType.GetMethod("Failure");
            
            return (TResponse)genericFailureMethod?.Invoke(null, [validationErrors, ResultType.ValidationError]);
        }
        
        var failureMethod = typeof(Result).GetMethod("Failure");

        return (TResponse)failureMethod?.Invoke(null, [validationErrors, ResultType.ValidationError]);
    }
}