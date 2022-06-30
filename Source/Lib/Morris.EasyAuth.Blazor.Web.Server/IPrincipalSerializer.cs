using System;
using System.Security.Claims;

namespace Morris.EasyAuth.Blazor.Web.Server;

public interface IPrincipalSerializer
{
	string Serialize(ClaimsPrincipal principal, Func<Claim, bool> claimFilter);
}
