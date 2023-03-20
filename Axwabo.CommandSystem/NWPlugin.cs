#if !EXILED
using System;
using System.IO;
using Axwabo.CommandSystem.Patches;
using Axwabo.CommandSystem.Registration;
using HarmonyLib;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;
using PluginAPI.Helpers;

namespace Axwabo.CommandSystem;

/// <summary>
/// The main plugin class for the Northwood Plugin API.
/// </summary>
public sealed class Plugin
{

    /// <summary>Gets the plugin's config directory.</summary>
    public static string PluginDirectory => Path.Combine(Paths.Plugins, "Axwabo.CommandSystem");

    /// <summary>The current plugin instance.</summary>
    public static Plugin Instance { get; private set; }

    private Harmony _harmony;

    /// <summary>The plugin configuration.</summary>
    [PluginConfig]
    public Config Config = new();

    [PluginEntryPoint("Axwabo.CommandSystem", "1.0.0", "Adds a sophisticated command system to the game.", "Axwabo")]
    [PluginPriority(LoadPriority.Lowest)]
    private void OnEnable()
    {
        Instance = this;
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
        Log.Info("Axwabo.CommandSystem has been enabled!");
    }

    [PluginUnload]
    private void OnDisable()
    {
        ProcessPlayersListPatch.UnregisterEvent();
        Instance = null;
        CommandRegistrationProcessor.UnregisterAll(this);
        _harmony.UnpatchAll();
        Log.Info("Axwabo.CommandSystem has been disabled!");
    }

}

#endif
