#pragma warning disable CS1591
using System.Text.RegularExpressions;
using HarmonyLib;

namespace Axwabo.CommandSystem.Patches;

/// <summary>
/// A patch that removes the IL offsets from stack traces when using the <see cref="Misc.RemoveStacktraceZeroes"/> method.
/// </summary>
[HarmonyPatch(typeof(Misc), nameof(Misc.RemoveStacktraceZeroes))]
public static class RemoveStackTraceZeroesPatch
{

    private static readonly Regex ReplaceRegex = new(@"\s?\[[0-9a-fx]+\] in <[0-9a-f]+>:0", RegexOptions.Compiled | RegexOptions.CultureInvariant);

    private static readonly CodeInstruction[] Instructions =
    [
        Ldarg(0),
        Call(typeof(RemoveStackTraceZeroesPatch), nameof(StripILOffsets)),
        Return
    ];

    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        => CommandSystemPlugin.Instance?.Config?.StripIntermediateLanguageOffsets ?? false ? Instructions : instructions;

    /// <summary>
    /// Removes all IL offsets from given the stack trace string.
    /// </summary>
    /// <param name="input">The stack trace string.</param>
    /// <returns>The stripped stack trace.</returns>
    public static string StripILOffsets(string input) => ReplaceRegex.Replace(input, "");

}
