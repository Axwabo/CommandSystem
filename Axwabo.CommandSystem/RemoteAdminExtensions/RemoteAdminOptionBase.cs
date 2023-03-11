using Axwabo.CommandSystem.Attributes.RaExt;
using Axwabo.CommandSystem.Exceptions;
using Axwabo.CommandSystem.PropertyManager;
using RemoteAdmin;

namespace Axwabo.CommandSystem.RemoteAdminExtensions;

public abstract class RemoteAdminOptionBase {

    private static uint _autoDecrement = 10;

    protected virtual string InitOnlyOptionId { get; }

    protected virtual bool CanBeUsedAsStandaloneSelector => false;

    private readonly string _staticText;

    private InvalidNameException InvalidId => new($"Option identifier on type {GetType().FullName} is empty, numeric only or contains one of the following invalid characters: {RemoteAdminExtensionPropertyManager.InvalidCharacters}");

    public RemoteAdminOptionBase() {
        var id = InitOnlyOptionId;
        if (!string.IsNullOrWhiteSpace(id) && !RemoteAdminExtensionPropertyManager.IsValidOptionName(id))
            throw InvalidId;
        var resolved = RemoteAdminExtensionPropertyManager.TryResolveProperties(this, out id, out _staticText);
        if (id == AutoOptionIdAttribute.Identifier)
            OptionIdentifier = "-" + ++_autoDecrement;
        else if (!resolved)
            throw InvalidId;
        else
            OptionIdentifier = (CanBeUsedAsStandaloneSelector ? "@" : "$") + id;
    }

    public string OptionIdentifier { get; }

    public string GetText(CommandSender sender) => GenerateDisplayText(sender);

    protected virtual string GenerateDisplayText(CommandSender sender) => _staticText;

    public abstract string OnClick(RequestDataButton button, PlayerCommandSender sender);

}
