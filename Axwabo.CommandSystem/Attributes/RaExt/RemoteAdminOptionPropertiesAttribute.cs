extern alias E;
using Axwabo.CommandSystem.Attributes.Interfaces;
#if EXILED
using E::Axwabo.Helpers;
#else
using Axwabo.Helpers;
#endif

namespace Axwabo.CommandSystem.Attributes.RaExt;

/// <summary>
/// Specifies multiple properties of a RA option.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class RemoteAdminOptionPropertiesAttribute : OptionIconAttribute, IRemoteAdminOptionIdentifier, IStaticOptionText, IStandaloneSelectorOption
{

    /// <inheritdoc/>
    public string Id { get; }

    /// <inheritdoc/>
    public string Text { get; init; }

    /// <inheritdoc/>
    public bool CanBeUsedAsStandaloneSelector { get; init; }

    /// <summary>
    /// Initializes a new <see cref="RemoteAdminOptionPropertiesAttribute"/> with only the identifier.
    /// </summary>
    /// <param name="identifier">The identifier of the option. To auto-generate an ID, use <see cref="AutoGenerateIdAttribute.Identifier">$@</see>.</param>
    public RemoteAdminOptionPropertiesAttribute(string identifier) : base(null) => Id = identifier;

    /// <summary>
    /// Initializes a new <see cref="RemoteAdminOptionPropertiesAttribute"/> with the identifier, display text and optionally if it can be used aa a standalone selector.
    /// </summary>
    /// <param name="identifier">The identifier of the option. To auto-generate an ID, use <see cref="AutoGenerateIdAttribute.Identifier">$@</see>.</param>
    /// <param name="displayText">The text displayed to users.</param>
    /// <param name="canBeUsedAsStandaloneSelector">Whether the option can be used as a standalone selector.</param>
    public RemoteAdminOptionPropertiesAttribute(string identifier, string displayText, bool canBeUsedAsStandaloneSelector = false) : base(null)
    {
        Id = identifier;
        Text = displayText;
        CanBeUsedAsStandaloneSelector = canBeUsedAsStandaloneSelector;
    }

    /// <summary>
    /// Initializes a new <see cref="RemoteAdminOptionPropertiesAttribute"/> with the identifier, display text, text color and optionally the icon properties.
    /// </summary>
    /// <param name="identifier">The identifier of the option. To auto-generate an ID, use <see cref="AutoGenerateIdAttribute.Identifier">$@</see>.</param>
    /// <param name="displayText">The text displayed to users.</param>
    /// <param name="textColor">The color of the display text.</param>
    /// <param name="iconContent">The content of the icon.</param>
    /// <param name="iconColor">The color of the icon (excluding brackets).</param>
    public RemoteAdminOptionPropertiesAttribute(string identifier, string displayText, string textColor, string iconContent = null, string iconColor = null) : base(iconContent)
    {
        Id = identifier;
        Text = displayText.Color(textColor);
        ContentColor = iconColor;
    }

}
