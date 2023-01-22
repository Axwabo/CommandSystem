using System;

namespace Axwabo.CommandSystem.Registration.AttributeResolvers {

    internal interface IResolverContainer<out TReturn> {
        
        bool Takes(Type type);

        TReturn Invoke(Attribute attribute);

    }

}
