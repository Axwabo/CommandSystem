namespace Axwabo.CommandSystem;

internal static class Log
{

    public static void Debug(object obj) =>
#if EXILED
        Exiled.API.Features.Log.Debug(obj);
#else
        PluginAPI.Core.Log.Debug(obj?.ToString());
#endif

    public static void Info(object obj) =>
#if EXILED
        Exiled.API.Features.Log.Info(obj);
#else
        PluginAPI.Core.Log.Info(obj?.ToString());
#endif


    public static void Warn(object obj) =>
#if EXILED
        Exiled.API.Features.Log.Warn(obj);
#else
        PluginAPI.Core.Log.Warning(obj?.ToString());
#endif

    public static void Error(object obj) =>
#if EXILED
        Exiled.API.Features.Log.Error(obj);
#else
        PluginAPI.Core.Log.Error(obj?.ToString());
#endif

}
