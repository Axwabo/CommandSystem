﻿namespace Axwabo.CommandSystem.Attributes.Targeting.Interfaces;

/// <summary>
/// Supplies a static no targets found message.
/// </summary>
public interface IStaticNoTargetsFoundMessage
{

    /// <summary>
    /// Gets the "no targets were found" message.
    /// </summary>
    string NoTargets { get; }

}
