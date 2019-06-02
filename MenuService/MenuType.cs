using System;
using System.Xml.Serialization;

namespace smartRestaurant.MenuService
{
	[XmlType(Namespace="http://ws.smartRestaurant.net")]
	public class MenuType
	{
		public int ID;

		public string Name;

		public string Description;

		public double Tax1;

		public double Tax2;

		public MenuItem[] MenuItems;

		public MenuType()
		{
		}
	}
}