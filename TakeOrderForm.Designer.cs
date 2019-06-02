using smartRestaurant.Controls;
using smartRestaurant.Data;
using smartRestaurant.MenuService;
using smartRestaurant.OrderService;
using smartRestaurant.TableService;
using smartRestaurant.Utils;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Resources;
using System.Text;
using System.Windows.Forms;

namespace smartRestaurant
{
	public class TakeOrderForm : SmartForm
	{
		private const int INPUT_MENU = 0;

		private const int INPUT_UNIT = 1;

		private const int INPUT_OPTION = 2;

		private const int INPUT_GUEST = 10;

		private const int INPUT_BILL = 11;

		private const int INPUT_MOVEITEM = 20;

		private static string FIELD_CUST_TEXT;

		private int inputState;

		private string inputValue;

		private int guestNumber;

		private int billNumber;

		private int employeeID;

		private bool isChanged;

		private bool moveItem;

		private int takeOutOrderID;

		private bool takeOutMode;

		private int takeOutCustID;

		private bool takeOrderResume;

		private TableInformation tableInfo;

		private int[] tableIDList;

		private MenuOption[] menuOptions;

		private MenuType[] menuTypes;

		private MenuType selectedType;

		private OrderInformation orderInfo;

		private OrderBill selectedBill;

		private OrderBillItem selectedItem;

		private smartRestaurant.TableService.TableService tabService = new smartRestaurant.TableService.TableService();

		private GroupPanel OrderPanel;

		private Label FieldBill;

		private Label LblBill;

		private Label FieldGuest;

		private Label LblGuest;

		private Label FieldTable;

		private Label LblTable;

		private ImageList ButtonImgList;

		private ImageButton BtnMain;

		private ImageList NumberImgList;

		private ImageButton BtnCancel;

		private ImageButton BtnUndo;

		private ImageButton BtnMessage;

		private ImageButton BtnPrintKitchen;

		private GroupPanel groupPanel2;

		private GroupPanel groupPanel3;

		private ButtonListPad CategoryPad;

		private ButtonListPad OptionPad;

		private IContainer components;

		private NumberPad NumberKeyPad;

		private Label FieldInputType;

		private Label FieldCurrentInput;

		private ImageButton BtnDown;

		private ImageButton BtnUp;

		private ItemsList ListOrderItem;

		private ImageButton BtnPrintReceiptAll;

		private ImageButton BtnPrintReceipt;

		private ImageButton BtnMoveItem;

		private ImageButton BtnSearch;

		private Label FieldCustName;

		private GroupPanel PanCustName;

		private ImageButton BtnServeItem;

		private ItemsList ListOrderItemBy;

		private Label LblPageID;

		private ImageButton BtnAmount;

		private Label LblCopyright;

		private Label LblTotalText;

		private Label LblTotalValue;

		private ItemsList ListOrderCount;

		public int CustomerID
		{
			set
			{
				this.takeOutCustID = value;
			}
		}

		public string CustomerName
		{
			set
			{
				this.FieldCustName.Text = value;
			}
		}

		public int EmployeeID
		{
			set
			{
				this.employeeID = value;
			}
		}

		public int OrderID
		{
			set
			{
				this.takeOutOrderID = value;
			}
		}

		public TableInformation Table
		{
			set
			{
				if (value != null)
				{
					this.tableInfo = value;
					this.guestNumber = value.NumberOfSeat;
					this.billNumber = value.NumberOfSeat;
				}
			}
		}

		public bool TakeOrderResume
		{
			set
			{
				this.takeOrderResume = value;
			}
		}

		static TakeOrderForm()
		{
			TakeOrderForm.FIELD_CUST_TEXT = "- Please input name -";
		}

		public TakeOrderForm()
		{
			this.InitializeComponent();
			this.employeeID = 1;
			this.takeOutMode = false;
			this.LoadMenus();
			this.SetCategory();
		}

		private void AddOrderBillItem(smartRestaurant.MenuService.MenuItem menu)
		{
			OrderBillItem orderBillItem = OrderManagement.AddOrderBillItem(this.selectedBill, menu, this.employeeID);
			if (orderBillItem == null)
			{
				this.StartInputMenu();
				this.isChanged = false;
				return;
			}
			this.selectedItem = orderBillItem;
			this.isChanged = true;
			this.StartInputOption();
		}

		private void BtnAmount_Click(object sender, EventArgs e)
		{
			if (this.selectedItem != null)
			{
				this.StartInputUnit();
			}
		}

		private void BtnCancel_Click(object sender, EventArgs e)
		{
			if (this.selectedItem != null)
			{
				this.isChanged |= OrderManagement.CancelOrderBillItem(this.selectedBill, this.selectedItem, this.employeeID);
				this.UpdateOrderGrid();
			}
		}

		private void BtnDown_Click(object sender, EventArgs e)
		{
			this.ListOrderItem.Down(5);
			this.UpdateOrderButton();
		}

		private void BtnMain_Click(object sender, EventArgs e)
		{
			if (this.orderInfo.TableID > 0)
			{
				this.tabService.UpdateTableLockInuse(this.orderInfo.TableID, false);
			}
			((MainForm)base.MdiParent).ShowMainMenuForm();
		}

		private void BtnMessage_Click(object sender, EventArgs e)
		{
			string str;
			if (this.selectedItem == null)
			{
				return;
			}
			string str1 = KeyboardForm.Show("Message", this.selectedItem.Message);
			if (str1 != null)
			{
				OrderBillItem orderBillItem = this.selectedItem;
				if (str1 == "")
				{
					str = null;
				}
				else
				{
					str = str1;
				}
				orderBillItem.Message = str;
				this.selectedItem.ChangeFlag = true;
				this.isChanged = true;
			}
			this.UpdateOrderGrid();
		}

		private void BtnMoveItem_Click(object sender, EventArgs e)
		{
			if (this.selectedItem != null)
			{
				this.StartMoveItem();
			}
		}

		private void BtnPrintKitchen_Click(object sender, EventArgs e)
		{
			string str;
			smartRestaurant.OrderService.OrderService orderService = new smartRestaurant.OrderService.OrderService();
			WaitingForm.Show("Print to Kitchen");
			base.Enabled = false;
			if (this.takeOutMode)
			{
				str = orderService.SendOrder(this.orderInfo, this.takeOutCustID, this.FieldCustName.Text);
			}
			else
			{
				str = orderService.SendOrder(this.orderInfo, 0, null);
				int num = 0;
				try
				{
					num = int.Parse(str);
					orderService.SetTableReference(num, this.tableIDList);
				}
				catch (Exception exception)
				{
					MessageBox.Show(exception.ToString());
					num = 0;
				}
			}
			base.Enabled = true;
			WaitingForm.HideForm();
			try
			{
				this.orderInfo.OrderID = int.Parse(str);
			}
			catch (Exception exception1)
			{
				MessageBox.Show(str);
				return;
			}
			if (this.orderInfo.TableID > 0)
			{
				this.tabService.UpdateTableLockInuse(this.orderInfo.TableID, false);
			}
			((MainForm)base.MdiParent).ShowMainMenuForm();
		}

		private void BtnPrintReceipt_Click(object sender, EventArgs e)
		{
			this.SaveBeforePrintReceipt();
			((MainForm)base.MdiParent).ShowPrintReceiptForm(this.tableInfo, this.orderInfo, this.selectedBill);
		}

		private void BtnPrintReceiptAll_Click(object sender, EventArgs e)
		{
			this.SaveBeforePrintReceipt();
			WaitingForm.Show("Print Receipt");
			base.Enabled = false;
			Receipt.PrintReceiptAll(this.orderInfo);
			base.Enabled = true;
			WaitingForm.HideForm();
			if (this.orderInfo.TableID > 0)
			{
				this.tabService.UpdateTableLockInuse(this.orderInfo.TableID, false);
			}
			((MainForm)base.MdiParent).ShowMainMenuForm();
		}

		private void BtnSearch_Click(object sender, EventArgs e)
		{
			if (this.FieldCustName.Text == TakeOrderForm.FIELD_CUST_TEXT)
			{
				this.FieldCustName.Text = "";
				this.takeOutCustID = 0;
			}
			((MainForm)base.MdiParent).ShowTakeOutForm(this.tableInfo, this.takeOutCustID, this.FieldCustName.Text, true);
		}

		private void BtnServeItem_Click(object sender, EventArgs e)
		{
			if (this.selectedItem != null)
			{
				this.selectedItem.ServeTime = DateTime.Now;
				this.isChanged |= true;
				this.UpdateOrderGrid();
			}
		}

        private void BtnUndo_Click(object sender, EventArgs e)
		{
			if (this.selectedItem != null)
			{
				this.isChanged |= OrderManagement.UndoCancelOrderBillItem(this.selectedItem, this.employeeID);
				this.UpdateOrderGrid();
			}
		}

		private void BtnUp_Click(object sender, EventArgs e)
		{
			this.ListOrderItem.Up(5);
			this.UpdateOrderButton();
		}

