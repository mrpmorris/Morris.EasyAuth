using Microsoft.Extensions.DependencyInjection;
using System;

namespace Morris.BasicAuth.Blazor.Web.Server;

public class BasicAuthBuilder
{
	private readonly IServiceCollection Services;
	private ServerAuthOptions? Options;
	private Type? PrincipalSerializerType;

	public BasicAuthBuilder(IServiceCollection services, string? baseAddress)
	{
		Services = services ?? throw new ArgumentNullException(nameof(services));
	}

	public BasicAuthBuilder WithOptions(ServerAuthOptions options)
	{
		Options = options ?? throw new ArgumentNullException(nameof(options));
		return this;
	}

	public BasicAuthBuilder UseSerializer<T>()
		where T : class, IPrincipalSerializer
	{
		PrincipalSerializerType = typeof(T);
		return this;
	}

	internal void Build()
	{
		Services.AddScoped(
			typeof(IPrincipalSerializer),
			PrincipalSerializerType ?? typeof(PrincipalJsonSerializer));

		ServerAuthOptions options = Options ?? CreateDefaultOptions();
		Services.AddScoped(sp => options!);
	}

	private ServerAuthOptions CreateDefaultOptions()
	{
		var options = new ServerAuthOptions(
			signInUrlTemplate: null,
			getUserClaimsUrl: null);
		return options;
	}
}
