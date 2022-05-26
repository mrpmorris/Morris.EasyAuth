using System.Net.Http;

namespace Morris.BasicAuth.Blazor.Web.Client;

internal class NonRedirectingHttpClient : HttpClient
{
	internal const string HttpClientId = "Morris.BasicAuth.Blazor.Web.Client.AuthHttpClient";

	public NonRedirectingHttpClient() : base(CreateHandler())
	{
	}

	public static HttpMessageHandler CreateHandler()
	{
		return new HttpClientHandler { AllowAutoRedirect = false };
	}
}
