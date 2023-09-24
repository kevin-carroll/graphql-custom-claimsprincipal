namespace custom_claimsprincipal.Controllers
{
    using GraphQL.AspNet.Attributes;
    using Microsoft.AspNetCore.Authorization;

    [GraphRoute("employee")]
    public class EmployeeController : CompanyGraphControllerBase
    {
        [Query("id")]
        [Authorize]
        public string RetrieveMyEmployeeId()
        {
            // if CompanyUser is an instance of CompanyClaimsPrincipal
            // and if it contains all the claims from the original authorization
            // then employee id should be set
            return this.CompanyUser?.EmployeeId;
        }

        [Query("phrase")]
        [Authorize]
        public string RetrieveCustomPhrase()
        {
            // if CompanyUser is an instance of CompanyClaimsPrincipal
            // and if it is the SAME instance from the http middleware component
            // then the phrase should be set to the correct phrase
            return this.CompanyUser?.Phrase;
        }
    }
}