using Axwabo.CommandSystem.Permissions;

namespace Axwabo.CommandSystem.Commands.Wrappers;

internal sealed class MethodBasedTargetingCommand : UnifiedTargetingCommand, IMethodBasedCommand
{

    protected override IPermissionChecker Permissions { get; }

    public ContainerCommand Container { get; }

    public MethodInfo ExecuteMethod { get; }

    public MethodBasedTargetingCommand(BaseCommandProperties properties, IPermissionChecker permissions, MethodInfo executeMethod, ContainerCommand container) : base(properties)
    {
        Permissions = permissions;
        ExecuteMethod = executeMethod;
        Container = container;
    }

    protected override CommandResult ExecuteOnTargets(List<ReferenceHub> targets, ArraySegment<string> arguments, CommandSender sender)
        => (CommandResult) ExecuteMethod.Invoke(Container, [targets, arguments, sender]);

}
