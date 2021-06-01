using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Dapper;

namespace Infra.Persistence.Dapper
{
  public interface IDapper : IDisposable
  {
    DbConnection GetDbConnection();

    T Get<T>(
      string sqlQuery,
      DynamicParameters parameters,
      CommandType commandType = CommandType.StoredProcedure);

    List<T> GetAll<T>(
      string sqlQuery,
      DynamicParameters parameters,
      CommandType commandType = CommandType.StoredProcedure);

    int Execute(
      string sqlQuery,
      DynamicParameters parameters,
      CommandType commandType = CommandType.StoredProcedure);
  }
}
