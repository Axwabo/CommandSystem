using HarmonyLib;
using RemoteAdmin.Communication;

namespace Axwabo.CommandSystem.Patches.RemoteAdminExtensions;

[HarmonyPatch(typeof(RaPlayerList), nameof(RaPlayerList.ReceiveData), typeof(CommandSender), typeof(string))]
internal static class RemoteAdminPlayerListPatch
{

    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        if (!(CommandSystemPlugin.Instance?.Config?.EnableRemoteAdminExtensions ?? true))
            return instructions;
        var list = new List<CodeInstruction>(instructions);
        var index = list.FindCall(nameof(RaPlayerList.SortPlayersDescending)) + 3;
        list.InsertRange(index, [
            Ldarg(1),
            Ldloc(7),
            Int1,
            Int1,
            Call(typeof(RemoteAdminOptionManager), nameof(RemoteAdminOptionManager.AppendAllOptions)),
            Pop
        ]);
        return list;
    }

}
