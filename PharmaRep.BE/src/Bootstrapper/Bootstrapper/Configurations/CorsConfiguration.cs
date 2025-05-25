namespace Bootstrapper.Configurations;

public record CorsConfiguration(IEnumerable<string> AllowedOrigins,
    IEnumerable<string> AllowedMethods,
    IEnumerable<string> AllowedHeaders);