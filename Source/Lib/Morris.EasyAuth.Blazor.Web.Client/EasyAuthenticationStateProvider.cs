using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace Morris.EasyAuth.Blazor.Web.Client;

internal class EasyAuthenticationStateProvider : AuthenticationStateProvider
{
	private static readonly AuthenticationState Anonymous;
	private readonly ClientAuthOptions AuthOptions;
	private readonly IPrincipalDeserializer PrincipalDeserializer;
	private readonly IHttpClientFactory HttpClientFactory;

	static EasyAuthenticationStateProvider()
	{
		var anonymousIdentity = new ClaimsIdentity();
		var anonymousUser = new ClaimsPrincipal(anonymousIdentity);
		Anonymous = new AuthenticationState(anonymousUser);
	}

	public EasyAuthenticationStateProvider(
		ClientAuthOptions authOptions,
		IPrincipalDeserializer principalDeserializer,
		IHttpClientFactory httpClientFactory)
	{
		AuthOptions = authOptions ?? throw new ArgumentNullException(nameof(authOptions));
		PrincipalDeserializer = principalDeserializer ?? throw new ArgumentNullException(nameof(principalDeserializer));
		HttpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
	}

	public override async Task<AuthenticationState> GetAuthenticationStateAsync()
	{
		var httpClient = HttpClientFactory.CreateClient(NonRedirectingHttpClient.HttpClientId);
		var request = new HttpRequestMessage(HttpMethod.Get, AuthOptions.GetUserIdentityUrl);
		var response = await httpClient.SendAsync(request);

		string responseType = response.Content.Headers.ContentType?.MediaType ?? "";
		bool isJsonResponse = string.Equals(responseType, "application/json", StringComparison.OrdinalIgnoreCase);
		if (response.StatusCode != System.Net.HttpStatusCode.OK || !isJsonResponse)
			return Anonymous;
		
		try
		{
			string body = await response.Content.ReadAsStringAsync();
			ClaimsPrincipal principal = PrincipalDeserializer.Deserialize(body);
			return new AuthenticationState(principal);
		}
		catch (JsonException)
		{
			return Anonymous;
		}
	}
}
