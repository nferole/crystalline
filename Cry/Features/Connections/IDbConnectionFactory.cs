﻿using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cry.Features.Connection
{
    public interface IDbConnectionFactory
    {
        SqlConnection CreateSqlServerConnection();
    }
}
