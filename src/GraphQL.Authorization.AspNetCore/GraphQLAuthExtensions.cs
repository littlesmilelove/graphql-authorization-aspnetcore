using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using GraphQL.Validation;
using GraphQL.Authorization.AspNetCore;

namespace GraphQL.Authorization.AspNetCore
{
    public static class GraphQLAuthExtensions
    {
        public static void AddGraphQLAuth(this IServiceCollection services)
        {
            services.TryAddScoped<IAuthorizationEvaluator, AuthorizationEvaluator>();
            services.AddTransient<IValidationRule, AuthorizationValidationRule>();
        }
    }
}
