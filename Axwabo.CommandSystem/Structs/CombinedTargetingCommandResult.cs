#pragma warning disable CS1591
namespace Axwabo.CommandSystem.Structs;

public readonly ref struct CombinedTargetingCommandResult
{

    public readonly int Affected;

    public readonly string Response;


    public CombinedTargetingCommandResult(int affected, string response)
    {
        Affected = affected;
        Response = response;
    }

    public static implicit operator CombinedTargetingCommandResult(string s) => new(0, s);

    public static implicit operator bool(CombinedTargetingCommandResult r) => !string.IsNullOrEmpty(r.Response);

}
