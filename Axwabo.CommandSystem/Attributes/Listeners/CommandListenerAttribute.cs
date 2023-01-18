using System;

namespace Axwabo.CommandSystem.Attributes.Listeners {

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class CommandListenerAttribute : Attribute {

        public readonly CommandTarget Target;

        public CommandListenerAttribute(CommandTarget target) => Target = target;

        public static CommandTarget Combine(CommandTarget current, CommandListenerAttribute attribute)
            => attribute == null ? current : current | attribute.Target;

    }

}