		private void CategoryPad_PadClick(object sender, ButtonListPadEventArgs e)
		{
			int num;
			if (this.inputState == 1 || this.inputState == 2)
			{
				this.StartInputMenu();
			}
			if (this.inputState != 0)
			{
				return;
			}
			if (e.Value == null)
			{
				return;
			}
			try
			{
				num = int.Parse(e.Value);
			}
			catch (Exception exception)
			{
				MessageForm.Show(exception.ToString(), "Exception");
				return;
			}
			this.inputValue = e.Button.Text.Substring(0, 2);
			this.UpdateMonitor();
			this.selectedType = this.menuTypes[num];
			smartRestaurant.MenuService.MenuItem[] menuItems = this.selectedType.MenuItems;
			this.OptionPad.AutoRefresh = false;
			this.OptionPad.Items.Clear();
			this.OptionPad.ItemStart = 0;
			if (menuItems != null)
			{
				for (int i = 0; i < (int)menuItems.Length; i++)
				{
					this.OptionPad.Items.Add(new ButtonItem(MenuManagement.GetMenuLanguageName(menuItems[i]), i.ToString()));
				}
			}
			this.OptionPad.Red = e.Button.Red;
			this.OptionPad.Green = e.Button.Green;
			this.OptionPad.Blue = e.Button.Blue;
			this.OptionPad.AutoRefresh = true;
		}

