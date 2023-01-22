using System;
using Axwabo.CommandSystem.Attributes.Interfaces;

namespace Axwabo.CommandSystem.Attributes {

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class UsageAttribute : Attribute, IUsage {

        public string[] Usage { get; }

        public UsageAttribute(params string[] usage) => Usage = usage;

    }

}
