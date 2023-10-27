using Axwabo.CommandSystem.Exceptions;
using Axwabo.CommandSystem.RemoteAdminExtensions;
using Axwabo.CommandSystem.Selectors;
using HarmonyLib;
using static Axwabo.CommandSystem.Selectors.PlayerSelectionManager;
using Console = GameCore.Console;

namespace Axwabo.CommandSystem.Patches;

[HarmonyPatch(typeof(Console), nameof(Console.TypeCommand))]
internal static class ConsolePatch
{

    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var list = new List<CodeInstruction>(instructions);
        var pre = list.FindCode(OpCodes.Ldloc_2);

        list.InsertRange(pre, new[]
        {
            This.MoveBlocksFrom(list[pre]),
            Ldarg(2),
            Stfld(typeof(PlayerSelectionManager), nameof(CurrentSender))
        });
        list.InsertRange(list.FindCode(OpCodes.Stloc_S, start: pre) + 1, new[]
        {
            Null,
            Stfld(typeof(PlayerSelectionManager), nameof(CurrentSender)),
            Ldarg(2),
            Ldloc(1),
            Ldloc(10),
            Ldloc(9),
            Call<DeveloperMode>(nameof(DeveloperMode.OnCommandExecuted))
        });

        var failedIndex = list.FindIndex(i => i.operand is CommandExecutionFailedError);
        list.RemoveRange(failedIndex, 6);
        list.InsertRange(failedIndex, new[]
        {
            Null,
            Stfld(typeof(PlayerSelectionManager), nameof(CurrentSender)),
            Ldarg(2),
            Ldloc(1),
            Ldloc(12),
            Call<DeveloperMode>(nameof(DeveloperMode.OnExceptionThrown)),
            Ldloc(12),
            Call<PlayerListProcessorException>(nameof(PlayerListProcessorException.CreateMessage)),
            Stloc(13)
        });
        return list;
    }

}
