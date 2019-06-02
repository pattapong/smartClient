using smartRestaurant.CustomerService;
using System;
using System.Collections;

namespace smartRestaurant.Data
{
	public class Customer
	{
		public static Road[] roads;

		public static Road[] Roads
		{
			get
			{
				if (Customer.roads == null)
				{
					Customer.roads = (new smartRestaurant.CustomerService.CustomerService()).GetRoads();
				}
				return Customer.roads;
			}
		}

		static Customer()
		{
			Customer.roads = null;
		}

		public Customer()
		{
		}

		public static Road GetRoad(int roadID)
		{
			if (Customer.roads == null)
			{
				Customer.roads = (new smartRestaurant.CustomerService.CustomerService()).GetRoads();
			}
			if (Customer.roads == null)
			{
				return null;
			}
			for (int i = 0; i < (int)Customer.roads.Length; i++)
			{
				if (Customer.roads[i].RoadID == roadID)
				{
					return Customer.roads[i];
				}
			}
			return null;
		}

		public static Road[] SearchRoad(string roadName)
		{
			if (Customer.roads == null)
			{
				Customer.roads = (new smartRestaurant.CustomerService.CustomerService()).GetRoads();
			}
			if (Customer.roads == null)
			{
				return null;
			}
			string upper = roadName.ToUpper();
			ArrayList arrayLists = new ArrayList();
			for (int i = 0; i < (int)Customer.roads.Length; i++)
			{
				if (Customer.roads[i].RoadName.ToUpper().IndexOf(upper) == 0)
				{
					arrayLists.Add(Customer.roads[i]);
				}
			}
			return (Road[])arrayLists.ToArray(typeof(Road));
		}
	}
}