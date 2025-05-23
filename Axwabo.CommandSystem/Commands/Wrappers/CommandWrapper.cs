﻿using Axwabo.CommandSystem.Exceptions;
using CommandSystem;

namespace Axwabo.CommandSystem.Commands.Wrappers;

internal sealed class CommandWrapper : ICommand, IUsageProvider
{

    private static readonly string[] MultipleChoices = ["...multiple choices"];

    internal readonly CommandBase BackingCommand;

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

        CommandResult result;
        try
        {
            result = BackingCommand.ExecuteBase(arguments, s);
        }
        catch (Exception e)
        {
            DeveloperMode.OnExceptionThrown(sender, arguments.Array, e);
            response = PlayerListProcessorException.CreateMessage(e);
            return false;
        }

        response = result.IsEmpty ? "[no response]" : result;
        return result.Success;
    }

}
