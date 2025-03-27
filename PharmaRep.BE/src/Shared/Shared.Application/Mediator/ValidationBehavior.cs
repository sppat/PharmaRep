using FluentValidation;
using MediatR;
using Shared.Application.Results;

namespace Shared.Application.Mediator;

public class ValidationBehavior<TRequest, TResponse>(IValidator<TRequest> validator) : IPipelineBehavior<TRequest, TResponse> where TResponse : Result
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid)
        {
            return await next();
        }
        
        var errors = validationResult.Errors
            .Select(e => e.ErrorMessage)
            .ToList();
        
        if (typeof(TResponse) == typeof(Result))
        {
            return (TResponse)Result.Failure(errors, ResultType.ValidationError);
        }
        
        var genericArgument = typeof(TResponse).GetGenericArguments().First();
        var genericResult = typeof(Result<>).MakeGenericType(genericArgument)
            .GetMethod(nameof(Result<object>.Failure), [typeof(ICollection<string>), typeof(ResultType)])
            ?.Invoke(null, [errors, ResultType.ValidationError]);

        return (TResponse)genericResult;
    }
}