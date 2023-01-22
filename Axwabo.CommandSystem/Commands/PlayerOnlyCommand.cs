using System;
using RemoteAdmin;

namespace Axwabo.CommandSystem.Commands {

    public abstract class PlayerOnlyCommand : CommandBase {

        protected sealed override CommandResult Execute(ArraySegment<string> arguments, CommandSender sender)
            => sender is not PlayerCommandSender player
                ? "!Only a player can execute this command."
                : ExecuteAsPlayer(arguments, player);

        protected abstract CommandResult ExecuteAsPlayer(ArraySegment<string> arguments, PlayerCommandSender sender);

    }

}
