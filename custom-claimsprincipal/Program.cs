namespace GraphQL.AspNet.Examples.CustomClaimsPrincipal
{
    using custom_claimsprincipal;
    using custom_claimsprincipal.GraphQLComponents;
    using custom_claimsprincipal.GraphQLMiddleware;
    using GraphQL.AspNet.Configuration;
    using GraphQL.AspNet.Examples.CustomClaimsPrincipal.Authentication;
    using GraphQL.AspNet.Middleware.SchemaItemSecurity;
    using GraphQL.AspNet.Schemas;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    public class Program
    {
        private const string ALL_ORIGINS_POLICY = "_allOrigins";

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // apply an unrestricted cors policy for demoing
            // to allow use on many of the tools for testing (graphiql, altair etc.)
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(
                    ALL_ORIGINS_POLICY,
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    });
            });

            // **********************************************************
            // Add authentication options using a "mocked"
            // authentication setup for demo purposes. Normally you'd connect with your identity provider
            // here  (e.g. Auth0, Azure B2C etc.)
            // **********************************************************
            builder.Services.AddAuthentication()
                .AddMockedAuthentication();
            builder.Services.AddAuthorization();


            // **********************************************************
            // Register a custom http processor that contains an override for the user security context
            // that is passed to the graphql runtime
            // **********************************************************
            var schemaBuilder = builder.Services.AddGraphQL(o =>
            {
                o.QueryHandler.HttpProcessorType = typeof(CompanyGraphQLHttpProcessor<GraphSchema>);
            });

            // **********************************************************
            // Build the http pipeline
            // **********************************************************
            var app = builder.Build();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors(ALL_ORIGINS_POLICY);

            // 1) authorize the request
            app.UseAuthorization();

            // 2) repackage the claims principal on the http pipeline
            //    such that HttpContext.User will be an instance of 'CompanyClaimsPrincipal'
            app.UseMiddleware<RebuildClaimsPrincipalMiddleware>();

            // 3) Hand the http context to graphql for processing
            app.UseGraphQL();
            app.Run();
        }
    }
}