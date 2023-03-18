using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Axwabo.Helpers.Pools;
using CommandSystem.Commands.Shared;
using HarmonyLib;
using static Axwabo.Helpers.Harmony.InstructionHelper;

namespace Axwabo.CommandSystem.Patches;

[HarmonyPatch(typeof(HelpCommand), nameof(HelpCommand.Execute))]
internal static class HelpCommandPatch
{

    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var list = ListPool<CodeInstruction>.Shared.Rent(instructions);

        var start = list.FindIndex(i => i.operand is MethodInfo {Name: "get_Command"}) - 1;
        var end = list.FindLastIndex(i => i.opcode == OpCodes.Leave_S) + 1;
        var labels = list[start].ExtractLabels();
        list.RemoveRange(start, end - start);
        list.InsertRange(start, new[]
        {
            Ldarg(3).WithLabels(labels),
            Ldloc(0),
            Ldarg(1),
            Call(typeof(CommandHelpers), nameof(CommandHelpers.GetHelpForCommand)),
            StindRef
        });

        foreach (var codeInstruction in list)
            yield return codeInstruction;
        ListPool<CodeInstruction>.Shared.Return(list);
    }

}
