using Dapper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace EPS_Service_API.API.Repositories
{
    public class DataHelperSQL_Server
    {

        private readonly IConfiguration _config;

        public DataHelperSQL_Server(IConfiguration config)
        {
            _config = config;
        }

        public async Task<List<T>> QueryData<T, U>(string storedProcedure, U parameters)
        {
            using (IDbConnection connection = new SqlConnection(_config.GetConnectionString("MSSQL")))
            {
                var rows = await connection.QueryAsync<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                return rows.ToList();
            }
        }

        public async Task<int> ExecuteData<T>(string storedProcedure, T parameters)
        {
            using (IDbConnection connection = new SqlConnection(_config.GetConnectionString("MSSQL")))
            {
                return await connection.ExecuteAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
            }
        }

    }
}
