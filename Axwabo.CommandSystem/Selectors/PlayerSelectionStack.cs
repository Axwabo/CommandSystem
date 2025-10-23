using System.Collections;
using Axwabo.CommandSystem.Commands;
using Axwabo.CommandSystem.Extensions;
using Console = GameCore.Console;

namespace Axwabo.CommandSystem.Selectors;

/// <summary>
/// A stack of <see cref="HubCollection" /> type lists.
/// </summary>
public sealed class PlayerSelectionStack : MonoBehaviour, IEnumerable<HubCollection>
{

    /// <summary>The maximum amount of lists that can be stored on the selection stack.</summary>
    public const int MaxSize = 1024;

    /// <summary>
    /// Checks if the amount of lists to add would overflow the maximum size of the stack.
    /// </summary>
    /// <param name="addCount">The amount of lists to add.</param>
    /// <returns>Whether the added lists overflow the max size.</returns>
    public bool CheckOverflow(int addCount) => Count + addCount > MaxSize;

    private readonly List<HubCollection> _stack = [];

    /// <summary>The last index of the underlying list.</summary>
    public int LastIndex => _stack.Count - 1;

    /// <summary>
    /// Pushes a <see cref="ReferenceHub"/> onto the stack as a single element of a <see cref="HubCollection"/>.
    /// </summary>
    /// <param name="hub">The hub to push.</param>
    public void Push(ReferenceHub hub) => Push(new HubCollection {hub});

    /// <summary>
    /// Pushes an enumerable of <see cref="ReferenceHub"/> onto the stack as one <see cref="HubCollection"/>.
    /// </summary>
    /// <param name="hubs">The hubs to push.</param>
    public void Push(IEnumerable<ReferenceHub> hubs)
    {
        var collection = hubs as HubCollection ?? new HubCollection(hubs);
        if (CheckOverflow(collection.Count))
            throw new InvalidOperationException($"The stack has reached its maximum size of {MaxSize} lists");
        _stack.Add(collection);
    }

    /// <summary>
    /// Pops the topmost <see cref="HubCollection"/> from the stack.
    /// </summary>
    /// <returns>The popped <see cref="HubCollection"/>.</returns>
    public HubCollection Pop() => PopAt(0);

    /// <summary>
    /// Pops the <see cref="HubCollection"/> at the specified index from the stack.
    /// </summary>
    /// <param name="index">The index of the list to pop.</param>
    /// <returns>The popped <see cref="HubCollection"/>.</returns>
    public HubCollection PopAt(int index)
    {
        var i = LastIndex - index;
        var removed = _stack[i];
        _stack.RemoveAt(i);
        return removed;
    }

    /// <summary>
    /// Gets the list and clears the stack.
    /// </summary>
    /// <returns>All selections in the stack.</returns>
    public List<ReferenceHub> PopAll()
    {
        var all = new HashSet<ReferenceHub>();
        var count = Count;
        for (var i = count - 1; i >= 0; i--)
            all.AddRange(_stack[i]);
        _stack.Clear();
        return all.ToList();
    }

    /// <summary>
    /// Peeks the topmost <see cref="HubCollection"/> on the stack, creating a new list.
    /// </summary>
    /// <returns>The topmost selection.</returns>
    public HubCollection Peek() => new(this[0]);

    /// <summary>
    /// Peeks the <see cref="HubCollection"/> at the specified index on the stack, creating a new list.
    /// </summary>
    /// <param name="index">The index of the list to peek.</param>
    /// <returns>The peeked selection.</returns>
    public HubCollection PeekAt(int index) => new(this[index]);

    /// <summary>
    /// Gets the exact list reference at the specified index.
    /// </summary>
    /// <param name="result">The index of the list to get.</param>
    /// <returns>The list at the specified index.</returns>
    /// <remarks>The order is reversed meaning that index 0 is the topmost value but the last in the list.</remarks>
    public HubCollection this[int result] => _stack[LastIndex - result];

    /// <summary>Clears the stack.</summary>
    public void Clear() => _stack.Clear();

