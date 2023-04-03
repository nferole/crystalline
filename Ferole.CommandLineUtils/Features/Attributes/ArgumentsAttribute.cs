using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferole.CommandLineUtils.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ArgumentsAttribute : Attribute
    {
        public Type ArgumentType { get; }
        public object? Arguments { get; set; }
        public bool Visible { get; }

        public ArgumentsAttribute(Type argumentType)
        {
            ArgumentType = argumentType;
            Arguments = Activator.CreateInstance(type: argumentType);
        }
    }
}
