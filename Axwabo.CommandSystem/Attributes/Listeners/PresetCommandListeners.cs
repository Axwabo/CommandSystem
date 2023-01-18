namespace Axwabo.CommandSystem.Attributes.Listeners {

    public sealed class RemoteAdminCommand : CommandListenerAttribute {

        public RemoteAdminCommand() : base(CommandTarget.RemoteAdmin) {
        }

    }

    public sealed class ServerConsoleCommand : CommandListenerAttribute {

        public ServerConsoleCommand() : base(CommandTarget.ServerConsole) {
        }

    }

    public sealed class ClientCommand : CommandListenerAttribute {

        public ClientCommand() : base(CommandTarget.Client) {
        }

    }

}
