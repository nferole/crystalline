using Ferole.CommandLineUtils.ConsoleHelpers;

namespace Cry.Features.FileWriters
{
    public class FileWriter : IFileWriter
    {
        public async Task<bool> SaveFileAsync(string outputPath, byte[] data)
        {
            bool success = false;
            if (!File.Exists(outputPath))
            {
                try
                {
                    await using var writer = new BinaryWriter(File.OpenWrite(outputPath));
                    writer.Write(data);
                    success = true;
                }
                catch (IOException ioEx)
                {
                    CliExt.WriteSameLineColor($"\rAn I/O error occurred: {ioEx.Message}. Please check if the file is in use or the path is accessible.", ConsoleColor.Red);
                }
                catch (UnauthorizedAccessException uaEx)
                {
                    CliExt.WriteSameLineColor($"\rAn access error occurred: {uaEx.Message}. Please check your permissions for the path: {outputPath}", ConsoleColor.Red);
                }
                catch (Exception e)
                {
                    CliExt.WriteSameLineColor($"\rAn unexpected error occurred: {e.Message}", ConsoleColor.Red);
                }
            }
            else
            {
                CliExt.WriteSameLineColor($"\rThe file {outputPath} already exists. Skipping file writing operation.", ConsoleColor.Yellow);
            }

            return success;
        }
    }



}
