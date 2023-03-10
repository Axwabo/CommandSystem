using System;

namespace Axwabo.CommandSystem.Attributes;

/// <summary>
/// Defines the types of default command handlers.
/// </summary>
[Flags]
public enum CommandHandlerType : byte {

    /// <summary>No command handler.</summary>
    None = 0,

    /// <summary>A command in the Remote Admin.</summary>
    RemoteAdmin = 1,

    /// <summary>A command in the Server Console.</summary>
    ServerConsole = 2,

    /// <summary>A command in the Client Console.</summary>
    Client = 4,

    /// <summary>A command in the Remote Admin and the Server Console.</summary>
    RaAndServer = RemoteAdmin | ServerConsole,

    /// <summary>A command in the Remote Admin and the Client Console.</summary>
    RaAndClient = RemoteAdmin | Client,

    /// <summary>A command present in all command handlers.</summary>
    All = RemoteAdmin | ServerConsole | Client

}
