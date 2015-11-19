using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using smartRestaurant.Controls;
using smartRestaurant.Data;
using smartRestaurant.TableService;
using smartRestaurant.CustomerService;
using smartRestaurant.OrderService;
using smartRestaurant.Utils;
using System.Resources;

namespace smartRestaurant
{
	/// <summary>
	/// Summary description for TakeOutForm.
	/// </summary>
	public class TakeOutForm : SmartForm
	{
		// Fields
		private ImageButton BtnCancel;
		private ImageButton BtnClear;
		private ImageButton BtnCustDown;
		private ImageButton BtnCustList;
		private ImageButton BtnCustSearch;
		private ImageButton BtnCustUp;
		private ImageButton BtnDelete;
		private ImageButton BtnDown;
		private ImageButton BtnEditOrder;
		private ImageButton BtnExit;
		private ImageButton BtnKBAddress;
		private ImageButton BtnKBFName;
		private ImageButton BtnKBLName;
		private ImageButton BtnKBMName;
		private ImageButton BtnMain;
		private ImageButton BtnManager;
		private ImageButton BtnMemo;
		private ImageButton BtnPay;
		private ImageButton BtnSave;
		private ImageButton BtnTakeOrder;
		private ImageButton BtnUp;
		private ImageList ButtonImgList;
		private ImageList ButtonLiteImgList;
		private IContainer components;
		private CustomerService.CustomerInformation[] custList;
		private int employeeID;
		private TextBox FieldAddress;
		private Label FieldArea;
		private TextBox FieldFName;
		private TextBox FieldLName;
		private TextBox FieldMName;
		private TextBox FieldOtherRoad;
		private TextBox FieldPhone;
		private TextBox FieldRoad;
		private const int INPUT_ADDRESS = 3;
		private const int INPUT_FIRSTNAME = 0;
		private const int INPUT_LASTNAME = 2;
		private const int INPUT_MEMO = 4;
		private const int INPUT_MIDDLENAME = 1;
		private int inputState;
		private ImageList KeyboardImgList;
		private Label LblAddress;
		private Label LblArea;
		private Label LblCopyright;
		private Label LblFName;
		private Label LblHeaderName;
		private Label LblHeaderOrder;
		private Label LblHeaderPhone;
		private Label LblHeaderTime;
		private Label LblLName;
		private Label LblMName;
		private Label LblOtherRoad;
		private Label LblPageID;
		private Label LblPhone;
		private Label LblRoad;
		private ItemsList ListCustName;
		private ItemsList ListCustPhone;
		private ItemsList ListOrderQueue;
		private ItemsList ListOrderTime;
		private ListBox LstRoad;
		private string memo;
		private ImageList NumberImgList;
		private NumberPad NumberKeyPad;
		private bool orderListSortFlag;
		private GroupPanel PanCustField;
		private bool roadIgnoreFlag;
		private bool roadLstIgnoreFlag;
		private CustomerService.CustomerInformation selectedCust;
		private Road selectedRoad;
		private TakeOutInformation selectedTakeOut;
		private TableInformation tableInfo;
		private bool takeOrderMode;
		private TakeOutInformation[] takeOutList;

		// Methods
		public TakeOutForm()
		{
			this.InitializeComponent();
			this.selectedCust = null;
			this.takeOrderMode = false;
			this.orderListSortFlag = true;
		}

		private void BtnCancel_Click(object sender, EventArgs e)
		{
			if (this.selectedTakeOut == null)
			{
				MessageForm.Show("Cancel", "Please select order first.");
			}
			else
			{
				smartRestaurant.OrderService.OrderService service = new smartRestaurant.OrderService.OrderService();
				OrderInformation orderByOrderID = service.GetOrderByOrderID(this.selectedTakeOut.OrderID);
				if (orderByOrderID == null)
				{
					MessageBox.Show("Can't load order information for this order.");
				}
				else if ((orderByOrderID.Bills == null) || (orderByOrderID.Bills.Length <= 0))
				{
					MessageBox.Show("No order item in this order.");
				}
				else if (OrderManagement.CancelOrder(orderByOrderID, this.employeeID))
				{
					service.SendOrder(orderByOrderID, 0, null);
					this.selectedTakeOut = null;
					this.UpdateTakeOutList();
					this.UpdateOrderButton();
				}
			}
		}

		private void BtnClear_Click(object sender, EventArgs e)
		{
			this.ClearCustomer();
			this.UpdateMemoButton();
			this.UpdateCustomerButton();
		}

		private void BtnCustDown_Click(object sender, EventArgs e)
		{
			this.ListCustPhone.Down(5);
			this.UpdateCustomerButton();
		}

		private void BtnCustList_Click(object sender, EventArgs e)
		{
			this.custList = new smartRestaurant.CustomerService.CustomerService().GetCustomers();
			if (this.selectedCust != null)
			{
				for (int i = 0; i < this.custList.Length; i++)
				{
					if (this.custList[i].CustID == this.selectedCust.CustID)
					{
						this.selectedCust = this.custList[i];
						break;
					}
				}
			}
			this.UpdateCustomerList();
		}

