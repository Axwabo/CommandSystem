#if EXILED
using System;
using Axwabo.CommandSystem.Registration;
using Exiled.API.Enums;
using Exiled.API.Features;
using HarmonyLib;

namespace Axwabo.CommandSystem;

/// <summary>
/// The main plugin class for EXILED.
/// </summary>
public sealed class Plugin : Plugin<Config> {

    /// <summary>The current plugin instance.</summary>
    public static Plugin Instance { get; private set; }

    private Harmony _harmony;

    /// <summary>Called when the plugin is enabled.</summary>
    public override void OnEnabled() {
        Instance = this;
        _harmony = new Harmony("Axwabo.CommandSystem");
        try {
            _harmony.PatchAll();
        } catch (Exception e) {
            Log.Error("Patching failed! Some features will not work properly.\n" + e);
        }

        CommandRegistrationProcessor.RegisterAll(this);
        Log.Info("Axwabo.CommandSystem has been enabled!");
    }

    /// <summary>Called when the plugin is disabled.</summary>
    public override void OnDisabled() {
        Instance = this;
        CommandRegistrationProcessor.UnregisterAll(this);
        _harmony.UnpatchAll();
        Log.Info("Axwabo.CommandSystem has been disabled!");
    }

    /// <summary>Gets the name of the plugin.</summary>
    public override string Name => "Axwabo.CommandSystem";
    /// <summary>Gets the prefix of the plugin used in the config.</summary>
    public override string Prefix => "CommandSystem";
    /// <summary>Gets the author of the plugin.</summary>
    public override string Author => "Axwabo";
    /// <summary>Gets the priority of the plugin.</summary>
    public override PluginPriority Priority => PluginPriority.Highest;
    /// <summary>Gets the version of the plugin.</summary>
    public override Version Version { get; } = new(1, 0, 0);

}

#endif
