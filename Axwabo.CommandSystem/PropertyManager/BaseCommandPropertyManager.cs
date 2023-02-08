﻿using System;
using System.Collections.Generic;
using System.Reflection;
using Axwabo.CommandSystem.Attributes.Interfaces;
using Axwabo.CommandSystem.Permissions;
using Axwabo.CommandSystem.Registration;

namespace Axwabo.CommandSystem.PropertyManager;

public static class BaseCommandPropertyManager {

    public static CommandRegistrationProcessor CurrentProcessor { get; internal set; }

    public static bool ValidateRegistration(CommandBase command, bool throwIfProcessorIsNull = true) {
        if (command is null)
            throw new ArgumentNullException(nameof(command));
        if (CurrentProcessor != null)
            return true;
        return throwIfProcessorIsNull
            ? throw new InvalidOperationException("Attempted to resolve command properties outside of a registration process.")
            : false;
    }

    public static bool TryResolveProperties(CommandBase command, out string name, out string description, out string[] aliases, out string[] usage, out int minArguments) {
        ValidateRegistration(command);
        name = null;
        description = null;
        var aliasList = new List<string>();
        var usageList = new List<string>();
        minArguments = 0;
        foreach (var attribute in command.GetType().GetCustomAttributes()) {
            if (ResolveBaseAttribute(attribute, ref name, ref description, aliasList, usageList, ref minArguments))
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

    public static IPermissionChecker ResolvePermissionChecker(CommandBase command) {
        if (!ValidateRegistration(command, false))
            return null;
        var list = new List<IPermissionChecker>();
        list.AddIfNotNull(command as IPermissionChecker);
        foreach (var attribute in command.GetType().GetCustomAttributes()) {
            if (list.AddIfNotNull(ResolveBasePermissionChecker(attribute)))
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
        SingleVanillaPermissionsAttribute single => new SimpleVanillaPlayerPermissionChecker(single.Permissions),
        OneOfVanillaPermissionsAttribute oneOf => new AtLeastOneVanillaPlayerPermissionChecker(oneOf.Permissions),
        CedModPermissionsAttribute cedMod => new CedModPermissionChecker(cedMod.Permission),
        _ => null
    };

    private static IPermissionChecker ResolveInstanceBasedPermissionChecker(Attribute attribute, CommandBase command) {
        if (attribute is IInstanceBasedPermissionCreator creator)
            return creator.Create(command);
        if (!attribute.GetType().TryGetGenericInterface(typeof(IGenericInstanceBasedPermissionCreator<>), out var type))
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

    private static bool ResolveBaseAttribute(Attribute attribute, ref string name, ref string description, List<string> aliases, List<string> usage, ref int minArguments) {
        var completed = false;
        if (attribute is ICommandName n) {
            name = n.Name.ToLower();
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

        return completed;
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
