using System;
using System.Xml.Serialization;

namespace smartRestaurant.OrderService
{
	[XmlType(Namespace="http://ws.smartRestaurant.net")]
	public class OrderWaiting
	{
		public int OrderID;

		public string TableName;

		public OrderBillItemWaiting[] Items;

		public OrderWaiting()
		{
		}
	}
}