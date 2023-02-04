using System;
using Axwabo.CommandSystem.Exceptions;
using PlayerRoles;

namespace Axwabo.CommandSystem.Selectors;

#region Delegates

public delegate bool HubFilter(ReferenceHub hub);

public delegate bool ParameterizedHubFilter<in T>(ReferenceHub hub, T parameter);

#endregion

public static class PresetHubFilters {

    public static HubFilter Invert(this HubFilter filter) => hub => !filter(hub);

    public static HubFilter FromParameterized<T>(ParameterizedHubFilter<T> filter, T parameter) => hub => filter(hub, parameter);

    #region Instantiators

    public static HubFilter Role(string role) => Enum.TryParse(role.EnsureNotEmpty("Role type must not be empty"), true, out RoleTypeId id)
        ? FromParameterized(Role, id)
        : throw new PlayerListProcessorException($"Invalid role type: {role}");

    public static HubFilter Nickname(string nickname) => FromParameterized(Nickname, nickname.EnsureNotEmpty("Nickname must not be empty"));

    public static bool RemoteAdmin(ReferenceHub hub) => hub.serverRoles.RemoteAdmin;

    #endregion

    #region Parameterized

    public static bool Role(ReferenceHub hub, RoleTypeId id) => hub.roleManager.CurrentRole.RoleTypeId == id;

    public static bool Nickname(ReferenceHub hub, string parameter) => hub.nicknameSync.Network_myNickSync.ContainsIgnoreCase(parameter);

    #endregion

}
