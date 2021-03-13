using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace FintechCalculator.Services
{
	public class Dapper : IDapper
	{
		private readonly IConfiguration _config;
		private string Connectionstring = "DefaultConnection";

		public Dapper(IConfiguration config)
		{
			_config = config;
		}
		public void Dispose()
		{
			throw new NotImplementedException();
		}

		public int Execute(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
		{
			throw new NotImplementedException();
		}

		public List<T> GetAll<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
		{
			using IDbConnection db = new SqlConnection(_config.GetConnectionString(Connectionstring));
			return db.Query<T>(sp, parms, commandType: commandType).ToList();
		}

		public DbConnection GetDbConnection()
		{
			return new SqlConnection(_config.GetConnectionString(Connectionstring));
		}

		public T GetT<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
		{
			using IDbConnection db = new SqlConnection(_config.GetConnectionString(Connectionstring));
			return db.Query<T>(sp, parms, commandType: commandType).FirstOrDefault();
		}
		public T Insert<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
		{
			T result;
			using IDbConnection db = new SqlConnection(_config.GetConnectionString(Connectionstring));
			try
			{
				if (db.State == ConnectionState.Closed)
					db.Open();

				using var tran = db.BeginTransaction();
				try
				{
					result = db.Query<T>(sp, parms, commandType: commandType, transaction: tran).FirstOrDefault();
					tran.Commit();
				}
				catch (Exception ex)
				{
					tran.Rollback();
					throw ex;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				if (db.State == ConnectionState.Open)
					db.Close();
			}

			return result;
		}


		public T Update<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
		{
			{
				T result;
				using IDbConnection db = new SqlConnection(_config.GetConnectionString(Connectionstring));
				try
				{
					if (db.State == ConnectionState.Closed)
						db.Open();

					using var tran = db.BeginTransaction();
					try
					{
						result = db.Query<T>(sp, parms, commandType: commandType, transaction: tran).FirstOrDefault();
						tran.Commit();
					}
					catch (Exception ex)
					{
						tran.Rollback();
						throw ex;
					}
				}
				catch (Exception ex)
				{
					throw ex;
				}
				finally
				{
					if (db.State == ConnectionState.Open)
						db.Close();
				}

				return result;
			}
		}

	}
}
