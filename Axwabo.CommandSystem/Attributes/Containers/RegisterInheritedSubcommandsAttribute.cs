namespace Axwabo.CommandSystem.Attributes.Containers;

/// <summary>
/// Specifies that all superclasses of this type should be scanned for <see cref="MethodBasedSubcommandAttribute">method-based subcommands.</see>
/// This attribute is <b>not</b> inherited.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class RegisterInheritedSubcommandsAttribute : Attribute;
