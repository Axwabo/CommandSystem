﻿using HarmonyLib;
using RemoteAdmin.Communication;

namespace Axwabo.CommandSystem.Patches.RemoteAdminExtensions;

[HarmonyPatch(typeof(RaPlayer), nameof(RaPlayer.ReceiveData), typeof(CommandSender), typeof(string))]
internal static class RequestDataPatch
{

    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
    {
        var list = new List<CodeInstruction>(instructions);
        var cfg = CommandSystemPlugin.Instance.Config;
        if (cfg?.EnableRemoteAdminExtensions ?? true)
            PatchExtensions(list, generator);
        if (cfg?.CopyNicknameInsteadOfId ?? false)
            PatchNickname(list);
        return list;
    }

    private static void PatchExtensions(List<CodeInstruction> list, ILGenerator generator)
    {
        var label = generator.DefineLabel();
        var index = list.FindCode(OpCodes.Isinst) + 2;
        var response = generator.Local<string>();
        list[index].labels.Add(label);
        list.InsertRange(index, [
            Ldloc(0),
            Int1,
            LdelemRef,
            Ldloc(2),
            Int0,
            Equality,
            Ldloc(3),
            response.LoadAddress(),
            Call(RemoteAdminOptionManager.HandleCustomRequest),
            label.False(),
            This,
            Ldarg(1),
            response.Load(),
            Call(RemoteAdminOptionManager.SendReply),
            Return
        ]);
    }

    private static void PatchNickname(List<CodeInstruction> list)
    {
        var startIndex = list.FindCall("get_Item");
        var nicknameIndex = list.FindIndex(i => i.operand is string s && s.Contains("Nickname:"));
        list[nicknameIndex].operand = "<color=white>Nickname: {0} <color=green><link=CP_ID>\uF0C5</link></color>";

        var playerIdStringIndex = list.FindIndex(startIndex, i => i.operand is string s && s.Contains("Player ID"));
        list[playerIdStringIndex].operand = "\nPlayer ID: {0}";

        var send = list.FindCall<RaClipboard>(nameof(RaClipboard.Send), start: startIndex) - 5;
        list.RemoveRange(send, 5);
        list.InsertRange(send, [
            Ldloc(10),
            Get<NicknameSync>(nameof(NicknameSync.MyNick))
        ]);
    }

}
