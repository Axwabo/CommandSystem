using System.Collections.Generic;
using System.Text.RegularExpressions;
using HarmonyLib;
using static Axwabo.Helpers.Harmony.InstructionHelper;

namespace Axwabo.CommandSystem.Patches;

/// <summary>
/// A patch that removes the IL offsets from stack traces when using the <see cref="Misc.RemoveStacktraceZeroes"/> method.
/// </summary>
[HarmonyPatch(typeof(Misc), nameof(Misc.RemoveStacktraceZeroes))]
public static class RemoveStackTraceZeroesPatch
{

    private static readonly Regex ReplaceRegex = new("\\s\\[(?:[0-9a-fx]+)\\] in <(?:[0-9a-f]+)>:0", RegexOptions.Compiled | RegexOptions.CultureInvariant);

    private static readonly CodeInstruction[] Instructions =
    {
        Ldarg(0),
        Call(typeof(RemoveStackTraceZeroesPatch), nameof(StripILOffsets)),
        Return
    };

    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) => Instructions;

    /// <summary>
    /// Removes all IL offsets from given the stack trace string.
    /// </summary>
    /// <param name="input">The stack trace string.</param>
    /// <returns>The stripped stack trace.</returns>
    public static string StripILOffsets(string input) => ReplaceRegex.Replace(input, "");

}
