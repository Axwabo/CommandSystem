using Axwabo.CommandSystem.Attributes.RaExt;
using RemoteAdmin;

namespace Axwabo.CommandSystem.RemoteAdminExtensions;

[RemoteAdminOptionProperties("stack", "Stack", "green")]
[StandaloneSelector]
internal sealed class StackOption : RemoteAdminOptionBase
{

    protected override string HandleButtonClick(RequestDataButton button, PlayerCommandSender sender) => null;

}
