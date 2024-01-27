using CommandSystem;
using HarmonyLib;

namespace Axwabo.CommandSystem.Patches;

[HarmonyPatch(typeof(QueryProcessor), nameof(QueryProcessor.ParseCommandsToStruct))]
internal static class ParseCommandsPatch
{

    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var list = new List<CodeInstruction>(instructions);
        var index = list.FindIndex(i => i.opcode == OpCodes.Isinst && i.operand as Type == typeof(IHiddenCommand));
        list.RemoveRange(index, 3);
        list.Insert(index, Call(typeof(CommandHelpers), nameof(CommandHelpers.IsHidden)));
        return list;
    }

}
