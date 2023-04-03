using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Cry.Features.FileWriters;

namespace Cry.Features.DataExtractors
{
    internal class Base64StringDataExtractor : IDataExtractor
    {
        private readonly string _base64String;
        private readonly string _path;
        private readonly IFileWriter _fileWriter;

        public Base64StringDataExtractor(string base64String, string outputPath, IFileWriter fileWriter)
        {
            _base64String = base64String;
            _path = outputPath;
            _fileWriter = fileWriter;
        }

        public async Task<bool> ExecuteAndSaveFilesAsync()
        {
            try
            {
                byte[] bytes = Convert.FromBase64String(_base64String);
                string decodedString = Encoding.UTF8.GetString(bytes);
                await _fileWriter.SaveFileAsync(_path, bytes);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while decoding and saving the file: {ex.Message}");
                return false;
            }
        }
    }
}
