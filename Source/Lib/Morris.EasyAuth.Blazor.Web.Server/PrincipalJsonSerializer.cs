using System;
using System.Security.Claims;
using System.Text.Json;

namespace Morris.EasyAuth.Blazor.Web.Server;

internal class PrincipalJsonSerializer : IPrincipalSerializer
{
	public static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

	public string Serialize(ClaimsPrincipal principal, Func<Claim, bool> claimsFilter)
	{
		if (principal is null)
			throw new ArgumentNullException(nameof(principal));
		if (claimsFilter is null)
			throw new ArgumentNullException(nameof(claimsFilter));

		var principalData = ClaimsPrincipalData.FromClaimsPrincipal(principal, claimsFilter);
		return JsonSerializer.Serialize(principalData, JsonOptions);
	}
}
