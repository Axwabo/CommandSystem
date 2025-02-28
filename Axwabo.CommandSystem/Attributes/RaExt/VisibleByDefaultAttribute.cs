namespace Axwabo.CommandSystem.Attributes.RaExt;

/// <summary>
/// Specifies that the Remote Admin option should be visible to all users by default.
/// This attribute is <b>not</b> inherited.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class VisibleByDefaultAttribute : Attribute;
