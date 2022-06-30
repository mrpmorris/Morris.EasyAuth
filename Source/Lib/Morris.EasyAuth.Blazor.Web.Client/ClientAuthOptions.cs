namespace Morris.EasyAuth.Blazor.Web.Client;

public class ClientAuthOptions
{
	public string GetUserIdentityUrl { get; } = "/api/account/get-user-identity";
	public string SignInUrlTemplate { get; } = "/account/sign-in?returnUrl={0}";

	public ClientAuthOptions(string? signInUrlTemplate, string? getUserClaimsUrl)
	{
		GetUserIdentityUrl = getUserClaimsUrl ?? GetUserIdentityUrl;
		SignInUrlTemplate = signInUrlTemplate ?? SignInUrlTemplate;
	}
}
