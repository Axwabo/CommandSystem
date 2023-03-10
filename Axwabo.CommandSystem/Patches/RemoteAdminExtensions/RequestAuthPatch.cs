﻿using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using Axwabo.CommandSystem.RemoteAdminExtensions;
using HarmonyLib;
using RemoteAdmin;
using RemoteAdmin.Communication;
using static Axwabo.Helpers.Harmony.InstructionHelper;

namespace Axwabo.CommandSystem.Patches.RemoteAdminExtensions;

[HarmonyPatch(typeof(RaPlayerAuth), nameof(RaPlayerAuth.ReceiveData))]
internal static class RequestAuthPatch {

    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator) {
        if (!Plugin.Instance.Config.EnableRemoteAdminExtensions)
            return instructions;

        var list = new List<CodeInstruction>(instructions);
        var label = generator.DefineLabel();
        list[0].labels.Add(label);
        var response = generator.Local<string>();
        list.InsertRange(0, new[] {
            Ldarg(2),
            Call<string>(nameof(string.Trim), Array.Empty<Type>()),
            RequestDataButton.RequestAuth.Load(),
            Ldarg(1),
            IsInstance<PlayerCommandSender>(),
            response.LoadAddress(),
            Call(typeof(RemoteAdminOptionManager), nameof(RemoteAdminOptionManager.HandleCustomRequest)),
            label.False(),
            Ldarg(1),
            String("PlayerInfo#<color=white>{0}</color>"),
            response.Load(),
            Call<string>(nameof(string.Format), new[] {typeof(string), typeof(object)}),
            Int1,
            Int1,
            String("null"),
            Call<CommandSender>(nameof(CommandSender.RaReply)),
            Return
        });

        return list;
    }

}
