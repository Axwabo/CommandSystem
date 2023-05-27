using Axwabo.Helpers.Config.Translations;

namespace Axwabo.CommandSystem.Translations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
public abstract class CommandResultTranslationAttribute : TranslationAttribute, ITranslationRegisteredTrigger
{

    public bool? IsSuccess { get; init; }

    protected CommandResultTranslationAttribute(Enum value, bool? isSuccess = null) : base(value) => IsSuccess = isSuccess;

    public void OnProcessed(MemberInfo member)
    {
        if (IsSuccess.HasValue)
            CommandResultTranslationSuccessManager.Register(EnumType, Value, IsSuccess.Value);
    }

}
