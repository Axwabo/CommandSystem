using System;
using System.Linq;
using Axwabo.CommandSystem.Exceptions;

namespace Axwabo.CommandSystem;

public static class GenericTypeExtensions {

    public static Type FirstGenericType(object o) {
        var type = o as Type ?? o.GetType();
        var args = type.GenericTypeArguments;
        return args is not {Length: not 0} ? null : args[0];
    }

    public static Type GetGenericInterface(this Type targetType, Type genericDefinition) => targetType.GetInterfaces().FirstOrDefault(e => e.IsGenericType && e.GetGenericTypeDefinition() == genericDefinition);

    public static bool TryGetGenericInterface(this Type targetType, Type genericDefinition, out Type genericInterface) => (genericInterface = GetGenericInterface(targetType, genericDefinition)) != null;

    public static Type ImplementedGenericType(this object resolver, Type interfaceType)
        => FirstGenericType(
            GetGenericInterface(resolver.GetType(), interfaceType)
            ?? throw new TypeMismatchException($"Interface does not implement the generic type {interfaceType.Name}")
        );

}
