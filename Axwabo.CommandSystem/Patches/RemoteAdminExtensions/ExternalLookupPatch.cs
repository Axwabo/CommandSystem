using CommandSystem.Commands.RemoteAdmin;
using HarmonyLib;

namespace Axwabo.CommandSystem.Patches.RemoteAdminExtensions;

[HarmonyPatch(typeof(ExternalLookupCommand), nameof(ExternalLookupCommand.Execute))]
internal static class ExternalLookupPatch
{

    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
    {
        if (!CommandSystemPlugin.Instance.Config.EnableRemoteAdminExtensions)
            return instructions;
        var list = new List<CodeInstruction>(instructions);
        var label = generator.DefineLabel();
        list[0].labels.Add(label);
        list.InsertRange(0, new[]
        {
            String(" "),
            Ldarg(1),
            Box<ArraySegment<string>>(),
            Call<string>(nameof(string.Join), new[] {typeof(string), typeof(IEnumerable<string>)}),
            RequestDataButton.ExternalLookup.Load(),
            Ldarg(2),
            IsInstance<PlayerCommandSender>(),
            Ldarg(3),
            Call(typeof(RemoteAdminOptionManager), nameof(RemoteAdminOptionManager.HandleCustomRequest)),
            label.False(),
            Int1,
            Return
        });
        return list;
    }

}
