﻿extern alias E;
using CommandSystem.Commands.Shared;
using HarmonyLib;

namespace Axwabo.CommandSystem.Patches;

[HarmonyPatch(typeof(HelpCommand), nameof(HelpCommand.GetCommandList))]
internal static class GetCommandListPatch
{

    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        foreach (var codeInstruction in instructions)
            yield return codeInstruction.opcode == OpCodes.Isinst
                ? Call(typeof(CommandHelpers), nameof(CommandHelpers.IsHidden))
                : codeInstruction;
    }

}
