#if !EXILED
namespace Axwabo.CommandSystem;

/// <summary>
/// Configuration for the Northwood Plugin API.
/// </summary>
public sealed class Config
{

    /// <summary>Whether playerID copying should be replaced with nickname copying.</summary>
    public bool CopyNicknameInsteadOfID { get; set; } = true;

    /// <summary>Whether Remote Admin extensions should be enabled.</summary>
    public bool EnableRemoteAdminExtensions { get; set; } = true;

}

#endif
