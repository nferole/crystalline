using Cry.Features.Commands;
using Cry.Features.Connection;
using Cry.Features.FileWriters;
using Cry.Features.Inputs;
using Cry.Features.Logging;
using Ferole.CommandLineUtils.Inputs;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IO;
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
                using var connection = _connectionFactory.CreateSqlServerConnection();
                await connection.OpenAsync();

                using var command = new SqlCommand(_cliCommand.Query, connection);
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

        private async Task ExtractAndSaveFilesAsync(SqlDataReader reader)
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

            if (!await _fileWriter.SaveFileAsync(outputPath, blob))
            {
                var message = $"File {outputPath} already exists, skipping.";
                Console.WriteLine(message);
                await _logWriter.WriteLineAsync(message);
            }
            else
            {
                var message = "";
                foreach(var column in _cliCommand.ColumnsLog)
                {
                    await _logWriter.WriteLineAsync($"{reader[column]},{fileName}");

                }
            }
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



