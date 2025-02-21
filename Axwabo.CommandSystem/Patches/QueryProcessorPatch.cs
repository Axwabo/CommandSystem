using Axwabo.CommandSystem.Exceptions;
using Axwabo.CommandSystem.Selectors;
using HarmonyLib;
using static Axwabo.CommandSystem.Selectors.PlayerSelectionManager;

namespace Axwabo.CommandSystem.Patches;

[HarmonyPatch(typeof(QueryProcessor), nameof(QueryProcessor.ProcessGameConsoleQuery))]
internal static class QueryProcessorPatch
{

    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var list = new List<CodeInstruction>(instructions);
        var index = list.FindCode(OpCodes.Stloc_0);
        list.InsertRange(index, [
            Duplicate,
            Stfld(typeof(PlayerSelectionManager), nameof(CurrentSender))
        ]);

        var failedIndex = list.FindIndex(i => i.operand is CommandExecutionFailedError);
        var failEnd = list.FindCode(OpCodes.Stloc_S, start: failedIndex);
        list.RemoveRange(failedIndex, failEnd - failedIndex);
        list.InsertRange(failedIndex, [
            Null,
            Stfld(typeof(PlayerSelectionManager), nameof(CurrentSender)),
            Ldloc(0),
            Ldloc(1),
            Ldloc(9),
            Call<DeveloperMode>(nameof(DeveloperMode.OnExceptionThrown)),
            Ldloc(9),
            Call<PlayerListProcessorException>(nameof(PlayerListProcessorException.CreateMessage))
        ]);
        return list;
    }

}
