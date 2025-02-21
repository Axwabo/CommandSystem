using Axwabo.CommandSystem.Commands;
using Axwabo.Helpers.Config.Translations;

namespace Axwabo.CommandSystem.Translations;

/// <summary>
/// A class that associates enum translation keys with command result success states.
/// </summary>
public static class CommandResultTranslationSuccessManager
{

    private static readonly Dictionary<string, bool> SuccessLookup = new();

    /// <summary>
    /// Registers the given key as a successful or unsuccessful command result.
    /// </summary>
    /// <param name="key">The translation key.</param>
    /// <param name="isSuccess">Whether the command result is successful.</param>
    public static void Register(Enum key, bool isSuccess) => Register(key.GetType(), key, isSuccess);

    internal static void Register(Type type, Enum value, bool isSuccess) => SuccessLookup[$"{type.FullName}:{value}"] = isSuccess;

    /// <summary>
    /// Determines whether the given key is registered as a successful command result.
    /// </summary>
    /// <param name="key">The translation key.</param>
    /// <returns>Whether the command result is successful. If the state was not yet registered, the return value will be true.</returns>
    public static bool IsSuccessfulCommandResult(this Enum key) => !SuccessLookup.TryGetValue($"{key.GetType().FullName}:{key}", out var isSuccess) || isSuccess;

    /// <summary>
    /// Attempts to get the registered success state for the given key.
    /// </summary>
    /// <param name="key">The translation key.</param>
    /// <param name="isSuccess">The success state.</param>
    /// <returns>Whether the state was registered.</returns>
    public static bool TryGetCommandResultSuccess(this Enum key, out bool isSuccess) => SuccessLookup.TryGetValue($"{key.GetType().FullName}:{key}", out isSuccess);

    /// <summary>
    /// Translates the given key and returns a command result with the translated text.
    /// </summary>
    /// <param name="key">The translation key.</param>
    /// <typeparam name="T">The enum type.</typeparam>
    /// <returns>The command result.</returns>
    public static CommandResult TranslateResult<T>(this T key) where T : Enum => ToResult(key, key.Translate());

    /// <summary>
    /// Translates the given key and returns a command result with the translated text.
    /// </summary>
    /// <param name="key">The translation key.</param>
    /// <param name="arg0">The first argument.</param>
    /// <typeparam name="T">The enum type.</typeparam>
    /// <returns>The command result.</returns>
    public static CommandResult TranslateResult<T>(this T key, object arg0) where T : Enum => ToResult(key, key.Translate(arg0));

    /// <summary>
    /// Translates the given key and returns a command result with the translated text.
    /// </summary>
    /// <param name="key">The translation key.</param>
    /// <param name="arg0">The first argument.</param>
    /// <param name="arg1">The second argument.</param>
    /// <typeparam name="T">The enum type.</typeparam>
    /// <returns>The command result.</returns>
    public static CommandResult TranslateResult<T>(this T key, object arg0, object arg1) where T : Enum => ToResult(key, key.Translate(arg0, arg1));

    /// <summary>
    /// Translates the given key and returns a command result with the translated text.
    /// </summary>
    /// <param name="key">The translation key.</param>
    /// <param name="arg0">The first argument.</param>
    /// <param name="arg1">The second argument.</param>
    /// <param name="arg2">The third argument.</param>
    /// <typeparam name="T">The enum type.</typeparam>
    /// <returns>The command result.</returns>
    public static CommandResult TranslateResult<T>(this T key, object arg0, object arg1, object arg2) where T : Enum => ToResult(key, key.Translate(arg0, arg1, arg2));

    /// <summary>
    /// Translates the given key and returns a command result with the translated text.
    /// </summary>
    /// <param name="key">The translation key.</param>
    /// <param name="args">The arguments.</param>
    /// <typeparam name="T">The enum type.</typeparam>
    /// <returns>The command result.</returns>
    public static CommandResult TranslateResult<T>(this T key, params object[] args) where T : Enum => ToResult(key, key.Translate(args));

    /// <summary>
    /// Translates the given enum of unknown type and returns a command result with the translated text.
    /// </summary>
    /// <param name="key">The translation key.</param>
    /// <param name="args">The arguments.</param>
    /// <returns>The command result.</returns>
    public static CommandResult TranslateResultRaw(this Enum key, params object[] args) => ToResult(key, key.TranslateRaw(args));

    private static CommandResult ToResult(Enum key, string translation) => !TryGetCommandResultSuccess(key, out var isSuccess)
        ? new CommandResult(translation)
        : new CommandResult(isSuccess, translation);

}
