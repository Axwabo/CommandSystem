using Axwabo.CommandSystem.Attributes.Targeting.Interfaces;
using Axwabo.CommandSystem.Commands;
using Axwabo.CommandSystem.Commands.MessageOverrides;
using Axwabo.CommandSystem.Registration;

namespace Axwabo.CommandSystem.PropertyManager;

/// <summary>Attribute to property handler for targeting commands.</summary>
public static class TargetingCommandPropertyManager
{

    /// <summary>
    /// Resolves the properties of a targeting command.
    /// </summary>
    /// <param name="command">The command to resolve properties for.</param>
    /// <param name="noTargetsFound">The message to display when no targets are found.</param>
    /// <param name="affectedMultipleMessage">The message to display when multiple targets are affected.</param>
    /// <param name="affectedOneMessage">The message to display when one target is affected.</param>
    /// <param name="noPlayersAffected">The message to display when no targets are affected.</param>
    /// <param name="shouldAffectSpectators">Whether the command should affect spectators.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="command"/> is null.</exception>
    public static void ResolveProperties(
        CommandBase command,
        ref string noTargetsFound,
        ref string affectedMultipleMessage,
        ref string affectedOneMessage,
        ref string noPlayersAffected,
        ref bool shouldAffectSpectators
    )
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));
        foreach (var attribute in command.GetType().GetCustomAttributes())
            ResolveBaseAttribute(attribute, ref noTargetsFound, ref affectedMultipleMessage, ref affectedOneMessage, ref noPlayersAffected, ref shouldAffectSpectators);
    }

    private static void ResolveBaseAttribute(
        Attribute attribute,
        ref string noTargetsFound,
        ref string affectedMultipleMessage,
        ref string affectedOneMessage,
        ref string noPlayersAffected,
        ref bool shouldAffectSpectators
    )
    {
        if (attribute is IStaticNoTargetsFoundMessage noTargets)
            noTargets.NoTargets.SetFieldIfNotNull(ref noTargetsFound);

        if (attribute is IStaticAffectedMultiplePlayersMessage affected)
            affected.AffectedMultiple.SetFieldIfNotNull(ref affectedMultipleMessage);

        if (attribute is IStaticAffectedOnePlayerMessage affectedOne)
            affectedOne.AffectedOne.SetFieldIfNotNull(ref affectedOneMessage);

        if (attribute is IStaticNoPlayersAffectedMessage noPlayers)
            noPlayers.NoPlayersAffected.SetFieldIfNotNull(ref noPlayersAffected);

        if (attribute is IShouldAffectSpectators affectSpectators)
            shouldAffectSpectators = affectSpectators.AffectSpectators;
    }

    /// <summary>
    /// Resolves the message generators for a targeting command.
    /// </summary>
    /// <param name="command">The command to resolve generators for.</param>
    /// <param name="affectedMultiple">The generator for multiple affected messages.</param>
    /// <param name="affectedAll">The generator for all affected messages.</param>
    /// <param name="affectedOne">The generator for one affected messages.</param>
    /// <param name="selectionManager">The selection manager for the command.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="command"/> is null.</exception>
    public static void ResolveGenerators(
        UnifiedTargetingCommand command,
        out IAffectedMultiplePlayersMessageGenerator affectedMultiple,
        out IAffectedAllPlayersMessageGenerator affectedAll,
        out IAffectedOnePlayerMessageGenerator affectedOne,
        out ITargetSelectionManager selectionManager
    )
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));
        affectedMultiple = null;
        affectedAll = null;
        affectedOne = null;
        selectionManager = null;
        foreach (var attribute in command.GetType().GetCustomAttributes())
        {
            var type = attribute.GetType();
            ResolveMultipleAffected(type, attribute, ref affectedMultiple);
            ResolveSingleAffected(type, attribute, ref affectedOne);
            ResolveAllAffected(type, attribute, ref affectedAll);
            ResolveSelectionManager(type, attribute, ref selectionManager);
        }

        command.SafeCastAndSetIfNull(ref affectedMultiple);
        command.SafeCastAndSetIfNull(ref affectedAll);
        command.SafeCastAndSetIfNull(ref affectedOne);
        command.SafeCastAndSetIfNull(ref selectionManager);
    }

    private static bool ProcNull(out CommandRegistrationProcessor proc)
    {
        proc = BaseCommandPropertyManager.CurrentProcessor;
        return proc == null;
    }

    private static void ResolveMultipleAffected(Type type, Attribute attribute, ref IAffectedMultiplePlayersMessageGenerator affectedMultipleMessage)
    {
        if (attribute.SafeCastAndSetIfNull(ref affectedMultipleMessage))
            return;
        if (ProcNull(out var proc))
            return;
        foreach (var resolver in proc.TargetingMultipleMessageResolvers)
            if (resolver.Takes(type))
                affectedMultipleMessage = resolver.Resolve(attribute);
    }

    private static void ResolveSingleAffected(Type type, Attribute attribute, ref IAffectedOnePlayerMessageGenerator affectedSingle)
    {
        if (attribute.SafeCastAndSetIfNull(ref affectedSingle))
            return;
        if (ProcNull(out var proc))
            return;
        foreach (var resolver in proc.TargetingSingleMessageResolvers)
            if (resolver.Takes(type))
                affectedSingle = resolver.Resolve(attribute);
    }

    private static void ResolveAllAffected(Type type, Attribute attribute, ref IAffectedAllPlayersMessageGenerator affectedAll)
    {
        if (attribute.SafeCastAndSetIfNull(ref affectedAll))
            return;
        if (ProcNull(out var proc))
            return;
        foreach (var resolver in proc.TargetingAllMessageResolvers)
            if (resolver.Takes(type))
                affectedAll = resolver.Resolve(attribute);
    }

    private static void ResolveSelectionManager(Type type, Attribute attribute, ref ITargetSelectionManager selectionManager)
    {
        if (attribute.SafeCastAndSetIfNull(ref selectionManager))
            return;
        if (ProcNull(out var proc))
            return;
        foreach (var resolver in proc.TargetSelectionManagerResolvers)
            if (resolver.Takes(type))
                selectionManager = resolver.Resolve(attribute);
    }

}
