using System;
using System.Xml.Serialization;

namespace smartRestaurant.UserAuthorizeService
{
	[XmlType(Namespace="http://ws.smartRestaurant.net")]
	public class UserProfile
	{
		public string Name;

		public int EmployeeTypeID;

		public UserProfile()
		{
		}
	}
}