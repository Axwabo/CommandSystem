using System;
using System.Collections.Generic;
using System.Reflection;
using Axwabo.CommandSystem.Attributes;
using Axwabo.CommandSystem.PropertyManager;
using Axwabo.CommandSystem.Registration.AttributeResolvers;
using CommandSystem;
using RemoteAdmin;
using Console = GameCore.Console;

namespace Axwabo.CommandSystem.Registration {

    public sealed class CommandRegistrationProcessor {

        #region Auto-Register

        public static void RegisterAll(object assemblyMemberInstance) => RegisterAll(assemblyMemberInstance.GetType());

        public static void RegisterAll(Type typeInAssembly)
            => new CommandRegistrationProcessor(typeInAssembly.Assembly)
                .WithRegistrationAttributesFrom(typeInAssembly)
                .Execute();

        public static void RegisterAll(Assembly assembly) => new CommandRegistrationProcessor(assembly).Execute();

        #endregion

        #region Unregister

        public static void UnregisterAll(object assemblyMemberInstance) => UnregisterAll(assemblyMemberInstance.GetType());

        public static void UnregisterAll(Type typeInAssembly) => UnregisterAll(typeInAssembly.Assembly);

        public static void UnregisterAll(Assembly assembly) {
            UnregisterFromHandler(assembly, CommandProcessor.RemoteAdminCommandHandler);
            UnregisterFromHandler(assembly, Console.singleton.ConsoleCommandHandler);
            UnregisterFromHandler(assembly, QueryProcessor.DotCommandHandler);
        }

        private static void UnregisterFromHandler(Assembly assembly, ICommandHandler handler) {
            foreach (var cmd in handler.AllCommands)
                if (cmd is CommandWrapper wrapper && wrapper.BackingCommand.GetType().Assembly == assembly)
                    handler.UnregisterCommand(cmd);
        }

        #endregion

        #region Create

        public static CommandRegistrationProcessor Create(object assemblyMemberInstance) => Create(assemblyMemberInstance.GetType().Assembly);

        public static CommandRegistrationProcessor Create(Type typeInAssembly) => Create(typeInAssembly.Assembly);

        public static CommandRegistrationProcessor Create(Assembly assembly) => new(assembly);

        #endregion

        #region Fields

        public Assembly TargetAssembly { get; }

        private CommandRegistrationProcessor(Assembly assembly) => TargetAssembly = assembly;

        internal readonly List<CommandNameResolverContainer> NameResolvers = new();

        internal readonly List<CommandDescriptionResolverContainer> DescriptionResolvers = new();

        internal readonly List<CommandPermissionCreatorContainer> PermissionCreators = new();

        #endregion

        #region Exec

        public void Execute() {
            CommandPropertyManager.CurrentProcessor = this;
            try {
                foreach (var type in TargetAssembly.GetTypes())
                    if (!type.IsAbstract && typeof(CommandBase).IsAssignableFrom(type))
                        RegisterCommand(type);
            } finally {
                CommandPropertyManager.CurrentProcessor = null;
            }
        }

        private static void RegisterCommand(Type type) {
            var targets = CommandTarget.None;
            foreach (var attr in type.GetCustomAttributes())
                if (attr is CommandListenerAttribute ctx)
                    targets = CommandListenerAttribute.Combine(targets, ctx);
            if (targets is CommandTarget.None)
                return;
            var wrapper = new CommandWrapper((CommandBase) Activator.CreateInstance(type));
            if (targets.HasFlagFast(CommandTarget.RemoteAdmin))
                CommandProcessor.RemoteAdminCommandHandler.RegisterCommand(wrapper);
            if (targets.HasFlagFast(CommandTarget.ServerConsole))
                Console.singleton.ConsoleCommandHandler.RegisterCommand(wrapper);
            if (targets.HasFlagFast(CommandTarget.Client))
                QueryProcessor.DotCommandHandler.RegisterCommand(wrapper);
        }

        #endregion

    }

}
