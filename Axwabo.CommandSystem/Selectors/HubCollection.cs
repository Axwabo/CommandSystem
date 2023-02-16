using System.Collections.Generic;
using RemoteAdmin;

namespace Axwabo.CommandSystem.Selectors;

public sealed class HubCollection : List<ReferenceHub> {

    public static HubCollection Empty => new();

    public HubCollection() {
    }

    public HubCollection(int capacity) : base(capacity) {
    }

    public HubCollection(IEnumerable<ReferenceHub> collection) : base(collection) {
    }

    public HubCollection(ReferenceHub hub) : base(1) => Add(hub);

    public static HubCollection From(ReferenceHub hub, List<ReferenceHub> candidates = null)
        => hub == null
            ? Empty
            : candidates == null || candidates.Contains(hub)
                ? new HubCollection(1) {hub}
                : Empty;

    public static HubCollection From(CommandSender sender, List<ReferenceHub> candidates = null)
        => From((sender as PlayerCommandSender)?.ReferenceHub, candidates);

}
