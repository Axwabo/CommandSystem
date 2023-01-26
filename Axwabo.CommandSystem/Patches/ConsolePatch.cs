using System.Collections.Generic;
using System.Reflection.Emit;
using Axwabo.CommandSystem.Selectors;
using Axwabo.Helpers.Pools;
using GameCore;
using HarmonyLib;
using static Axwabo.CommandSystem.Selectors.PlayerSelectionManager;
using static Axwabo.Helpers.Harmony.InstructionHelper;

namespace Axwabo.CommandSystem.Patches;

[HarmonyPatch(typeof(Console), nameof(Console.TypeCommand))]
internal static class ConsolePatch {

    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
        var list = ListPool<CodeInstruction>.Shared.Rent(instructions);
        var pre = list.FindCode(OpCodes.Ldloc_2);
        list.InsertRange(pre, new[] {
            This.MoveBlocksFrom(list[pre]),
            Ldarg(2),
            Stfld(typeof(PlayerSelectionManager), nameof(CurrentSender))
        });
        list.InsertRange(list.FindCode(OpCodes.Stloc_S, start: pre) + 1, new[] {
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
