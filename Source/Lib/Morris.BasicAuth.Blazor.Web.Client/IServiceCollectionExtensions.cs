using Microsoft.Extensions.DependencyInjection;
using System;

namespace Morris.BasicAuth.Blazor.Web.Client;

public static class IServiceCollectionExtensions
{
	public static IServiceCollection AddBasicAuth(
		this IServiceCollection services,
		string baseAddress,
		Action<BasicAuthBuilder>? builder = null)
	{
		if (baseAddress is null)
			throw new ArgumentNullException(nameof(baseAddress));

		var basicAuthBuilder = new BasicAuthBuilder(services, baseAddress);
		if (builder is not null)
			builder(basicAuthBuilder);
		basicAuthBuilder.Build();

		return services;
	}
}
