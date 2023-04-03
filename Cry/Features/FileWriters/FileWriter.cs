using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cry.Features.FileWriters
{
    public class FileWriter : IFileWriter
    {
        public async Task<bool> SaveFileAsync(string outputPath, byte[] data)
        {
            if (File.Exists(outputPath))
                return false;

            await using var writer = new BinaryWriter(File.OpenWrite(outputPath));
            writer.Write(data);
            return true;
        }
    }

}
