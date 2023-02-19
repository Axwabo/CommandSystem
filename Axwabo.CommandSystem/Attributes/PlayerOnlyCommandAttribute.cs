using System;
using Axwabo.CommandSystem.Attributes.Interfaces;

namespace Axwabo.CommandSystem.Attributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class PlayerOnlyCommandAttribute : Attribute, IPlayerOnlyAttribute {

}
