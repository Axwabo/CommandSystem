using Axwabo.CommandSystem.Attributes.Interfaces;

namespace Axwabo.CommandSystem.Attributes.RaExt;

/// <summary>
/// Specifies the identifier of a Remote Admin option. Must not contain any of the <see cref="RemoteAdminOptionManager.InvalidCharacters"/>. Must not be numeric-only, including a leading hyphen.
/// </summary>
/// <seealso cref="RemoteAdminOptionManager.IsValidOptionId"/>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class RemoteAdminOptionIdentifierAttribute : Attribute, IRemoteAdminOptionIdentifier
{

    /// <inheritdoc />
    public string Id { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="RemoteAdminOptionIdentifierAttribute"/> attribute.
    /// </summary>
    /// <param name="name">The identifier of the Remote Admin option.</param>
    public RemoteAdminOptionIdentifierAttribute(string name) => Id = name;

}
