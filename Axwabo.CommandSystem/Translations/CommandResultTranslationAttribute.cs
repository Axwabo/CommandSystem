using Axwabo.Helpers.Config.Translations;

namespace Axwabo.CommandSystem.Translations;

/// <summary>
/// A <see cref="TranslationAttribute"/> storing the success state of the command result. This attribute is inherited.
/// </summary>
/// <seealso cref="CommandResultTranslationSuccessManager"/>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
public abstract class CommandResultTranslationAttribute : TranslationAttribute, ITranslationRegisteredTrigger
{

    /// <summary>
    /// Determines the success state of the command result.
    /// </summary>
    /// <remarks>If null, the state will be inferred from the result string (false if the string starts with an !).</remarks>
    public bool? IsSuccess { get; init; }

    /// <summary>
    /// Creates a new <see cref="CommandResultTranslationAttribute"/> instance.
    /// </summary>
    /// <param name="value">The translation key.</param>
    /// <param name="isSuccess">The success state of the command result.</param>
    /// <remarks>If <paramref name="isSuccess"/> null, the state will be inferred from the result string (false if the string starts with an !).</remarks>
    protected CommandResultTranslationAttribute(Enum value, bool? isSuccess = null) : base(value) => IsSuccess = isSuccess;

    /// <inheritdoc />
    public void OnProcessed(MemberInfo member)
    {
        if (IsSuccess.HasValue)
            CommandResultTranslationSuccessManager.Register(EnumType, Value, IsSuccess.Value);
    }

}
