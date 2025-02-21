using Axwabo.CommandSystem.Permissions;

namespace Axwabo.CommandSystem.Commands.Wrappers;

internal sealed class MethodBasedCommand : CommandBase, IMethodBasedCommand
{

    protected override IPermissionChecker Permissions { get; }

    public ContainerCommand Container { get; }

    public MethodInfo ExecuteMethod { get; }

    public MethodBasedCommand(BaseCommandProperties properties, IPermissionChecker permissions, MethodInfo executeMethod, ContainerCommand container) : base(properties)
    {
        Permissions = permissions;
        ExecuteMethod = executeMethod;
        Container = container;
    }

    protected override CommandResult Execute(ArraySegment<string> arguments, CommandSender sender)
        => (CommandResult) ExecuteMethod.Invoke(Container, [arguments, sender]);

}
