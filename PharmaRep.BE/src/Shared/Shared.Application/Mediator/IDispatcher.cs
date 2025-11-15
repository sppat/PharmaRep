namespace Shared.Application.Mediator;

public interface IDispatcher
{
    Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
}