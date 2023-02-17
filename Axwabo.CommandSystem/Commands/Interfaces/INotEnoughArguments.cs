using System;
using Axwabo.CommandSystem.Structs;

namespace Axwabo.CommandSystem.Commands.Interfaces;

public interface INotEnoughArguments {

    CommandResult OnNotEnoughArgumentsProvided(ArraySegment<string> arguments, CommandSender sender);

}
