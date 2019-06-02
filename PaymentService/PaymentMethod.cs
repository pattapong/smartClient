using System;
using System.Xml.Serialization;

namespace smartRestaurant.PaymentService
{
	[XmlType(Namespace="http://ws.smartRestaurant.net")]
	public class PaymentMethod
	{
		public int paymentMethodID;

		public string paymentMethodName;

		public PaymentMethod()
		{
		}
	}
}