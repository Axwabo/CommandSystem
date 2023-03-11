using System;
using Axwabo.CommandSystem.Attributes.Interfaces;

namespace Axwabo.CommandSystem.Attributes.RaExt;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class AutoOptionIdAttribute : Attribute, IRemoteAdminOptionIdentifier {

    public const string Identifier = "@$";

    public string Name => Identifier;

}
