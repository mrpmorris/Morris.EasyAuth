using System.Net.Http;

namespace Morris.BasicAuth.Blazor.Web.Client;

public class AuthHttpClient : HttpClient
{
	internal const string HttpClientId = "Morris.BasicAuth.Blazor.Web.Client.AuthHttpClient";

	public AuthHttpClient() : base(CreateHandler())
	{
	}

	public static HttpMessageHandler CreateHandler()
	{
		return new HttpClientHandler { AllowAutoRedirect = false };
	}
}
