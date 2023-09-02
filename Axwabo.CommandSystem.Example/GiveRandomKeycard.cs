using System;
using System.Linq;
using Axwabo.CommandSystem.Attributes;
using Axwabo.CommandSystem.Attributes.Targeting;
using Axwabo.CommandSystem.Commands;
using Axwabo.CommandSystem.Commands.MessageOverrides;
using Axwabo.CommandSystem.Permissions;
using InventorySystem;

namespace Axwabo.CommandSystem.Example;

[RemoteAdminCommand]
[AffectedOnePlayerMessage("{0} has received a random keycard")] // {0} will be replaced with the player's name using string.Format
public sealed class GiveRandomKeycard : SeparatedTargetingCommand, IAffectedMultiplePlayersMessageGenerator
{

    private static readonly ItemType[] KeycardItems =
    {
        ItemType.KeycardJanitor,
        ItemType.KeycardScientist,
        ItemType.KeycardResearchCoordinator,
        ItemType.KeycardZoneManager,
        ItemType.KeycardGuard,
        ItemType.KeycardMTFPrivate,
        ItemType.KeycardContainmentEngineer,
        ItemType.KeycardMTFOperative,
        ItemType.KeycardMTFCaptain,
        ItemType.KeycardFacilityManager,
        ItemType.KeycardChaosInsurgency,
        ItemType.KeycardO5
    };

    // you can also override basic command properties instead of using attributes
    // targets are still required to be specified with attributes
    public override string Name => "giveKeycard";

    public override string Description => "Gives a random keycard to the specified players";

    // just use attributes for this if can use a built-in permission checker
    // or implement IPermissionChecker in the command/custom attribute
    // also check out permission creators (resolvers)
    protected override IPermissionChecker Permissions { get; } = new SimpleVanillaPlayerPermissionChecker(PlayerPermissions.GivingItems);

    protected override CommandResult ExecuteOn(ReferenceHub target, ArraySegment<string> arguments, CommandSender sender)
    {
        if (target.inventory.UserInventory.Items.Values.Any(i => i.Category == ItemCategory.Keycard))
            return "!Player already has a keycard";
        if (!target.inventory.ServerAddItem(KeycardItems.RandomItem())) // implicit null-check
            return "!Failed to add item to player's inventory";
        return "Keycard added";
    }

    // all affected overrides this
    public string OnAffected(int players) => $"Given a random keycard to {players} players";

    protected override string NoPlayersAffected => "Nobody received a keycard";

}
