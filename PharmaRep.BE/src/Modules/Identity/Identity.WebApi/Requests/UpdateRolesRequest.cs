namespace Identity.WebApi.Requests;

public record UpdateRolesRequest(IEnumerable<string> Roles);
