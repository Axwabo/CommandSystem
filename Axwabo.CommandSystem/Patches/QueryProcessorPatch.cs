﻿using Axwabo.CommandSystem.Exceptions;
using Axwabo.CommandSystem.Selectors;
using HarmonyLib;
using static Axwabo.CommandSystem.Selectors.PlayerSelectionManager;

namespace Axwabo.CommandSystem.Patches;

[HarmonyPatch(typeof(QueryProcessor), nameof(QueryProcessor.ProcessGameConsoleQuery))]
internal static class QueryProcessorPatch
{

    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var list = new List<CodeInstruction>(instructions);
        var pre = list.FindCode(OpCodes.Ldloc_1);
        list.InsertRange(pre, new[]
        {
            This.MoveBlocksFrom(list[pre]),
            Ldfld<QueryProcessor>(nameof(QueryProcessor._sender)),
            Stfld(typeof(PlayerSelectionManager), nameof(CurrentSender))
        });
        list.InsertRange(list.FindCode(OpCodes.Stloc_2, start: pre) + 1, new[]
        {
            Null,
            Stfld(typeof(PlayerSelectionManager), nameof(CurrentSender)),
            This,
            Ldfld<QueryProcessor>(nameof(QueryProcessor._sender)),
            Ldloc(0),
            Ldloc(3),
            Ldloc(2),
            Call<DeveloperMode>(nameof(DeveloperMode.OnCommandExecuted))
        });

        var failedIndex = list.FindIndex(i => i.operand is CommandExecutionFailedError);
        var failEnd = list.FindCode(OpCodes.Stloc_S, start: failedIndex);
        list.RemoveRange(failedIndex, failEnd - failedIndex);
        list.InsertRange(failedIndex, new[]
        {
            Null,
            Stfld(typeof(PlayerSelectionManager), nameof(CurrentSender)),
            This,
            Ldfld<QueryProcessor>(nameof(QueryProcessor._sender)),
            Ldloc(0),
            Ldloc(4),
            Call<DeveloperMode>(nameof(DeveloperMode.OnExceptionThrown)),
            Ldloc(4),
            Call<PlayerListProcessorException>(nameof(PlayerListProcessorException.CreateMessage))
        });
        return list;
    }

}
