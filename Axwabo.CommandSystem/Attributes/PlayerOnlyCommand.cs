using System;

namespace Axwabo.CommandSystem.Attributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class PlayerOnlyCommand : Attribute {

}
