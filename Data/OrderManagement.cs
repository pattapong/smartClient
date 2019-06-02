using smartRestaurant.MenuService;
using smartRestaurant.OrderService;
using smartRestaurant.TableService;
using smartRestaurant.Utils;
using System;
using System.Text;

namespace smartRestaurant.Data
{
	public class OrderManagement
	{
		public OrderManagement()
		{
		}

		public static bool AddOrderBillItem(OrderBill selectedBill, OrderBillItem item)
		{
			if (selectedBill == null || selectedBill.CloseBillDate != AppParameter.MinDateTime)
			{
				return false;
			}
			OrderBillItem[] items = selectedBill.Items;
			if (items == null)
			{
				selectedBill.Items = new OrderBillItem[1];
			}
			else
			{
				selectedBill.Items = new OrderBillItem[(int)items.Length + 1];
				for (int i = 0; i < (int)items.Length; i++)
				{
					selectedBill.Items[i] = items[i];
				}
			}
			selectedBill.Items[(int)selectedBill.Items.Length - 1] = item;
			return true;
		}

		public static OrderBillItem AddOrderBillItem(OrderBill selectedBill, MenuItem menu, int employeeID)
		{
			OrderBillItem orderBillItem = new OrderBillItem()
			{
				MenuID = menu.ID,
				Unit = 1,
				DefaultOption = true,
				Status = 1,
				EmployeeID = employeeID,
				ServeTime = AppParameter.MinDateTime,
				ChangeFlag = true
			};
			if (OrderManagement.AddOrderBillItem(selectedBill, orderBillItem))
			{
				return orderBillItem;
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
			for (int i = 0; i < (int)orderInfo.Bills.Length; i++)
			{
				for (int j = 0; j < (int)orderInfo.Bills[i].Items.Length; j++)
				{
					if (orderInfo.Bills[i] != null && !(orderInfo.Bills[i].CloseBillDate != AppParameter.MinDateTime))
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
			if (selectedBill == null || selectedBill.CloseBillDate != AppParameter.MinDateTime)
			{
				return false;
			}
			if (item.BillDetailID != 0)
			{
				int num = CancelForm.Show("Select Cancel Reason");
				if (num < 0)
				{
					return false;
				}
				item.CancelReasonID = num;
				item.Status = 0;
				item.EmployeeID = employeeID;
				item.ChangeFlag = true;
			}
			else
			{
				OrderBillItem[] items = selectedBill.Items;
				if (items == null || (int)items.Length <= 1)
				{
					selectedBill.Items = null;
				}
				else
				{
					selectedBill.Items = new OrderBillItem[(int)items.Length - 1];
					int num1 = 0;
					for (int i = 0; i < (int)items.Length; i++)
					{
						if (items[i] != item)
						{
							selectedBill.Items[num1] = items[i];
							num1++;
						}
					}
				}
			}
			return true;
		}

		public static OrderInformation CreateOrderObject(OrderInformation orderInfo, int employeeID, TableInformation tableInfo, int guestNumber, int billNumber)
		{
			if (orderInfo != null)
			{
				int billID = 0;
				OrderBill[] bills = orderInfo.Bills;
				orderInfo.Bills = new OrderBill[billNumber];
				for (int i = 0; i < billNumber; i++)
				{
					if (i >= (int)bills.Length)
					{
						orderInfo.Bills[i] = new OrderBill();
						orderInfo.Bills[i].CloseBillDate = AppParameter.MinDateTime;
						orderInfo.Bills[i].EmployeeID = employeeID;
						int num = billID + 1;
						billID = num;
						orderInfo.Bills[i].BillID = num;
					}
					else
					{
						orderInfo.Bills[i] = bills[i];
						billID = orderInfo.Bills[i].BillID;
					}
				}
			}
			else
			{
				orderInfo = new OrderInformation()
				{
					OrderID = 0,
					OrderTime = DateTime.Now,
					TableID = tableInfo.TableID,
					EmployeeID = employeeID,
					NumberOfGuest = guestNumber,
					CloseOrderDate = AppParameter.MinDateTime,
					CreateDate = DateTime.Now,
					Bills = new OrderBill[billNumber]
				};
				for (int j = 0; j < billNumber; j++)
				{
					orderInfo.Bills[j] = new OrderBill();
					orderInfo.Bills[j].CloseBillDate = AppParameter.MinDateTime;
					orderInfo.Bills[j].EmployeeID = employeeID;
					orderInfo.Bills[j].BillID = j + 1;
				}
			}
			return orderInfo;
		}

		public static bool IsCancel(OrderBillItem item)
		{
			return item.Status == 0;
		}

		public static bool MoveOrderBillItem(OrderBill sourceBill, OrderBill destBill, OrderBillItem item)
		{
			if (sourceBill == null || sourceBill.CloseBillDate != AppParameter.MinDateTime || destBill == null || destBill.CloseBillDate != AppParameter.MinDateTime)
			{
				return false;
			}
			OrderBillItem[] items = destBill.Items;
			if (items == null)
			{
				destBill.Items = new OrderBillItem[1];
			}
			else
			{
				destBill.Items = new OrderBillItem[(int)items.Length + 1];
				for (int i = 0; i < (int)items.Length; i++)
				{
					destBill.Items[i] = items[i];
				}
			}
			destBill.Items[(int)destBill.Items.Length - 1] = item;
			items = sourceBill.Items;
			if (items == null || (int)items.Length <= 1)
			{
				sourceBill.Items = null;
			}
			else
			{
				sourceBill.Items = new OrderBillItem[(int)items.Length - 1];
				int num = 0;
				for (int j = 0; j < (int)items.Length; j++)
				{
					if (items[j] != item)
					{
						sourceBill.Items[num] = items[j];
						num++;
					}
				}
			}
			return true;
		}

		public static string OrderBillCountDisplayString(OrderBillItem item, ref double price)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(item.Unit);
			price = 0;
			if (AppParameter.ShowOrderItemPrice)
			{
				MenuItem menuItemFromID = MenuManagement.GetMenuItemFromID(item.MenuID);
				if (menuItemFromID != null)
				{
					price = menuItemFromID.Price * (double)item.Unit;
					stringBuilder.Append("\n");
					stringBuilder.Append(price.ToString("N"));
				}
			}
			return stringBuilder.ToString();
		}

		public static string OrderBillItemDisplayString(OrderBillItem item)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (item.ServeTime == AppParameter.MinDateTime)
			{
				stringBuilder.Append("[O] ");
			}
			else
			{
				stringBuilder.Append("[F] ");
			}
			MenuItem menuItemFromID = MenuManagement.GetMenuItemFromID(item.MenuID);
			if (menuItemFromID != null)
			{
				stringBuilder.Append(MenuManagement.GetMenuLanguageName(menuItemFromID));
			}
			else
			{
				stringBuilder.Append("Unknown");
			}
			if (item.Message != null && item.Message.Length > 0)
			{
				stringBuilder.Append("*");
			}
			if (!item.DefaultOption && item.ItemChoices != null)
			{
				int num = 0;
				for (int i = 0; i < (int)item.ItemChoices.Length; i++)
				{
					bool flag = false;
					for (int j = 0; j < (int)menuItemFromID.MenuDefaults.Length; j++)
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
							if (num != 0)
							{
								stringBuilder.Append("/");
							}
							else
							{
								stringBuilder.Append("\n-");
							}
							stringBuilder.Append(optionChoiceFromID.ChoiceName);
							num++;
						}
					}
				}
			}
			return stringBuilder.ToString();
		}

		public static bool UndoCancelOrderBillItem(OrderBillItem item, int employeeID)
		{
			if (item == null)
			{
				return false;
			}
			if (OrderManagement.IsCancel(item))
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