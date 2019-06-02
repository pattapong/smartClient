using System;
using System.Xml.Serialization;

namespace smartRestaurant.PaymentService
{
	[XmlType(Namespace="http://ws.smartRestaurant.net")]
	public class Discount
	{
		public int promotionID;

		public int promotionType;

		public string description;

		public double amount;

		public double discountPercent;

		public double discountAmount;

		public Discount()
		{
		}
	}
}