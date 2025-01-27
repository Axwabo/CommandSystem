using Axwabo.CommandSystem.Selectors;
using HarmonyLib;
using static Axwabo.CommandSystem.Selectors.PlayerSelectionManager;

namespace Axwabo.CommandSystem.Patches;

[HarmonyPatch(typeof(CommandProcessor), nameof(CommandProcessor.ProcessQuery))]
internal static class CommandProcessorPatch
{

    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
    {
        var list = new List<CodeInstruction>(instructions);

        var emptyArrayFix = list.FindCode(OpCodes.Stloc_0) + 1;
        var getCommandLabel = generator.DefineLabel();

        // fixes the case where External Lookup is clicked while not selecting a target (string is empty)
        list[emptyArrayFix].labels.Add(getCommandLabel);
        list.InsertRange(emptyArrayFix, new[]
        {
            Ldloc(0),
            Ldlen,
            getCommandLabel.True(),
            String("Command name was not specified!"),
            Return
        });

        var pre = list.FindCode(OpCodes.Ldloc_1);
        list.InsertRange(pre, new[]
        {
            This.MoveBlocksFrom(list[pre]),
            Ldarg(1),
            Stfld(typeof(PlayerSelectionManager), nameof(CurrentSender))
        });

        var failedIndex = list.FindIndex(i => i.operand is CommandExecutionFailedError);

        var send = list.FindCall("ToUpperInvariant", failedIndex) - 6;
        list.RemoveRange(send, 10);
        list.InsertRange(send, new[]
        {
            Ldloc(0),
            Int0,
            LdelemRef,
            Call<string>(nameof(string.ToUpperInvariant)),
            String("#"),
            Ldloc(12),
            Call<string>(nameof(string.Concat), new[] {typeof(string), typeof(string), typeof(string)}),
            Int0,
            Int1,
            String("")
        });
        return list;
    }

}
