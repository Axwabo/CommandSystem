using System;
using Axwabo.CommandSystem.Attributes.Listeners;

namespace Axwabo.CommandSystem.Registration {

    public static class RegistrationExtensions {
        
        public static bool HasFlagFast(this CommandTarget target, CommandTarget flag) => (target & flag) == flag;

        public static CommandRegistrationProcessor WithRegistrationAttributesFrom(this CommandRegistrationProcessor processor, Type type) {
            // TODO: handle attributes
            return processor;
        }

    }

}
