using System;
using Axwabo.CommandSystem.Attributes.Interfaces;

namespace Axwabo.CommandSystem.Attributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class CommandPropertiesAttribute : CommandTargetAttribute, ICommandName, IDescription, IAliases, IMinArguments {

    public string Name { get; }

    public string Description { get; }

    public string[] Aliases { get; }

    public int MinArguments { get; }

    public CommandPropertiesAttribute(CommandHandlerType target, string name, string description = null, params string[] aliases) : base(target) {
        Name = name;
        Description = description;
        Aliases = aliases;
    }

    public CommandPropertiesAttribute(CommandHandlerType target, string name, int minArguments, string description = null, params string[] aliases) : base(target) {
        Name = name;
        Description = description;
        Aliases = aliases;
        MinArguments = minArguments;
    }

    public CommandPropertiesAttribute(string name, string description = null, params string[] aliases) : this(CommandHandlerType.None, name, description, aliases) {
    }

    public CommandPropertiesAttribute(string name, int minArguments, string description = null, params string[] aliases) : this(CommandHandlerType.None, name, minArguments, description, aliases) {
    }

}
