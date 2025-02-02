namespace Axwabo.CommandSystem;

internal static class Log
{

    public static void Debug(object obj) => PluginAPI.Core.Log.Debug(obj?.ToString(), Plugin.Instance?.Config.Debug ?? true);

    public static void Info(object obj) => PluginAPI.Core.Log.Info(obj?.ToString());

    public static void Warn(object obj) => PluginAPI.Core.Log.Warning(obj?.ToString());

    public static void Error(object obj) => PluginAPI.Core.Log.Error(obj?.ToString());

}
