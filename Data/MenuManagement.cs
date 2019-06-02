using smartRestaurant.MenuService;
using System;

namespace smartRestaurant.Data
{
	public class MenuManagement
	{
		private static MenuType[] menuTypes;

		private static MenuOption[] menuOptions;

		public static MenuOption[] MenuOptions
		{
			get
			{
				return MenuManagement.menuOptions;
			}
		}

		public static MenuType[] MenuTypes
		{
			get
			{
				return MenuManagement.menuTypes;
			}
		}

		public MenuManagement()
		{
		}

		public static MenuItem GetMenuItemFromID(MenuType type, int id)
		{
			MenuItem menuItems = null;
			if (type.MenuItems != null && (int)type.MenuItems.Length > 0)
			{
				int num = 0;
				while (num < (int)type.MenuItems.Length)
				{
					if (type.MenuItems[num].ID != id)
					{
						num++;
					}
					else
					{
						menuItems = type.MenuItems[num];
						break;
					}
				}
			}
			return menuItems;
		}

		public static MenuItem GetMenuItemFromID(int id)
		{
			MenuItem menuItemFromID = null;
			for (int i = 0; i < (int)MenuManagement.menuTypes.Length; i++)
			{
				menuItemFromID = MenuManagement.GetMenuItemFromID(MenuManagement.menuTypes[i], id);
				if (menuItemFromID != null)
				{
					break;
				}
			}
			return menuItemFromID;
		}

		public static MenuItem GetMenuItemKeyID(int id)
		{
			MenuItem menuItems = null;
			for (int i = 0; i < (int)MenuManagement.menuTypes.Length; i++)
			{
				if (MenuManagement.menuTypes[i].MenuItems != null && (int)MenuManagement.menuTypes[i].MenuItems.Length > 0)
				{
					int num = 0;
					while (num < (int)MenuManagement.menuTypes[i].MenuItems.Length)
					{
						if (MenuManagement.menuTypes[i].MenuItems[num].KeyID != id)
						{
							num++;
						}
						else
						{
							menuItems = MenuManagement.menuTypes[i].MenuItems[num];
							break;
						}
					}
				}
			}
			return menuItems;
		}

		public static string GetMenuLanguageName(MenuItem menuItem)
		{
			return menuItem.Name;
		}

		public static MenuType GetMenuTypeFromID(int id)
		{
			MenuType menuType = null;
			int num = 0;
			while (num < (int)MenuManagement.menuTypes.Length)
			{
				if (MenuManagement.menuTypes[num].ID != id)
				{
					num++;
				}
				else
				{
					menuType = MenuManagement.menuTypes[num];
					break;
				}
			}
			return menuType;
		}

		public static OptionChoice GetOptionChoiceFromID(int id)
		{
			for (int i = 0; i < (int)MenuManagement.menuOptions.Length; i++)
			{
				for (int j = 0; j < (int)MenuManagement.menuOptions[i].OptionChoices.Length; j++)
				{
					if (MenuManagement.menuOptions[i].OptionChoices[j].ChoiceID == id)
					{
						return MenuManagement.menuOptions[i].OptionChoices[j];
					}
				}
			}
			return null;
		}

		public static void LoadMenus()
		{
			smartRestaurant.MenuService.MenuService menuService = new smartRestaurant.MenuService.MenuService();
			MenuManagement.menuTypes = menuService.GetMenus("TOUCH");
			MenuManagement.menuOptions = menuService.GetOptions("TOUCH");
		}
	}
}