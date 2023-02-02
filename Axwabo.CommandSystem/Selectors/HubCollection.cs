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

    public static HubCollection From(ReferenceHub hub) => hub == null ? Empty : new HubCollection(hub);

    public static HubCollection From(CommandSender sender) => From((sender as PlayerCommandSender)?.ReferenceHub);

}
