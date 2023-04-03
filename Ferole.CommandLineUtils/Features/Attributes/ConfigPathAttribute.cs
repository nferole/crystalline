using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferole.CommandLineUtils.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ConfigPathAttribute : Attribute
    {
    }
}
