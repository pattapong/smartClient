using System;
using System.Collections;
using System.Windows.Forms;
using smartRestaurant.UserAuthorizeService;
using smartRestaurant.CustomerService;


namespace smartRestaurant.Data
{
	/// <summary>
	/// Summary description for RoadItem.
	/// </summary>
	public class RoadItem
	{
		// Fields
		public string AreaName;
		public Road Object;
		public int RoadID;
		public string RoadName;

		// Methods
		public override string ToString()
		{
			return (this.RoadName + " " + this.Object.RoadTypeName + "(" + this.AreaName + ")");
		}
	}

 

}
