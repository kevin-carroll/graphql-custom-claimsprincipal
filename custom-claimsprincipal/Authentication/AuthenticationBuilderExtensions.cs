namespace GraphQL.AspNet.Examples.CustomClaimsPrincipal.Authentication
{
    using System;
    using Microsoft.AspNetCore.Authentication;

    public static class AuthenticationBuilderExtensions
    {
        public static AuthenticationBuilder AddMockedAuthentication(
            this AuthenticationBuilder builder)
        {
            // Add custom authentication scheme with custom options and custom handler
            return builder.AddScheme<MockedAuthOptions, MockedAuthHandler>(
                MockedAuthOptions.AuthSchemeName,
                (o) => { });
        }
    }
}