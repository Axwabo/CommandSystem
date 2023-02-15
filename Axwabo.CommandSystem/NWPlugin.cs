#if !EXILED
using System;
using Axwabo.CommandSystem.Registration;
using HarmonyLib;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;

namespace Axwabo.CommandSystem;

public sealed class Plugin {

    private Harmony _harmony;

    [PluginEntryPoint("Axwabo.CommandSystem", "1.0.0", "Adds a sophisticated command system to the game.", "Axwabo")]
    [PluginPriority(LoadPriority.Lowest)]
    private void OnEnable() {
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
        CommandRegistrationProcessor.UnregisterAll(this);
        _harmony.UnpatchAll();
        Log.Info("Axwabo.CommandSystem has been disabled!");
    }

}

#endif
