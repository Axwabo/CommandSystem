﻿using System;
using Axwabo.CommandSystem.Permissions;
using Axwabo.CommandSystem.PropertyManager.Resolvers;

namespace Axwabo.CommandSystem.Example.Resolvers;

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
        if (!ExamplePlugin.Config.Permissions.TryGetValue(attribute.Command, out var permissions))
            permissions = ""; // no permissions required
        return new StringPermissionChecker(permissions);
    }

}
