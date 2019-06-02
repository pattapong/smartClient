using smartRestaurant.UserAuthorizeService;
using System;

namespace smartRestaurant.Data
{
	public class UserProfile
	{
		private static int MANAGER_TYPE_ID;

		private static int AUDITOR_TYPE_ID;

		private static int EMPLOYEE_TYPE_ID;

		private int userID;

		private string userName;

		private int empTypeID;

		public int EmployeeTypeID
		{
			get
			{
				return this.empTypeID;
			}
		}

		public int UserID
		{
			get
			{
				return this.userID;
			}
		}

		public string UserName
		{
			get
			{
				return this.userName;
			}
		}

		static UserProfile()
		{
			smartRestaurant.Data.UserProfile.MANAGER_TYPE_ID = 1;
			smartRestaurant.Data.UserProfile.AUDITOR_TYPE_ID = 2;
			smartRestaurant.Data.UserProfile.EMPLOYEE_TYPE_ID = 100;
		}

		public UserProfile(int userID, string userName, int empTypeID)
		{
			this.userID = userID;
			this.userName = userName;
			this.empTypeID = empTypeID;
		}

		public static smartRestaurant.Data.UserProfile CheckLogin(int userID, string password)
		{
			smartRestaurant.UserAuthorizeService.UserProfile userProfile = (new smartRestaurant.UserAuthorizeService.UserAuthorizeService()).CheckLogin(userID, password);
			if (userProfile == null)
			{
				return null;
			}
			return new smartRestaurant.Data.UserProfile(userID, userProfile.Name, userProfile.EmployeeTypeID);
		}

		public static void CheckLogout(int userID)
		{
			(new smartRestaurant.UserAuthorizeService.UserAuthorizeService()).CheckLogout(userID);
		}

		public bool IsAuditor()
		{
			return this.empTypeID == smartRestaurant.Data.UserProfile.AUDITOR_TYPE_ID;
		}

		public bool IsManager()
		{
			return this.empTypeID == smartRestaurant.Data.UserProfile.MANAGER_TYPE_ID;
		}
	}
}