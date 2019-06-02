using System;
using System.Xml.Serialization;

namespace smartRestaurant.TableService
{
	[XmlType(Namespace="http://ws.smartRestaurant.net")]
	public class TableInformation
	{
		public int TableID;

		public int NumberOfSeat;

		public string TableName;

		public TableInformation()
		{
		}
	}
}