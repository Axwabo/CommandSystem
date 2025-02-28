using Axwabo.CommandSystem.Attributes.Interfaces;

namespace Axwabo.CommandSystem.Attributes.RaExt;

/// <summary>
/// Attribute to set the text of a option to a specified string.
/// This attribute is <b>not</b> inherited.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class StaticOptionTextAttribute : Attribute, IStaticOptionText
{

    /// <summary>The displayed text of the option.</summary>
    public string Text { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="StaticOptionTextAttribute"/> class.
    /// </summary>
    public StaticOptionTextAttribute(string text) => Text = text;

    /// <summary>
    /// Initializes a new instance of the <see cref="StaticOptionTextAttribute"/> class.
    /// </summary>
    /// <param name="text">The displayed text of the option.</param>
    /// <param name="color">The color of the text.</param>
    public StaticOptionTextAttribute(string text, string color) : this(text.Color(color))
    {
    }

}
