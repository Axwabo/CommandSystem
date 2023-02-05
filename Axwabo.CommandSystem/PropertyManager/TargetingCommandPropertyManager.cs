using System;
using System.Reflection;
using Axwabo.CommandSystem.Attributes.Advanced.Interfaces;

namespace Axwabo.CommandSystem.PropertyManager;

public static class TargetingCommandPropertyManager {

    public static void ResolveProperties(CommandBase command, ref string noTargetsFound, ref string affectedMessage, ref string noPlayersAffected, ref bool shouldAffectSpectators) {
        BaseCommandPropertyManager.ValidateRegistration(command);
        foreach (var attribute in command.GetType().GetCustomAttributes()) {
            if (ResolveBaseAttribute(attribute, ref noTargetsFound, ref affectedMessage, ref noPlayersAffected, ref shouldAffectSpectators))
                continue;
            var type = attribute.GetType();
            // TODO: parameterized resolvers
        }
    }

    private static bool ResolveBaseAttribute(Attribute attribute, ref string noTargetsFound, ref string affectedMessage, ref string noPlayersAffected, ref bool shouldAffectSpectators) {
        var completed = false;
        if (attribute is IStaticNoTargetsFoundMessage noTargets) {
            noTargets.Message.SetFieldIfNotNull(ref noTargetsFound);
            completed = true;
        }

        if (attribute is IShouldAffectSpectators affectSpectators) {
            shouldAffectSpectators = affectSpectators.AffectSpectators;
            completed = true;
        }

        return completed;
    }

}
