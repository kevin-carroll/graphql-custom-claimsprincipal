namespace GraphQL.AspNet.Examples.CustomClaimsPrincipal.Authentication
{
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.Extensions.Primitives;

    public class MockedAuthOptions : AuthenticationSchemeOptions
    {
        public const string AuthSchemeName = "mocked-authentication";
        public string Scheme => AuthSchemeName;
    }
}