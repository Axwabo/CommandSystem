using System;
using Axwabo.CommandSystem.Attributes.Interfaces;

namespace Axwabo.CommandSystem.Attributes.RaExt;

/// <summary>
/// Specifies that the Remote Admin option can also be used as a standalone selector.
/// </summary>
/// <seealso cref="Axwabo.CommandSystem.RemoteAdminExtensions.RemoteAdminOptionBase.CanBeUsedAsStandaloneSelector"/>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class StandaloneSelectorAttribute : Attribute, IStandaloneSelectorOption
{

    /// <inheritdoc />
    public bool CanBeUsedAsStandaloneSelector => true;

}
