#if EXILED
extern alias E;
using E::Axwabo.Helpers;
#else
using Axwabo.Helpers;
#endif
using PlayerRoles;
using PlayerStatsSystem;

namespace Axwabo.CommandSystem.Selectors.Filtering;

/// <summary>
/// Contains a number of predefined filters for <see cref="ReferenceHub"/> instances.
/// </summary>
public static class PresetHubFilters
{

    /// <summary>
    /// Inverts the given filter.
    /// </summary>
    /// <param name="filter">The filter to invert.</param>
    /// <returns>The inverted filter.</returns>
    public static HubFilter Invert(this HubFilter filter) => hub => !filter(hub);

    /// <summary>
    /// Creates a simple filter from a <see cref="ParameterizedHubFilter{T}"/>.
    /// </summary>
    /// <param name="filter">The filter to convert.</param>
    /// <param name="parameter">The parameter to pass to the filter.</param>
    /// <typeparam name="T">The type of the parameter.</typeparam>
    /// <returns>The converted filter.</returns>
    public static HubFilter FromParameterized<T>(ParameterizedHubFilter<T> filter, T parameter) => hub => filter(hub, parameter);

    /// <summary>
    /// Gets the value for the given stat on a <see cref="ReferenceHub"/>.
    /// </summary>
    /// <param name="hub">The hub to get the stat value from.</param>
    /// <typeparam name="TStat">The type of the stat.</typeparam>
    /// <returns>The current value.</returns>
    public static float StatValue<TStat>(this ReferenceHub hub) where TStat : StatBase => hub.playerStats.GetModule<TStat>().CurValue;

    #region Instantiators

    /// <summary>
    /// Gets a <see cref="HubFilter"/> for the given role type.
    /// </summary>
    /// <param name="role">The role type to check for.</param>
    /// <returns>The filter.</returns>
    public static HubFilter Role(string role)
        => FromParameterized(Role, ValueRange<RoleTypeId>.Parse(role.EnsureNotEmpty("Role type must not be empty"), Parse.Role));

    /// <summary>
    /// Gets a <see cref="HubFilter"/> for the given team.
    /// </summary>
    /// <param name="team">The team to check for.</param>
    /// <returns>The filter.</returns>
    public static HubFilter Team(string team)
        => FromParameterized(Team, ValueRange<Team>.Parse(team.EnsureNotEmpty("Team must not be empty"), Parse.EnumIgnoreCase));

    /// <summary>
    /// Gets a <see cref="HubFilter"/> for the given player id.
    /// </summary>
    /// <param name="id">The player id to check for.</param>
    /// <returns>The filter.</returns>
    /// <remarks>PlayerID differs from UserID. PlayerID is the number you can see in the Remote Admin GUI next to the player's nickname.</remarks>
    public static HubFilter Id(string id)
        => FromParameterized(Id, ValueRange<int>.Parse(id.EnsureNotEmpty("Player id must not be empty"), Parse.Int));

    /// <summary>
    /// Gets a <see cref="HubFilter"/> for the given nickname.
    /// </summary>
    /// <param name="nickname">The nickname to check for.</param>
    /// <returns>The filter.</returns>
    public static HubFilter Nickname(string nickname)
        => FromParameterized(Nickname, nickname.EnsureNotEmpty("Nickname must not be empty"));

    /// <summary>
    /// Checks whether the given player is has access to the Remote Admin panel.
    /// </summary>
    /// <param name="hub">The player to check.</param>
    /// <returns>Whether the player has access to the Remote Admin panel.</returns>
    public static bool RemoteAdmin(ReferenceHub hub) => hub.serverRoles.RemoteAdmin;

    /// <summary>
    /// Checks whether the player is on the current sender's player selection stack.
    /// </summary>
    /// <remarks>The stack of the <see cref="PlayerSelectionManager.CurrentSender"/> is used at the time of accessing this property.</remarks>
    public static HubFilter Stack
    {
        get
        {
            var stack = PlayerSelectionStack.Get(PlayerSelectionManager.CurrentSender);
            return stack == null ? null : hub => stack.Contains(hub);
        }
    }

    /// <summary>
    /// Gets a <see cref="HubFilter"/> for the given held item type.
    /// </summary>
    /// <param name="item">The item type to check for.</param>
    /// <returns>The filter.</returns>
    public static HubFilter CurrentItem(string item)
        => FromParameterized(CurrentItem, ValueRange<ItemType>.Parse(item.EnsureNotEmpty("Item type must not be empty"), Parse.Item));

    /// <summary>
    /// Checks whether the given player has god mode enabled.
    /// </summary>
    /// <param name="hub">The player to check.</param>
    /// <returns>Whether the player has god mode enabled.</returns>
    public static bool GodMode(ReferenceHub hub) => hub.characterClassManager.GodMode;

