using System;
using System.Xml.Serialization;

namespace smartRestaurant.PaymentService
{
	[XmlType(Namespace="http://ws.smartRestaurant.net")]
	public class PromotionCard
	{
		public int promotionID;

		public string description;

		public double amount;

		public PromotionCard()
		{
		}
	}
}