using System;

namespace Axwabo.CommandSystem.Example.Resolvers;

internal sealed class EnumCommandAttribute : Attribute
{

    public CustomCommandType Command { get; }

    public EnumCommandAttribute(CustomCommandType command) => Command = command;

    public static string GetDescription(CustomCommandType command) => throw new NotImplementedException();

}
