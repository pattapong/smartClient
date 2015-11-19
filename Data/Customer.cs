using System;
using System.Collections;
using smartRestaurant.CustomerService;

namespace smartRestaurant.Data
{
	/// <summary>
	/// Summary description for Customer.
	/// </summary>
	public class Customer
	{
		// Fields
		public static Road[] roads = null;

		// Methods
		public static Road GetRoad(int roadID)
		{
			if (roads == null)
			{
				roads = new smartRestaurant.CustomerService.CustomerService().GetRoads();
			}
			if (roads != null)
			{
				for (int i = 0; i < roads.Length; i++)
				{
					if (roads[i].RoadID == roadID)
					{
						return roads[i];
					}
				}
			}
			return null;
		}

		public static Road[] SearchRoad(string roadName)
		{
			if (roads == null)
			{
				roads = new smartRestaurant.CustomerService.CustomerService().GetRoads();
			}
			if (roads == null)
			{
				return null;
			}
			string str = roadName.ToUpper();
			ArrayList list = new ArrayList();
			for (int i = 0; i < roads.Length; i++)
			{
				if (roads[i].RoadName.ToUpper().IndexOf(str) == 0)
				{
					list.Add(roads[i]);
				}
			}
			return (Road[]) list.ToArray(typeof(Road));
		}

		// Properties
		public static Road[] Roads
		{
			get
			{
				if (roads == null)
				{
					roads = new smartRestaurant.CustomerService.CustomerService().GetRoads();
				}
				return roads;
			}
		}
	}


}
