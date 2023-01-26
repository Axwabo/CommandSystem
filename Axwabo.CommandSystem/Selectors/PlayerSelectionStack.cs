using System.Collections.Generic;
using System.Linq;
using Axwabo.Helpers;
using Axwabo.Helpers.Pools;
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

    public bool IsEmpty => Stack.Count == 0;

    public int Count => Stack.Count;

    public override string ToString() {
        var sb = StringBuilderPool.Shared.Rent();
        lock (Stack) {
            var count = Stack.Count;
            for (var i = count - 1; i >= 0; i--) {
                var hubs = Stack[i];
                if (hubs.Count == 0) {
                    sb.AppendLine($"#{count - i - 1} [EMPTY]");
                    continue;
                }

                sb.AppendLine($"#{count - i - 1} ({"player".Pluralize(hubs.Count)}):");
                sb.AppendLine("> " + Stack[i].CombineNicknames("\n> "));
            }
        }

        return StringBuilderPool.Shared.ToStringReturn(sb).TrimEnd();
    }

    public static PlayerSelectionStack Get(Component component) => component.GetOrAddComponent<PlayerSelectionStack>();

    public static PlayerSelectionStack Get(CommandSender sender) => sender switch {
        PlayerCommandSender player => Get(player.ReferenceHub),
        ServerConsoleSender => Get(GameCore.Console.singleton),
        _ => null
    };

}
