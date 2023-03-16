using System;
using Axwabo.CommandSystem.Registration;

namespace Axwabo.CommandSystem.Attributes;

/// <summary>
/// An attribute to specify that a command or Remote Admin option should not be automatically registered. 
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class DoNotAutoRegister : Attribute, IRegistrationFilter
{

    /// <summary>Returns false.</summary>
    public bool AllowRegistration => false;

}
