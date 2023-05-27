using Axwabo.CommandSystem.Translations;
using Axwabo.Helpers.Config.Translations;

namespace Axwabo.CommandSystem;

/// <summary>
/// Represents the result of a command execution.
/// </summary>
public readonly struct CommandResult
{

    #region Static

    private const string InvalidCast = "Implicit cast of a null string to a CommandResult. If you want to return a nullable CommandResult, use \"CommandResult.Null\". If you want to return an empty response, use the constructor or the \"Succeeded\" method.";

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
    public CommandResult(string response)
    {
        var success = string.IsNullOrEmpty(response) || !response.StartsWith("!");
        Response = success ? response : response.Substring(1);
        Success = success;
    }

    /// <summary>
    /// Creates a new <see cref="CommandResult"/> instance with a response.
    /// </summary>
    /// <param name="success">Whether the command was successful.</param>
    /// <param name="response">The response of the command.</param>
    public CommandResult(bool success, string response)
    {
        Response = response;
        Success = success;
    }

    #endregion

    #region Casts

    /// <summary>
    /// Casts a string to a <see cref="CommandResult"/> with the string as the response.
    /// </summary>
    /// <param name="response">The string to cast.</param>
    /// <returns>A new <see cref="CommandResult"/>.</returns>
    /// <remarks>If the response starts with '!', it will be treated as a failing response. If you want your response to start with '!', use <see cref="Succeeded"/> or <see cref="CommandResult(bool,string)">the other constructor</see>.</remarks>
    /// <seealso cref="CommandResult(string)"/>
    public static implicit operator CommandResult(string response) => new(response ?? throw new ArgumentNullException(nameof(response), InvalidCast));

    /// <summary>
    /// Creates an empty <see cref="CommandResult"/> with the given success state.
    /// </summary>
    /// <param name="success">The success state.</param>
    /// <returns>A new <see cref="CommandResult"/>.</returns>
    public static implicit operator CommandResult(bool success) => new(success, null);

    /// <summary>
    /// Creates a <see cref="CommandResult"/> from a tuple of a string and a bool.
    /// </summary>
    /// <param name="tuple">The tuple to cast.</param>
    /// <returns>A new <see cref="CommandResult"/>.</returns>
    public static implicit operator CommandResult((string, bool) tuple) => new(tuple.Item2, tuple.Item1);

    /// <summary>
    /// Creates a <see cref="CommandResult"/> from a tuple of a bool and a string.
    /// </summary>
    /// <param name="tuple">The tuple to cast.</param>
    /// <returns>A new <see cref="CommandResult"/>.</returns>
    public static implicit operator CommandResult((bool, string) tuple) => new(tuple.Item1, tuple.Item2);

    /// <summary>
    /// Casts a <see cref="CommandResult"/> to a bool, determining its success state.
    /// </summary>
    /// <param name="result">The <see cref="CommandResult"/> to cast.</param>
    /// <returns>Whether the command succeeded.</returns>
    public static implicit operator bool(CommandResult result) => result.Success;

    /// <summary>
    /// Casts a <see cref="CommandResult"/> to a string, returning its response.
    /// </summary>
    /// <param name="result">The <see cref="CommandResult"/> to cast.</param>
    /// <returns>The response of the command.</returns>
    /// <seealso cref="IsEmpty"/>
    /// <seealso cref="Response"/>
    public static implicit operator string(CommandResult result) => result.Response;

    /// <summary>
    /// Converts the given translatable enum to a <see cref="CommandResult"/> with the translated response.
    /// </summary>
    /// <param name="translation">The response to translate.</param>
    /// <returns>A new <see cref="CommandResult"/>.</returns>
    /// <seealso cref="CommandResultTranslationSuccessManager.TranslateResultRaw"/>
    /// <seealso cref="TranslationHelper.TranslateRaw"/>
    /// <seealso cref="CommandResultTranslationAttribute"/>
    /// <seealso cref="TranslationAttribute"/>
    public static implicit operator CommandResult(Enum translation) => translation.TranslateResultRaw();

    /// <inheritdoc cref="op_Implicit(Enum)"/>
    public static implicit operator CommandResult((Enum, object) translation) => translation.Item1.TranslateResultRaw(translation.Item2);

    /// <inheritdoc cref="op_Implicit(Enum)"/>
    public static implicit operator CommandResult((Enum, object, object) translation) => translation.Item1.TranslateResultRaw(translation.Item2, translation.Item3);

    /// <inheritdoc cref="op_Implicit(Enum)"/>
    public static implicit operator CommandResult((Enum, object, object, object) translation) => translation.Item1.TranslateResultRaw(translation.Item2, translation.Item3, translation.Item4);

    /// <inheritdoc cref="op_Implicit(Enum)"/>
    public static implicit operator CommandResult((Enum, object, object, object, object) translation) => translation.Item1.TranslateResultRaw(translation.Item2, translation.Item3, translation.Item4, translation.Item5);

    #endregion

}
