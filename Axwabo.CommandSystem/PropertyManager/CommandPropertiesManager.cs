using System;
using System.Reflection;
using Axwabo.CommandSystem.Attributes.Interfaces;
using Axwabo.CommandSystem.Registration;

namespace Axwabo.CommandSystem.PropertyManager {

    public static class CommandPropertiesManager {

        public static CommandRegistrationProcessor CurrentProcessor { get; internal set; }

        public static bool TryResolveBasicProperties(CommandBase command, out string name) {
            if (CurrentProcessor == null)
                throw new InvalidOperationException("Attempted to resolve command properties outside of a registration process.");
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            name = null;
            foreach (var attribute in command.GetType().GetCustomAttributes()) {
                if (ResolveBaseAttribute(attribute, ref name))
                    continue;
                var type = attribute.GetType();
                ResolveName(ref name, type, attribute);
            }

            return name != null;
        }

        private static bool ResolveBaseAttribute(Attribute attribute, ref string name) {
            var success = false;
            if (attribute is ICommandName n) {
                name = n.Name;
                success = true;
            }

            return success;
        }

        private static void ResolveName(ref string name, Type type, Attribute attribute) {
            if (CurrentProcessor.NameResolvers.TryGetValue(type, out var nameResolver))
                name = nameResolver(attribute);
        }

    }

}
