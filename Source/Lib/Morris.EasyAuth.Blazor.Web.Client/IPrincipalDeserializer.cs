using System.Security.Claims;

namespace Morris.EasyAuth.Blazor.Web.Client;

public interface IPrincipalDeserializer
{
	ClaimsPrincipal Deserialize(string payload);
}
