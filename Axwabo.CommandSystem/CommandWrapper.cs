#if EXILED
using Exiled.API.Features;
#else
using PluginAPI.Core;
#endif
using System;
using CommandSystem;

namespace Axwabo.CommandSystem;

internal sealed class CommandWrapper : ICommand, IUsageProvider
{

    private static readonly string[] MultipleChoices = {"...multiple choices"};

    public readonly CommandBase BackingCommand;

    public CommandWrapper(CommandBase backingCommand) => BackingCommand = backingCommand;

    public string Command => BackingCommand.Name;

    public string[] Aliases => BackingCommand.Aliases;

    public string Description => BackingCommand.Description;

    public string[] Usage
    {
        get
        {
            var array = BackingCommand.Usage;
            return array is not {Length: not 0}
                ? null
                : array.Length switch
                {
                    1 => array[0].Split(),
                    _ => MultipleChoices
                };
        }
    }

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (sender is not CommandSender s)
        {
            response = $"FATAL ERROR: sender is not a CommandSender, are you implementing the interface yourself?\n{CommandHelpers.GetTypeInfo(GetType())}";
            Log.Error($"Could not execute command {Command}:\n{response}");
            return false;
        }

        var result = BackingCommand.ExecuteBase(arguments, s);
        response = result.IsEmpty ? "[no response]" : result;
        return result.Success;
    }

}
