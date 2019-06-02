using System;
using System.Xml.Serialization;

namespace smartRestaurant.MenuService
{
	[XmlType(Namespace="http://ws.smartRestaurant.net")]
	public class MenuDefault
	{
		public int MenuID;

		public int OptionID;

		public int DefaultChoiceID;

		public MenuDefault()
		{
		}
	}
}