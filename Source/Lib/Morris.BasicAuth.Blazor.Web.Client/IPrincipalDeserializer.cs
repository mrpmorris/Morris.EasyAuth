using System.Security.Claims;

namespace Morris.BasicAuth.Blazor.Web.Client;

public interface IPrincipalDeserializer
{
	ClaimsPrincipal Deserialize(string payload);
}
