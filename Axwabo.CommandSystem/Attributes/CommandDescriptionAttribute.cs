using System;
using Axwabo.CommandSystem.Attributes.Interfaces;

namespace Axwabo.CommandSystem.Attributes {

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class CommandDescriptionAttribute : Attribute, ICommandDescription {

        public string Description { get; }

        public CommandDescriptionAttribute(string description) => Description = description;

    }

}
