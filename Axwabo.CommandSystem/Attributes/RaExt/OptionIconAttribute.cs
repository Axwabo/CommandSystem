using Axwabo.CommandSystem.Attributes.Interfaces;
using Axwabo.CommandSystem.RemoteAdminExtensions;

namespace Axwabo.CommandSystem.Attributes.RaExt;

/// <summary>
/// An attribute to set the icon of a <see cref="RemoteAdminOptionBase"/>. This attribute is inherited.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class OptionIconAttribute : Attribute, IOptionIconProvider
{

    /// <inheritdoc cref="BlinkingIcon.Content"/>
    public string IconContent { get; init; }

    /// <inheritdoc cref="BlinkingIcon.SurroundWithBrackets"/>
    public bool SurroundWithBrackets { get; init; }

    /// <inheritdoc cref="BlinkingIcon.TrailingSpace"/>
    public bool TrailingSpace { get; init; } = true;

    /// <inheritdoc cref="BlinkingIcon.ShouldBlink"/>
    public bool ShouldBlink { get; init; } = true;

    /// <summary>The color of the content (excluding brackets).</summary>
    public string ContentColor { get; init; }

    /// <inheritdoc cref="BlinkingIcon.OverallColor"/>
    public string OverallColor { get; init; }

    /// <summary>
    /// Initializes a new <see cref="OptionIconAttribute"/> instance.
    /// </summary>
    /// <param name="iconContent">The content of this icon.</param>
    public OptionIconAttribute(string iconContent) => IconContent = iconContent;

    /// <inheritdoc />
    public BlinkingIcon CreateIcon()
    {
        if (IconContent == null)
            return null;
        var icon = ContentColor != null
            ? new BlinkingIcon(IconContent, ContentColor, SurroundWithBrackets)
            : new BlinkingIcon(IconContent, SurroundWithBrackets);
        if (OverallColor != null && ColorUtility.TryParseHtmlString(OverallColor, out var color))
            icon.OverallColor = color;
        icon.TrailingSpace = TrailingSpace;
        icon.ShouldBlink = ShouldBlink;
        return icon;
    }

}
