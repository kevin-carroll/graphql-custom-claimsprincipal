namespace custom_claimsprincipal
{
    using custom_claimsprincipal.Models;
    using Microsoft.AspNetCore.Http;
    using System.Threading.Tasks;

    public class RebuildClaimsPrincipalMiddleware
    {
        private readonly RequestDelegate _next;

        public RebuildClaimsPrincipalMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.User != null)
            {
                var user = new CompanyClaimsPrincipal(context.User);

                // set a custom phrase during this http middleware intance
                // that is unique at this location. We can check the instance
                // of the ClaimsPrincipal in a controller to ensure its the SAME instance
                // made here and not a new instance
                user.Phrase = $"assigned via {nameof(RebuildClaimsPrincipalMiddleware)}";
                context.User = user;
            }

            // Call the next delegate/middleware in the pipeline.
            await _next(context);
        }
    }
}