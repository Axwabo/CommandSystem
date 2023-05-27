using Axwabo.CommandSystem.Example.Resolvers;
using Axwabo.CommandSystem.Example.Translations;
using Axwabo.CommandSystem.Registration;
using Axwabo.Helpers.Config.Translations;
using PluginAPI.Core.Attributes;

namespace Axwabo.CommandSystem.Example;

[EnumCommandPropertyResolver]
public sealed class ExamplePlugin
{

    [PluginConfig]
    public static ExampleConfig Config = new(); // expose the config to the property resolver

    [PluginEntryPoint("CommandSystemExample", "1.0.0", "Example plugin for the command system.", "Axwabo")]
    private void OnEnabled()
    {
        // register all commands in the assembly; the EnumCommandPropertyResolver will be used to resolve custom command properties
        CommandRegistrationProcessor.RegisterAll(this);
        // register all translations in the config
        TranslationHelper.RegisterAllTranslations(Config.Translations);
    }

    [PluginUnload]
    private void OnDisabled()
    {
        CommandRegistrationProcessor.UnregisterAll(this);
        TranslationHelper.UnregisterAllTranslations<GreetingType>();
    }

}
