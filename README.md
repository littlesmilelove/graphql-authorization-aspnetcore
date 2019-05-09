# GraphQL Authorization AspNetCore

A toolset for Asp.net core authorizing integrate to graph types for GraphQL .NET [GraphQL .NET](https://github.com/graphql-dotnet/graphql-dotnet).

This project is base on [GraphQL .Net Authorization](https://github.com/graphql-dotnet/authorization).

# Usage

* Register the authorization classes in your AspNetCore `ConfigureServices` before `GraphQL .NET`.
* Define your own `GraphQLUserContext` and add it to `GraphQL .NET`.
* Use `GraphQLAuthorize` attribute if using Schema + Handler syntax.

# Examples

Define UserContext.
```csharp
public class GraphQLUserContext : IProvideClaimsPrincipal
{
    public ClaimsPrincipal User { get; set; }
}
```

Inject service.
```csharp
services.AddGraphQLAuth();

services.AddGraphQL(options =>
{
    options.ExposeExceptions = true;
}).AddUserContextBuilder(context => new GraphQLUserContext { User = context.User });
```

Define a policy.
```csharp
services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireClaim("Admin");
});
```

GraphType first syntax - use `AuthorizeWith`.

```csharp
public class MyType : ObjectGraphType
{
    public MyType()
    {
        this.AuthorizeWith("AdminPolicy");
        Field<StringGraphType>("name").AuthorizeWith("SomePolicy");
    }
}
```

Schema first syntax - use `GraphQLAuthorize` attribute.

```csharp
[GraphQLAuthorize(Policy = "MyPolicy")]
public class MutationType
{
    [GraphQLAuthorize(Policy = "AnotherPolicy")]
    public async Task<string> CreateSomething(MyInput input)
    {
        return Guid.NewGuid().ToString();
    }
}
```
