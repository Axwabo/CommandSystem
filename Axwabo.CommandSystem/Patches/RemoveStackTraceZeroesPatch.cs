using System.Collections.Generic;
using System.Text.RegularExpressions;
using HarmonyLib;
using static Axwabo.Helpers.Harmony.InstructionHelper;

namespace Axwabo.CommandSystem.Patches;

[HarmonyPatch(typeof(Misc), nameof(Misc.RemoveStacktraceZeroes))]
public static class RemoveStackTraceZeroesPatch {

    public static readonly Regex ReplaceRegex = new("\\s\\[(?:[0-9a-fx]+)\\] in <(?:[0-9a-f]+)>:0", RegexOptions.Compiled | RegexOptions.CultureInvariant);

    private static readonly CodeInstruction[] Instructions = {
        Ldarg(0),
        Call(typeof(RemoveStackTraceZeroesPatch), nameof(Replace)),
        Return
    };

    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) => Instructions;

    public static string Replace(string input) => ReplaceRegex.Replace(input, "");

}
