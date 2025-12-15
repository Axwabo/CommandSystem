using Axwabo.CommandSystem.Exceptions;
using Axwabo.CommandSystem.Selectors;
using HarmonyLib;
using static Axwabo.CommandSystem.Selectors.PlayerSelectionManager;
using Console = GameCore.Console;

namespace Axwabo.CommandSystem.Patches;

[HarmonyPatch(typeof(Console), nameof(Console.TypeCommand))]
internal static class ConsolePatch
{

    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
    {
        var list = new List<CodeInstruction>(instructions);
        var array = generator.Local<string[]>();
        var setSender = list.FindCall("get_Sender") + 1;

        list.InsertRange(setSender, [
            Duplicate,
            Stfld(typeof(PlayerSelectionManager), nameof(CurrentSender))
        ]);

        var split = list.FindCall(nameof(string.Split));
        list.InsertRange(split + 2, [
            Duplicate,
            array.Set()
        ]);

        var failedIndex = list.FindIndex(i => i.operand is CommandExecutionFailedError);
        var failEnd = list.FindCode(OpCodes.Stloc_S, start: failedIndex);
        list.RemoveRange(failedIndex, failEnd - failedIndex);
        list.InsertRange(failedIndex, [
            Ldarg(2),
            array.Load(),
            Ldloc(13),
            Call<DeveloperMode>(nameof(DeveloperMode.OnExceptionThrown)),
            Ldloc(13),
            Call<PlayerListProcessorException>(nameof(PlayerListProcessorException.CreateMessage))
        ]);
        return list;
    }

}
