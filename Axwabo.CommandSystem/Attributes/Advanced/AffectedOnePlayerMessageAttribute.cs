using System;
using Axwabo.CommandSystem.Attributes.Advanced.Interfaces;

namespace Axwabo.CommandSystem.Attributes.Advanced;

/// <summary>
/// Supplies a static affected one player message.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
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