		private void ChangeBillCount()
		{
			if (this.billNumber < (int)this.orderInfo.Bills.Length && this.billNumber >= 1)
			{
				for (int i = this.billNumber; i < (int)this.orderInfo.Bills.Length; i++)
				{
					OrderBillItem[] items = this.orderInfo.Bills[i].Items;
					if (items != null)
					{
						OrderBillItem[] orderBillItemArray = this.orderInfo.Bills[0].Items;
						this.orderInfo.Bills[0].Items = new OrderBillItem[(int)orderBillItemArray.Length + (int)items.Length];
						int num = 0;
						for (int j = 0; j < (int)orderBillItemArray.Length; j++)
						{
							int num1 = num;
							num = num1 + 1;
							this.orderInfo.Bills[0].Items[num1] = orderBillItemArray[j];
						}
						for (int k = 0; k < (int)items.Length; k++)
						{
							int num2 = num;
							num = num2 + 1;
							this.orderInfo.Bills[0].Items[num2] = items[k];
						}
						this.orderInfo.Bills[i].Items = null;
					}
				}
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void FieldBill_Click(object sender, EventArgs e)
		{
			if (!this.takeOutMode)
			{
				this.inputState = 11;
				this.inputValue = "";
				this.UpdateMonitor();
			}
		}

		private void FieldCustName_Click(object sender, EventArgs e)
		{
			string str;
			str = (this.FieldCustName.Text == "" || this.FieldCustName.Text == TakeOrderForm.FIELD_CUST_TEXT ? "" : this.FieldCustName.Text);
			string str1 = KeyboardForm.Show("Customer Name", str);
			if (str1 != null)
			{
				if (str1 != "")
				{
					this.FieldCustName.Text = str1;
					return;
				}
				this.FieldCustName.Text = TakeOrderForm.FIELD_CUST_TEXT;
			}
		}

		private void FieldGuest_Click(object sender, EventArgs e)
		{
			if (!this.takeOutMode)
			{
				this.inputState = 10;
				this.inputValue = "";
				this.UpdateMonitor();
			}
		}

		private void FieldTable_Click(object sender, EventArgs e)
		{
			int num;
			if (!this.takeOutMode)
			{
				int[] numArray = TableForm.ShowMulti("Merge Table", this.tableInfo, this.tableIDList);
				if (numArray != null)
				{
					num = ((int)numArray.Length != 1 || numArray[0] != -1 ? (int)numArray.Length : 0);
					if (this.tableIDList != null)
					{
						if (num == (int)this.tableIDList.Length)
						{
							int num1 = 0;
							while (num1 < num)
							{
								if (numArray[num1] == this.tableIDList[num1])
								{
									num1++;
								}
								else
								{
									this.isChanged = true;
									goto Label0;
								}
							}
						}
						else
						{
							this.isChanged = true;
						}
					}
					else if (num > 0)
					{
						this.isChanged = true;
					}
				Label0:
					if (num != 0)
					{
						this.tableIDList = numArray;
					}
					else
					{
						this.tableIDList = null;
					}
					this.UpdateFlowButton();
				}
			}
		}

		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			ResourceManager resourceManager = new ResourceManager(typeof(TakeOrderForm));
			this.OrderPanel = new GroupPanel();
			this.FieldBill = new Label();
			this.LblBill = new Label();
			this.FieldGuest = new Label();
			this.LblGuest = new Label();
			this.FieldTable = new Label();
			this.LblTable = new Label();
			this.BtnMain = new ImageButton();
			this.ButtonImgList = new ImageList(this.components);
			this.NumberImgList = new ImageList(this.components);
			this.BtnCancel = new ImageButton();
			this.BtnUndo = new ImageButton();
			this.BtnMessage = new ImageButton();
			this.BtnPrintKitchen = new ImageButton();
			this.BtnPrintReceiptAll = new ImageButton();
			this.BtnPrintReceipt = new ImageButton();
			this.groupPanel2 = new GroupPanel();
			this.BtnAmount = new ImageButton();
			this.FieldCurrentInput = new Label();
			this.FieldInputType = new Label();
			this.NumberKeyPad = new NumberPad();
			this.CategoryPad = new ButtonListPad();
			this.groupPanel3 = new GroupPanel();
			this.OptionPad = new ButtonListPad();
			this.ListOrderItem = new ItemsList();
			this.ListOrderCount = new ItemsList();
			this.ListOrderItemBy = new ItemsList();
			this.BtnDown = new ImageButton();
			this.BtnUp = new ImageButton();
			this.BtnMoveItem = new ImageButton();
			this.PanCustName = new GroupPanel();
			this.FieldCustName = new Label();
			this.BtnSearch = new ImageButton();
			this.BtnServeItem = new ImageButton();
			this.LblPageID = new Label();
			this.LblCopyright = new Label();
			this.LblTotalText = new Label();
			this.LblTotalValue = new Label();
			this.OrderPanel.SuspendLayout();
			this.groupPanel2.SuspendLayout();
			this.groupPanel3.SuspendLayout();
			this.PanCustName.SuspendLayout();
			base.SuspendLayout();
			this.OrderPanel.BackColor = Color.Transparent;
			this.OrderPanel.Caption = null;
			this.OrderPanel.Controls.Add(this.FieldBill);
			this.OrderPanel.Controls.Add(this.LblBill);
			this.OrderPanel.Controls.Add(this.FieldGuest);
			this.OrderPanel.Controls.Add(this.LblGuest);
			this.OrderPanel.Controls.Add(this.FieldTable);
			this.OrderPanel.Controls.Add(this.LblTable);
			this.OrderPanel.Font = new System.Drawing.Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.OrderPanel.Location = new Point(560, 0);
			this.OrderPanel.Name = "OrderPanel";
			this.OrderPanel.ShowHeader = true;
			this.OrderPanel.Size = new System.Drawing.Size(449, 58);
			this.OrderPanel.TabIndex = 1;
			this.FieldBill.BackColor = Color.White;
			this.FieldBill.Cursor = Cursors.Hand;
			this.FieldBill.Location = new Point(384, 1);
			this.FieldBill.Name = "FieldBill";
			this.FieldBill.Size = new System.Drawing.Size(64, 56);
			this.FieldBill.TabIndex = 11;
			this.FieldBill.Text = "1";
			this.FieldBill.TextAlign = ContentAlignment.MiddleCenter;
			this.FieldBill.Click += new EventHandler(this.FieldBill_Click);
			this.LblBill.BackColor = Color.Orange;
			this.LblBill.Location = new Point(312, 1);
			this.LblBill.Name = "LblBill";
			this.LblBill.Size = new System.Drawing.Size(72, 56);
			this.LblBill.TabIndex = 10;
			this.LblBill.Text = "Bill:";
			this.LblBill.TextAlign = ContentAlignment.MiddleCenter;
			this.FieldGuest.BackColor = Color.White;
			this.FieldGuest.Cursor = Cursors.Hand;
			this.FieldGuest.Location = new Point(248, 1);
			this.FieldGuest.Name = "FieldGuest";
			this.FieldGuest.Size = new System.Drawing.Size(64, 56);
			this.FieldGuest.TabIndex = 9;
			this.FieldGuest.Text = "1";
			this.FieldGuest.TextAlign = ContentAlignment.MiddleCenter;
			this.FieldGuest.Click += new EventHandler(this.FieldGuest_Click);
			this.LblGuest.BackColor = Color.Orange;
			this.LblGuest.Location = new Point(176, 1);
			this.LblGuest.Name = "LblGuest";
			this.LblGuest.Size = new System.Drawing.Size(72, 56);
			this.LblGuest.TabIndex = 8;
			this.LblGuest.Text = "Seat:";
			this.LblGuest.TextAlign = ContentAlignment.MiddleCenter;
			this.FieldTable.BackColor = Color.White;
			this.FieldTable.Cursor = Cursors.Hand;
			this.FieldTable.Location = new Point(73, 1);
			this.FieldTable.Name = "FieldTable";
			this.FieldTable.Size = new System.Drawing.Size(103, 56);
			this.FieldTable.TabIndex = 7;
			this.FieldTable.Text = "1";
			this.FieldTable.TextAlign = ContentAlignment.MiddleCenter;
			this.FieldTable.Click += new EventHandler(this.FieldTable_Click);
			this.LblTable.BackColor = Color.Orange;
			this.LblTable.Location = new Point(1, 1);
			this.LblTable.Name = "LblTable";
			this.LblTable.Size = new System.Drawing.Size(72, 56);
			this.LblTable.TabIndex = 6;
			this.LblTable.Text = "Table:";
			this.LblTable.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnMain.BackColor = Color.Transparent;
			this.BtnMain.Blue = 2f;
			this.BtnMain.Cursor = Cursors.Hand;
			this.BtnMain.Green = 2f;
			this.BtnMain.ImageClick = (Image)resourceManager.GetObject("BtnMain.ImageClick");
			this.BtnMain.ImageClickIndex = 1;
			this.BtnMain.ImageIndex = 0;
			this.BtnMain.ImageList = this.ButtonImgList;
			this.BtnMain.IsLock = false;
			this.BtnMain.Location = new Point(456, 64);
			this.BtnMain.Name = "BtnMain";
			this.BtnMain.ObjectValue = null;
			this.BtnMain.Red = 1f;
			this.BtnMain.Size = new System.Drawing.Size(110, 60);
			this.BtnMain.TabIndex = 2;
			this.BtnMain.Text = "Main";
			this.BtnMain.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnMain.Click += new EventHandler(this.BtnMain_Click);
			this.ButtonImgList.ColorDepth = ColorDepth.Depth32Bit;
			this.ButtonImgList.ImageSize = new System.Drawing.Size(110, 60);
			this.ButtonImgList.ImageStream = (ImageListStreamer)resourceManager.GetObject("ButtonImgList.ImageStream");
			this.ButtonImgList.TransparentColor = Color.Transparent;
			this.NumberImgList.ColorDepth = ColorDepth.Depth32Bit;
			this.NumberImgList.ImageSize = new System.Drawing.Size(72, 60);
			this.NumberImgList.ImageStream = (ImageListStreamer)resourceManager.GetObject("NumberImgList.ImageStream");
			this.NumberImgList.TransparentColor = Color.Transparent;
			this.BtnCancel.BackColor = Color.Transparent;
			this.BtnCancel.Blue = 2f;
			this.BtnCancel.Cursor = Cursors.Hand;
			this.BtnCancel.Green = 1f;
			this.BtnCancel.ImageClick = (Image)resourceManager.GetObject("BtnCancel.ImageClick");
			this.BtnCancel.ImageClickIndex = 1;
			this.BtnCancel.ImageIndex = 0;
			this.BtnCancel.ImageList = this.ButtonImgList;
			this.BtnCancel.IsLock = false;
			this.BtnCancel.Location = new Point(8, 64);
			this.BtnCancel.Name = "BtnCancel";
			this.BtnCancel.ObjectValue = null;
			this.BtnCancel.Red = 2f;
			this.BtnCancel.Size = new System.Drawing.Size(110, 60);
			this.BtnCancel.TabIndex = 8;
			this.BtnCancel.Text = "Cancel";
			this.BtnCancel.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnCancel.Click += new EventHandler(this.BtnCancel_Click);
			this.BtnUndo.BackColor = Color.Transparent;
			this.BtnUndo.Blue = 2f;
			this.BtnUndo.Cursor = Cursors.Hand;
			this.BtnUndo.Green = 1f;
			this.BtnUndo.ImageClick = (Image)resourceManager.GetObject("BtnUndo.ImageClick");
			this.BtnUndo.ImageClickIndex = 1;
			this.BtnUndo.ImageIndex = 0;
			this.BtnUndo.ImageList = this.ButtonImgList;
			this.BtnUndo.IsLock = false;
			this.BtnUndo.Location = new Point(120, 64);
			this.BtnUndo.Name = "BtnUndo";
			this.BtnUndo.ObjectValue = null;
			this.BtnUndo.Red = 2f;
			this.BtnUndo.Size = new System.Drawing.Size(110, 60);
			this.BtnUndo.TabIndex = 9;
			this.BtnUndo.Text = "Undo";
			this.BtnUndo.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnUndo.Click += new EventHandler(this.BtnUndo_Click);
			this.BtnMessage.BackColor = Color.Transparent;
			this.BtnMessage.Blue = 1f;
			this.BtnMessage.Cursor = Cursors.Hand;
			this.BtnMessage.Green = 1.75f;
			this.BtnMessage.ImageClick = (Image)resourceManager.GetObject("BtnMessage.ImageClick");
			this.BtnMessage.ImageClickIndex = 1;
			this.BtnMessage.ImageIndex = 0;
			this.BtnMessage.ImageList = this.ButtonImgList;
			this.BtnMessage.IsLock = false;
			this.BtnMessage.Location = new Point(568, 64);
			this.BtnMessage.Name = "BtnMessage";
			this.BtnMessage.ObjectValue = null;
			this.BtnMessage.Red = 1.75f;
			this.BtnMessage.Size = new System.Drawing.Size(110, 60);
			this.BtnMessage.TabIndex = 10;
			this.BtnMessage.Text = "Message";
			this.BtnMessage.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnMessage.Click += new EventHandler(this.BtnMessage_Click);
			this.BtnPrintKitchen.BackColor = Color.Transparent;
			this.BtnPrintKitchen.Blue = 0.75f;
			this.BtnPrintKitchen.Cursor = Cursors.Hand;
			this.BtnPrintKitchen.Green = 1f;
			this.BtnPrintKitchen.ImageClick = (Image)resourceManager.GetObject("BtnPrintKitchen.ImageClick");
			this.BtnPrintKitchen.ImageClickIndex = 1;
			this.BtnPrintKitchen.ImageIndex = 0;
			this.BtnPrintKitchen.ImageList = this.ButtonImgList;
			this.BtnPrintKitchen.IsLock = false;
			this.BtnPrintKitchen.Location = new Point(680, 64);
			this.BtnPrintKitchen.Name = "BtnPrintKitchen";
			this.BtnPrintKitchen.ObjectValue = null;
			this.BtnPrintKitchen.Red = 1f;
			this.BtnPrintKitchen.Size = new System.Drawing.Size(110, 60);
			this.BtnPrintKitchen.TabIndex = 11;
			this.BtnPrintKitchen.Text = "Print-Kitchen / Save";
			this.BtnPrintKitchen.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnPrintKitchen.Click += new EventHandler(this.BtnPrintKitchen_Click);
			this.BtnPrintReceiptAll.BackColor = Color.Transparent;
			this.BtnPrintReceiptAll.Blue = 1.75f;
			this.BtnPrintReceiptAll.Cursor = Cursors.Hand;
			this.BtnPrintReceiptAll.Green = 1f;
			this.BtnPrintReceiptAll.ImageClick = (Image)resourceManager.GetObject("BtnPrintReceiptAll.ImageClick");
			this.BtnPrintReceiptAll.ImageClickIndex = 1;
			this.BtnPrintReceiptAll.ImageIndex = 0;
			this.BtnPrintReceiptAll.ImageList = this.ButtonImgList;
			this.BtnPrintReceiptAll.IsLock = false;
			this.BtnPrintReceiptAll.Location = new Point(792, 64);
			this.BtnPrintReceiptAll.Name = "BtnPrintReceiptAll";
			this.BtnPrintReceiptAll.ObjectValue = null;
			this.BtnPrintReceiptAll.Red = 1.75f;
			this.BtnPrintReceiptAll.Size = new System.Drawing.Size(110, 60);
			this.BtnPrintReceiptAll.TabIndex = 12;
			this.BtnPrintReceiptAll.Text = "Print-Receipt All";
			this.BtnPrintReceiptAll.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnPrintReceiptAll.Click += new EventHandler(this.BtnPrintReceiptAll_Click);
			this.BtnPrintReceipt.BackColor = Color.Transparent;
			this.BtnPrintReceipt.Blue = 1f;
			this.BtnPrintReceipt.Cursor = Cursors.Hand;
			this.BtnPrintReceipt.Green = 1f;
			this.BtnPrintReceipt.ImageClick = (Image)resourceManager.GetObject("BtnPrintReceipt.ImageClick");
			this.BtnPrintReceipt.ImageClickIndex = 1;
			this.BtnPrintReceipt.ImageIndex = 0;
			this.BtnPrintReceipt.ImageList = this.ButtonImgList;
			this.BtnPrintReceipt.IsLock = false;
			this.BtnPrintReceipt.Location = new Point(904, 64);
			this.BtnPrintReceipt.Name = "BtnPrintReceipt";
			this.BtnPrintReceipt.ObjectValue = null;
			this.BtnPrintReceipt.Red = 0.75f;
			this.BtnPrintReceipt.Size = new System.Drawing.Size(110, 60);
			this.BtnPrintReceipt.TabIndex = 13;
			this.BtnPrintReceipt.Text = "Pay";
			this.BtnPrintReceipt.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnPrintReceipt.Click += new EventHandler(this.BtnPrintReceipt_Click);
			this.groupPanel2.BackColor = Color.Transparent;
			this.groupPanel2.Caption = null;
			this.groupPanel2.Controls.Add(this.BtnAmount);
			this.groupPanel2.Controls.Add(this.FieldCurrentInput);
			this.groupPanel2.Controls.Add(this.FieldInputType);
			this.groupPanel2.Controls.Add(this.NumberKeyPad);
			this.groupPanel2.Controls.Add(this.CategoryPad);
			this.groupPanel2.Font = new System.Drawing.Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.groupPanel2.Location = new Point(328, 128);
			this.groupPanel2.Name = "groupPanel2";
			this.groupPanel2.ShowHeader = false;
			this.groupPanel2.Size = new System.Drawing.Size(344, 624);
			this.groupPanel2.TabIndex = 15;
			this.BtnAmount.BackColor = Color.Transparent;
			this.BtnAmount.Blue = 1f;
			this.BtnAmount.Cursor = Cursors.Hand;
			this.BtnAmount.Green = 1f;
			this.BtnAmount.ImageClick = (Image)resourceManager.GetObject("BtnAmount.ImageClick");
			this.BtnAmount.ImageClickIndex = 1;
			this.BtnAmount.ImageIndex = 0;
			this.BtnAmount.ImageList = this.NumberImgList;
			this.BtnAmount.IsLock = false;
			this.BtnAmount.Location = new Point(256, 555);
			this.BtnAmount.Name = "BtnAmount";
			this.BtnAmount.ObjectValue = null;
			this.BtnAmount.Red = 2f;
			this.BtnAmount.Size = new System.Drawing.Size(72, 60);
			this.BtnAmount.TabIndex = 20;
			this.BtnAmount.Text = "Amount";
			this.BtnAmount.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnAmount.Click += new EventHandler(this.BtnAmount_Click);
			this.BtnAmount.DoubleClick += new EventHandler(this.BtnAmount_Click);
			this.FieldCurrentInput.BackColor = Color.Black;
			this.FieldCurrentInput.Font = new System.Drawing.Font("Tahoma", 14.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.FieldCurrentInput.ForeColor = Color.Cyan;
			this.FieldCurrentInput.Location = new Point(136, 312);
			this.FieldCurrentInput.Name = "FieldCurrentInput";
			this.FieldCurrentInput.Size = new System.Drawing.Size(200, 40);
			this.FieldCurrentInput.TabIndex = 9;
			this.FieldCurrentInput.TextAlign = ContentAlignment.MiddleCenter;
			this.FieldInputType.BackColor = Color.Black;
			this.FieldInputType.Font = new System.Drawing.Font("Tahoma", 14.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.FieldInputType.ForeColor = Color.Cyan;
			this.FieldInputType.Location = new Point(8, 312);
			this.FieldInputType.Name = "FieldInputType";
			this.FieldInputType.Size = new System.Drawing.Size(128, 40);
			this.FieldInputType.TabIndex = 8;
			this.FieldInputType.Text = "Menu";
			this.FieldInputType.TextAlign = ContentAlignment.MiddleCenter;
			this.NumberKeyPad.BackColor = Color.White;
			this.NumberKeyPad.Image = (Image)resourceManager.GetObject("NumberKeyPad.Image");
			this.NumberKeyPad.ImageClick = (Image)resourceManager.GetObject("NumberKeyPad.ImageClick");
			this.NumberKeyPad.ImageClickIndex = 1;
			this.NumberKeyPad.ImageIndex = 0;
			this.NumberKeyPad.ImageList = this.NumberImgList;
			this.NumberKeyPad.Location = new Point(24, 360);
			this.NumberKeyPad.Name = "NumberKeyPad";
			this.NumberKeyPad.Size = new System.Drawing.Size(226, 255);
			this.NumberKeyPad.TabIndex = 7;
			this.NumberKeyPad.Text = "numberPad1";
			this.NumberKeyPad.PadClick += new NumberPadEventHandler(this.NumberKeyPad_PadClick);
			this.CategoryPad.AutoRefresh = true;
			this.CategoryPad.BackColor = Color.White;
			this.CategoryPad.Blue = 1f;
			this.CategoryPad.Column = 3;
			this.CategoryPad.Font = new System.Drawing.Font("Tahoma", 12f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.CategoryPad.Green = 1f;
			this.CategoryPad.Image = (Image)resourceManager.GetObject("CategoryPad.Image");
			this.CategoryPad.ImageClick = (Image)resourceManager.GetObject("CategoryPad.ImageClick");
			this.CategoryPad.ImageClickIndex = 1;
			this.CategoryPad.ImageIndex = 0;
			this.CategoryPad.ImageList = this.ButtonImgList;
			this.CategoryPad.ItemStart = 0;
			this.CategoryPad.Location = new Point(5, 5);
			this.CategoryPad.Name = "CategoryPad";
			this.CategoryPad.Padding = 1;
			this.CategoryPad.Red = 1f;
			this.CategoryPad.Row = 5;
			this.CategoryPad.Size = new System.Drawing.Size(332, 304);
			this.CategoryPad.TabIndex = 6;
			this.CategoryPad.Text = "buttonListPad2";
			this.CategoryPad.PadClick += new ButtonListPadEventHandler(this.CategoryPad_PadClick);
			this.groupPanel3.BackColor = Color.Transparent;
			this.groupPanel3.Caption = null;
			this.groupPanel3.Controls.Add(this.OptionPad);
			this.groupPanel3.Font = new System.Drawing.Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.groupPanel3.Location = new Point(672, 128);
			this.groupPanel3.Name = "groupPanel3";
			this.groupPanel3.ShowHeader = false;
			this.groupPanel3.Size = new System.Drawing.Size(344, 624);
			this.groupPanel3.TabIndex = 16;
			this.OptionPad.AutoRefresh = true;
			this.OptionPad.BackColor = Color.White;
			this.OptionPad.Blue = 1f;
			this.OptionPad.Column = 3;
			this.OptionPad.Font = new System.Drawing.Font("Tahoma", 12f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.OptionPad.Green = 1f;
			this.OptionPad.Image = (Image)resourceManager.GetObject("OptionPad.Image");
			this.OptionPad.ImageClick = (Image)resourceManager.GetObject("OptionPad.ImageClick");
			this.OptionPad.ImageClickIndex = 1;
			this.OptionPad.ImageIndex = 0;
			this.OptionPad.ImageList = this.ButtonImgList;
			this.OptionPad.ItemStart = 0;
			this.OptionPad.Location = new Point(5, 5);
			this.OptionPad.Name = "OptionPad";
			this.OptionPad.Padding = 1;
			this.OptionPad.Red = 1f;
			this.OptionPad.Row = 10;
			this.OptionPad.Size = new System.Drawing.Size(332, 609);
			this.OptionPad.TabIndex = 5;
			this.OptionPad.Text = "buttonListPad1";
			this.OptionPad.PadClick += new ButtonListPadEventHandler(this.OptionPad_PadClick);
			this.ListOrderItem.Alignment = ContentAlignment.MiddleLeft;
			this.ListOrderItem.AutoRefresh = true;
			this.ListOrderItem.BackAlterColor = Color.FromArgb(192, 255, 255);
			this.ListOrderItem.BackHeaderColor = Color.FromArgb(255, 192, 128);
			this.ListOrderItem.BackHeaderSelectedColor = Color.FromArgb(192, 0, 0);
			this.ListOrderItem.BackNormalColor = Color.White;
			this.ListOrderItem.BackSelectedColor = Color.Blue;
			this.ListOrderItem.BindList1 = this.ListOrderCount;
			this.ListOrderItem.BindList2 = null;
			this.ListOrderItem.Border = 0;
			this.ListOrderItem.Font = new System.Drawing.Font("Tahoma", 12f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.ListOrderItem.ForeAlterColor = Color.Black;
			this.ListOrderItem.ForeHeaderColor = Color.Black;
			this.ListOrderItem.ForeHeaderSelectedColor = Color.White;
			this.ListOrderItem.ForeNormalColor = Color.Black;
			this.ListOrderItem.ForeSelectedColor = Color.White;
			this.ListOrderItem.ItemHeight = 40;
			this.ListOrderItem.ItemWidth = 240;
			this.ListOrderItem.Location = new Point(8, 128);
			this.ListOrderItem.Name = "ListOrderItem";
			this.ListOrderItem.Row = 13;
			this.ListOrderItem.SelectedIndex = 0;
			this.ListOrderItem.Size = new System.Drawing.Size(240, 520);
			this.ListOrderItem.TabIndex = 17;
			this.ListOrderItem.ItemClick += new ItemsListEventHandler(this.ListOrderItem_ItemClick);
			this.ListOrderCount.Alignment = ContentAlignment.MiddleCenter;
			this.ListOrderCount.AutoRefresh = true;
			this.ListOrderCount.BackAlterColor = Color.FromArgb(192, 255, 255);
			this.ListOrderCount.BackHeaderColor = Color.FromArgb(255, 192, 128);
			this.ListOrderCount.BackHeaderSelectedColor = Color.FromArgb(192, 0, 0);
			this.ListOrderCount.BackNormalColor = Color.White;
			this.ListOrderCount.BackSelectedColor = Color.Blue;
			this.ListOrderCount.BindList1 = this.ListOrderItem;
			this.ListOrderCount.BindList2 = this.ListOrderItemBy;
			this.ListOrderCount.Border = 0;
			this.ListOrderCount.Font = new System.Drawing.Font("Tahoma", 12f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.ListOrderCount.ForeAlterColor = Color.Black;
			this.ListOrderCount.ForeHeaderColor = Color.Black;
			this.ListOrderCount.ForeHeaderSelectedColor = Color.White;
			this.ListOrderCount.ForeNormalColor = Color.Black;
			this.ListOrderCount.ForeSelectedColor = Color.White;
			this.ListOrderCount.ItemHeight = 40;
			this.ListOrderCount.ItemWidth = 40;
			this.ListOrderCount.Location = new Point(248, 128);
			this.ListOrderCount.Name = "ListOrderCount";
			this.ListOrderCount.Row = 13;
			this.ListOrderCount.SelectedIndex = 0;
			this.ListOrderCount.Size = new System.Drawing.Size(40, 520);
			this.ListOrderCount.TabIndex = 36;
			this.ListOrderCount.ItemClick += new ItemsListEventHandler(this.ListOrderItem_ItemClick);
			this.ListOrderItemBy.Alignment = ContentAlignment.MiddleCenter;
			this.ListOrderItemBy.AutoRefresh = true;
			this.ListOrderItemBy.BackAlterColor = Color.FromArgb(192, 255, 255);
			this.ListOrderItemBy.BackHeaderColor = Color.FromArgb(255, 192, 128);
			this.ListOrderItemBy.BackHeaderSelectedColor = Color.FromArgb(192, 0, 0);
			this.ListOrderItemBy.BackNormalColor = Color.White;
			this.ListOrderItemBy.BackSelectedColor = Color.Blue;
			this.ListOrderItemBy.BindList1 = this.ListOrderCount;
			this.ListOrderItemBy.BindList2 = null;
			this.ListOrderItemBy.Border = 0;
			this.ListOrderItemBy.Font = new System.Drawing.Font("Tahoma", 12f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.ListOrderItemBy.ForeAlterColor = Color.Black;
			this.ListOrderItemBy.ForeHeaderColor = Color.Black;
			this.ListOrderItemBy.ForeHeaderSelectedColor = Color.White;
			this.ListOrderItemBy.ForeNormalColor = Color.Black;
			this.ListOrderItemBy.ForeSelectedColor = Color.White;
			this.ListOrderItemBy.ItemHeight = 40;
			this.ListOrderItemBy.ItemWidth = 40;
			this.ListOrderItemBy.Location = new Point(288, 128);
			this.ListOrderItemBy.Name = "ListOrderItemBy";
			this.ListOrderItemBy.Row = 13;
			this.ListOrderItemBy.SelectedIndex = 0;
			this.ListOrderItemBy.Size = new System.Drawing.Size(40, 520);
			this.ListOrderItemBy.TabIndex = 34;
			this.ListOrderItemBy.ItemClick += new ItemsListEventHandler(this.ListOrderItem_ItemClick);
			this.BtnDown.BackColor = Color.Transparent;
			this.BtnDown.Blue = 2f;
			this.BtnDown.Cursor = Cursors.Hand;
			this.BtnDown.Green = 1f;
			this.BtnDown.ImageClick = (Image)resourceManager.GetObject("BtnDown.ImageClick");
			this.BtnDown.ImageClickIndex = 5;
			this.BtnDown.ImageIndex = 4;
			this.BtnDown.ImageList = this.ButtonImgList;
			this.BtnDown.IsLock = false;
			this.BtnDown.Location = new Point(208, 692);
			this.BtnDown.Name = "BtnDown";
			this.BtnDown.ObjectValue = null;
			this.BtnDown.Red = 2f;
			this.BtnDown.Size = new System.Drawing.Size(110, 60);
			this.BtnDown.TabIndex = 19;
			this.BtnDown.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnDown.Click += new EventHandler(this.BtnDown_Click);
			this.BtnDown.DoubleClick += new EventHandler(this.BtnDown_Click);
			this.BtnUp.BackColor = Color.Transparent;
			this.BtnUp.Blue = 2f;
			this.BtnUp.Cursor = Cursors.Hand;
			this.BtnUp.Green = 1f;
			this.BtnUp.ImageClick = (Image)resourceManager.GetObject("BtnUp.ImageClick");
			this.BtnUp.ImageClickIndex = 3;
			this.BtnUp.ImageIndex = 2;
			this.BtnUp.ImageList = this.ButtonImgList;
			this.BtnUp.IsLock = false;
			this.BtnUp.Location = new Point(16, 692);
			this.BtnUp.Name = "BtnUp";
			this.BtnUp.ObjectValue = null;
			this.BtnUp.Red = 2f;
			this.BtnUp.Size = new System.Drawing.Size(110, 60);
			this.BtnUp.TabIndex = 18;
			this.BtnUp.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnUp.Click += new EventHandler(this.BtnUp_Click);
			this.BtnUp.DoubleClick += new EventHandler(this.BtnUp_Click);
			this.BtnMoveItem.BackColor = Color.Transparent;
			this.BtnMoveItem.Blue = 2f;
			this.BtnMoveItem.Cursor = Cursors.Hand;
			this.BtnMoveItem.Green = 1f;
			this.BtnMoveItem.ImageClick = (Image)resourceManager.GetObject("BtnMoveItem.ImageClick");
			this.BtnMoveItem.ImageClickIndex = 1;
			this.BtnMoveItem.ImageIndex = 0;
			this.BtnMoveItem.ImageList = this.ButtonImgList;
			this.BtnMoveItem.IsLock = false;
			this.BtnMoveItem.Location = new Point(232, 64);
			this.BtnMoveItem.Name = "BtnMoveItem";
			this.BtnMoveItem.ObjectValue = null;
			this.BtnMoveItem.Red = 2f;
			this.BtnMoveItem.Size = new System.Drawing.Size(110, 60);
			this.BtnMoveItem.TabIndex = 20;
			this.BtnMoveItem.Text = "Move Item";
			this.BtnMoveItem.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnMoveItem.Click += new EventHandler(this.BtnMoveItem_Click);
			this.PanCustName.BackColor = Color.Transparent;
			this.PanCustName.Caption = null;
			this.PanCustName.Controls.Add(this.FieldCustName);
			this.PanCustName.Font = new System.Drawing.Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.PanCustName.Location = new Point(248, 0);
			this.PanCustName.Name = "PanCustName";
			this.PanCustName.ShowHeader = false;
			this.PanCustName.Size = new System.Drawing.Size(200, 58);
			this.PanCustName.TabIndex = 22;
			this.FieldCustName.Cursor = Cursors.Hand;
			this.FieldCustName.Location = new Point(1, 1);
			this.FieldCustName.Name = "FieldCustName";
			this.FieldCustName.Size = new System.Drawing.Size(199, 56);
			this.FieldCustName.TabIndex = 0;
			this.FieldCustName.TextAlign = ContentAlignment.MiddleLeft;
			this.FieldCustName.Click += new EventHandler(this.FieldCustName_Click);
			this.BtnSearch.BackColor = Color.Transparent;
			this.BtnSearch.Blue = 0.5f;
			this.BtnSearch.Cursor = Cursors.Hand;
			this.BtnSearch.Green = 1f;
			this.BtnSearch.ImageClick = null;
			this.BtnSearch.ImageClickIndex = 0;
			this.BtnSearch.ImageIndex = 0;
			this.BtnSearch.ImageList = this.ButtonImgList;
			this.BtnSearch.IsLock = false;
			this.BtnSearch.Location = new Point(449, 0);
			this.BtnSearch.Name = "BtnSearch";
			this.BtnSearch.ObjectValue = null;
			this.BtnSearch.Red = 1f;
			this.BtnSearch.Size = new System.Drawing.Size(110, 60);
			this.BtnSearch.TabIndex = 23;
			this.BtnSearch.Text = "Search";
			this.BtnSearch.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnSearch.Click += new EventHandler(this.BtnSearch_Click);
			this.BtnServeItem.BackColor = Color.Transparent;
			this.BtnServeItem.Blue = 2f;
			this.BtnServeItem.Cursor = Cursors.Hand;
			this.BtnServeItem.Green = 1f;
			this.BtnServeItem.ImageClick = (Image)resourceManager.GetObject("BtnServeItem.ImageClick");
			this.BtnServeItem.ImageClickIndex = 1;
			this.BtnServeItem.ImageIndex = 0;
			this.BtnServeItem.ImageList = this.ButtonImgList;
			this.BtnServeItem.IsLock = false;
			this.BtnServeItem.Location = new Point(344, 64);
			this.BtnServeItem.Name = "BtnServeItem";
			this.BtnServeItem.ObjectValue = null;
			this.BtnServeItem.Red = 2f;
			this.BtnServeItem.Size = new System.Drawing.Size(110, 60);
			this.BtnServeItem.TabIndex = 24;
			this.BtnServeItem.Text = "Serve Item";
			this.BtnServeItem.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnServeItem.Click += new EventHandler(this.BtnServeItem_Click);
			this.LblPageID.BackColor = Color.Transparent;
			this.LblPageID.Font = new System.Drawing.Font("Tahoma", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.LblPageID.ForeColor = Color.FromArgb(103, 138, 198);
			this.LblPageID.Location = new Point(816, 752);
			this.LblPageID.Name = "LblPageID";
			this.LblPageID.Size = new System.Drawing.Size(192, 23);
			this.LblPageID.TabIndex = 33;
			this.LblPageID.Text = "| STTO011";
			this.LblPageID.TextAlign = ContentAlignment.TopRight;
			this.LblCopyright.BackColor = Color.Transparent;
			this.LblCopyright.Font = new System.Drawing.Font("Tahoma", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.LblCopyright.ForeColor = Color.FromArgb(103, 138, 198);
			this.LblCopyright.Location = new Point(8, 752);
			this.LblCopyright.Name = "LblCopyright";
			this.LblCopyright.Size = new System.Drawing.Size(280, 16);
			this.LblCopyright.TabIndex = 35;
			this.LblCopyright.Text = "Copyright (c) 2004. All rights reserved.";
			this.LblTotalText.BackColor = Color.FromArgb(255, 255, 128);
			this.LblTotalText.Font = new System.Drawing.Font("Tahoma", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.LblTotalText.Location = new Point(8, 648);
			this.LblTotalText.Name = "LblTotalText";
			this.LblTotalText.Size = new System.Drawing.Size(208, 40);
			this.LblTotalText.TabIndex = 37;
			this.LblTotalText.Text = "Total (Before Tax)";
			this.LblTotalText.TextAlign = ContentAlignment.MiddleLeft;
			this.LblTotalValue.BackColor = Color.FromArgb(255, 255, 128);
			this.LblTotalValue.Font = new System.Drawing.Font("Tahoma", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.LblTotalValue.ForeColor = Color.FromArgb(0, 0, 192);
			this.LblTotalValue.Location = new Point(216, 648);
			this.LblTotalValue.Name = "LblTotalValue";
			this.LblTotalValue.Size = new System.Drawing.Size(112, 40);
			this.LblTotalValue.TabIndex = 38;
			this.LblTotalValue.Text = "0.00";
			this.LblTotalValue.TextAlign = ContentAlignment.MiddleRight;
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
			base.ClientSize = new System.Drawing.Size(1020, 764);
			base.Controls.Add(this.LblTotalValue);
			base.Controls.Add(this.LblTotalText);
			base.Controls.Add(this.ListOrderCount);
			base.Controls.Add(this.LblCopyright);
			base.Controls.Add(this.LblPageID);
			base.Controls.Add(this.BtnServeItem);
			base.Controls.Add(this.BtnSearch);
			base.Controls.Add(this.PanCustName);
			base.Controls.Add(this.BtnMoveItem);
			base.Controls.Add(this.BtnDown);
			base.Controls.Add(this.BtnUp);
			base.Controls.Add(this.ListOrderItem);
			base.Controls.Add(this.groupPanel3);
			base.Controls.Add(this.groupPanel2);
			base.Controls.Add(this.BtnPrintReceipt);
			base.Controls.Add(this.BtnPrintReceiptAll);
			base.Controls.Add(this.BtnPrintKitchen);
			base.Controls.Add(this.BtnMessage);
			base.Controls.Add(this.BtnUndo);
			base.Controls.Add(this.BtnCancel);
			base.Controls.Add(this.BtnMain);
			base.Controls.Add(this.OrderPanel);
			base.Controls.Add(this.ListOrderItemBy);
			base.Name = "TakeOrderForm";
			this.Text = "Take Order";
			this.OrderPanel.ResumeLayout(false);
			this.groupPanel2.ResumeLayout(false);
			this.groupPanel3.ResumeLayout(false);
			this.PanCustName.ResumeLayout(false);
			base.ResumeLayout(false);
		}

		private void ListOrderItem_ItemClick(object sender, ItemsListEventArgs e)
		{
			OrderBill orderBill = null;
			OrderBillItem orderBillItem = null;
			if (this.moveItem)
			{
				orderBill = this.selectedBill;
				orderBillItem = this.selectedItem;
			}
			if (e.Item.Value is OrderBill)
			{
				this.selectedBill = (OrderBill)e.Item.Value;
				this.selectedItem = null;
				this.StartInputMenu();
			}
			else if (e.Item.Value is OrderBillItem)
			{
				this.selectedItem = (OrderBillItem)e.Item.Value;
				for (int i = 0; i >= 0 && i < (int)this.orderInfo.Bills.Length; i++)
				{
					if (this.orderInfo.Bills[i].Items != null)
					{
						int num = 0;
						while (num < (int)this.orderInfo.Bills[i].Items.Length)
						{
							if (this.orderInfo.Bills[i].Items[num] != this.selectedItem)
							{
								num++;
							}
							else
							{
								this.selectedBill = this.orderInfo.Bills[i];
								i = -2;
								break;
							}
						}
					}
				}
				if (this.selectedBill.CloseBillDate == AppParameter.MinDateTime)
				{
					this.StartInputOption();
				}
				else
				{
					this.StartInputMenu();
				}
			}
			if (orderBillItem != null)
			{
				if (orderBill == this.selectedBill)
				{
					this.UpdateOrderGrid();
				}
				else if (OrderManagement.MoveOrderBillItem(orderBill, this.selectedBill, orderBillItem))
				{
					this.isChanged = true;
					this.UpdateOrderGrid();
					return;
				}
			}
		}

		private void LoadMenus()
		{
			MenuManagement.LoadMenus();
			this.menuTypes = MenuManagement.MenuTypes;
			this.menuOptions = MenuManagement.MenuOptions;
		}

        private void NumberKeyPad_PadClick(object sender, NumberPadEventArgs e)
        {
            if (e.IsNumeric)
            {
                if ((this.inputState == 2) || (this.inputState == 20))
                {
                    this.StartInputMenu();
                }
                this.inputValue = this.inputValue + e.Number.ToString();
                this.UpdateMonitor();
            }
            else if (e.IsCancel)
            {
                if (this.inputValue.Length > 1)
                {
                    this.inputValue = this.inputValue.Substring(0, this.inputValue.Length - 1);
                    this.UpdateMonitor();
                }
                else
                {
                    this.StartInputMenu();
                }
            }
            else if (e.IsEnter)
            {
                int num;
                try
                {
                    num = int.Parse(this.inputValue);
                }
                catch (Exception)
                {
                    num = 0;
                }
                switch (this.inputState)
                {
                    case 0:
                        {
                            if ((num == 0) || (this.selectedBill == null))
                            {
                                break;
                            }
                            smartRestaurant.MenuService.MenuItem menuItemKeyID = MenuManagement.GetMenuItemKeyID(num);
                            if (menuItemKeyID == null)
                            {
                                break;
                            }
                            this.AddOrderBillItem(menuItemKeyID);
                            return;
                        }
                    case 1:
                        if (this.selectedItem == null)
                        {
                            break;
                        }
                        if ((num > 0) && (this.selectedItem.Unit != num))
                        {
                            this.selectedItem.Unit = num;
                            this.selectedItem.ChangeFlag = true;
                            this.isChanged = true;
                        }
                        this.StartInputMenu();
                        return;

                    case 10:
                        if (num > 0)
                        {
                            this.guestNumber = num;
                            this.UpdateTableInformation();
                        }
                        break;

                    case 11:
                        if (num > 0)
                        {
                            this.billNumber = num;
                            if (this.billNumber > this.guestNumber)
                            {
                                this.guestNumber = this.billNumber;
                            }
                            this.ChangeBillCount();
                            this.UpdateOrderGrid();
                            this.UpdateTableInformation();
                        }
                        break;
                }
                this.StartInputMenu();
            }
        }

        private void OptionPad_PadClick(object sender, ButtonListPadEventArgs e)
		{
			int num;
			int num1;
			int num2;
			smartRestaurant.MenuService.MenuItem menuItems;
			if (this.inputState != 0 && this.inputState != 2 || this.inputState == 0 && (this.inputValue == null || this.inputValue.Length == 0))
			{
				return;
			}
			if (e.Value == null)
			{
				return;
			}
			try
			{
				if (this.inputState != 2)
				{
					int num3 = 0;
					num2 = num3;
					num1 = num3;
					num = int.Parse(e.Value);
				}
				else
				{
					string[] strArrays = e.Value.Split(new char[] { ':' });
					num1 = int.Parse(strArrays[0]);
					num2 = int.Parse(strArrays[1]);
					num = 0;
				}
			}
			catch (Exception exception)
			{
				MessageForm.Show(exception.ToString(), "Exception");
				return;
			}
			switch (this.inputState)
			{
				case 0:
				{
					menuItems = this.selectedType.MenuItems[num];
					this.AddOrderBillItem(menuItems);
					return;
				}
				case 1:
				{
					break;
				}
				case 2:
				{
					string str = string.Concat(num1, ":");
					string str1 = num2.ToString();
					if (this.selectedItem.ItemChoices == null)
					{
						menuItems = MenuManagement.GetMenuItemFromID(this.selectedItem.MenuID);
						this.selectedItem.ItemChoices = new OrderItemChoice[(int)menuItems.MenuDefaults.Length];
						for (int i = 0; i < (int)menuItems.MenuDefaults.Length; i++)
						{
							this.selectedItem.ItemChoices[i] = new OrderItemChoice();
							this.selectedItem.ItemChoices[i].OptionID = menuItems.MenuDefaults[i].OptionID;
							this.selectedItem.ItemChoices[i].ChoiceID = menuItems.MenuDefaults[i].DefaultChoiceID;
						}
						this.selectedItem.DefaultOption = false;
					}
					for (int j = 0; j < this.OptionPad.Items.Count; j++)
					{
						ButtonItem item = (ButtonItem)this.OptionPad.Items[j];
						if (item.Value.Substring(0, str.Length) == str)
						{
							if (item.Value.Substring(str.Length) != str1)
							{
								this.OptionPad.SetMatrix(j, 1f, 1f, 1f);
							}
							else
							{
								this.OptionPad.SetMatrix(j, 2f, 1f, 2f);
							}
						}
					}
					for (int k = 0; k < (int)this.selectedItem.ItemChoices.Length; k++)
					{
						if (this.selectedItem.ItemChoices[k].OptionID == num1)
						{
							this.selectedItem.ItemChoices[k].ChoiceID = num2;
							this.selectedItem.DefaultOption = false;
							this.selectedItem.ChangeFlag = true;
							this.isChanged = true;
							return;
						}
					}
					break;
				}
				default:
				{
					return;
				}
			}
		}

		private void SaveBeforePrintReceipt()
		{
			string str;
			smartRestaurant.OrderService.OrderService orderService = new smartRestaurant.OrderService.OrderService();
			if (this.takeOutMode)
			{
				str = orderService.SendOrderPrint(this.orderInfo, this.takeOutCustID, this.FieldCustName.Text, false);
			}
			else
			{
				str = orderService.SendOrderPrint(this.orderInfo, 0, null, false);
				int num = 0;
				try
				{
					num = int.Parse(str);
					orderService.SetTableReference(num, this.tableIDList);
				}
				catch (Exception exception)
				{
					MessageBox.Show(exception.ToString());
					num = 0;
				}
			}
		}

		private void SetCategory()
		{
			this.CategoryPad.AutoRefresh = false;
			this.CategoryPad.SetMatrix(0, 1f, 1f, 1f);
			this.CategoryPad.SetMatrix(1, 2f, 1f, 1f);
			this.CategoryPad.SetMatrix(2, 1f, 2f, 1f);
			this.CategoryPad.SetMatrix(3, 1f, 1f, 2f);
			this.CategoryPad.SetMatrix(4, 2f, 2f, 1f);
			this.CategoryPad.SetMatrix(5, 2f, 1f, 2f);
			this.CategoryPad.SetMatrix(6, 1f, 2f, 2f);
			this.CategoryPad.SetMatrix(7, 1.75f, 1f, 1f);
			this.CategoryPad.SetMatrix(8, 1f, 1.75f, 1f);
			this.CategoryPad.SetMatrix(9, 1f, 1f, 1.75f);
			this.CategoryPad.SetMatrix(10, 1.75f, 1.75f, 1f);
			this.CategoryPad.SetMatrix(11, 1.75f, 1f, 1.75f);
			this.CategoryPad.SetMatrix(12, 1f, 1.75f, 1.75f);
			this.CategoryPad.SetMatrix(13, 0.75f, 1f, 1f);
			this.CategoryPad.SetMatrix(14, 1f, 1f, 1f);
			for (int i = 0; i < (int)this.menuTypes.Length; i++)
			{
				this.CategoryPad.Items.Add(new ButtonItem(this.menuTypes[i].Name, i.ToString()));
			}
			this.CategoryPad.AutoRefresh = true;
		}

		private void StartInputMenu()
		{
			this.inputState = 0;
			this.inputValue = "";
			this.selectedItem = null;
			this.selectedType = null;
			this.moveItem = false;
			this.CategoryPad.AutoRefresh = false;
			this.CategoryPad.ItemStart = 0;
			this.CategoryPad.AutoRefresh = true;
			this.UpdateOrderGrid();
			this.UpdateMonitor();
			this.OptionPad.AutoRefresh = false;
			this.OptionPad.Items.Clear();
			this.OptionPad.ItemStart = 0;
			this.OptionPad.Red = 1f;
			this.OptionPad.Green = 1f;
			this.OptionPad.Blue = 1f;
			this.OptionPad.AutoRefresh = true;
		}

		private void StartInputOption()
		{
			int num;
			smartRestaurant.MenuService.MenuItem menuItemFromID = MenuManagement.GetMenuItemFromID(this.selectedItem.MenuID);
			if (menuItemFromID == null)
			{
				this.StartInputMenu();
				return;
			}
			this.inputState = 2;
			this.inputValue = "";
			this.selectedType = null;
			this.moveItem = false;
			this.CategoryPad.AutoRefresh = false;
			this.CategoryPad.ItemStart = 0;
			this.CategoryPad.AutoRefresh = true;
			this.UpdateOrderGrid();
			this.UpdateMonitor();
			this.OptionPad.AutoRefresh = false;
			this.OptionPad.Items.Clear();
			this.OptionPad.ItemStart = 0;
			this.OptionPad.Red = 1f;
			this.OptionPad.Green = 1f;
			this.OptionPad.Blue = 1f;
			if (this.menuOptions != null)
			{
				for (int i = 0; i < (int)this.menuOptions.Length; i++)
				{
					int length = 0;
					if (this.selectedItem.DefaultOption)
					{
						if (menuItemFromID.MenuDefaults != null)
						{
							length = (int)menuItemFromID.MenuDefaults.Length;
						}
					}
					else if (this.selectedItem.ItemChoices != null)
					{
						length = (int)this.selectedItem.ItemChoices.Length;
					}
					for (int j = 0; j < length; j++)
					{
						num = (!this.selectedItem.DefaultOption ? this.selectedItem.ItemChoices[j].OptionID : menuItemFromID.MenuDefaults[j].OptionID);
						if (this.menuOptions[i].OptionID == num)
						{
							for (int k = 0; k < (int)this.menuOptions[i].OptionChoices.Length; k++)
							{
								ButtonItem buttonItem = new ButtonItem(this.menuOptions[i].OptionChoices[k].ChoiceName, string.Concat(this.menuOptions[i].OptionChoices[k].OptionID.ToString(), ":", this.menuOptions[i].OptionChoices[k].ChoiceID.ToString()));
								if (j % 2 != 0)
								{
									buttonItem.ForeColor = Color.Blue;
								}
								else
								{
									buttonItem.ForeColor = Color.Red;
								}
								this.OptionPad.Items.Add(buttonItem);
								if (this.selectedItem.DefaultOption)
								{
									if (this.menuOptions[i].OptionChoices[k].ChoiceID == menuItemFromID.MenuDefaults[j].DefaultChoiceID)
									{
										this.OptionPad.SetMatrix(this.OptionPad.Items.Count - 1, 2f, 1f, 2f);
									}
								}
								else if (this.selectedItem.ItemChoices != null && this.menuOptions[i].OptionChoices[k].ChoiceID == this.selectedItem.ItemChoices[j].ChoiceID)
								{
									this.OptionPad.SetMatrix(this.OptionPad.Items.Count - 1, 2f, 1f, 2f);
								}
							}
						}
					}
				}
			}
			this.OptionPad.AutoRefresh = true;
		}

		private void StartInputUnit()
		{
			this.inputState = 1;
			this.inputValue = "";
			this.selectedType = null;
			this.moveItem = false;
			this.CategoryPad.AutoRefresh = false;
			this.CategoryPad.ItemStart = 0;
			this.CategoryPad.AutoRefresh = true;
			this.UpdateOrderGrid();
			this.UpdateMonitor();
			this.OptionPad.AutoRefresh = false;
			this.OptionPad.Items.Clear();
			this.OptionPad.ItemStart = 0;
			this.OptionPad.Red = 1f;
			this.OptionPad.Green = 1f;
			this.OptionPad.Blue = 1f;
			this.OptionPad.AutoRefresh = true;
		}

		private void StartMoveItem()
		{
			this.inputState = 20;
			this.inputValue = "-> Select Seat";
			this.selectedType = null;
			this.moveItem = true;
			this.CategoryPad.AutoRefresh = false;
			this.CategoryPad.ItemStart = 0;
			this.CategoryPad.AutoRefresh = true;
			this.UpdateOrderGrid();
			this.UpdateMonitor();
			this.OptionPad.AutoRefresh = false;
			this.OptionPad.Items.Clear();
			this.OptionPad.ItemStart = 0;
			this.OptionPad.Red = 1f;
			this.OptionPad.Green = 1f;
			this.OptionPad.Blue = 1f;
			this.OptionPad.AutoRefresh = true;
		}

		private void UpdateFlowButton()
		{
			if (!this.isChanged)
			{
				this.BtnPrintKitchen.Enabled = false;
			}
			else if (!this.takeOutMode)
			{
				this.BtnPrintKitchen.Enabled = true;
			}
			else if (this.FieldCustName.Text == null || this.FieldCustName.Text == "" || this.FieldCustName.Text == TakeOrderForm.FIELD_CUST_TEXT)
			{
				this.BtnPrintKitchen.Enabled = false;
			}
			else
			{
				this.BtnPrintKitchen.Enabled = true;
			}
			if (this.orderInfo.OrderID != 0 && !this.isChanged && this.selectedBill != null && this.selectedBill.CloseBillDate == AppParameter.MinDateTime)
			{
				this.BtnPrintReceipt.Enabled = true;
				this.BtnPrintReceiptAll.Enabled = true;
				return;
			}
			this.BtnPrintReceipt.Enabled = false;
			this.BtnPrintReceiptAll.Enabled = false;
		}

		public override void UpdateForm()
		{
			this.LoadMenus();
			this.takeOutMode = this.tableInfo.TableID == 0;
			smartRestaurant.OrderService.OrderService orderService = new smartRestaurant.OrderService.OrderService();
			if (!this.takeOrderResume)
			{
				if (!this.takeOutMode)
				{
					this.orderInfo = orderService.GetOrder(this.tableInfo.TableID);
					if (this.orderInfo == null)
					{
						this.tableIDList = null;
					}
					else
					{
						if (this.tableInfo.TableID != this.orderInfo.TableID)
						{
							this.tableInfo = this.tabService.GetTableInformation(this.orderInfo.TableID);
						}
						this.tableIDList = orderService.GetTableReference(this.orderInfo.OrderID);
					}
				}
				else if (this.takeOutOrderID <= 0)
				{
					this.orderInfo = null;
					this.tableIDList = null;
				}
				else
				{
					this.orderInfo = orderService.GetOrderByOrderID(this.takeOutOrderID);
					this.tableIDList = null;
				}
				this.isChanged = false;
			}
			else if (!this.takeOutMode)
			{
				this.orderInfo = orderService.GetOrder(this.tableInfo.TableID);
			}
			if (this.orderInfo != null)
			{
				this.guestNumber = this.orderInfo.NumberOfGuest;
				this.billNumber = (int)this.orderInfo.Bills.Length;
			}
			else if (this.guestNumber <= 0)
			{
				int num = 1;
				int num1 = num;
				this.billNumber = num;
				this.guestNumber = num1;
			}
			if (!AppParameter.IsDemo())
			{
				if (!AppParameter.ShowOrderItemPrice)
				{
					this.ListOrderItem.ItemWidth = 280;
					this.ListOrderCount.Left = 288;
					this.ListOrderCount.ItemWidth = 40;
					this.ListOrderItem.Row = 14;
					this.ListOrderCount.Row = 14;
					this.LblTotalText.Visible = false;
					this.LblTotalValue.Visible = false;
				}
				else
				{
					this.ListOrderItem.ItemWidth = 240;
					this.ListOrderCount.Left = 248;
					this.ListOrderCount.ItemWidth = 80;
					this.ListOrderItem.Row = 13;
					this.ListOrderCount.Row = 13;
					this.LblTotalText.Visible = true;
					this.LblTotalValue.Visible = true;
				}
				this.ListOrderItemBy.Visible = false;
				this.LblGuest.Text = "Seat";
			}
			else
			{
				this.ListOrderItem.ItemWidth = 240;
				this.ListOrderCount.Left = 248;
				this.ListOrderItem.Row = 14;
				this.ListOrderCount.Row = 14;
				this.ListOrderItemBy.Row = 14;
				this.ListOrderItemBy.Visible = true;
				this.LblTotalText.Visible = false;
				this.LblTotalValue.Visible = false;
				this.LblGuest.Text = "Guest";
			}
			this.ListOrderCount.Left = this.ListOrderItem.Left + this.ListOrderItem.ItemWidth;
			this.selectedBill = null;
			this.selectedItem = null;
			this.selectedType = null;
			if (this.orderInfo != null && this.orderInfo.Bills != null)
			{
				int num2 = 0;
				while (num2 < (int)this.orderInfo.Bills.Length)
				{
					if (this.orderInfo.Bills[num2].CloseBillDate != AppParameter.MinDateTime)
					{
						num2++;
					}
					else
					{
						this.selectedBill = this.orderInfo.Bills[num2];
						break;
					}
				}
			}
			this.LblPageID.Text = string.Concat("Employee ID:", ((MainForm)base.MdiParent).UserID, " | ");
			if (!this.takeOutMode)
			{
				Label lblPageID = this.LblPageID;
				lblPageID.Text = string.Concat(lblPageID.Text, "STTO011");
			}
			else
			{
				Label label = this.LblPageID;
				label.Text = string.Concat(label.Text, "STTO021");
			}
			this.PanCustName.Visible = this.takeOutMode;
			this.BtnSearch.Visible = this.takeOutMode;
			this.OptionPad.AutoRefresh = false;
			ButtonListPad optionPad = this.OptionPad;
			ButtonListPad buttonListPad = this.OptionPad;
			float single = 1f;
			float single1 = single;
			this.OptionPad.Blue = single;
			float single2 = single1;
			single1 = single2;
			buttonListPad.Green = single2;
			optionPad.Red = single1;
			this.OptionPad.AutoRefresh = true;
			this.ListOrderItem.Reset();
			this.ListOrderCount.Reset();
			this.ListOrderItemBy.Reset();
			this.UpdateTableInformation();
			this.StartInputMenu();
		}

        private void UpdateMonitor()
        {
            switch (this.inputState)
            {
                case 1:
                    this.FieldInputType.Text = "Unit";
                    break;

                case 2:
                    this.FieldInputType.Text = "Option";
                    break;

                case 10:
                    this.FieldInputType.Text = "Guest";
                    break;

                case 11:
                    this.FieldInputType.Text = "Bill";
                    break;

                case 20:
                    this.FieldInputType.Text = "Move Item";
                    break;

                default:
                    this.inputState = 0;
                    this.FieldInputType.Text = "Menu";
                    break;
            }
            if ((this.inputState == 1) && (this.inputValue == ""))
            {
                this.FieldCurrentInput.ForeColor = Color.Yellow;
                this.FieldCurrentInput.Text = this.selectedItem.Unit.ToString();
            }
            else
            {
                this.FieldCurrentInput.ForeColor = Color.Cyan;
                this.FieldCurrentInput.Text = this.inputValue;
            }
            if (this.inputState == 10)
            {
                this.FieldGuest.BackColor = Color.Blue;
                this.FieldGuest.ForeColor = Color.White;
            }
            else
            {
                this.FieldGuest.BackColor = Color.White;
                this.FieldGuest.ForeColor = Color.Black;
            }
            if (this.inputState == 11)
            {
                this.FieldBill.BackColor = Color.Blue;
                this.FieldBill.ForeColor = Color.White;
            }
            else
            {
                this.FieldBill.BackColor = Color.White;
                this.FieldBill.ForeColor = Color.Black;
            }
            if (this.takeOutMode && (this.FieldCustName.Text == ""))
            {
                this.FieldCustName.Text = FIELD_CUST_TEXT;
            }
        }

        private void UpdateOrderButton()
		{
			if (this.moveItem || this.selectedItem == null || !(this.selectedBill.CloseBillDate == AppParameter.MinDateTime))
			{
				this.BtnCancel.Enabled = false;
				this.BtnUndo.Enabled = false;
				this.BtnMoveItem.Enabled = false;
				this.BtnServeItem.Enabled = false;
				this.BtnMessage.Enabled = false;
				this.BtnAmount.Enabled = false;
			}
			else
			{
				if (this.selectedItem.ServeTime == AppParameter.MinDateTime)
				{
					this.BtnCancel.Enabled = !OrderManagement.IsCancel(this.selectedItem);
					this.BtnUndo.Enabled = OrderManagement.IsCancel(this.selectedItem);
					this.BtnServeItem.Enabled = !OrderManagement.IsCancel(this.selectedItem);
					this.BtnMessage.Enabled = !OrderManagement.IsCancel(this.selectedItem);
					this.BtnAmount.Enabled = !OrderManagement.IsCancel(this.selectedItem);
				}
				this.BtnMoveItem.Enabled = (OrderManagement.IsCancel(this.selectedItem) ? false : (int)this.orderInfo.Bills.Length > 1);
			}
			this.BtnUp.Enabled = this.ListOrderItem.CanUp;
			this.BtnDown.Enabled = this.ListOrderItem.CanDown;
		}

		private void UpdateOrderGrid()
		{
			StringBuilder stringBuilder = new StringBuilder();
			this.ListOrderItem.AutoRefresh = false;
			this.ListOrderCount.AutoRefresh = false;
			this.ListOrderItemBy.AutoRefresh = false;
			this.ListOrderItem.Items.Clear();
			this.ListOrderCount.Items.Clear();
			this.ListOrderItemBy.Items.Clear();
			if (this.orderInfo == null || (int)this.orderInfo.Bills.Length != this.billNumber)
			{
				this.orderInfo = OrderManagement.CreateOrderObject(this.orderInfo, this.employeeID, this.tableInfo, this.guestNumber, this.billNumber);
			}
			OrderBill[] bills = this.orderInfo.Bills;
			if (this.selectedBill == null)
			{
				this.selectedBill = bills[0];
			}
			double num = 0;
			double num1 = 0;
			for (int i = 0; i < (int)bills.Length; i++)
			{
				stringBuilder.Length = 0;
				if (!AppParameter.IsDemo())
				{
					stringBuilder.Append("Seat #");
				}
				else
				{
					stringBuilder.Append("Bill #");
				}
				stringBuilder.Append(i + 1);
				if (bills[i].CloseBillDate != AppParameter.MinDateTime)
				{
					stringBuilder.Append(" (Closed)");
				}
				DataItem dataItem = new DataItem(stringBuilder.ToString(), bills[i], true);
				this.ListOrderItem.Items.Add(dataItem);
				dataItem = new DataItem("Amt.", bills[i], true);
				this.ListOrderCount.Items.Add(dataItem);
				dataItem = new DataItem("Emp#", bills[i], true);
				this.ListOrderItemBy.Items.Add(dataItem);
				if (this.selectedBill == bills[i] && this.selectedItem == null)
				{
					this.ListOrderItem.SelectedIndex = this.ListOrderItem.Items.Count - 1;
					this.ListOrderCount.SelectedIndex = this.ListOrderCount.Items.Count - 1;
					this.ListOrderItemBy.SelectedIndex = this.ListOrderItemBy.Items.Count - 1;
				}
				OrderBillItem[] items = bills[i].Items;
				if (items != null)
				{
					for (int j = 0; j < (int)items.Length; j++)
					{
						bool flag = OrderManagement.IsCancel(items[j]);
						dataItem = new DataItem(OrderManagement.OrderBillItemDisplayString(items[j]), items[j], false);
						if (flag)
						{
							dataItem.Strike = true;
						}
						this.ListOrderItem.Items.Add(dataItem);
						dataItem = new DataItem(OrderManagement.OrderBillCountDisplayString(items[j], ref num1), items[j], false);
						if (flag)
						{
							dataItem.Strike = true;
						}
						this.ListOrderCount.Items.Add(dataItem);
						dataItem = new DataItem(items[j].EmployeeID.ToString(), items[j], false);
						if (flag)
						{
							dataItem.Strike = true;
						}
						this.ListOrderItemBy.Items.Add(dataItem);
						if (this.selectedItem == items[j])
						{
							this.ListOrderItem.SelectedIndex = this.ListOrderItem.Items.Count - 1;
							this.ListOrderCount.SelectedIndex = this.ListOrderCount.Items.Count - 1;
							this.ListOrderItemBy.SelectedIndex = this.ListOrderItemBy.Items.Count - 1;
						}
						if (!flag)
						{
							num += num1;
						}
					}
				}
			}
			this.LblTotalValue.Text = num.ToString("N");
			this.ListOrderItem.AutoRefresh = true;
			this.ListOrderCount.AutoRefresh = true;
			this.ListOrderItemBy.AutoRefresh = true;
			this.UpdateOrderButton();
			this.UpdateFlowButton();
		}

		private void UpdateTableInformation()
		{
			this.FieldTable.Text = this.tableInfo.TableName;
			this.FieldGuest.Text = this.guestNumber.ToString();
			this.FieldBill.Text = this.billNumber.ToString();
			if (this.orderInfo != null)
			{
				this.orderInfo.NumberOfGuest = this.guestNumber;
			}
		}
	}
}