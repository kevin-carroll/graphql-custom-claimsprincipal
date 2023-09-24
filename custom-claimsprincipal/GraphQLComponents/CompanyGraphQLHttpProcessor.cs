namespace custom_claimsprincipal.GraphQLComponents
{
    using custom_claimsprincipal.GraphQLMiddleware;
    using GraphQL.AspNet.Engine;
    using GraphQL.AspNet.Interfaces.Engine;
    using GraphQL.AspNet.Interfaces.Logging;
    using GraphQL.AspNet.Interfaces.Schema;
    using GraphQL.AspNet.Interfaces.Security;

    public class CompanyGraphQLHttpProcessor<TSchema> : DefaultGraphQLHttpProcessor<TSchema>
        where TSchema : class, ISchema
    {
        public CompanyGraphQLHttpProcessor(
            TSchema schema,
            IGraphQLRuntime<TSchema> runtime,
            IQueryResponseWriter<TSchema> writer,
            IGraphEventLogger logger = null)
            : base(schema, runtime, writer, logger)
        {
        }

        protected override IUserSecurityContext CreateUserSecurityContext()
        {
            // grab an instance of the internally generated security context
            // then decorate it with additional business logic
            var baseContext = base.CreateUserSecurityContext();
            return new CompanyUserSecurityContext(this.HttpContext, baseContext);
        }
    }
}