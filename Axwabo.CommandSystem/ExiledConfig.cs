#if EXILED
using Exiled.API.Interfaces;

namespace Axwabo.CommandSystem;

/// <summary>
/// Configuration for EXILED.
/// </summary>
public sealed class Config : IConfig {

    /// <summary>Whether the plugin is enabled.</summary>
    public bool IsEnabled { get; set; } = true;

    /// <summary>Whether debug should be shown.</summary>
    public bool Debug { get; set; } = false;

    /// <summary>Whether playerID copying should be replaced with nickname copying.</summary>
    public bool CopyNicknameInsteadOfID { get; set; } = true;

    /// <summary>Whether Remote Admin extensions should be enabled.</summary>
    public bool EnableRemoteAdminExtensions { get; set; } = true;

}

#endif
