namespace Axwabo.CommandSystem;

public delegate bool TryParseDelegate<T>(string value, out T result);
