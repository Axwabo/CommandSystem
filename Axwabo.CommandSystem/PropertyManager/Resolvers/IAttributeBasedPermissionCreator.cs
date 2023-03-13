using System;
using Axwabo.CommandSystem.Permissions;

namespace Axwabo.CommandSystem.PropertyManager.Resolvers;

/// <summary>Base interface to resolve a <see cref="IPermissionChecker"/>. You must implement <see cref="IAttributeBasedPermissionCreator{TAttribute}"/> due to reflection magic.</summary>
public interface IAttributeBasedPermissionCreator
{

}

/// <summary>
/// An interface to resolve an <see cref="IPermissionChecker"/> based on an attribute.
/// </summary>
/// <typeparam name="TAttribute">The type of the attribute to resolve the permission checker from.</typeparam>
public interface IAttributeBasedPermissionCreator<in TAttribute> : IAttributeBasedPermissionCreator where TAttribute : Attribute
{

    /// <summary>
    /// Resolves the permission checker based on the attribute.
    /// </summary>
    /// <param name="attribute">The attribute to resolve the permission checker from.</param>
    /// <returns>The resolved <see cref="IPermissionChecker"/>.</returns>
    IPermissionChecker CreatePermissionCheckerInstance(TAttribute attribute);

}
