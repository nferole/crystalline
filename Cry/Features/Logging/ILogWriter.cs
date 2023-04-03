using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cry.Features.Logging
{
    public interface ILogWriter
    {
        Task WriteLineAsync(string message);
    }
}
