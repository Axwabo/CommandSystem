namespace Axwabo.CommandSystem.Structs;

/// <summary>
/// Represents the result of a command execution.
/// </summary>
public readonly struct CommandResult {

    #region Static

    /// <summary>
    /// Creates a new succeeding <see cref="CommandResult"/> with the given response.
    /// </summary>
    /// <param name="response">The response.</param>
    /// <returns>A new <see cref="CommandResult"/>.</returns>
    public static CommandResult Succeeded(string response) => new(true, response);

    /// <summary>
    /// Creates a new failing <see cref="CommandResult"/> with the given response.
    /// </summary>
    /// <param name="response">The response.</param>
    /// <returns>A new <see cref="CommandResult"/>.</returns>
    public static CommandResult Failed(string response) => new(false, response);

    /// <summary>A null <see cref="CommandResult"/> as a <see cref="System.Nullable{T}"/>.</summary>
    public static CommandResult? Null => null;

    #endregion

    #region Members

    /// <summary>The response of the command.</summary>
    public readonly string Response;

    /// <summary>Whether the command was successful.</summary>
    public readonly bool Success;

    /// <summary>Determines whether response is empty.</summary>
    public bool IsEmpty => string.IsNullOrEmpty(Response);

    /// <summary>
    /// Creates a new <see cref="CommandResult"/> instance with a response.
    /// </summary>
    /// <param name="response">The response of the command.</param>
    /// <remarks>If the response starts with '!', it will be treated as a failing response. If you want your response to start with '!', use <see cref="Succeeded"/> or <see cref="CommandResult(bool,string)">the other constructor</see>.</remarks>
    public CommandResult(string response) {
        var success = string.IsNullOrEmpty(response) || !response.StartsWith("!");
        Response = success ? response : response.Substring(1);
        Success = success;
    }

    /// <summary>
    /// Creates a new <see cref="CommandResult"/> instance with a response.
    /// </summary>
    /// <param name="success">Whether the command was successful.</param>
    /// <param name="response">The response of the command.</param>
    public CommandResult(bool success, string response) {
        Response = response;
        Success = success;
    }

    #endregion

    #region Casts

    /// <summary>
    /// Casts a string to a <see cref="CommandResult"/> with the string as the response.
    /// </summary>
    /// <param name="s">The string to cast.</param>
    /// <returns>A new <see cref="CommandResult"/>.</returns>
    /// <remarks>If the response starts with '!', it will be treated as a failing response. If you want your response to start with '!', use <see cref="Succeeded"/> or <see cref="CommandResult(bool,string)">the other constructor</see>.</remarks>
    /// <seealso cref="CommandResult(string)"/>
    public static implicit operator CommandResult(string s) => new(s);

    /// <summary>
    /// Creates an empty <see cref="CommandResult"/> with the given success state.
    /// </summary>
    /// <param name="s">The success state.</param>
    /// <returns>A new <see cref="CommandResult"/>.</returns>
    public static implicit operator CommandResult(bool s) => new(s, null);

    /// <summary>
    /// Casts a <see cref="CommandResult"/> to a bool, determining its success state.
    /// </summary>
    /// <param name="r">The <see cref="CommandResult"/> to cast.</param>
    /// <returns>Whether the command succeeded.</returns>
    public static implicit operator bool(CommandResult r) => r.Success;

    #endregion

}
