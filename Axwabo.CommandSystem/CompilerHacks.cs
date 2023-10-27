extern alias E;
global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Reflection;
global using System.Reflection.Emit;
global using UnityEngine;
#if EXILED
global using E::Axwabo.Helpers.Pools;
global using static E::Axwabo.Helpers.Harmony.InstructionHelper;
#else
global using Axwabo.Helpers.Pools;
global using static Axwabo.Helpers.Harmony.InstructionHelper;
#endif

// ReSharper disable once CheckNamespace

namespace System.Runtime.CompilerServices;

internal sealed class IsExternalInit
{

}
