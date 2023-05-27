using Axwabo.CommandSystem.Attributes.Interfaces;

namespace Axwabo.CommandSystem.Attributes.RaExt;

/// <summary>
/// Specifies that the Remote Admin option identifier should be automatically generated. This attribute is inherited.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class AutoGenerateIdAttribute : Attribute, IRemoteAdminOptionIdentifier
{

    /// <summary>A string literal that is used to identify the attribute.</summary>
    public const string Identifier = "$@";

    /// <inheritdoc />
    public string Id => Identifier;

}
