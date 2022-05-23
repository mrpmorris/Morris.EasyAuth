using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

namespace Morris.BasicAuth.Blazor.Web.Client;

public class BasicAuthBuilder
{
	private readonly IServiceCollection Services;
	private readonly string? BaseAddress;
	private ClientAuthOptions? Options;
	private Type? PrincipalDeserializerType;
	private bool RedirectToSignInPageAutomatically = true;

	public BasicAuthBuilder(IServiceCollection services, string? baseAddress)
	{
		Services = services ?? throw new ArgumentNullException(nameof(services));
		BaseAddress = baseAddress;
	}

	public BasicAuthBuilder WithOptions(ClientAuthOptions options)
	{
		Options = options ?? throw new ArgumentNullException(nameof(options));
		return this;
	}

	public BasicAuthBuilder DoNotRedirectToSignInPageAutomatically()
	{
		RedirectToSignInPageAutomatically = false;
		return this;
	}

	public BasicAuthBuilder UseDeserializer<T>()
		where T : class, IPrincipalDeserializer
	{
		PrincipalDeserializerType = typeof(T);
		return this;
	}

	internal void Build()
	{
		Services.AddScoped(
			typeof(IPrincipalDeserializer), 
			PrincipalDeserializerType ?? typeof(PrincipalJsonDeserializer));

		ClientAuthOptions options = Options ?? CreateDefaultOptions();
		Services.AddScoped(sp => options!);

		if (RedirectToSignInPageAutomatically)
			RegisterRedirectToSignInPageHttpClient();

		Services.AddHttpClient<AuthHttpClient>(
			AuthHttpClient.HttpClientId,
			x =>
			{
				if (BaseAddress is not null)
					x.BaseAddress = new Uri(BaseAddress!);
			});

		Services.AddScoped<AuthenticationStateProvider, BasicAuthenticationStateProvider>();
	}

	private ClientAuthOptions CreateDefaultOptions()
	{
		var options = new ClientAuthOptions(
			signInUrlTemplate: null,
			getUserClaimsUrl: null);
		return options;
	}

	private void RegisterRedirectToSignInPageHttpClient()
	{
		Services.AddScoped(sp =>
		{
			var navigationManager = sp.GetRequiredService<NavigationManager>();
			var authOptions = sp.GetRequiredService<ClientAuthOptions>();
			var handler = new AuthorizationMessageHandlerWithPageRedirect(navigationManager, authOptions);
			var result = new HttpClient(handler);
			if (BaseAddress is not null)
				result.BaseAddress = new Uri(BaseAddress);
			return result;
		});
	}
}
