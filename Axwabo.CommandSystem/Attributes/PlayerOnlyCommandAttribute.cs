using Axwabo.CommandSystem.Attributes.Interfaces;

namespace Axwabo.CommandSystem.Attributes;

/// <summary>
/// Sets the command to be only usable by players. This attribute is inherited.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class PlayerOnlyCommandAttribute : Attribute, IPlayerOnlyAttribute
{

}
