#if EXILED
extern alias E;
using E::Axwabo.Helpers;
#else
using Axwabo.Helpers;
#endif
using System.Text.RegularExpressions;
using Axwabo.CommandSystem.Selectors;
using HarmonyLib;
using UnityEngine.SceneManagement;
using Utils;

#pragma warning disable CS1591

namespace Axwabo.CommandSystem.Patches;

[HarmonyPatch(typeof(RAUtils), nameof(RAUtils.ProcessPlayerIdOrNamesList))]
public static class ProcessPlayersListPatch
{

    private static readonly Regex QuotedNicknameRegex = new("@\"(.*?)\".|@[^\\s.]+\\.");

    public static List<ReferenceHub> ProcessPlayerIdOrNamesList(ArraySegment<string> args, int startIndex, out string[] newArgs, bool keepEmptyEntries)
    {
        if (PlayerSelectionManager.TryProcessPlayersCustom(args, startIndex, keepEmptyEntries, out var targets, out newArgs))
            return targets;
        try
        {
            var formatted = RAUtils.FormatArguments(args, startIndex);
            var referenceHubList = new HashSet<ReferenceHub>();
            if (formatted.StartsWith("@", StringComparison.Ordinal)) // does this even work? lmao NW
            {
                foreach (Match match in QuotedNicknameRegex.Matches(formatted))
                {
                    formatted = RAUtils.ReplaceFirst(formatted, match.Value, "");
                    var name = match.Value.Substring(1).Replace("\"", "").Replace(".", "");
                    var list = ReferenceHub.AllHubs.Where(ply => ply.nicknameSync.MyNick.Equals(name, StringComparison.OrdinalIgnoreCase)).ToList();
                    if (list.Count == 1)
                        referenceHubList.Add(list[0]);
                }

                newArgs = formatted.Split(new[] {' '}, (StringSplitOptions) (keepEmptyEntries ? 0 : 1));
                return referenceHubList.ToList();
            }

            AddSeparatedPlayers(args, startIndex, referenceHubList);
            newArgs = args.Count <= 1 ? null : RAUtils.FormatArguments(args, startIndex + 1).Split(new[] {' '}, (StringSplitOptions) (keepEmptyEntries ? 0 : 1));
            return referenceHubList.ToList();
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
            newArgs = null;
            return null;
        }
    }

    private static void AddSeparatedPlayers(ArraySegment<string> args, int startIndex, HashSet<ReferenceHub> referenceHubList)
    {
        if (args.At(startIndex).Length <= 0)
            return;
        if (char.IsDigit(args.At(startIndex)[0]))
            AddPlayersBasedOnIdList(args, startIndex, referenceHubList);
        else if (char.IsLetter(args.At(startIndex)[0]))
            AddPlayersBasedOnNicknameList(args, startIndex, referenceHubList);
    }

    private static void AddPlayersBasedOnNicknameList(ArraySegment<string> args, int startIndex, HashSet<ReferenceHub> referenceHubList)
    {
        foreach (var s in args.At(startIndex).Split(new[] {'.'}, StringSplitOptions.None))
        foreach (var hub in ReferenceHub.AllHubs)
            if (hub.nicknameSync.MyNick.Equals(s, StringComparison.OrdinalIgnoreCase))
                referenceHubList.Add(hub);
    }

    private static void AddPlayersBasedOnIdList(ArraySegment<string> args, int startIndex, HashSet<ReferenceHub> referenceHubList)
    {
        foreach (var s in args.At(startIndex).Split(new[] {'.'}, StringSplitOptions.None))
            if (int.TryParse(s, out var result) && ReferenceHub.TryGetHub(result, out var hub))
                referenceHubList.Add(hub);
    }

    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) => new[]
    {
        Ldarg(0),
        Ldarg(1),
        Ldarg(2),
        Ldarg(3),
        Call(ProcessPlayerIdOrNamesList),
        Return
    };

    private static bool _alreadyTriedToUnpatch;

    internal static void RegisterEvent() => SceneManager.sceneLoaded += OnSceneWasLoaded;

    internal static void UnregisterEvent() => SceneManager.sceneLoaded -= OnSceneWasLoaded;

    private static void OnSceneWasLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (_alreadyTriedToUnpatch)
            return;
        _alreadyTriedToUnpatch = true;
        try
        {
            var result = RemoveCedModPatch();
            if (result != null)
                Log.Info(result);
        }
        catch (Exception e)
        {
            Log.Warn("Failed to unpatch CedMod's player list processor, player selectors will not work!\n" + e);
        }
    }

    private static string RemoveCedModPatch()
    {
        var type = AccessTools.TypeByName("CedMod.CedModMain");
        if (type is null)
            return null;
        var harmony = type.StaticGet("Singleton")?.Get<Harmony>("_harmony");
        if (harmony is null)
            return "Could not find CedMod's Harmony instance.";
        var method = AccessTools.Method(AccessTools.TypeByName("CedMod.Patches.RAProcessPlayerPatch"), "Prefix");
        if (method is null)
            return "CedMod player list patch was found but without the prefix method.";
        harmony.Unpatch(AccessTools.Method(typeof(RAUtils), nameof(RAUtils.ProcessPlayerIdOrNamesList)), method);
        return "CedMod player list processor has been unpatched.";
    }

}
