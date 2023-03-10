using RemoteAdmin;

namespace Axwabo.CommandSystem.RemoteAdminExtensions;

public abstract class RemoteAdminOptionBase {

    protected RemoteAdminOptionBase(string optionName) => OptionName = optionName;

    public string OptionName { get; }

    public string GetText(CommandSender sender) => GenerateDisplayText(sender);

    protected abstract string GenerateDisplayText(CommandSender sender);

    public abstract string OnClick(RequestDataButton button, PlayerCommandSender sender);

}
