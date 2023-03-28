using System;
using System.Collections.Generic;

namespace Axwabo.CommandSystem.Commands.Interfaces;

/// <summary>
/// A targeting command preprocessor that can be used to prevent execution if required.
/// </summary>
public interface ITargetingPreExecutionFilter
{

    /// <summary>
    /// Called after the permission checks, argument count check, target parsing and before target check, second argument count validation and execution.
    /// </summary>
    /// <param name="targets">The targets parsed from the command.</param>
    /// <param name="arguments">The arguments passed to the command excluding targets.</param>
    /// <param name="sender">The sender of the command.</param>
    /// <returns>A <see cref="CommandResult"/> if the execution should not continue. To allow execution, return <see cref="CommandResult.Null">CommandResult.Null</see>.</returns>
    CommandResult? OnBeforeExecuted(List<ReferenceHub> targets, ArraySegment<string> arguments, CommandSender sender);

}
