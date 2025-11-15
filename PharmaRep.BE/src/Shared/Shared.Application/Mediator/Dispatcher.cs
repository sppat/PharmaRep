using System.Collections.Concurrent;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Application.Mediator;

public class Dispatcher(IServiceProvider serviceProvider) : IDispatcher
{
    private delegate Task<object> HandlerDecoratorDelegate(IServiceProvider serviceProvider, object request, CancellationToken cancellationToken);
    
    private readonly ConcurrentDictionary<Type, HandlerDecoratorDelegate> _handlers = new();
    
    public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken)
    {
        var handlerDecorator = _handlers.GetOrAdd(request.GetType(), type => GetDecorator(request));
        
        return (TResponse)await handlerDecorator(serviceProvider, request, cancellationToken);
    }

    private static HandlerDecoratorDelegate GetDecorator<TResponse>(IRequest<TResponse> request)
    {
        var handlerDecoratorMethodInfo = typeof(Dispatcher)
            .GetMethod(nameof(HandlerDecorator), BindingFlags.NonPublic | BindingFlags.Static)!
            .MakeGenericMethod(request.GetType(), typeof(TResponse));
        
        return (HandlerDecoratorDelegate)Delegate.CreateDelegate(typeof(HandlerDecoratorDelegate), handlerDecoratorMethodInfo);
    }

    private static async Task<object> HandlerDecorator<TRequest, TResponse>(
        IServiceProvider serviceProvider, 
        object request,
        CancellationToken cancellationToken) where TRequest : IRequest<TResponse>
    {
        var handler = serviceProvider.GetRequiredService<IRequestHandler<TRequest, TResponse>>();
        
        return await handler.HandleAsync((TRequest)request, cancellationToken);
    }
}