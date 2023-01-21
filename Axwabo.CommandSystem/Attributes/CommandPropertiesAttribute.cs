using System;
using Axwabo.CommandSystem.Attributes.Interfaces;

namespace Axwabo.CommandSystem.Attributes {

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class CommandPropertiesAttribute : CommandListenerAttribute, ICommandName, ICommandDescription {

        public string Name { get; }

        public string Description { get; }

        public CommandPropertiesAttribute(CommandTarget target, string name, string description = null) : base(target) {
            Name = name;
            Description = description;
        }

        public CommandPropertiesAttribute(string name, string description = null) : this(CommandTarget.None, name, description) {
        }

    }

}
