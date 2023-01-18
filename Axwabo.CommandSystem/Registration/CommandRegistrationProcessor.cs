using System;
using System.Reflection;

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
            // TODO: Implement
        }

    }

}
