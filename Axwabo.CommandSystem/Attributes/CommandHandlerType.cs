using System;

namespace Axwabo.CommandSystem.Attributes;

[Flags]
public enum CommandHandlerType : byte {

    None = 0,
    RemoteAdmin = 1,
    ServerConsole = 2,
    Client = 4,
    RaAndServer = RemoteAdmin | ServerConsole,
    RaAndClient = RemoteAdmin | Client,
    All = RemoteAdmin | ServerConsole | Client

}
