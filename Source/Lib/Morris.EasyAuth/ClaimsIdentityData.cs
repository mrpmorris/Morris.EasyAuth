using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Morris.EasyAuth;

public class ClaimsIdentityData
{
	public string? AuthenticationType { get; set; }
	public string? NameType { get; set; }
	public string? RoleType { get; set; }

	public ClaimData[] Claims { get; set; } = Array.Empty<ClaimData>();

	public ClaimsIdentityData() { }

	public ClaimsIdentityData(string? authenticationType, string? nameType, string? roleType, IEnumerable<ClaimData> claims) 
	{
		AuthenticationType = authenticationType;
		NameType = nameType;
		RoleType = roleType;
		Claims = claims?.ToArray() ?? throw new ArgumentNullException(nameof(claims));
	}

	public static ClaimsIdentityData FromClaimsIdentity(ClaimsIdentity identity, Func<Claim, bool> claimsFilter)
	{
		if (identity is null)
			throw new ArgumentNullException(nameof(identity));
		if (claimsFilter is null)
			throw new ArgumentNullException(nameof(claimsFilter));

		var result =
			new ClaimsIdentityData(
				authenticationType: identity.AuthenticationType,
				nameType: identity.NameClaimType,
				roleType: identity.RoleClaimType,
				claims: identity.Claims.Where(x => claimsFilter(x)).Select(x => ClaimData.FromClaim(x)));
		return result;
	}

	public ClaimsIdentity ToClaimsIdentity()
	{
		var result =
			new ClaimsIdentity(
				claims: Claims.Select(x => x.ToClaim()),
				authenticationType: AuthenticationType,
				nameType: NameType,
				roleType: RoleType);
		return result;
	}
}
