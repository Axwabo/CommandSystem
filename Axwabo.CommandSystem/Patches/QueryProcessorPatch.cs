using System.Collections.Generic;
using System.Reflection.Emit;
using Axwabo.CommandSystem.Selectors;
using Axwabo.Helpers.Harmony;
using Axwabo.Helpers.Pools;
using HarmonyLib;
using RemoteAdmin;
using static Axwabo.CommandSystem.Selectors.PlayerSelectionManager;
using static Axwabo.Helpers.Harmony.InstructionHelper;

namespace Axwabo.CommandSystem.Patches;

[HarmonyPatch(typeof(QueryProcessor), nameof(QueryProcessor.ProcessGameConsoleQuery))]
internal static class QueryProcessorPatch {

    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
        var list = ListPool<CodeInstruction>.Shared.Rent(instructions);
        var pre = list.FindCode(OpCodes.Ldloc_1);
        list.InsertRange(pre, new[] {
            This.MoveBlocksFrom(list[pre]),
            Ldfld<QueryProcessor>(nameof(QueryProcessor._sender)),
            Stfld(typeof(PlayerSelectionManager), nameof(CurrentSender))
        });
        list.InsertRange(list.FindCode(OpCodes.Stloc_2, start: pre) + 1, new[] {
            Null,
            Stfld(typeof(PlayerSelectionManager), nameof(CurrentSender))
        });
        list.InsertRange(list.FindIndex(i => i.operand is CommandExecutionFailedError), new[] {
            Null,
            Stfld(typeof(PlayerSelectionManager), nameof(CurrentSender))
        });
        foreach (var codeInstruction in list)
            yield return codeInstruction;
        ListPool<CodeInstruction>.Shared.Return(list);
    }

}
