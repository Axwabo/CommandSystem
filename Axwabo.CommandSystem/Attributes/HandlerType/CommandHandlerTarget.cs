using System;

namespace Axwabo.CommandSystem.Attributes.HandlerType {

    [Flags]
    public enum CommandHandlerTarget : byte {

        None = 0,
        RemoteAdmin = 1,
        ServerConsole = 2,
        Client = 4

    }

}
