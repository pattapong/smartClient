using System;
using System.Xml.Serialization;

namespace smartRestaurant.MenuService
{
	[XmlType(Namespace="http://ws.smartRestaurant.net")]
	public class OptionChoice
	{
		public int OptionID;

		public int ChoiceID;

		public string ChoiceName;

		public OptionChoice()
		{
		}
	}
}