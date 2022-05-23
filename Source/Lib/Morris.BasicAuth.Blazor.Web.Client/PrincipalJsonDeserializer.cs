using System;
using System.Security.Claims;
using System.Text.Json;

namespace Morris.BasicAuth.Blazor.Web.Client;

internal class PrincipalJsonDeserializer : IPrincipalDeserializer
{
	public static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

	public ClaimsPrincipal Deserialize(string payload)
	{
		if (payload is null)
			throw new ArgumentNullException(nameof(payload));

		var principleData = JsonSerializer.Deserialize<ClaimsPrincipalData>(payload, JsonOptions)!;
		return principleData.ToClaimsPrincipal();
	}
}
