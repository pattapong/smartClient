using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Text;
using smartRestaurant.Controls;
using smartRestaurant.Data;
using smartRestaurant.Utils;
using smartRestaurant.MenuService;
using smartRestaurant.OrderService;
using smartRestaurant.TableService;
using System.Resources;
using System.Windows.Forms;

namespace smartRestaurant
{
	/// <summary>
	/// Summary description for TakeOrderForm.
	/// </summary>
	public class TakeOrderForm : SmartForm
	{
		// Fields
		private int billNumber;
		private ImageButton BtnAmount;
		private ImageButton BtnCancel;
		private ImageButton BtnDown;
		private ImageButton BtnMain;
		private ImageButton BtnMessage;
		private ImageButton BtnMoveItem;
		private ImageButton BtnPrintKitchen;
		private ImageButton BtnPrintReceipt;
		private ImageButton BtnPrintReceiptAll;
		private ImageButton BtnSearch;
		private ImageButton BtnServeItem;
		private ImageButton BtnUndo;
		private ImageButton BtnUp;
		private ImageList ButtonImgList;
		private ButtonListPad CategoryPad;
		private IContainer components;
		private int employeeID;
		private static string FIELD_CUST_TEXT = "- Please input name -";
		private Label FieldBill;
		private Label FieldCurrentInput;
		private Label FieldCustName;
		private Label FieldGuest;
		private Label FieldInputType;
		private Label FieldTable;
		private GroupPanel groupPanel2;
		private GroupPanel groupPanel3;
		private int guestNumber;
		private const int INPUT_BILL = 11;
		private const int INPUT_GUEST = 10;
		private const int INPUT_MENU = 0;
		private const int INPUT_MOVEITEM = 20;
		private const int INPUT_OPTION = 2;
		private const int INPUT_UNIT = 1;
		private int inputState;
		private string inputValue;
		private bool isChanged;
		private Label LblBill;
		private Label LblCopyright;
		private Label LblGuest;
		private Label LblPageID;
		private Label LblTable;
		private Label LblTotalText;
		private Label LblTotalValue;
		private ItemsList ListOrderCount;
		private ItemsList ListOrderItem;
		private ItemsList ListOrderItemBy;
		private MenuOption[] menuOptions;
		private MenuType[] menuTypes;
		private bool moveItem;
		private ImageList NumberImgList;
		private NumberPad NumberKeyPad;
		private ButtonListPad OptionPad;
		private OrderInformation orderInfo;
		private GroupPanel OrderPanel;
		private GroupPanel PanCustName;
		private OrderBill selectedBill;
		private OrderBillItem selectedItem;
		private MenuType selectedType;
		private int[] tableIDList;
		private TableInformation tableInfo;
		private smartRestaurant.TableService.TableService tabService = new smartRestaurant.TableService.TableService();
		private bool takeOrderResume;
		private int takeOutCustID;
		private bool takeOutMode;
		private int takeOutOrderID;

		// Methods
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
			OrderBillItem item = OrderManagement.AddOrderBillItem(this.selectedBill, menu, this.employeeID);
			if (item != null)
			{
				this.selectedItem = item;
				this.isChanged = true;
				this.StartInputOption();
			}
			else
			{
				this.StartInputMenu();
				this.isChanged = false;
			}
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
			((MainForm) base.MdiParent).ShowMainMenuForm();
		}

		private void BtnMessage_Click(object sender, EventArgs e)
		{
			if (this.selectedItem != null)
			{
				string str = KeyboardForm.Show("Message", this.selectedItem.Message);
				if (str != null)
				{
					this.selectedItem.Message = (str == "") ? null : str;
					this.selectedItem.ChangeFlag = true;
					this.isChanged = true;
				}
				this.UpdateOrderGrid();
			}
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
			smartRestaurant.OrderService.OrderService service = new smartRestaurant.OrderService.OrderService();
			WaitingForm.Show("Print to Kitchen");
			base.Enabled = false;
			if (!this.takeOutMode)
			{
				str = service.SendOrder(this.orderInfo, 0, null);
				int orderID = 0;
				try
				{
					orderID = int.Parse(str);
					service.SetTableReference(orderID, this.tableIDList);
				}
				catch (Exception exception)
				{
					MessageBox.Show(exception.ToString());
					orderID = 0;
				}
			}
			else
			{
				str = service.SendOrder(this.orderInfo, this.takeOutCustID, this.FieldCustName.Text);
			}
			base.Enabled = true;
			WaitingForm.HideForm();
			try
			{
				this.orderInfo.OrderID = int.Parse(str);
			}
			catch (Exception)
			{
				MessageBox.Show(str);
				return;
			}
			if (this.orderInfo.TableID > 0)
			{
				this.tabService.UpdateTableLockInuse(this.orderInfo.TableID, false);
			}
			((MainForm) base.MdiParent).ShowMainMenuForm();
		}

		private void BtnPrintReceipt_Click(object sender, EventArgs e)
		{
			this.SaveBeforePrintReceipt();
			((MainForm) base.MdiParent).ShowPrintReceiptForm(this.tableInfo, this.orderInfo, this.selectedBill);
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
			((MainForm) base.MdiParent).ShowMainMenuForm();
		}

