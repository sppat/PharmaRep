using System.Collections.Concurrent;

using Shared.Application.Results;

namespace Shared.Application.Mediator;

public class Dispatcher(
	IServiceProvider serviceProvider,
	HandlerWrapperFactory wrapperFactory) : IDispatcher
{
	internal delegate Task<object> HandlerDecoratorDelegate(IServiceProvider serviceProvider, object request, CancellationToken cancellationToken);

	private readonly ConcurrentDictionary<Type, HandlerDecoratorDelegate> _handlers = new();

	public async Task<Result<T>> SendAsync<T>(IRequest<Result<T>> request, CancellationToken cancellationToken)
	{
		var handlerDecorator = _handlers.GetOrAdd(request.GetType(), _ => wrapperFactory.CreateHandlerWrapper(request));

		return (Result<T>)await handlerDecorator(serviceProvider, request, cancellationToken);
	}
}
