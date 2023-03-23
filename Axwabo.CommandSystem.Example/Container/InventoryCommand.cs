using Axwabo.CommandSystem.Attributes;
using Axwabo.CommandSystem.Attributes.Containers;
using Axwabo.CommandSystem.Commands;

namespace Axwabo.CommandSystem.Example.Container;

[CommandProperties(CommandHandlerType.RemoteAdmin, "inventory")]
// this container registers two subcommands, see and drop
// the equip command is a subcommand of this, specified in its class
[UsesSubcommands(typeof(SeeInventory), typeof(DropItem))]
public sealed class InventoryCommand : ContainerCommand
{

}
