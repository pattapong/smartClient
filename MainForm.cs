using smartRestaurant.Business;
using smartRestaurant.Data;
using smartRestaurant.OrderService;
using smartRestaurant.TableService;
using smartRestaurant.Utils;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace smartRestaurant
{
	public class MainForm : Form
	{
		private System.ComponentModel.Container components = null;

		private static MainForm instance;

		private UserProfile userProfile;

		private bool isFromTakeOut;

		private LoginForm loginForm;

		private MainMenuForm mainMenuForm;

		private TakeOrderForm takeOrderForm;

		private PrintReceiptForm printReceiptForm;

		private TakeOutForm takeOutForm;

		private ReserveForm reserveForm;

		private SalesForm salesForm;

		public static MainForm Instance
		{
			get
			{
				return MainForm.instance;
			}
		}

		public UserProfile User
		{
			get
			{
				return this.userProfile;
			}
			set
			{
				this.userProfile = value;
			}
		}

		public string UserID
		{
			get
			{
				if (this.userProfile == null)
				{
					return "";
				}
				return this.userProfile.UserID.ToString();
			}
		}

		public MainForm()
		{
			this.InitializeComponent();
			MainForm.instance = this;
			this.isFromTakeOut = false;
			this.InitializeForm();
			this.ShowLoginForm();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		public void Exit()
		{
			if (base.MdiChildren != null)
			{
				for (int i = 0; i < (int)base.MdiChildren.Length; i++)
				{
					base.MdiChildren[i].Close();
				}
			}
			base.Close();
		}

		private void InitializeComponent()
		{
			base.AutoScale = false;
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 16);
			base.ClientSize = new System.Drawing.Size(1024, 768);
			base.ControlBox = false;
			this.Font = new System.Drawing.Font("Tahoma", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 222);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			base.IsMdiContainer = true;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "MainForm";
			base.StartPosition = FormStartPosition.Manual;
			this.Text = "smartRestaurant";
		}

		private void InitializeForm()
		{
			this.Cursor = Cursors.WaitCursor;
			this.loginForm = new LoginForm()
			{
				MdiParent = this
			};
			this.mainMenuForm = new MainMenuForm()
			{
				MdiParent = this
			};
			this.takeOrderForm = new TakeOrderForm()
			{
				MdiParent = this
			};
			this.printReceiptForm = new PrintReceiptForm()
			{
				MdiParent = this
			};
			this.takeOutForm = new TakeOutForm()
			{
				MdiParent = this
			};
			this.reserveForm = new ReserveForm()
			{
				MdiParent = this
			};
			this.salesForm = new SalesForm()
			{
				MdiParent = this
			};
			this.Cursor = Cursors.Default;
		}

		[STAThread]
		private static void Main()
		{
			Application.Run(new MainForm());
		}

		public void ShowLoginForm()
		{
			this.loginForm.Show();
			this.loginForm.BringToFront();
			this.loginForm.UpdateForm();
		}

		public void ShowMainMenuForm(bool ignoreTakeOut)
		{
			if (!ignoreTakeOut && (AppParameter.DeliveryModeOnly || this.isFromTakeOut))
			{
				this.ShowTakeOutForm(null, 0, null, false);
				return;
			}
			this.isFromTakeOut = false;
			this.mainMenuForm.Show();
			this.mainMenuForm.BringToFront();
			this.mainMenuForm.UpdateForm();
		}

		public void ShowMainMenuForm()
		{
			this.ShowMainMenuForm(false);
		}

		public void ShowPrintReceiptForm(TableInformation table, OrderInformation order, OrderBill orderBill)
		{
			if (table == null || order == null || orderBill == null)
			{
				return;
			}
			this.printReceiptForm.Table = table;
			this.printReceiptForm.Order = order;
			this.printReceiptForm.OrderBill = orderBill;
			if (this.userProfile != null)
			{
				this.printReceiptForm.EmployeeID = this.userProfile.UserID;
			}
			this.printReceiptForm.Show();
			this.printReceiptForm.BringToFront();
			this.printReceiptForm.UpdateForm();
		}

		public void ShowReserveForm()
		{
			this.reserveForm.Show();
			this.reserveForm.BringToFront();
			this.reserveForm.UpdateForm();
		}

		public void ShowSalesForm()
		{
			this.salesForm.Show();
			this.salesForm.BringToFront();
			this.salesForm.UpdateForm();
		}

		public void ShowTakeOrderForm(TableInformation table)
		{
			if (table == null)
			{
				this.takeOrderForm.TakeOrderResume = true;
			}
			else
			{
				if ((new smartRestaurant.TableService.TableService()).UpdateTableLockInuse(table.TableID, true) <= 0 && !this.User.IsManager())
				{
					MessageForm.Show("Table locked", "This table locked from other client.");
					this.ShowMainMenuForm();
					return;
				}
				this.takeOrderForm.Table = table;
				this.takeOrderForm.OrderID = 0;
				this.takeOrderForm.TakeOrderResume = false;
			}
			if (this.userProfile != null)
			{
				this.takeOrderForm.EmployeeID = this.userProfile.UserID;
			}
			this.takeOrderForm.Show();
			this.takeOrderForm.BringToFront();
			this.takeOrderForm.UpdateForm();
		}

		public void ShowTakeOrderForm(TableInformation table, int orderID, int custID, string custName)
		{
			this.takeOrderForm.Table = table;
			this.takeOrderForm.OrderID = orderID;
			this.takeOrderForm.CustomerID = custID;
			this.takeOrderForm.CustomerName = custName;
			this.takeOrderForm.TakeOrderResume = false;
			if (this.userProfile != null)
			{
				this.takeOrderForm.EmployeeID = this.userProfile.UserID;
			}
			this.takeOrderForm.Show();
			this.takeOrderForm.BringToFront();
			this.takeOrderForm.UpdateForm();
		}

		public void ShowTakeOrderForm(int custID, string custName)
		{
			if (custName != null)
			{
				this.takeOrderForm.CustomerID = custID;
				this.takeOrderForm.CustomerName = custName;
			}
			this.takeOrderForm.TakeOrderResume = true;
			if (this.userProfile != null)
			{
				this.takeOrderForm.EmployeeID = this.userProfile.UserID;
			}
			this.takeOrderForm.Show();
			this.takeOrderForm.BringToFront();
			this.takeOrderForm.UpdateForm();
		}

		public void ShowTakeOutForm(TableInformation table, int custID, string custName, bool takeOrder)
		{
			this.isFromTakeOut = true;
			this.takeOutForm.Table = table;
			this.takeOutForm.TakeOrderMode = takeOrder;
			this.takeOutForm.CustomerName = custName;
			if (this.userProfile != null)
			{
				this.takeOutForm.EmployeeID = this.userProfile.UserID;
			}
			this.takeOutForm.Show();
			this.takeOutForm.BringToFront();
			this.takeOutForm.UpdateForm();
		}
	}
}