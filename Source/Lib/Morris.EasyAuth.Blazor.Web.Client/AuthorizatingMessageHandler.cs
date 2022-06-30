using Microsoft.AspNetCore.Components;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Morris.EasyAuth.Blazor.Web.Client;

public class AuthorizingMessageHandler : DelegatingHandler
{
	private readonly NavigationManager NavigationManager;
	private readonly ClientAuthOptions AuthOptions;

	public AuthorizingMessageHandler(
		NavigationManager navigationManager,
		ClientAuthOptions authOptions)
	{
		NavigationManager = navigationManager ?? throw new ArgumentNullException(nameof(navigationManager));
		AuthOptions = authOptions ?? throw new ArgumentNullException(nameof(authOptions));
	}

	protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		HttpResponseMessage response = await base.SendAsync(request, cancellationToken);
		// Blazor WASM returns a zero StatusCode from JS if it performs a redirect
		if (response.StatusCode != System.Net.HttpStatusCode.Unauthorized && response.StatusCode != 0)
			return response;

		string returnUrl = HttpUtility.UrlEncode(NavigationManager.Uri);
		string url = string.Format(AuthOptions.SignInUrlTemplate, returnUrl);
		NavigationManager.NavigateTo(url, forceLoad: true);
		await Task.Delay(30_000); // This just prevents us from returning a result to the requester
		return response;
	}
}