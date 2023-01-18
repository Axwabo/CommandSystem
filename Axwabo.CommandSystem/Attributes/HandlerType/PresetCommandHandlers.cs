namespace Axwabo.CommandSystem.Attributes.HandlerType {

    public sealed class RemoteAdminCommandHandler : CommandExecutionContextAttribute {

        public RemoteAdminCommandHandler() : base(CommandHandlerTarget.RemoteAdmin) {
        }

    }

    public sealed class ServerConsoleCommandHandler : CommandExecutionContextAttribute {

        public ServerConsoleCommandHandler() : base(CommandHandlerTarget.ServerConsole) {
        }

    }

    public sealed class ClientCommandHandler : CommandExecutionContextAttribute {

        public ClientCommandHandler() : base(CommandHandlerTarget.Client) {
        }

    }

}
