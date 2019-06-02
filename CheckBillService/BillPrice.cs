using System;
using System.Xml.Serialization;

namespace smartRestaurant.CheckBillService
{
	[XmlType(Namespace="http://ws.smartRestaurant.net")]
	public class BillPrice
	{
		public double totalPrice;

		public double totalDiscount;

		public double totalTax1;

		public double totalTax2;

		public BillPrice()
		{
		}
	}
}