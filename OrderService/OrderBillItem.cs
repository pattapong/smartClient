using System;
using System.Xml.Serialization;

namespace smartRestaurant.OrderService
{
	[XmlType(Namespace="http://ws.smartRestaurant.net")]
	public class OrderBillItem
	{
		public int BillDetailID;

		public int MenuID;

		public int Unit;

		public bool DefaultOption;

		public byte Status;

		public string Message;

		public DateTime ServeTime;

		public int CancelReasonID;

		public int EmployeeID;

		public bool ChangeFlag;

		public OrderItemChoice[] ItemChoices;

		public OrderBillItem()
		{
		}
	}
}