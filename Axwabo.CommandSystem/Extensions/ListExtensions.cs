namespace Axwabo.CommandSystem.Extensions;

/// <summary>Extension methods for adding conditionally adding objects to <see cref="List{T}"/>s.</summary>
public static class ListExtensions
{

    /// <summary>
    /// Adds the given value to the list if it is not null.
    /// </summary>
    /// <param name="list">The list to add the object to.</param>
    /// <param name="value">The value to add.</param>
    /// <typeparam name="T">The type of the list.</typeparam>
    /// <returns>Whether the value was added.</returns>
    public static bool AddIfNotNull<T>(this List<T> list, T value) where T : class
    {
        if (value == null)
            return false;
        list.Add(value);
        return true;
    }

    /// <summary>
    /// Safely casts the value and adds it to the list.
    /// </summary>
    /// <param name="list">The list to add the object to.</param>
    /// <param name="value">The value to add.</param>
    /// <typeparam name="T">The type of the list.</typeparam>
    /// <returns>Whether the value was added.</returns>
    public static bool SafeCastAndAdd<T>(this List<T> list, object value) where T : class
    {
        if (value is not T t)
            return false;
        list.Add(t);
        return true;
    }

}
