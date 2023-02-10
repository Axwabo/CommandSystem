namespace Axwabo.CommandSystem.Structs;

public readonly struct CommandResult {

    #region Static

    public static CommandResult Succeeded(string response) => new(true, response);

    public static CommandResult Failed(string response) => new(false, response);

    #endregion

    #region Members

    public readonly string Response;

    public readonly bool Success;

    public bool IsEmpty => string.IsNullOrEmpty(Response);

    public CommandResult(string response) {
        var success = string.IsNullOrEmpty(response) || !response.StartsWith("!");
        Response = success ? response : response.Substring(1);
        Success = success;
    }

    public CommandResult(bool success, string response) {
        Response = response;
        Success = success;
    }

    #endregion

    #region Casts

    public static implicit operator CommandResult(string s) => new(s);

    public static implicit operator CommandResult(bool s) => new(s, null);

    public static implicit operator bool(CommandResult r) => r.Success;

    #endregion

}
