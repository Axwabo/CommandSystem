using System;
using CommandSystem;

namespace Axwabo.CommandSystem {

    internal sealed class CommandWrapper : ICommand, IUsageProvider {

        internal readonly CommandBase BackingCommand;

        public CommandWrapper(CommandBase backingCommand) => BackingCommand = backingCommand;

        public string Command => BackingCommand.Name;

        public string[] Aliases => BackingCommand.Aliases;

        public string Description => BackingCommand.Description;

        public string[] Usage => BackingCommand.Usage;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response) {
            var result = BackingCommand.Execute(arguments, sender);
            response = result.Response;
            return result.Success;
        }

    }

}
