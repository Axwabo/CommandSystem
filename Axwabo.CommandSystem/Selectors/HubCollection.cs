using System.Collections.Generic;

namespace Axwabo.CommandSystem.Selectors;

public sealed class HubCollection : List<ReferenceHub> {

    public static HubCollection Empty => new();

    public HubCollection() {
    }

    public HubCollection(int capacity) : base(capacity) {
    }

    public HubCollection(IEnumerable<ReferenceHub> collection) : base(collection) {
    }

}
