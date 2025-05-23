﻿using HarmonyLib;
using RemoteAdmin.Communication;

namespace Axwabo.CommandSystem.Patches.RemoteAdminExtensions;

[HarmonyPatch(typeof(RaPlayerAuth), nameof(RaPlayerAuth.ReceiveData))]
internal static class RequestAuthPatch
{

    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
    {
        if (!(CommandSystemPlugin.Instance?.Config?.EnableRemoteAdminExtensions ?? true))
            return instructions;

        var list = new List<CodeInstruction>(instructions);
        var label = generator.DefineLabel();
        var index = list.FindCode(OpCodes.Isinst) - 1;
        var instruction = list[index];
        var response = generator.Local<string>();
        list.InsertRange(index, [
            Ldarg(2).MoveLabelsFrom(instruction),
            Call<string>(nameof(string.Trim), []),
            RequestDataButton.RequestAuth.Load(),
            Ldarg(1),
            IsInstance<PlayerCommandSender>(),
            response.LoadAddress(),
            Call(typeof(RemoteAdminOptionManager), nameof(RemoteAdminOptionManager.HandleCustomRequest)),
            label.False(),
            Ldarg(1),
            String("PlayerInfo#<color=white>{0}</color>"),
            response.Load(),
            Call<string>(nameof(string.Format), [typeof(string), typeof(object)]),
            Int1,
            Int1,
            String("null"),
            Call<CommandSender>(nameof(CommandSender.RaReply)),
            Return
        ]);
        instruction.labels.Add(label);
        return list;
    }

}
