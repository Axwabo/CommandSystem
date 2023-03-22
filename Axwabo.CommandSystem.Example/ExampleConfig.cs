using System.Collections.Generic;
using Axwabo.CommandSystem.Example.Resolvers;

namespace Axwabo.CommandSystem.Example;

public sealed class ExampleConfig
{

    public Dictionary<CustomCommandType, string> Permissions { get; set; } = new()
    {
        {CustomCommandType.Flash, "example.flash"},
        {CustomCommandType.SendHint, "example.sendhint"},
        {CustomCommandType.DiscordBroadcast, "example.discordbroadcast"}
    };

}
