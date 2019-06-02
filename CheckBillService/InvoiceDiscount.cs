using System;
using System.Xml.Serialization;

namespace smartRestaurant.CheckBillService
{
	[XmlType(Namespace="http://ws.smartRestaurant.net")]
	public class InvoiceDiscount
	{
		public int invoiceDiscountID;

		public int invoiceID;

		public int promotionID;

		public InvoiceDiscount()
		{
		}
	}
}