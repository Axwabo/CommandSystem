using System;
using System.Collections.Generic;
using System.Reflection;
using Axwabo.CommandSystem.Attributes.Interfaces;
using Axwabo.CommandSystem.Exceptions;
using Axwabo.CommandSystem.Permissions;
using Axwabo.CommandSystem.RemoteAdminExtensions;

namespace Axwabo.CommandSystem.PropertyManager;

/// <summary>Attribute to property handler for Remote Admin options.</summary>
public static class RemoteAdminExtensionPropertyManager
{

    /// <summary>
    /// Attempts to resolve the basic properties of a RA option.
    /// </summary>
    /// <param name="option">The option to resolve properties for.</param>
    /// <param name="id">The identifier of the option.</param>
    /// <param name="staticText">The static text of the option.</param>
    /// <returns>Whether the option id was successfully resolved.</returns>
    public static bool TryResolveProperties(RemoteAdminOptionBase option, out string id, out string staticText)
    {
        id = null;
        staticText = null;
        foreach (var attribute in option.GetType().GetCustomAttributes())
        {
            if (ResolveBaseAttribute(option, attribute, ref id, ref staticText))
                continue;
            // TODO: custom resolvers
        }

        return RemoteAdminOptionManager.IsValidOptionId(id);
    }

    private static bool ResolveBaseAttribute(RemoteAdminOptionBase option, Attribute attribute, ref string id, ref string staticText)
    {
        var completed = false;
        if (attribute is IRemoteAdminOptionIdentifier identifier)
        {
            id = identifier.Id ?? throw new InvalidNameException($"Null option identifier provided by attribute {attribute.GetType().FullName} on type {option.GetType().FullName}.");
            completed = true;
        }

        if (attribute is IStaticOptionText text)
        {
            text.Text.SetFieldIfNotNull(ref staticText);
            completed = true;
        }

        return completed;
    }

    /// <summary>
    /// Resolves a RA option permission checker from an attribute that implements <see cref="IInstanceBasedPermissionCreator"/> or <see cref="IGenericRemoteAdminOptionInstanceBasedPermissionCreator{TCommand}"/>.
    /// </summary>
    /// <param name="attribute">The attribute to use.</param>
    /// <param name="option">The option to pass in as an argument.</param>
    /// <returns>The permission checker or null if it was not an instance-based attribute.</returns>
    public static IPermissionChecker ResolveInstanceBasedPermissionChecker(Attribute attribute, RemoteAdminOptionBase option)
        => attribute is IInstanceBasedPermissionCreator creator
            ? creator.Create(option)
            : GenericTypeExtensions.TryGetGenericInterface(attribute.GetType(), typeof(IGenericRemoteAdminOptionInstanceBasedPermissionCreator<>), out var type)
                ? type.GetMethod("Create").InvokeIfSingleParameterMatchesType<IPermissionChecker>(attribute, option)
                : null;

    /// <summary>
    /// Resolves the permission checkers for a RA option.
    /// </summary>
    /// <param name="option">The command to resolve permission checkers for.</param>
    /// <param name="member">The member to resolve permission checkers for. If null, the command's type will be used.</param>
    /// <returns>The permission checker for the command. If multiple were found, they will be merged into a <see cref="CombinedPermissionChecker"/>.</returns>
    public static IPermissionChecker ResolvePermissionChecker(RemoteAdminOptionBase option, MemberInfo member = null)
    {
        var list = new List<IPermissionChecker>();
        if (member is Type)
            list.SafeCastAndAdd(option);
        foreach (var attribute in (member ?? option.GetType()).GetCustomAttributes())
        {
            if (list.AddIfNotNull(BaseCommandPropertyManager.ResolveBuiltInPermissionChecker(attribute)))
                continue;
            if (!list.AddIfNotNull(ResolveInstanceBasedPermissionChecker(attribute, option)))
                list.AddIfNotNull(BaseCommandPropertyManager.ResolveCustomPermissionChecker(attribute));
        }

        return list.Count switch
        {
            0 => null,
            1 => list[0],
            _ => new CombinedPermissionChecker(list.ToArray())
        };
    }

}
