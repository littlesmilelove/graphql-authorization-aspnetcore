using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using GraphQL.Validation;
using GraphQL.Authorization.AspNetCore;

namespace GraphQL.Authorization.AspNetCore
{
    public static class GraphQLAuthExtensions
    {
        public static void AddGraphQLAuth(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Singleton)
        {
            var authorizationEvaluatorDescriptor = ServiceDescriptor.Describe(typeof(IAuthorizationEvaluator), typeof(AuthorizationEvaluator), lifetime);
            services.TryAdd(authorizationEvaluatorDescriptor);

            services.AddTransient<IValidationRule, AuthorizationValidationRule>();
        }
    }
}
