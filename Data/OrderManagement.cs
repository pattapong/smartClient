using System;
using System.Text;
using smartRestaurant.MenuService;
using smartRestaurant.OrderService;
using smartRestaurant.TableService;
using smartRestaurant.Utils;

namespace smartRestaurant.Data
{
	/// <summary>
	/// Summary description for OrderManagement.
	/// </summary>
	public class OrderManagement
	{
		// Methods
		public static bool AddOrderBillItem(OrderBill selectedBill, OrderBillItem item)
		{
			if ((selectedBill == null) || (selectedBill.CloseBillDate != AppParameter.MinDateTime))
			{
				return false;
			}
			OrderBillItem[] items = selectedBill.Items;
			if (items != null)
			{
				selectedBill.Items = new OrderBillItem[items.Length + 1];
				for (int i = 0; i < items.Length; i++)
				{
					selectedBill.Items[i] = items[i];
				}
			}
			else
			{
				selectedBill.Items = new OrderBillItem[1];
			}
			selectedBill.Items[selectedBill.Items.Length - 1] = item;
			return true;
		}

		public static OrderBillItem AddOrderBillItem(OrderBill selectedBill, MenuItem menu, int employeeID)
		{
			OrderBillItem item = new OrderBillItem();
			item.MenuID = menu.ID;
			item.Unit = 1;
			item.DefaultOption = true;
			item.Status = 1;
			item.EmployeeID = employeeID;
			item.ServeTime = AppParameter.MinDateTime;
			item.ChangeFlag = true;
			if (AddOrderBillItem(selectedBill, item))
			{
				return item;
			}
			return null;
		}

		public static bool CancelOrder(OrderInformation orderInfo, int employeeID)
		{
			if (orderInfo == null)
			{
				return false;
			}
			int num = CancelForm.Show("Select Cancel Reason");
			if (num < 0)
			{
				return false;
			}
			for (int i = 0; i < orderInfo.Bills.Length; i++)
			{
				for (int j = 0; j < orderInfo.Bills[i].Items.Length; j++)
				{
					if ((orderInfo.Bills[i] != null) && !(orderInfo.Bills[i].CloseBillDate != AppParameter.MinDateTime))
					{
						orderInfo.Bills[i].Items[j].CancelReasonID = num;
						orderInfo.Bills[i].Items[j].Status = 0;
						orderInfo.Bills[i].Items[j].EmployeeID = employeeID;
						orderInfo.Bills[i].Items[j].ChangeFlag = true;
					}
				}
				orderInfo.Bills[i].CloseBillDate = DateTime.Now;
			}
			orderInfo.CloseOrderDate = DateTime.Now;
			return true;
		}

		public static bool CancelOrderBillItem(OrderBill selectedBill, OrderBillItem item, int employeeID)
		{
			if ((selectedBill == null) || (selectedBill.CloseBillDate != AppParameter.MinDateTime))
			{
				return false;
			}
			if (item.BillDetailID == 0)
			{
				OrderBillItem[] items = selectedBill.Items;
				if ((items != null) && (items.Length > 1))
				{
					selectedBill.Items = new OrderBillItem[items.Length - 1];
					int index = 0;
					for (int i = 0; i < items.Length; i++)
					{
						if (items[i] != item)
						{
							selectedBill.Items[index] = items[i];
							index++;
						}
					}
				}
				else
				{
					selectedBill.Items = null;
				}
			}
			else
			{
				int num3 = CancelForm.Show("Select Cancel Reason");
				if (num3 < 0)
				{
					return false;
				}
				item.CancelReasonID = num3;
				item.Status = 0;
				item.EmployeeID = employeeID;
				item.ChangeFlag = true;
			}
			return true;
		}

		public static OrderInformation CreateOrderObject(OrderInformation orderInfo, int employeeID, TableInformation tableInfo, int guestNumber, int billNumber)
		{
			if (orderInfo == null)
			{
				orderInfo = new OrderInformation();
				orderInfo.OrderID = 0;
				orderInfo.OrderTime = DateTime.Now;
				orderInfo.TableID = tableInfo.TableID;
				orderInfo.EmployeeID = employeeID;
				orderInfo.NumberOfGuest = guestNumber;
				orderInfo.CloseOrderDate = AppParameter.MinDateTime;
				orderInfo.CreateDate = DateTime.Now;
				orderInfo.Bills = new OrderBill[billNumber];
				for (int j = 0; j < billNumber; j++)
				{
					orderInfo.Bills[j] = new OrderBill();
					orderInfo.Bills[j].CloseBillDate = AppParameter.MinDateTime;
					orderInfo.Bills[j].EmployeeID = employeeID;
					orderInfo.Bills[j].BillID = j + 1;
				}
				return orderInfo;
			}
			int billID = 0;
			OrderBill[] bills = orderInfo.Bills;
			orderInfo.Bills = new OrderBill[billNumber];
			for (int i = 0; i < billNumber; i++)
			{
				if (i < bills.Length)
				{
					orderInfo.Bills[i] = bills[i];
					billID = orderInfo.Bills[i].BillID;
				}
				else
				{
					orderInfo.Bills[i] = new OrderBill();
					orderInfo.Bills[i].CloseBillDate = AppParameter.MinDateTime;
					orderInfo.Bills[i].EmployeeID = employeeID;
					orderInfo.Bills[i].BillID = ++billID;
				}
			}
			return orderInfo;
		}