		private void BtnSearch_Click(object sender, EventArgs e)
		{
			if (this.FieldCustName.Text == FIELD_CUST_TEXT)
			{
				this.FieldCustName.Text = "";
				this.takeOutCustID = 0;
			}
			((MainForm) base.MdiParent).ShowTakeOutForm(this.tableInfo, this.takeOutCustID, this.FieldCustName.Text, true);
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
			if ((this.inputState == 1) || (this.inputState == 2))
			{
				this.StartInputMenu();
			}
			if ((this.inputState == 0) && (e.Value != null))
			{
				int num;
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
					for (int i = 0; i < menuItems.Length; i++)
					{
						this.OptionPad.Items.Add(new ButtonItem(MenuManagement.GetMenuLanguageName(menuItems[i]), i.ToString()));
					}
				}
				this.OptionPad.Red = e.Button.Red;
				this.OptionPad.Green = e.Button.Green;
				this.OptionPad.Blue = e.Button.Blue;
				this.OptionPad.AutoRefresh = true;
			}
		}

		private void ChangeBillCount()
		{
			if ((this.billNumber < this.orderInfo.Bills.Length) && (this.billNumber >= 1))
			{
				for (int i = this.billNumber; i < this.orderInfo.Bills.Length; i++)
				{
					OrderBillItem[] items = this.orderInfo.Bills[i].Items;
					if (items != null)
					{
						OrderBillItem[] itemArray2 = this.orderInfo.Bills[0].Items;
						this.orderInfo.Bills[0].Items = new OrderBillItem[itemArray2.Length + items.Length];
						int num2 = 0;
						for (int j = 0; j < itemArray2.Length; j++)
						{
							this.orderInfo.Bills[0].Items[num2++] = itemArray2[j];
						}
						for (int k = 0; k < items.Length; k++)
						{
							this.orderInfo.Bills[0].Items[num2++] = items[k];
						}
						this.orderInfo.Bills[i].Items = null;
					}
				}
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && (this.components != null))
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
			string text;
			if ((this.FieldCustName.Text == "") || (this.FieldCustName.Text == FIELD_CUST_TEXT))
			{
				text = "";
			}
			else
			{
				text = this.FieldCustName.Text;
			}
			string str2 = KeyboardForm.Show("Customer Name", text);
			if (str2 != null)
			{
				if (str2 != "")
				{
					this.FieldCustName.Text = str2;
				}
				else
				{
					this.FieldCustName.Text = FIELD_CUST_TEXT;
				}
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
			if (!this.takeOutMode)
			{
				int[] numArray = TableForm.ShowMulti("Merge Table", this.tableInfo, this.tableIDList);
				if (numArray != null)
				{
					int length;
					if ((numArray.Length == 1) && (numArray[0] == -1))
					{
						length = 0;
					}
					else
					{
						length = numArray.Length;
					}
					if (this.tableIDList != null)
					{
						if (length != this.tableIDList.Length)
						{
							this.isChanged = true;
						}
						else
						{
							for (int i = 0; i < length; i++)
							{
								if (numArray[i] != this.tableIDList[i])
								{
									this.isChanged = true;
									break;
								}
							}
						}
					}
					else if (length > 0)
					{
						this.isChanged = true;
					}
					if (length == 0)
					{
						this.tableIDList = null;
					}
					else
					{
						this.tableIDList = numArray;
					}
					this.UpdateFlowButton();
				}
			}
		}

		private void InitializeComponent()
		{
			this.components = new Container();
			ResourceManager manager = new ResourceManager(typeof(TakeOrderForm));
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
			this.OrderPanel.Font = new Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.OrderPanel.Location = new Point(560, 0);
			this.OrderPanel.Name = "OrderPanel";
			this.OrderPanel.ShowHeader = true;
			this.OrderPanel.Size = new Size(0x1c1, 0x3a);
			this.OrderPanel.TabIndex = 1;
			this.FieldBill.BackColor = Color.White;
			this.FieldBill.Cursor = Cursors.Hand;
			this.FieldBill.Location = new Point(0x180, 1);
			this.FieldBill.Name = "FieldBill";
			this.FieldBill.Size = new Size(0x40, 0x38);
			this.FieldBill.TabIndex = 11;
			this.FieldBill.Text = "1";
			this.FieldBill.TextAlign = ContentAlignment.MiddleCenter;
			this.FieldBill.Click += new EventHandler(this.FieldBill_Click);
			this.LblBill.BackColor = Color.Orange;
			this.LblBill.Location = new Point(0x138, 1);
			this.LblBill.Name = "LblBill";
			this.LblBill.Size = new Size(0x48, 0x38);
			this.LblBill.TabIndex = 10;
			this.LblBill.Text = "Bill:";
			this.LblBill.TextAlign = ContentAlignment.MiddleCenter;
			this.FieldGuest.BackColor = Color.White;
			this.FieldGuest.Cursor = Cursors.Hand;
			this.FieldGuest.Location = new Point(0xf8, 1);
			this.FieldGuest.Name = "FieldGuest";
			this.FieldGuest.Size = new Size(0x40, 0x38);
			this.FieldGuest.TabIndex = 9;
			this.FieldGuest.Text = "1";
			this.FieldGuest.TextAlign = ContentAlignment.MiddleCenter;
			this.FieldGuest.Click += new EventHandler(this.FieldGuest_Click);
			this.LblGuest.BackColor = Color.Orange;
			this.LblGuest.Location = new Point(0xb0, 1);
			this.LblGuest.Name = "LblGuest";
			this.LblGuest.Size = new Size(0x48, 0x38);
			this.LblGuest.TabIndex = 8;
			this.LblGuest.Text = "Seat:";
			this.LblGuest.TextAlign = ContentAlignment.MiddleCenter;
			this.FieldTable.BackColor = Color.White;
			this.FieldTable.Cursor = Cursors.Hand;
			this.FieldTable.Location = new Point(0x49, 1);
			this.FieldTable.Name = "FieldTable";
			this.FieldTable.Size = new Size(0x67, 0x38);
			this.FieldTable.TabIndex = 7;
			this.FieldTable.Text = "1";
			this.FieldTable.TextAlign = ContentAlignment.MiddleCenter;
			this.FieldTable.Click += new EventHandler(this.FieldTable_Click);
			this.LblTable.BackColor = Color.Orange;
			this.LblTable.Location = new Point(1, 1);
			this.LblTable.Name = "LblTable";
			this.LblTable.Size = new Size(0x48, 0x38);
			this.LblTable.TabIndex = 6;
			this.LblTable.Text = "Table:";
			this.LblTable.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnMain.BackColor = Color.Transparent;
			this.BtnMain.Blue = 2f;
			this.BtnMain.Cursor = Cursors.Hand;
			this.BtnMain.Green = 2f;
			this.BtnMain.ImageClick = (Image) manager.GetObject("BtnMain.ImageClick");
			this.BtnMain.ImageClickIndex = 1;
			this.BtnMain.ImageIndex = 0;
			this.BtnMain.ImageList = this.ButtonImgList;
			this.BtnMain.IsLock = false;
			this.BtnMain.Location = new Point(0x1c8, 0x40);
			this.BtnMain.Name = "BtnMain";
			this.BtnMain.ObjectValue = null;
			this.BtnMain.Red = 1f;
			this.BtnMain.Size = new Size(110, 60);
			this.BtnMain.TabIndex = 2;
			this.BtnMain.Text = "Main";
			this.BtnMain.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnMain.Click += new EventHandler(this.BtnMain_Click);
			this.ButtonImgList.ColorDepth = ColorDepth.Depth32Bit;
			this.ButtonImgList.ImageSize = new Size(110, 60);
			this.ButtonImgList.ImageStream = (ImageListStreamer) manager.GetObject("ButtonImgList.ImageStream");
			this.ButtonImgList.TransparentColor = Color.Transparent;
			this.NumberImgList.ColorDepth = ColorDepth.Depth32Bit;
			this.NumberImgList.ImageSize = new Size(0x48, 60);
			this.NumberImgList.ImageStream = (ImageListStreamer) manager.GetObject("NumberImgList.ImageStream");
			this.NumberImgList.TransparentColor = Color.Transparent;
			this.BtnCancel.BackColor = Color.Transparent;
			this.BtnCancel.Blue = 2f;
			this.BtnCancel.Cursor = Cursors.Hand;
			this.BtnCancel.Green = 1f;
			this.BtnCancel.ImageClick = (Image) manager.GetObject("BtnCancel.ImageClick");
			this.BtnCancel.ImageClickIndex = 1;
			this.BtnCancel.ImageIndex = 0;
			this.BtnCancel.ImageList = this.ButtonImgList;
			this.BtnCancel.IsLock = false;
			this.BtnCancel.Location = new Point(8, 0x40);
			this.BtnCancel.Name = "BtnCancel";
			this.BtnCancel.ObjectValue = null;
			this.BtnCancel.Red = 2f;
			this.BtnCancel.Size = new Size(110, 60);
			this.BtnCancel.TabIndex = 8;
			this.BtnCancel.Text = "Cancel";
			this.BtnCancel.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnCancel.Click += new EventHandler(this.BtnCancel_Click);
			this.BtnUndo.BackColor = Color.Transparent;
			this.BtnUndo.Blue = 2f;
			this.BtnUndo.Cursor = Cursors.Hand;
			this.BtnUndo.Green = 1f;
			this.BtnUndo.ImageClick = (Image) manager.GetObject("BtnUndo.ImageClick");
			this.BtnUndo.ImageClickIndex = 1;
			this.BtnUndo.ImageIndex = 0;
			this.BtnUndo.ImageList = this.ButtonImgList;
			this.BtnUndo.IsLock = false;
			this.BtnUndo.Location = new Point(120, 0x40);
			this.BtnUndo.Name = "BtnUndo";
			this.BtnUndo.ObjectValue = null;
			this.BtnUndo.Red = 2f;
			this.BtnUndo.Size = new Size(110, 60);
			this.BtnUndo.TabIndex = 9;
			this.BtnUndo.Text = "Undo";
			this.BtnUndo.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnUndo.Click += new EventHandler(this.BtnUndo_Click);
			this.BtnMessage.BackColor = Color.Transparent;
			this.BtnMessage.Blue = 1f;
			this.BtnMessage.Cursor = Cursors.Hand;
			this.BtnMessage.Green = 1.75f;
			this.BtnMessage.ImageClick = (Image) manager.GetObject("BtnMessage.ImageClick");
			this.BtnMessage.ImageClickIndex = 1;
			this.BtnMessage.ImageIndex = 0;
			this.BtnMessage.ImageList = this.ButtonImgList;
			this.BtnMessage.IsLock = false;
			this.BtnMessage.Location = new Point(0x238, 0x40);
			this.BtnMessage.Name = "BtnMessage";
			this.BtnMessage.ObjectValue = null;
			this.BtnMessage.Red = 1.75f;
			this.BtnMessage.Size = new Size(110, 60);
			this.BtnMessage.TabIndex = 10;
			this.BtnMessage.Text = "Message";
			this.BtnMessage.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnMessage.Click += new EventHandler(this.BtnMessage_Click);
			this.BtnPrintKitchen.BackColor = Color.Transparent;
			this.BtnPrintKitchen.Blue = 0.75f;
			this.BtnPrintKitchen.Cursor = Cursors.Hand;
			this.BtnPrintKitchen.Green = 1f;
			this.BtnPrintKitchen.ImageClick = (Image) manager.GetObject("BtnPrintKitchen.ImageClick");
			this.BtnPrintKitchen.ImageClickIndex = 1;
			this.BtnPrintKitchen.ImageIndex = 0;
			this.BtnPrintKitchen.ImageList = this.ButtonImgList;
			this.BtnPrintKitchen.IsLock = false;
			this.BtnPrintKitchen.Location = new Point(680, 0x40);
			this.BtnPrintKitchen.Name = "BtnPrintKitchen";
			this.BtnPrintKitchen.ObjectValue = null;
			this.BtnPrintKitchen.Red = 1f;
			this.BtnPrintKitchen.Size = new Size(110, 60);
			this.BtnPrintKitchen.TabIndex = 11;
			this.BtnPrintKitchen.Text = "Print-Kitchen / Save";
			this.BtnPrintKitchen.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnPrintKitchen.Click += new EventHandler(this.BtnPrintKitchen_Click);
			this.BtnPrintReceiptAll.BackColor = Color.Transparent;
			this.BtnPrintReceiptAll.Blue = 1.75f;
			this.BtnPrintReceiptAll.Cursor = Cursors.Hand;
			this.BtnPrintReceiptAll.Green = 1f;
			this.BtnPrintReceiptAll.ImageClick = (Image) manager.GetObject("BtnPrintReceiptAll.ImageClick");
			this.BtnPrintReceiptAll.ImageClickIndex = 1;
			this.BtnPrintReceiptAll.ImageIndex = 0;
			this.BtnPrintReceiptAll.ImageList = this.ButtonImgList;
			this.BtnPrintReceiptAll.IsLock = false;
			this.BtnPrintReceiptAll.Location = new Point(0x318, 0x40);
			this.BtnPrintReceiptAll.Name = "BtnPrintReceiptAll";
			this.BtnPrintReceiptAll.ObjectValue = null;
			this.BtnPrintReceiptAll.Red = 1.75f;
			this.BtnPrintReceiptAll.Size = new Size(110, 60);
			this.BtnPrintReceiptAll.TabIndex = 12;
			this.BtnPrintReceiptAll.Text = "Print-Receipt All";
			this.BtnPrintReceiptAll.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnPrintReceiptAll.Click += new EventHandler(this.BtnPrintReceiptAll_Click);
			this.BtnPrintReceipt.BackColor = Color.Transparent;
			this.BtnPrintReceipt.Blue = 1f;
			this.BtnPrintReceipt.Cursor = Cursors.Hand;
			this.BtnPrintReceipt.Green = 1f;
			this.BtnPrintReceipt.ImageClick = (Image) manager.GetObject("BtnPrintReceipt.ImageClick");
			this.BtnPrintReceipt.ImageClickIndex = 1;
			this.BtnPrintReceipt.ImageIndex = 0;
			this.BtnPrintReceipt.ImageList = this.ButtonImgList;
			this.BtnPrintReceipt.IsLock = false;
			this.BtnPrintReceipt.Location = new Point(0x388, 0x40);
			this.BtnPrintReceipt.Name = "BtnPrintReceipt";
			this.BtnPrintReceipt.ObjectValue = null;
			this.BtnPrintReceipt.Red = 0.75f;
			this.BtnPrintReceipt.Size = new Size(110, 60);
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
			this.groupPanel2.Font = new Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.groupPanel2.Location = new Point(0x148, 0x80);
			this.groupPanel2.Name = "groupPanel2";
			this.groupPanel2.ShowHeader = false;
			this.groupPanel2.Size = new Size(0x158, 0x270);
			this.groupPanel2.TabIndex = 15;
			this.BtnAmount.BackColor = Color.Transparent;
			this.BtnAmount.Blue = 1f;
			this.BtnAmount.Cursor = Cursors.Hand;
			this.BtnAmount.Green = 1f;
			this.BtnAmount.ImageClick = (Image) manager.GetObject("BtnAmount.ImageClick");
			this.BtnAmount.ImageClickIndex = 1;
			this.BtnAmount.ImageIndex = 0;
			this.BtnAmount.ImageList = this.NumberImgList;
			this.BtnAmount.IsLock = false;
			this.BtnAmount.Location = new Point(0x100, 0x22b);
			this.BtnAmount.Name = "BtnAmount";
			this.BtnAmount.ObjectValue = null;
			this.BtnAmount.Red = 2f;
			this.BtnAmount.Size = new Size(0x48, 60);
			this.BtnAmount.TabIndex = 20;
			this.BtnAmount.Text = "Amount";
			this.BtnAmount.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnAmount.Click += new EventHandler(this.BtnAmount_Click);
			this.BtnAmount.DoubleClick += new EventHandler(this.BtnAmount_Click);
			this.FieldCurrentInput.BackColor = Color.Black;
			this.FieldCurrentInput.Font = new Font("Tahoma", 14.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.FieldCurrentInput.ForeColor = Color.Cyan;
			this.FieldCurrentInput.Location = new Point(0x88, 0x138);
			this.FieldCurrentInput.Name = "FieldCurrentInput";
			this.FieldCurrentInput.Size = new Size(200, 40);
			this.FieldCurrentInput.TabIndex = 9;
			this.FieldCurrentInput.TextAlign = ContentAlignment.MiddleCenter;
			this.FieldInputType.BackColor = Color.Black;
			this.FieldInputType.Font = new Font("Tahoma", 14.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.FieldInputType.ForeColor = Color.Cyan;
			this.FieldInputType.Location = new Point(8, 0x138);
			this.FieldInputType.Name = "FieldInputType";
			this.FieldInputType.Size = new Size(0x80, 40);
			this.FieldInputType.TabIndex = 8;
			this.FieldInputType.Text = "Menu";
			this.FieldInputType.TextAlign = ContentAlignment.MiddleCenter;
			this.NumberKeyPad.BackColor = Color.White;
			this.NumberKeyPad.Image = (Image) manager.GetObject("NumberKeyPad.Image");
			this.NumberKeyPad.ImageClick = (Image) manager.GetObject("NumberKeyPad.ImageClick");
			this.NumberKeyPad.ImageClickIndex = 1;
			this.NumberKeyPad.ImageIndex = 0;
			this.NumberKeyPad.ImageList = this.NumberImgList;
			this.NumberKeyPad.Location = new Point(0x18, 360);
			this.NumberKeyPad.Name = "NumberKeyPad";
			this.NumberKeyPad.Size = new Size(0xe2, 0xff);
			this.NumberKeyPad.TabIndex = 7;
			this.NumberKeyPad.Text = "numberPad1";
			this.NumberKeyPad.PadClick += new smartRestaurant.Controls.NumberPad.NumberPadEventHandler(this.NumberKeyPad_PadClick);
			this.CategoryPad.AutoRefresh = true;
			this.CategoryPad.BackColor = Color.White;
			this.CategoryPad.Blue = 1f;
			this.CategoryPad.Column = 3;
			this.CategoryPad.Font = new Font("Tahoma", 12f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.CategoryPad.Green = 1f;
			this.CategoryPad.Image = (Image) manager.GetObject("CategoryPad.Image");
			this.CategoryPad.ImageClick = (Image) manager.GetObject("CategoryPad.ImageClick");
			this.CategoryPad.ImageClickIndex = 1;
			this.CategoryPad.ImageIndex = 0;
			this.CategoryPad.ImageList = this.ButtonImgList;
			this.CategoryPad.ItemStart = 0;
			this.CategoryPad.Location = new Point(5, 5);
			this.CategoryPad.Name = "CategoryPad";
			this.CategoryPad.Padding = 1;
			this.CategoryPad.Red = 1f;
			this.CategoryPad.Row = 5;
			this.CategoryPad.Size = new Size(0x14c, 0x130);
			this.CategoryPad.TabIndex = 6;
			this.CategoryPad.Text = "buttonListPad2";
			this.CategoryPad.PadClick += new ButtonListPadEventHandler(this.CategoryPad_PadClick);
			this.groupPanel3.BackColor = Color.Transparent;
			this.groupPanel3.Caption = null;
			this.groupPanel3.Controls.Add(this.OptionPad);
			this.groupPanel3.Font = new Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.groupPanel3.Location = new Point(0x2a0, 0x80);
			this.groupPanel3.Name = "groupPanel3";
			this.groupPanel3.ShowHeader = false;
			this.groupPanel3.Size = new Size(0x158, 0x270);
			this.groupPanel3.TabIndex = 0x10;
			this.OptionPad.AutoRefresh = true;
			this.OptionPad.BackColor = Color.White;
			this.OptionPad.Blue = 1f;
			this.OptionPad.Column = 3;
			this.OptionPad.Font = new Font("Tahoma", 12f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.OptionPad.Green = 1f;
			this.OptionPad.Image = (Image) manager.GetObject("OptionPad.Image");
			this.OptionPad.ImageClick = (Image) manager.GetObject("OptionPad.ImageClick");
			this.OptionPad.ImageClickIndex = 1;
			this.OptionPad.ImageIndex = 0;
			this.OptionPad.ImageList = this.ButtonImgList;
			this.OptionPad.ItemStart = 0;
			this.OptionPad.Location = new Point(5, 5);
			this.OptionPad.Name = "OptionPad";
			this.OptionPad.Padding = 1;
			this.OptionPad.Red = 1f;
			this.OptionPad.Row = 10;
			this.OptionPad.Size = new Size(0x14c, 0x261);
			this.OptionPad.TabIndex = 5;
			this.OptionPad.Text = "buttonListPad1";
			this.OptionPad.PadClick += new ButtonListPadEventHandler(this.OptionPad_PadClick);
			this.ListOrderItem.Alignment = ContentAlignment.MiddleLeft;
			this.ListOrderItem.AutoRefresh = true;
			this.ListOrderItem.BackAlterColor = Color.FromArgb(0xc0, 0xff, 0xff);
			this.ListOrderItem.BackHeaderColor = Color.FromArgb(0xff, 0xc0, 0x80);
			this.ListOrderItem.BackHeaderSelectedColor = Color.FromArgb(0xc0, 0, 0);
			this.ListOrderItem.BackNormalColor = Color.White;
			this.ListOrderItem.BackSelectedColor = Color.Blue;
			this.ListOrderItem.BindList1 = this.ListOrderCount;
			this.ListOrderItem.BindList2 = null;
			this.ListOrderItem.Border = 0;
			this.ListOrderItem.Font = new Font("Tahoma", 12f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.ListOrderItem.ForeAlterColor = Color.Black;
			this.ListOrderItem.ForeHeaderColor = Color.Black;
			this.ListOrderItem.ForeHeaderSelectedColor = Color.White;
			this.ListOrderItem.ForeNormalColor = Color.Black;
			this.ListOrderItem.ForeSelectedColor = Color.White;
			this.ListOrderItem.ItemHeight = 40;
			this.ListOrderItem.ItemWidth = 240;
			this.ListOrderItem.Location = new Point(8, 0x80);
			this.ListOrderItem.Name = "ListOrderItem";
			this.ListOrderItem.Row = 13;
			this.ListOrderItem.SelectedIndex = 0;
			this.ListOrderItem.Size = new Size(240, 520);
			this.ListOrderItem.TabIndex = 0x11;
			this.ListOrderItem.ItemClick += new smartRestaurant.Controls.ItemsList.ItemsListEventHandler(this.ListOrderItem_ItemClick);
			this.ListOrderCount.Alignment = ContentAlignment.MiddleCenter;
			this.ListOrderCount.AutoRefresh = true;
			this.ListOrderCount.BackAlterColor = Color.FromArgb(0xc0, 0xff, 0xff);
			this.ListOrderCount.BackHeaderColor = Color.FromArgb(0xff, 0xc0, 0x80);
			this.ListOrderCount.BackHeaderSelectedColor = Color.FromArgb(0xc0, 0, 0);
			this.ListOrderCount.BackNormalColor = Color.White;
			this.ListOrderCount.BackSelectedColor = Color.Blue;
			this.ListOrderCount.BindList1 = this.ListOrderItem;
			this.ListOrderCount.BindList2 = this.ListOrderItemBy;
			this.ListOrderCount.Border = 0;
			this.ListOrderCount.Font = new Font("Tahoma", 12f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.ListOrderCount.ForeAlterColor = Color.Black;
			this.ListOrderCount.ForeHeaderColor = Color.Black;
			this.ListOrderCount.ForeHeaderSelectedColor = Color.White;
			this.ListOrderCount.ForeNormalColor = Color.Black;
			this.ListOrderCount.ForeSelectedColor = Color.White;
			this.ListOrderCount.ItemHeight = 40;
			this.ListOrderCount.ItemWidth = 40;
			this.ListOrderCount.Location = new Point(0xf8, 0x80);
			this.ListOrderCount.Name = "ListOrderCount";
			this.ListOrderCount.Row = 13;
			this.ListOrderCount.SelectedIndex = 0;
			this.ListOrderCount.Size = new Size(40, 520);
			this.ListOrderCount.TabIndex = 0x24;
			this.ListOrderCount.ItemClick += new smartRestaurant.Controls.ItemsList.ItemsListEventHandler(this.ListOrderItem_ItemClick);
			this.ListOrderItemBy.Alignment = ContentAlignment.MiddleCenter;
			this.ListOrderItemBy.AutoRefresh = true;
			this.ListOrderItemBy.BackAlterColor = Color.FromArgb(0xc0, 0xff, 0xff);
			this.ListOrderItemBy.BackHeaderColor = Color.FromArgb(0xff, 0xc0, 0x80);
			this.ListOrderItemBy.BackHeaderSelectedColor = Color.FromArgb(0xc0, 0, 0);
			this.ListOrderItemBy.BackNormalColor = Color.White;
			this.ListOrderItemBy.BackSelectedColor = Color.Blue;
			this.ListOrderItemBy.BindList1 = this.ListOrderCount;
			this.ListOrderItemBy.BindList2 = null;
			this.ListOrderItemBy.Border = 0;
			this.ListOrderItemBy.Font = new Font("Tahoma", 12f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.ListOrderItemBy.ForeAlterColor = Color.Black;
			this.ListOrderItemBy.ForeHeaderColor = Color.Black;
			this.ListOrderItemBy.ForeHeaderSelectedColor = Color.White;
			this.ListOrderItemBy.ForeNormalColor = Color.Black;
			this.ListOrderItemBy.ForeSelectedColor = Color.White;
			this.ListOrderItemBy.ItemHeight = 40;
			this.ListOrderItemBy.ItemWidth = 40;
			this.ListOrderItemBy.Location = new Point(0x120, 0x80);
			this.ListOrderItemBy.Name = "ListOrderItemBy";
			this.ListOrderItemBy.Row = 13;
			this.ListOrderItemBy.SelectedIndex = 0;
			this.ListOrderItemBy.Size = new Size(40, 520);
			this.ListOrderItemBy.TabIndex = 0x22;
			this.ListOrderItemBy.ItemClick += new smartRestaurant.Controls.ItemsList.ItemsListEventHandler(this.ListOrderItem_ItemClick);
			this.BtnDown.BackColor = Color.Transparent;
			this.BtnDown.Blue = 2f;
			this.BtnDown.Cursor = Cursors.Hand;
			this.BtnDown.Green = 1f;
			this.BtnDown.ImageClick = (Image) manager.GetObject("BtnDown.ImageClick");
			this.BtnDown.ImageClickIndex = 5;
			this.BtnDown.ImageIndex = 4;
			this.BtnDown.ImageList = this.ButtonImgList;
			this.BtnDown.IsLock = false;
			this.BtnDown.Location = new Point(0xd0, 0x2b4);
			this.BtnDown.Name = "BtnDown";
			this.BtnDown.ObjectValue = null;
			this.BtnDown.Red = 2f;
			this.BtnDown.Size = new Size(110, 60);
			this.BtnDown.TabIndex = 0x13;
			this.BtnDown.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnDown.Click += new EventHandler(this.BtnDown_Click);
			this.BtnDown.DoubleClick += new EventHandler(this.BtnDown_Click);
			this.BtnUp.BackColor = Color.Transparent;
			this.BtnUp.Blue = 2f;
			this.BtnUp.Cursor = Cursors.Hand;
			this.BtnUp.Green = 1f;
			this.BtnUp.ImageClick = (Image) manager.GetObject("BtnUp.ImageClick");
			this.BtnUp.ImageClickIndex = 3;
			this.BtnUp.ImageIndex = 2;
			this.BtnUp.ImageList = this.ButtonImgList;
			this.BtnUp.IsLock = false;
			this.BtnUp.Location = new Point(0x10, 0x2b4);
			this.BtnUp.Name = "BtnUp";
			this.BtnUp.ObjectValue = null;
			this.BtnUp.Red = 2f;
			this.BtnUp.Size = new Size(110, 60);
			this.BtnUp.TabIndex = 0x12;
			this.BtnUp.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnUp.Click += new EventHandler(this.BtnUp_Click);
			this.BtnUp.DoubleClick += new EventHandler(this.BtnUp_Click);
			this.BtnMoveItem.BackColor = Color.Transparent;
			this.BtnMoveItem.Blue = 2f;
			this.BtnMoveItem.Cursor = Cursors.Hand;
			this.BtnMoveItem.Green = 1f;
			this.BtnMoveItem.ImageClick = (Image) manager.GetObject("BtnMoveItem.ImageClick");
			this.BtnMoveItem.ImageClickIndex = 1;
			this.BtnMoveItem.ImageIndex = 0;
			this.BtnMoveItem.ImageList = this.ButtonImgList;
			this.BtnMoveItem.IsLock = false;
			this.BtnMoveItem.Location = new Point(0xe8, 0x40);
			this.BtnMoveItem.Name = "BtnMoveItem";
			this.BtnMoveItem.ObjectValue = null;
			this.BtnMoveItem.Red = 2f;
			this.BtnMoveItem.Size = new Size(110, 60);
			this.BtnMoveItem.TabIndex = 20;
			this.BtnMoveItem.Text = "Move Item";
			this.BtnMoveItem.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnMoveItem.Click += new EventHandler(this.BtnMoveItem_Click);
			this.PanCustName.BackColor = Color.Transparent;
			this.PanCustName.Caption = null;
			this.PanCustName.Controls.Add(this.FieldCustName);
			this.PanCustName.Font = new Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.PanCustName.Location = new Point(0xf8, 0);
			this.PanCustName.Name = "PanCustName";
			this.PanCustName.ShowHeader = false;
			this.PanCustName.Size = new Size(200, 0x3a);
			this.PanCustName.TabIndex = 0x16;
			this.FieldCustName.Cursor = Cursors.Hand;
			this.FieldCustName.Location = new Point(1, 1);
			this.FieldCustName.Name = "FieldCustName";
			this.FieldCustName.Size = new Size(0xc7, 0x38);
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
			this.BtnSearch.Location = new Point(0x1c1, 0);
			this.BtnSearch.Name = "BtnSearch";
			this.BtnSearch.ObjectValue = null;
			this.BtnSearch.Red = 1f;
			this.BtnSearch.Size = new Size(110, 60);
			this.BtnSearch.TabIndex = 0x17;
			this.BtnSearch.Text = "Search";
			this.BtnSearch.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnSearch.Click += new EventHandler(this.BtnSearch_Click);
			this.BtnServeItem.BackColor = Color.Transparent;
			this.BtnServeItem.Blue = 2f;
			this.BtnServeItem.Cursor = Cursors.Hand;
			this.BtnServeItem.Green = 1f;
			this.BtnServeItem.ImageClick = (Image) manager.GetObject("BtnServeItem.ImageClick");
			this.BtnServeItem.ImageClickIndex = 1;
			this.BtnServeItem.ImageIndex = 0;
			this.BtnServeItem.ImageList = this.ButtonImgList;
			this.BtnServeItem.IsLock = false;
			this.BtnServeItem.Location = new Point(0x158, 0x40);
			this.BtnServeItem.Name = "BtnServeItem";
			this.BtnServeItem.ObjectValue = null;
			this.BtnServeItem.Red = 2f;
			this.BtnServeItem.Size = new Size(110, 60);
			this.BtnServeItem.TabIndex = 0x18;
			this.BtnServeItem.Text = "Serve Item";
			this.BtnServeItem.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnServeItem.Click += new EventHandler(this.BtnServeItem_Click);
			this.LblPageID.BackColor = Color.Transparent;
			this.LblPageID.Font = new Font("Tahoma", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.LblPageID.ForeColor = Color.FromArgb(0x67, 0x8a, 0xc6);
			this.LblPageID.Location = new Point(0x330, 0x2f0);
			this.LblPageID.Name = "LblPageID";
			this.LblPageID.Size = new Size(0xc0, 0x17);
			this.LblPageID.TabIndex = 0x21;
			this.LblPageID.Text = "| STTO011";
			this.LblPageID.TextAlign = ContentAlignment.TopRight;
			this.LblCopyright.BackColor = Color.Transparent;
			this.LblCopyright.Font = new Font("Tahoma", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.LblCopyright.ForeColor = Color.FromArgb(0x67, 0x8a, 0xc6);
			this.LblCopyright.Location = new Point(8, 0x2f0);
			this.LblCopyright.Name = "LblCopyright";
			this.LblCopyright.Size = new Size(280, 0x10);
			this.LblCopyright.TabIndex = 0x23;
			this.LblCopyright.Text = "Copyright (c) 2004. All rights reserved.";
			this.LblTotalText.BackColor = Color.FromArgb(0xff, 0xff, 0x80);
			this.LblTotalText.Font = new Font("Tahoma", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.LblTotalText.Location = new Point(8, 0x288);
			this.LblTotalText.Name = "LblTotalText";
			this.LblTotalText.Size = new Size(0xd0, 40);
			this.LblTotalText.TabIndex = 0x25;
			this.LblTotalText.Text = "Total (Before Tax)";
			this.LblTotalText.TextAlign = ContentAlignment.MiddleLeft;
			this.LblTotalValue.BackColor = Color.FromArgb(0xff, 0xff, 0x80);
			this.LblTotalValue.Font = new Font("Tahoma", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.LblTotalValue.ForeColor = Color.FromArgb(0, 0, 0xc0);
			this.LblTotalValue.Location = new Point(0xd8, 0x288);
			this.LblTotalValue.Name = "LblTotalValue";
			this.LblTotalValue.Size = new Size(0x70, 40);
			this.LblTotalValue.TabIndex = 0x26;
			this.LblTotalValue.Text = "0.00";
			this.LblTotalValue.TextAlign = ContentAlignment.MiddleRight;
			this.AutoScaleBaseSize = new Size(6, 15);
			base.ClientSize = new Size(0x3fc, 0x2fc);
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
			OrderBill sourceBill = null;
			OrderBillItem selectedItem = null;
			if (this.moveItem)
			{
				sourceBill = this.selectedBill;
				selectedItem = this.selectedItem;
			}
			if (e.Item.Value is OrderBill)
			{
				this.selectedBill = (OrderBill) e.Item.Value;
				this.selectedItem = null;
				this.StartInputMenu();
			}
			else if (e.Item.Value is OrderBillItem)
			{
				this.selectedItem = (OrderBillItem) e.Item.Value;
				for (int i = 0; (i >= 0) && (i < this.orderInfo.Bills.Length); i++)
				{
					if (this.orderInfo.Bills[i].Items != null)
					{
						for (int j = 0; j < this.orderInfo.Bills[i].Items.Length; j++)
						{
							if (this.orderInfo.Bills[i].Items[j] == this.selectedItem)
							{
								this.selectedBill = this.orderInfo.Bills[i];
								i = -2;
								break;
							}
						}
					}
				}
				if (this.selectedBill.CloseBillDate != AppParameter.MinDateTime)
				{
					this.StartInputMenu();
				}
				else
				{
					this.StartInputOption();
				}
			}
			if (selectedItem != null)
			{
				if (sourceBill != this.selectedBill)
				{
					if (OrderManagement.MoveOrderBillItem(sourceBill, this.selectedBill, selectedItem))
					{
						this.isChanged = true;
						this.UpdateOrderGrid();
					}
				}
				else
				{
					this.UpdateOrderGrid();
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
			if ((((this.inputState == 0) || (this.inputState == 2)) && ((this.inputState != 0) || ((this.inputValue != null) && (this.inputValue.Length != 0)))) && (e.Value != null))
			{
				int num;
				int num2;
				int num3;
				smartRestaurant.MenuService.MenuItem menuItemFromID;
				try
				{
					if (this.inputState == 2)
					{
						string[] strArray = e.Value.Split(new char[] { ':' });
						num2 = int.Parse(strArray[0]);
						num3 = int.Parse(strArray[1]);
						num = 0;
					}
					else
					{
						num2 = num3 = 0;
						num = int.Parse(e.Value);
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
						menuItemFromID = this.selectedType.MenuItems[num];
						this.AddOrderBillItem(menuItemFromID);
						return;

					case 1:
						return;

					case 2:
					{
						string str = num2 + ":";
						string str2 = num3.ToString();
						if (this.selectedItem.ItemChoices == null)
						{
							menuItemFromID = MenuManagement.GetMenuItemFromID(this.selectedItem.MenuID);
							this.selectedItem.ItemChoices = new OrderItemChoice[menuItemFromID.MenuDefaults.Length];
							for (int k = 0; k < menuItemFromID.MenuDefaults.Length; k++)
							{
								this.selectedItem.ItemChoices[k] = new OrderItemChoice();
								this.selectedItem.ItemChoices[k].OptionID = menuItemFromID.MenuDefaults[k].OptionID;
								this.selectedItem.ItemChoices[k].ChoiceID = menuItemFromID.MenuDefaults[k].DefaultChoiceID;
							}
							this.selectedItem.DefaultOption = false;
						}
						for (int i = 0; i < this.OptionPad.Items.Count; i++)
						{
							ButtonItem item2 = (ButtonItem) this.OptionPad.Items[i];
							if (item2.Value.Substring(0, str.Length) == str)
							{
								if (item2.Value.Substring(str.Length) == str2)
								{
									this.OptionPad.SetMatrix(i, 2f, 1f, 2f);
								}
								else
								{
									this.OptionPad.SetMatrix(i, 1f, 1f, 1f);
								}
							}
						}
						for (int j = 0; j < this.selectedItem.ItemChoices.Length; j++)
						{
							if (this.selectedItem.ItemChoices[j].OptionID == num2)
							{
								this.selectedItem.ItemChoices[j].ChoiceID = num3;
								this.selectedItem.DefaultOption = false;
								this.selectedItem.ChangeFlag = true;
								this.isChanged = true;
								return;
							}
						}
						return;
					}
				}
			}
		}

		private void SaveBeforePrintReceipt()
		{
			string str;
			smartRestaurant.OrderService.OrderService service = new smartRestaurant.OrderService.OrderService();
			if (!this.takeOutMode)
			{
				str = service.SendOrderPrint(this.orderInfo, 0, null, false);
				int orderID = 0;
				try
				{
					orderID = int.Parse(str);
					service.SetTableReference(orderID, this.tableIDList);
				}
				catch (Exception exception)
				{
					MessageBox.Show(exception.ToString());
					orderID = 0;
				}
			}
			else
			{
				str = service.SendOrderPrint(this.orderInfo, this.takeOutCustID, this.FieldCustName.Text, false);
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
			for (int i = 0; i < this.menuTypes.Length; i++)
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
			smartRestaurant.MenuService.MenuItem menuItemFromID = MenuManagement.GetMenuItemFromID(this.selectedItem.MenuID);
			if (menuItemFromID == null)
			{
				this.StartInputMenu();
			}
			else
			{
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
					for (int i = 0; i < this.menuOptions.Length; i++)
					{
						int length = 0;
						if (this.selectedItem.DefaultOption)
						{
							if (menuItemFromID.MenuDefaults != null)
							{
								length = menuItemFromID.MenuDefaults.Length;
							}
						}
						else if (this.selectedItem.ItemChoices != null)
						{
							length = this.selectedItem.ItemChoices.Length;
						}
						for (int j = 0; j < length; j++)
						{
							int optionID;
							if (this.selectedItem.DefaultOption)
							{
								optionID = menuItemFromID.MenuDefaults[j].OptionID;
							}
							else
							{
								optionID = this.selectedItem.ItemChoices[j].OptionID;
							}
							if (this.menuOptions[i].OptionID == optionID)
							{
								for (int k = 0; k < this.menuOptions[i].OptionChoices.Length; k++)
								{
									ButtonItem item2 = new ButtonItem(this.menuOptions[i].OptionChoices[k].ChoiceName, this.menuOptions[i].OptionChoices[k].OptionID.ToString() + ":" + this.menuOptions[i].OptionChoices[k].ChoiceID.ToString());
									if ((j % 2) == 0)
									{
										item2.ForeColor = Color.Red;
									}
									else
									{
										item2.ForeColor = Color.Blue;
									}
									this.OptionPad.Items.Add(item2);
									if (this.selectedItem.DefaultOption)
									{
										if (this.menuOptions[i].OptionChoices[k].ChoiceID == menuItemFromID.MenuDefaults[j].DefaultChoiceID)
										{
											this.OptionPad.SetMatrix(this.OptionPad.Items.Count - 1, 2f, 1f, 2f);
										}
									}
									else if ((this.selectedItem.ItemChoices != null) && (this.menuOptions[i].OptionChoices[k].ChoiceID == this.selectedItem.ItemChoices[j].ChoiceID))
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
			if (this.isChanged)
			{
				if (this.takeOutMode)
				{
					if (((this.FieldCustName.Text == null) || (this.FieldCustName.Text == "")) || (this.FieldCustName.Text == FIELD_CUST_TEXT))
					{
						this.BtnPrintKitchen.Enabled = false;
					}
					else
					{
						this.BtnPrintKitchen.Enabled = true;
					}
				}
				else
				{
					this.BtnPrintKitchen.Enabled = true;
				}
			}
			else
			{
				this.BtnPrintKitchen.Enabled = false;
			}
			if (((this.orderInfo.OrderID != 0) && !this.isChanged) && ((this.selectedBill != null) && (this.selectedBill.CloseBillDate == AppParameter.MinDateTime)))
			{
				this.BtnPrintReceipt.Enabled = true;
				this.BtnPrintReceiptAll.Enabled = true;
			}
			else
			{
				this.BtnPrintReceipt.Enabled = false;
				this.BtnPrintReceiptAll.Enabled = false;
			}
		}

		public override void UpdateForm()
		{
			this.LoadMenus();
			this.takeOutMode = this.tableInfo.TableID == 0;
			smartRestaurant.OrderService.OrderService service = new smartRestaurant.OrderService.OrderService();
			if (!this.takeOrderResume)
			{
				if (!this.takeOutMode)
				{
					this.orderInfo = service.GetOrder(this.tableInfo.TableID);
					if (this.orderInfo != null)
					{
						if (this.tableInfo.TableID != this.orderInfo.TableID)
						{
							this.tableInfo = this.tabService.GetTableInformation(this.orderInfo.TableID);
						}
						this.tableIDList = service.GetTableReference(this.orderInfo.OrderID);
					}
					else
					{
						this.tableIDList = null;
					}
				}
				else if (this.takeOutOrderID > 0)
				{
					this.orderInfo = service.GetOrderByOrderID(this.takeOutOrderID);
					this.tableIDList = null;
				}
				else
				{
					this.orderInfo = null;
					this.tableIDList = null;
				}
				this.isChanged = false;
			}
			else if (!this.takeOutMode)
			{
				this.orderInfo = service.GetOrder(this.tableInfo.TableID);
			}
			if (this.orderInfo != null)
			{
				this.guestNumber = this.orderInfo.NumberOfGuest;
				this.billNumber = this.orderInfo.Bills.Length;
			}
			else if (this.guestNumber <= 0)
			{
				this.guestNumber = this.billNumber = 1;
			}
			if (AppParameter.IsDemo())
			{
				this.ListOrderItem.ItemWidth = 240;
				this.ListOrderCount.Left = 0xf8;
				this.ListOrderItem.Row = 14;
				this.ListOrderCount.Row = 14;
				this.ListOrderItemBy.Row = 14;
				this.ListOrderItemBy.Visible = true;
				this.LblTotalText.Visible = false;
				this.LblTotalValue.Visible = false;
				this.LblGuest.Text = "Guest";
			}
			else
			{
				if (AppParameter.ShowOrderItemPrice)
				{
					this.ListOrderItem.ItemWidth = 240;
					this.ListOrderCount.Left = 0xf8;
					this.ListOrderCount.ItemWidth = 80;
					this.ListOrderItem.Row = 13;
					this.ListOrderCount.Row = 13;
					this.LblTotalText.Visible = true;
					this.LblTotalValue.Visible = true;
				}
				else
				{
					this.ListOrderItem.ItemWidth = 280;
					this.ListOrderCount.Left = 0x120;
					this.ListOrderCount.ItemWidth = 40;
					this.ListOrderItem.Row = 14;
					this.ListOrderCount.Row = 14;
					this.LblTotalText.Visible = false;
					this.LblTotalValue.Visible = false;
				}
				this.ListOrderItemBy.Visible = false;
				this.LblGuest.Text = "Seat";
			}
			this.ListOrderCount.Left = this.ListOrderItem.Left + this.ListOrderItem.ItemWidth;
			this.selectedBill = null;
			this.selectedItem = null;
			this.selectedType = null;
			if ((this.orderInfo != null) && (this.orderInfo.Bills != null))
			{
				for (int i = 0; i < this.orderInfo.Bills.Length; i++)
				{
					if (this.orderInfo.Bills[i].CloseBillDate == AppParameter.MinDateTime)
					{
						this.selectedBill = this.orderInfo.Bills[i];
						break;
					}
				}
			}
			this.LblPageID.Text = "Employee ID:" + ((MainForm) base.MdiParent).UserID + " | ";
			if (this.takeOutMode)
			{
				this.LblPageID.Text = this.LblPageID.Text + "STTO021";
			}
			else
			{
				this.LblPageID.Text = this.LblPageID.Text + "STTO011";
			}
			this.PanCustName.Visible = this.takeOutMode;
			this.BtnSearch.Visible = this.takeOutMode;
			this.OptionPad.AutoRefresh = false;
			this.OptionPad.Red = this.OptionPad.Green = this.OptionPad.Blue = 1f;
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
			if ((!this.moveItem && (this.selectedItem != null)) && (this.selectedBill.CloseBillDate == AppParameter.MinDateTime))
			{
				if (this.selectedItem.ServeTime == AppParameter.MinDateTime)
				{
					this.BtnCancel.Enabled = !OrderManagement.IsCancel(this.selectedItem);
					this.BtnUndo.Enabled = OrderManagement.IsCancel(this.selectedItem);
					this.BtnServeItem.Enabled = !OrderManagement.IsCancel(this.selectedItem);
					this.BtnMessage.Enabled = !OrderManagement.IsCancel(this.selectedItem);
					this.BtnAmount.Enabled = !OrderManagement.IsCancel(this.selectedItem);
				}
				this.BtnMoveItem.Enabled = !OrderManagement.IsCancel(this.selectedItem) && (this.orderInfo.Bills.Length > 1);
			}
			else
			{
				this.BtnCancel.Enabled = false;
				this.BtnUndo.Enabled = false;
				this.BtnMoveItem.Enabled = false;
				this.BtnServeItem.Enabled = false;
				this.BtnMessage.Enabled = false;
				this.BtnAmount.Enabled = false;
			}
			this.BtnUp.Enabled = this.ListOrderItem.CanUp;
			this.BtnDown.Enabled = this.ListOrderItem.CanDown;
		}

		private void UpdateOrderGrid()
		{
			StringBuilder builder = new StringBuilder();
			this.ListOrderItem.AutoRefresh = false;
			this.ListOrderCount.AutoRefresh = false;
			this.ListOrderItemBy.AutoRefresh = false;
			this.ListOrderItem.Items.Clear();
			this.ListOrderCount.Items.Clear();
			this.ListOrderItemBy.Items.Clear();
			if ((this.orderInfo == null) || (this.orderInfo.Bills.Length != this.billNumber))
			{
				this.orderInfo = OrderManagement.CreateOrderObject(this.orderInfo, this.employeeID, this.tableInfo, this.guestNumber, this.billNumber);
			}
			OrderBill[] bills = this.orderInfo.Bills;
			if (this.selectedBill == null)
			{
				this.selectedBill = bills[0];
			}
			double num3 = 0.0;
			double price = 0.0;
			for (int i = 0; i < bills.Length; i++)
			{
				builder.Length = 0;
				if (AppParameter.IsDemo())
				{
					builder.Append("Bill #");
				}
				else
				{
					builder.Append("Seat #");
				}
				builder.Append((int) (i + 1));
				if (bills[i].CloseBillDate != AppParameter.MinDateTime)
				{
					builder.Append(" (Closed)");
				}
				DataItem item = new DataItem(builder.ToString(), bills[i], true);
				this.ListOrderItem.Items.Add(item);
				item = new DataItem("Amt.", bills[i], true);
				this.ListOrderCount.Items.Add(item);
				item = new DataItem("Emp#", bills[i], true);
				this.ListOrderItemBy.Items.Add(item);
				if ((this.selectedBill == bills[i]) && (this.selectedItem == null))
				{
					this.ListOrderItem.SelectedIndex = this.ListOrderItem.Items.Count - 1;
					this.ListOrderCount.SelectedIndex = this.ListOrderCount.Items.Count - 1;
					this.ListOrderItemBy.SelectedIndex = this.ListOrderItemBy.Items.Count - 1;
				}
				OrderBillItem[] items = bills[i].Items;
				if (items != null)
				{
					for (int j = 0; j < items.Length; j++)
					{
						bool flag = OrderManagement.IsCancel(items[j]);
						item = new DataItem(OrderManagement.OrderBillItemDisplayString(items[j]), items[j], false);
						if (flag)
						{
							item.Strike = true;
						}
						this.ListOrderItem.Items.Add(item);
						item = new DataItem(OrderManagement.OrderBillCountDisplayString(items[j], ref price), items[j], false);
						if (flag)
						{
							item.Strike = true;
						}
						this.ListOrderCount.Items.Add(item);
						item = new DataItem(items[j].EmployeeID.ToString(), items[j], false);
						if (flag)
						{
							item.Strike = true;
						}
						this.ListOrderItemBy.Items.Add(item);
						if (this.selectedItem == items[j])
						{
							this.ListOrderItem.SelectedIndex = this.ListOrderItem.Items.Count - 1;
							this.ListOrderCount.SelectedIndex = this.ListOrderCount.Items.Count - 1;
							this.ListOrderItemBy.SelectedIndex = this.ListOrderItemBy.Items.Count - 1;
						}
						if (!flag)
						{
							num3 += price;
						}
					}
				}
			}
			this.LblTotalValue.Text = num3.ToString("N");
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

		// Properties
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
	}


 

}
