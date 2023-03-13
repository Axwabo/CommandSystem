namespace Axwabo.CommandSystem.RemoteAdminExtensions;

/// <summary>
/// The buttons in the Request Data in the Remote Admin.
/// </summary>
public enum RequestDataButton : byte
{

    /// <summary>The simple "REQUEST" button.</summary>
    BasicInfo = 0,

    /// <summary>The "REQUEST IP" button.</summary>
    RequestIP = 1,

    /// <summary>The "REQUEST AUTH" button.</summary>
    RequestAuth = 2,

    /// <summary>The "EXTERNAL LOOKUP" button.</summary>
    ExternalLookup = 3

}
