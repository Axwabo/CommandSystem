using System;
using CommandSystem;

namespace Axwabo.CommandSystem;

public static class CommandHelpers {

    public static string GetImplementationLocation(ICommand command)
        => GetTypeInfo(command is CommandWrapper wrapper ? wrapper.BackingCommand.GetType() : command.GetType());

    public static string GetTypeInfo(Type type) => type.Assembly.GetName().Name + ":" + type.FullName;

    public static string GetUsage(ICommand command)
        => command is not IUsageProvider {Usage: {Length: not 0} usage}
            ? ""
            : $"\nUsage:\n{string.Join("\n", usage)}";

    public static bool IsHidden(ICommand command) => command is CommandWrapper wrapper
        ? wrapper.BackingCommand is Commands.IHiddenCommand
        : command is IHiddenCommand;

}
