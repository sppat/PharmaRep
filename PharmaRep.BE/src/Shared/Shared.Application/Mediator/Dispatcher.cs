using System.Collections.Concurrent;

namespace Shared.Application.Mediator;

public class Dispatcher(
    IServiceProvider serviceProvider,
    HandlerWrapperFactory wrapperFactory) : IDispatcher
{
    internal delegate Task<object> HandlerDecoratorDelegate(IServiceProvider serviceProvider, object request, CancellationToken cancellationToken);
    
    private readonly ConcurrentDictionary<Type, HandlerDecoratorDelegate> _handlers = new();
    
    public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken)
    {
        var handlerDecorator = _handlers.GetOrAdd(request.GetType(), _ => wrapperFactory.CreateHandlerWrapper(request));
        
        return (TResponse)await handlerDecorator(serviceProvider, request, cancellationToken);
    }
}