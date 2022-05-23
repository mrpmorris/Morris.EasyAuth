using System;
using System.Security.Claims;

namespace Morris.BasicAuth.Blazor.Web.Server;

public interface IPrincipalSerializer
{
	string Serialize(ClaimsPrincipal principal, Func<Claim, bool> claimFilter);
}
