namespace Axwabo.CommandSystem {

    public readonly ref struct TargetingCommandResult {

        public readonly int Affected;

        public readonly string Response;


        public TargetingCommandResult(int affected, string response) {
            Affected = affected;
            Response = response;
        }

        public static implicit operator TargetingCommandResult(string s) => new(0, s);

        public static implicit operator bool(TargetingCommandResult r) => !string.IsNullOrEmpty(r.Response);

    }

}
