namespace Axwabo.CommandSystem.Registration;

internal sealed class ResolverContainer<TResolver, TResult>
{

    private readonly MethodInfo _method;

    private readonly Type _parameter;

    private readonly TResolver _instance;

    public ResolverContainer(MethodInfo method, Type parameter, TResolver instance)
    {
        _method = method;
        _parameter = parameter;
        _instance = instance;
    }

    public bool Takes(Type type) => _parameter.IsAssignableFrom(type);

    public TResult Resolve(Attribute attribute) => (TResult) _method.Invoke(_instance, new object[] {attribute});

}
