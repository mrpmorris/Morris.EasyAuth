using Microsoft.Extensions.DependencyInjection;
using System;

namespace Morris.EasyAuth.Blazor.Web.Server;

public static class IServiceCollectionExtensions
{
	public static IServiceCollection AddEasyAuth(
		this IServiceCollection services,
		string? baseAddress,
		Action<EasyAuthBuilder>? builder = null)
	{
		var EasyAuthBuilder = new EasyAuthBuilder(services, baseAddress);
		if (builder is not null)
			builder(EasyAuthBuilder);
		EasyAuthBuilder.Build();
		return services;
	}
}
