using Axwabo.CommandSystem.Attributes.RaExt;
using Axwabo.CommandSystem.RemoteAdminExtensions;
using RemoteAdmin;

namespace Axwabo.CommandSystem.Selectors;

[RemoteAdminOptionProperties("stack", "Stack", "green")]
[StandaloneSelector]
internal sealed class StackOption : RemoteAdminOptionBase
{

    protected override string HandleButtonClick(RequestDataButton button, PlayerCommandSender sender) => null;

}
