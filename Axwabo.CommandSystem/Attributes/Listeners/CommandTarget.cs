using System;

namespace Axwabo.CommandSystem.Attributes.Listeners {

    [Flags]
    public enum CommandTarget : byte {

        None = 0,
        RemoteAdmin = 1,
        ServerConsole = 2,
        Client = 4,
        All = RemoteAdmin | ServerConsole | Client

    }

}
