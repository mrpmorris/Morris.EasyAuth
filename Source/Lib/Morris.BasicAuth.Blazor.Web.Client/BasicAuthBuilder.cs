using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Morris.BasicAuth.Blazor.Web.Client;

public class BasicAuthBuilder
{
	private readonly IServiceCollection Services;
	private readonly string? BaseAddress;
	private ClientAuthOptions? Options;
	private Type? PrincipalDeserializerType;

	public BasicAuthBuilder(IServiceCollection services, string? baseAddress)
	{
		Services = services ?? throw new ArgumentNullException(nameof(services));
		BaseAddress = baseAddress;
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

		Services.AddScoped<AuthorizingMessageHandler>();
		Services.AddScoped<AuthenticationStateProvider, BasicAuthenticationStateProvider>();

		ClientAuthOptions options = Options ?? CreateDefaultOptions();
		Services.AddScoped(sp => options!);

		// Client to get user claims
		Services.AddHttpClient<NonRedirectingHttpClient>(
			NonRedirectingHttpClient.HttpClientId,
			x =>
			{
				if (BaseAddress is not null)
					x.BaseAddress = new Uri(BaseAddress!);
			});
	}

	private ClientAuthOptions CreateDefaultOptions()
	{
		var options = new ClientAuthOptions(
			signInUrlTemplate: null,
			getUserClaimsUrl: null);
		return options;
	}
}
