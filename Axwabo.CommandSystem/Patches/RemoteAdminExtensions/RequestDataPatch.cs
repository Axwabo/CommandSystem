using Axwabo.CommandSystem.RemoteAdminExtensions;
using HarmonyLib;
using RemoteAdmin.Communication;

namespace Axwabo.CommandSystem.Patches.RemoteAdminExtensions;

[HarmonyPatch(typeof(RaPlayer), nameof(RaPlayer.ReceiveData), typeof(CommandSender), typeof(string))]
internal static class RequestDataPatch
{

    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
    {
        var list = new List<CodeInstruction>(instructions);
        var cfg = Plugin.Instance.Config;
        if (cfg.EnableRemoteAdminExtensions)
            PatchExtensions(list, generator);
        if (cfg.CopyNicknameInsteadOfId)
            PatchNickname(list);
        return list;
    }

    private static void PatchExtensions(List<CodeInstruction> list, ILGenerator generator)
    {
        var label = generator.DefineLabel();
        var index = list.FindCode(OpCodes.Isinst) + 2;
        var response = generator.Local<string>();
        list[index].labels.Add(label);
        list.InsertRange(index, new[]
        {
            Ldloc(0),
            Int1,
            LdelemRef,
            Ldloc(2),
            Int0,
            Equality,
            Ldloc(3),
            response.LoadAddress(),
            Call(typeof(RemoteAdminOptionManager), nameof(RemoteAdminOptionManager.HandleCustomRequest)),
            label.False(),
            This,
            Ldarg(1),
            response.Load(),
            Call(typeof(RemoteAdminOptionManager), nameof(RemoteAdminOptionManager.SendReply)),
            Return
        });
    }

    private static void PatchNickname(List<CodeInstruction> list)
    {
        var startIndex = list.FindCall<List<ReferenceHub>>("get_Item");
        var combined = list.FindIndex(i => i.operand is "Nickname: ") + 3;
        list.RemoveAt(combined);
        list.InsertRange(combined, new[]
        {
            String(" <color=green><link=CP_ID>\uF0C5</link></color>"),
            Call<string>(nameof(string.Concat), new[] {typeof(string), typeof(string), typeof(string)}),
        });

        var playerIdStringIndex = list.FindIndex(startIndex, i => i.operand is string s && s.Contains("Player ID"));
        list[playerIdStringIndex].operand = "\nPlayer ID: {0}";

        var send = list.FindCall<RaClipboard>(nameof(RaClipboard.Send), start: startIndex) - 6;
        list.RemoveRange(send, 6);
        list.InsertRange(send, new[]
        {
            RaClipboard.RaClipBoardType.PlayerId.Load(),
            Ldloc(9),
            Get<NicknameSync>(nameof(NicknameSync.MyNick))
        });
    }

}
