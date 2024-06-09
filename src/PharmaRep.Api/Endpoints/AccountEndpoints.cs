namespace PharmaRep.Api;

public static class AccountEndpoints
{
    public static void AddAccountEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapPost("/api/account/register", async () => 
        {
            return Results.Ok();
        });
    }
}
