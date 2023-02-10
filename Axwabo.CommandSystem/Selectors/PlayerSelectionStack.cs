using System.Collections.Generic;
using System.Linq;
using Axwabo.CommandSystem.Structs;
using Axwabo.Helpers;
using Axwabo.Helpers.Pools;
using GameCore;
using RemoteAdmin;
using UnityEngine;

namespace Axwabo.CommandSystem.Selectors;

public sealed class PlayerSelectionStack : MonoBehaviour {

    public readonly List<HubCollection> Stack = new();

    public int LastIndex => Stack.Count - 1;

    public void Push(ReferenceHub hub) => Push(new HubCollection {hub});

    public void Push(IEnumerable<ReferenceHub> hubs) => Stack.Add(hubs as HubCollection ?? new HubCollection(hubs));

    public HubCollection Pop() => PopAt(0);

    public HubCollection PopAt(int index) {
        var i = LastIndex - index;
        var removed = Stack[i];
        Stack.RemoveAt(i);
        return removed;
    }

    public List<ReferenceHub> PopAll() {
        var all = new HashSet<ReferenceHub>();
        var count = Count;
        for (var i = count - 1; i >= 0; i--)
            all.AddRange(Stack[i]);
        Stack.Clear();
        return all.ToList();
    }

    public HubCollection Peek() => new(this[0]);

    public HubCollection PeekAt(int index) => new(this[index]);

    public HubCollection this[int result] => Stack[LastIndex - result];

    public void Clear() => Stack.Clear();

    public void Reverse() => Stack.Reverse();

    public void DuplicateAll() {
        var dup = Stack.ToArray();
        Stack.AddRange(dup);
    }

    public bool Contains(ReferenceHub hub) {
        lock (Stack) {
            foreach (var hubs in Stack)
                if (hubs.Contains(hub))
                    return true;
        }

        return false;
    }

    public bool IsEmpty => Stack.Count == 0;

    public int Count => Stack.Count;

    public override string ToString() => ToString(true);

    public string ToString(bool inverted) {
        var sb = StringBuilderPool.Shared.Rent();
        lock (Stack) {
            var count = Stack.Count;

            void Append(int index) {
                var hubs = Stack[index];
                var visualIndex = count - index - 1;
                if (hubs.Count == 0) {
                    sb.AppendLine($"#{visualIndex} [EMPTY]");
                    return;
                }

                sb.AppendLine($"#{visualIndex} ({"player".PluralizeWithCount(hubs.Count)}):");
                sb.AppendLine("> " + Stack[index].CombineNicknames("\n> "));
            }

            if (inverted)
                for (var i = 0; i < count; i++)
                    Append(i);
            else
                for (var i = count - 1; i >= 0; i--)
                    Append(i);
        }

        return StringBuilderPool.Shared.ToStringReturn(sb).TrimEnd();
    }

    public static PlayerSelectionStack Get(Component component) => component.GetOrAddComponent<PlayerSelectionStack>();

    public static PlayerSelectionStack Get(CommandSender sender) => sender switch {
        PlayerCommandSender player => Get(player.ReferenceHub),
        ServerConsoleSender => Get(Console.singleton),
        _ => null
    };

    public static bool PreprocessCommand(CommandSender sender, out PlayerSelectionStack selection, out CommandResult result, bool canBeEmpty = false) {
        selection = Get(sender);
        if (selection == null) {
            result = $"!Cannot get a selection stack object from {sender.GetType().FullName}.";
            return false;
        }

        if (selection.IsEmpty && !canBeEmpty) {
            result = "!The selection stack is empty.";
            return false;
        }

        result = true;
        return true;
    }

}
