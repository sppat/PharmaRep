using System.Net.Http.Headers;

using Microsoft.JSInterop;

namespace PharmaRep.Admin.Utils;

public class BearerTokenHandler(IJSRuntime jSRuntime) : DelegatingHandler
{
	private const string AuthType = "Bearer";

	protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		var token = await jSRuntime.InvokeAsync<string>(Constants.JSConstants.GetItemFunction, Constants.AuthConstants.AuthTokenKey);

		if (!string.IsNullOrEmpty(token))
		{
			request.Headers.Authorization = new AuthenticationHeaderValue(AuthType, token);
		}

		return await base.SendAsync(request, cancellationToken);
	}
}
