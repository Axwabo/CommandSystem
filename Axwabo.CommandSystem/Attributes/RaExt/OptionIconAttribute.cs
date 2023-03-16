using System;
using Axwabo.CommandSystem.Attributes.Interfaces;
using Axwabo.CommandSystem.RemoteAdminExtensions;
using UnityEngine;

namespace Axwabo.CommandSystem.Attributes.RaExt;

/// <summary>
/// An attribute to set the icon of a <see cref="RemoteAdminOptionBase"/>.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class OptionIconAttribute : Attribute, IOptionIconProvider
{

    /// <inheritdoc cref="BlinkingIcon.Content"/>
    public string Content { get; }

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
    /// <param name="content">The content of this icon.</param>
    public OptionIconAttribute(string content) => Content = content;

    /// <inheritdoc />
    public BlinkingIcon CreateIcon()
    {
        var icon = ContentColor != null
            ? new BlinkingIcon(Content, ContentColor, SurroundWithBrackets)
            : new BlinkingIcon(Content, SurroundWithBrackets);
        if (OverallColor != null && ColorUtility.TryParseHtmlString(OverallColor, out var color))
            icon.OverallColor = color;
        icon.TrailingSpace = TrailingSpace;
        icon.ShouldBlink = ShouldBlink;
        return icon;
    }

}
