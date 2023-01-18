using System;

namespace Axwabo.CommandSystem.Attributes {

    [Flags]
    public enum CommandTarget : byte {

        None = 0,
        RemoteAdmin = 1,
        ServerConsole = 2,
        Client = 4,
        RaAndConsole = RemoteAdmin | ServerConsole,
        RaAndClient = RemoteAdmin | Client,
        All = RemoteAdmin | ServerConsole | Client

    }

}
