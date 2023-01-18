using System;
using HarmonyLib;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;

namespace Axwabo.CommandSystem {

    public sealed class Plugin {

        public static Plugin Instance { get; private set; }

        private Harmony _harmony;

        [PluginEntryPoint("Axwabo.CommandSystem", "1.0.0", "Adds a sophisticated command system to the game.", "Axwabo")]
        private void OnEnable() {
            Instance = this;
            _harmony = new Harmony("Axwabo.CommandSystem");
            try {
                _harmony.PatchAll();
            } catch (Exception e) {
                Log.Error("Patching failed! Some features will not work properly.\n" + e);
            }

            Log.Info("Axwabo.CommandSystem has been enabled!");
        }

        [PluginUnload]
        private void OnDisable() {
            Instance = null;
            _harmony.UnpatchAll();
            Log.Info("Axwabo.CommandSystem has been disabled!");
        }

    }

}
