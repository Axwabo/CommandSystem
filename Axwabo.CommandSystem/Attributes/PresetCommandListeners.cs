namespace Axwabo.CommandSystem.Attributes;

/// <summary>
/// Registers the command in the <see cref="global::CommandSystem.RemoteAdminCommandHandler"/>, making it accessible from the Text-Based Remote Admin panel. 
/// </summary>
public sealed class RemoteAdminCommand : CommandTargetAttribute
{

    /// <summary>
    /// Initializes a new instance of the <see cref="RemoteAdminCommand"/> class.
    /// </summary>
    public RemoteAdminCommand() : base(CommandHandlerType.RemoteAdmin)
    {
    }

}

/// <summary>
/// Registers the command in the <see cref="global::CommandSystem.GameConsoleCommandHandler"/>, making it accessible from the server console. 
/// </summary>
public sealed class ServerCommand : CommandTargetAttribute
{

    /// <summary>
    /// Initializes a new instance of the <see cref="ServerCommand"/> class.
    /// </summary>
    public ServerCommand() : base(CommandHandlerType.ServerConsole)
    {
    }

}

/// <summary>
/// Registers the command in the <see cref="global::CommandSystem.ClientCommandHandler"/>, making it accessible from the client (player) console. 
/// </summary>
public sealed class ClientCommand : CommandTargetAttribute
{

    /// <summary>
    /// Initializes a new instance of the <see cref="ClientCommand"/> class.
    /// </summary>
    public ClientCommand() : base(CommandHandlerType.Client)
    {
    }

}
