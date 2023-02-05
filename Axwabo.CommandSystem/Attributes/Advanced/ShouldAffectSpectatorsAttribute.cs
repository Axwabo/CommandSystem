using System;
using Axwabo.CommandSystem.Attributes.Advanced.Interfaces;

namespace Axwabo.CommandSystem.Attributes.Advanced;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class ShouldAffectSpectatorsAttribute : Attribute, IShouldAffectSpectators {

    public bool AffectSpectators { get; }

    public ShouldAffectSpectatorsAttribute(bool affectSpectators = true) => AffectSpectators = affectSpectators;

}
