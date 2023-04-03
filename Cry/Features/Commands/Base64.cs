using Cry.Features.DataExtractors;
using Cry.Features.FileWriters;
using Ferole.CommandLineUtils.Attributes;
using Ferole.CommandLineUtils.Inputs;

namespace Cry.Features.Commands
{
    public class Base64 : ICLICommand
    {
        [Help("Base64-encoded string")]
        [Shorthand("s")]
        public string String { get; set; } = String.Empty;
        [Help("Output file")]
        [Shorthand("o")]
        public string Output { get; set; } = String.Empty;
        [ConfigPath]
        public string? Config { get; set; } = string.Empty;
        public async Task<bool> ExecuteCommandAsync()
        {
            var fileWriter = new FileWriter();
            var base64Extractor = new Base64StringDataExtractor(this.String, Output, fileWriter);
            var success = await base64Extractor.ExecuteAndSaveFilesAsync();
            if (success)
            {
                Console.WriteLine("File(s) extracted and saved successfully.");
            }
            else
            {
                Console.WriteLine("Failed to extract and save file(s).");
            };
            return success;
        }
    }
}
