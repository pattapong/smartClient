using System;
using System.Xml.Serialization;

namespace smartRestaurant.ReserveService
{
	[XmlType(Namespace="http://ws.smartRestaurant.net")]
	public class TableReserve
	{
		public int reserveTableID;

		public int tableID;

		public DateTime reserveDate;

		public int seat;

		public int customerID;

		public string custFirstName;

		public string custMiddleName;

		public string custLastName;

		public TableReserve()
		{
		}
	}
}