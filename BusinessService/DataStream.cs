using System;
using System.Xml.Serialization;

namespace smartRestaurant.BusinessService
{
	[XmlType(Namespace="http://ws.smartRestaurant.net")]
	public class DataStream
	{
		public string[] Column;

		public DataStream()
		{
		}
	}
}