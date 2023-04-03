using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cry.Features.Logging
{
    public class LogWriter : ILogWriter
    {
        private readonly string _outputPath;
        public LogWriter(string outputPath)
        {
            _outputPath = outputPath;
        }

        public async Task WriteLineAsync(string line)
        {
            await using var sw = new StreamWriter(_outputPath, true);
            await sw.WriteLineAsync(line);
        }
    }
}
