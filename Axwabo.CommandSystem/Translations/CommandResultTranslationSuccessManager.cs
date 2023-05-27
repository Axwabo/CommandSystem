extern alias E;

#if EXILED
using E::Axwabo.Helpers.Config.Translations;

#else
using Axwabo.Helpers.Config.Translations;
#endif

namespace Axwabo.CommandSystem.Translations;

public static class CommandResultTranslationSuccessManager
{

    private static readonly Dictionary<string, bool> SuccessLookup = new();

    public static void Register(Enum key, bool isSuccess) => Register(key.GetType(), key, isSuccess);

    internal static void Register(Type type, Enum value, bool isSuccess) => SuccessLookup[$"{type.FullName}:{value}"] = isSuccess;

    public static bool IsSuccessfulResult(this Enum key) => !SuccessLookup.TryGetValue($"{key.GetType().FullName}:{key}", out var isSuccess) || isSuccess;

    public static bool TryGetResultSuccess(this Enum key, out bool isSuccess) => SuccessLookup.TryGetValue($"{key.GetType().FullName}:{key}", out isSuccess);

    public static CommandResult TranslateResult<T>(this T key) where T : Enum => ToResult(key, key.Translate());

    public static CommandResult TranslateResult<T>(this T key, object arg0) where T : Enum => ToResult(key, key.Translate(arg0));

    public static CommandResult TranslateResult<T>(this T key, object arg0, object arg1) where T : Enum => ToResult(key, key.Translate(arg0, arg1));

    public static CommandResult TranslateResult<T>(this T key, object arg0, object arg1, object arg2) where T : Enum => ToResult(key, key.Translate(arg0, arg1, arg2));

    public static CommandResult TranslateResult<T>(this T key, params object[] args) where T : Enum => ToResult(key, key.Translate(args));

    public static CommandResult TranslateResultRaw(this Enum key, params object[] args) => ToResult(key, key.TranslateRaw(args));

    private static CommandResult ToResult(Enum key, string translation) => !TryGetResultSuccess(key, out var isSuccess)
        ? new CommandResult(translation)
        : new CommandResult(isSuccess, translation);

}
