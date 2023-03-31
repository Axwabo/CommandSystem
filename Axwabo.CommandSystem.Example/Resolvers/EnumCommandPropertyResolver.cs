using Axwabo.CommandSystem.Permissions;
using Axwabo.CommandSystem.PropertyManager.Resolvers;

namespace Axwabo.CommandSystem.Example.Resolvers;

internal sealed class EnumCommandPropertyResolver :
    ICommandNameResolver<EnumCommandAttribute>,
    ICommandDescriptionResolver<EnumCommandAttribute>,
    IAttributeBasedPermissionResolver<EnumCommandAttribute>
{

    private readonly ExampleConfig _config;

    public EnumCommandPropertyResolver(ExampleConfig config) => _config = config;

    public string ResolveName(EnumCommandAttribute attribute) => attribute.Command.ToString();

    public string ResolveDescription(EnumCommandAttribute attribute) => EnumCommandAttribute.GetDescription(attribute.Command);

    public IPermissionChecker CreatePermissionCheckerInstance(EnumCommandAttribute attribute)
        => new StringPermissionChecker(_config.Permissions[attribute.Command]);

}
