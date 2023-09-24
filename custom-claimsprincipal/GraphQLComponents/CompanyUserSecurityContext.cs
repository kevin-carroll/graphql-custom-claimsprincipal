﻿namespace custom_claimsprincipal.GraphQLMiddleware
{
    using custom_claimsprincipal.Models;
    using GraphQL.AspNet.Interfaces.Security;
    using GraphQL.AspNet.Web.Security;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Http;
    using System.Security.Claims;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// This class decorates the default <see cref="IUserSecurityContext"/> generated by
    /// graphql and forces the runtime to use our custom claims principal if its found
    /// on the http context. When not found, graphql aspnet falls into its normal authentication flow.
    /// </summary>
    public class CompanyUserSecurityContext : IUserSecurityContext
    {
        private HttpContext _httpContext;
        private IUserSecurityContext _defaultUserSecurityContext;

        public CompanyUserSecurityContext(HttpContext context, IUserSecurityContext defaultUserSecurityContext)
        {
            _httpContext = context;
            _defaultUserSecurityContext = defaultUserSecurityContext;
        }

        public async Task<IAuthenticationResult> AuthenticateAsync(string scheme, CancellationToken token = default)
        {
            // WARNING: This method call is NOT thread safe.
            //
            //          Also, This method runs for EVERY field in the request
            //          that contains authorization requirements
            //          do not perform any side effects that will effect performance.
            //          Consider caching the final
            //          authResult to reduce heap allocations
            var httpUser = _httpContext.User;

            // if the user on the http context is our instance of company user
            // then we know for a fact its authetnicated. We can just use it
            //
            // scheme may or may not be relevant for your use case
            // depending on how many configured authentication options you provide to your users
            // and your configured authorization policies
            if (httpUser is CompanyClaimsPrincipal
                && httpUser.Identity.AuthenticationType == scheme)
            {
                // generate a successul authentication result
                var httpAuthResult = AuthenticateResult.Success(
                    new AuthenticationTicket(httpUser, scheme));

                // package the result for the graphql security pipeline
                var authResult = new HttpContextAuthenticationResult(
                    scheme,
                    httpAuthResult);

                return authResult;
            }

            return await _defaultUserSecurityContext.AuthenticateAsync(scheme, token);
        }

        public Task<IAuthenticationResult> AuthenticateAsync(CancellationToken token = default)
            => _defaultUserSecurityContext.AuthenticateAsync(token);

        public ClaimsPrincipal DefaultUser => _defaultUserSecurityContext.DefaultUser;
    }
}