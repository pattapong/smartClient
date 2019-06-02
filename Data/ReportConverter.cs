using smartRestaurant.BusinessService;
using System;
using System.Data;

namespace smartRestaurant.Data
{
	public class ReportConverter
	{
		public ReportConverter()
		{
		}

		public static DataTable Convert(DataStream[] dStream)
		{
			if (dStream == null)
			{
				return null;
			}
			DataTable dataTable = new DataTable();
			DataRow column = null;
			for (int i = 0; i < (int)dStream.Length; i++)
			{
				for (int j = 0; j < (int)dStream[i].Column.Length; j++)
				{
					if (i != 0)
					{
						if (j == 0)
						{
							column = dataTable.NewRow();
							dataTable.Rows.Add(column);
						}
						column[j] = dStream[i].Column[j];
					}
					else
					{
						dataTable.Columns.Add(dStream[i].Column[j], typeof(string));
					}
				}
			}
			return dataTable;
		}
	}
}