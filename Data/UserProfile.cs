using System;
using smartRestaurant.UserAuthorizeService;

namespace smartRestaurant.Data
{
	/// <summary>
	/// Summary description for UserProfile.
	/// </summary>
	public class UserProfile
	{
		// Fields
		private static int AUDITOR_TYPE_ID = 2;
		private static int EMPLOYEE_TYPE_ID = 100;
		private int empTypeID;
		private static int MANAGER_TYPE_ID = 1;
		private int userID;
		private string userName;

		// Methods
		public UserProfile(int userID, string userName, int empTypeID)
		{
			this.userID = userID;
			this.userName = userName;
			this.empTypeID = empTypeID;
		}

		public static UserProfile CheckLogin(int userID, string password)
		{
			UserAuthorizeService.UserAuthorizeService service = new UserAuthorizeService.UserAuthorizeService();
			UserAuthorizeService.UserProfile user = service.CheckLogin(userID, password);
			if (user == null)
				return null;
			return new UserProfile(userID, user.Name, user.EmployeeTypeID);
		}

		public static void CheckLogout(int userID)
		{
			new smartRestaurant.UserAuthorizeService.UserAuthorizeService().CheckLogout(userID);
		}

		public bool IsAuditor()
		{
			return (this.empTypeID == AUDITOR_TYPE_ID);
		}

		public bool IsManager()
		{
			return (this.empTypeID == MANAGER_TYPE_ID);
		}

		// Properties
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
	}

}
