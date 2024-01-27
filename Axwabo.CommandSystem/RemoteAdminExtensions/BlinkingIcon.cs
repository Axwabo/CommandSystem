namespace Axwabo.CommandSystem.RemoteAdminExtensions;

/// <summary>
/// A container for a string that will only display when the global time is even.
/// </summary>
public sealed class BlinkingIcon
{

    /// <summary>Determines whether the global time is even.</summary>
    public static bool IsGloballyActive => (int) Time.unscaledTime % 2 == 0;

    /// <summary>The content of this icon.</summary>
    public string Content { get; set; }

    /// <summary>Whether to surround the content with brackets [].</summary>
    public bool SurroundWithBrackets { get; set; }

    /// <summary>Whether to add a trailing space after the content. Defaults to true.</summary>
    public bool TrailingSpace { get; set; } = true;

    /// <summary>Whether this icon should blink. Defaults to true.</summary>
    public bool ShouldBlink { get; set; } = true;

    /// <summary>The overall color of this icon. Defaults to <see cref="Color.white"/>.</summary>
    public Color OverallColor { get; set; } = Color.white;

    /// <summary>
    /// Creates a new <see cref="BlinkingIcon"/> instance.
    /// </summary>
    /// <param name="content">The content of this icon.</param>
    /// <param name="surroundWithBrackets">Whether to surround the content with brackets []. Defaults to false.</param>
    public BlinkingIcon(string content, bool surroundWithBrackets = false)
    {
        Content = content;
        SurroundWithBrackets = surroundWithBrackets;
    }

    /// <summary>
    /// Creates a new <see cref="BlinkingIcon"/> instance.
    /// </summary>
    /// <param name="content">The content of this icon.</param>
    /// <param name="contentColor">The color of the content (excluding brackets).</param>
    /// <param name="surroundWithBrackets">Whether to surround the content with brackets []. Defaults to false.</param>
    public BlinkingIcon(string content, string contentColor, bool surroundWithBrackets = false) : this(content.Color(contentColor), surroundWithBrackets)
    {
    }

    /// <summary>
    /// Creates a new <see cref="BlinkingIcon"/> instance.
    /// </summary>
    /// <param name="content">The content of this icon.</param>
    /// <param name="overallColor">The color of the icon (including brackets).</param>
    /// <param name="surroundWithBrackets">Whether to surround the content with brackets []. Defaults to false.</param>
    public BlinkingIcon(string content, Color overallColor, bool surroundWithBrackets = false)
    {
        Content = content;
        OverallColor = overallColor;
        SurroundWithBrackets = surroundWithBrackets;
    }

    /// <inheritdoc />
    public override string ToString()
        => ShouldBlink && !IsGloballyActive
            ? ""
            : $"{(SurroundWithBrackets ? "[" : "")}{Content}{(SurroundWithBrackets ? "]" : "")}{(TrailingSpace ? " " : "")}".Color(OverallColor);

}
