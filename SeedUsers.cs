using System.Net.Http.Json;
using System.Text.Json.Serialization;

var registerRequests = new RegisterRequest[]
{
    new(FirstName: "Alice",
        LastName: "Johnson",
        Email: "alice@johnson.com",
        Password: "P@ssw0rd"),
    new(FirstName: "John",
        LastName: "Doe",
        Email: "john@doe.com",
        Password: "P@ssw0rd"),
    new(FirstName: "Bob",
        LastName: "Marley",
        Email: "bob@marley.com",
        Password: "P@ssw0rd"),
    new(FirstName: "Michael",
        LastName: "Jordan",
        Email: "michael@jordan.com",
        Password: "P@ssw0rd"),
    new(FirstName: "Jack",
        LastName: "Daniels",
        Email: "jack@daniels.com",
        Password: "P@ssw0rd"),
};

var responseTaskList = new List<Task<HttpResponseMessage>>();
using var httpClient = new HttpClient();
foreach (var registerRequest in registerRequests)
{
    var response = httpClient.PostAsJsonAsync("http://localhost:5000/api/identity/auth/register", 
        registerRequest, 
        RegisterRequestJsonContext.Default.RegisterRequest);

    responseTaskList.Add(response);
}

try
{
    await Task.WhenAll(responseTaskList);
}
catch (HttpRequestException ex)
{
    Console.WriteLine($"An error occurred during user registration: {ex.Message}");
}

Console.WriteLine("=======================================");
Console.WriteLine("Finished processing user registrations.");

[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(RegisterRequest))]
public partial class RegisterRequestJsonContext : JsonSerializerContext {}

public record RegisterRequest(string FirstName, string LastName, string Email, string Password);