using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferole.CommandLineUtils.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class HelpAttribute : Attribute
    {
        public string HelpInfo { get; }
        public bool Visible { get; }

        public HelpAttribute(string helpinfo, bool visible = true)
        {
            HelpInfo = helpinfo;
            Visible = visible;
        }
    }
}
