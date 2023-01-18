using System;
using System.Reflection;
using Axwabo.CommandSystem.Attributes.Listeners;
using RemoteAdmin;

namespace Axwabo.CommandSystem.Registration {

    public sealed class CommandRegistrationProcessor {

        public static void RegisterAll(object assemblyMemberInstance) => RegisterAll(assemblyMemberInstance.GetType());

        public static void RegisterAll(Type typeInAssembly)
            => new CommandRegistrationProcessor(typeInAssembly.Assembly)
                .WithRegistrationAttributesFrom(typeInAssembly)
                .Execute();

        public static void RegisterAll(Assembly assembly) => new CommandRegistrationProcessor(assembly).Execute();

        public static CommandRegistrationProcessor Create(object assemblyMemberInstance) => Create(assemblyMemberInstance.GetType().Assembly);

        public static CommandRegistrationProcessor Create(Type typeInAssembly) => Create(typeInAssembly.Assembly);

        public static CommandRegistrationProcessor Create(Assembly assembly) => new(assembly);

        public Assembly TargetAssembly { get; }

        private CommandRegistrationProcessor(Assembly assembly) => TargetAssembly = assembly;

        public void Execute() {
            foreach (var type in TargetAssembly.GetTypes())
                if (!type.IsAbstract && typeof(CommandBase).IsAssignableFrom(type))
                    RegisterCommand(type);
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
                GameCore.Console.singleton.ConsoleCommandHandler.RegisterCommand(wrapper);
            if (targets.HasFlagFast(CommandTarget.Client))
                QueryProcessor.DotCommandHandler.RegisterCommand(wrapper);
        }

    }

}