		public static bool IsCancel(OrderBillItem item)
		{
			return (item.Status == 0);
		}

		public static bool MoveOrderBillItem(OrderBill sourceBill, OrderBill destBill, OrderBillItem item)
		{
			if (((sourceBill == null) || (sourceBill.CloseBillDate != AppParameter.MinDateTime)) || ((destBill == null) || (destBill.CloseBillDate != AppParameter.MinDateTime)))
			{
				return false;
			}
			OrderBillItem[] items = destBill.Items;
			if (items != null)
			{
				destBill.Items = new OrderBillItem[items.Length + 1];
				for (int i = 0; i < items.Length; i++)
				{
					destBill.Items[i] = items[i];
				}
			}
			else
			{
				destBill.Items = new OrderBillItem[1];
			}
			destBill.Items[destBill.Items.Length - 1] = item;
			items = sourceBill.Items;
			if ((items != null) && (items.Length > 1))
			{
				sourceBill.Items = new OrderBillItem[items.Length - 1];
				int index = 0;
				for (int j = 0; j < items.Length; j++)
				{
					if (items[j] != item)
					{
						sourceBill.Items[index] = items[j];
						index++;
					}
				}
			}
			else
			{
				sourceBill.Items = null;
			}
			return true;
		}

		public static string OrderBillCountDisplayString(OrderBillItem item, ref double price)
		{
			StringBuilder builder = new StringBuilder();
			builder.Append(item.Unit);
			price = 0.0;
			if (AppParameter.ShowOrderItemPrice)
			{
				MenuItem menuItemFromID = MenuManagement.GetMenuItemFromID(item.MenuID);
				if (menuItemFromID != null)
				{
					price = menuItemFromID.Price * item.Unit;
					builder.Append("\n");
					builder.Append(((double) price).ToString("N"));
				}
			}
			return builder.ToString();
		}

		public static string OrderBillItemDisplayString(OrderBillItem item)
		{
			StringBuilder builder = new StringBuilder();
			if (item.ServeTime != AppParameter.MinDateTime)
			{
				builder.Append("[F] ");
			}
			else
			{
				builder.Append("[O] ");
			}
			MenuItem menuItemFromID = MenuManagement.GetMenuItemFromID(item.MenuID);
			if (menuItemFromID == null)
			{
				builder.Append("Unknown");
			}
			else
			{
				builder.Append(MenuManagement.GetMenuLanguageName(menuItemFromID));
			}
			if ((item.Message != null) && (item.Message.Length > 0))
			{
				builder.Append("*");
			}
			if (!item.DefaultOption && (item.ItemChoices != null))
			{
				int num = 0;
				for (int i = 0; i < item.ItemChoices.Length; i++)
				{
					bool flag = false;
					for (int j = 0; j < menuItemFromID.MenuDefaults.Length; j++)
					{
						if (menuItemFromID.MenuDefaults[j].DefaultChoiceID == item.ItemChoices[i].ChoiceID)
						{
							flag = true;
						}
					}
					if (!flag)
					{
						OptionChoice optionChoiceFromID = MenuManagement.GetOptionChoiceFromID(item.ItemChoices[i].ChoiceID);
						if (optionChoiceFromID != null)
						{
							if (num == 0)
							{
								builder.Append("\n-");
							}
							else
							{
								builder.Append("/");
							}
							builder.Append(optionChoiceFromID.ChoiceName);
							num++;
						}
					}
				}
			}
			return builder.ToString();
		}

		public static bool UndoCancelOrderBillItem(OrderBillItem item, int employeeID)
		{
			if (item == null)
			{
				return false;
			}
			if (IsCancel(item))
			{
				item.Status = 1;
				item.CancelReasonID = 0;
				item.EmployeeID = employeeID;
				item.ChangeFlag = true;
			}
			return true;
		}
	}

 

}
