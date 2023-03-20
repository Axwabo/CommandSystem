extern alias E;
#if EXILED
using E::Axwabo.Helpers.Harmony;
#else
using Axwabo.Helpers.Harmony;
#endif
using System.Collections.Generic;
using System.Reflection.Emit;
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
                ? InstructionHelper.Call(typeof(CommandHelpers), nameof(CommandHelpers.IsHidden))
                : codeInstruction;
    }

}
