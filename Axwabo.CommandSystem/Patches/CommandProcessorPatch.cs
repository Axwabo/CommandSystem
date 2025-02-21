using Axwabo.CommandSystem.Exceptions;
using Axwabo.CommandSystem.Selectors;
using HarmonyLib;
using static Axwabo.CommandSystem.Selectors.PlayerSelectionManager;

namespace Axwabo.CommandSystem.Patches;

[HarmonyPatch(typeof(CommandProcessor), nameof(CommandProcessor.ProcessQuery))]
internal static class CommandProcessorPatch
{

    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var list = new List<CodeInstruction>(instructions);
        var setSender = list.FindCode(OpCodes.Starg_S);
        list.InsertRange(setSender, [
            Duplicate,
            Stfld(typeof(PlayerSelectionManager), nameof(CurrentSender))
        ]);

        var failedIndex = list.FindIndex(i => i.operand is CommandExecutionFailedError);
        var failEnd = list.FindCode(OpCodes.Stloc_S, start: failedIndex);
        list.RemoveRange(failedIndex, failEnd - failedIndex);
        list.InsertRange(failedIndex, [
            Ldarg(1),
            Ldloc(0),
            Ldloc(11),
            Call<DeveloperMode>(nameof(DeveloperMode.OnExceptionThrown)),
            Ldloc(11),
            Call<PlayerListProcessorException>(nameof(PlayerListProcessorException.CreateMessage))
        ]);
        return list;
    }

}
