﻿namespace Axwabo.CommandSystem.Attributes.Advanced.Interfaces;

/// <summary>
/// Supplies a static no players affected message.
/// </summary>
public interface IStaticNoPlayersAffectedMessage
{

    /// <summary>
    /// Gets the "no players affected" message.
    /// </summary>
    string NoPlayersAffected { get; }

}
