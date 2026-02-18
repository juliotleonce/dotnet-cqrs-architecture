namespace poketra_vyrt_api.Infrastructure.Security.Policy;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class AllowNotVerifiedAttribute: Attribute { }