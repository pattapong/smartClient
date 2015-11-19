using System;
using System.Data;
using smartRestaurant.BusinessService;

namespace smartRestaurant.Data
{
	/// <summary>
	/// Summary description for ReportConverter.
	/// </summary>
	public class ReportConverter
	{
		// Methods
		public static DataTable Convert(DataStream[] dStream)
		{
			if (dStream == null)
			{
				return null;
			}
			DataTable table = new DataTable();
			DataRow row = null;
			for (int i = 0; i < dStream.Length; i++)
			{
				for (int j = 0; j < dStream[i].Column.Length; j++)
				{
					if (i == 0)
					{
						table.Columns.Add(dStream[i].Column[j], typeof(string));
					}
					else
					{
						if (j == 0)
						{
							row = table.NewRow();
							table.Rows.Add(row);
						}
						row[j] = dStream[i].Column[j];
					}
				}
			}
			return table;
		}
	}

 

}
