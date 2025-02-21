using Axwabo.CommandSystem.Commands;
using CommandSystem.Commands.Shared;
using HarmonyLib;

namespace Axwabo.CommandSystem.Patches;

[HarmonyPatch(typeof(HelpCommand), nameof(HelpCommand.Execute))]
internal static class HelpCommandPatch
{

    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var list = new List<CodeInstruction>(instructions);
        var start = list.FindCall("get_Command") - 1;
        var end = list.FindLastIndex(i => i.opcode == OpCodes.Leave_S) + 1;
        var labels = list[start].ExtractLabels();
        list.RemoveRange(start, end - start);
        list.InsertRange(start, [
            Ldarg(3).WithLabels(labels),
            Ldloc(0),
            Ldarg(1),
            Call(typeof(CommandHelpers), nameof(CommandHelpers.GetHelpForCommand)),
            StindRef
        ]);
        return list;
    }

}
