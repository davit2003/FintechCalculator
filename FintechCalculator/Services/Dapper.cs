using Dapper;
using FintechCalculator.DataModels;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
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

		public ReturnModel GetAll<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
		{
			var returnMomdel = new ReturnModel();
			using IDbConnection db =
				new SqlConnection(_config.GetConnectionString(Connectionstring));
			//parms = new DynamicParameters();


			//parms.Add("@Kind", InvoiceKind.WebInvoice, DbType.Int32, ParameterDirection.Input);
			//parms.Add("@Code", "Many_Insert_0", DbType.String, ParameterDirection.Input);
			//parms.Add("@RowCount", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

			int counter = 1;
			try
			{
				for (int i = 0; i < counter; i++)
				{
					using (var webClient = new WebClient())
					{
						var resp = JsonConvert.DeserializeObject<ReturnModel>(
							webClient.DownloadString($"https://www.myhome.ge/ka/s/?AdTypeID=1&PrTypeID=5&Page={i}&Ajax=1"));
						if (i == 0)
						{
							counter = Convert.ToInt32(resp.Data.Cnt) % 22 != 0 ? (Convert.ToInt32(resp.Data.Cnt) / 22) + 1
								: Convert.ToInt32(resp.Data.Cnt) / 22;
						}
						if (resp.Data != null)
							foreach (var item in resp.Data.Prs)
							{
								parms.Add("@product_id", item.product_id);
								parms.Add("@loc_id ", item.loc_id);
								parms.Add("@street_address", item.street_address);
								parms.Add("@adtype_id", item.adtype_id);
								parms.Add("@product_type_id", item.product_type_id);
								parms.Add("@price", item.price);
								parms.Add("@area_size_value", item.area_size_value);
								parms.Add("@currency_id", item.currency_id);
								parms.Add("@estate_type_id", item.estate_type_id);
								parms.Add("@area_size", item.area_size);
								parms.Add("@area_size_type_id", item.area_size_type_id);
								parms.Add("@map_lat", item.map_lat);
								parms.Add("@map_lon", item.map_lon);
								parms.Add("@special_persons", item.special_persons);
								parms.Add("@bedrooms", item.bedrooms);
								parms.Add("@floor", item.floor);
								parms.Add("@parking_id", item.parking_id);
								parms.Add("@canalization", item.canalization);
								parms.Add("@water", item.water);
								parms.Add("@electricity", item.electricity);
							}

						var resps = Insert<T>(sp, parms, commandType = CommandType.StoredProcedure);

					}
				}

			}
			catch (Exception ex)
			{
				throw ex;
			}
			return returnMomdel;
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