    /// <summary>
    /// Gets a <see cref="HubFilter"/> for the given player's health.
    /// </summary>
    /// <param name="value">The health value to check for.</param>
    /// <returns>The filter.</returns>
    public static HubFilter Health(string value)
        => FromParameterized(Health, ValueRange<float>.Parse(value.EnsureNotEmpty("Health must not be empty"), Parse.Float));

    /// <summary>
    /// Gets a <see cref="HubFilter"/> for the given player's artificial health.
    /// </summary>
    /// <param name="value">The artificial health value to check for.</param>
    /// <returns>The filter.</returns>
    public static HubFilter ArtificialHealth(string value)
        => FromParameterized(ArtificialHealth, ValueRange<float>.Parse(value.EnsureNotEmpty("Artificial health must not be empty"), Parse.Float));

    /// <summary>
    /// Gets a <see cref="HubFilter"/> for the given player's hume shield.
    /// </summary>
    /// <param name="value">The hume shield value to check for.</param>
    /// <returns>The filter.</returns>
    public static HubFilter HumeShield(string value)
        => FromParameterized(HumeShield, ValueRange<float>.Parse(value.EnsureNotEmpty("Hume shield must not be empty"), Parse.Float));

    #endregion

    #region Parameterized

    /// <summary>
    /// Checks whether the player's role type is within the given range.
    /// </summary>
    /// <param name="hub">The player to check.</param>
    /// <param name="range">The range to check for.</param>
    /// <returns>Whether the player's role is within the range.</returns>
    public static bool Role(ReferenceHub hub, ValueRange<RoleTypeId> range) => range.IsWithinRange(hub.roleManager.CurrentRole.RoleTypeId);

    /// <summary>
    /// Checks whether the player's team is within the given range.
    /// </summary>
    /// <param name="hub">The player to check.</param>
    /// <param name="range">The range to check for.</param>
    /// <returns>Whether the player's team is within the range.</returns>
    public static bool Team(ReferenceHub hub, ValueRange<Team> range) => range.IsWithinRange(hub.roleManager.CurrentRole.Team);

    /// <summary>
    /// Checks whether the player's nickname contains the given string, ignoring case.
    /// </summary>
    /// <param name="hub">The player to check.</param>
    /// <param name="parameter">The string to check for.</param>
    /// <returns>Whether the player's nickname contains the string.</returns>
    public static bool Nickname(ReferenceHub hub, string parameter) => hub.nicknameSync.Network_myNickSync.ContainsIgnoreCase(parameter);

    /// <summary>
    /// Checks whether the player's id is within the given range.
    /// </summary>
    /// <param name="hub">The player to check.</param>
    /// <param name="parameter">The range to check for.</param>
    /// <returns>Whether the player's id is within the range.</returns>
    /// <remarks>PlayerID differs from UserID. PlayerID is the number you can see in the Remote Admin GUI next to the player's nickname.</remarks>
    public static bool Id(ReferenceHub hub, ValueRange<int> parameter) => parameter.IsWithinRange(hub.PlayerId);

    /// <summary>
    /// Checks whether the player's current item is within the given range.
    /// </summary>
    /// <param name="hub">The player to check.</param>
    /// <param name="parameter">The range to check for.</param>
    /// <returns>Whether the player's current item is within the range.</returns>
    public static bool CurrentItem(ReferenceHub hub, ValueRange<ItemType> parameter) => parameter.IsWithinRange(hub.inventory.CurItem.TypeId);

    /// <summary>
    /// Checks whether the player's health is within the given range.
    /// </summary>
    /// <param name="hub">The player to check.</param>
    /// <param name="parameter">The range to check for.</param>
    /// <returns>Whether the player's health is within the range.</returns>
    public static bool Health(ReferenceHub hub, ValueRange<float> parameter) => parameter.IsWithinRange(hub.StatValue<HealthStat>());

    /// <summary>
    /// Checks whether the player's artificial health is within the given range.
    /// </summary>
    /// <param name="hub">The player to check.</param>
    /// <param name="parameter">The range to check for.</param>
    /// <returns>Whether the player's artificial health is within the range.</returns>
    public static bool ArtificialHealth(ReferenceHub hub, ValueRange<float> parameter) => parameter.IsWithinRange(hub.StatValue<AhpStat>());

    /// <summary>
    /// Checks whether the player's hume shield is within the given range.
    /// </summary>
    /// <param name="hub">The player to check.</param>
    /// <param name="parameter">The range to check for.</param>
    /// <returns>Whether the player's hume shield is within the range.</returns>
    public static bool HumeShield(ReferenceHub hub, ValueRange<float> parameter) => parameter.IsWithinRange(hub.StatValue<HumeShieldStat>());

    #endregion

}
