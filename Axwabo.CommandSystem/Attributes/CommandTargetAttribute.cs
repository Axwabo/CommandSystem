using System;

namespace Axwabo.CommandSystem.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
public class CommandTargetAttribute : Attribute {

    public readonly CommandHandlerType Target;

    public CommandTargetAttribute(CommandHandlerType target) => Target = target;

    public static CommandHandlerType Combine(CommandHandlerType current, CommandTargetAttribute attribute)
        => attribute == null ? current : current | attribute.Target;

}
