namespace Axwabo.CommandSystem.Permissions;

/// <summary>
/// A permission attribute that uses a string as the permission. This attribute is inherited.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public sealed class StringPermissionsAttribute : Attribute
{

    /// <summary>The required permission string.</summary>
    public readonly string Permission;

    /// <summary>
    /// Initializes a new instance of the <see cref="StringPermissionsAttribute"/> class.
    /// </summary>
    /// <param name="permission">The required permission.</param>
    public StringPermissionsAttribute(string permission) => Permission = permission;

}
