using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FintechCalculator.DataModels
{
	public class MapData
	{

		public string CadastralCode { get; set; }
		public int? ProductTypeId { get; set; }

		public int? IsCanalisation { get; set; }
		public int? IsRoad { get; set; }
		public int? IsWater { get; set; }
		public int? IsGas { get; set; }
		public int? Bedrooms { get; set; }
		public int? Floor { get; set; }
		public int? ParkingId { get; set; }
		public int? Electricity { get; set; }
		public int? Rooms{ get; set; }


	}
}
