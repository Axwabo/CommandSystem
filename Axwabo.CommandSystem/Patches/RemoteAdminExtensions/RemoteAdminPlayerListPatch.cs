extern alias E;
using Axwabo.CommandSystem.RemoteAdminExtensions;
using HarmonyLib;
using RemoteAdmin.Communication;

namespace Axwabo.CommandSystem.Patches.RemoteAdminExtensions;

[HarmonyPatch(typeof(RaPlayerList), nameof(RaPlayerList.ReceiveData), typeof(CommandSender), typeof(string))]
internal static class RemoteAdminPlayerListPatch
{

    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        if (!Plugin.Instance.Config.EnableRemoteAdminExtensions)
            return instructions;
        var list = new List<CodeInstruction>(instructions);
        var index = list.FindIndex(i => i.operand is MethodInfo {Name: nameof(RaPlayerList.SortPlayersDescending)}) + 3;
        list.InsertRange(index, new[]
        {
            Ldarg(1),
            Ldloc(8),
            Int1,
            Int1,
            Call(typeof(RemoteAdminOptionManager), nameof(RemoteAdminOptionManager.AppendAllOptions)),
            Pop
        });
        return list;
    }

}
