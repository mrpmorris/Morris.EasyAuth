using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

namespace Morris.BasicAuth.Blazor.Web.Client;

public class BasicAuthBuilder
{
	private readonly IServiceCollection Services;
	private readonly string BaseAddress;
	private bool ShouldRegisterDefaultHttpClient = true;
	private ClientAuthOptions? Options;
	private Type? PrincipalDeserializerType;

	public BasicAuthBuilder(IServiceCollection services, string baseAddress)
	{
		Services = services ?? throw new ArgumentNullException(nameof(services));
		BaseAddress = baseAddress ?? throw new ArgumentNullException(nameof(baseAddress));
	}

	public BasicAuthBuilder DoNotRegisterDefaltHttpClient()
	{
		ShouldRegisterDefaultHttpClient = false;
		return this;
	}

	public BasicAuthBuilder UseDeserializer<T>()
		where T : class, IPrincipalDeserializer
	{
		PrincipalDeserializerType = typeof(T);
		return this;
	}

	public BasicAuthBuilder WithOptions(ClientAuthOptions options)
	{
		Options = options ?? throw new ArgumentNullException(nameof(options));
		return this;
	}

	internal void Build()
	{
		Services.AddScoped(
			typeof(IPrincipalDeserializer),
			PrincipalDeserializerType ?? typeof(PrincipalJsonDeserializer));

		Services.AddTransient<AuthorizingMessageHandler>();
		Services.AddScoped<AuthenticationStateProvider, BasicAuthenticationStateProvider>();

		ClientAuthOptions options = Options ?? CreateDefaultOptions();
		Services.AddScoped(sp => options!);

		// Client to get user claims
		Services.AddHttpClient<NonRedirectingHttpClient>(
			NonRedirectingHttpClient.HttpClientId, x => x.BaseAddress = new Uri(BaseAddress!));

		if (ShouldRegisterDefaultHttpClient)
			RegisterDefaultHttpClient();
	}

	private ClientAuthOptions CreateDefaultOptions()
	{
		var options = new ClientAuthOptions(
			signInUrlTemplate: null,
			getUserClaimsUrl: null);
		return options;
	}

	private void RegisterDefaultHttpClient()
	{
		Services.AddScoped(sp =>
		{
			var handler = sp.GetRequiredService<AuthorizingMessageHandler>();
			handler.InnerHandler = new HttpClientHandler();
			var result = new HttpClient(handler) { BaseAddress = new Uri(BaseAddress) };
			return result;
		});
		Services
			.AddHttpClient(
				name: "",
				configureClient: client => client.BaseAddress = new Uri(BaseAddress))
			.AddHttpMessageHandler<AuthorizingMessageHandler>();
	}
}
