using System;
using Axwabo.CommandSystem.Attributes;
using Axwabo.CommandSystem.Structs;

namespace Axwabo.CommandSystem.Example;

[CommandProperties(CommandHandlerType.RemoteAdmin, "simpleCommand", "Simple description", "simpleAlias")]
[PlayerOnlyCommand] // only players can execute this command
public sealed class SimpleCommand : CommandBase
{

    protected override CommandResult Execute(ArraySegment<string> arguments, CommandSender sender)
    {
        return "I am a simple command!";
    }

}
