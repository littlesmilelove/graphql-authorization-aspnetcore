using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GraphQL.Authorization.AspNetCore
{
    public interface IAuthorizationEvaluator
    {
        Task<AuthorizationResult> Evaluate(
            ClaimsPrincipal principal,
            object userContext,
            Dictionary<string, object> arguments,
            IEnumerable<string> requiredPolicies);
    }

    public class AuthorizationEvaluator : IAuthorizationEvaluator
    {
        IAuthorizationService _authorizationService;
        public AuthorizationEvaluator(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        public async Task<AuthorizationResult> Evaluate(
            ClaimsPrincipal principal,
            object userContext,
            Dictionary<string, object> arguments,
            IEnumerable<string> requiredPolicies)
        {
            var context = new AuthorizationContext();
            context.User = principal ?? new ClaimsPrincipal(new ClaimsIdentity());
            context.UserContext = userContext;
            context.Arguments = arguments;

            var tasks = new List<Task>();

            requiredPolicies.Apply(policy =>
            {
                var task = AuthorizeAsync(policy);
                tasks.Add(task);
            });

            await Task.WhenAll(tasks.ToArray());

            return !context.HasErrors
                ? AuthorizationResult.Success()
                : AuthorizationResult.Fail(context.Errors);


            async Task AuthorizeAsync(string policyName)
            {
                var result = await _authorizationService.AuthorizeAsync(context.User, null, policyName);
                if (!result.Succeeded)
                {
                    context.ReportError($"Required policy '{policyName}' is not present.");
                }
            }
        }
    }
}
