namespace Axwabo.CommandSystem.Extensions;

/// <summary>Extensions for setting field references based on nullity.</summary>
public static class RefExtensions
{

    /// <summary>
    /// Sets the field reference if this value is not null.
    /// </summary>
    /// <param name="value">The value to use.</param>
    /// <param name="field">The field reference to set.</param>
    /// <typeparam name="T">The type of the field.</typeparam>
    public static void SetFieldIfNotNull<T>(this T value, ref T field) where T : class
    {
        if (value != null)
            field = value;
    }

    /// <summary>
    /// Safely casts this value and sets the referenced field if the field's value is null.
    /// </summary>
    /// <param name="value">The value to cast.</param>
    /// <param name="field">The field reference to set.</param>
    /// <typeparam name="T">The type to cast to.</typeparam>
    /// <returns>Whether the value was set.</returns>
    public static bool SafeCastAndSetIfNull<T>(this object value, ref T field) where T : class
    {
        if (field != null || value is not T t)
            return false;
        field = t;
        return true;
    }

}
