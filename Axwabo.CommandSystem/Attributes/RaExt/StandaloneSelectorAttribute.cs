using Axwabo.CommandSystem.RemoteAdminExtensions.Interfaces;

namespace Axwabo.CommandSystem.Attributes.RaExt;

/// <summary>
/// Specifies that the Remote Admin option can also be used as a standalone selector. This attribute is inherited.
/// </summary>
/// <seealso cref="Axwabo.CommandSystem.Selectors.PlayerSelectionManager"/>
[AttributeUsage(AttributeTargets.Class)]
public sealed class StandaloneSelectorAttribute : Attribute, IStandaloneSelectorOption
{

    /// <inheritdoc />
    public bool CanBeUsedAsStandaloneSelector => true;

}
