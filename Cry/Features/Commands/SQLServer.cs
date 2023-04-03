using Cry.Features.Connection;
using Cry.Features.DataExtractors;
using Cry.Features.FileWriters;
using Cry.Features.Logging;
using Ferole.CommandLineUtils.Attributes;
using Ferole.CommandLineUtils.Inputs;

namespace Cry.Features.Commands
{
    public class SQLServer : ICLICommand
    {
        [Help("Endpoint to database")]
        public string? Endpoint { get; set; }
        [Help("User with access")]
        public string? Username { get; set; }
        [Help("User password")]
        public string? Password { get; set; }
        [Help("Query to extract data from")]
        public string? Query { get; set; }
        [Help("Set static output file type, same for all files")]
        public string FileType { get; set; } = String.Empty;
        [Help("Column from [Query] to use as output file type, i.e: png")]
        public string? ColumnFileType { get; set; }
        [Help("Output folder")]
        public string Output { get; set; } = String.Empty;
        [Help("Column from [Query] to use as output file name")]
        public string? ColumnFileName { get; set; }
        [Help("Static file-name. Only useful when the query returns 1 result")]
        public string? FileName { get; set; }
        [Help("Database / Initial Catalog")]
        public string? Database { get; set; }
        [Help("The BLOB data to save to a file")]
        public string? ColumnBlob { get; set; }
        [Help("Set a path to a file to enable transformation logging")]
        public string LogPath { get; set; } = String.Empty;
        [Help("Specify a csv list of column names from [Query] to build a log message: [Column1...]>Filename.FileType")]
        public List<string> ColumnsLog { get; set; } = new List<string>();

        [ConfigPath]
        public string? Config { get; set; } = string.Empty;

        public async Task<bool> ExecuteCommandAsync()
        {
            var fileWriter = new FileWriter();
            var logWriter = new LogWriter(LogPath);
            var connectionFactory = new SqlServerConnectionFactory(this);
            var sqlExtractor = new SQLServerExtractor(this, connectionFactory, fileWriter, logWriter);
            var success = await sqlExtractor.ExecuteAndSaveFilesAsync();
            if (success)
            {
                Console.WriteLine("File(s) extracted and saved successfully.");
            }
            else
            {
                Console.WriteLine("Failed to extract and save file(s).");
            }
            return success;
        }

    }
}
