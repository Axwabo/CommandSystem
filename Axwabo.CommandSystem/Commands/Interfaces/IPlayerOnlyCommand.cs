using System;
using Axwabo.CommandSystem.Structs;

namespace Axwabo.CommandSystem.Commands.Interfaces;

public interface IPlayerOnlyCommand {

    CommandResult? OnNotPlayer(ArraySegment<string> arguments, CommandSender sender);

}
