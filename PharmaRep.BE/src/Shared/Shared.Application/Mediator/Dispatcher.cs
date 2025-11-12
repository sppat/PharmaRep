namespace Shared.Application.Mediator;

public class Dispatcher(IServiceProvider serviceProvider) : IDispatcher
{
    public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        var handlerInterface = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));
        var handler = serviceProvider.GetService(handlerInterface);
        if (handler is null) throw new InvalidOperationException("Could not create handler instance.");

        return await (Task<TResponse>)((dynamic)handler).HandleAsync((dynamic)request, cancellationToken);
    }

    public Task SendAsync(IRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}