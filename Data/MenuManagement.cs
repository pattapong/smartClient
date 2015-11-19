using System;
using smartRestaurant.MenuService;
using smartRestaurant.Utils;

namespace smartRestaurant.Data
{
	/// <summary>
	/// Summary description for Menu.
	/// </summary>
	public class MenuManagement
	{
		// Fields
		private static MenuOption[] menuOptions;
		private static MenuType[] menuTypes;

		// Methods
		public static MenuItem GetMenuItemFromID(int id)
		{
			MenuItem menuItemFromID = null;
			for (int i = 0; i < menuTypes.Length; i++)
			{
				menuItemFromID = GetMenuItemFromID(menuTypes[i], id);
				if (menuItemFromID != null)
				{
					return menuItemFromID;
				}
			}
			return menuItemFromID;
		}

		public static MenuItem GetMenuItemFromID(MenuType type, int id)
		{
			if ((type.MenuItems != null) && (type.MenuItems.Length > 0))
			{
				for (int i = 0; i < type.MenuItems.Length; i++)
				{
					if (type.MenuItems[i].ID == id)
					{
						return type.MenuItems[i];
					}
				}
			}
			return null;
		}

		public static MenuItem GetMenuItemKeyID(int id)
		{
			MenuItem item = null;
			for (int i = 0; i < menuTypes.Length; i++)
			{
				if ((menuTypes[i].MenuItems != null) && (menuTypes[i].MenuItems.Length > 0))
				{
					for (int j = 0; j < menuTypes[i].MenuItems.Length; j++)
					{
						if (menuTypes[i].MenuItems[j].KeyID == id)
						{
							item = menuTypes[i].MenuItems[j];
							break;
						}
					}
				}
			}
			return item;
		}

		public static string GetMenuLanguageName(MenuItem menuItem)
		{
			return menuItem.Name;
		}

		public static MenuType GetMenuTypeFromID(int id)
		{
			for (int i = 0; i < menuTypes.Length; i++)
			{
				if (menuTypes[i].ID == id)
				{
					return menuTypes[i];
				}
			}
			return null;
		}

		public static OptionChoice GetOptionChoiceFromID(int id)
		{
			for (int i = 0; i < menuOptions.Length; i++)
			{
				for (int j = 0; j < menuOptions[i].OptionChoices.Length; j++)
				{
					if (menuOptions[i].OptionChoices[j].ChoiceID == id)
					{
						return menuOptions[i].OptionChoices[j];
					}
				}
			}
			return null;
		}

		public static void LoadMenus()
		{
			smartRestaurant.MenuService.MenuService service = new smartRestaurant.MenuService.MenuService();
			menuTypes = service.GetMenus("TOUCH");
			menuOptions = service.GetOptions("TOUCH");
		}

		// Properties
		public static MenuOption[] MenuOptions
		{
			get
			{
				return menuOptions;
			}
		}

		public static MenuType[] MenuTypes
		{
			get
			{
				return menuTypes;
			}
		}
	}
 

}
