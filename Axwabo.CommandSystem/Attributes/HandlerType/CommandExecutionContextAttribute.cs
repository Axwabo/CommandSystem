using System;

namespace Axwabo.CommandSystem.Attributes.HandlerType {

    public class CommandExecutionContextAttribute : Attribute {

        public readonly CommandHandlerTarget Target;

        public CommandExecutionContextAttribute(CommandHandlerTarget target) => Target = target;

        public static CommandHandlerTarget Combine(CommandHandlerTarget current, CommandExecutionContextAttribute attribute) 
            => attribute == null ? current : current | attribute.Target;

    }

}
