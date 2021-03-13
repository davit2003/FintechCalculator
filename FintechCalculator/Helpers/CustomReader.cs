using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FintechCalculator.Helpers
{
	public static class CustomReader
	{
		public static Tuple <double, double> GetCoordinates(string data)
		{

			double coord_x = -1, coord_y = -1;

			string shape_str = "SHAPE_WKT";
			int shape_ind = 0;

			for (int i = 0; i < data.Length; i++)
			{
				char ch = data[i];

				if (ch == '[' && data[i + 1] == ']') goto ret_val;

				if (ch == shape_str[shape_ind])
				{
					shape_ind++;

					if (shape_ind == shape_str.Length)
					{
						i += 15;

						string str_x = "";
						string str_y = "";

						bool is_x = true;

						while (true)
						{
							if (data[i] == ',')
							{
								coord_y = Convert.ToDouble(str_y);

								break;
							}
							else if (data[i] == ' ')
							{
								coord_x = Convert.ToDouble(str_x);

								is_x = false;
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

						goto ret_val;
					}
				}
				else shape_ind = 0;
			}

			ret_val:  return new Tuple <double, double> (coord_x, coord_y);
		}
	}
}
