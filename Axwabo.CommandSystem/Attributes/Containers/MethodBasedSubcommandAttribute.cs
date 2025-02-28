namespace Axwabo.CommandSystem.Attributes.Containers;

/// <summary>
/// Specifies that the method should be registered as a subcommand in its parent container.
/// This attribute is <b>not</b> inherited.
/// </summary>
[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public sealed class MethodBasedSubcommandAttribute : Attribute;
