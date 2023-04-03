using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferole.CommandLineUtils.Inputs
{
    public interface ICLICommand
    {
        Task<bool> ExecuteCommandAsync();
    }
}
