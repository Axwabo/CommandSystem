using Axwabo.CommandSystem.Attributes.Interfaces;
using Axwabo.CommandSystem.Commands;
using Axwabo.CommandSystem.Exceptions;
using Axwabo.CommandSystem.Extensions;
using Axwabo.CommandSystem.Permissions;
using Axwabo.CommandSystem.PropertyManager.Resolvers;
using Axwabo.CommandSystem.Registration;

namespace Axwabo.CommandSystem.PropertyManager;

/// <summary>Attribute to property handler for base commands.</summary>
public static class BaseCommandPropertyManager
{

    /// <summary>The current command registration processor.</summary>
    public static CommandRegistrationProcessor CurrentProcessor { get; internal set; }

    /// <summary>
    /// Attempts to resolve the basic properties of a command.
    /// </summary>
    /// <param name="member">The member to resolve attributes for.</param>
    /// <returns>A <see cref="BaseCommandProperties"/> class containing the resolved information.</returns>
    public static BaseCommandProperties ResolveProperties(MemberInfo member)
    {
        var properties = new BaseCommandProperties();
        var aliasList = new List<string>();
        var usageList = new List<string>();
        foreach (var attribute in member.GetCustomAttributes())
        {
            ResolveBaseAttribute(member,
                attribute,
                ref properties.Name,
                ref properties.Description,
                aliasList,
                usageList,
                ref properties.MinArguments,
                ref properties.PlayerOnly
            );
            if (CurrentProcessor == null)
                continue;
            var type = attribute.GetType();
            CurrentProcessor.NameResolvers.Resolve(ref properties.Name, type, attribute);
            CurrentProcessor.DescriptionResolvers.Resolve(ref properties.Description, type, attribute);
            CurrentProcessor.AliasResolvers.ResolveArray(aliasList, type, attribute);
            CurrentProcessor.UsageResolvers.ResolveArray(usageList, type, attribute);
            CurrentProcessor.PlayerOnlyResolvers.Resolve(ref properties.PlayerOnly, type, attribute);
        }

        properties.Aliases = aliasList.ToArray();
        properties.Usage = usageList.Count == 0 ? null : usageList.ToArray();
        return properties;
    }

    /// <summary>
    /// Resolves the permission checkers for a command.
    /// </summary>
    /// <param name="command">The command to resolve permission checkers for.</param>
    /// <param name="member">The member to resolve permission checkers for. If null, the command's type will be used.</param>
    /// <returns>The permission checker for the command. If multiple were found, they will be merged into a <see cref="CombinedPermissionChecker"/>.</returns>
    public static IPermissionChecker ResolvePermissionChecker(CommandBase command, MemberInfo member = null)
    {
        var list = new List<IPermissionChecker>();
        if (member is Type)
            list.SafeCastAndAdd(command);
        foreach (var attribute in (member ?? command.GetType()).GetCustomAttributes())
        {
            if (list.AddIfNotNull(ResolveBuiltInPermissionChecker(attribute)))
                continue;
            if (!list.AddIfNotNull(ResolveInstanceBasedPermissionChecker(attribute, command)))
                list.AddIfNotNull(ResolveCustomPermissionChecker(attribute));
        }

        return list.Count switch
        {
            0 => null,
            1 => list[0],
            _ => new CombinedPermissionChecker(list.ToArray())
        };
    }

    /// <summary>
    /// Resolves a permission checker from a built-in attribute.
    /// </summary>
    /// <param name="attribute">The attribute to use.</param>
    /// <returns>The permission checker or null if it was not a built-in attribute.</returns>
    public static IPermissionChecker ResolveBuiltInPermissionChecker(Attribute attribute) => attribute switch
    {
        VanillaPermissionsAttribute single => new SimpleVanillaPlayerPermissionChecker(single.Permission),
        OneOfVanillaPermissionsAttribute oneOf => new AtLeastOneVanillaPermissionChecker(oneOf.Permissions),
        AllVanillaPermissionsAttribute all => new AllVanillaPermissionChecker(all.Permissions),
        StringPermissionsAttribute stringBased => new StringPermissionChecker(stringBased.Permission),
        _ => null
    };

    /// <summary>
    /// Resolves a command permission checker from an attribute that implements <see cref="IInstanceBasedPermissionResolver"/> or <see cref="IGenericCommandInstanceBasedPermissionResolver{TCommand}"/>.
    /// </summary>
    /// <param name="attribute">The attribute to use.</param>
    /// <param name="command">The command to pass in as an argument.</param>
    /// <returns>The permission checker or null if it was not an instance-based attribute.</returns>
    public static IPermissionChecker ResolveInstanceBasedPermissionChecker(Attribute attribute, CommandBase command)
        => attribute is IInstanceBasedPermissionResolver creator
            ? creator.CreateFromCommand(command)
            : GenericTypeExtensions.TryGetGenericInterface(attribute.GetType(), typeof(IGenericCommandInstanceBasedPermissionResolver<>), out var type)
                ? type.GetMethod("Create").InvokeIfSingleParameterMatchesType<IPermissionChecker>(attribute, command)
                : null;

    /// <summary>
    /// Resolves the permission checker from a custom attribute.
    /// </summary>
    /// <param name="attribute">The attribute to create the permission checker from.</param>
    /// <returns>The permission checker or null if no registered resolvers were found in <see cref="CurrentProcessor"/>.</returns>
    public static IPermissionChecker ResolveCustomPermissionChecker(Attribute attribute)
    {
        if (CurrentProcessor == null)
            return null;
        var attributeType = attribute.GetType();
        foreach (var creator in CurrentProcessor.PermissionCreators)
            if (creator.Takes(attributeType))
                return creator.Resolve(attribute);
        return null;
    }

    private static void ResolveBaseAttribute(
        MemberInfo member,
        Attribute attribute,
        ref string name,
        ref string description,
        List<string> aliases,
        List<string> usage,
        ref int minArguments,
        ref bool playerOnly
    )
    {
        if (attribute is ICommandName n)
            name = n.Name ?? throw new InvalidNameException($"Null command name provided by attribute {attribute.GetType().FullName} {(member is Type t ? "on type " + t.FullName : $"in member \"{member.Name}\" of type {member.DeclaringType!.FullName}")}");

        if (attribute is IDescription d)
            description = d.Description;

        if (attribute is IAliases {Aliases: { } aliasList})
            aliases.AddRange(aliasList);

        if (attribute is IUsage {Usage: { } usageList})
            usage.AddRange(usageList);

        if (attribute is IMinArguments m)
            minArguments = m.MinArguments;

        if (attribute is IPlayerOnlyAttribute)
            playerOnly = true;
    }

    internal static void Resolve<TResolver, TResult>(this IEnumerable<ResolverContainer<TResolver, TResult>> resolvers, ref TResult value, Type type, Attribute attribute)
    {
        foreach (var resolver in resolvers)
            if (resolver.Takes(type))
                value = resolver.Resolve(attribute);
    }

    internal static void ResolveArray<TResolver, TResult>(this IEnumerable<ResolverContainer<TResolver, TResult[]>> resolvers, List<TResult> list, Type type, Attribute attribute) where TResult : class
    {
        foreach (var resolver in resolvers)
            if (resolver.Takes(type))
            {
                var resolved = resolver.Resolve(attribute);
                if (resolved != null)
                    list.AddRange(resolved);
            }
    }

}
