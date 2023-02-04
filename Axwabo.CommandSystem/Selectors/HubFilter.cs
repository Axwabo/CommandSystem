using System;
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

    public static HubFilter Role(string role) => FromParameterized(Role, ValueRange<RoleTypeId>.Parse(role.EnsureNotEmpty("Role type must not be empty"), Enum.TryParse));

    public static HubFilter Id(string id) => FromParameterized(Id, ValueRange<int>.Parse(id.EnsureNotEmpty("Player id must not be empty"), int.TryParse));

    public static HubFilter Nickname(string nickname) => FromParameterized(Nickname, nickname.EnsureNotEmpty("Nickname must not be empty"));

    public static bool RemoteAdmin(ReferenceHub hub) => hub.serverRoles.RemoteAdmin;

    public static HubFilter Stack {
        get {
            var stack = PlayerSelectionStack.Get(PlayerSelectionManager.CurrentSender);
            return stack == null ? null : hub => stack.Contains(hub);
        }
    }

    #endregion

    #region Parameterized

    public static bool Role(ReferenceHub hub, ValueRange<RoleTypeId> range) => range.IsWithinRange(hub.roleManager.CurrentRole.RoleTypeId);

    public static bool Nickname(ReferenceHub hub, string parameter) => hub.nicknameSync.Network_myNickSync.ContainsIgnoreCase(parameter);

    public static bool Id(ReferenceHub hub, ValueRange<int> parameter) => parameter.IsWithinRange(hub.PlayerId);

    #endregion

}
