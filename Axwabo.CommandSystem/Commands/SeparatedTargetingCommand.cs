using Axwabo.CommandSystem.Commands.Interfaces;

namespace Axwabo.CommandSystem.Commands;

/// <summary>
/// A base class for handling a command which accepts a list of targets as the first argument.
/// This class executes the command on each player separately and combines the result.
/// </summary>
/// <seealso cref="UnifiedTargetingCommand"/>
public abstract class SeparatedTargetingCommand : UnifiedTargetingCommand
{

    /// <inheritdoc />
    protected override CommandResult ExecuteOnTargets(List<ReferenceHub> targets, ArraySegment<string> arguments, CommandSender sender)
    {
        var succeeded = new List<CommandResultOnTarget>();
        var failed = new List<CommandResultOnTarget>();
        foreach (var target in targets)
        {
            var result = ExecuteOn(target, arguments, sender);
            if (result)
                succeeded.Add(new CommandResultOnTarget(target, result.Response));
            else
                failed.Add(new CommandResultOnTarget(target, result, false));
        }

        return succeeded.Count == 0 && failed.Count == 0
            ? CommandResult.Failed(NoPlayersAffected)
            : (this as ICustomResultCompiler)?.CompileResultCustom(succeeded, failed)
              ?? CompileResult(succeeded);
    }

    /// <summary>
    /// Executes the command on a single target.
    /// </summary>
    /// <param name="target">The target to execute the command on.</param>
    /// <param name="arguments">The arguments passed to the command.</param>
    /// <param name="sender">The sender of the command.</param>
    /// <returns>The result of execution.</returns>
    protected abstract CommandResult ExecuteOn(ReferenceHub target, ArraySegment<string> arguments, CommandSender sender);

    /// <summary>
    /// Compiles the result of the command execution.
    /// </summary>
    /// <param name="success">The list of players that were successfully affected by the command.</param>
    /// <returns>The combined result of the command execution.</returns>
    protected CommandResult CompileResult(List<CommandResultOnTarget> success)
    {
        var affected = success.Count;
        return affected == 0
            ? CommandResult.Failed(NoPlayersAffected)
            : CommandResult.Succeeded(
                affected == 1
                    ? GetAffectedMessageSingle(success[0].Target)
                    : IsEveryoneAffectedInternal(affected)
                        ? GetAffectedMessageAll(affected)
                        : GetAffectedMessage(affected)
            );
    }

}
