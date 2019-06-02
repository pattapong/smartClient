using System;
using System.Xml.Serialization;

namespace smartRestaurant.MenuService
{
	[XmlType(Namespace="http://ws.smartRestaurant.net")]
	public class MenuItem
	{
		public int ID;

		public int KeyID;

		public int TypeID;

		public string KeyIDText;

		public string Name;

		public string Description;

		public double Price;

		public MenuDefault[] MenuDefaults;

		public MenuItem()
		{
		}
	}
}