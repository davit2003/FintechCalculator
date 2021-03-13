using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FintechCalculator.DataModels
{
	public class ReturnModel
	{
		public int StatusCode { get; set; }
		public string StatusMessage { get; set; }
		public Data Data { get; set; }


	}
}
