namespace GraphQL.AspNet.Examples.CustomClaimsPrincipal.Authentication
{
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Microsoft.Net.Http.Headers;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;

    public class MockedAuthHandler : AuthenticationHandler<MockedAuthOptions>
    {
        public const string USERNAME_CLAIM_TYPE = "mycompany-username";
        public const string ROLES_CLAIM_TYPE = "mycompany-roles";
        public const string EMPLOYEE_ID_CLAIM_TYPE = "mycompany-employeeid";

        private static IReadOnlyList<AppUser> _users;

        static MockedAuthHandler()
        {
            // mock two fake "user accounts"
            // that can be authenticated by supplying their id as an auth token
            var users = new List<AppUser>();
            users.Add(new AppUser("abc123", "bobSmith", "employee-id-bob"));
            users.Add(new AppUser("xyz456", "janeSmith", "employee-id-jane"));
            _users = users;
        }

        private readonly IOptionsMonitor<MockedAuthOptions> _options;

        public MockedAuthHandler(
            IOptionsMonitor<MockedAuthOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
            _options = options;
        }

        /// <inheritdoc />
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var result = this.Authenticate();
            return Task.FromResult(result);
        }

        private AuthenticateResult Authenticate()
        {
            // Get Authorization header value
            if (!Request.Headers.TryGetValue(HeaderNames.Authorization, out var authorization))
                return AuthenticateResult.Fail("No Authorization Header Provided on the Request.");

            var authValue = authorization.ToString();
            if (string.IsNullOrWhiteSpace(authValue))
                return AuthenticateResult.Fail("Invalid Authorization Header Provided on the Request.");

            // The auth header value from Authorization header must be a valid user id
            authValue = authValue.Trim();
            var user = _users.SingleOrDefault(x => x.Id == authValue);
            if (user == null)
                return AuthenticateResult.Fail("Invalid user Id.");

            // Create an authentication ticket using a standard claims principal
            // and some claims
            // in a real world scenario these would probably be decoded from a verified bearer token
            var identity = new ClaimsIdentity(Options.Scheme, USERNAME_CLAIM_TYPE, ROLES_CLAIM_TYPE);
            identity.AddClaim(new Claim(USERNAME_CLAIM_TYPE, user.UserName));
            identity.AddClaim(new Claim(EMPLOYEE_ID_CLAIM_TYPE, user.EmployeeId));

            var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), Options.Scheme);

            return AuthenticateResult.Success(ticket);
        }
    }
}