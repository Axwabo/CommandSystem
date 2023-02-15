#if EXILED
using Exiled.API.Interfaces;

namespace Axwabo.CommandSystem;

public sealed class Config : IConfig {

    public bool IsEnabled { get; set; } = true;

    public bool Debug { get; set; } = false;

}

#endif
