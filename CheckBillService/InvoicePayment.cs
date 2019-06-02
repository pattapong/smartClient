using System;
using System.Xml.Serialization;

namespace smartRestaurant.CheckBillService
{
	[XmlType(Namespace="http://ws.smartRestaurant.net")]
	public class InvoicePayment
	{
		public int paymentMethodID;

		public double receive;

		public InvoicePayment()
		{
		}
	}
}