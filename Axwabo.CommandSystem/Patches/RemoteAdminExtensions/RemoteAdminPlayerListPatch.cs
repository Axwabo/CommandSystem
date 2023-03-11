using System.Collections.Generic;
using System.Reflection;
using Axwabo.CommandSystem.RemoteAdminExtensions;
using HarmonyLib;
using RemoteAdmin.Communication;
using static Axwabo.Helpers.Harmony.InstructionHelper;

namespace Axwabo.CommandSystem.Patches.RemoteAdminExtensions;

[HarmonyPatch(typeof(RaPlayerList), nameof(RaPlayerList.ReceiveData), typeof(CommandSender), typeof(string))]
internal static class RemoteAdminPlayerListPatch {

    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
        if (!Plugin.Instance.Config.EnableRemoteAdminExtensions)
            return instructions;
        var list = new List<CodeInstruction>(instructions);
        var index = list.FindIndex(i => i.operand is MethodInfo {Name: "Rent"}) + 2;
        list.InsertRange(index, new[] {
            Ldarg(1),
            Ldloc(8),
            Int1,
            Call(typeof(RemoteAdminOptionManager), nameof(RemoteAdminOptionManager.AppendAllOptions))
        });
        return list;
    }

}
