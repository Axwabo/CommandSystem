using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Axwabo.CommandSystem.Exceptions;
using Axwabo.CommandSystem.Selectors;
using Axwabo.Helpers.Pools;
using HarmonyLib;
using RemoteAdmin;
using static Axwabo.CommandSystem.Selectors.PlayerSelectionManager;
using static Axwabo.Helpers.Harmony.InstructionHelper;

namespace Axwabo.CommandSystem.Patches;

[HarmonyPatch(typeof(CommandProcessor), nameof(CommandProcessor.ProcessQuery))]
internal static class CommandProcessorPatch {

    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
        var list = ListPool<CodeInstruction>.Shared.Rent(instructions);
        var pre = list.FindCode(OpCodes.Ldloc_2);
        list.InsertRange(pre, new[] {
            This.MoveBlocksFrom(list[pre]),
            Ldarg(1),
            Stfld(typeof(PlayerSelectionManager), nameof(CurrentSender))
        });
        list.InsertRange(list.FindCode(OpCodes.Stloc_S, start: pre) + 1, new[] {
            Null,
            Stfld(typeof(PlayerSelectionManager), nameof(CurrentSender))
        });
        var failedIndex = list.FindIndex(i => i.operand is CommandExecutionFailedError);
        list.RemoveRange(failedIndex, 6);
        list.InsertRange(failedIndex, new[] {
            Null,
            Stfld(typeof(PlayerSelectionManager), nameof(CurrentSender)),
            Ldloc(11),
            Call<PlayerListProcessorException>(nameof(PlayerListProcessorException.CreateMessage)),
            Stloc(12)
        });
        var send = list.FindIndex(failedIndex, i => i.operand is MethodInfo {Name: "ToUpperInvariant"}) - 6;
        list.RemoveRange(send, 10);
        list.InsertRange(send, new[] {
            Ldloc(1),
            Int0,
            LdelemRef,
            Call<string>(nameof(string.ToUpperInvariant)),
            String("#"),
            Ldloc(12),
            Call<string>(nameof(string.Concat), new[] {typeof(string), typeof(string), typeof(string)}),
            Int0,
            Int1,
            String("")
        });
        foreach (var codeInstruction in list)
            yield return codeInstruction;
        ListPool<CodeInstruction>.Shared.Return(list);
    }

}
