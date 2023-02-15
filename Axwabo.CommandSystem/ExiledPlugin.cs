#if EXILED
using System;
using Axwabo.CommandSystem.Registration;
using Exiled.API.Enums;
using Exiled.API.Features;
using HarmonyLib;

namespace Axwabo.CommandSystem;

public sealed class Plugin : Plugin<Config> {

    private Harmony _harmony;

    public override void OnEnabled() {
        _harmony = new Harmony("Axwabo.CommandSystem");
        try {
            _harmony.PatchAll();
        } catch (Exception e) {
            Log.Error("Patching failed! Some features will not work properly.\n" + e);
        }

        CommandRegistrationProcessor.RegisterAll(this);
        Log.Info("Axwabo.CommandSystem has been enabled!");
    }

    public override void OnDisabled() {
        CommandRegistrationProcessor.UnregisterAll(this);
        _harmony.UnpatchAll();
        Log.Info("Axwabo.CommandSystem has been disabled!");
    }

    public override string Name => "Axwabo.CommandSystem";
    public override string Prefix => "CommandSystem";
    public override string Author => "Axwabo";
    public override PluginPriority Priority => PluginPriority.Highest;
    public override Version Version { get; } = new(1, 0, 0);

}

#endif
