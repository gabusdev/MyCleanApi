using Microsoft.Data.SqlClient;
using System.Data;

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
