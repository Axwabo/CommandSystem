using System;

namespace Axwabo.CommandSystem.Exceptions;

public sealed class InvalidNameException : Exception {

    public InvalidNameException(string message) : base(message) {
    }

}
