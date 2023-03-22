using System.Collections.Generic;
using Axwabo.CommandSystem.Attributes.RaExt;
using Axwabo.CommandSystem.RemoteAdminExtensions;
using RemoteAdmin;

namespace Axwabo.CommandSystem.Example;

[RemoteAdminOptionProperties("counter", "Counter", "yellow")]
public sealed class CounterOption : RemoteAdminOptionBase
{

    private static readonly Dictionary<PlayerCommandSender, int> Counter = new();

    protected override string HandleButtonClick(RequestDataButton button, PlayerCommandSender sender)
    {
        switch (button)
        {
            case RequestDataButton.BasicInfo:
                return Count(sender, -1);
            case RequestDataButton.RequestIP:
                return Count(sender, 1);
            case RequestDataButton.RequestAuth:
                Counter.Remove(sender);
                return "Counter has been reset";
            case RequestDataButton.ExternalLookup:
                Counter[sender] = 69420;
                return "Nice";
            default:
                return null;
        }
    }

    protected override string GenerateDisplayText(CommandSender sender)
    {
        var count = Counter.TryGetValue((PlayerCommandSender) sender, out var value) ? value : 0;
        return "Counter: " + count;
    }

    private static string Count(PlayerCommandSender sender, int amount)
    {
        if (Counter.TryGetValue(sender, out var count))
            count += amount;
        else
            count = 0;
        Counter[sender] = count;
        return count.ToString();
    }

}
