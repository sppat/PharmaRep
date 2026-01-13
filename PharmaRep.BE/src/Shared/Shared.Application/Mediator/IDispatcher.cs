using Shared.Application.Results;

namespace Shared.Application.Mediator;

public interface IDispatcher
{
	Task<Result<T>> SendAsync<T>(IRequest<Result<T>> request, CancellationToken cancellationToken = default);
}
