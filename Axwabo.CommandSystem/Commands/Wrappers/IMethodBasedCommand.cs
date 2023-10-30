namespace Axwabo.CommandSystem.Commands.Wrappers;

internal interface IMethodBasedCommand
{

    ContainerCommand Container { get; }

    MethodInfo ExecuteMethod { get; }

}
