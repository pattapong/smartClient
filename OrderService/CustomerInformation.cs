using System;
using System.Xml.Serialization;

namespace smartRestaurant.OrderService
{
	[XmlType(Namespace="http://ws.smartRestaurant.net")]
	public class CustomerInformation
	{
		public int CustID;

		public string FirstName;

		public string MiddleName;

		public string LastName;

		public string Telephone;

		public string Address;

		public string Description;

		public int RoadID;

		public string OtherRoadName;

		public CustomerInformation()
		{
		}
	}
}