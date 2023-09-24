namespace custom_claimsprincipal.Controllers
{
    using custom_claimsprincipal.Models;
    using GraphQL.AspNet.Controllers;

    public abstract class CompanyGraphControllerBase : GraphController
    {
        /// <summary>
        /// Gets the assigned user context as the company's customized instance
        /// for easy processing
        /// </summary>
        /// <value>The company user.</value>
        public CompanyClaimsPrincipal CompanyUser => this.User as CompanyClaimsPrincipal;
    }
}