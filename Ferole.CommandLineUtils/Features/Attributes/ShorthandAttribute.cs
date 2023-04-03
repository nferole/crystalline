using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferole.CommandLineUtils.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ShorthandAttribute : Attribute
    {
        public string Shorthand { get; }

        public ShorthandAttribute(string shorthand)
        {
            Shorthand = shorthand;
        }
    }
}
