using System;

namespace Axwabo.CommandSystem.Exceptions;

/// <summary>
/// An exception that is thrown when a <see cref="CommandBase">command</see> or <see cref="Axwabo.CommandSystem.RemoteAdminExtensions.RemoteAdminOptionBase">RA option</see> name is invalid.
/// </summary>
public sealed class InvalidNameException : Exception
{

    /// <summary>Initializes a new instance of the <see cref="InvalidNameException"/> class.</summary>
    /// <param name="message">The message that describes the error.</param>
    public InvalidNameException(string message) : base(message)
    {
    }

}
