using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Infra.Persistence.Dapper
{
  public class Dapper : IDapper
  {
    private readonly IConfiguration _configuration;
    private readonly string _connectionStringName;

    public Dapper(
      IConfiguration configuration,
      string connectionStringName)
    {
      _configuration = configuration;
      _connectionStringName = connectionStringName;
    }

    public void Dispose()
    {
    }

    public int Execute(
      string sqlQuery,
      DynamicParameters parameters,
      CommandType commandType = CommandType.StoredProcedure)
    {
      throw new NotImplementedException();
    }

    public T Get<T>(
      string sqlQuery,
      DynamicParameters parameters,
      CommandType commandType = CommandType.Text)
    {
      using IDbConnection db = GetDbConnection();

      return db.Query<T>(
          sqlQuery,
          parameters,
          commandType: commandType)
        .FirstOrDefault();
    }

    public List<T> GetAll<T>(
      string sqlQuery,
      DynamicParameters parameters,
      CommandType commandType = CommandType.StoredProcedure)
    {
      using IDbConnection db = GetDbConnection();

      return db.Query<T>(
          sqlQuery,
          parameters,
          commandType: commandType)
        .ToList();
    }

    public DbConnection GetDbConnection()
    {
      return new SqlConnection(_configuration.GetConnectionString(_connectionStringName));
    }
  }
}
