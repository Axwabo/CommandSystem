#pragma warning disable CS1591
#if !EXILED
using System.ComponentModel;

namespace Axwabo.CommandSystem;

/// <summary>
/// Configuration for the Northwood Plugin API.
/// </summary>
public sealed class Config
{

    public bool Debug { get; set; } = false;

    [Description("Whether playerID copying should be replaced with nickname copying.")]
    public bool CopyNicknameInsteadOfId { get; set; } = true;

    [Description("Whether Remote Admin extensions should be enabled.")]
    public bool EnableRemoteAdminExtensions { get; set; } = true;

    [Description("Whether to strip IL offsets from the exception stack trace.")]
    public bool StripIntermediateLanguageOffsets { get; set; } = true;

    [Description("Whether to allow selecting the host player as a target using custom selectors.")]
    public bool AllowSelectingHost { get; set; } = true;

}

#endif
