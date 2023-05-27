using System;
using Axwabo.CommandSystem.Translations;

namespace Axwabo.CommandSystem.Example.Translations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public sealed class GreetingTranslationAttribute : CommandResultTranslationAttribute
{

    public GreetingTranslationAttribute(GreetingType value) : base(value, true)
    {
    }

}
