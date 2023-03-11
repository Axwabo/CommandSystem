using System;
using System.Collections.Generic;
using System.Reflection;
using Axwabo.CommandSystem.Attributes.Interfaces;
using Axwabo.CommandSystem.Exceptions;
using Axwabo.CommandSystem.Permissions;
using Axwabo.CommandSystem.Registration;

namespace Axwabo.CommandSystem.PropertyManager;

/// <summary>Attribute to property handler for base commands.</summary>
public static class BaseCommandPropertyManager {

    /// <summary>The current command registration processor.</summary>
    public static CommandRegistrationProcessor CurrentProcessor { get; internal set; }

    /// <summary>
    /// Attempts to resolve the basic properties of a command.
    /// </summary>
    /// <param name="command">The command to resolve properties for.</param>
    /// <param name="name">The name of the command.</param>
    /// <param name="description">The description of the command.</param>
    /// <param name="aliases">Aliases for the command.</param>
    /// <param name="usage">Usage examples for the command.</param>
    /// <param name="minArguments">The minimum number of arguments required to execute the command.</param>
    /// <param name="playerOnly">Whether the command can only be executed by a player.</param>
    /// <returns>Whether the command name successfully resolved.</returns>
    public static bool TryResolveProperties(CommandBase command, out string name, out string description, out string[] aliases, out string[] usage, out int minArguments, out bool playerOnly) {
        name = null;
        description = null;
        var aliasList = new List<string>();
        var usageList = new List<string>();
        minArguments = 0;
        playerOnly = false;
        foreach (var attribute in command.GetType().GetCustomAttributes()) {
            if (ResolveBaseAttribute(command, attribute, ref name, ref description, aliasList, usageList, ref minArguments, ref playerOnly))
                continue;
            var type = attribute.GetType();
            ResolveName(ref name, type, attribute);
            ResolveDescription(ref description, type, attribute);
            ResolveAliases(aliasList, type, attribute);
            ResolveUsage(usageList, type, attribute);
        }

        aliases = aliasList.ToArray();
        usage = usageList.Count == 0 ? null : usageList.ToArray();
        return !string.IsNullOrEmpty(name);
    }

    /// <summary>
    /// Resolves the permission checkers for a command.
    /// </summary>
    /// <param name="command">The command to resolve permission checkers for.</param>
    /// <returns>The permission checker for the command. If multiple were found, they will be merged into a <see cref="CombinedPermissionChecker"/>.</returns>
    public static IPermissionChecker ResolvePermissionChecker(CommandBase command) {
        var list = new List<IPermissionChecker>();
        list.AddIfNotNull(command as IPermissionChecker);
        foreach (var attribute in command.GetType().GetCustomAttributes()) {
            if (list.AddIfNotNull(ResolveBasePermissionChecker(attribute)) || CurrentProcessor == null)
                continue;
            if (!list.AddIfNotNull(ResolveInstanceBasedPermissionChecker(attribute, command)))
                list.AddIfNotNull(ResolveCustomPermissionChecker(attribute));
        }

        return list.Count switch {
            0 => null,
            1 => list[0],
            _ => new CombinedPermissionChecker(list.ToArray())
        };
    }

    private static IPermissionChecker ResolveBasePermissionChecker(Attribute attribute) => attribute switch {
        SingleVanillaPermissionsAttribute single => new SimpleVanillaPlayerPermissionChecker(single.Permission),
        OneOfVanillaPermissionsAttribute oneOf => new AtLeastOneVanillaPlayerPermissionChecker(oneOf.Permissions),
        StringPermissionsAttribute stringBased => new StringPermissionChecker(stringBased.Permission),
        _ => null
    };

    private static IPermissionChecker ResolveInstanceBasedPermissionChecker(Attribute attribute, CommandBase command) {
        if (attribute is IInstanceBasedPermissionCreator creator)
            return creator.Create(command);
        if (!GenericTypeExtensions.TryGetGenericInterface(attribute.GetType(), typeof(IGenericInstanceBasedPermissionCreator<>), out var type))
            return null;
        var method = type.GetMethod("Create");
        if (method == null)
            return null;
        var parameters = method.GetParameters();
        return parameters is not {Length: 1} || !parameters[0].ParameterType.IsInstanceOfType(command)
            ? null
            : (IPermissionChecker) method.Invoke(attribute, new object[] {command});
    }

    private static IPermissionChecker ResolveCustomPermissionChecker(Attribute attribute) {
        foreach (var creator in CurrentProcessor.PermissionCreators)
            if (creator.Takes(attribute.GetType()))
                return creator.Resolve(attribute);
        return null;
    }

    private static bool ResolveBaseAttribute(CommandBase command, Attribute attribute, ref string name, ref string description, List<string> aliases, List<string> usage, ref int minArguments, ref bool playerOnly) {
        var completed = false;
        if (attribute is ICommandName n) {
            name = n.Name ?? throw new InvalidNameException($"Null command name provided by attribute {attribute.GetType().FullName} on type {command.GetType().FullName}.");
            completed = true;
        }

        if (attribute is IDescription d) {
            description = d.Description;
            completed = true;
        }

        if (attribute is IAliases a) {
            var list = a.Aliases;
            if (list != null)
                aliases.AddRange(list);
        }

        if (attribute is IUsage u) {
            var list = u.Usage;
            if (list != null)
                usage.AddRange(list);
        }

        if (attribute is IMinArguments m) {
            minArguments = m.MinArguments;
            completed = true;
        }

        if (attribute is IPlayerOnlyAttribute) {
            playerOnly = true;
            completed = true;
        }

        return completed || CurrentProcessor == null;
    }

    private static void ResolveName(ref string name, Type type, Attribute attribute) {
        foreach (var resolver in CurrentProcessor.NameResolvers)
            if (resolver.Takes(type))
                name = resolver.Resolve(attribute);
    }

    private static void ResolveDescription(ref string description, Type type, Attribute attribute) {
        foreach (var resolver in CurrentProcessor.DescriptionResolvers)
            if (resolver.Takes(type))
                description = resolver.Resolve(attribute);
    }

    private static void ResolveAliases(List<string> aliases, Type type, Attribute attribute) {
        foreach (var resolver in CurrentProcessor.AliasResolvers)
            if (resolver.Takes(type)) {
                var resolved = resolver.Resolve(attribute);
                if (resolved != null)
                    aliases.AddRange(resolved);
            }
    }

    private static void ResolveUsage(List<string> usage, Type type, Attribute attribute) {
        foreach (var resolver in CurrentProcessor.UsageResolvers)
            if (resolver.Takes(type)) {
                var resolved = resolver.Resolve(attribute);
                if (resolved != null)
                    usage.AddRange(resolved);
            }
    }

}
