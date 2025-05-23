﻿using Axwabo.CommandSystem.Attributes.Targeting.Interfaces;

namespace Axwabo.CommandSystem.Attributes.Targeting;

/// <summary>
/// Supplies a static affected one player message. This attribute is inherited.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class AffectedOnePlayerMessageAttribute : Attribute, IStaticAffectedOnePlayerMessage
{

    /// <inheritdoc />
    public string AffectedOne { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AffectedOnePlayerMessageAttribute"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    public AffectedOnePlayerMessageAttribute(string message) => AffectedOne = message;

}
