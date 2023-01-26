﻿using System;
using Axwabo.CommandSystem.Patches;
using CommandSystem;
using PluginAPI.Core;

namespace Axwabo.CommandSystem;

internal sealed class CommandWrapper : ICommand, IUsageProvider {

    internal readonly CommandBase BackingCommand;

    public CommandWrapper(CommandBase backingCommand) => BackingCommand = backingCommand;

    public string Command => BackingCommand.Name;

    public string[] Aliases => BackingCommand.Aliases;

    public string Description => BackingCommand.Description;

    public string[] Usage => BackingCommand.Usage;

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response) {
        if (sender is not CommandSender s) {
            response = $"FATAL ERROR: sender is not a CommandSender, are you implementing the interface yourself?\n{CommandImplementationPatch.GetTypeInfo(GetType())}";
            Log.Error($"Could not execute command {Command}:\n{response}");
            return false;
        }

        var result = BackingCommand.ExecuteBase(arguments, s);
        response = result.IsEmpty ? "[no response]" : result.Response;
        return result.Success;
    }

}
