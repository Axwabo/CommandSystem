using Axwabo.CommandSystem.Commands.Interfaces;
using Axwabo.CommandSystem.Permissions;

namespace Axwabo.CommandSystem.Commands.Wrappers;

internal sealed class MethodBasedCommand : CommandBase, IPlayerOnlyCommand
{

    private static string _nextName;

    public static void SetNextCommandName(string name) => _nextName = name;

    private readonly bool _initialized;

    private readonly string _name;

    public override string Name => _initialized ? _name : _nextName;

    public override string Description { get; }

    public override string[] Aliases { get; }

    public override string[] Usage { get; }

    protected override int MinArguments { get; }

    protected override IPermissionChecker Permissions { get; }

    private bool PlayerOnly { get; }

    private MethodInfo ExecuteMethod { get; }

    private ContainerCommand Container { get; }

    public MethodBasedCommand(string description, string[] aliases, string[] usage, int minArguments, IPermissionChecker permissions, bool playerOnly, MethodInfo executeMethod, ContainerCommand container)
    {
        _name = _nextName;
        Description = description;
        Aliases = aliases;
        Usage = usage;
        MinArguments = minArguments;
        Permissions = permissions;
        ExecuteMethod = executeMethod;
        Container = container;
        PlayerOnly = playerOnly;
        _initialized = true;
    }

    protected override CommandResult Execute(ArraySegment<string> arguments, CommandSender sender)
        => (CommandResult) ExecuteMethod.Invoke(Container, new object[] {arguments, sender});

    public CommandResult? OnNotPlayer(ArraySegment<string> arguments, CommandSender sender)
        => PlayerOnly ? CommandResult.Succeeded(MustBePlayer) : CommandResult.Null;

}
