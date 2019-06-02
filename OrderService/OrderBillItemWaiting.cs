using System;
using System.Xml.Serialization;

namespace smartRestaurant.OrderService
{
	[XmlType(Namespace="http://ws.smartRestaurant.net")]
	public class OrderBillItemWaiting
	{
		public int BillDetailID;

		public string MenuKeyID;

		public int Unit;

		public string Choice;

		public OrderBillItemWaiting()
		{
		}
	}
}