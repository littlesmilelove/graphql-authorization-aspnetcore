using GraphQL.Authorization.AspNetCore;
using GraphQL.Utilities;

namespace GraphQL.Authorization.AspNetCore
{
    public class GraphQLAuthorizeAttribute : GraphQLAttribute
    {
        public GraphQLAuthorizeAttribute(string policy)
        {
            Policy = policy;
        }

        public string Policy { get; set; }

        public override void Modify(TypeConfig type)
        {
            type.AuthorizeWith(Policy);
        }

        public override void Modify(FieldConfig field)
        {
            field.AuthorizeWith(Policy);
        }
    }
}
