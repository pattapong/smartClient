using System;
using System.Xml.Serialization;

namespace smartRestaurant.CustomerService
{
	[XmlType(Namespace="http://ws.smartRestaurant.net")]
	public class Road
	{
		public int RoadID;

		public string RoadName;

		public string RoadTypeName;

		public string AreaName;

		public string AreaTypeName;

		public Road()
		{
		}
	}
}