﻿using Axwabo.CommandSystem.Commands.MessageOverrides;

namespace Axwabo.CommandSystem.PropertyManager.Resolvers;

/// <summary>Base interface to resolve an <see cref="ITargetSelectionManager"/>. You must implement <see cref="ITargetSelectionResolver{TAttribute}"/> due to reflection magic.</summary>
public interface ITargetSelectionResolver
{

}

/// <summary>
/// An interface to resolve an <see cref="ITargetSelectionManager"/> based on an attribute.
/// </summary>
/// <typeparam name="TAttribute">The type of the attribute to resolve the manager from.</typeparam>
public interface ITargetSelectionResolver<in TAttribute> : ITargetSelectionResolver where TAttribute : Attribute
{

    /// <summary>
    /// Resolves the <see cref="ITargetSelectionManager"/> based on the attribute.
    /// </summary>
    /// <param name="attribute">The attribute to resolve the manager from.</param>
    /// <returns>The resolved <see cref="ITargetSelectionManager"/>.</returns>
    ITargetSelectionManager ResolveManager(TAttribute attribute);

}
