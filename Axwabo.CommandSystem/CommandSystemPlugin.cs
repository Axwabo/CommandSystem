using Axwabo.CommandSystem.Patches;
using Axwabo.CommandSystem.Registration;
using Axwabo.CommandSystem.RemoteAdminExtensions.Commands;
using HarmonyLib;
using LabApi.Events.Handlers;
using LabApi.Loader;
using LabApi.Loader.Features.Plugins;

#pragma warning disable CS1591

namespace Axwabo.CommandSystem;

public sealed class CommandSystemPlugin : Plugin<CommandSystemConfig>
{

    public override string Name => "Axwabo.CommandSystem";
    public override string Description => "Adds a sophisticated command system to the game.";
    public override string Author => "Axwabo";
    public override Version Version => GetType().Assembly.GetName().Version;
    public override Version RequiredApiVersion { get; } = new(1, 0, 0);

    /// <summary>Gets the plugin's config directory.</summary>
    public static string PluginDirectory { get; private set; }

    /// <summary>The current plugin instance.</summary>
    public static CommandSystemPlugin Instance { get; private set; }

    private Harmony _harmony;

    public override void Enable()
    {
        Instance = this;
        PluginDirectory = this.GetConfigDirectory().FullName;
        _harmony = new Harmony("Axwabo.CommandSystem");
        try
        {
            _harmony.PatchAll();
            ProcessPlayersListPatch.RegisterEvent();
        }
        catch (Exception e)
        {
            Log.Error("Patching failed! Some features will not work properly.\n" + e);
        }

        CommandRegistrationProcessor.RegisterAll(this);
        OptionPreferencesContainer.LoadState();
        Shutdown.OnQuit += OptionPreferencesContainer.SaveState;
        ServerEvents.CommandExecuted += DeveloperMode.OnCommandExecuted;
        PlayerEvents.RequestingRaPlayerList += RemoteAdminOptionManager.AppendAllOptions;
        Log.Info("Axwabo.CommandSystem has been enabled!");
    }

    public override void Disable()
    {
        ProcessPlayersListPatch.UnregisterEvent();
        Instance = null;
        CommandRegistrationProcessor.UnregisterAll(this);
        _harmony.UnpatchAll(_harmony.Id);
        _harmony = null;
        OptionPreferencesContainer.SaveState();
        Shutdown.OnQuit -= OptionPreferencesContainer.SaveState;
        ServerEvents.CommandExecuted -= DeveloperMode.OnCommandExecuted;
        PlayerEvents.RequestingRaPlayerList -= RemoteAdminOptionManager.AppendAllOptions;
        Log.Info("Axwabo.CommandSystem has been disabled!");
    }

}
