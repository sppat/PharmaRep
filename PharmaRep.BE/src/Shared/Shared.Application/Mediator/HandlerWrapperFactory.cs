using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Shared.Application.Results;

namespace Shared.Application.Mediator;

public class HandlerWrapperFactory
{
    internal Dispatcher.HandlerDecoratorDelegate CreateHandlerWrapper<TResponse>(IRequest<TResponse> request)
    {
        var handlerDecoratorMethodInfo = typeof(HandlerWrapperFactory)
            .GetMethod(nameof(HandlerDecorator), BindingFlags.NonPublic | BindingFlags.Static)!
            .MakeGenericMethod(request.GetType(), typeof(TResponse));
        
        return (Dispatcher.HandlerDecoratorDelegate)Delegate.CreateDelegate(typeof(Dispatcher.HandlerDecoratorDelegate), handlerDecoratorMethodInfo);
    }

    private static async Task<object?> HandlerDecorator<TRequest, TResponse>(
        IServiceProvider serviceProvider, 
        object request,
        CancellationToken cancellationToken) where TRequest : IRequest<TResponse>
    {
        var handler = serviceProvider.GetRequiredService<IRequestHandler<TRequest, TResponse>>();
        
        return await handler.HandleAsync((TRequest)request, cancellationToken);
    }
}