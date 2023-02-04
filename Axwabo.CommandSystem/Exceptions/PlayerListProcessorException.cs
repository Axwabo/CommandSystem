using System;
using static Axwabo.CommandSystem.Selectors.PlayerSelectionManager;

namespace Axwabo.CommandSystem.Exceptions;

public sealed class PlayerListProcessorException : Exception {

    public PlayerListProcessorException() {
    }

    public PlayerListProcessorException(string message) : base(message) {
    }

    public PlayerListProcessorException(string message, Exception inner) : base(message, inner) {
    }

    public static string CreateMessage(Exception e)
        => e is PlayerListProcessorException
            ? FailedToParsePlayerList + e.Message
            : CommandExecutionFailedError + Misc.RemoveStacktraceZeroes(e.ToString());

}
