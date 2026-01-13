namespace PharmaRep.Admin.Configurations;

public sealed class ApiClientConfiguration
{
	public const string Section = "ApiClient";

	public string BaseAddress { get; set; } = string.Empty;
}
