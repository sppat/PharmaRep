namespace Shared.Application.Mediator;

public class Dispatcher(IServiceProvider serviceProvider) : IDispatcher
{
    public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        var handlerDecoratorType = typeof(RequestHandlerDecoratorImpl<,>).MakeGenericType(request.GetType(), typeof(TResponse));
        var handlerDecorator = (RequestHandlerDecorator<TResponse>)Activator.CreateInstance(handlerDecoratorType);
        if (handlerDecorator is null) throw new InvalidCastException("Could not create handler decorator instance.");

        return await handlerDecorator.HandleAsync(request, serviceProvider, cancellationToken);
    }
}