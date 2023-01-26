using System;
using Axwabo.CommandSystem.Attributes.Interfaces;

namespace Axwabo.CommandSystem.Attributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class DescriptionAttribute : Attribute, IDescription {

    public string Description { get; }

    public DescriptionAttribute(string description) => Description = description;

}
