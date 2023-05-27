namespace Axwabo.CommandSystem.Exceptions;

/// <summary>
/// An exception that is thrown when the actual type does not match the expected type.
/// </summary>
public sealed class TypeMismatchException : Exception
{

    /// <summary>Initializes a new instance of the <see cref="TypeMismatchException"/> class.</summary>
    /// <param name="message">The message that describes the error.</param>
    public TypeMismatchException(string message) : base(message)
    {
    }

}
