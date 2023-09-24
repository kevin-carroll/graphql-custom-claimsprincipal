namespace custom_claimsprincipal.Models
{
    using GraphQL.AspNet.Examples.CustomClaimsPrincipal.Authentication;
    using System.Security.Claims;

    public class CompanyClaimsPrincipal : ClaimsPrincipal
    {
        public CompanyClaimsPrincipal(ClaimsPrincipal otherPrincipal)
            : base(otherPrincipal)
        {
        }

        public string EmployeeId => this.FindFirstValue(MockedAuthHandler.EMPLOYEE_ID_CLAIM_TYPE);

        public string Phrase { get; set; }
    }
}