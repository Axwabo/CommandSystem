namespace Axwabo.CommandSystem.Registration;

/// <summary>
/// A filter that can be used to prevent registration of a command or Remote Admin option.
/// </summary>
public interface IRegistrationFilter
{

    /// <summary>
    /// Whether the instance should be registered.
    /// </summary>
    bool AllowRegistration { get; }

}
