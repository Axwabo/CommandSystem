using System;

namespace Axwabo.CommandSystem.Attributes;

/// <summary>
/// An attribute that specifies which default command handlers a command should be registered to.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
public class CommandTargetAttribute : Attribute
{

    /// <summary>The command handler types.</summary>
    public CommandHandlerType Targets { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandTargetAttribute"/> class.
    /// </summary>
    /// <param name="targets">The command handler types.</param>
    public CommandTargetAttribute(CommandHandlerType targets) => Targets = targets;

    /// <summary>
    /// Combines the current command handler type with the one from the attribute.
    /// </summary>
    /// <param name="current">The command handler type to combine the attribute with.</param>
    /// <param name="attribute">The attribute to combine.</param>
    /// <returns>The combination of the two types.</returns>
    public static CommandHandlerType Combine(CommandHandlerType current, CommandTargetAttribute attribute)
        => attribute == null ? current : current | attribute.Targets;

}
