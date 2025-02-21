using Axwabo.CommandSystem.Commands.Interfaces;

namespace Axwabo.CommandSystem.PropertyManager.Resolvers;

/// <summary>Base interface to resolve an <see cref="ICustomResultCompiler"/>. You must implement <see cref="IResultCompilerResolver{TAttribute}"/> due to reflection magic.</summary>
public interface IResultCompilerResolver;

/// <summary>
/// An interface to resolve an <see cref="ICustomResultCompiler"/> based on an attribute.
/// </summary>
/// <typeparam name="TAttribute">The type of the attribute to resolve the manager from.</typeparam>
public interface IResultCompilerResolver<in TAttribute> : IResultCompilerResolver where TAttribute : Attribute
{

    /// <summary>
    /// Resolves the <see cref="ICustomResultCompiler"/> based on the attribute.
    /// </summary>
    /// <param name="attribute">The attribute to resolve the manager from.</param>
    /// <returns>The resolved <see cref="ICustomResultCompiler"/>.</returns>
    ICustomResultCompiler ResolveCompiler(TAttribute attribute);

}
