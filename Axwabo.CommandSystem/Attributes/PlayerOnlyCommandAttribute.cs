using System;
using Axwabo.CommandSystem.Attributes.Interfaces;

namespace Axwabo.CommandSystem.Attributes;

/// <summary>
/// Sets the command to be only usable by players.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class PlayerOnlyCommandAttribute : Attribute, IPlayerOnlyAttribute
{

}
