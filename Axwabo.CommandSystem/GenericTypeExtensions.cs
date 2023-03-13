using System;
using System.Linq;
using Axwabo.CommandSystem.Exceptions;

namespace Axwabo.CommandSystem;

/// <summary>
/// Provides methods for working with generic types.
/// </summary>
public static class GenericTypeExtensions
{

    /// <summary>
    /// Gets the first generic type argument of the given type.
    /// </summary>
    /// <param name="o">The object or type to get the generic type argument from.</param>
    /// <returns>The first generic type argument of the given type.</returns>
    /// <remarks>If a <see cref="Type"/> object is given it will be used.</remarks>
    public static Type FirstGenericArgument(object o)
    {
        if (o == null)
            throw new ArgumentNullException(nameof(o));
        var type = o as Type ?? o.GetType();
        var args = type.GenericTypeArguments;
        return args is not {Length: not 0} ? null : args[0];
    }

    /// <summary>
    /// Gets the first inherited generic type from an unknown type definition.
    /// </summary>
    /// <param name="targetType">The type to search for the generic type definition.</param>
    /// <param name="genericDefinition">The generic type definition to search for.</param>
    /// <returns>The first inherited generic type.</returns>
    /// <example>
    /// Assume the following hierarchy:<br/>
    /// - IGeneric&lt;T&gt;<br/>
    /// - BaseClass : IGeneric&lt;string&gt;<br/>
    /// To get the generic type definition of IGeneric&lt;string&gt; from BaseClass, use the following code:
    /// <code>
    /// var genericType = GetGenericInterface(typeof(BaseClass), typeof(IGeneric&lt;&gt;));
    /// </code>
    /// </example>
    public static Type GetGenericInterface(Type targetType, Type genericDefinition) => targetType.GetInterfaces().FirstOrDefault(e => e.IsGenericType && e.GetGenericTypeDefinition() == genericDefinition);

    /// <summary>
    /// Gets the first inherited generic type from an unknown type definition.
    /// </summary>
    /// <param name="targetType">The type to search for the generic type definition.</param>
    /// <param name="genericDefinition"></param>
    /// <param name="genericInterface"></param>
    /// <returns></returns>
    public static bool TryGetGenericInterface(Type targetType, Type genericDefinition, out Type genericInterface) => (genericInterface = GetGenericInterface(targetType, genericDefinition)) != null;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="resolver"></param>
    /// <param name="interfaceType"></param>
    /// <returns></returns>
    /// <exception cref="TypeMismatchException"></exception>
    public static Type ImplementedGenericType(object resolver, Type interfaceType)
        => FirstGenericArgument(
            GetGenericInterface(resolver.GetType(), interfaceType)
            ?? throw new TypeMismatchException($"Interface does not implement the generic type {interfaceType.Name}")
        );

}
