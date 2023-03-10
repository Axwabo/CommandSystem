﻿using System.Collections.Generic;
using System.Reflection.Emit;
using Axwabo.CommandSystem.RemoteAdminExtensions;
using Axwabo.Helpers.Pools;
using HarmonyLib;
using RemoteAdmin.Communication;
using static Axwabo.Helpers.Harmony.InstructionHelper;

namespace Axwabo.CommandSystem.Patches.RemoteAdminExtensions;

[HarmonyPatch(typeof(RaPlayer), nameof(RaPlayer.ReceiveData), typeof(CommandSender), typeof(string))]
internal static class RequestDataPatch {

    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator) {
        var list = ListPool<CodeInstruction>.Shared.Rent(instructions);

        var cfg = Plugin.Instance.Config;
        if (cfg.EnableRemoteAdminExtensions)
            PatchExtensions(list, generator);
        if (cfg.CopyNicknameInsteadOfID)
            PatchNickname(list);

        foreach (var codeInstruction in list)
            yield return codeInstruction;

        ListPool<CodeInstruction>.Shared.Return(list);
    }

    private static void PatchExtensions(List<CodeInstruction> list, ILGenerator generator) {
        var label = generator.DefineLabel();
        var index = list.FindCode(OpCodes.Isinst) + 2;
        var response = generator.Local<string>();
        list[index].labels.Add(label);
        list.InsertRange(index, new[] {
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
            Ldarg(1),
            String("${0} {1}"),
            This,
            Get<RaPlayer>(nameof(RaPlayer.DataId)),
            Box<int>(),
            response.Load(),
            Call<string>(nameof(string.Format), new[] {typeof(string), typeof(object), typeof(object)}),
            Int1,
            Int1,
            Ldfld<string>(nameof(string.Empty)),
            Call<CommandSender>(nameof(CommandSender.RaReply)),
            Return
        });
    }

    private static void PatchNickname(List<CodeInstruction> list) {
        var startIndex = list.FindCall<List<ReferenceHub>>("get_Item");
        var combined = list.FindIndex(i => i.operand is "Nickname: ") + 3;
        list.RemoveAt(combined);
        list.InsertRange(combined, new[] {
            String(" <color=green><link=CP_ID>\uF0C5</link></color>"),
            Call<string>(nameof(string.Concat), new[] {typeof(string), typeof(string), typeof(string)}),
        });

        var playerIdStringIndex = list.FindIndex(startIndex, i => i.operand is string s && s.Contains("Player ID"));
        list[playerIdStringIndex].operand = "\nPlayer ID: {0}";

        var send = list.FindCall<RaClipboard>(nameof(RaClipboard.Send), start: startIndex) - 6;
        list.RemoveRange(send, 6);
        list.InsertRange(send, new[] {
            RaClipboard.RaClipBoardType.PlayerId.Load(),
            Ldloc(9),
            Get<NicknameSync>(nameof(NicknameSync.MyNick))
        });
    }

}
