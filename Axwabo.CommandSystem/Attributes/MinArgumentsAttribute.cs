using System;
using Axwabo.CommandSystem.Attributes.Interfaces;

namespace Axwabo.CommandSystem.Attributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class MinArgumentsAttribute : Attribute, IMinArguments {

    public int MinArguments { get; }

    public MinArgumentsAttribute(int minArguments) => MinArguments = minArguments;

}
