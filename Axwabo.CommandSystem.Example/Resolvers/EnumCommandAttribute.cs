using Axwabo.CommandSystem.Attributes;

namespace Axwabo.CommandSystem.Example.Resolvers;

internal sealed class EnumCommandAttribute : CommandTargetAttribute
{

    public CustomCommandType Command { get; }

    public EnumCommandAttribute(CustomCommandType command) : base(CommandHandlerType.RemoteAdmin) => Command = command;

    public static string GetDescription(CustomCommandType command) => command switch
    {
        CustomCommandType.Flash => "Adds a flash effect to the specified players",
        CustomCommandType.DiscordBroadcast => "Broadcasts a message to join the Discord server",
        CustomCommandType.SendHint => "Sends a hint to the specified players",
        _ => null
    };

}
