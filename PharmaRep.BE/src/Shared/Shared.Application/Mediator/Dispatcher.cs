using Microsoft.Extensions.DependencyInjection;

namespace Shared.Application.Mediator;

public class Dispatcher(IServiceProvider serviceProvider) : IDispatcher
{
    public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        var genericHandlerType = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));
        
        var handler = serviceProvider.GetRequiredService(genericHandlerType);

        var handleAsync = genericHandlerType.GetMethod(nameof(IRequestHandler<object, object>.HandleAsync));
        if (handleAsync is null) throw new InvalidOperationException("Could not resolve HandleAsync method");

        var resultTask = (Task<TResponse>)handleAsync.Invoke(handler, [request, cancellationToken]);
        if (resultTask is null) throw new InvalidOperationException("Could not invoke HandleAsync method");

        return await resultTask;
    }

    public Task SendAsync(IRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}