using System;

namespace Axwabo.CommandSystem.Commands.Interfaces;

/// <summary>
/// A command preprocessor that is invoked when the command is executed with not enough arguments. A <see cref="CommandBase"/> can implement this interface.
/// </summary>
public interface INotEnoughArgumentsHandler
{

    /// <summary>
    /// Called when the command is executed with not enough arguments.
    /// </summary>
    /// <param name="arguments">The arguments passed to the command.</param>
    /// <param name="sender">The sender of the command.</param>
    /// <param name="required">The amount of arguments required.</param>
    /// <returns>A <see cref="CommandResult"/> indicating the success or failure. To use the default message, return <see cref="CommandResult.Null"/>.</returns>
    CommandResult? OnNotEnoughArgumentsProvided(ArraySegment<string> arguments, CommandSender sender, int required);

}
