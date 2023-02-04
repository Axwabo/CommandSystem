namespace Axwabo.CommandSystem.Structs;

public readonly struct CommandResultOnTarget {

    public readonly ReferenceHub Target;

    public readonly string Response;

    public readonly bool Success;

    public bool IsEmpty => string.IsNullOrEmpty(Response);

    public CommandResultOnTarget(ReferenceHub target, string response, bool success = true) {
        Target = target;
        Response = response;
        Success = success;
    }

    public CommandResultOnTarget(ReferenceHub target, bool success) {
        Target = target;
        Response = null;
        Success = success;
    }

    public static implicit operator bool(CommandResultOnTarget r) => r.Success;

}
