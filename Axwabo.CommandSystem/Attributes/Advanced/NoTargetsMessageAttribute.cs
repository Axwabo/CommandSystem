using System;
using Axwabo.CommandSystem.Attributes.Advanced.Interfaces;

namespace Axwabo.CommandSystem.Attributes.Advanced;

/// <summary>
/// Supplies a static no targets found message.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class NoTargetsMessageAttribute : Attribute, IStaticNoTargetsFoundMessage {

    /// <inheritdoc />
    public string NoTargets { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="NoTargetsMessageAttribute"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    public NoTargetsMessageAttribute(string message) => NoTargets = message;

}
