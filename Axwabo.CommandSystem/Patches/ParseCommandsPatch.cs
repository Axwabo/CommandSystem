using CommandSystem;
using HarmonyLib;
using RemoteAdmin;

namespace Axwabo.CommandSystem.Patches;

[HarmonyPatch(typeof(QueryProcessor), nameof(QueryProcessor.ParseCommandsToStruct))]
internal static class ParseCommandsPatch
{

    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var list = ListPool<CodeInstruction>.Shared.Rent(instructions);
        var index = list.FindIndex(i => i.opcode == OpCodes.Isinst
                                        && i.operand as Type == typeof(IHiddenCommand));
        list.RemoveRange(index, 3);
        list.Insert(index, Call(typeof(CommandHelpers), nameof(CommandHelpers.IsHidden)));
        foreach (var codeInstruction in list)
            yield return codeInstruction;
        ListPool<CodeInstruction>.Shared.Return(list);
    }

}
