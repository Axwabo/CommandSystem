using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Axwabo.CommandSystem.Selectors;
using Axwabo.Helpers.Pools;
using HarmonyLib;
using Utils;
using static Axwabo.Helpers.Harmony.InstructionHelper;

namespace Axwabo.CommandSystem.Patches;

[HarmonyPatch(typeof(RAUtils), nameof(RAUtils.ProcessPlayerIdOrNamesList))]
internal static class ProcessPlayersListPatch {

    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator) {
        var list = ListPool<CodeInstruction>.Shared.Rent(instructions);
        var targets = generator.DeclareLocal(typeof(List<ReferenceHub>));
        var label = generator.DefineLabel();
        list[0].labels.Add(label);
        list.InsertRange(0, new[] {
            Ldarg(0),
            Ldarg(1),
            Ldarg(3),
            targets.LoadAddress(),
            Ldarg(2),
            Call(typeof(PlayerSelectionManager), nameof(PlayerSelectionManager.TryProcessPlayersCustom)),
            label.False(),
            targets.Load(),
            Return
        });

        var atProcessor = list.FindCode(OpCodes.Ldstr) - 1;
        var atEnd = list.FindCode(OpCodes.Leave, start: list.FindCode(OpCodes.Newarr));
        list.RemoveRange(atProcessor, atEnd - atProcessor + 1);

        var digitCheck = list.FindCode(OpCodes.Ble_S) + 1;
        list.RemoveRange(digitCheck, list.FindCode(OpCodes.Brfalse_S, start: digitCheck) - digitCheck + 1);

        var parseSingle = list.FindIndex(i => i.operand is MethodInfo {Name: nameof(int.TryParse)}) - 4;
        var loopStart = Ldloc(1).MoveLabelsFrom(list[parseSingle]).MoveBlocksFrom(list[parseSingle]);
        list.RemoveRange(parseSingle, 17);
        list.InsertRange(parseSingle, new[] {
            loopStart,
            Ldloc(8),
            Ldloc(9),
            LdelemRef,
            Ldloc(1),
            Call(typeof(PlayerSelectionManager), nameof(PlayerSelectionManager.ParseSingleQuery)),
        });
        foreach (var codeInstruction in list)
            yield return codeInstruction;
    }

}
