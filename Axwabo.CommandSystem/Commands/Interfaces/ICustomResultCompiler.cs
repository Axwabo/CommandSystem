using System.Collections.Generic;

namespace Axwabo.CommandSystem.Commands.Interfaces;

/// <summary>
/// An interface used by <see cref="SeparatedTargetingCommand"/>
/// </summary>
public interface ICustomResultCompiler
{

    /// <summary>
    /// Compiles the result of executing a targeting command.
    /// </summary>
    /// <param name="success">The list of targets that the command has been successfully executed on.</param>
    /// <param name="failures">The list of targets that the command has failed to execute on.</param>
    CommandResult CompileResultCustom(List<CommandResultOnTarget> success, List<CommandResultOnTarget> failures);

}
