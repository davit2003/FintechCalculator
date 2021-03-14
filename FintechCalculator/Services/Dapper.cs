using Dapper;
using FintechCalculator.DataModels;
using FintechCalculator.Helpers;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
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
			db.Open();

			//parms.Add("@Kind", InvoiceKind.WebInvoice, DbType.Int32, ParameterDirection.Input);
			//parms.Add("@Code", "Many_Insert_0", DbType.String, ParameterDirection.Input);
			//parms.Add("@RowCount", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

			int counter = 1;
			try
			{
				var parameters = new DynamicParameters();
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
								parameters.Add("@product_id", item.product_id);
								parameters.Add("@loc_id ", item.loc_id);
								parameters.Add("@street_address", item.street_address);
								parameters.Add("@adtype_id", item.adtype_id);
								parameters.Add("@product_type_id", item.product_type_id);
								parameters.Add("@price", item.price);
								parameters.Add("@area_size_value", item.area_size_value);
								parameters.Add("@currency_id", item.currency_id);
								parameters.Add("@estate_type_id", item.estate_type_id);
								parameters.Add("@area_size", item.area_size);
								parameters.Add("@area_size_type_id", item.area_size_type_id);
								parameters.Add("@map_lat", item.map_lat);
								parameters.Add("@map_lon", item.map_lon);
								parameters.Add("@special_persons", item.special_persons);
								parameters.Add("@bedrooms", item.bedrooms);
								parameters.Add("@floor", item.floor);
								parameters.Add("@parking_id", item.parking_id);
								parameters.Add("@canalization", item.canalization);
								parameters.Add("@water", item.water);
								parameters.Add("@electricity", item.electricity);

								var resps = Insert<T>(db, sp, parameters, commandType = CommandType.StoredProcedure);
							}


					}
				}

			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				db.Close();
			}
			return returnMomdel;
		}

		public DbConnection GetDbConnection()
		{
			return new SqlConnection(_config.GetConnectionString(Connectionstring));
		}

		public double GetT<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
		{
			using IDbConnection db = new SqlConnection(_config.GetConnectionString(Connectionstring));
			var parameters = new DynamicParameters();
			var coordinates = new Tuple<double, double>(0, 0);
			try
			{

				db.Open();
				var client = new RestClient("http://maps.napr.gov.ge/NgmapExt/dwr/call/plaincall/JDwrQueryData.autocompliteCombo.dwr");
				client.Timeout = -1;
				var request = new RestRequest(Method.POST);
				request.AddHeader("Content-Type", "text/plain");
				request.AddHeader("Cookie", "JSESSIONID=699474C2861AF17251DFA95EBDE49E8F");
				request.AddParameter("text/plain", "callCount=1\r\nwindowName=c0-param0\r\nc0-scriptName=JDwrQueryData\r\nc0-methodName=autocompliteCombo\r\nc0-id=0\r\nc0-e1=string:" + sp + "\r\nc0-e2=string:42.640181640813%2C40.078241357707%2C45.342818359187%2C44.119300947392\r\nc0-param0=Object_Object:{comboValue:reference:c0-e1, mapExtent:reference:c0-e2}\r\nbatchId=17\r\ninstanceId=0\r\npage=%2F\r\nscriptSessionId=icwHq8S5yD2BJ*Skf5wsjTfJIwn/p2dJIwn-Cx1etctUo", ParameterType.RequestBody);
				IRestResponse response = client.Execute(request);
				//Console.WriteLine(response.Content);
				if (response.StatusCode == HttpStatusCode.OK)
				{
					coordinates = CustomReader.GetCoordinates(response.Content);
				}

				parameters.Add("@map_lat", coordinates.Item1);
				parameters.Add("@map_lon", coordinates.Item2);
				var res = db.QueryFirstOrDefault<double>("spGetDataByCoordinates", parameters, commandType: CommandType.StoredProcedure);

				return res;
			}
			catch (Exception ex)
			{

				throw ex;
			}
			finally
			{
				db.Close();
			}

			//return db.Query<T>(sp, parms, commandType: commandType).FirstOrDefault();

		}
		public T Insert<T>(IDbConnection db, string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
		{
			T result;
			//using IDbConnection db = new SqlConnection(_config.GetConnectionString(Connectionstring));
			try
			{
				//if (db.State == ConnectionState.Closed)
				//	db.Open();

				//using var tran = db.BeginTransaction();
				try
				{
					result = db.Query<T>(sp, parms, commandType: commandType).FirstOrDefault();
					//tran.Commit();
				}
				catch (Exception ex)
				{
					//tran.Rollback();
					throw ex;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				//if (db.State == ConnectionState.Open)
				//	db.Close();
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
