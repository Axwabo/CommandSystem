using Axwabo.CommandSystem.Attributes.Interfaces;
using Axwabo.CommandSystem.Attributes.RaExt;
using Axwabo.CommandSystem.Exceptions;
using Axwabo.CommandSystem.Permissions;
using Axwabo.CommandSystem.RemoteAdminExtensions;
using Axwabo.CommandSystem.RemoteAdminExtensions.Interfaces;

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
    /// <param name="icon">The icon of the option.</param>
    /// <param name="isVisibleByDefault">Whether the option is visible to users by default.</param>
    /// <param name="canBeUsedAsStandaloneSelector">Whether the option can be used as a standalone selector.</param>
    /// <returns>Whether the option id was successfully resolved.</returns>
    public static bool TryResolveProperties(RemoteAdminOptionBase option, out string id, out string staticText, out BlinkingIcon icon, out bool isVisibleByDefault, out bool canBeUsedAsStandaloneSelector)
    {
        id = null;
        staticText = null;
        icon = option is IOptionIconProvider provider ? provider.CreateIcon() : null;
        isVisibleByDefault = false;
        canBeUsedAsStandaloneSelector = false;
        foreach (var attribute in option.GetType().GetCustomAttributes())
        {
            ResolveBaseAttribute(option, attribute, ref id, ref staticText, ref icon, ref canBeUsedAsStandaloneSelector, ref isVisibleByDefault);
            if (BaseCommandPropertyManager.CurrentProcessor == null)
                continue;
            var type = attribute.GetType();
            BaseCommandPropertyManager.CurrentProcessor.RemoteAdminOptionIdResolvers.Resolve(ref id, type, attribute);
            BaseCommandPropertyManager.CurrentProcessor.StaticOptionTextResolvers.Resolve(ref staticText, type, attribute);
            BaseCommandPropertyManager.CurrentProcessor.OptionIconResolvers.Resolve(ref icon, type, attribute);
        }

        return RemoteAdminOptionManager.IsValidOptionId(id);
    }

    private static void ResolveBaseAttribute(
        RemoteAdminOptionBase option,
        Attribute attribute,
        ref string id,
        ref string staticText,
        ref BlinkingIcon icon,
        ref bool canBeUsedAsStandaloneSelector,
        ref bool isVisibleByDefault
    )
    {
        if (attribute is VisibleByDefaultAttribute)
        {
            isVisibleByDefault = true;
            return;
        }

        if (attribute is IRemoteAdminOptionIdentifier identifier)
            id = identifier.Id ?? throw new InvalidNameException($"Null option identifier provided by attribute {attribute.GetType().FullName} on type {option.GetType().FullName}.");

        if (attribute is IStaticOptionText text)
            text.Text.SetFieldIfNotNull(ref staticText);

        if (attribute is IOptionIconProvider iconProvider)
            iconProvider.CreateIcon().SetFieldIfNotNull(ref icon);

        if (attribute is IStandaloneSelectorOption {CanBeUsedAsStandaloneSelector: true})
            canBeUsedAsStandaloneSelector = true;
    }

    /// <summary>
    /// Resolves a RA option permission checker from an attribute that implements <see cref="IInstanceBasedPermissionResolver"/> or <see cref="IGenericRemoteAdminOptionInstanceBasedPermissionResolver{TOption}"/>.
    /// </summary>
    /// <param name="attribute">The attribute to use.</param>
    /// <param name="option">The option to pass in as an argument.</param>
    /// <returns>The permission checker or null if it was not an instance-based attribute.</returns>
    public static IPermissionChecker ResolveInstanceBasedPermissionChecker(Attribute attribute, RemoteAdminOptionBase option)
        => attribute is IInstanceBasedPermissionResolver creator
            ? creator.CreateFromOption(option)
            : GenericTypeExtensions.TryGetGenericInterface(attribute.GetType(), typeof(IGenericRemoteAdminOptionInstanceBasedPermissionResolver<>), out var type)
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
