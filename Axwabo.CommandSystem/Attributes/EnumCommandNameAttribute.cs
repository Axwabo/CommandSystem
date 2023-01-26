using System;
using Axwabo.CommandSystem.Attributes.Interfaces;

namespace Axwabo.CommandSystem.Attributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public abstract class EnumCommandNameAttribute : Attribute, ICommandName {

    public Enum EnumValue { get; }

    public string Name { get; }

    protected EnumCommandNameAttribute(Enum enumValue) {
        EnumValue = enumValue;
        Name = enumValue.ToString();
    }

}
