namespace Axwabo.CommandSystem;

/// <summary>
/// Stores the result of a command along with the <see cref="ReferenceHub"/> it was executed on.
/// </summary>
public readonly struct CommandResultOnTarget
{

    /// <summary>The target of execution.</summary>
    public readonly ReferenceHub Target;

    /// <summary>The response of the command.</summary>
    public readonly string Response;

    /// <summary>Whether the command was successful.</summary>
    public readonly bool Success;

    /// <summary>Quick nickname access.</summary>
    public readonly string Nick;

    /// <summary>Determines whether response is empty.</summary>
    public bool IsEmpty => string.IsNullOrEmpty(Response);

    /// <summary>
    /// Creates a new <see cref="CommandResultOnTarget"/> instance with a response.
    /// </summary>
    /// <param name="target">The target of execution.</param>
    /// <param name="response">The response of the command.</param>
    /// <param name="success">Whether the command was successful.</param>
    public CommandResultOnTarget(ReferenceHub target, string response, bool success = true)
    {
        Target = target;
        Response = response;
        Success = success;
        Nick = target.nicknameSync.MyNick;
    }

    /// <summary>
    /// Creates a new <see cref="CommandResultOnTarget"/> instance with an empty response.
    /// </summary>
    /// <param name="target">The target of execution.</param>
    /// <param name="success">Whether the command was successful.</param>
    public CommandResultOnTarget(ReferenceHub target, bool success)
    {
        Target = target;
        Response = null;
        Success = success;
        Nick = target.nicknameSync.MyNick;
    }

    /// <summary>
    /// Casts a <see cref="CommandResultOnTarget"/> to a bool, determining its success state.
    /// </summary>
    /// <param name="r">The <see cref="CommandResultOnTarget"/> to cast.</param>
    /// <returns>Whether the command succeeded.</returns>
    public static implicit operator bool(CommandResultOnTarget r) => r.Success;

}
