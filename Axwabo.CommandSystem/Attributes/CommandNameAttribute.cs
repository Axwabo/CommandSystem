using System;
using Axwabo.CommandSystem.Attributes.Interfaces;

namespace Axwabo.CommandSystem.Attributes {

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class CommandNameAttribute : Attribute, ICommandName {

        public string Name { get; }

        public CommandNameAttribute(string name) => Name = !string.IsNullOrEmpty(name) ? name : throw new ArgumentNullException(name);

    }

}
