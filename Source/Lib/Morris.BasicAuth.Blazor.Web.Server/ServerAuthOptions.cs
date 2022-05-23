namespace Morris.BasicAuth.Blazor.Web.Server;

public class ServerAuthOptions
{
	public string GetUserIdentityUrl { get; } = "/api/account/get-user-identity";
	public string SignInUrlTemplate { get; } = "/account/sign-in?returnUrl={0}";

	public ServerAuthOptions(string? signInUrlTemplate, string? getUserClaimsUrl)
	{
		GetUserIdentityUrl = getUserClaimsUrl ?? GetUserIdentityUrl;
		SignInUrlTemplate = signInUrlTemplate ?? SignInUrlTemplate;
	}
}
