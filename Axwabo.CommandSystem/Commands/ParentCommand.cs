using System;
using System.Collections.Generic;
using Axwabo.CommandSystem.Structs;

namespace Axwabo.CommandSystem.Commands;

public abstract class ParentCommand : CommandBase
{

    protected readonly List<CommandBase> Subcommands = new();

    public IReadOnlyList<CommandBase> AllSubcommands => Subcommands.AsReadOnly();

    protected internal void RegisterSubcommand<T>() where T : CommandBase, new() => RegisterSubcommand(new T());

    protected internal void RegisterSubcommand(CommandBase command)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));
        if (!TryGetSubcommand(command.Name, out var existing, true))
        {
            Subcommands.Add(command);
            return;
        }

        if (command.GetType() == existing.GetType())
            throw new InvalidOperationException($"Duplicate registration of subcommand \"{command.GetType().FullName}\" in parent command \"{GetType().FullName}\"");
        throw new InvalidOperationException($"Subcommand \"{command.GetType().FullName}\" already exists in parent command \"{GetType().FullName}\"; conflict with \"{existing.GetType().FullName}\"");
    }

    public bool TryGetSubcommand(string name, out CommandBase command, bool nameOnly = false)
    {
        if (name == null)
            throw new ArgumentNullException(nameof(name));
        CommandBase byName = null;
        CommandBase byAlias = null;
        foreach (var cmd in Subcommands)
            if (cmd.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                byName = cmd;
            else if (cmd.Aliases.Contains(name, StringComparison.OrdinalIgnoreCase) && !nameOnly)
                byAlias = cmd;
        command = byName ?? byAlias;
        return command != null;
    }

    protected sealed override int MinArguments => ParentMinArguments + 1;

    protected virtual int ParentMinArguments => base.MinArguments;

    protected override CommandResult Execute(ArraySegment<string> arguments, CommandSender sender)
        => !TryGetSubcommand(arguments.At(0), out var subcommand)
            ? ExecuteParent(arguments, sender)
            : subcommand.ExecuteBase(arguments.Segment(1), sender);

    protected virtual CommandResult ExecuteParent(ArraySegment<string> arguments, CommandSender sender) => "!Unknown subcommand!";

}
