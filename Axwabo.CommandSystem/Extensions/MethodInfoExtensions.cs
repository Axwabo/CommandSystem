namespace Axwabo.CommandSystem.Extensions;

/// <summary>Extension methods for <see cref="MethodInfo"/>s.</summary>
public static class MethodInfoExtensions
{

    /// <summary>
    /// Invokes the instance method if the method has a single parameter and the argument can be assigned to that parameter.
    /// </summary>
    /// <param name="method">The method to invoke.</param>
    /// <param name="instance">The instance to invoke the method on.</param>
    /// <param name="argument">The argument to pass to the method.</param>
    /// <returns>The return value of the method, or <see langword="null"/> if the method could not be invoked.</returns>
    public static object InvokeIfSingleParameterMatchesType(this MethodInfo method, object instance, object argument)
    {
        if (method == null)
            return null;
        var parameters = method.GetParameters();
        return parameters is {Length: 1} && parameters[0].ParameterType.IsInstanceOfType(argument)
            ? method.Invoke(instance, [argument])
            : null;
    }

    /// <summary>
    /// Invokes the instance method if the method has a single parameter and the argument can be assigned to that parameter, then casts the result to <typeparamref name="TReturn"/>.
    /// </summary>
    /// <param name="method">The method to invoke.</param>
    /// <param name="instance">The instance to invoke the method on.</param>
    /// <param name="argument">The argument to pass to the method.</param>
    /// <typeparam name="TReturn">The type to cast the return value to.</typeparam>
    /// <returns>The return value of the method, or <see langword="default"/> if the method could not be invoked.</returns>
    public static TReturn InvokeIfSingleParameterMatchesType<TReturn>(this MethodInfo method, object instance, object argument)
        => (TReturn) method.InvokeIfSingleParameterMatchesType(instance, argument);

}
