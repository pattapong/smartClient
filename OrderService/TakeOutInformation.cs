using System;
using System.Xml.Serialization;

namespace smartRestaurant.OrderService
{
	[XmlType(Namespace="http://ws.smartRestaurant.net")]
	public class TakeOutInformation
	{
		public int OrderID;

		public DateTime OrderDate;

		public CustomerInformation CustInfo;

		public TakeOutInformation()
		{
		}
	}
}