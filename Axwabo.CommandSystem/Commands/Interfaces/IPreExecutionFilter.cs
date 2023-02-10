using System;
using Axwabo.CommandSystem.Structs;

namespace Axwabo.CommandSystem.Commands.Interfaces;

public interface IPreExecutionFilter {

    CommandResult? OnBeforeExecuted(ArraySegment<string> arguments, CommandSender sender);

}
