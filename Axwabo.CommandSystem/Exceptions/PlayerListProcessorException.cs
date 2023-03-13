using System;
using Axwabo.CommandSystem.Selectors;
using static Axwabo.CommandSystem.Selectors.PlayerSelectionManager;

namespace Axwabo.CommandSystem.Exceptions;

/// <summary>
/// An exception thrown while processing a player list.
/// </summary>
public sealed class PlayerListProcessorException : Exception
{

    /// <summary>Creates a new <see cref="PlayerListProcessorException"/> instance.</summary>
    public PlayerListProcessorException()
    {
    }

    /// <summary>Creates a new <see cref="PlayerListProcessorException"/> instance.</summary>
    /// <param name="message">The message of the exception.</param>
    public PlayerListProcessorException(string message) : base(message)
    {
    }

    /// <summary>Creates a new <see cref="PlayerListProcessorException"/> instance.</summary>
    /// <param name="message">The message of the exception.</param>
    /// <param name="inner">The inner exception.</param>
    public PlayerListProcessorException(string message, Exception inner) : base(message, inner)
    {
    }

    /// <summary>
    /// Converts the exception to a command execution failure message.
    /// </summary>
    /// <param name="e">The exception to convert.</param>
    /// <returns>The command execution failure message.</returns>
    /// <remarks>If the exception is a <see cref="PlayerListProcessorException"/>, the message will start with <see cref="PlayerSelectionManager.FailedToParsePlayerList"/>.</remarks>
    public static string CreateMessage(Exception e)
        => e is PlayerListProcessorException
            ? FailedToParsePlayerList + e.Message
            : CommandExecutionFailedError + Misc.RemoveStacktraceZeroes(e.ToString());

}
