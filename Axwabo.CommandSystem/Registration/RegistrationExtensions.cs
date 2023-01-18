using System;

namespace Axwabo.CommandSystem.Registration {

    public static class RegistrationExtensions {

        public static CommandRegistrationProcessor WithRegistrationAttributesFrom(this CommandRegistrationProcessor processor, Type type) {
            // TODO: handle attributes
            return processor;
        }

    }

}
