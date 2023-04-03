using Cry.Features.Commands;
using Cry.Features.Inputs;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cry.Features.Connection
{
    public class SqlServerConnectionFactory : IDbConnectionFactory
    {
        private SQLServer _sqlServerData;

        public SqlServerConnectionFactory(SQLServer sqlServerData)
        {
            _sqlServerData = sqlServerData;
        }

        public SqlConnection CreateSqlServerConnection()
        {
            var builder = new SqlConnectionStringBuilder
            {
                DataSource = _sqlServerData.Endpoint,
                UserID = _sqlServerData.Username,
                Password = _sqlServerData.Password,
                InitialCatalog = _sqlServerData.Database
            };

            return new SqlConnection(builder.ConnectionString);
        }
    }
}
