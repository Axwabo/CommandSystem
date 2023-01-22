using System.Collections.Generic;
using System.Reflection.Emit;
using Axwabo.CommandSystem.Selectors;
using Axwabo.Helpers.Pools;
using HarmonyLib;
using Utils;
using static Axwabo.Helpers.Harmony.InstructionHelper;

namespace Axwabo.CommandSystem.Patches {

    [HarmonyPatch(typeof(RAUtils), nameof(RAUtils.ProcessPlayerIdOrNamesList))]
    internal static class ProcessPlayersListPatch {

        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator) {
            var list = ListPool<CodeInstruction>.Shared.Rent(instructions);
            var targets = generator.DeclareLocal(typeof(List<ReferenceHub>));
            var label = generator.DefineLabel();
            var entryPoint = list[0].WithLabels(label);
            list.InsertRange(0, new[] {
                Ldarg(0).MoveBlocksFrom(entryPoint),
                Ldarg(1),
                Ldarg(3),
                targets.LoadAddress(),
                Ldarg(2),
                Call(typeof(PlayerSelectionManager), nameof(PlayerSelectionManager.TryProcessPlayersCustom)),
                label.False(),
                targets.Load(),
                Stloc(7),
                ((Label) list[list.FindCode(OpCodes.Leave)].operand).Jump()
            });
            foreach (var codeInstruction in list)
                yield return codeInstruction;
        }

    }

}
