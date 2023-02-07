using System;
using Axwabo.CommandSystem.Attributes.Advanced.Interfaces;

namespace Axwabo.CommandSystem.Attributes.Advanced;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class NoTargetsMessageAttribute : Attribute, IStaticNoTargetsFoundMessage {

    public string NoTargets { get; }

    public NoTargetsMessageAttribute(string message) => NoTargets = message;

}
