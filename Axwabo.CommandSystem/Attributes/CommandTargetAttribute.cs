using System;

namespace Axwabo.CommandSystem.Attributes;

/// <summary>
/// An attribute that specifies which default command handlers a command should be registered to.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
public class CommandTargetAttribute : Attribute {

    /// <summary>The command handler type.</summary>
    public readonly CommandHandlerType Target;

    /// <summary>
    /// Creates a new <see cref="CommandTargetAttribute"/>.
    /// </summary>
    /// <param name="target">The command handler type.</param>
    public CommandTargetAttribute(CommandHandlerType target) => Target = target;

    /// <summary>
    /// Combines the current command handler type with the one from the attribute.
    /// </summary>
    /// <param name="current">The command handler type to combine the attribute with.</param>
    /// <param name="attribute">The attribute to combine.</param>
    /// <returns>The combination of the two types.</returns>
    public static CommandHandlerType Combine(CommandHandlerType current, CommandTargetAttribute attribute)
        => attribute == null ? current : current | attribute.Target;

}
