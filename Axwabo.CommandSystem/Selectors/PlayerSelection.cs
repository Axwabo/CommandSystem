using System.Collections.Generic;
using UnityEngine;

namespace Axwabo.CommandSystem.Selectors {

    public sealed class PlayerSelection : MonoBehaviour {

        public readonly Stack<HubCollection> Stack = new();

        public void Push(ReferenceHub hub) => Stack.Push(new HubCollection {hub});
        
        public void Push(IEnumerable<ReferenceHub> hubs) => Stack.Push(new HubCollection(hubs));
        
        public void Push(HubCollection hubs) => Stack.Push(hubs);
        
        public HubCollection Pop() => Stack.Pop();
        
        public HubCollection Peek() => Stack.Peek();
        
        public void Clear() => Stack.Clear();
        
        public bool IsEmpty => Stack.Count == 0;

    }

}
