using Axwabo.CommandSystem.Commands.Interfaces;
using Axwabo.CommandSystem.Commands.MessageOverrides;

namespace Axwabo.CommandSystem.Commands;

/// <summary>Stores basic information about a <see cref="UnifiedTargetingCommand"/>.</summary>
public sealed class TargetingCommandProperties
{

    /// <summary>Base properties for <see cref="CommandBase"/>.</summary>
    public BaseCommandProperties BaseProperties;

    /// <summary>Whether spectators should be targeted. Defaults to true.</summary>
    public bool ShouldAffectSpectators = true;

    /// <summary>A custom target filtering policy.</summary>
    public ITargetFilteringPolicy FilteringPolicy;

    /// <summary>A custom target selection manager.</summary>
    public ITargetSelectionManager SelectionManager;

    /// <summary>The message to return when no targets were found.</summary>
    public string NoTargetsFoundMessage;

    /// <summary>The message to return when no players were affected.</summary>
    public string NoPlayersAffectedMessage;

    /// <summary>
    /// The message to format when one player was affected.
    /// This message is used in the default <see cref="IAffectedOnePlayerMessageGenerator"/> could be resolved.
    /// </summary>
    /// <seealso cref="DefaultTargetingMessageGenerator"/>
    public string AffectedOneMessage;

    /// <summary>
    /// The message to format when multiple players were affected.
    /// This message is used in the default <see cref="IAffectedMultiplePlayersMessageGenerator"/> if no such generator could be resolved.
    /// This message is used in the default <see cref="IAffectedMultiplePlayersMessageGenerator"/> if no such generator could be resolved.
    /// </summary>
    /// <seealso cref="DefaultTargetingMessageGenerator"/>
    public string AffectedMultipleMessage;

    /// <summary>The message generator to use when one player was affected.</summary>
    public IAffectedOnePlayerMessageGenerator AffectedOneGenerator;

    /// <summary>The message generator to use when mupltiple players were affected.</summary>
    public IAffectedMultiplePlayersMessageGenerator AffectedMultipleGenerator;

    /// <summary>The message generator to use when all targeted players were affected.</summary>
    public IAffectedAllPlayersMessageGenerator AffectedAllGenerator;

    /// <summary>
    /// Creates a shallow copy of this <see cref="TargetingCommandProperties"/> instance.
    /// The members of <see cref="BaseProperties"/> are not cloned.
    /// </summary>
    /// <returns>The cloned <see cref="BaseCommandProperties"/>.</returns>
    public TargetingCommandProperties Clone() => new()
    {
        BaseProperties = BaseProperties,
        ShouldAffectSpectators = ShouldAffectSpectators,
        FilteringPolicy = FilteringPolicy,
        SelectionManager = SelectionManager,
        NoTargetsFoundMessage = NoTargetsFoundMessage,
        NoPlayersAffectedMessage = NoPlayersAffectedMessage,
        AffectedOneMessage = AffectedOneMessage,
        AffectedMultipleMessage = AffectedMultipleMessage,
        AffectedOneGenerator = AffectedOneGenerator,
        AffectedMultipleGenerator = AffectedMultipleGenerator,
        AffectedAllGenerator = AffectedAllGenerator
    };

}
