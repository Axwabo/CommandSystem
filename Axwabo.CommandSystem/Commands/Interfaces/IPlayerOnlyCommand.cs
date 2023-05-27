namespace Axwabo.CommandSystem.Commands.Interfaces;

/// <summary>
/// A command preprocessor that is invoked when the command is executed by a non-player. A <see cref="CommandBase"/> can implement this interface.
/// </summary>
public interface IPlayerOnlyCommand
{

    /// <summary>
    /// Called when the command is executed by a non-player.
    /// </summary>
    /// <param name="arguments">The arguments passed to the command.</param>
    /// <param name="sender">The sender of the command.</param>
    /// <returns>A <see cref="CommandResult"/> indicating the success or failure. To use the default message, return <see cref="CommandResult.Null">CommandResult.Null</see>.</returns>
    CommandResult? OnNotPlayer(ArraySegment<string> arguments, CommandSender sender);

}
