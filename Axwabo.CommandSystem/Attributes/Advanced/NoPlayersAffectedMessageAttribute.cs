﻿using System;
using Axwabo.CommandSystem.Attributes.Advanced.Interfaces;

namespace Axwabo.CommandSystem.Attributes.Advanced;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class NoPlayersAffectedMessageAttribute : Attribute, IStaticNoPlayersAffectedMessage {

    public string NoPlayersAffected { get; }

    public NoPlayersAffectedMessageAttribute(string message) => NoPlayersAffected = message;

}