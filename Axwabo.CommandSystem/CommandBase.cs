using System;
using Axwabo.CommandSystem.Structs;
using CommandSystem;

namespace Axwabo.CommandSystem {

    public abstract class CommandBase {

        public virtual string Name { get; }
        
        public virtual string Description { get; }
        
        public virtual string[] Aliases { get; }
        
        public virtual string[] Usage { get; }

        public abstract CommandResult Execute(ArraySegment<string> arguments, ICommandSender sender);

    }

}
