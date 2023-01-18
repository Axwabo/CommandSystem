using System;

namespace Axwabo.CommandSystem.PropertyManager {

    public delegate string CommandNameResolver<in TAttribute>(TAttribute attribute) where TAttribute : Attribute;
    
    public delegate string CommandDescriptionResolver<in TAttribute>(TAttribute attribute) where TAttribute : Attribute;
    
    

}
