using System;
using Axwabo.CommandSystem.Attributes.Advanced.Interfaces;

namespace Axwabo.CommandSystem.Attributes.Advanced;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class AffectedMultiplePlayersMessageAttribute : Attribute, IStaticAffectedMultiplePlayersMessage {

    public string AffectedMultiple { get; }

    public AffectedMultiplePlayersMessageAttribute(string message) => AffectedMultiple = message;

}
