using Axwabo.CommandSystem.Commands.Interfaces;
using Axwabo.CommandSystem.Permissions;

namespace Axwabo.CommandSystem.Commands.Wrappers;

internal sealed class MethodBasedTargetingCommand : UnifiedTargetingCommand, IPlayerOnlyCommand
{

    private static string _nextName;

    public static void SetNextCommandName(string name) => _nextName = name;

    private readonly bool _initialized;

    private readonly string _name;

    public override string Name => _initialized ? _name : _nextName;

    public override string Description { get; }

    public override string[] Aliases { get; }

    protected override string[] UsageWithoutPlayers { get; }

    protected override int MinArgumentsWithoutTargets { get; }

    protected override IPermissionChecker Permissions { get; }

    private bool PlayerOnly { get; }

    private MethodInfo ExecuteMethod { get; }

    private ContainerCommand Container { get; }

    public MethodBasedTargetingCommand(string description, string[] aliases, string[] usage, int minArguments, IPermissionChecker permissions, bool playerOnly, MethodInfo executeMethod, ContainerCommand container)
    {
        _name = _nextName;
        Description = description;
        Aliases = aliases;
        UsageWithoutPlayers = usage;
        MinArgumentsWithoutTargets = minArguments;
        Permissions = permissions;
        ExecuteMethod = executeMethod;
        Container = container;
        PlayerOnly = playerOnly;
        _initialized = true;
    }

    protected override CommandResult ExecuteOnTargets(List<ReferenceHub> targets, ArraySegment<string> arguments, CommandSender sender)
        => (CommandResult) ExecuteMethod.Invoke(Container, new object[] {targets, arguments, sender});

    public CommandResult? OnNotPlayer(ArraySegment<string> arguments, CommandSender sender)
        => PlayerOnly ? CommandResult.Succeeded(MustBePlayer) : CommandResult.Null;

}
