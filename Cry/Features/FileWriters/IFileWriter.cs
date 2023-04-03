using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cry.Features.FileWriters
{
    public interface IFileWriter
    {
        Task<bool> SaveFileAsync(string outputPath, byte[] data);
    }
}
