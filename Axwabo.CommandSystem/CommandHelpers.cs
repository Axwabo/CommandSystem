using System;
using CommandSystem;

namespace Axwabo.CommandSystem;

public static class CommandHelpers {

    public static string GetImplementationLocation(ICommand command)
        => GetTypeInfo(command is CommandWrapper wrapper ? wrapper.BackingCommand.GetType() : command.GetType());

    public static string GetTypeInfo(Type type) => type.Assembly.GetName().Name + ":" + type.FullName;

    public static string GetUsage(ICommand command) => command switch {
        CommandWrapper {BackingCommand: var bc} => bc.Usage is {Length: not 0} ? "\n" + bc.CombinedUsage : "",
        IUsageProvider {Usage.Length: not 0} usage => $"\nUsage: {command.Command} {usage.DisplayCommandUsage()}",
        _ => ""
    };

    public static bool IsHidden(ICommand command) => command is CommandWrapper wrapper
        ? wrapper.BackingCommand is Commands.Interfaces.IHiddenCommand
        : command is IHiddenCommand;

}
