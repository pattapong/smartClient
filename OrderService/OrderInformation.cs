using System;
using System.Xml.Serialization;

namespace smartRestaurant.OrderService
{
	[XmlType(Namespace="http://ws.smartRestaurant.net")]
	public class OrderInformation
	{
		public int OrderID;

		public DateTime OrderTime;

		public int TableID;

		public int EmployeeID;

		public int NumberOfGuest;

		public DateTime CloseOrderDate;

		public DateTime CreateDate;

		public OrderBill[] Bills;

		public OrderInformation()
		{
		}
	}
}