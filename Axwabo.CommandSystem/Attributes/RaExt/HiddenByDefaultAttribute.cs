using System;

namespace Axwabo.CommandSystem.Attributes.RaExt;

/// <summary>
/// Specifies that the Remote Admin option should be hidden for all users by default.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class HiddenByDefaultAttribute : Attribute
{

}
