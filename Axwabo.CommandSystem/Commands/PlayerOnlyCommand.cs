using System;
using Axwabo.CommandSystem.Structs;
using RemoteAdmin;

namespace Axwabo.CommandSystem.Commands;

public abstract class PlayerOnlyCommand : CommandBase {

    protected virtual string FailureNonPlayer => "Only a player can execute this command.";

    protected sealed override CommandResult Execute(ArraySegment<string> arguments, CommandSender sender)
        => sender is not PlayerCommandSender player
            ? CommandResult.Failed(FailureNonPlayer)
            : ExecuteAsPlayer(arguments, player.ReferenceHub);

    protected abstract CommandResult ExecuteAsPlayer(ArraySegment<string> arguments, ReferenceHub sender);

}
