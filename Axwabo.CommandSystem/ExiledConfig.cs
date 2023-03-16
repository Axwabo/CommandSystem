#if EXILED
using Exiled.API.Interfaces;

namespace Axwabo.CommandSystem;

/// <summary>
/// Configuration for EXILED.
/// </summary>
public sealed class Config : IConfig
{

    /// <summary>Whether the plugin is enabled.</summary>
    public bool IsEnabled { get; set; } = true;

    /// <summary>Whether debug should be shown.</summary>
    public bool Debug { get; set; } = false;

    /// <summary>Whether playerID copying should be replaced with nickname copying.</summary>
    [Description("Whether playerID copying should be replaced with nickname copying.")]
    public bool CopyNicknameInsteadOfId { get; set; } = true;

    /// <summary>Whether Remote Admin extensions should be enabled.</summary>
    [Description("Whether Remote Admin extensions should be enabled.")]
    public bool EnableRemoteAdminExtensions { get; set; } = true;
    
    /// <summary>Whether to strip intermediate language offsets from the stack trace.</summary>
    [Description("Whether to strip IL offsets from the exception stack trace.")]
    public bool StripIntermediateLanguageOffsets { get; set; } = true;

}

#endif
