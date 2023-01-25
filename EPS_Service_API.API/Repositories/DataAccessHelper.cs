/*
// Using empty parameter
return await _dataAccess.QueryData<ProductDTO, dynamic>(sqlStatement, new { }, _connectionStringName);

// Using anonymous
return await _dataAccess.QueryData<ProductDTO, dynamic>(sqlStatement, new { CategoryId = categoryId }, _connectionStringName);

// Using Dynamic Parameters
DynamicParameters p = new DynamicParameters();
p.Add("CategoryId", productDTO.categoryId);
return await _dataAccess.QueryData<ProductDTO, dynamic>(sqlStatement, p, _connectionStringName);

// Using anonymous
return await _dataAccess.ExecuteData(sqlStatement, new { Id = id }, _connectionStringName);

// Using Dynamic Parameters
DynamicParameters p = new DynamicParameters();
p.Add("Title", productDTO.Title);
p.Add("CategoryId", productDTO.CategoryId);
return await _dataAccess.ExecuteData(sqlStatement, p, _connectionStringName);

// Using Dynamic Parameters with output parameter
DynamicParameters p = new DynamicParameters();
p.Add("Id", DbType.Int32, direction: ParameterDirection.Output);
p.Add("Name", vendorDTO.Name);
await _dataAccess.ExecuteData(sqlStatement, p, _connectionStringName);
return p.Get<int>("Id");
*/

using Dapper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace EPS_Service_API.Repositories
{
    public class DataAccessHelper
    {
        private readonly IConfiguration _config;

        public DataAccessHelper(IConfiguration config)
        {
            _config = config;
        }

        public async Task<List<T>> QueryData<T, U>(string storedProcedure, U parameters)
        {
            using (IDbConnection connection = new MySqlConnection(_config.GetConnectionString("MySQL")))
            {
                var rows = await connection.QueryAsync<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                return rows.ToList();
            }
        }

        public async Task<int> ExecuteData<T>(string storedProcedure, T parameters)
        {
            using (IDbConnection connection = new MySqlConnection(_config.GetConnectionString("MySQL")))
            {
                return await connection.ExecuteAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
            }
        }

    }
}