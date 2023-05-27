namespace Axwabo.CommandSystem.Commands.Interfaces;

/// <summary>
/// A command preprocessor that can be used to prevent execution if required.
/// </summary>
public interface IPreExecutionFilter
{

    /// <summary>
    /// Called after the permission checks and before checking for the argument count and execution.
    /// </summary>
    /// <param name="arguments">The arguments passed to the command.</param>
    /// <param name="sender">The sender of the command.</param>
    /// <returns>A <see cref="CommandResult"/> if the execution should not continue. To allow execution, return <see cref="CommandResult.Null">CommandResult.Null</see>.</returns>
    CommandResult? OnBeforeExecuted(ArraySegment<string> arguments, CommandSender sender);

}
