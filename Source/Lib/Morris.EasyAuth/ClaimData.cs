using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Morris.EasyAuth;

public class ClaimData
{
	public string Type { get; set; } = "";
	public IDictionary<string, string> Properties { get; set; } = new Dictionary<string, string>();
	public string OriginalIssuer { get; set; } = "";
	public string Issuer { get; set; } = "";
	public string ValueType { get; set; } = "";
	public string Value { get; set; } = "";

	public static ClaimData FromClaim(Claim claim)
	{
		if (claim is null)
			throw new ArgumentNullException(nameof(claim));

		return new ClaimData
		{
			Type = claim.Type,
			Properties = claim.Properties,
			OriginalIssuer = claim.OriginalIssuer,
			Issuer = claim.Issuer,
			ValueType = claim.ValueType,
			Value = claim.Value
		};
	}

	public Claim ToClaim()
	{
		var result = new Claim(
			type: Type,
			value: Value,
			valueType: ValueType,
			issuer: Issuer,
			originalIssuer: OriginalIssuer);

		foreach(var kvp in Properties)
			result.Properties.Add(kvp.Key, kvp.Value);

		return result;
	}
}
