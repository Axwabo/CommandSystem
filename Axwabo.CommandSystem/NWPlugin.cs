#if !EXILED
using System;
using Axwabo.CommandSystem.Registration;
using HarmonyLib;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;

namespace Axwabo.CommandSystem;

/// <summary>
/// The main plugin class for the Northwood Plugin API.
/// </summary>
public sealed class Plugin {

    /// <summary>The current plugin instance.</summary>
    public static Plugin Instance { get; private set; }

    private Harmony _harmony;

    /// <summary>The plugin configuration.</summary>
    [PluginConfig]
    public Config Config = new();

    [PluginEntryPoint("Axwabo.CommandSystem", "1.0.0", "Adds a sophisticated command system to the game.", "Axwabo")]
    [PluginPriority(LoadPriority.Lowest)]
    private void OnEnable() {
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

    [PluginUnload]
    private void OnDisable() {
        Instance = null;
        CommandRegistrationProcessor.UnregisterAll(this);
        _harmony.UnpatchAll();
        Log.Info("Axwabo.CommandSystem has been disabled!");
    }

}

#endif
