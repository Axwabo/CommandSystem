using System;
using Axwabo.CommandSystem.Attributes.Interfaces;

namespace Axwabo.CommandSystem.Attributes.Advanced;

public sealed class ShouldAffectSpectators : Attribute, IAffectSpectators {

    public bool AffectSpectators { get; }

    public ShouldAffectSpectators(bool affectSpectators = true) => AffectSpectators = affectSpectators;

}
