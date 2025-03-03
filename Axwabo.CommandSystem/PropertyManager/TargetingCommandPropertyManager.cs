using Axwabo.CommandSystem.Attributes.Targeting.Interfaces;
using Axwabo.CommandSystem.Commands;
using Axwabo.CommandSystem.Commands.Interfaces;
using Axwabo.CommandSystem.Commands.MessageOverrides;
using Axwabo.CommandSystem.Extensions;
using Axwabo.CommandSystem.Registration;

namespace Axwabo.CommandSystem.PropertyManager;

/// <summary>Attribute to property handler for targeting commands.</summary>
public static class TargetingCommandPropertyManager
{

    /// <summary>
    /// Resolves the properties of a targeting command.
    /// </summary>
    /// <param name="member">The member to resolve properties from.</param>
    /// <param name="command">The command to use for potential interface resolving.</param>
    /// <returns>A <see cref="TargetingCommandProperties"/> class containing the resolved information.</returns>
    /// <remarks>This method does not resolve <see cref="TargetingCommandProperties.BaseProperties"/>. Use <see cref="BaseCommandPropertyManager.ResolveProperties"/> to resolve them.</remarks>
    public static TargetingCommandProperties ResolveProperties(MemberInfo member, CommandBase command = null)
    {
        var properties = new TargetingCommandProperties();
        foreach (var attribute in member.GetCustomAttributes())
        {
            ResolveBaseAttribute(
                attribute,
                ref properties.NoTargetsFoundMessage,
                ref properties.AffectedOneMessage,
                ref properties.AffectedMultipleMessage,
                ref properties.NoPlayersAffectedMessage, ref properties.ShouldAffectSpectators);

            var type = attribute.GetType();
            ResolveSingleAffected(type, attribute, ref properties.AffectedOneGenerator);
            ResolveMultipleAffected(type, attribute, ref properties.AffectedMultipleGenerator);
            ResolveAllAffected(type, attribute, ref properties.AffectedAllGenerator);
            ResolveSelectionManager(type, attribute, ref properties.SelectionManager);
        }

        command.SafeCastAndSetIfNull(ref properties.AffectedOneGenerator);
        command.SafeCastAndSetIfNull(ref properties.AffectedMultipleGenerator);
        command.SafeCastAndSetIfNull(ref properties.AffectedAllGenerator);
        command.SafeCastAndSetIfNull(ref properties.FilteringPolicy);
        command.SafeCastAndSetIfNull(ref properties.SelectionManager);
        return properties;
    }

    private static void ResolveBaseAttribute(
        Attribute attribute,
        ref string noTargetsFound,
        ref string affectedOneMessage,
        ref string affectedMultipleMessage,
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
    /// Resolves the custom result compiler for a targeting command.
    /// </summary>
    /// <param name="command">The command to resolve the compiler for.</param>
    /// <returns>The custom result compiler for the command. If none was found, returns null.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="command"/> is null.</exception>
    public static ICustomResultCompiler ResolveCustomResultCompiler(SeparatedTargetingCommand command)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));
        ICustomResultCompiler compiler = null;
        foreach (var attribute in command.GetType().GetCustomAttributes())
        {
            if (attribute is ICustomResultCompiler ac)
                compiler = ac;
            if (ProcSet(out var proc))
                proc.TargetingResultCompilerResolvers.Resolve(ref compiler, command.GetType(), attribute);
        }

        command.SafeCastAndSetIfNull(ref compiler);
        return compiler;
    }

    private static bool ProcSet(out CommandRegistrationProcessor proc)
    {
        proc = BaseCommandPropertyManager.CurrentProcessor;
        return proc != null;
    }

    private static void ResolveMultipleAffected(Type type, Attribute attribute, ref IAffectedMultiplePlayersMessageGenerator affectedMultipleMessage)
    {
        if (!attribute.SafeCastAndSetIfNull(ref affectedMultipleMessage) && ProcSet(out var proc))
            proc.TargetingMultipleMessageResolvers.Resolve(ref affectedMultipleMessage, type, attribute);
    }

    private static void ResolveSingleAffected(Type type, Attribute attribute, ref IAffectedOnePlayerMessageGenerator affectedSingle)
    {
        if (!attribute.SafeCastAndSetIfNull(ref affectedSingle) && ProcSet(out var proc))
            proc.TargetingSingleMessageResolvers.Resolve(ref affectedSingle, type, attribute);
    }

    private static void ResolveAllAffected(Type type, Attribute attribute, ref IAffectedAllPlayersMessageGenerator affectedAll)
    {
        if (!attribute.SafeCastAndSetIfNull(ref affectedAll) && ProcSet(out var proc))
            proc.TargetingAllMessageResolvers.Resolve(ref affectedAll, type, attribute);
    }

    private static void ResolveSelectionManager(Type type, Attribute attribute, ref ITargetSelectionManager selectionManager)
    {
        if (!attribute.SafeCastAndSetIfNull(ref selectionManager) && ProcSet(out var proc))
            proc.TargetSelectionManagerResolvers.Resolve(ref selectionManager, type, attribute);
    }

}