    /// <summary>Reverses the order of the stack.</summary>
    public void Reverse() => _stack.Reverse();

    /// <summary>Duplicates all values in the stack.</summary>
    public void DuplicateAll()
    {
        var dup = _stack.ToArray();
        _stack.AddRange(dup);
    }

    /// <summary>
    /// Determines whether the given <see cref="ReferenceHub"/> is present in any of the lists.
    /// </summary>
    /// <param name="hub">The hub to search for.</param>
    /// <returns>Whether the hub is present.</returns>
    public bool Contains(ReferenceHub hub)
    {
        foreach (var hubs in _stack)
            if (hubs.Contains(hub))
                return true;
        return false;
    }

    /// <summary>Determines whether the stack is empty.</summary>
    public bool IsEmpty => _stack.Count == 0;

    /// <summary>Gets the amount of lists in the stack.</summary>
    public int Count => _stack.Count;

    /// <inheritdoc />
    public IEnumerator<HubCollection> GetEnumerator() => _stack.GetEnumerator();

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <summary>
    /// Stringifies the stack, combining the nicknames of the players.
    /// </summary>
    /// <returns>The stack as a string.</returns>
    /// <remarks>The topmost value (at index 0) will be at the end of the string.</remarks>
    /// <seealso cref="ToString(bool)"/>
    public override string ToString() => ToString(true);

    /// <summary>
    /// Stringifies the stack, combining the nicknames of the players.
    /// </summary>
    /// <param name="inverted">Whether to invert the order of the stack.</param>
    /// <returns>The stack as a string.</returns>
    /// <remarks>If the order is reversed, it will start from the bottom.</remarks>
    public string ToString(bool inverted)
    {
        var sb = StringBuilderPool.Shared.Rent();
        lock (_stack)
        {
            var count = _stack.Count;

            void Append(int index)
            {
                var hubs = _stack[index];
                var visualIndex = count - index - 1;
                if (hubs.Count == 0)
                {
                    sb.AppendLine($"#{visualIndex} [EMPTY]");
                    return;
                }

                sb.AppendLine($"#{visualIndex} ({"player".PluralizeWithCount(hubs.Count)}):");
                sb.AppendLine("> " + _stack[index].CombineNicknames("\n> "));
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

    /// <summary>
    /// Gets the <see cref="PlayerSelectionStack"/> from the Game Object of the given <see cref="Component"/>.
    /// </summary>
    /// <param name="component">The component to get the stack from.</param>
    /// <returns>The <see cref="PlayerSelectionStack"/> on the component's Game Object.</returns>
    public static PlayerSelectionStack Get(Component component) => component.GetOrAddComponent<PlayerSelectionStack>();

    /// <summary>
    /// Gets the <see cref="PlayerSelectionStack"/> from the given <see cref="CommandSender"/>.
    /// </summary>
    /// <param name="sender">The sender to get the stack from.</param>
    /// <returns>The <see cref="PlayerSelectionStack"/> on the sender's Game Object. May be null.</returns>
    public static PlayerSelectionStack Get(CommandSender sender) => sender switch
    {
        PlayerCommandSender player => Get(player.ReferenceHub),
        ServerConsoleSender => Get(Console.Singleton),
        _ => null
    };

    /// <summary>
    /// Ensures that the stack command can be executed by the given sender.
    /// </summary>
    /// <param name="sender">The sender to check.</param>
    /// <param name="selection">The selection stack.</param>
    /// <param name="result">The result of the command upon failure.</param>
    /// <param name="canBeEmpty">Whether the stack can be empty.</param>
    /// <returns>Whether the command can be executed.</returns>
    public static bool PreprocessCommand(CommandSender sender, out PlayerSelectionStack selection, out CommandResult result, bool canBeEmpty = false)
    {
        selection = Get(sender);
        if (selection == null)
        {
            result = $"!Cannot get a selection stack object from {sender.GetType().FullName}.";
            return false;
        }

        if (selection.IsEmpty && !canBeEmpty)
        {
            result = "!The selection stack is empty.";
            return false;
        }

        result = true;
        return true;
    }

}
