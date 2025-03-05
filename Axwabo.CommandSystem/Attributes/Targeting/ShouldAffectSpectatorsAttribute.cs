using Axwabo.CommandSystem.Commands.Interfaces;

namespace Axwabo.CommandSystem.Attributes.Targeting;

/// <summary>
/// Controls whether the command should affect spectators. This attribute is inherited.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
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
