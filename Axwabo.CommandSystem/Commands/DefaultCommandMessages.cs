namespace Axwabo.CommandSystem.Commands;

/// <summary>Constants used by built-in command types.</summary>
public static class DefaultCommandMessages
{

    /// <summary>The message to display when only a player is allowed to use the command but the sender is not a player.</summary>
    public const string MustBePlayer = "You must be a player to use this command!";

    /// <summary>The message to display when a targeting command doesn't find any targets.</summary>
    public const string NoTargetsFound = "No targets were found.";

    /// <summary>The message to display when a targeting command didn't affect any players.</summary>
    public const string NoPlayersAffected = "No players were affected.";

}
