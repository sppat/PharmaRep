namespace PharmaRep.Admin.Configurations;

public record ApiClientConfiguration
{
	public const string Section = "ApiClient";

	public string BaseAddress { get; init; } = string.Empty;
}
