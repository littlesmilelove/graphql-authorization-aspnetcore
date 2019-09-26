using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace GraphQL.Authorization.AspNetCore
{
    public class AuthorizationContext
    {
        public AuthorizationContext(ClaimsPrincipal user, object userContext, Dictionary<string, object> arguments)
        {
            User = user;
            UserContext = userContext;
            Arguments = arguments ?? new Dictionary<string, object>();
        }

        private readonly List<string> _errors = new List<string>();

        public ClaimsPrincipal User { get; set; }

        public object UserContext { get; set; }

        public Dictionary<string, object> Arguments { get; set; }

        public IEnumerable<string> Errors => _errors;

        public bool HasErrors => _errors.Any();

        public void ReportError(string error)
        {
            _errors.Add(error);
        }
    }
}
