using Cry.Features.Commands;
using Cry.Features.Connection;
using Cry.Features.FileWriters;
using Cry.Features.Inputs;
using Cry.Features.Logging;
using Ferole.CommandLineUtils.ConsoleHelpers;
using Ferole.CommandLineUtils.Inputs;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Cry.Features.DataExtractors
{

    public class SQLServerExtractor : IDataExtractor
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IFileWriter _fileWriter;
        private readonly ILogWriter _logWriter;
        private readonly SQLServer _cliCommand;

        public SQLServerExtractor(SQLServer cliCommand, IDbConnectionFactory connectionFactory, IFileWriter fileWriter, ILogWriter logWriter)
        {
            _connectionFactory = connectionFactory;
            _fileWriter = fileWriter;
            _logWriter = logWriter;
            _cliCommand = cliCommand;
        }

        public async Task<bool> ExecuteAndSaveFilesAsync()
        {
            try
            {
                Console.WriteLine("Connecting to target database...");

                using var connection = _connectionFactory.CreateSqlServerConnection();
                await connection.OpenAsync();
                Console.WriteLine("Connection to database established.");

                using var command = new SqlCommand(_cliCommand.Query, connection);

                Console.WriteLine("Executing Query...");
                using var reader = await command.ExecuteReaderAsync();

                ValidateReaderHasRows(reader);

                while (await reader.ReadAsync())
                {
                    await ExtractAndSaveFilesAsync(reader);
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }

            return true;
        }

        private static void ValidateReaderHasRows(SqlDataReader reader)
        {
            if (!reader.HasRows)
            {
                throw new InvalidOperationException("No rows found");
            }
        }

        private async Task<bool> ExtractAndSaveFilesAsync(SqlDataReader reader)
        {
            string fileName = GetFileName(reader);
            string fileType = GetFileType(reader);
            object blobObject = reader[_cliCommand.ColumnBlob];

            if (blobObject == null)
            {
                throw new InvalidOperationException("BLOB-data does not exist");
            }

            byte[] blob = GetByteArrayFromColumn(blobObject);
            string outputFile = $"{fileName}.{fileType}";
            string outputPath = Path.Combine(_cliCommand.Output, outputFile);

            CliExt.WriteSameLine($"Processing file {outputPath}...");
            if (!await _fileWriter.SaveFileAsync(outputPath, blob))
            {
                var message = $"File {outputPath} could not be written, skipping.";
                CliExt.WriteSameLineColor(message, ConsoleColor.Yellow);
                await _logWriter.WriteLineAsync(message);
                return false;
            }
            else
            {
                CliExt.WriteSameLine($"\rVerifying {outputPath}...");
                if (_cliCommand.VerifyFiles)
                    await VerifyFileMatchesData(blob, outputPath);

                CliExt.WriteSameLineColor($"\rFile {outputFile} verified and done!", ConsoleColor.Green);

                foreach (var column in _cliCommand.ColumnsLog)
                {
                    await _logWriter.WriteLineAsync($"{reader[column]},{fileName}");
                }

                return true;
            }
        }
        private async Task VerifyFileMatchesData(byte[] originalData, string filePath)
        {
            byte[] fileData = await File.ReadAllBytesAsync(filePath);

            string originalHash = ComputeHash(originalData);
            string fileHash = ComputeHash(fileData);

            if (originalHash != fileHash)
            {
                var message = $"Mismatch error: {filePath} does not match the original data. Saved to log.";
                CliExt.WriteSameLine(message);
                await _logWriter.WriteLineAsync(message);
            }
        }
        private string ComputeHash(byte[] data)
        {
            using var sha256 = SHA256.Create();
            byte[] hash = sha256.ComputeHash(data);
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }
        private static byte[] GetByteArrayFromColumn(object blobObject)
        {
            byte[] blob;
            if (blobObject.GetType() == typeof(byte[]))
            {
                blob = (byte[])blobObject;
            }
            else if (blobObject.GetType() == typeof(string))
            {
                string base64String = (string)blobObject;
                blob = Convert.FromBase64String(base64String);
            }
            else
            {
                throw new InvalidOperationException("Invalid BLOB-data type");
            }

            return blob;
        }
        private string GetFileType(SqlDataReader reader)
        {
            string? fileType = null;
            if (!string.IsNullOrEmpty(_cliCommand.ColumnFileType))
            {
                fileType = reader[_cliCommand.ColumnFileType].ToString();
            }
            if (!string.IsNullOrEmpty(_cliCommand.FileType))
            {
                fileType = _cliCommand.FileType;
            }
            if (fileType == null)
                throw new Exception("No output filen name assigned.");
            
            return fileType;
        }
        private string GetFileName(SqlDataReader reader)
        {
            string? filename = null;
            if(!string.IsNullOrEmpty(_cliCommand.ColumnFileName))
            {
                filename = reader[_cliCommand.ColumnFileName].ToString();
            }
            if(!string.IsNullOrEmpty(_cliCommand.FileName))
            {
                filename = _cliCommand.FileName;
            }
            if(filename == null)
            {
                throw new Exception("No output filen name assigned.");
            }
            return filename;
        }
    }

}



