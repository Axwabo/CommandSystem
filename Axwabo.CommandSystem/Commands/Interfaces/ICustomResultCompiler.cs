using System.Collections.Generic;
using Axwabo.CommandSystem.Structs;

namespace Axwabo.CommandSystem.Commands.Interfaces;

public interface ICustomResultCompiler {

    CommandResult CompileCustomResult(List<CommandResultOnTarget> success, List<CommandResultOnTarget> failures);

}
