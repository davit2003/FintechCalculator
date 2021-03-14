using Dapper;
using FintechCalculator.DataModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace FintechCalculator.Services
{
	public interface IDapper : IDisposable
	{
		DbConnection GetDbConnection();
		decimal GetT<T>(MapData mapData, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
		ReturnModel GetAll<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
		int Execute(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
		T Insert<T>(IDbConnection db, string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
		T Update<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
	}

}