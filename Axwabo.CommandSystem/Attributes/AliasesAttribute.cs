using System;
using Axwabo.CommandSystem.Attributes.Interfaces;

namespace Axwabo.CommandSystem.Attributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class AliasesAttribute : Attribute, IAliases {

    public string[] Aliases { get; }

    public AliasesAttribute(params string[] aliases) => Aliases = aliases;

}
