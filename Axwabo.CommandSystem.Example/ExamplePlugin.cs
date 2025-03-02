using System;
using Axwabo.CommandSystem.Example.Resolvers;
using Axwabo.CommandSystem.Example.Translations;
using Axwabo.CommandSystem.Registration;
using Axwabo.Helpers.Config.Translations;
using LabApi.Loader.Features.Plugins;

namespace Axwabo.CommandSystem.Example;

[EnumCommandPropertyResolver]
public sealed class ExamplePlugin : Plugin<ExampleConfig>
{

    internal static ExamplePlugin Instance { get; private set; }

    public override string Name => "CommandSystemExample";
    public override string Description => "Example plugin for the command system.";
    public override string Author => "Axwabo";
    public override Version Version => GetType().Assembly.GetName().Version;
    public override Version RequiredApiVersion { get; } = new(1, 0, 0);

    public override void Enable()
    {
        Instance = this;
        // register all commands in the assembly; the EnumCommandPropertyResolver will be used to resolve custom command properties
        CommandRegistrationProcessor.RegisterAll(this);
        // register all translations in the config
        if (Config != null)
            TranslationHelper.RegisterAllTranslations(Config.Translations);
    }

    public override void Disable()
    {
        Instance = null;
        CommandRegistrationProcessor.UnregisterAll(this);
        TranslationHelper.UnregisterAllTranslations<GreetingType>();
    }

}
