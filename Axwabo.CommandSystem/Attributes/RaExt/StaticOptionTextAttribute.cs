using System;
using Axwabo.CommandSystem.Attributes.Interfaces;

namespace Axwabo.CommandSystem.Attributes.RaExt;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class StaticOptionTextAttribute : Attribute, IStaticOptionText {

    public string Text { get; }

    public StaticOptionTextAttribute(string text) => Text = text;

}
