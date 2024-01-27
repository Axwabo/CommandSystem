namespace Axwabo.CommandSystem.Selectors;

/// <summary>
/// A list of <see cref="ReferenceHub"/> instances.
/// </summary>
public sealed class HubCollection : List<ReferenceHub>
{

    /// <summary>Gets a new empty <see cref="HubCollection"/>.</summary>
    public static HubCollection Empty => new();

    /// <summary>Creates a new <see cref="HubCollection"/> instance.</summary>
    public HubCollection()
    {
    }

    /// <summary>
    /// Creates a new <see cref="HubCollection"/> instance with the specified capacity.
    /// </summary>
    /// <param name="capacity">The initial capacity of the collection.</param>
    public HubCollection(int capacity) : base(capacity)
    {
    }

    /// <summary>
    /// Creates a new <see cref="HubCollection"/> instance with the specified elements.
    /// </summary>
    /// <param name="collection">The elements to add to the collection.</param>
    public HubCollection(IEnumerable<ReferenceHub> collection) : base(collection)
    {
    }

    /// <summary>
    /// Creates a new <see cref="HubCollection"/> instance with the specified element.
    /// </summary>
    /// <param name="hub">The element to add to the collection.</param>
    public HubCollection(ReferenceHub hub) : base(1) => Add(hub);

    /// <summary>
    /// Creates a new <see cref="HubCollection"/> instance with the specified element if it is present in the specified list of candidates or if the list is null.
    /// </summary>
    /// <param name="hub">The element to add to the collection.</param>
    /// <param name="candidates">The list of candidates.</param>
    /// <returns>A new <see cref="HubCollection"/> instance.</returns>
    public static HubCollection From(ReferenceHub hub, List<ReferenceHub> candidates = null)
        => hub == null
            ? Empty
            : candidates == null || candidates.Contains(hub)
                ? new HubCollection(1) {hub}
                : Empty;

    /// <summary>
    /// Creates a new <see cref="HubCollection"/> instance with the specified <see cref="CommandSender"/> if it is present in the specified list of candidates or if the list is null.
    /// </summary>
    /// <param name="sender">The <see cref="PlayerCommandSender"/> to add to the collection.</param>
    /// <param name="candidates">The list of candidates.</param>
    /// <returns>A new <see cref="HubCollection"/> instance.</returns>
    public static HubCollection From(CommandSender sender, List<ReferenceHub> candidates = null)
        => From((sender as PlayerCommandSender)?.ReferenceHub, candidates);

}
