using System.Collections.Generic;
using Axwabo.Helpers;
using RemoteAdmin;
using UnityEngine;

namespace Axwabo.CommandSystem.Selectors {

    public sealed class PlayerSelectionStack : MonoBehaviour {

        public readonly Stack<HubCollection> Stack = new();

        public void Push(ReferenceHub hub) => Stack.Push(new HubCollection {hub});

        public void Push(IEnumerable<ReferenceHub> hubs) => Stack.Push(hubs as HubCollection ?? new HubCollection(hubs));

        public HubCollection Pop() => Stack.Pop();

        public HubCollection Peek() => Stack.Peek();

        public void Clear() => Stack.Clear();

        public bool IsEmpty => Stack.Count == 0;

        public static PlayerSelectionStack Get(Component component) => component.GetOrAddComponent<PlayerSelectionStack>();

        public static PlayerSelectionStack Get(CommandSender sender) => sender switch {
            PlayerCommandSender player => Get(player.ReferenceHub),
            ServerConsoleSender => Get(GameCore.Console.singleton),
            _ => null
        };

    }

}
