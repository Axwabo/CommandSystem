namespace Axwabo.CommandSystem.Structs;

public delegate bool TryParseDelegate<T>(string value, out T result);
