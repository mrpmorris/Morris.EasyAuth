using System;
using System.Linq;
using System.Security.Claims;

namespace Morris.EasyAuth;

public class ClaimsPrincipalData
{
	public ClaimsIdentityData[] Identities { get; set; } = Array.Empty<ClaimsIdentityData>();

	public static ClaimsPrincipalData FromClaimsPrincipal(ClaimsPrincipal principal, Func<Claim, bool>? claimsFilter)
	{
		if (principal is null)
			throw new ArgumentNullException(nameof(principal));
		if (claimsFilter is null)
			throw new ArgumentNullException(nameof(claimsFilter));

		var result = new ClaimsPrincipalData
		{
			Identities = principal.Identities
			.Select(x => ClaimsIdentityData.FromClaimsIdentity(x, claimsFilter))
			.ToArray()
		};
		return result;
	}

	public ClaimsPrincipal ToClaimsPrincipal()
	{
		var result = new ClaimsPrincipal(Identities.Select(x => x.ToClaimsIdentity()));
		return result;
	}
}
