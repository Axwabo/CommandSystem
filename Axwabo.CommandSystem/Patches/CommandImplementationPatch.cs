using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Axwabo.Helpers.Harmony;
using Axwabo.Helpers.Pools;
using CommandSystem.Commands.Shared;
using HarmonyLib;
using static Axwabo.Helpers.Harmony.InstructionHelper;

namespace Axwabo.CommandSystem.Patches;

[HarmonyPatch(typeof(HelpCommand), nameof(HelpCommand.Execute))]
internal static class CommandImplementationPatch {

    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
        var list = ListPool<CodeInstruction>.Shared.Rent(instructions);
        var index = list.FindIndex(i => i.operand is MethodInfo {Name: "GetType"}) - 1;
        var leave = list.FindCode(OpCodes.Leave_S, start: index);
        var blocks = list[index].ExtractBlocks();
        list.RemoveRange(index, leave - index);
        list.InsertRange(index, new[] {
            Ldarg(3).WithBlocks(blocks),
            Ldarg(3),
            LdindRef,
            Ldloc(0),
            Call(typeof(CommandHelpers), nameof(CommandHelpers.GetUsage)),
            Call<string>(nameof(string.Concat), new[] {typeof(string), typeof(string)}),
            String("\nImplemented in "),
            Ldloc(0),
            Call(typeof(CommandHelpers), nameof(CommandHelpers.GetImplementationLocation)),
            Call<string>(nameof(string.Concat), new[] {typeof(string), typeof(string), typeof(string)}),
            StindRef
        });
        foreach (var codeInstruction in list)
            yield return codeInstruction;
        ListPool<CodeInstruction>.Shared.Return(list);
    }

}
