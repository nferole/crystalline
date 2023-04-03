using Cry.Features.Inputs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Cry.Features.Help
{
    public static class HelpHeader
    {
        public static string GetLogoString()
        {
            return 
    @"
     ██████╗██████╗ ██╗   ██╗███████╗████████╗ █████╗ ██╗     ██╗███╗   ██╗███████╗
    ██╔════╝██╔══██╗╚██╗ ██╔╝██╔════╝╚══██╔══╝██╔══██╗██║     ██║████╗  ██║██╔════╝
    ██║     ██████╔╝ ╚████╔╝ ███████╗   ██║   ███████║██║     ██║██╔██╗ ██║█████╗  
    ██║     ██╔══██╗  ╚██╔╝  ╚════██║   ██║   ██╔══██║██║     ██║██║╚██╗██║██╔══╝  
    ╚██████╗██║  ██║   ██║   ███████║   ██║   ██║  ██║███████╗██║██║ ╚████║███████╗
     ╚═════╝╚═╝  ╚═╝   ╚═╝   ╚══════╝   ╚═╝   ╚═╝  ╚═╝╚══════╝╚═╝╚═╝  ╚═══╝╚══════╝

    Created by Niklas Ferole, Butrint Ferole and Jonathan Tellbe
    This application extracts files from various databases and saves them to a specified output folder.

Options:
    --help          Display this help text.

Command Line Arguments:
    ";

        }
    }
}
