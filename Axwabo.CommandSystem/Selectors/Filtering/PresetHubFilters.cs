using Axwabo.CommandSystem.Structs;
using PlayerRoles;
using PlayerStatsSystem;

namespace Axwabo.CommandSystem.Selectors.Filtering;

public static class PresetHubFilters {

    public static HubFilter Invert(this HubFilter filter) => hub => !filter(hub);

    public static HubFilter FromParameterized<T>(ParameterizedHubFilter<T> filter, T parameter) => hub => filter(hub, parameter);

    public static float StatValue<TStat>(this ReferenceHub hub) where TStat : StatBase => hub.playerStats.GetModule<TStat>().CurValue;

    #region Instantiators

    public static HubFilter Role(string role)
        => FromParameterized(Role, ValueRange<RoleTypeId>.Parse(role.EnsureNotEmpty("Role type must not be empty"), Extensions.TryParseIgnoreCase));

    public static HubFilter Team(string team)
        => FromParameterized(Team, ValueRange<Team>.Parse(team.EnsureNotEmpty("Team must not be empty"), Extensions.TryParseIgnoreCase));

    public static HubFilter Id(string id)
        => FromParameterized(Id, ValueRange<int>.Parse(id.EnsureNotEmpty("Player id must not be empty"), Extensions.TryParseInt));

    public static HubFilter Nickname(string nickname)
        => FromParameterized(Nickname, nickname.EnsureNotEmpty("Nickname must not be empty"));

    public static bool RemoteAdmin(ReferenceHub hub) => hub.serverRoles.RemoteAdmin;

    public static HubFilter Stack {
        get {
            var stack = PlayerSelectionStack.Get(PlayerSelectionManager.CurrentSender);
            return stack == null ? null : hub => stack.Contains(hub);
        }
    }

    public static HubFilter CurrentItem(string item)
        => FromParameterized(CurrentItem, ValueRange<ItemType>.Parse(item.EnsureNotEmpty("Item type must not be empty"), Extensions.TryParseIgnoreCase));

    public static bool GodMode(ReferenceHub hub) => hub.characterClassManager.GodMode;

    public static bool Noclip(ReferenceHub hub) => hub.playerStats.GetModule<AdminFlagsStat>().HasFlag(AdminFlags.Noclip);

    public static HubFilter Health(string value)
        => FromParameterized(Health, ValueRange<float>.Parse(value.EnsureNotEmpty("Health must not be empty"), Extensions.TryParseFloat));

    public static HubFilter ArtificialHealth(string value)
        => FromParameterized(ArtificialHealth, ValueRange<float>.Parse(value.EnsureNotEmpty("Artificial health must not be empty"), Extensions.TryParseFloat));

    public static HubFilter HumeShield(string value)
        => FromParameterized(HumeShield, ValueRange<float>.Parse(value.EnsureNotEmpty("Hume shield must not be empty"), Extensions.TryParseFloat));

    #endregion

    #region Parameterized

    public static bool Role(ReferenceHub hub, ValueRange<RoleTypeId> range) => range.IsWithinRange(hub.roleManager.CurrentRole.RoleTypeId);

    public static bool Team(ReferenceHub hub, ValueRange<Team> range) => range.IsWithinRange(hub.roleManager.CurrentRole.Team);

    public static bool Nickname(ReferenceHub hub, string parameter) => hub.nicknameSync.Network_myNickSync.ContainsIgnoreCase(parameter);

    public static bool Id(ReferenceHub hub, ValueRange<int> parameter) => parameter.IsWithinRange(hub.PlayerId);

    public static bool CurrentItem(ReferenceHub hub, ValueRange<ItemType> parameter) => parameter.IsWithinRange(hub.inventory.CurItem.TypeId);

    public static bool Health(ReferenceHub hub, ValueRange<float> parameter) => parameter.IsWithinRange(hub.StatValue<HealthStat>());

    public static bool ArtificialHealth(ReferenceHub hub, ValueRange<float> parameter) => parameter.IsWithinRange(hub.StatValue<AhpStat>());

    public static bool HumeShield(ReferenceHub hub, ValueRange<float> parameter) => parameter.IsWithinRange(hub.StatValue<HumeShieldStat>());

    #endregion

}
