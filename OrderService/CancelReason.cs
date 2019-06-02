using System;
using System.Xml.Serialization;

namespace smartRestaurant.OrderService
{
	[XmlType(Namespace="http://ws.smartRestaurant.net")]
	public class CancelReason
	{
		public int CancelReasonID;

		public string Reason;

		public CancelReason()
		{
		}
	}
}