using System;
using Axwabo.CommandSystem.Attributes;

namespace Axwabo.CommandSystem.Example.Translations;

[CommandProperties(CommandHandlerType.All, "greet", 1, "Responds with a random greeting.")]
public sealed class GreetCommand : CommandBase
{

    private static readonly GreetingType[] AllTypes = (GreetingType[]) Enum.GetValues(typeof(GreetingType));

    protected override CommandResult Execute(ArraySegment<string> arguments, CommandSender sender)
    {
        var name = string.Join(" ", arguments);
        // a tuple is automatically converted to a CommandResult
        // the name might be used in some translations
        return (AllTypes.RandomItem(), name);
    }

}
