namespace Axwabo.CommandSystem.Attributes;

public sealed class RemoteAdminCommand : CommandTargetAttribute {

    public RemoteAdminCommand() : base(CommandHandlerType.RemoteAdmin) {
    }

}

public sealed class ServerCommand : CommandTargetAttribute {

    public ServerCommand() : base(CommandHandlerType.ServerConsole) {
    }

}

public sealed class ClientCommand : CommandTargetAttribute {

    public ClientCommand() : base(CommandHandlerType.Client) {
    }

}
