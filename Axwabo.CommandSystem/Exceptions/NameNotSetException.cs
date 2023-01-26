using System;

namespace Axwabo.CommandSystem.Exceptions;

public sealed class NameNotSetException : Exception {

    public NameNotSetException(string message) : base(message) {
    }

}
