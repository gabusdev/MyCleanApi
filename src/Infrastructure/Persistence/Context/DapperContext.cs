using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Context;
internal class DapperContext
{
    private readonly string _connectionString;
    public DapperContext(IConfiguration config)
    {
        _connectionString = config["DatabaseSettings:ConnectionString"] 
            ?? throw new InvalidOperationException("There is no Connection String");
    }
    public IDbConnection CreateConnection()
        => new SqlConnection(_connectionString);
}
