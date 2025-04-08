using Microsoft.Extensions.DependencyInjection;

namespace Shared.Application.Mediator;

public abstract class RequestHandlerDecorator<TResponse>
{
    public abstract Task<TResponse> HandleAsync(IRequest<TResponse> request, IServiceProvider serviceProvider, CancellationToken cancellationToken);
}

public class RequestHandlerDecoratorImpl<TRequest, TResponse> : RequestHandlerDecorator<TResponse>
{
    public override async Task<TResponse> HandleAsync(IRequest<TResponse> request, IServiceProvider serviceProvider, CancellationToken cancellationToken)
    {
        return await serviceProvider.GetServices<IDispatcherMiddleware<TRequest, TResponse>>()
            .Reverse()
            .Aggregate((RequestHandlerDelegate<TResponse>)Handler, Pipeline)(cancellationToken);
        
        Task<TResponse> Handler(CancellationToken ct) => serviceProvider.GetRequiredService<IRequestHandler<TRequest, TResponse>>()
            .HandleAsync((TRequest)request, ct == CancellationToken.None ? cancellationToken : ct);
        
        RequestHandlerDelegate<TResponse> Pipeline(RequestHandlerDelegate<TResponse> next, IDispatcherMiddleware<TRequest, TResponse> middleware)
            => t => middleware.HandleAsync((TRequest)request, next, t == CancellationToken.None ? cancellationToken : t);   
    }
}