using Utils;

namespace Axwabo.CommandSystem.Extensions;

/// <summary>Extension methods for string <see cref="ArraySegment{T}"/>s.</summary>
public static class StringArraySegmentExtensions
{

    /// <summary>
    /// Concatenates the members of a string array segment starting from the given index using the specified separator between each member.
    /// </summary>
    /// <param name="arguments">The string to use as a separator.<paramref name="separator" /> is included in the returned string only if <paramref name="arguments" /> has more than one element.</param>
    /// <param name="start">The index to start joining from.</param>
    /// <param name="separator">The string to use as a separator.<paramref name="separator" /> is included in the returned string only if the new segment has more than one element.</param>
    /// <returns>The joined string.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if the index indicates an array with a negative count:<br/>
    /// - the segment was already empty and the index is greater than zero<br/>
    /// - the index is greater than the segment's length.
    /// </exception>
    public static string Join(this ArraySegment<string> arguments, int start = 0, string separator = " ")
        => string.Join(separator, arguments.Segment(start));

    /// <summary>
    /// Parses the list of arguments into a list of <see cref="ReferenceHub"/> instances.
    /// </summary>
    /// <param name="arguments">The arguments to parse.</param>
    /// <param name="newArgs">The new arguments after the player IDs and names have been processed.</param>
    /// <param name="startIndex">The index to start processing from.</param>
    /// <returns>A list of <see cref="ReferenceHub"/> instances.</returns>
    public static List<ReferenceHub> GetTargets(this ArraySegment<string> arguments, out string[] newArgs, int startIndex = 0)
        => RAUtils.ProcessPlayerIdOrNamesList(arguments, startIndex, out newArgs);

}
