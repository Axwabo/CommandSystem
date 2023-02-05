using System;

namespace Axwabo.CommandSystem.Registration.Containers;

internal interface IResolverContainer<out TReturn> {

    bool Takes(Type type);

    TReturn Resolve(Attribute attribute);

}
