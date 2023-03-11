using System;
using System.Linq;
using System.Reflection;
using Axwabo.CommandSystem.Attributes.Interfaces;
using Axwabo.CommandSystem.Exceptions;
using Axwabo.CommandSystem.RemoteAdminExtensions;

namespace Axwabo.CommandSystem.PropertyManager;

public static class RemoteAdminExtensionPropertyManager {

    public const string InvalidCharacters = "$@()\"<>.";

    private static readonly char[] InvalidCharactersArray = InvalidCharacters.ToCharArray();

    private static bool NonDigit(char arg) => arg != '-' && !char.IsDigit(arg);

    public static bool IsValidOptionName(string s) {
        if (s == null)
            return false;
        s = s.Trim();
        return s.Length > 0 && s.IndexOfAny(InvalidCharactersArray) == -1 && s.Any(NonDigit);
    }

    public static bool TryResolveProperties(RemoteAdminOptionBase option, out string id, out string staticText) {
        id = null;
        staticText = null;
        foreach (var attribute in option.GetType().GetCustomAttributes()) {
            if (ResolveBaseAttribute(option, attribute, ref id, ref staticText))
                continue;
            // TODO: custom resolvers
        }

        return IsValidOptionName(id);
    }

    private static bool ResolveBaseAttribute(RemoteAdminOptionBase option, Attribute attribute, ref string id, ref string staticText) {
        var completed = false;
        if (attribute is IRemoteAdminOptionIdentifier identifier) {
            id = identifier.Name ?? throw new InvalidNameException($"Null option identifier provided by attribute {attribute.GetType().FullName} on type {option.GetType().FullName}.");
            completed = true;
        }

        if (attribute is IStaticOptionText text) {
            text.Text.SetFieldIfNotNull(ref staticText);
            completed = true;
        }

        return completed;
    }

}
