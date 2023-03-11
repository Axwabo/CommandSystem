using System;
using Axwabo.CommandSystem.Attributes.Interfaces;

namespace Axwabo.CommandSystem.Attributes.RaExt;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class RemoteAdminOptionIdentifierAttribute : Attribute, IRemoteAdminOptionIdentifier {

    public string Name { get; }

    public RemoteAdminOptionIdentifierAttribute(string name) => Name = name;

}
