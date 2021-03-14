using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FintechCalculator.Helpers
{
	public static class CustomReader
	{
		private static string FIELD_NAME = "SHAPE_WKT";

		// note 1304

		private static int GetCoordsInd(string data)
        {
			int ind = 0;

			for (int i = 0; i < data.Length; i++)
			{
				char ch = data[i];

				if (ch == FIELD_NAME[ind])
				{
					ind++;

					if (ind == FIELD_NAME.Length)
					{
						return i + 14;
					}
				}
				else ind = 0;
			}

			return -1;
		}

		public static List <Tuple <double, double>> GetAllCoordinates(string data)
        {
			var list = new List <Tuple<double, double>> ();
			double coord_x = -1, coord_y = -1;
			int i = GetCoordsInd(data);

			if (i == -1) goto ret_val;

			string str_x = "";
			string str_y = "";

			bool is_x = true;

			while (data[i] != ')')
			{
				if (data[i] == ',')
				{
					coord_y = Convert.ToDouble(str_y);

					is_x = true;
					str_y = "";

					list.Add(new Tuple <double, double> (coord_x, coord_y));

					i++;
				}
				else if (data[i] == ' ')
				{
					coord_x = Convert.ToDouble(str_x);

					is_x = false;
					str_x = "";
				}
				else if (is_x)
				{
					str_x += data[i];
				}
				else
				{
					str_y += data[i];
				}

				i++;
			}

			ret_val: return list;
        }

		public static double CalcArea(string data)
        {
			List <Tuple<double, double>> coords = GetAllCoordinates(data);
			double area = 0;

			if (coords.Count < 3) goto ret_val;

			for (int i = 0; i < coords.Count; i++)
            {
				area += coords[i].Item1 * coords[(i + 1) % coords.Count].Item2 - coords[(i + 1) % coords.Count].Item1 * coords[i].Item2;
			}

			area = Math.Abs(area) * .5;

			ret_val: return area;
		}

		public static Tuple <double, double> GetCoordinates(string data)
		{
			List<Tuple<double, double>> coords = GetAllCoordinates(data);
			double coord_x = -1, coord_y = -1;

			if (coords.Count == 0) goto ret_val;

			coord_x = coords[0].Item1;
			coord_y = coords[0].Item2;

			ret_val:  return new Tuple <double, double> (coord_x, coord_y);
		}
	}
}
