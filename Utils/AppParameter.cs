using smartRestaurant.CheckBillService;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;

namespace smartRestaurant.Utils
{
	public class AppParameter
	{
		public const int LANG_ENGLISH = 0;

		public const int LANG_THAI = 1;

		public const int LANG_FRENCH = 2;

		private static bool loadedWaitingListEnabled;

		private static bool loadedShowWaitingListButton;

		private static bool loadedDeliveryModeOnly;

		private static bool loadedShowOrderItemPrice;

		private static int menuLanguage;

		private static CultureInfo culture;

		private static bool waitingListEnabled;

		private static bool showWaitingListButton;

		private static bool deliveryModeOnly;

		private static bool showOrderItemPrice;

		private static DateTime minDateTime;

		private static bool setMinDateTime;

		public static CultureInfo Culture
		{
			get
			{
				if (AppParameter.culture == null)
				{
					AppParameter.culture = new CultureInfo(ConfigurationSettings.AppSettings["Culture"]);
				}
				return AppParameter.culture;
			}
		}

		public static bool DeliveryModeOnly
		{
			get
			{
				bool flag;
				try
				{
					if (!AppParameter.loadedDeliveryModeOnly)
					{
						AppParameter.deliveryModeOnly = ConfigurationSettings.AppSettings["DeliveryModeOnly"].ToUpper() == "YES";
						AppParameter.loadedDeliveryModeOnly = true;
					}
					flag = AppParameter.deliveryModeOnly;
				}
				catch (Exception exception)
				{
					flag = true;
				}
				return flag;
			}
		}

		public static int MenuLanguage
		{
			get
			{
				if (AppParameter.menuLanguage < 0)
				{
					string item = ConfigurationSettings.AppSettings["MenuLanguage"];
					if (item == null)
					{
						AppParameter.menuLanguage = 0;
					}
					else
					{
						item = item.ToUpper();
						if (item == "TH")
						{
							AppParameter.menuLanguage = 1;
						}
						else if (item != "FR")
						{
							AppParameter.menuLanguage = 0;
						}
						else
						{
							AppParameter.menuLanguage = 2;
						}
					}
				}
				return AppParameter.menuLanguage;
			}
		}

		public static DateTime MinDateTime
		{
			get
			{
				if (!AppParameter.setMinDateTime)
				{
					AppParameter.minDateTime = new DateTime(1900, 1, 1, 0, 0, 0, 0);
				}
				return AppParameter.minDateTime;
			}
		}

		public static bool ShowOrderItemPrice
		{
			get
			{
				bool flag;
				try
				{
					if (!AppParameter.loadedShowOrderItemPrice)
					{
						AppParameter.showOrderItemPrice = ConfigurationSettings.AppSettings["ShowOrderItemPrice"].ToUpper() == "YES";
						AppParameter.loadedShowOrderItemPrice = true;
					}
					flag = AppParameter.showOrderItemPrice;
				}
				catch (Exception exception)
				{
					flag = true;
				}
				return flag;
			}
		}

		public static bool ShowWaitingListButton
		{
			get
			{
				bool flag;
				try
				{
					if (!AppParameter.loadedShowWaitingListButton)
					{
						AppParameter.showWaitingListButton = ConfigurationSettings.AppSettings["ShowWaitingListButton"].ToUpper() == "YES";
						AppParameter.loadedShowWaitingListButton = true;
					}
					flag = AppParameter.showWaitingListButton;
				}
				catch (Exception exception)
				{
					flag = true;
				}
				return flag;
			}
		}

		public static string TableListStyle
		{
			get
			{
				string item;
				try
				{
					item = ConfigurationSettings.AppSettings["TableListStyle"];
				}
				catch (Exception exception)
				{
					item = "1";
				}
				return item;
			}
		}

		public static string Tax1
		{
			get
			{
				string description = (new smartRestaurant.CheckBillService.CheckBillService()).GetDescription("TAX1");
				if (description == null)
				{
					return "Tax1";
				}
				return description;
			}
		}

		public static string Tax2
		{
			get
			{
				string description = (new smartRestaurant.CheckBillService.CheckBillService()).GetDescription("TAX2");
				if (description == null)
				{
					return "Tax2";
				}
				return description;
			}
		}

		public static bool WaitingListEnabled
		{
			get
			{
				bool flag;
				try
				{
					if (!AppParameter.loadedWaitingListEnabled)
					{
						AppParameter.waitingListEnabled = ConfigurationSettings.AppSettings["WaitingListEnabled"].ToUpper() == "YES";
						AppParameter.loadedWaitingListEnabled = true;
					}
					flag = AppParameter.waitingListEnabled;
				}
				catch (Exception exception)
				{
					flag = true;
				}
				return flag;
			}
		}

		static AppParameter()
		{
			AppParameter.loadedWaitingListEnabled = false;
			AppParameter.loadedShowWaitingListButton = false;
			AppParameter.loadedDeliveryModeOnly = false;
			AppParameter.loadedShowOrderItemPrice = false;
			AppParameter.menuLanguage = -1;
			AppParameter.culture = null;
			AppParameter.setMinDateTime = false;
		}

		public AppParameter()
		{
		}

		public static bool IsDemo()
		{
			string item = ConfigurationSettings.AppSettings["Demo"];
			if (item == null)
			{
				return false;
			}
			return item == "1";
		}
	}
}