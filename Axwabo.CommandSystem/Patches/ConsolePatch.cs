using Axwabo.CommandSystem.Exceptions;
using Axwabo.CommandSystem.Selectors;
using HarmonyLib;
using LabApi.Events.Arguments.ServerEvents;
using static Axwabo.CommandSystem.Selectors.PlayerSelectionManager;
using Console = GameCore.Console;

namespace Axwabo.CommandSystem.Patches;

[HarmonyPatch(typeof(Console), nameof(Console.TypeCommand))]
internal static class ConsolePatch
{

    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var list = new List<CodeInstruction>(instructions);
        var setSender = list.FindCall<CommandExecutingEventArgs>("get_Sender") + 1;

        list.InsertRange(setSender, [
            Duplicate,
            Stfld(typeof(PlayerSelectionManager), nameof(CurrentSender))
        ]);

        var failedIndex = list.FindIndex(i => i.operand is CommandExecutionFailedError);
        var failEnd = list.FindCode(OpCodes.Stloc_S, start: failedIndex);
        list.RemoveRange(failedIndex, failEnd - failedIndex);
        list.InsertRange(failedIndex, [
            Ldarg(2),
            Ldloc(1),
            Ldloc(15),
            Call<DeveloperMode>(nameof(DeveloperMode.OnExceptionThrown)),
            Ldloc(15),
            Call<PlayerListProcessorException>(nameof(PlayerListProcessorException.CreateMessage)),
        ]);
        return list;
    }

}
