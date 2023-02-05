using System;
using Axwabo.CommandSystem.Attributes.Advanced.Interfaces;

namespace Axwabo.CommandSystem.Attributes.Advanced;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class AffectedOnePlayerMessageAttribute : Attribute, IStaticAffectedOnePlayerMessage {

    public string Message { get; }

    public AffectedOnePlayerMessageAttribute(string message) => Message = message;

}