		private void BtnCustSearch_Click(object sender, EventArgs e)
		{
			if (((this.FieldPhone.Text == "") && (this.FieldFName.Text == "")) && ((this.FieldMName.Text == "") && (this.FieldLName.Text == "")))
			{
				MessageBox.Show("Please fill phone number, first name, middle name, or last name.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
			else
			{
				this.custList = new smartRestaurant.CustomerService.CustomerService().SearchCustomers(this.FieldPhone.Text, this.FieldFName.Text, this.FieldMName.Text, this.FieldLName.Text);
				if ((this.custList != null) && (this.custList.Length == 1))
				{
					this.selectedCust = this.custList[0];
					this.UpdateCustomerField();
				}
				this.UpdateCustomerList();
			}
		}

		private void BtnCustUp_Click(object sender, EventArgs e)
		{
			this.ListCustPhone.Up(5);
			this.UpdateCustomerButton();
		}

		private void BtnDelete_Click(object sender, EventArgs e)
		{
			if (this.selectedCust == null)
			{
				MessageBox.Show("Please select from customer list first.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
			else
			{
				string text = new smartRestaurant.CustomerService.CustomerService().DeleteCustomer(this.selectedCust.CustID);
				if (text != null)
				{
					MessageBox.Show(text);
				}
				else
				{
					this.ClearCustomer();
					this.BtnCustList_Click(null, null);
				}
			}
		}

		private void BtnDown_Click(object sender, EventArgs e)
		{
			this.ListOrderQueue.Down(5);
			this.UpdateOrderButton();
		}

		private void BtnEditOrder_Click(object sender, EventArgs e)
		{
			if (this.selectedTakeOut != null)
			{
				StringBuilder builder = new StringBuilder();
				builder.Append(this.selectedTakeOut.CustInfo.FirstName);
				if ((this.selectedTakeOut.CustInfo.MiddleName != null) && (this.selectedTakeOut.CustInfo.MiddleName != ""))
				{
					builder.Append(" ");
					builder.Append(this.selectedTakeOut.CustInfo.MiddleName);
				}
				if ((this.selectedTakeOut.CustInfo.LastName != null) && (this.selectedTakeOut.CustInfo.LastName != ""))
				{
					builder.Append(" ");
					builder.Append(this.selectedTakeOut.CustInfo.LastName);
				}
				((MainForm) base.MdiParent).ShowTakeOrderForm(this.tableInfo, this.selectedTakeOut.OrderID, this.selectedTakeOut.CustInfo.CustID, builder.ToString());
			}
		}

		private void BtnExit_Click(object sender, EventArgs e)
		{
			UserProfile.CheckLogout(((MainForm) base.MdiParent).User.UserID);
			((MainForm) base.MdiParent).User = null;
			((MainForm) base.MdiParent).ShowLoginForm();
		}

		private void BtnKBAddress_Click(object sender, EventArgs e)
		{
			this.FieldAddress.Focus();
			this.inputState = 3;
			this.CheckKeyboardOutput(KeyboardForm.Show("Address", this.FieldAddress.Text));
		}

		private void BtnKBFName_Click(object sender, EventArgs e)
		{
			this.FieldFName.Focus();
			this.inputState = 0;
			this.CheckKeyboardOutput(KeyboardForm.Show("First Name", this.FieldFName.Text));
		}

		private void BtnKBLName_Click(object sender, EventArgs e)
		{
			this.FieldLName.Focus();
			this.inputState = 2;
			this.CheckKeyboardOutput(KeyboardForm.Show("Last Name", this.FieldLName.Text));
		}

		private void BtnKBMName_Click(object sender, EventArgs e)
		{
			this.FieldMName.Focus();
			this.inputState = 1;
			this.CheckKeyboardOutput(KeyboardForm.Show("Middle Name", this.FieldMName.Text));
		}

		private void BtnMain_Click(object sender, EventArgs e)
		{
			if (this.TakeOrderMode)
			{
				((MainForm) base.MdiParent).ShowTakeOrderForm(0, null);
			}
			else
			{
				((MainForm) base.MdiParent).ShowMainMenuForm(true);
			}
		}

		private void BtnManager_Click(object sender, EventArgs e)
		{
			((MainForm) base.MdiParent).ShowSalesForm();
		}

		private void BtnMemo_Click(object sender, EventArgs e)
		{
			this.inputState = 4;
			this.CheckKeyboardOutput(KeyboardForm.Show("Memo", this.memo));
			this.UpdateMemoButton();
		}

		private void BtnPay_Click(object sender, EventArgs e)
		{
			if (this.selectedTakeOut == null)
			{
				MessageForm.Show("Pay", "Please select order first.");
			}
			else
			{
				OrderInformation orderByOrderID = new smartRestaurant.OrderService.OrderService().GetOrderByOrderID(this.selectedTakeOut.OrderID);
				if (orderByOrderID == null)
				{
					MessageBox.Show("Can't load order information for this order.");
				}
				else if ((orderByOrderID.Bills == null) || (orderByOrderID.Bills.Length <= 0))
				{
					MessageBox.Show("No order item in this order.");
				}
				else
				{
					((MainForm) base.MdiParent).ShowPrintReceiptForm(this.tableInfo, orderByOrderID, orderByOrderID.Bills[0]);
				}
			}
		}

		private void BtnSave_Click(object sender, EventArgs e)
		{
			if (this.FieldFName.Text == string.Empty)
			{
				MessageBox.Show("Please fill first name.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
			else
			{
				smartRestaurant.CustomerService.CustomerInformation selectedCust;
				if (this.selectedCust != null)
				{
					selectedCust = this.selectedCust;
				}
				else
				{
					selectedCust = new smartRestaurant.CustomerService.CustomerInformation();
					selectedCust.CustID = 0;
				}
				selectedCust.Telephone = this.FieldPhone.Text;
				selectedCust.FirstName = this.FieldFName.Text;
				selectedCust.MiddleName = this.FieldMName.Text;
				selectedCust.LastName = this.FieldLName.Text;
				selectedCust.Address = this.FieldAddress.Text;
				selectedCust.RoadID = (this.selectedRoad != null) ? this.selectedRoad.RoadID : 0;
				selectedCust.OtherRoadName = this.FieldOtherRoad.Text;
				selectedCust.Description = this.memo;
				string text = new smartRestaurant.CustomerService.CustomerService().SetCustomer(selectedCust);
				if (text != null)
				{
					MessageBox.Show(text);
				}
				else
				{
					this.BtnCustSearch_Click(null, null);
				}
			}
		}

		private void BtnTakeOrder_Click(object sender, EventArgs e)
		{
			int custID;
			if (this.selectedCust != null)
			{
				custID = this.selectedCust.CustID;
			}
			else
			{
				custID = 0;
			}
			if (this.TakeOrderMode)
			{
				((MainForm) base.MdiParent).ShowTakeOrderForm(custID, this.FieldFName.Text.Trim());
			}
			else
			{
				((MainForm) base.MdiParent).ShowTakeOrderForm(this.tableInfo, 0, custID, this.FieldFName.Text);
			}
		}

		private void BtnUp_Click(object sender, EventArgs e)
		{
			this.ListOrderQueue.Up(5);
			this.UpdateOrderButton();
		}

		private void CheckKeyboardOutput(string result)
		{
			if (result != null)
			{
				switch (this.inputState)
				{
					case 0:
						this.FieldFName.Text = result;
						break;

					case 1:
						this.FieldMName.Text = result;
						break;

					case 2:
						this.FieldLName.Text = result;
						break;

					case 3:
						this.FieldAddress.Text = result;
						break;

					case 4:
						this.memo = result;
						break;
				}
				this.UpdateCustomerButton();
			}
		}

		private void ClearCustomer()
		{
			this.roadIgnoreFlag = true;
			this.selectedCust = null;
			this.FieldPhone.Text = string.Empty;
			this.FieldFName.Text = string.Empty;
			this.FieldMName.Text = string.Empty;
			this.FieldLName.Text = string.Empty;
			this.FieldAddress.Text = string.Empty;
			this.memo = string.Empty;
			this.FieldRoad.Text = string.Empty;
			this.FieldOtherRoad.Text = string.Empty;
			this.FieldArea.Text = string.Empty;
			this.ListCustPhone.SelectedIndex = -1;
			this.ListCustPhone.AutoRefresh = true;
			this.ListCustName.AutoRefresh = true;
			this.roadIgnoreFlag = false;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && (this.components != null))
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void FieldInput_Enter(object sender, EventArgs e)
		{
			TextBox box = (TextBox) sender;
			box.BackColor = Color.FromArgb(0xff, 0xff, 0xc0);
			if (box == this.FieldRoad)
			{
				this.SearchRoad();
			}
			else
			{
				this.LstRoad.Visible = false;
			}
		}

		private void FieldInput_Leave(object sender, EventArgs e)
		{
			TextBox box = (TextBox) sender;
			box.BackColor = Color.White;
			if ((box == this.FieldPhone) && (this.FieldPhone.Text != string.Empty))
			{
				this.BtnCustSearch_Click(null, null);
			}
		}

		private void FieldInput_TextChanged(object sender, EventArgs e)
		{
			this.UpdateCustomerButton();
		}

		private void FieldOtherRoad_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Tab)
			{
				this.FieldPhone.Focus();
			}
		}

		private void FieldOtherRoad_TextChanged(object sender, EventArgs e)
		{
			if (this.FieldOtherRoad.Text != string.Empty)
			{
				this.roadIgnoreFlag = true;
				this.selectedRoad = null;
				this.FieldRoad.Text = string.Empty;
				this.FieldArea.Text = "N/A";
				this.roadIgnoreFlag = false;
			}
		}

		private void FieldRoad_KeyDown(object sender, KeyEventArgs e)
		{
			if ((e.KeyCode == Keys.Down) || (e.KeyCode == Keys.Up))
			{
				this.roadLstIgnoreFlag = true;
				this.LstRoad.SelectedIndex = 0;
				this.LstRoad.Focus();
			}
		}

		private void FieldRoad_TextChanged(object sender, EventArgs e)
		{
			if (!this.roadIgnoreFlag)
			{
				this.selectedRoad = null;
				this.FieldOtherRoad.Text = string.Empty;
				this.FieldArea.Text = string.Empty;
				this.SearchRoad();
			}
		}

		private void InitializeComponent()
		{
			this.components = new Container();
			ResourceManager manager = new ResourceManager(typeof(TakeOutForm));
			this.BtnPay = new ImageButton();
			this.ButtonImgList = new ImageList(this.components);
			this.BtnTakeOrder = new ImageButton();
			this.BtnCancel = new ImageButton();
			this.BtnMain = new ImageButton();
			this.NumberImgList = new ImageList(this.components);
			this.BtnDown = new ImageButton();
			this.BtnUp = new ImageButton();
			this.ListOrderQueue = new ItemsList();
			this.ListOrderTime = new ItemsList();
			this.PanCustField = new GroupPanel();
			this.FieldAddress = new TextBox();
			this.LblOtherRoad = new Label();
			this.BtnKBAddress = new ImageButton();
			this.KeyboardImgList = new ImageList(this.components);
			this.LblAddress = new Label();
			this.LstRoad = new ListBox();
			this.BtnMemo = new ImageButton();
			this.FieldRoad = new TextBox();
			this.FieldArea = new Label();
			this.LblArea = new Label();
			this.BtnKBLName = new ImageButton();
			this.FieldLName = new TextBox();
			this.BtnKBMName = new ImageButton();
			this.FieldMName = new TextBox();
			this.BtnKBFName = new ImageButton();
			this.FieldFName = new TextBox();
			this.FieldPhone = new TextBox();
			this.BtnSave = new ImageButton();
			this.ButtonLiteImgList = new ImageList(this.components);
			this.BtnClear = new ImageButton();
			this.LblRoad = new Label();
			this.LblLName = new Label();
			this.LblMName = new Label();
			this.LblFName = new Label();
			this.LblPhone = new Label();
			this.NumberKeyPad = new NumberPad();
			this.BtnDelete = new ImageButton();
			this.FieldOtherRoad = new TextBox();
			this.ListCustPhone = new ItemsList();
			this.ListCustName = new ItemsList();
			this.BtnCustDown = new ImageButton();
			this.BtnCustUp = new ImageButton();
			this.BtnCustList = new ImageButton();
			this.BtnCustSearch = new ImageButton();
			this.LblHeaderName = new Label();
			this.LblHeaderPhone = new Label();
			this.LblHeaderOrder = new Label();
			this.BtnEditOrder = new ImageButton();
			this.LblPageID = new Label();
			this.LblCopyright = new Label();
			this.LblHeaderTime = new Label();
			this.BtnManager = new ImageButton();
			this.BtnExit = new ImageButton();
			this.PanCustField.SuspendLayout();
			base.SuspendLayout();
			this.BtnPay.BackColor = Color.Transparent;
			this.BtnPay.Blue = 1f;
			this.BtnPay.Cursor = Cursors.Hand;
			this.BtnPay.Green = 1f;
			this.BtnPay.ImageClick = (Image) manager.GetObject("BtnPay.ImageClick");
			this.BtnPay.ImageClickIndex = 1;
			this.BtnPay.ImageIndex = 0;
			this.BtnPay.ImageList = this.ButtonImgList;
			this.BtnPay.IsLock = false;
			this.BtnPay.IsMark = false;
			this.BtnPay.Location = new Point(0xe8, 0x40);
			this.BtnPay.Name = "BtnPay";
			this.BtnPay.ObjectValue = null;
			this.BtnPay.Red = 2f;
			this.BtnPay.Size = new Size(110, 60);
			this.BtnPay.TabIndex = 0x1c;
			this.BtnPay.Text = "Pay";
			this.BtnPay.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnPay.Click += new EventHandler(this.BtnPay_Click);
			this.ButtonImgList.ColorDepth = ColorDepth.Depth32Bit;
			this.ButtonImgList.ImageSize = new Size(110, 60);
			this.ButtonImgList.ImageStream = (ImageListStreamer) manager.GetObject("ButtonImgList.ImageStream");
			this.ButtonImgList.TransparentColor = Color.Transparent;
			this.BtnTakeOrder.BackColor = Color.Transparent;
			this.BtnTakeOrder.Blue = 1.75f;
			this.BtnTakeOrder.Cursor = Cursors.Hand;
			this.BtnTakeOrder.Green = 1f;
			this.BtnTakeOrder.ImageClick = (Image) manager.GetObject("BtnTakeOrder.ImageClick");
			this.BtnTakeOrder.ImageClickIndex = 1;
			this.BtnTakeOrder.ImageIndex = 0;
			this.BtnTakeOrder.ImageList = this.ButtonImgList;
			this.BtnTakeOrder.IsLock = false;
			this.BtnTakeOrder.IsMark = false;
			this.BtnTakeOrder.Location = new Point(0x388, 0x40);
			this.BtnTakeOrder.Name = "BtnTakeOrder";
			this.BtnTakeOrder.ObjectValue = null;
			this.BtnTakeOrder.Red = 1.75f;
			this.BtnTakeOrder.Size = new Size(0x70, 60);
			this.BtnTakeOrder.TabIndex = 0x1b;
			this.BtnTakeOrder.Text = "Take Order";
			this.BtnTakeOrder.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnTakeOrder.Click += new EventHandler(this.BtnTakeOrder_Click);
			this.BtnCancel.BackColor = Color.Transparent;
			this.BtnCancel.Blue = 2f;
			this.BtnCancel.Cursor = Cursors.Hand;
			this.BtnCancel.Green = 1f;
			this.BtnCancel.ImageClick = (Image) manager.GetObject("BtnCancel.ImageClick");
			this.BtnCancel.ImageClickIndex = 1;
			this.BtnCancel.ImageIndex = 0;
			this.BtnCancel.ImageList = this.ButtonImgList;
			this.BtnCancel.IsLock = false;
			this.BtnCancel.IsMark = false;
			this.BtnCancel.Location = new Point(8, 0x40);
			this.BtnCancel.Name = "BtnCancel";
			this.BtnCancel.ObjectValue = null;
			this.BtnCancel.Red = 2f;
			this.BtnCancel.Size = new Size(110, 60);
			this.BtnCancel.TabIndex = 0x16;
			this.BtnCancel.Text = "Cancel";
			this.BtnCancel.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnCancel.Click += new EventHandler(this.BtnCancel_Click);
			this.BtnMain.BackColor = Color.Transparent;
			this.BtnMain.Blue = 2f;
			this.BtnMain.Cursor = Cursors.Hand;
			this.BtnMain.Green = 2f;
			this.BtnMain.ImageClick = (Image) manager.GetObject("BtnMain.ImageClick");
			this.BtnMain.ImageClickIndex = 1;
			this.BtnMain.ImageIndex = 0;
			this.BtnMain.ImageList = this.ButtonImgList;
			this.BtnMain.IsLock = false;
			this.BtnMain.IsMark = false;
			this.BtnMain.Location = new Point(0x1a8, 0x40);
			this.BtnMain.Name = "BtnMain";
			this.BtnMain.ObjectValue = null;
			this.BtnMain.Red = 1f;
			this.BtnMain.Size = new Size(110, 60);
			this.BtnMain.TabIndex = 0x15;
			this.BtnMain.Text = "Main";
			this.BtnMain.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnMain.Click += new EventHandler(this.BtnMain_Click);
			this.NumberImgList.ColorDepth = ColorDepth.Depth32Bit;
			this.NumberImgList.ImageSize = new Size(0x48, 60);
			this.NumberImgList.ImageStream = (ImageListStreamer) manager.GetObject("NumberImgList.ImageStream");
			this.NumberImgList.TransparentColor = Color.Transparent;
			this.BtnDown.BackColor = Color.Transparent;
			this.BtnDown.Blue = 2f;
			this.BtnDown.Cursor = Cursors.Hand;
			this.BtnDown.Green = 1f;
			this.BtnDown.ImageClick = (Image) manager.GetObject("BtnDown.ImageClick");
			this.BtnDown.ImageClickIndex = 5;
			this.BtnDown.ImageIndex = 4;
			this.BtnDown.ImageList = this.ButtonImgList;
			this.BtnDown.IsLock = false;
			this.BtnDown.IsMark = false;
			this.BtnDown.Location = new Point(0xd0, 0x2b4);
			this.BtnDown.Name = "BtnDown";
			this.BtnDown.ObjectValue = null;
			this.BtnDown.Red = 2f;
			this.BtnDown.Size = new Size(110, 60);
			this.BtnDown.TabIndex = 0x1f;
			this.BtnDown.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnDown.Click += new EventHandler(this.BtnDown_Click);
			this.BtnUp.BackColor = Color.Transparent;
			this.BtnUp.Blue = 2f;
			this.BtnUp.Cursor = Cursors.Hand;
			this.BtnUp.Green = 1f;
			this.BtnUp.ImageClick = (Image) manager.GetObject("BtnUp.ImageClick");
			this.BtnUp.ImageClickIndex = 3;
			this.BtnUp.ImageIndex = 2;
			this.BtnUp.ImageList = this.ButtonImgList;
			this.BtnUp.IsLock = false;
			this.BtnUp.IsMark = false;
			this.BtnUp.Location = new Point(0x10, 0x2b4);
			this.BtnUp.Name = "BtnUp";
			this.BtnUp.ObjectValue = null;
			this.BtnUp.Red = 2f;
			this.BtnUp.Size = new Size(110, 60);
			this.BtnUp.TabIndex = 30;
			this.BtnUp.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnUp.Click += new EventHandler(this.BtnUp_Click);
			this.ListOrderQueue.Alignment = ContentAlignment.MiddleLeft;
			this.ListOrderQueue.AutoRefresh = true;
			this.ListOrderQueue.BackAlterColor = Color.FromArgb(0xc0, 0xff, 0xff);
			this.ListOrderQueue.BackHeaderColor = Color.FromArgb(0xff, 0xc0, 0x80);
			this.ListOrderQueue.BackHeaderSelectedColor = Color.FromArgb(0xc0, 0, 0);
			this.ListOrderQueue.BackNormalColor = Color.White;
			this.ListOrderQueue.BackSelectedColor = Color.Blue;
			this.ListOrderQueue.BindList1 = this.ListOrderTime;
			this.ListOrderQueue.BindList2 = null;
			this.ListOrderQueue.Border = 0;
			this.ListOrderQueue.Font = new Font("Tahoma", 12f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.ListOrderQueue.ForeAlterColor = Color.Black;
			this.ListOrderQueue.ForeHeaderColor = Color.Black;
			this.ListOrderQueue.ForeHeaderSelectedColor = Color.White;
			this.ListOrderQueue.ForeNormalColor = Color.Black;
			this.ListOrderQueue.ForeSelectedColor = Color.White;
			this.ListOrderQueue.ItemHeight = 40;
			this.ListOrderQueue.ItemWidth = 0xc4;
			this.ListOrderQueue.Location = new Point(8, 0xa8);
			this.ListOrderQueue.Name = "ListOrderQueue";
			this.ListOrderQueue.Row = 13;
			this.ListOrderQueue.SelectedIndex = 0;
			this.ListOrderQueue.Size = new Size(0xc4, 520);
			this.ListOrderQueue.TabIndex = 0x1d;
			this.ListOrderQueue.ItemClick += new smartRestaurant.Controls.ItemsList.ItemsListEventHandler(this.ListOrderQueue_ItemClick);
			this.ListOrderTime.Alignment = ContentAlignment.MiddleCenter;
			this.ListOrderTime.AutoRefresh = true;
			this.ListOrderTime.BackAlterColor = Color.FromArgb(0xc0, 0xff, 0xff);
			this.ListOrderTime.BackHeaderColor = Color.FromArgb(0xff, 0xc0, 0x80);
			this.ListOrderTime.BackHeaderSelectedColor = Color.FromArgb(0xc0, 0, 0);
			this.ListOrderTime.BackNormalColor = Color.White;
			this.ListOrderTime.BackSelectedColor = Color.Blue;
			this.ListOrderTime.BindList1 = this.ListOrderQueue;
			this.ListOrderTime.BindList2 = null;
			this.ListOrderTime.Border = 0;
			this.ListOrderTime.Font = new Font("Tahoma", 12f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.ListOrderTime.ForeAlterColor = Color.Black;
			this.ListOrderTime.ForeHeaderColor = Color.Black;
			this.ListOrderTime.ForeHeaderSelectedColor = Color.White;
			this.ListOrderTime.ForeNormalColor = Color.Black;
			this.ListOrderTime.ForeSelectedColor = Color.White;
			this.ListOrderTime.ItemHeight = 40;
			this.ListOrderTime.ItemWidth = 0x7c;
			this.ListOrderTime.Location = new Point(0xcc, 0xa8);
			this.ListOrderTime.Name = "ListOrderTime";
			this.ListOrderTime.Row = 13;
			this.ListOrderTime.SelectedIndex = 0;
			this.ListOrderTime.Size = new Size(0x7c, 520);
			this.ListOrderTime.TabIndex = 0x2e;
			this.ListOrderTime.ItemClick += new smartRestaurant.Controls.ItemsList.ItemsListEventHandler(this.ListOrderQueue_ItemClick);
			this.PanCustField.BackColor = Color.Transparent;
			this.PanCustField.Caption = null;
			this.PanCustField.Controls.Add(this.FieldAddress);
			this.PanCustField.Controls.Add(this.LblOtherRoad);
			this.PanCustField.Controls.Add(this.BtnKBAddress);
			this.PanCustField.Controls.Add(this.LblAddress);
			this.PanCustField.Controls.Add(this.LstRoad);
			this.PanCustField.Controls.Add(this.BtnMemo);
			this.PanCustField.Controls.Add(this.FieldRoad);
			this.PanCustField.Controls.Add(this.FieldArea);
			this.PanCustField.Controls.Add(this.LblArea);
			this.PanCustField.Controls.Add(this.BtnKBLName);
			this.PanCustField.Controls.Add(this.FieldLName);
			this.PanCustField.Controls.Add(this.BtnKBMName);
			this.PanCustField.Controls.Add(this.FieldMName);
			this.PanCustField.Controls.Add(this.BtnKBFName);
			this.PanCustField.Controls.Add(this.FieldFName);
			this.PanCustField.Controls.Add(this.FieldPhone);
			this.PanCustField.Controls.Add(this.BtnSave);
			this.PanCustField.Controls.Add(this.BtnClear);
			this.PanCustField.Controls.Add(this.LblRoad);
			this.PanCustField.Controls.Add(this.LblLName);
			this.PanCustField.Controls.Add(this.LblMName);
			this.PanCustField.Controls.Add(this.LblFName);
			this.PanCustField.Controls.Add(this.LblPhone);
			this.PanCustField.Controls.Add(this.NumberKeyPad);
			this.PanCustField.Controls.Add(this.BtnDelete);
			this.PanCustField.Controls.Add(this.FieldOtherRoad);
			this.PanCustField.Font = new Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.PanCustField.Location = new Point(0x148, 0x80);
			this.PanCustField.Name = "PanCustField";
			this.PanCustField.ShowHeader = false;
			this.PanCustField.Size = new Size(0x158, 0x270);
			this.PanCustField.TabIndex = 0x20;
			this.FieldAddress.Anchor = AnchorStyles.Left;
			this.FieldAddress.BackColor = Color.White;
			this.FieldAddress.BorderStyle = BorderStyle.FixedSingle;
			this.FieldAddress.Font = new Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.FieldAddress.Location = new Point(0x60, 0x58);
			this.FieldAddress.Multiline = true;
			this.FieldAddress.Name = "FieldAddress";
			this.FieldAddress.Size = new Size(0xc0, 0x60);
			this.FieldAddress.TabIndex = 0x2d;
			this.FieldAddress.Text = "";
			this.FieldAddress.Leave += new EventHandler(this.FieldInput_Leave);
			this.FieldAddress.Enter += new EventHandler(this.FieldInput_Enter);
			this.LblOtherRoad.Location = new Point(0x10, 0xe0);
			this.LblOtherRoad.Name = "LblOtherRoad";
			this.LblOtherRoad.Size = new Size(80, 40);
			this.LblOtherRoad.TabIndex = 0x36;
			this.LblOtherRoad.Text = "Other Rd";
			this.LblOtherRoad.TextAlign = ContentAlignment.MiddleLeft;
			this.BtnKBAddress.BackColor = Color.Transparent;
			this.BtnKBAddress.Blue = 1f;
			this.BtnKBAddress.Cursor = Cursors.Hand;
			this.BtnKBAddress.Font = new Font("Tahoma", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.BtnKBAddress.Green = 1f;
			this.BtnKBAddress.ImageClick = (Image) manager.GetObject("BtnKBAddress.ImageClick");
			this.BtnKBAddress.ImageClickIndex = 1;
			this.BtnKBAddress.ImageIndex = 0;
			this.BtnKBAddress.ImageList = this.KeyboardImgList;
			this.BtnKBAddress.IsLock = false;
			this.BtnKBAddress.IsMark = false;
			this.BtnKBAddress.Location = new Point(0x120, 0x58);
			this.BtnKBAddress.Name = "BtnKBAddress";
			this.BtnKBAddress.ObjectValue = null;
			this.BtnKBAddress.Red = 1f;
			this.BtnKBAddress.Size = new Size(40, 40);
			this.BtnKBAddress.TabIndex = 0x2e;
			this.BtnKBAddress.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnKBAddress.Click += new EventHandler(this.BtnKBAddress_Click);
			this.KeyboardImgList.ColorDepth = ColorDepth.Depth32Bit;
			this.KeyboardImgList.ImageSize = new Size(40, 40);
			this.KeyboardImgList.ImageStream = (ImageListStreamer) manager.GetObject("KeyboardImgList.ImageStream");
			this.KeyboardImgList.TransparentColor = Color.Transparent;
			this.LblAddress.Location = new Point(0x10, 0x58);
			this.LblAddress.Name = "LblAddress";
			this.LblAddress.Size = new Size(80, 40);
			this.LblAddress.TabIndex = 0x18;
			this.LblAddress.Text = "Address";
			this.LblAddress.TextAlign = ContentAlignment.MiddleLeft;
			this.LstRoad.ItemHeight = 0x13;
			this.LstRoad.Location = new Point(0x60, 0xe0);
			this.LstRoad.Name = "LstRoad";
			this.LstRoad.Size = new Size(0xc0, 0x17);
			this.LstRoad.TabIndex = 0x33;
			this.LstRoad.Visible = false;
			this.LstRoad.KeyDown += new KeyEventHandler(this.LstRoad_KeyDown);
			this.LstRoad.SelectedIndexChanged += new EventHandler(this.LstRoad_SelectedIndexChanged);
			this.BtnMemo.BackColor = Color.Transparent;
			this.BtnMemo.Blue = 1f;
			this.BtnMemo.Cursor = Cursors.Hand;
			this.BtnMemo.Font = new Font("Tahoma", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.BtnMemo.Green = 1f;
			this.BtnMemo.ImageClick = (Image) manager.GetObject("BtnMemo.ImageClick");
			this.BtnMemo.ImageClickIndex = 3;
			this.BtnMemo.ImageIndex = 2;
			this.BtnMemo.ImageList = this.KeyboardImgList;
			this.BtnMemo.IsLock = false;
			this.BtnMemo.IsMark = false;
			this.BtnMemo.Location = new Point(0x120, 0x108);
			this.BtnMemo.Name = "BtnMemo";
			this.BtnMemo.ObjectValue = null;
			this.BtnMemo.Red = 1f;
			this.BtnMemo.Size = new Size(40, 40);
			this.BtnMemo.TabIndex = 0x35;
			this.BtnMemo.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnMemo.Click += new EventHandler(this.BtnMemo_Click);
			this.FieldRoad.Anchor = AnchorStyles.Left;
			this.FieldRoad.BackColor = Color.White;
			this.FieldRoad.BorderStyle = BorderStyle.FixedSingle;
			this.FieldRoad.Font = new Font("Tahoma", 27f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.FieldRoad.Location = new Point(0x60, 0xb8);
			this.FieldRoad.Name = "FieldRoad";
			this.FieldRoad.Size = new Size(0xe8, 40);
			this.FieldRoad.TabIndex = 0x34;
			this.FieldRoad.Text = "";
			this.FieldRoad.KeyDown += new KeyEventHandler(this.FieldRoad_KeyDown);
			this.FieldRoad.TextChanged += new EventHandler(this.FieldRoad_TextChanged);
			this.FieldRoad.Leave += new EventHandler(this.FieldInput_Leave);
			this.FieldRoad.Enter += new EventHandler(this.FieldInput_Enter);
			this.FieldArea.Location = new Point(0x60, 0x108);
			this.FieldArea.Name = "FieldArea";
			this.FieldArea.Size = new Size(0xc0, 40);
			this.FieldArea.TabIndex = 50;
			this.FieldArea.TextAlign = ContentAlignment.MiddleLeft;
			this.LblArea.Location = new Point(0x10, 0x108);
			this.LblArea.Name = "LblArea";
			this.LblArea.Size = new Size(80, 40);
			this.LblArea.TabIndex = 0x31;
			this.LblArea.Text = "Area";
			this.LblArea.TextAlign = ContentAlignment.MiddleLeft;
			this.BtnKBLName.BackColor = Color.Transparent;
			this.BtnKBLName.Blue = 1f;
			this.BtnKBLName.Cursor = Cursors.Hand;
			this.BtnKBLName.Font = new Font("Tahoma", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.BtnKBLName.Green = 1f;
			this.BtnKBLName.ImageClick = (Image) manager.GetObject("BtnKBLName.ImageClick");
			this.BtnKBLName.ImageClickIndex = 1;
			this.BtnKBLName.ImageIndex = 0;
			this.BtnKBLName.ImageList = this.KeyboardImgList;
			this.BtnKBLName.IsLock = false;
			this.BtnKBLName.IsMark = false;
			this.BtnKBLName.Location = new Point(0x120, 0x80);
			this.BtnKBLName.Name = "BtnKBLName";
			this.BtnKBLName.ObjectValue = null;
			this.BtnKBLName.Red = 1f;
			this.BtnKBLName.Size = new Size(40, 40);
			this.BtnKBLName.TabIndex = 0x2c;
			this.BtnKBLName.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnKBLName.Visible = false;
			this.BtnKBLName.Click += new EventHandler(this.BtnKBLName_Click);
			this.FieldLName.Anchor = AnchorStyles.Left;
			this.FieldLName.BackColor = Color.White;
			this.FieldLName.BorderStyle = BorderStyle.FixedSingle;
			this.FieldLName.Enabled = false;
			this.FieldLName.Font = new Font("Tahoma", 27f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.FieldLName.Location = new Point(0x60, 0x80);
			this.FieldLName.Name = "FieldLName";
			this.FieldLName.Size = new Size(0xc0, 40);
			this.FieldLName.TabIndex = 0x2b;
			this.FieldLName.Text = "";
			this.FieldLName.Visible = false;
			this.FieldLName.TextChanged += new EventHandler(this.FieldInput_TextChanged);
			this.FieldLName.Leave += new EventHandler(this.FieldInput_Leave);
			this.FieldLName.Enter += new EventHandler(this.FieldInput_Enter);
			this.BtnKBMName.BackColor = Color.Transparent;
			this.BtnKBMName.Blue = 1f;
			this.BtnKBMName.Cursor = Cursors.Hand;
			this.BtnKBMName.Font = new Font("Tahoma", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.BtnKBMName.Green = 1f;
			this.BtnKBMName.ImageClick = (Image) manager.GetObject("BtnKBMName.ImageClick");
			this.BtnKBMName.ImageClickIndex = 1;
			this.BtnKBMName.ImageIndex = 0;
			this.BtnKBMName.ImageList = this.KeyboardImgList;
			this.BtnKBMName.IsLock = false;
			this.BtnKBMName.IsMark = false;
			this.BtnKBMName.Location = new Point(0x120, 0x58);
			this.BtnKBMName.Name = "BtnKBMName";
			this.BtnKBMName.ObjectValue = null;
			this.BtnKBMName.Red = 1f;
			this.BtnKBMName.Size = new Size(40, 40);
			this.BtnKBMName.TabIndex = 0x2a;
			this.BtnKBMName.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnKBMName.Visible = false;
			this.BtnKBMName.Click += new EventHandler(this.BtnKBMName_Click);
			this.FieldMName.Anchor = AnchorStyles.Left;
			this.FieldMName.BackColor = Color.White;
			this.FieldMName.BorderStyle = BorderStyle.FixedSingle;
			this.FieldMName.Enabled = false;
			this.FieldMName.Font = new Font("Tahoma", 27f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.FieldMName.Location = new Point(0x60, 0x58);
			this.FieldMName.Name = "FieldMName";
			this.FieldMName.Size = new Size(0xc0, 40);
			this.FieldMName.TabIndex = 0x29;
			this.FieldMName.Text = "";
			this.FieldMName.Visible = false;
			this.FieldMName.TextChanged += new EventHandler(this.FieldInput_TextChanged);
			this.FieldMName.Leave += new EventHandler(this.FieldInput_Leave);
			this.FieldMName.Enter += new EventHandler(this.FieldInput_Enter);
			this.BtnKBFName.BackColor = Color.Transparent;
			this.BtnKBFName.Blue = 1f;
			this.BtnKBFName.Cursor = Cursors.Hand;
			this.BtnKBFName.Font = new Font("Tahoma", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.BtnKBFName.Green = 1f;
			this.BtnKBFName.ImageClick = (Image) manager.GetObject("BtnKBFName.ImageClick");
			this.BtnKBFName.ImageClickIndex = 1;
			this.BtnKBFName.ImageIndex = 0;
			this.BtnKBFName.ImageList = this.KeyboardImgList;
			this.BtnKBFName.IsLock = false;
			this.BtnKBFName.IsMark = false;
			this.BtnKBFName.Location = new Point(0x120, 0x30);
			this.BtnKBFName.Name = "BtnKBFName";
			this.BtnKBFName.ObjectValue = null;
			this.BtnKBFName.Red = 1f;
			this.BtnKBFName.Size = new Size(40, 40);
			this.BtnKBFName.TabIndex = 40;
			this.BtnKBFName.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnKBFName.Click += new EventHandler(this.BtnKBFName_Click);
			this.FieldFName.Anchor = AnchorStyles.Left;
			this.FieldFName.BackColor = Color.White;
			this.FieldFName.BorderStyle = BorderStyle.FixedSingle;
			this.FieldFName.Font = new Font("Tahoma", 27f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.FieldFName.Location = new Point(0x60, 0x30);
			this.FieldFName.Name = "FieldFName";
			this.FieldFName.Size = new Size(0xc0, 40);
			this.FieldFName.TabIndex = 0x27;
			this.FieldFName.Text = "";
			this.FieldFName.TextChanged += new EventHandler(this.FieldInput_TextChanged);
			this.FieldFName.Leave += new EventHandler(this.FieldInput_Leave);
			this.FieldFName.Enter += new EventHandler(this.FieldInput_Enter);
			this.FieldPhone.Anchor = AnchorStyles.Left;
			this.FieldPhone.BackColor = Color.White;
			this.FieldPhone.BorderStyle = BorderStyle.FixedSingle;
			this.FieldPhone.Font = new Font("Tahoma", 27f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.FieldPhone.Location = new Point(0x60, 8);
			this.FieldPhone.Name = "FieldPhone";
			this.FieldPhone.Size = new Size(0xe8, 40);
			this.FieldPhone.TabIndex = 0x26;
			this.FieldPhone.Text = "";
			this.FieldPhone.TextChanged += new EventHandler(this.FieldInput_TextChanged);
			this.FieldPhone.Leave += new EventHandler(this.FieldInput_Leave);
			this.FieldPhone.Enter += new EventHandler(this.FieldInput_Enter);
			this.BtnSave.BackColor = Color.Transparent;
			this.BtnSave.Blue = 2f;
			this.BtnSave.Cursor = Cursors.Hand;
			this.BtnSave.Font = new Font("Tahoma", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.BtnSave.Green = 1f;
			this.BtnSave.ImageClick = (Image) manager.GetObject("BtnSave.ImageClick");
			this.BtnSave.ImageClickIndex = 1;
			this.BtnSave.ImageIndex = 0;
			this.BtnSave.ImageList = this.ButtonLiteImgList;
			this.BtnSave.IsLock = false;
			this.BtnSave.IsMark = false;
			this.BtnSave.Location = new Point(0xe5, 0x138);
			this.BtnSave.Name = "BtnSave";
			this.BtnSave.ObjectValue = null;
			this.BtnSave.Red = 2f;
			this.BtnSave.Size = new Size(110, 40);
			this.BtnSave.TabIndex = 0x24;
			this.BtnSave.Text = "Save";
			this.BtnSave.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnSave.Click += new EventHandler(this.BtnSave_Click);
			this.ButtonLiteImgList.ColorDepth = ColorDepth.Depth32Bit;
			this.ButtonLiteImgList.ImageSize = new Size(110, 40);
			this.ButtonLiteImgList.ImageStream = (ImageListStreamer) manager.GetObject("ButtonLiteImgList.ImageStream");
			this.ButtonLiteImgList.TransparentColor = Color.Transparent;
			this.BtnClear.BackColor = Color.Transparent;
			this.BtnClear.Blue = 2f;
			this.BtnClear.Cursor = Cursors.Hand;
			this.BtnClear.Font = new Font("Tahoma", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.BtnClear.Green = 1f;
			this.BtnClear.ImageClick = (Image) manager.GetObject("BtnClear.ImageClick");
			this.BtnClear.ImageClickIndex = 1;
			this.BtnClear.ImageIndex = 0;
			this.BtnClear.ImageList = this.ButtonLiteImgList;
			this.BtnClear.IsLock = false;
			this.BtnClear.IsMark = false;
			this.BtnClear.Location = new Point(8, 0x138);
			this.BtnClear.Name = "BtnClear";
			this.BtnClear.ObjectValue = null;
			this.BtnClear.Red = 1f;
			this.BtnClear.Size = new Size(110, 40);
			this.BtnClear.TabIndex = 0x23;
			this.BtnClear.Text = "Clear";
			this.BtnClear.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnClear.Click += new EventHandler(this.BtnClear_Click);
			this.LblRoad.Location = new Point(0x10, 0xb8);
			this.LblRoad.Name = "LblRoad";
			this.LblRoad.Size = new Size(80, 40);
			this.LblRoad.TabIndex = 0x16;
			this.LblRoad.Text = "Road";
			this.LblRoad.TextAlign = ContentAlignment.MiddleLeft;
			this.LblLName.Location = new Point(0x10, 0x80);
			this.LblLName.Name = "LblLName";
			this.LblLName.Size = new Size(80, 40);
			this.LblLName.TabIndex = 20;
			this.LblLName.Text = "L.Name";
			this.LblLName.TextAlign = ContentAlignment.MiddleLeft;
			this.LblLName.Visible = false;
			this.LblMName.Location = new Point(0x10, 0x58);
			this.LblMName.Name = "LblMName";
			this.LblMName.Size = new Size(80, 40);
			this.LblMName.TabIndex = 0x13;
			this.LblMName.Text = "M.Name";
			this.LblMName.TextAlign = ContentAlignment.MiddleLeft;
			this.LblMName.Visible = false;
			this.LblFName.Location = new Point(0x10, 0x30);
			this.LblFName.Name = "LblFName";
			this.LblFName.Size = new Size(80, 40);
			this.LblFName.TabIndex = 0x12;
			this.LblFName.Text = "Name";
			this.LblFName.TextAlign = ContentAlignment.MiddleLeft;
			this.LblPhone.Location = new Point(0x10, 8);
			this.LblPhone.Name = "LblPhone";
			this.LblPhone.Size = new Size(80, 40);
			this.LblPhone.TabIndex = 0x11;
			this.LblPhone.Text = "Phone#";
			this.LblPhone.TextAlign = ContentAlignment.MiddleLeft;
			this.NumberKeyPad.BackColor = Color.White;
			this.NumberKeyPad.Image = (Image) manager.GetObject("NumberKeyPad.Image");
			this.NumberKeyPad.ImageClick = (Image) manager.GetObject("NumberKeyPad.ImageClick");
			this.NumberKeyPad.ImageClickIndex = 1;
			this.NumberKeyPad.ImageIndex = 0;
			this.NumberKeyPad.ImageList = this.NumberImgList;
			this.NumberKeyPad.Location = new Point(0x40, 360);
			this.NumberKeyPad.Name = "NumberKeyPad";
			this.NumberKeyPad.Size = new Size(0xe2, 0xff);
			this.NumberKeyPad.TabIndex = 7;
			this.NumberKeyPad.Text = "numberPad1";
			this.NumberKeyPad.PadClick += new smartRestaurant.Controls.NumberPad.NumberPadEventHandler(this.NumberKeyPad_PadClick);
			this.BtnDelete.BackColor = Color.Transparent;
			this.BtnDelete.Blue = 2f;
			this.BtnDelete.Cursor = Cursors.Hand;
			this.BtnDelete.Font = new Font("Tahoma", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.BtnDelete.Green = 2f;
			this.BtnDelete.ImageClick = (Image) manager.GetObject("BtnDelete.ImageClick");
			this.BtnDelete.ImageClickIndex = 1;
			this.BtnDelete.ImageIndex = 0;
			this.BtnDelete.ImageList = this.ButtonLiteImgList;
			this.BtnDelete.IsLock = false;
			this.BtnDelete.IsMark = false;
			this.BtnDelete.Location = new Point(0xe5, 0x138);
			this.BtnDelete.Name = "BtnDelete";
			this.BtnDelete.ObjectValue = null;
			this.BtnDelete.Red = 1f;
			this.BtnDelete.Size = new Size(110, 40);
			this.BtnDelete.TabIndex = 0x25;
			this.BtnDelete.Text = "Delete";
			this.BtnDelete.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnDelete.Visible = false;
			this.BtnDelete.Click += new EventHandler(this.BtnDelete_Click);
			this.FieldOtherRoad.Anchor = AnchorStyles.Left;
			this.FieldOtherRoad.BackColor = Color.White;
			this.FieldOtherRoad.BorderStyle = BorderStyle.FixedSingle;
			this.FieldOtherRoad.Font = new Font("Tahoma", 27f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.FieldOtherRoad.Location = new Point(0x60, 0xe0);
			this.FieldOtherRoad.Name = "FieldOtherRoad";
			this.FieldOtherRoad.Size = new Size(0xe8, 40);
			this.FieldOtherRoad.TabIndex = 0x37;
			this.FieldOtherRoad.Text = "";
			this.FieldOtherRoad.KeyDown += new KeyEventHandler(this.FieldOtherRoad_KeyDown);
			this.FieldOtherRoad.TextChanged += new EventHandler(this.FieldOtherRoad_TextChanged);
			this.FieldOtherRoad.Leave += new EventHandler(this.FieldInput_Leave);
			this.FieldOtherRoad.Enter += new EventHandler(this.FieldInput_Enter);
			this.ListCustPhone.Alignment = ContentAlignment.MiddleLeft;
			this.ListCustPhone.AutoRefresh = true;
			this.ListCustPhone.BackAlterColor = Color.FromArgb(0xc0, 0xff, 0xff);
			this.ListCustPhone.BackHeaderColor = Color.FromArgb(0xff, 0xc0, 0x80);
			this.ListCustPhone.BackHeaderSelectedColor = Color.FromArgb(0xc0, 0, 0);
			this.ListCustPhone.BackNormalColor = Color.White;
			this.ListCustPhone.BackSelectedColor = Color.Blue;
			this.ListCustPhone.BindList1 = this.ListCustName;
			this.ListCustPhone.BindList2 = null;
			this.ListCustPhone.Border = 0;
			this.ListCustPhone.Font = new Font("Tahoma", 12f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.ListCustPhone.ForeAlterColor = Color.Black;
			this.ListCustPhone.ForeHeaderColor = Color.Black;
			this.ListCustPhone.ForeHeaderSelectedColor = Color.White;
			this.ListCustPhone.ForeNormalColor = Color.Black;
			this.ListCustPhone.ForeSelectedColor = Color.White;
			this.ListCustPhone.ItemHeight = 40;
			this.ListCustPhone.ItemWidth = 0x90;
			this.ListCustPhone.Location = new Point(0x2a0, 0xa8);
			this.ListCustPhone.Name = "ListCustPhone";
			this.ListCustPhone.Row = 13;
			this.ListCustPhone.SelectedIndex = 0;
			this.ListCustPhone.Size = new Size(0x90, 520);
			this.ListCustPhone.TabIndex = 0x21;
			this.ListCustPhone.ItemClick += new smartRestaurant.Controls.ItemsList.ItemsListEventHandler(this.ListCustPhone_ItemClick);
			this.ListCustName.Alignment = ContentAlignment.MiddleLeft;
			this.ListCustName.AutoRefresh = true;
			this.ListCustName.BackAlterColor = Color.FromArgb(0xc0, 0xff, 0xff);
			this.ListCustName.BackHeaderColor = Color.FromArgb(0xff, 0xc0, 0x80);
			this.ListCustName.BackHeaderSelectedColor = Color.FromArgb(0xc0, 0, 0);
			this.ListCustName.BackNormalColor = Color.White;
			this.ListCustName.BackSelectedColor = Color.Blue;
			this.ListCustName.BindList1 = this.ListCustPhone;
			this.ListCustName.BindList2 = null;
			this.ListCustName.Border = 0;
			this.ListCustName.Font = new Font("Tahoma", 12f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.ListCustName.ForeAlterColor = Color.Black;
			this.ListCustName.ForeHeaderColor = Color.Black;
			this.ListCustName.ForeHeaderSelectedColor = Color.White;
			this.ListCustName.ForeNormalColor = Color.Black;
			this.ListCustName.ForeSelectedColor = Color.White;
			this.ListCustName.ItemHeight = 40;
			this.ListCustName.ItemWidth = 0xc4;
			this.ListCustName.Location = new Point(0x330, 0xa8);
			this.ListCustName.Name = "ListCustName";
			this.ListCustName.Row = 13;
			this.ListCustName.SelectedIndex = 0;
			this.ListCustName.Size = new Size(0xc4, 520);
			this.ListCustName.TabIndex = 40;
			this.ListCustName.ItemClick += new smartRestaurant.Controls.ItemsList.ItemsListEventHandler(this.ListCustPhone_ItemClick);
			this.BtnCustDown.BackColor = Color.Transparent;
			this.BtnCustDown.Blue = 1f;
			this.BtnCustDown.Cursor = Cursors.Hand;
			this.BtnCustDown.Green = 1f;
			this.BtnCustDown.ImageClick = (Image) manager.GetObject("BtnCustDown.ImageClick");
			this.BtnCustDown.ImageClickIndex = 5;
			this.BtnCustDown.ImageIndex = 4;
			this.BtnCustDown.ImageList = this.ButtonImgList;
			this.BtnCustDown.IsLock = false;
			this.BtnCustDown.IsMark = false;
			this.BtnCustDown.Location = new Point(0x388, 0x2b4);
			this.BtnCustDown.Name = "BtnCustDown";
			this.BtnCustDown.ObjectValue = null;
			this.BtnCustDown.Red = 2f;
			this.BtnCustDown.Size = new Size(110, 60);
			this.BtnCustDown.TabIndex = 0x23;
			this.BtnCustDown.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnCustDown.Click += new EventHandler(this.BtnCustDown_Click);
			this.BtnCustUp.BackColor = Color.Transparent;
			this.BtnCustUp.Blue = 1f;
			this.BtnCustUp.Cursor = Cursors.Hand;
			this.BtnCustUp.Green = 1f;
			this.BtnCustUp.ImageClick = (Image) manager.GetObject("BtnCustUp.ImageClick");
			this.BtnCustUp.ImageClickIndex = 3;
			this.BtnCustUp.ImageIndex = 2;
			this.BtnCustUp.ImageList = this.ButtonImgList;
			this.BtnCustUp.IsLock = false;
			this.BtnCustUp.IsMark = false;
			this.BtnCustUp.Location = new Point(680, 0x2b4);
			this.BtnCustUp.Name = "BtnCustUp";
			this.BtnCustUp.ObjectValue = null;
			this.BtnCustUp.Red = 2f;
			this.BtnCustUp.Size = new Size(110, 60);
			this.BtnCustUp.TabIndex = 0x22;
			this.BtnCustUp.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnCustUp.Click += new EventHandler(this.BtnCustUp_Click);
			this.BtnCustList.BackColor = Color.Transparent;
			this.BtnCustList.Blue = 1f;
			this.BtnCustList.Cursor = Cursors.Hand;
			this.BtnCustList.Green = 1f;
			this.BtnCustList.ImageClick = (Image) manager.GetObject("BtnCustList.ImageClick");
			this.BtnCustList.ImageClickIndex = 1;
			this.BtnCustList.ImageIndex = 0;
			this.BtnCustList.ImageList = this.ButtonImgList;
			this.BtnCustList.IsLock = false;
			this.BtnCustList.IsMark = false;
			this.BtnCustList.Location = new Point(0x318, 0x40);
			this.BtnCustList.Name = "BtnCustList";
			this.BtnCustList.ObjectValue = null;
			this.BtnCustList.Red = 2f;
			this.BtnCustList.Size = new Size(110, 60);
			this.BtnCustList.TabIndex = 0x25;
			this.BtnCustList.Text = "List All";
			this.BtnCustList.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnCustList.Click += new EventHandler(this.BtnCustList_Click);
			this.BtnCustSearch.BackColor = Color.Transparent;
			this.BtnCustSearch.Blue = 1f;
			this.BtnCustSearch.Cursor = Cursors.Hand;
			this.BtnCustSearch.Green = 1f;
			this.BtnCustSearch.ImageClick = (Image) manager.GetObject("BtnCustSearch.ImageClick");
			this.BtnCustSearch.ImageClickIndex = 1;
			this.BtnCustSearch.ImageIndex = 0;
			this.BtnCustSearch.ImageList = this.ButtonImgList;
			this.BtnCustSearch.IsLock = false;
			this.BtnCustSearch.IsMark = false;
			this.BtnCustSearch.Location = new Point(680, 0x40);
			this.BtnCustSearch.Name = "BtnCustSearch";
			this.BtnCustSearch.ObjectValue = null;
			this.BtnCustSearch.Red = 2f;
			this.BtnCustSearch.Size = new Size(110, 60);
			this.BtnCustSearch.TabIndex = 0x24;
			this.BtnCustSearch.Text = "Search";
			this.BtnCustSearch.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnCustSearch.Click += new EventHandler(this.BtnCustSearch_Click);
			this.LblHeaderName.BackColor = Color.Black;
			this.LblHeaderName.Font = new Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.LblHeaderName.ForeColor = Color.White;
			this.LblHeaderName.Location = new Point(0x330, 0x80);
			this.LblHeaderName.Name = "LblHeaderName";
			this.LblHeaderName.Size = new Size(0xc4, 40);
			this.LblHeaderName.TabIndex = 0x27;
			this.LblHeaderName.Text = "Name";
			this.LblHeaderName.TextAlign = ContentAlignment.MiddleCenter;
			this.LblHeaderPhone.BackColor = Color.Black;
			this.LblHeaderPhone.Font = new Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.LblHeaderPhone.ForeColor = Color.White;
			this.LblHeaderPhone.Location = new Point(0x2a0, 0x80);
			this.LblHeaderPhone.Name = "LblHeaderPhone";
			this.LblHeaderPhone.Size = new Size(0x90, 40);
			this.LblHeaderPhone.TabIndex = 0x26;
			this.LblHeaderPhone.Text = "Phone#";
			this.LblHeaderPhone.TextAlign = ContentAlignment.MiddleCenter;
			this.LblHeaderOrder.BackColor = Color.Black;
			this.LblHeaderOrder.Font = new Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.LblHeaderOrder.ForeColor = Color.White;
			this.LblHeaderOrder.Location = new Point(8, 0x80);
			this.LblHeaderOrder.Name = "LblHeaderOrder";
			this.LblHeaderOrder.Size = new Size(0xc4, 40);
			this.LblHeaderOrder.TabIndex = 0x29;
			this.LblHeaderOrder.Text = "Order Customer";
			this.LblHeaderOrder.TextAlign = ContentAlignment.MiddleLeft;
			this.BtnEditOrder.BackColor = Color.Transparent;
			this.BtnEditOrder.Blue = 1.75f;
			this.BtnEditOrder.Cursor = Cursors.Hand;
			this.BtnEditOrder.Green = 1f;
			this.BtnEditOrder.ImageClick = (Image) manager.GetObject("BtnEditOrder.ImageClick");
			this.BtnEditOrder.ImageClickIndex = 1;
			this.BtnEditOrder.ImageIndex = 0;
			this.BtnEditOrder.ImageList = this.ButtonImgList;
			this.BtnEditOrder.IsLock = false;
			this.BtnEditOrder.IsMark = false;
			this.BtnEditOrder.Location = new Point(120, 0x40);
			this.BtnEditOrder.Name = "BtnEditOrder";
			this.BtnEditOrder.ObjectValue = null;
			this.BtnEditOrder.Red = 1.75f;
			this.BtnEditOrder.Size = new Size(110, 60);
			this.BtnEditOrder.TabIndex = 0x2b;
			this.BtnEditOrder.Text = "Edit Order";
			this.BtnEditOrder.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnEditOrder.Click += new EventHandler(this.BtnEditOrder_Click);
			this.LblPageID.BackColor = Color.Transparent;
			this.LblPageID.Font = new Font("Tahoma", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.LblPageID.ForeColor = Color.FromArgb(0x67, 0x8a, 0xc6);
			this.LblPageID.Location = new Point(0x310, 0x2f0);
			this.LblPageID.Name = "LblPageID";
			this.LblPageID.Size = new Size(0xe0, 0x17);
			this.LblPageID.TabIndex = 0x2c;
			this.LblPageID.Text = "STTO020";
			this.LblPageID.TextAlign = ContentAlignment.TopRight;
			this.LblCopyright.BackColor = Color.Transparent;
			this.LblCopyright.Font = new Font("Tahoma", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.LblCopyright.ForeColor = Color.FromArgb(0x67, 0x8a, 0xc6);
			this.LblCopyright.Location = new Point(8, 0x2f0);
			this.LblCopyright.Name = "LblCopyright";
			this.LblCopyright.Size = new Size(280, 0x10);
			this.LblCopyright.TabIndex = 0x2d;
			this.LblCopyright.Text = "Copyright (c) 2004. All rights reserved.";
			this.LblHeaderTime.BackColor = Color.Black;
			this.LblHeaderTime.Cursor = Cursors.Hand;
			this.LblHeaderTime.Font = new Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.LblHeaderTime.ForeColor = Color.White;
			this.LblHeaderTime.Location = new Point(0xcc, 0x80);
			this.LblHeaderTime.Name = "LblHeaderTime";
			this.LblHeaderTime.Size = new Size(0x7c, 40);
			this.LblHeaderTime.TabIndex = 0x2f;
			this.LblHeaderTime.Text = "Time";
			this.LblHeaderTime.TextAlign = ContentAlignment.MiddleCenter;
			this.LblHeaderTime.Click += new EventHandler(this.LblHeaderTime_Click);
			this.LblHeaderTime.DoubleClick += new EventHandler(this.LblHeaderTime_Click);
			this.BtnManager.BackColor = Color.Transparent;
			this.BtnManager.Blue = 1f;
			this.BtnManager.Cursor = Cursors.Hand;
			this.BtnManager.Green = 2f;
			this.BtnManager.ImageClick = (Image) manager.GetObject("BtnManager.ImageClick");
			this.BtnManager.ImageClickIndex = 1;
			this.BtnManager.ImageIndex = 0;
			this.BtnManager.ImageList = this.ButtonImgList;
			this.BtnManager.IsLock = false;
			this.BtnManager.IsMark = false;
			this.BtnManager.Location = new Point(0x1a8, 0x40);
			this.BtnManager.Name = "BtnManager";
			this.BtnManager.ObjectValue = null;
			this.BtnManager.Red = 1f;
			this.BtnManager.Size = new Size(110, 60);
			this.BtnManager.TabIndex = 0x30;
			this.BtnManager.Text = "Manager";
			this.BtnManager.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnManager.Click += new EventHandler(this.BtnManager_Click);
			this.BtnExit.BackColor = Color.Transparent;
			this.BtnExit.Blue = 2f;
			this.BtnExit.Cursor = Cursors.Hand;
			this.BtnExit.Green = 2f;
			this.BtnExit.ImageClick = (Image) manager.GetObject("BtnExit.ImageClick");
			this.BtnExit.ImageClickIndex = 1;
			this.BtnExit.ImageIndex = 0;
			this.BtnExit.ImageList = this.ButtonImgList;
			this.BtnExit.IsLock = false;
			this.BtnExit.IsMark = false;
			this.BtnExit.Location = new Point(0x218, 0x40);
			this.BtnExit.Name = "BtnExit";
			this.BtnExit.ObjectValue = null;
			this.BtnExit.Red = 1f;
			this.BtnExit.Size = new Size(110, 60);
			this.BtnExit.TabIndex = 0x31;
			this.BtnExit.Text = "Logout";
			this.BtnExit.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnExit.Click += new EventHandler(this.BtnExit_Click);
			this.AutoScaleBaseSize = new Size(6, 15);
			base.ClientSize = new Size(0x3fc, 0x2fc);
			base.Controls.Add(this.BtnExit);
			base.Controls.Add(this.LblHeaderTime);
			base.Controls.Add(this.ListOrderTime);
			base.Controls.Add(this.LblCopyright);
			base.Controls.Add(this.LblPageID);
			base.Controls.Add(this.BtnEditOrder);
			base.Controls.Add(this.LblHeaderOrder);
			base.Controls.Add(this.ListCustName);
			base.Controls.Add(this.LblHeaderName);
			base.Controls.Add(this.LblHeaderPhone);
			base.Controls.Add(this.BtnCustList);
			base.Controls.Add(this.BtnCustSearch);
			base.Controls.Add(this.BtnCustDown);
			base.Controls.Add(this.BtnCustUp);
			base.Controls.Add(this.ListCustPhone);
			base.Controls.Add(this.PanCustField);
			base.Controls.Add(this.BtnDown);
			base.Controls.Add(this.BtnUp);
			base.Controls.Add(this.ListOrderQueue);
			base.Controls.Add(this.BtnPay);
			base.Controls.Add(this.BtnTakeOrder);
			base.Controls.Add(this.BtnCancel);
			base.Controls.Add(this.BtnMain);
			base.Controls.Add(this.BtnManager);
			base.Name = "TakeOutForm";
			this.Text = "Take Out List";
			this.PanCustField.ResumeLayout(false);
			base.ResumeLayout(false);
		}

		private void LblHeaderTime_Click(object sender, EventArgs e)
		{
			this.orderListSortFlag = !this.orderListSortFlag;
			this.UpdateTakeOutList();
		}

		private void ListCustPhone_ItemClick(object sender, ItemsListEventArgs e)
		{
			if (e.Item.Value is smartRestaurant.CustomerService.CustomerInformation)
			{
				this.selectedCust = (smartRestaurant.CustomerService.CustomerInformation) e.Item.Value;
				this.UpdateCustomerField();
			}
		}

		private void ListOrderQueue_ItemClick(object sender, ItemsListEventArgs e)
		{
			if (e.Item.Value is TakeOutInformation)
			{
				this.selectedTakeOut = (TakeOutInformation) e.Item.Value;
			}
			this.UpdateOrderButton();
		}

		private void LstRoad_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Return)
			{
				this.LstRoad_SelectedIndexChanged(sender, null);
			}
			else
			{
				this.roadLstIgnoreFlag = true;
			}
		}

		private void LstRoad_SelectedIndexChanged(object sender, EventArgs e)
		{
			RoadItem selectedItem = (RoadItem) this.LstRoad.SelectedItem;
			Road road = selectedItem.Object;
			if (this.roadLstIgnoreFlag)
			{
				this.roadIgnoreFlag = true;
				this.FieldRoad.Text = road.RoadName;
				this.FieldRoad.SelectAll();
				this.roadIgnoreFlag = false;
				this.roadLstIgnoreFlag = false;
			}
			else
			{
				this.roadIgnoreFlag = true;
				this.FieldRoad.Text = road.RoadName;
				this.FieldOtherRoad.Text = string.Empty;
				this.FieldArea.Text = road.AreaName;
				this.selectedRoad = road;
				this.roadIgnoreFlag = false;
				this.LstRoad.Visible = false;
			}
		}

		private void NumberKeyPad_PadClick(object sender, NumberPadEventArgs e)
		{
			this.FieldPhone.Focus();
			if (e.IsNumeric)
			{
				this.FieldPhone.Text = this.FieldPhone.Text + e.Number.ToString();
			}
			else if (e.IsCancel)
			{
				if (this.FieldPhone.Text.Length > 1)
				{
					this.FieldPhone.Text = this.FieldPhone.Text.Substring(0, this.FieldPhone.Text.Length - 1);
				}
				else
				{
					this.FieldPhone.Text = "";
				}
			}
			this.UpdateCustomerButton();
		}

		private void SearchRoad()
		{
			this.LstRoad.Items.Clear();
			if (Customer.Roads == null)
			{
				this.FieldArea.Text = "N/A";
				this.LstRoad.Visible = false;
				this.selectedRoad = null;
			}
			else
			{
				string str = this.FieldRoad.Text.ToUpper();
				for (int i = 0; i < Customer.Roads.Length; i++)
				{
					if (Customer.Roads[i].RoadName.ToUpper().IndexOf(str) == 0)
					{
						RoadItem item = new RoadItem();
						item.RoadID = Customer.Roads[i].RoadID;
						item.RoadName = Customer.Roads[i].RoadName;
						item.AreaName = Customer.Roads[i].AreaName;
						item.Object = Customer.Roads[i];
						this.LstRoad.Items.Add(item);
					}
				}
				if (this.LstRoad.Items.Count > 0)
				{
					this.LstRoad.Width = this.FieldRoad.Width;
					this.LstRoad.Height = 160;
					this.LstRoad.Top = this.FieldRoad.Top + this.FieldRoad.Height;
					this.LstRoad.Left = this.FieldRoad.Left;
					this.LstRoad.SelectedItem = null;
					this.LstRoad.Visible = true;
				}
				else
				{
					this.FieldArea.Text = "N/A";
					this.LstRoad.Visible = false;
					this.selectedRoad = null;
				}
			}
		}

		private void UpdateCustomerButton()
		{
			this.BtnTakeOrder.Enabled = this.FieldFName.Text != "";
			this.BtnSave.Enabled = this.FieldFName.Text != "";
			this.BtnDelete.Enabled = this.selectedCust != null;
			this.BtnCustSearch.Enabled = (((this.FieldPhone.Text != "") || (this.FieldFName.Text != "")) || (this.FieldMName.Text != "")) || (this.FieldLName.Text != "");
			this.BtnCustUp.Enabled = this.ListCustPhone.CanUp;
			this.BtnCustDown.Enabled = this.ListCustPhone.CanDown;
		}

		private void UpdateCustomerField()
		{
			if (this.selectedCust != null)
			{
				string firstName = this.selectedCust.FirstName;
				if (this.selectedCust.MiddleName != string.Empty)
				{
					string str2 = firstName;
					firstName = str2 + " " + this.selectedCust.MiddleName + " " + this.selectedCust.LastName;
				}
				else if (this.selectedCust.LastName != string.Empty)
				{
					firstName = firstName + " " + this.selectedCust.LastName;
				}
				this.FieldPhone.Text = this.selectedCust.Telephone;
				this.FieldFName.Text = firstName;
				this.FieldMName.Text = this.selectedCust.MiddleName;
				this.FieldLName.Text = this.selectedCust.LastName;
				if (this.selectedCust.Address != null)
				{
					this.FieldAddress.Text = this.selectedCust.Address.Replace("\n", "\r\n");
				}
				else
				{
					this.FieldAddress.Text = string.Empty;
				}
				this.memo = this.selectedCust.Description;
				this.UpdateMemoButton();
				this.roadIgnoreFlag = true;
				if (this.selectedCust.RoadID == 0)
				{
					this.FieldRoad.Text = string.Empty;
					this.FieldOtherRoad.Text = this.selectedCust.OtherRoadName;
					this.FieldArea.Text = "N/A";
				}
				else
				{
					this.selectedRoad = Customer.GetRoad(this.selectedCust.RoadID);
					if (this.selectedRoad != null)
					{
						this.FieldRoad.Text = this.selectedRoad.RoadName;
						this.FieldArea.Text = this.selectedRoad.AreaName;
					}
					else
					{
						this.FieldRoad.Text = string.Empty;
						this.FieldArea.Text = string.Empty;
					}
					this.FieldOtherRoad.Text = string.Empty;
				}
				this.FieldFName.Focus();
				this.roadIgnoreFlag = false;
			}
			this.UpdateCustomerButton();
		}

		private void UpdateCustomerList()
		{
			StringBuilder builder = new StringBuilder();
			this.ListCustPhone.AutoRefresh = false;
			this.ListCustName.AutoRefresh = false;
			this.ListCustPhone.Clear();
			this.ListCustName.Clear();
			if (this.custList == null)
			{
				this.ListCustPhone.AutoRefresh = true;
				this.ListCustName.AutoRefresh = true;
			}
			else
			{
				for (int i = 0; i < this.custList.Length; i++)
				{
					builder.Length = 0;
					builder.Append(this.custList[i].FirstName);
					builder.Append(" ");
					builder.Append(this.custList[i].MiddleName);
					builder.Append(" ");
					builder.Append(this.custList[i].LastName);
					DataItem item = new DataItem(this.custList[i].Telephone, this.custList[i], false);
					this.ListCustPhone.Items.Add(item);
					item = new DataItem(builder.ToString(), this.custList[i], false);
					this.ListCustName.Items.Add(item);
					if (this.selectedCust == this.custList[i])
					{
						this.ListCustPhone.SelectedIndex = this.ListCustPhone.Items.Count - 1;
					}
				}
				this.ListCustPhone.AutoRefresh = true;
				this.ListCustName.AutoRefresh = true;
				this.UpdateCustomerButton();
			}
		}

		public override void UpdateForm()
		{
			if (AppParameter.DeliveryModeOnly)
			{
				this.BtnMain.Visible = false;
				this.BtnManager.Visible = ((MainForm) base.MdiParent).User.IsManager() || ((MainForm) base.MdiParent).User.IsAuditor();
				this.BtnExit.Visible = true;
			}
			else
			{
				this.BtnMain.Visible = true;
				this.BtnManager.Visible = false;
				this.BtnExit.Visible = false;
			}
			if (this.tableInfo == null)
			{
				smartRestaurant.TableService.TableService service = new smartRestaurant.TableService.TableService();
				TableInformation[] tableList = service.GetTableList();
				while (tableList == null)
				{
					if (MessageBox.Show("Can't load table information.", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Hand) == DialogResult.Cancel)
					{
						((MainForm) base.MdiParent).Exit();
					}
					else
					{
						tableList = service.GetTableList();
					}
				}
				for (int i = 0; i < tableList.Length; i++)
				{
					if (tableList[i].TableID == 0)
					{
						this.tableInfo = tableList[i];
						break;
					}
				}
				if (this.tableInfo == null)
				{
					((MainForm) base.MdiParent).Exit();
				}
			}
			this.selectedTakeOut = null;
			this.selectedRoad = null;
			this.roadIgnoreFlag = false;
			this.roadLstIgnoreFlag = false;
			this.LblPageID.Text = "Employee ID:" + ((MainForm) base.MdiParent).UserID + " | STTO020";
			this.UpdateTakeOutList();
			if (this.FieldFName.Text == "")
			{
				this.BtnCustList_Click(null, null);
			}
			else
			{
				this.BtnCustSearch_Click(null, null);
			}
			this.FieldPhone.Focus();
		}

		private void UpdateMemoButton()
		{
			this.BtnMemo.IsMark = this.memo != string.Empty;
			this.BtnMemo.Refresh();
		}

		private void UpdateOrderButton()
		{
			if (this.selectedTakeOut == null)
			{
				this.BtnPay.Enabled = false;
				this.BtnCancel.Enabled = false;
				this.BtnEditOrder.Enabled = false;
			}
			else
			{
				this.BtnPay.Enabled = true;
				this.BtnCancel.Enabled = true;
				this.BtnEditOrder.Enabled = true;
			}
			this.BtnUp.Enabled = this.ListOrderQueue.CanUp;
			this.BtnDown.Enabled = this.ListOrderQueue.CanDown;
		}

		private void UpdateTakeOutList()
		{
			StringBuilder builder = new StringBuilder();
			this.ListOrderQueue.AutoRefresh = false;
			this.ListOrderTime.AutoRefresh = false;
			this.ListOrderQueue.Clear();
			this.ListOrderTime.Clear();
			this.ListOrderQueue.SelectedIndex = -1;
			this.takeOutList = new smartRestaurant.OrderService.OrderService().GetTakeOutList();
			if (this.takeOutList == null)
			{
				this.ListOrderQueue.AutoRefresh = true;
				this.ListOrderTime.AutoRefresh = true;
			}
			else
			{
				for (int i = 0; i < this.takeOutList.Length; i++)
				{
					int num2;
					string str;
					if (this.orderListSortFlag)
					{
						num2 = i;
					}
					else
					{
						num2 = (this.takeOutList.Length - i) - 1;
					}
					builder.Length = 0;
					builder.Append(this.takeOutList[num2].CustInfo.FirstName);
					if (this.takeOutList[num2].CustInfo.MiddleName != "")
					{
						builder.Append(" ");
						builder.Append(this.takeOutList[num2].CustInfo.MiddleName);
					}
					if (this.takeOutList[num2].CustInfo.LastName != "")
					{
						builder.Append(" ");
						builder.Append(this.takeOutList[num2].CustInfo.LastName);
					}
					DateTime orderDate = this.takeOutList[num2].OrderDate;
					if (orderDate != AppParameter.MinDateTime)
					{
						if ((orderDate.DayOfYear == DateTime.Today.DayOfYear) && (orderDate.Year == DateTime.Today.Year))
						{
							str = this.takeOutList[num2].OrderDate.ToString("HH:mm:ss");
						}
						else
						{
							str = this.takeOutList[num2].OrderDate.ToString("dd/MM HH:mm");
						}
					}
					else
					{
						str = string.Empty;
					}
					DataItem item = new DataItem(builder.ToString(), this.takeOutList[num2], false);
					this.ListOrderQueue.Items.Add(item);
					item = new DataItem(str, this.takeOutList[num2], false);
					this.ListOrderTime.Items.Add(item);
					if (this.selectedTakeOut == this.takeOutList[num2])
					{
						this.ListOrderQueue.SelectedIndex = this.ListOrderQueue.Items.Count - 1;
					}
				}
				this.ListOrderQueue.AutoRefresh = true;
				this.ListOrderTime.AutoRefresh = true;
				this.UpdateOrderButton();
			}
		}

		// Properties
		public string CustomerName
		{
			get
			{
				StringBuilder builder = new StringBuilder();
				builder.Append(this.FieldFName.Text);
				if ((this.FieldMName.Text != null) && (this.FieldMName.Text != ""))
				{
					builder.Append(" ");
					builder.Append(this.FieldMName.Text);
				}
				if ((this.FieldLName.Text != null) && (this.FieldLName.Text != ""))
				{
					builder.Append(" ");
					builder.Append(this.FieldLName.Text);
				}
				return builder.ToString();
			}
			set
			{
				this.ClearCustomer();
				if (value != null)
				{
					string[] strArray = value.Split(new char[] { ' ' });
					if (strArray.Length >= 1)
					{
						this.FieldFName.Text = strArray[0];
					}
					if (strArray.Length == 2)
					{
						this.FieldLName.Text = strArray[1];
					}
					else if (strArray.Length >= 3)
					{
						this.FieldMName.Text = strArray[1];
						this.FieldLName.Text = strArray[2];
					}
				}
			}
		}

		public int EmployeeID
		{
			set
			{
				this.employeeID = value;
			}
		}

		public TableInformation Table
		{
			get
			{
				return this.tableInfo;
			}
			set
			{
				this.tableInfo = value;
			}
		}

		public bool TakeOrderMode
		{
			get
			{
				return this.takeOrderMode;
			}
			set
			{
				this.takeOrderMode = value;
			}
		}
	}


}
