using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GraphQL.Builders;
using GraphQL.Types;

namespace GraphQL.Authorization.AspNetCore
{
    public static class AuthorizationMetadataExtensions
    {
        public static readonly string PolicyKey = "Authorization__Policies";

        public static bool RequiresAuthorization(this IProvideMetadata type)
        {
            return type.GetPolicies().Any();
        }

        public static Task<AuthorizationResult> Authorize(
            this IProvideMetadata type,
            ClaimsPrincipal principal,
            object userContext,
            Dictionary<string, object> arguments,
            IAuthorizationEvaluator evaluator)
        {
            var policies = type.GetPolicies();
            return evaluator.Evaluate(principal, userContext, arguments, policies);
        }

        public static void AuthorizeWith(this IProvideMetadata type, string policy)
        {
            var policies = type.GetPolicies();
            if (!policies.Contains(policy))
            {
                policies.Add(policy);
            }
            type.Metadata[PolicyKey] = policies;
        }

        public static FieldBuilder<TSourceType, TReturnType> AuthorizeWith<TSourceType, TReturnType>(
            this FieldBuilder<TSourceType, TReturnType> builder, string policy)
        {
            builder.FieldType.AuthorizeWith(policy);
            return builder;
        }

        public static List<string> GetPolicies(this IProvideMetadata type)
        {
            return type.GetMetadata(PolicyKey, new List<string>());
        }
    }
}
