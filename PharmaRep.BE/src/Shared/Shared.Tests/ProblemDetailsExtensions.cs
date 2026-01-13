using System.Text.Json;

using Microsoft.AspNetCore.Mvc;

namespace Shared.Tests;

public static class ProblemDetailsExtensions
{
	public static IEnumerable<string> GetErrors(this ProblemDetails problemDetails)
	{
		var errorsExist = problemDetails.Extensions.TryGetValue("errors", out var errors);
		if (!errorsExist)
		{
			throw new ArgumentException("The errors property does not exist in the ProblemDetails object.");
		}

		var errorsString = JsonSerializer.Serialize(errors);
		var errorsList = JsonSerializer.Deserialize<IEnumerable<string>>(errorsString);

		return errorsList;
	}
}
