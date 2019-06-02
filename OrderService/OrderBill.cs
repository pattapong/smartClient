using System;
using System.Xml.Serialization;

namespace smartRestaurant.OrderService
{
	[XmlType(Namespace="http://ws.smartRestaurant.net")]
	public class OrderBill
	{
		public int OrderBillID;

		public int BillID;

		public int EmployeeID;

		public DateTime CloseBillDate;

		public OrderBillItem[] Items;

		public OrderBill()
		{
		}
	}
}