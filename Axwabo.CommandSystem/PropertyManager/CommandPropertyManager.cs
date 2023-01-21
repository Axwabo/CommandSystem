using System;
using System.Reflection;
using Axwabo.CommandSystem.Attributes.Interfaces;
using Axwabo.CommandSystem.Exceptions;
using Axwabo.CommandSystem.Permissions;
using Axwabo.CommandSystem.Registration;

namespace Axwabo.CommandSystem.PropertyManager {

    public static class CommandPropertyManager {

        public static CommandRegistrationProcessor CurrentProcessor { get; internal set; }

        public static bool TryResolveProperties(CommandBase command, out string name, out string description) {
            if (CurrentProcessor == null)
                throw new AttributeResolverException("Attempted to resolve command properties outside of a registration process.");
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            name = null;
            description = null;
            foreach (var attribute in command.GetType().GetCustomAttributes()) {
                if (ResolveBaseAttribute(attribute, ref name, ref description))
                    continue;
                var type = attribute.GetType();
                ResolveName(ref name, type, attribute);
                ResolveDescription(ref description, type, attribute);
            }

            return !string.IsNullOrEmpty(name);
        }

        public static IPermissionChecker ResolvePermissionChecker(CommandBase command) {
            if (CurrentProcessor == null)
                return null;
            foreach (var attribute in command.GetType().GetCustomAttributes()) {
                if (attribute is VanillaPermissionsAttribute vanilla)
                    return new SimpleVanillaPlayerPermissionChecker(vanilla.Permissions);
                if (CurrentProcessor.PermissionResolvers.TryGetValue(attribute.GetType(), out var creator))
                    return creator.CreatePermissionCheckerInstance(attribute);
            }

            return null;
        }

        private static bool ResolveBaseAttribute(Attribute attribute, ref string name, ref string description) {
            var success = false;
            if (attribute is ICommandName n) {
                name = n.Name.ToLower();
                success = true;
            }

            if (attribute is ICommandDescription d) {
                description = d.Description;
                success = true;
            }

            return success;
        }

        private static void ResolveName(ref string name, Type type, Attribute attribute) {
            if (CurrentProcessor.NameResolvers.TryGetValue(type, out var nameResolver))
                name = nameResolver.ResolveName(attribute);
        }

        private static void ResolveDescription(ref string description, Type type, Attribute attribute) {
            if (CurrentProcessor.DescriptionResolvers.TryGetValue(type, out var descriptionResolver))
                description = descriptionResolver.ResolveDescription(attribute);
        }

    }

}
