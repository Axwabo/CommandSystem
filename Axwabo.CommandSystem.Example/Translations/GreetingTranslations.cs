namespace Axwabo.CommandSystem.Example.Translations;

public sealed class GreetingTranslations
{

    [GreetingTranslation(GreetingType.Hi)]
    public string Hi { get; set; } = "Hi!";

    [GreetingTranslation(GreetingType.HelloName)]
    public string Hello { get; set; } = "Hello, {0}!"; // string formatting is supported

    [GreetingTranslation(GreetingType.Welcome)]
    public string Welcome { get; set; } = "Welcome to the server!";

    [GreetingTranslation(GreetingType.Gday)]
    public string Gday { get; set; } = "G'day, mate!";

    [GreetingTranslation(GreetingType.LookingFancy)]
    public string LookingFancy { get; set; } = "Looking quite fancy today, {0}!";

}
