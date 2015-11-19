using System;
using System.Configuration;
using System.Globalization;

namespace smartRestaurant.Utils
{
	/// <summary>
	/// Summary description for AppParameter.
	/// </summary>
	public class AppParameter
	{
		// Fields
		private static CultureInfo culture = null;
		private static bool deliveryModeOnly;
		public const int LANG_ENGLISH = 0;
		public const int LANG_FRENCH = 2;
		public const int LANG_THAI = 1;
		private static bool loadedDeliveryModeOnly = false;
		private static bool loadedShowOrderItemPrice = false;
		private static bool loadedShowWaitingListButton = false;
		private static bool loadedWaitingListEnabled = false;
		private static int menuLanguage = -1;
		private static DateTime minDateTime;
		private static bool setMinDateTime = false;
		private static bool showOrderItemPrice;
		private static bool showWaitingListButton;
		private static bool waitingListEnabled;

		// Methods
		public static bool IsDemo()
		{
			string str = ConfigurationSettings.AppSettings["Demo"];
			return ((str != null) && (str == "1"));
		}

		// Properties
		public static CultureInfo Culture
		{
			get
			{
				if (culture == null)
				{
					culture = new CultureInfo(ConfigurationSettings.AppSettings["Culture"]);
				}
				return culture;
			}
		}

		public static bool DeliveryModeOnly
		{
			get
			{
				try
				{
					if (!loadedDeliveryModeOnly)
					{
						deliveryModeOnly = ConfigurationSettings.AppSettings["DeliveryModeOnly"].ToUpper() == "YES";
						loadedDeliveryModeOnly = true;
					}
					return deliveryModeOnly;
				}
				catch (Exception)
				{
					return true;
				}
			}
		}

		public static int MenuLanguage
		{
			get
			{
				if (menuLanguage < 0)
				{
					string str = ConfigurationSettings.AppSettings["MenuLanguage"];
					if (str != null)
					{
						str = str.ToUpper();
						if (str == "TH")
						{
							menuLanguage = 1;
						}
						else if (str == "FR")
						{
							menuLanguage = 2;
						}
						else
						{
							menuLanguage = 0;
						}
					}
					else
					{
						menuLanguage = 0;
					}
				}
				return menuLanguage;
			}
		}

		public static DateTime MinDateTime
		{
			get
			{
				if (!setMinDateTime)
				{
					minDateTime = new DateTime(0x76c, 1, 1, 0, 0, 0, 0);
				}
				return minDateTime;
			}
		}

		public static bool ShowOrderItemPrice
		{
			get
			{
				try
				{
					if (!loadedShowOrderItemPrice)
					{
						showOrderItemPrice = ConfigurationSettings.AppSettings["ShowOrderItemPrice"].ToUpper() == "YES";
						loadedShowOrderItemPrice = true;
					}
					return showOrderItemPrice;
				}
				catch (Exception)
				{
					return true;
				}
			}
		}

		public static bool ShowWaitingListButton
		{
			get
			{
				try
				{
					if (!loadedShowWaitingListButton)
					{
						showWaitingListButton = ConfigurationSettings.AppSettings["ShowWaitingListButton"].ToUpper() == "YES";
						loadedShowWaitingListButton = true;
					}
					return showWaitingListButton;
				}
				catch (Exception)
				{
					return true;
				}
			}
		}

		public static string TableListStyle
		{
			get
			{
				try
				{
					return ConfigurationSettings.AppSettings["TableListStyle"];
				}
				catch (Exception)
				{
					return "1";
				}
			}
		}

		public static string Tax1
		{
			get
			{
				string description = new smartRestaurant.CheckBillService.CheckBillService().GetDescription("TAX1");
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
				string description = new smartRestaurant.CheckBillService.CheckBillService().GetDescription("TAX2");
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
				try
				{
					if (!loadedWaitingListEnabled)
					{
						waitingListEnabled = ConfigurationSettings.AppSettings["WaitingListEnabled"].ToUpper() == "YES";
						loadedWaitingListEnabled = true;
					}
					return waitingListEnabled;
				}
				catch (Exception)
				{
					return true;
				}
			}
		}
	}


}
