using System;
using System.Xml.Serialization;

namespace smartRestaurant.OrderService
{
	[XmlType(Namespace="http://ws.smartRestaurant.net")]
	public class OrderItemChoice
	{
		public int OptionID;

		public int ChoiceID;

		public OrderItemChoice()
		{
		}
	}
}