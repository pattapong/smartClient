using System;
using System.Xml.Serialization;

namespace smartRestaurant.MenuService
{
	[XmlType(Namespace="http://ws.smartRestaurant.net")]
	public class MenuOption
	{
		public int OptionID;

		public string OptionName;

		public OptionChoice[] OptionChoices;

		public MenuOption()
		{
		}
	}
}