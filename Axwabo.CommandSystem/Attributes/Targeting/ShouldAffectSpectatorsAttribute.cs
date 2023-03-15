using System;
using Axwabo.CommandSystem.Attributes.Targeting.Interfaces;

namespace Axwabo.CommandSystem.Attributes.Targeting;

/// <summary>
/// Controls whether the command should affect spectators.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class ShouldAffectSpectatorsAttribute : Attribute, IShouldAffectSpectators
{

    /// <inheritdoc />
    public bool AffectSpectators { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ShouldAffectSpectatorsAttribute"/> class.
    /// </summary>
    /// <param name="affectSpectators">Whether the command should affect spectators.</param>
    public ShouldAffectSpectatorsAttribute(bool affectSpectators = true) => AffectSpectators = affectSpectators;

}
