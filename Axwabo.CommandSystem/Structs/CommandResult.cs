namespace Axwabo.CommandSystem.Structs {

    public readonly ref struct CommandResult {

        public readonly string Response;

        public readonly bool Success;

        public CommandResult(string response) {
            var success = string.IsNullOrEmpty(response) || !response.StartsWith("!");
            Response = success ? response : response.Substring(1);
            Success = success;
        }

        public CommandResult(string response, bool success) {
            Response = response;
            Success = success;
        }

        public bool IsEmpty => string.IsNullOrEmpty(Response);

        public static implicit operator CommandResult(string s) => new(s);

        public static implicit operator CommandResult(bool s) => new(null, s);

        public static implicit operator bool(CommandResult r) => r.Success;

    }

}
