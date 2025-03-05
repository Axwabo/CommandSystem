using System;
using Axwabo.CommandSystem.Permissions;
using Axwabo.CommandSystem.PropertyManager.Resolvers;

namespace Axwabo.CommandSystem.Example.Resolvers;

[AttributeUsage(AttributeTargets.Class)]
internal sealed class EnumCommandPropertyResolver :
    Attribute,
    ICommandNameResolver<EnumCommandAttribute>,
    ICommandDescriptionResolver<EnumCommandAttribute>,
    IAttributeBasedPermissionResolver<EnumCommandAttribute>
{

    public string ResolveName(EnumCommandAttribute attribute) => attribute.Command.ToString();

    public string ResolveDescription(EnumCommandAttribute attribute) => EnumCommandAttribute.GetDescription(attribute.Command);

    public IPermissionChecker CreatePermissionCheckerInstance(EnumCommandAttribute attribute)
    {
        var config = ExamplePlugin.Instance?.Config;
        if (config == null || !config.Permissions.TryGetValue(attribute.Command, out var permissions))
            permissions = ""; // no permissions required
        return new StringPermissionChecker(permissions);
    }

}
