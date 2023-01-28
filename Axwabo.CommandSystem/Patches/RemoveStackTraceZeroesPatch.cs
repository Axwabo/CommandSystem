using System.Collections.Generic;
using System.Text.RegularExpressions;
using HarmonyLib;
using static Axwabo.Helpers.Harmony.InstructionHelper;

namespace Axwabo.CommandSystem.Patches;

[HarmonyPatch(typeof(Misc), nameof(Misc.RemoveStacktraceZeroes))]
public static class RemoveStackTraceZeroesPatch {

    public static readonly Regex ReplaceRegex = new(" \\[(?:[0-9a-fx]+)\\] in <(?:[0-9a-f]+)>:0", RegexOptions.Compiled);

    private static readonly CodeInstruction[] Instructions = {
        Ldfld(typeof(RemoveStackTraceZeroesPatch), nameof(ReplaceRegex)),
        Ldarg(0),
        String(""),
        Call<Regex>(nameof(Regex.Replace), new[] {typeof(string), typeof(string)}),
        Return
    };

    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) => Instructions;

}
