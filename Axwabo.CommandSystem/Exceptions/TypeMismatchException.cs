using System;

namespace Axwabo.CommandSystem.Exceptions {

    public sealed class TypeMismatchException : Exception {

        public TypeMismatchException(Type supplied, Type expected, string objectDescription = null)
            : base($"The generic type {(objectDescription == null ? "" : "of " + objectDescription + " ")}({supplied.FullName}) does not match the given type ({expected.FullName}).") {
        }

    }

}
