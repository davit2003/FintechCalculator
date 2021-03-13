using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FintechCalculator.DataModels
{
	public class Data
	{
		public List<Maklers> Maklers { get; set; }
		public List<Prs> Prs { get; set; }
		public Floors Floors { get; set; }
		public Users User { get; set; }
		public string Cnt { get; set; }
		public int? Page { get; set; }
		public bool? Filtered { get; set; }
		public MapData MapData { get; set; }
		public Districts Districts { get; set; }
		public string filter { get; set; }

	}
}
