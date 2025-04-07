namespace Shared.Application.Mediator;

public interface IRequestHandler<in TRequest, TResponse>
{
    Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken);
}

public interface IRequestHandler<in TRequest>
{
    Task HandleAsync(TRequest request, CancellationToken cancellationToken);
}