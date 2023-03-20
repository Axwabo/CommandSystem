using Axwabo.CommandSystem.Registration;
using PluginAPI.Core.Attributes;

namespace Axwabo.CommandSystem.Example;

public sealed class ExamplePlugin
{

    [PluginEntryPoint("CommandSystemExample", "1.0.0", "Example plugin for the command system.", "Axwabo")]
    private void OnEnabled()
    {
        // register all commands and Remote Admin options in this assembly
        CommandRegistrationProcessor.RegisterAll(this);
    }

    [PluginUnload]
    private void OnDisabled()
    {
        CommandRegistrationProcessor.UnregisterAll(this);
    }

}
