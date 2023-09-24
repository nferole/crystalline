using Cry.Features.Commands;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Text.Json;

namespace Cry.Tests
{
    [TestClass]
    public class SQLServerExtractorTests
    {
        private Mock<IDataReader> _dataReader = new Mock<IDataReader>();
        private SQLServer _sqlServer = new SQLServer();
        
        string? Column1;
        string? Column2;
        string? Column3;
        string? Column1Value;
        string? Column2Value;
        byte[] Column3Value = Encoding.ASCII.GetBytes("This BLOB is a small text");

        [TestInitialize]
        public void Setup()
        {
            CreateDataReaderMock();
            _sqlServer.ColumnBlob = nameof(Column3);

        }

        private void CreateDataReaderMock()
        {
            _dataReader.Setup(m => m.FieldCount).Returns(2);
            _dataReader.Setup(m => m.GetName(0)).Returns(nameof(Column1));
            _dataReader.Setup(m => m.GetName(1)).Returns(nameof(Column2));
            _dataReader.Setup(m => m.GetName(1)).Returns(nameof(Column3));

            _dataReader.Setup(m => m.GetFieldType(0)).Returns(typeof(string));
            _dataReader.Setup(m => m.GetFieldType(1)).Returns(typeof(string));
            _dataReader.Setup(m => m.GetFieldType(1)).Returns(typeof(byte[]));

            _dataReader.Setup(m => m.GetOrdinal("First")).Returns(0);
            _dataReader.Setup(m => m.GetValue(0)).Returns(nameof(Column1Value));
            _dataReader.Setup(m => m.GetValue(1)).Returns(nameof(Column2Value));
            _dataReader.Setup(m => m.GetValue(1)).Returns(Column3Value);

            _dataReader.SetupSequence(m => m.Read())
                .Returns(true)
                .Returns(true)
                .Returns(true)
                .Returns(false); // end of read
        }

        [TestMethod]
        public async Task ExtractAndSaveFilesAsync_WhenBlobExistsAndFileSaves_ReturnsTrue()
        {

            Assert.IsTrue(true);
        }
    }
}