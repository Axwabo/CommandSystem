using Logger = LabApi.Features.Console.Logger;

namespace Axwabo.CommandSystem;

internal static class Log
{

    public static void Debug(object obj) => Logger.Debug(obj?.ToString() ?? "", CommandSystemPlugin.Instance?.Config?.Debug ?? true);

    public static void Info(object obj) => Logger.Info(obj?.ToString() ?? "");

    public static void Warn(object obj) => Logger.Warn(obj?.ToString() ?? "");

    public static void Error(object obj) => Logger.Error(obj?.ToString() ?? "");

}
