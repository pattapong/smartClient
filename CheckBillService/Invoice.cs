using System;
using System.Xml.Serialization;

namespace smartRestaurant.CheckBillService
{
	[XmlType(Namespace="http://ws.smartRestaurant.net")]
	public class Invoice
	{
		public int invoiceID;

		public int paymentMethodID;

		public int orderBillID;

		public int point;

		public double totalPrice;

		public double tax1;

		public double tax2;

		public double totalDiscount;

		public double totalReceive;

		public int employeeID;

		public int refInvoice;

		public InvoiceDiscount[] discounts;

		public InvoicePayment[] payments;

		public string invoiceNote;

		public Invoice()
		{
		}
	}
}