using Axwabo.CommandSystem.Registration;

namespace Axwabo.CommandSystem.Attributes;

/// <summary>
/// An attribute to specify that a command or Remote Admin option should not be automatically registered.
/// This attribute is <b>not</b> inherited.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false)]
public sealed class DoNotAutoRegister : Attribute, IRegistrationFilter
{

    /// <summary>Returns false.</summary>
    public bool AllowRegistration => false;

}
