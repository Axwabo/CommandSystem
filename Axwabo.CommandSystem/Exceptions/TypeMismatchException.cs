using System;

namespace Axwabo.CommandSystem.Exceptions {

    public sealed class TypeMismatchException : Exception {

        public TypeMismatchException(string message) : base(message) {
        }

    }

}
