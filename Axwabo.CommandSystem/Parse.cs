using System;
using Axwabo.CommandSystem.Structs;
using PlayerRoles;

namespace Axwabo.CommandSystem;

/// <summary>Parser methods for common types.</summary>
public static class Parse
{

    /// <summary>
    /// Attempts to parse the given string as an integer.
    /// </summary>
    /// <param name="value">The string to parse.</param>
    /// <param name="result">The result.</param>
    /// <returns>Whether the string was parsed successfully.</returns>
    public static bool Int(string value, out int result) => int.TryParse(value.Trim(), out result);

    public static bool Int(string value, ValueRange<int> range, out int result) => Int(value, out result) && range.IsWithinRange(result);

    /// <summary>
    /// Attempts to parse the given string as a float.
    /// </summary>
    /// <param name="value">The string to parse.</param>
    /// <param name="result">The result.</param>
    /// <returns>Whether the string was parsed successfully.</returns>
    public static bool Float(string value, out float result) => float.TryParse(value.Trim(), out result);

    public static bool Float(string value, ValueRange<float> range, out float result) => Float(value, out result) && range.IsWithinRange(result);

    public static bool Byte(string value, out byte result) => byte.TryParse(value.Trim(), out result);

    public static bool Byte(string value, ValueRange<byte> range, out byte result) => Byte(value, out result) && range.IsWithinRange(result);

    /// <summary>
    /// Attempts to parse the given string as an enum value, ignoring case.
    /// </summary>
    /// <param name="value">The string to parse.</param>
    /// <param name="result">The result.</param>
    /// <typeparam name="T">The enum type.</typeparam>
    /// <returns>Whether the string was parsed successfully.</returns>
    public static bool EnumIgnoreCase<T>(string value, out T result) where T : struct => Enum.TryParse(value.Trim(), true, out result);

    public static bool Item(string value, out ItemType result) => EnumIgnoreCase(value, out result) && result != ItemType.None;

    public static bool Role(string value, out RoleTypeId result) => EnumIgnoreCase(value, out result) && result != RoleTypeId.None;

}
