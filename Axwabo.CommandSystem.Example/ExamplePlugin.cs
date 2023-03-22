using Axwabo.CommandSystem.Example.Resolvers;
using Axwabo.CommandSystem.Registration;
using PluginAPI.Core.Attributes;

namespace Axwabo.CommandSystem.Example;

public sealed class ExamplePlugin
{

    [PluginConfig]
    private ExampleConfig _config = new();

    [PluginEntryPoint("CommandSystemExample", "1.0.0", "Example plugin for the command system.", "Axwabo")]
    private void OnEnabled()
    {
        // register all commands and Remote Admin options in this assembly
        var resolver = new EnumCommandPropertyResolver(_config);
        CommandRegistrationProcessor.Create(this)
            // you could make the EnumCommandPropertyResolver an attribute and apply it to the plugin class if you can expose the plugin instance
            .WithMultiplexResolverObject(resolver)
            .Execute();
    }

    [PluginUnload]
    private void OnDisabled()
    {
        CommandRegistrationProcessor.UnregisterAll(this);
    }

}
