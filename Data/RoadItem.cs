using smartRestaurant.CustomerService;
using System;

namespace smartRestaurant.Data
{
	public class RoadItem
	{
		public int RoadID;

		public string RoadName;

		public string AreaName;

		public Road Object;

		public RoadItem()
		{
		}

		public override string ToString()
		{
			string[] roadName = new string[] { this.RoadName, " ", this.Object.RoadTypeName, "(", this.AreaName, ")" };
			return string.Concat(roadName);
		}
	}
}