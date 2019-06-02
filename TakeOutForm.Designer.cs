using smartRestaurant.Controls;
using smartRestaurant.CustomerService;
using smartRestaurant.Data;
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
	public class TakeOutForm : SmartForm
	{
		private const int INPUT_FIRSTNAME = 0;

		private const int INPUT_MIDDLENAME = 1;

		private const int INPUT_LASTNAME = 2;

		private const int INPUT_ADDRESS = 3;

		private const int INPUT_MEMO = 4;

		private int employeeID;

		private bool takeOrderMode;

		private int inputState;

		private string memo;

		private TableInformation tableInfo;

		private smartRestaurant.CustomerService.CustomerInformation[] custList;

		private smartRestaurant.CustomerService.CustomerInformation selectedCust;

		private TakeOutInformation[] takeOutList;

		private TakeOutInformation selectedTakeOut;

		private Road selectedRoad;

		private bool roadIgnoreFlag;

		private bool roadLstIgnoreFlag;

		private bool orderListSortFlag;

		private ImageButton BtnCancel;

		private ImageButton BtnMain;

		private ImageList NumberImgList;

		private ImageList ButtonImgList;

		private ImageButton BtnPay;

		private ImageButton BtnTakeOrder;

		private ImageButton BtnDown;

		private ImageButton BtnUp;

		private ItemsList ListOrderQueue;

		private NumberPad NumberKeyPad;

		private ImageButton BtnCustDown;

		private ImageButton BtnCustUp;

		private ImageButton BtnCustList;

		private ImageButton BtnCustSearch;

		private ImageList ButtonLiteImgList;

		private Label LblHeaderName;

		private Label LblHeaderPhone;

		private Label LblLName;

		private Label LblMName;

		private Label LblFName;

		private Label LblPhone;

		private Label LblAddress;

		private ImageButton BtnClear;

		private ImageButton BtnSave;

		private ImageButton BtnDelete;

		private ItemsList ListCustPhone;

		private ItemsList ListCustName;

		private Label LblHeaderOrder;

		private IContainer components;

		private GroupPanel PanCustField;

		private ImageButton BtnEditOrder;

		private Label LblPageID;

		private TextBox FieldPhone;

		private TextBox FieldFName;

		private ImageList KeyboardImgList;

		private ImageButton BtnKBFName;

		private TextBox FieldMName;

		private ImageButton BtnKBMName;

		private TextBox FieldLName;

		private ImageButton BtnKBLName;

		private TextBox FieldAddress;

		private ImageButton BtnKBAddress;

		private Label LblRoad;

		private Label LblArea;

		private Label FieldArea;

		private ListBox LstRoad;

		private TextBox FieldRoad;

		private ImageButton BtnMemo;

		private ItemsList ListOrderTime;

		private Label LblHeaderTime;

		private TextBox FieldOtherRoad;

		private Label LblOtherRoad;

		private ImageButton BtnManager;

		private ImageButton BtnExit;

		private Label LblCopyright;

		public string CustomerName
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(this.FieldFName.Text);
				if (this.FieldMName.Text != null && this.FieldMName.Text != "")
				{
					stringBuilder.Append(" ");
					stringBuilder.Append(this.FieldMName.Text);
				}
				if (this.FieldLName.Text != null && this.FieldLName.Text != "")
				{
					stringBuilder.Append(" ");
					stringBuilder.Append(this.FieldLName.Text);
				}
				return stringBuilder.ToString();
			}
			set
			{
				this.ClearCustomer();
				if (value == null)
				{
					return;
				}
				string[] strArrays = value.Split(new char[] { ' ' });
				if ((int)strArrays.Length >= 1)
				{
					this.FieldFName.Text = strArrays[0];
				}
				if ((int)strArrays.Length == 2)
				{
					this.FieldLName.Text = strArrays[1];
					return;
				}
				if ((int)strArrays.Length >= 3)
				{
					this.FieldMName.Text = strArrays[1];
					this.FieldLName.Text = strArrays[2];
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
				return;
			}
			smartRestaurant.OrderService.OrderService orderService = new smartRestaurant.OrderService.OrderService();
			OrderInformation orderByOrderID = orderService.GetOrderByOrderID(this.selectedTakeOut.OrderID);
			if (orderByOrderID == null)
			{
				MessageBox.Show("Can't load order information for this order.");
				return;
			}
			if (orderByOrderID.Bills == null || (int)orderByOrderID.Bills.Length <= 0)
			{
				MessageBox.Show("No order item in this order.");
				return;
			}
			if (!OrderManagement.CancelOrder(orderByOrderID, this.employeeID))
			{
				return;
			}
			orderService.SendOrder(orderByOrderID, 0, null);
			this.selectedTakeOut = null;
			this.UpdateTakeOutList();
			this.UpdateOrderButton();
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
			this.custList = (new smartRestaurant.CustomerService.CustomerService()).GetCustomers();
			if (this.selectedCust != null)
			{
				int num = 0;
				while (num < (int)this.custList.Length)
				{
					if (this.custList[num].CustID != this.selectedCust.CustID)
					{
						num++;
					}
					else
					{
						this.selectedCust = this.custList[num];
						break;
					}
				}
			}
			this.UpdateCustomerList();
		}

		private void BtnCustSearch_Click(object sender, EventArgs e)
		{
			if (this.FieldPhone.Text == "" && this.FieldFName.Text == "" && this.FieldMName.Text == "" && this.FieldLName.Text == "")
			{
				MessageBox.Show("Please fill phone number, first name, middle name, or last name.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				return;
			}
			smartRestaurant.CustomerService.CustomerService customerService = new smartRestaurant.CustomerService.CustomerService();
			this.custList = customerService.SearchCustomers(this.FieldPhone.Text, this.FieldFName.Text, this.FieldMName.Text, this.FieldLName.Text);
			if (this.custList != null && (int)this.custList.Length == 1)
			{
				this.selectedCust = this.custList[0];
				this.UpdateCustomerField();
			}
			this.UpdateCustomerList();
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
				return;
			}
			string str = (new smartRestaurant.CustomerService.CustomerService()).DeleteCustomer(this.selectedCust.CustID);
			if (str != null)
			{
				MessageBox.Show(str);
				return;
			}
			this.ClearCustomer();
			this.BtnCustList_Click(null, null);
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
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(this.selectedTakeOut.CustInfo.FirstName);
				if (this.selectedTakeOut.CustInfo.MiddleName != null && this.selectedTakeOut.CustInfo.MiddleName != "")
				{
					stringBuilder.Append(" ");
					stringBuilder.Append(this.selectedTakeOut.CustInfo.MiddleName);
				}
				if (this.selectedTakeOut.CustInfo.LastName != null && this.selectedTakeOut.CustInfo.LastName != "")
				{
					stringBuilder.Append(" ");
					stringBuilder.Append(this.selectedTakeOut.CustInfo.LastName);
				}
				((MainForm)base.MdiParent).ShowTakeOrderForm(this.tableInfo, this.selectedTakeOut.OrderID, this.selectedTakeOut.CustInfo.CustID, stringBuilder.ToString());
			}
		}

		private void BtnExit_Click(object sender, EventArgs e)
		{
			UserProfile.CheckLogout(((MainForm)base.MdiParent).User.UserID);
			((MainForm)base.MdiParent).User = null;
			((MainForm)base.MdiParent).ShowLoginForm();
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
			if (!this.TakeOrderMode)
			{
				((MainForm)base.MdiParent).ShowMainMenuForm(true);
				return;
			}
			((MainForm)base.MdiParent).ShowTakeOrderForm(0, null);
		}

		private void BtnManager_Click(object sender, EventArgs e)
		{
			((MainForm)base.MdiParent).ShowSalesForm();
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
				return;
			}
			OrderInformation orderByOrderID = (new smartRestaurant.OrderService.OrderService()).GetOrderByOrderID(this.selectedTakeOut.OrderID);
			if (orderByOrderID == null)
			{
				MessageBox.Show("Can't load order information for this order.");
				return;
			}
			if (orderByOrderID.Bills == null || (int)orderByOrderID.Bills.Length <= 0)
			{
				MessageBox.Show("No order item in this order.");
				return;
			}
			((MainForm)base.MdiParent).ShowPrintReceiptForm(this.tableInfo, orderByOrderID, orderByOrderID.Bills[0]);
		}

		private void BtnSave_Click(object sender, EventArgs e)
		{
			smartRestaurant.CustomerService.CustomerInformation text;
			if (this.FieldFName.Text == string.Empty)
			{
				MessageBox.Show("Please fill first name.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				return;
			}
			text = (this.selectedCust == null ? new smartRestaurant.CustomerService.CustomerInformation()
			{
				CustID = 0
			} : this.selectedCust);
			text.Telephone = this.FieldPhone.Text;
			text.FirstName = this.FieldFName.Text;
			text.MiddleName = this.FieldMName.Text;
			text.LastName = this.FieldLName.Text;
			text.Address = this.FieldAddress.Text;
			text.RoadID = (this.selectedRoad != null ? this.selectedRoad.RoadID : 0);
			text.OtherRoadName = this.FieldOtherRoad.Text;
			text.Description = this.memo;
			string str = (new smartRestaurant.CustomerService.CustomerService()).SetCustomer(text);
			if (str != null)
			{
				MessageBox.Show(str);
				return;
			}
			this.BtnCustSearch_Click(null, null);
		}

		private void BtnTakeOrder_Click(object sender, EventArgs e)
		{
			int num;
			num = (this.selectedCust == null ? 0 : this.selectedCust.CustID);
			if (this.TakeOrderMode)
			{
				((MainForm)base.MdiParent).ShowTakeOrderForm(num, this.FieldFName.Text.Trim());
				return;
			}
			((MainForm)base.MdiParent).ShowTakeOrderForm(this.tableInfo, 0, num, this.FieldFName.Text);
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
					{
						this.FieldFName.Text = result;
						break;
					}
					case 1:
					{
						this.FieldMName.Text = result;
						break;
					}
					case 2:
					{
						this.FieldLName.Text = result;
						break;
					}
					case 3:
					{
						this.FieldAddress.Text = result;
						break;
					}
					case 4:
					{
						this.memo = result;
						break;
					}
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
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void FieldInput_Enter(object sender, EventArgs e)
		{
			TextBox textBox = (TextBox)sender;
			textBox.BackColor = Color.FromArgb(255, 255, 192);
			if (textBox == this.FieldRoad)
			{
				this.SearchRoad();
				return;
			}
			this.LstRoad.Visible = false;
		}

		private void FieldInput_Leave(object sender, EventArgs e)
		{
			TextBox white = (TextBox)sender;
			white.BackColor = Color.White;
			if (white == this.FieldPhone && this.FieldPhone.Text != string.Empty)
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
			if (this.FieldOtherRoad.Text == string.Empty)
			{
				return;
			}
			this.roadIgnoreFlag = true;
			this.selectedRoad = null;
			this.FieldRoad.Text = string.Empty;
			this.FieldArea.Text = "N/A";
			this.roadIgnoreFlag = false;
		}

		private void FieldRoad_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up)
			{
				this.roadLstIgnoreFlag = true;
				this.LstRoad.SelectedIndex = 0;
				this.LstRoad.Focus();
			}
		}

		private void FieldRoad_TextChanged(object sender, EventArgs e)
		{
			if (this.roadIgnoreFlag)
			{
				return;
			}
			this.selectedRoad = null;
			this.FieldOtherRoad.Text = string.Empty;
			this.FieldArea.Text = string.Empty;
			this.SearchRoad();
		}

		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			ResourceManager resourceManager = new ResourceManager(typeof(TakeOutForm));
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
			this.BtnPay.ImageClick = (Image)resourceManager.GetObject("BtnPay.ImageClick");
			this.BtnPay.ImageClickIndex = 1;
			this.BtnPay.ImageIndex = 0;
			this.BtnPay.ImageList = this.ButtonImgList;
			this.BtnPay.IsLock = false;
			this.BtnPay.IsMark = false;
			this.BtnPay.Location = new Point(232, 64);
			this.BtnPay.Name = "BtnPay";
			this.BtnPay.ObjectValue = null;
			this.BtnPay.Red = 2f;
			this.BtnPay.Size = new System.Drawing.Size(110, 60);
			this.BtnPay.TabIndex = 28;
			this.BtnPay.Text = "Pay";
			this.BtnPay.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnPay.Click += new EventHandler(this.BtnPay_Click);
			this.ButtonImgList.ColorDepth = ColorDepth.Depth32Bit;
			this.ButtonImgList.ImageSize = new System.Drawing.Size(110, 60);
			this.ButtonImgList.ImageStream = (ImageListStreamer)resourceManager.GetObject("ButtonImgList.ImageStream");
			this.ButtonImgList.TransparentColor = Color.Transparent;
			this.BtnTakeOrder.BackColor = Color.Transparent;
			this.BtnTakeOrder.Blue = 1.75f;
			this.BtnTakeOrder.Cursor = Cursors.Hand;
			this.BtnTakeOrder.Green = 1f;
			this.BtnTakeOrder.ImageClick = (Image)resourceManager.GetObject("BtnTakeOrder.ImageClick");
			this.BtnTakeOrder.ImageClickIndex = 1;
			this.BtnTakeOrder.ImageIndex = 0;
			this.BtnTakeOrder.ImageList = this.ButtonImgList;
			this.BtnTakeOrder.IsLock = false;
			this.BtnTakeOrder.IsMark = false;
			this.BtnTakeOrder.Location = new Point(904, 64);
			this.BtnTakeOrder.Name = "BtnTakeOrder";
			this.BtnTakeOrder.ObjectValue = null;
			this.BtnTakeOrder.Red = 1.75f;
			this.BtnTakeOrder.Size = new System.Drawing.Size(112, 60);
			this.BtnTakeOrder.TabIndex = 27;
			this.BtnTakeOrder.Text = "Take Order";
			this.BtnTakeOrder.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnTakeOrder.Click += new EventHandler(this.BtnTakeOrder_Click);
			this.BtnCancel.BackColor = Color.Transparent;
			this.BtnCancel.Blue = 2f;
			this.BtnCancel.Cursor = Cursors.Hand;
			this.BtnCancel.Green = 1f;
			this.BtnCancel.ImageClick = (Image)resourceManager.GetObject("BtnCancel.ImageClick");
			this.BtnCancel.ImageClickIndex = 1;
			this.BtnCancel.ImageIndex = 0;
			this.BtnCancel.ImageList = this.ButtonImgList;
			this.BtnCancel.IsLock = false;
			this.BtnCancel.IsMark = false;
			this.BtnCancel.Location = new Point(8, 64);
			this.BtnCancel.Name = "BtnCancel";
			this.BtnCancel.ObjectValue = null;
			this.BtnCancel.Red = 2f;
			this.BtnCancel.Size = new System.Drawing.Size(110, 60);
			this.BtnCancel.TabIndex = 22;
			this.BtnCancel.Text = "Cancel";
			this.BtnCancel.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnCancel.Click += new EventHandler(this.BtnCancel_Click);
			this.BtnMain.BackColor = Color.Transparent;
			this.BtnMain.Blue = 2f;
			this.BtnMain.Cursor = Cursors.Hand;
			this.BtnMain.Green = 2f;
			this.BtnMain.ImageClick = (Image)resourceManager.GetObject("BtnMain.ImageClick");
			this.BtnMain.ImageClickIndex = 1;
			this.BtnMain.ImageIndex = 0;
			this.BtnMain.ImageList = this.ButtonImgList;
			this.BtnMain.IsLock = false;
			this.BtnMain.IsMark = false;
			this.BtnMain.Location = new Point(424, 64);
			this.BtnMain.Name = "BtnMain";
			this.BtnMain.ObjectValue = null;
			this.BtnMain.Red = 1f;
			this.BtnMain.Size = new System.Drawing.Size(110, 60);
			this.BtnMain.TabIndex = 21;
			this.BtnMain.Text = "Main";
			this.BtnMain.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnMain.Click += new EventHandler(this.BtnMain_Click);
			this.NumberImgList.ColorDepth = ColorDepth.Depth32Bit;
			this.NumberImgList.ImageSize = new System.Drawing.Size(72, 60);
			this.NumberImgList.ImageStream = (ImageListStreamer)resourceManager.GetObject("NumberImgList.ImageStream");
			this.NumberImgList.TransparentColor = Color.Transparent;
			this.BtnDown.BackColor = Color.Transparent;
			this.BtnDown.Blue = 2f;
			this.BtnDown.Cursor = Cursors.Hand;
			this.BtnDown.Green = 1f;
			this.BtnDown.ImageClick = (Image)resourceManager.GetObject("BtnDown.ImageClick");
			this.BtnDown.ImageClickIndex = 5;
			this.BtnDown.ImageIndex = 4;
			this.BtnDown.ImageList = this.ButtonImgList;
			this.BtnDown.IsLock = false;
			this.BtnDown.IsMark = false;
			this.BtnDown.Location = new Point(208, 692);
			this.BtnDown.Name = "BtnDown";
			this.BtnDown.ObjectValue = null;
			this.BtnDown.Red = 2f;
			this.BtnDown.Size = new System.Drawing.Size(110, 60);
			this.BtnDown.TabIndex = 31;
			this.BtnDown.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnDown.Click += new EventHandler(this.BtnDown_Click);
			this.BtnUp.BackColor = Color.Transparent;
			this.BtnUp.Blue = 2f;
			this.BtnUp.Cursor = Cursors.Hand;
			this.BtnUp.Green = 1f;
			this.BtnUp.ImageClick = (Image)resourceManager.GetObject("BtnUp.ImageClick");
			this.BtnUp.ImageClickIndex = 3;
			this.BtnUp.ImageIndex = 2;
			this.BtnUp.ImageList = this.ButtonImgList;
			this.BtnUp.IsLock = false;
			this.BtnUp.IsMark = false;
			this.BtnUp.Location = new Point(16, 692);
			this.BtnUp.Name = "BtnUp";
			this.BtnUp.ObjectValue = null;
			this.BtnUp.Red = 2f;
			this.BtnUp.Size = new System.Drawing.Size(110, 60);
			this.BtnUp.TabIndex = 30;
			this.BtnUp.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnUp.Click += new EventHandler(this.BtnUp_Click);
			this.ListOrderQueue.Alignment = ContentAlignment.MiddleLeft;
			this.ListOrderQueue.AutoRefresh = true;
			this.ListOrderQueue.BackAlterColor = Color.FromArgb(192, 255, 255);
			this.ListOrderQueue.BackHeaderColor = Color.FromArgb(255, 192, 128);
			this.ListOrderQueue.BackHeaderSelectedColor = Color.FromArgb(192, 0, 0);
			this.ListOrderQueue.BackNormalColor = Color.White;
			this.ListOrderQueue.BackSelectedColor = Color.Blue;
			this.ListOrderQueue.BindList1 = this.ListOrderTime;
			this.ListOrderQueue.BindList2 = null;
			this.ListOrderQueue.Border = 0;
			this.ListOrderQueue.Font = new System.Drawing.Font("Tahoma", 12f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.ListOrderQueue.ForeAlterColor = Color.Black;
			this.ListOrderQueue.ForeHeaderColor = Color.Black;
			this.ListOrderQueue.ForeHeaderSelectedColor = Color.White;
			this.ListOrderQueue.ForeNormalColor = Color.Black;
			this.ListOrderQueue.ForeSelectedColor = Color.White;
			this.ListOrderQueue.ItemHeight = 40;
			this.ListOrderQueue.ItemWidth = 196;
			this.ListOrderQueue.Location = new Point(8, 168);
			this.ListOrderQueue.Name = "ListOrderQueue";
			this.ListOrderQueue.Row = 13;
			this.ListOrderQueue.SelectedIndex = 0;
			this.ListOrderQueue.Size = new System.Drawing.Size(196, 520);
			this.ListOrderQueue.TabIndex = 29;
			this.ListOrderQueue.ItemClick += new ItemsListEventHandler(this.ListOrderQueue_ItemClick);
			this.ListOrderTime.Alignment = ContentAlignment.MiddleCenter;
			this.ListOrderTime.AutoRefresh = true;
			this.ListOrderTime.BackAlterColor = Color.FromArgb(192, 255, 255);
			this.ListOrderTime.BackHeaderColor = Color.FromArgb(255, 192, 128);
			this.ListOrderTime.BackHeaderSelectedColor = Color.FromArgb(192, 0, 0);
			this.ListOrderTime.BackNormalColor = Color.White;
			this.ListOrderTime.BackSelectedColor = Color.Blue;
			this.ListOrderTime.BindList1 = this.ListOrderQueue;
			this.ListOrderTime.BindList2 = null;
			this.ListOrderTime.Border = 0;
			this.ListOrderTime.Font = new System.Drawing.Font("Tahoma", 12f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.ListOrderTime.ForeAlterColor = Color.Black;
			this.ListOrderTime.ForeHeaderColor = Color.Black;
			this.ListOrderTime.ForeHeaderSelectedColor = Color.White;
			this.ListOrderTime.ForeNormalColor = Color.Black;
			this.ListOrderTime.ForeSelectedColor = Color.White;
			this.ListOrderTime.ItemHeight = 40;
			this.ListOrderTime.ItemWidth = 124;
			this.ListOrderTime.Location = new Point(204, 168);
			this.ListOrderTime.Name = "ListOrderTime";
			this.ListOrderTime.Row = 13;
			this.ListOrderTime.SelectedIndex = 0;
			this.ListOrderTime.Size = new System.Drawing.Size(124, 520);
			this.ListOrderTime.TabIndex = 46;
			this.ListOrderTime.ItemClick += new ItemsListEventHandler(this.ListOrderQueue_ItemClick);
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
			this.PanCustField.Font = new System.Drawing.Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.PanCustField.Location = new Point(328, 128);
			this.PanCustField.Name = "PanCustField";
			this.PanCustField.ShowHeader = false;
			this.PanCustField.Size = new System.Drawing.Size(344, 624);
			this.PanCustField.TabIndex = 32;
			this.FieldAddress.Anchor = AnchorStyles.Left;
			this.FieldAddress.BackColor = Color.White;
			this.FieldAddress.BorderStyle = BorderStyle.FixedSingle;
			this.FieldAddress.Font = new System.Drawing.Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.FieldAddress.Location = new Point(96, 88);
			this.FieldAddress.Multiline = true;
			this.FieldAddress.Name = "FieldAddress";
			this.FieldAddress.Size = new System.Drawing.Size(192, 96);
			this.FieldAddress.TabIndex = 45;
			this.FieldAddress.Text = "";
			this.FieldAddress.Leave += new EventHandler(this.FieldInput_Leave);
			this.FieldAddress.Enter += new EventHandler(this.FieldInput_Enter);
			this.LblOtherRoad.Location = new Point(16, 224);
			this.LblOtherRoad.Name = "LblOtherRoad";
			this.LblOtherRoad.Size = new System.Drawing.Size(80, 40);
			this.LblOtherRoad.TabIndex = 54;
			this.LblOtherRoad.Text = "Other Rd";
			this.LblOtherRoad.TextAlign = ContentAlignment.MiddleLeft;
			this.BtnKBAddress.BackColor = Color.Transparent;
			this.BtnKBAddress.Blue = 1f;
			this.BtnKBAddress.Cursor = Cursors.Hand;
			this.BtnKBAddress.Font = new System.Drawing.Font("Tahoma", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.BtnKBAddress.Green = 1f;
			this.BtnKBAddress.ImageClick = (Image)resourceManager.GetObject("BtnKBAddress.ImageClick");
			this.BtnKBAddress.ImageClickIndex = 1;
			this.BtnKBAddress.ImageIndex = 0;
			this.BtnKBAddress.ImageList = this.KeyboardImgList;
			this.BtnKBAddress.IsLock = false;
			this.BtnKBAddress.IsMark = false;
			this.BtnKBAddress.Location = new Point(288, 88);
			this.BtnKBAddress.Name = "BtnKBAddress";
			this.BtnKBAddress.ObjectValue = null;
			this.BtnKBAddress.Red = 1f;
			this.BtnKBAddress.Size = new System.Drawing.Size(40, 40);
			this.BtnKBAddress.TabIndex = 46;
			this.BtnKBAddress.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnKBAddress.Click += new EventHandler(this.BtnKBAddress_Click);
			this.KeyboardImgList.ColorDepth = ColorDepth.Depth32Bit;
			this.KeyboardImgList.ImageSize = new System.Drawing.Size(40, 40);
			this.KeyboardImgList.ImageStream = (ImageListStreamer)resourceManager.GetObject("KeyboardImgList.ImageStream");
			this.KeyboardImgList.TransparentColor = Color.Transparent;
			this.LblAddress.Location = new Point(16, 88);
			this.LblAddress.Name = "LblAddress";
			this.LblAddress.Size = new System.Drawing.Size(80, 40);
			this.LblAddress.TabIndex = 24;
			this.LblAddress.Text = "Address";
			this.LblAddress.TextAlign = ContentAlignment.MiddleLeft;
			this.LstRoad.ItemHeight = 19;
			this.LstRoad.Location = new Point(96, 224);
			this.LstRoad.Name = "LstRoad";
			this.LstRoad.Size = new System.Drawing.Size(192, 23);
			this.LstRoad.TabIndex = 51;
			this.LstRoad.Visible = false;
			this.LstRoad.KeyDown += new KeyEventHandler(this.LstRoad_KeyDown);
			this.LstRoad.SelectedIndexChanged += new EventHandler(this.LstRoad_SelectedIndexChanged);
			this.BtnMemo.BackColor = Color.Transparent;
			this.BtnMemo.Blue = 1f;
			this.BtnMemo.Cursor = Cursors.Hand;
			this.BtnMemo.Font = new System.Drawing.Font("Tahoma", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.BtnMemo.Green = 1f;
			this.BtnMemo.ImageClick = (Image)resourceManager.GetObject("BtnMemo.ImageClick");
			this.BtnMemo.ImageClickIndex = 3;
			this.BtnMemo.ImageIndex = 2;
			this.BtnMemo.ImageList = this.KeyboardImgList;
			this.BtnMemo.IsLock = false;
			this.BtnMemo.IsMark = false;
			this.BtnMemo.Location = new Point(288, 264);
			this.BtnMemo.Name = "BtnMemo";
			this.BtnMemo.ObjectValue = null;
			this.BtnMemo.Red = 1f;
			this.BtnMemo.Size = new System.Drawing.Size(40, 40);
			this.BtnMemo.TabIndex = 53;
			this.BtnMemo.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnMemo.Click += new EventHandler(this.BtnMemo_Click);
			this.FieldRoad.Anchor = AnchorStyles.Left;
			this.FieldRoad.BackColor = Color.White;
			this.FieldRoad.BorderStyle = BorderStyle.FixedSingle;
			this.FieldRoad.Font = new System.Drawing.Font("Tahoma", 27f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.FieldRoad.Location = new Point(96, 184);
			this.FieldRoad.Name = "FieldRoad";
			this.FieldRoad.Size = new System.Drawing.Size(232, 40);
			this.FieldRoad.TabIndex = 52;
			this.FieldRoad.Text = "";
			this.FieldRoad.KeyDown += new KeyEventHandler(this.FieldRoad_KeyDown);
			this.FieldRoad.TextChanged += new EventHandler(this.FieldRoad_TextChanged);
			this.FieldRoad.Leave += new EventHandler(this.FieldInput_Leave);
			this.FieldRoad.Enter += new EventHandler(this.FieldInput_Enter);
			this.FieldArea.Location = new Point(96, 264);
			this.FieldArea.Name = "FieldArea";
			this.FieldArea.Size = new System.Drawing.Size(192, 40);
			this.FieldArea.TabIndex = 50;
			this.FieldArea.TextAlign = ContentAlignment.MiddleLeft;
			this.LblArea.Location = new Point(16, 264);
			this.LblArea.Name = "LblArea";
			this.LblArea.Size = new System.Drawing.Size(80, 40);
			this.LblArea.TabIndex = 49;
			this.LblArea.Text = "Area";
			this.LblArea.TextAlign = ContentAlignment.MiddleLeft;
			this.BtnKBLName.BackColor = Color.Transparent;
			this.BtnKBLName.Blue = 1f;
			this.BtnKBLName.Cursor = Cursors.Hand;
			this.BtnKBLName.Font = new System.Drawing.Font("Tahoma", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.BtnKBLName.Green = 1f;
			this.BtnKBLName.ImageClick = (Image)resourceManager.GetObject("BtnKBLName.ImageClick");
			this.BtnKBLName.ImageClickIndex = 1;
			this.BtnKBLName.ImageIndex = 0;
			this.BtnKBLName.ImageList = this.KeyboardImgList;
			this.BtnKBLName.IsLock = false;
			this.BtnKBLName.IsMark = false;
			this.BtnKBLName.Location = new Point(288, 128);
			this.BtnKBLName.Name = "BtnKBLName";
			this.BtnKBLName.ObjectValue = null;
			this.BtnKBLName.Red = 1f;
			this.BtnKBLName.Size = new System.Drawing.Size(40, 40);
			this.BtnKBLName.TabIndex = 44;
			this.BtnKBLName.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnKBLName.Visible = false;
			this.BtnKBLName.Click += new EventHandler(this.BtnKBLName_Click);
			this.FieldLName.Anchor = AnchorStyles.Left;
			this.FieldLName.BackColor = Color.White;
			this.FieldLName.BorderStyle = BorderStyle.FixedSingle;
			this.FieldLName.Enabled = false;
			this.FieldLName.Font = new System.Drawing.Font("Tahoma", 27f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.FieldLName.Location = new Point(96, 128);
			this.FieldLName.Name = "FieldLName";
			this.FieldLName.Size = new System.Drawing.Size(192, 40);
			this.FieldLName.TabIndex = 43;
			this.FieldLName.Text = "";
			this.FieldLName.Visible = false;
			this.FieldLName.TextChanged += new EventHandler(this.FieldInput_TextChanged);
			this.FieldLName.Leave += new EventHandler(this.FieldInput_Leave);
			this.FieldLName.Enter += new EventHandler(this.FieldInput_Enter);
			this.BtnKBMName.BackColor = Color.Transparent;
			this.BtnKBMName.Blue = 1f;
			this.BtnKBMName.Cursor = Cursors.Hand;
			this.BtnKBMName.Font = new System.Drawing.Font("Tahoma", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.BtnKBMName.Green = 1f;
			this.BtnKBMName.ImageClick = (Image)resourceManager.GetObject("BtnKBMName.ImageClick");
			this.BtnKBMName.ImageClickIndex = 1;
			this.BtnKBMName.ImageIndex = 0;
			this.BtnKBMName.ImageList = this.KeyboardImgList;
			this.BtnKBMName.IsLock = false;
			this.BtnKBMName.IsMark = false;
			this.BtnKBMName.Location = new Point(288, 88);
			this.BtnKBMName.Name = "BtnKBMName";
			this.BtnKBMName.ObjectValue = null;
			this.BtnKBMName.Red = 1f;
			this.BtnKBMName.Size = new System.Drawing.Size(40, 40);
			this.BtnKBMName.TabIndex = 42;
			this.BtnKBMName.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnKBMName.Visible = false;
			this.BtnKBMName.Click += new EventHandler(this.BtnKBMName_Click);
			this.FieldMName.Anchor = AnchorStyles.Left;
			this.FieldMName.BackColor = Color.White;
			this.FieldMName.BorderStyle = BorderStyle.FixedSingle;
			this.FieldMName.Enabled = false;
			this.FieldMName.Font = new System.Drawing.Font("Tahoma", 27f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.FieldMName.Location = new Point(96, 88);
			this.FieldMName.Name = "FieldMName";
			this.FieldMName.Size = new System.Drawing.Size(192, 40);
			this.FieldMName.TabIndex = 41;
			this.FieldMName.Text = "";
			this.FieldMName.Visible = false;
			this.FieldMName.TextChanged += new EventHandler(this.FieldInput_TextChanged);
			this.FieldMName.Leave += new EventHandler(this.FieldInput_Leave);
			this.FieldMName.Enter += new EventHandler(this.FieldInput_Enter);
			this.BtnKBFName.BackColor = Color.Transparent;
			this.BtnKBFName.Blue = 1f;
			this.BtnKBFName.Cursor = Cursors.Hand;
			this.BtnKBFName.Font = new System.Drawing.Font("Tahoma", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.BtnKBFName.Green = 1f;
			this.BtnKBFName.ImageClick = (Image)resourceManager.GetObject("BtnKBFName.ImageClick");
			this.BtnKBFName.ImageClickIndex = 1;
			this.BtnKBFName.ImageIndex = 0;
			this.BtnKBFName.ImageList = this.KeyboardImgList;
			this.BtnKBFName.IsLock = false;
			this.BtnKBFName.IsMark = false;
			this.BtnKBFName.Location = new Point(288, 48);
			this.BtnKBFName.Name = "BtnKBFName";
			this.BtnKBFName.ObjectValue = null;
			this.BtnKBFName.Red = 1f;
			this.BtnKBFName.Size = new System.Drawing.Size(40, 40);
			this.BtnKBFName.TabIndex = 40;
			this.BtnKBFName.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnKBFName.Click += new EventHandler(this.BtnKBFName_Click);
			this.FieldFName.Anchor = AnchorStyles.Left;
			this.FieldFName.BackColor = Color.White;
			this.FieldFName.BorderStyle = BorderStyle.FixedSingle;
			this.FieldFName.Font = new System.Drawing.Font("Tahoma", 27f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.FieldFName.Location = new Point(96, 48);
			this.FieldFName.Name = "FieldFName";
			this.FieldFName.Size = new System.Drawing.Size(192, 40);
			this.FieldFName.TabIndex = 39;
			this.FieldFName.Text = "";
			this.FieldFName.TextChanged += new EventHandler(this.FieldInput_TextChanged);
			this.FieldFName.Leave += new EventHandler(this.FieldInput_Leave);
			this.FieldFName.Enter += new EventHandler(this.FieldInput_Enter);
			this.FieldPhone.Anchor = AnchorStyles.Left;
			this.FieldPhone.BackColor = Color.White;
			this.FieldPhone.BorderStyle = BorderStyle.FixedSingle;
			this.FieldPhone.Font = new System.Drawing.Font("Tahoma", 27f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.FieldPhone.Location = new Point(96, 8);
			this.FieldPhone.Name = "FieldPhone";
			this.FieldPhone.Size = new System.Drawing.Size(232, 40);
			this.FieldPhone.TabIndex = 38;
			this.FieldPhone.Text = "";
			this.FieldPhone.TextChanged += new EventHandler(this.FieldInput_TextChanged);
			this.FieldPhone.Leave += new EventHandler(this.FieldInput_Leave);
			this.FieldPhone.Enter += new EventHandler(this.FieldInput_Enter);
			this.BtnSave.BackColor = Color.Transparent;
			this.BtnSave.Blue = 2f;
			this.BtnSave.Cursor = Cursors.Hand;
			this.BtnSave.Font = new System.Drawing.Font("Tahoma", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.BtnSave.Green = 1f;
			this.BtnSave.ImageClick = (Image)resourceManager.GetObject("BtnSave.ImageClick");
			this.BtnSave.ImageClickIndex = 1;
			this.BtnSave.ImageIndex = 0;
			this.BtnSave.ImageList = this.ButtonLiteImgList;
			this.BtnSave.IsLock = false;
			this.BtnSave.IsMark = false;
			this.BtnSave.Location = new Point(229, 312);
			this.BtnSave.Name = "BtnSave";
			this.BtnSave.ObjectValue = null;
			this.BtnSave.Red = 2f;
			this.BtnSave.Size = new System.Drawing.Size(110, 40);
			this.BtnSave.TabIndex = 36;
			this.BtnSave.Text = "Save";
			this.BtnSave.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnSave.Click += new EventHandler(this.BtnSave_Click);
			this.ButtonLiteImgList.ColorDepth = ColorDepth.Depth32Bit;
			this.ButtonLiteImgList.ImageSize = new System.Drawing.Size(110, 40);
			this.ButtonLiteImgList.ImageStream = (ImageListStreamer)resourceManager.GetObject("ButtonLiteImgList.ImageStream");
			this.ButtonLiteImgList.TransparentColor = Color.Transparent;
			this.BtnClear.BackColor = Color.Transparent;
			this.BtnClear.Blue = 2f;
			this.BtnClear.Cursor = Cursors.Hand;
			this.BtnClear.Font = new System.Drawing.Font("Tahoma", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.BtnClear.Green = 1f;
			this.BtnClear.ImageClick = (Image)resourceManager.GetObject("BtnClear.ImageClick");
			this.BtnClear.ImageClickIndex = 1;
			this.BtnClear.ImageIndex = 0;
			this.BtnClear.ImageList = this.ButtonLiteImgList;
			this.BtnClear.IsLock = false;
			this.BtnClear.IsMark = false;
			this.BtnClear.Location = new Point(8, 312);
			this.BtnClear.Name = "BtnClear";
			this.BtnClear.ObjectValue = null;
			this.BtnClear.Red = 1f;
			this.BtnClear.Size = new System.Drawing.Size(110, 40);
			this.BtnClear.TabIndex = 35;
			this.BtnClear.Text = "Clear";
			this.BtnClear.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnClear.Click += new EventHandler(this.BtnClear_Click);
			this.LblRoad.Location = new Point(16, 184);
			this.LblRoad.Name = "LblRoad";
			this.LblRoad.Size = new System.Drawing.Size(80, 40);
			this.LblRoad.TabIndex = 22;
			this.LblRoad.Text = "Road";
			this.LblRoad.TextAlign = ContentAlignment.MiddleLeft;
			this.LblLName.Location = new Point(16, 128);
			this.LblLName.Name = "LblLName";
			this.LblLName.Size = new System.Drawing.Size(80, 40);
			this.LblLName.TabIndex = 20;
			this.LblLName.Text = "L.Name";
			this.LblLName.TextAlign = ContentAlignment.MiddleLeft;
			this.LblLName.Visible = false;
			this.LblMName.Location = new Point(16, 88);
			this.LblMName.Name = "LblMName";
			this.LblMName.Size = new System.Drawing.Size(80, 40);
			this.LblMName.TabIndex = 19;
			this.LblMName.Text = "M.Name";
			this.LblMName.TextAlign = ContentAlignment.MiddleLeft;
			this.LblMName.Visible = false;
			this.LblFName.Location = new Point(16, 48);
			this.LblFName.Name = "LblFName";
			this.LblFName.Size = new System.Drawing.Size(80, 40);
			this.LblFName.TabIndex = 18;
			this.LblFName.Text = "Name";
			this.LblFName.TextAlign = ContentAlignment.MiddleLeft;
			this.LblPhone.Location = new Point(16, 8);
			this.LblPhone.Name = "LblPhone";
			this.LblPhone.Size = new System.Drawing.Size(80, 40);
			this.LblPhone.TabIndex = 17;
			this.LblPhone.Text = "Phone#";
			this.LblPhone.TextAlign = ContentAlignment.MiddleLeft;
			this.NumberKeyPad.BackColor = Color.White;
			this.NumberKeyPad.Image = (Image)resourceManager.GetObject("NumberKeyPad.Image");
			this.NumberKeyPad.ImageClick = (Image)resourceManager.GetObject("NumberKeyPad.ImageClick");
			this.NumberKeyPad.ImageClickIndex = 1;
			this.NumberKeyPad.ImageIndex = 0;
			this.NumberKeyPad.ImageList = this.NumberImgList;
			this.NumberKeyPad.Location = new Point(64, 360);
			this.NumberKeyPad.Name = "NumberKeyPad";
			this.NumberKeyPad.Size = new System.Drawing.Size(226, 255);
			this.NumberKeyPad.TabIndex = 7;
			this.NumberKeyPad.Text = "numberPad1";
			this.NumberKeyPad.PadClick += new NumberPadEventHandler(this.NumberKeyPad_PadClick);
			this.BtnDelete.BackColor = Color.Transparent;
			this.BtnDelete.Blue = 2f;
			this.BtnDelete.Cursor = Cursors.Hand;
			this.BtnDelete.Font = new System.Drawing.Font("Tahoma", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.BtnDelete.Green = 2f;
			this.BtnDelete.ImageClick = (Image)resourceManager.GetObject("BtnDelete.ImageClick");
			this.BtnDelete.ImageClickIndex = 1;
			this.BtnDelete.ImageIndex = 0;
			this.BtnDelete.ImageList = this.ButtonLiteImgList;
			this.BtnDelete.IsLock = false;
			this.BtnDelete.IsMark = false;
			this.BtnDelete.Location = new Point(229, 312);
			this.BtnDelete.Name = "BtnDelete";
			this.BtnDelete.ObjectValue = null;
			this.BtnDelete.Red = 1f;
			this.BtnDelete.Size = new System.Drawing.Size(110, 40);
			this.BtnDelete.TabIndex = 37;
			this.BtnDelete.Text = "Delete";
			this.BtnDelete.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnDelete.Visible = false;
			this.BtnDelete.Click += new EventHandler(this.BtnDelete_Click);
			this.FieldOtherRoad.Anchor = AnchorStyles.Left;
			this.FieldOtherRoad.BackColor = Color.White;
			this.FieldOtherRoad.BorderStyle = BorderStyle.FixedSingle;
			this.FieldOtherRoad.Font = new System.Drawing.Font("Tahoma", 27f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.FieldOtherRoad.Location = new Point(96, 224);
			this.FieldOtherRoad.Name = "FieldOtherRoad";
			this.FieldOtherRoad.Size = new System.Drawing.Size(232, 40);
			this.FieldOtherRoad.TabIndex = 55;
			this.FieldOtherRoad.Text = "";
			this.FieldOtherRoad.KeyDown += new KeyEventHandler(this.FieldOtherRoad_KeyDown);
			this.FieldOtherRoad.TextChanged += new EventHandler(this.FieldOtherRoad_TextChanged);
			this.FieldOtherRoad.Leave += new EventHandler(this.FieldInput_Leave);
			this.FieldOtherRoad.Enter += new EventHandler(this.FieldInput_Enter);
			this.ListCustPhone.Alignment = ContentAlignment.MiddleLeft;
			this.ListCustPhone.AutoRefresh = true;
			this.ListCustPhone.BackAlterColor = Color.FromArgb(192, 255, 255);
			this.ListCustPhone.BackHeaderColor = Color.FromArgb(255, 192, 128);
			this.ListCustPhone.BackHeaderSelectedColor = Color.FromArgb(192, 0, 0);
			this.ListCustPhone.BackNormalColor = Color.White;
			this.ListCustPhone.BackSelectedColor = Color.Blue;
			this.ListCustPhone.BindList1 = this.ListCustName;
			this.ListCustPhone.BindList2 = null;
			this.ListCustPhone.Border = 0;
			this.ListCustPhone.Font = new System.Drawing.Font("Tahoma", 12f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.ListCustPhone.ForeAlterColor = Color.Black;
			this.ListCustPhone.ForeHeaderColor = Color.Black;
			this.ListCustPhone.ForeHeaderSelectedColor = Color.White;
			this.ListCustPhone.ForeNormalColor = Color.Black;
			this.ListCustPhone.ForeSelectedColor = Color.White;
			this.ListCustPhone.ItemHeight = 40;
			this.ListCustPhone.ItemWidth = 144;
			this.ListCustPhone.Location = new Point(672, 168);
			this.ListCustPhone.Name = "ListCustPhone";
			this.ListCustPhone.Row = 13;
			this.ListCustPhone.SelectedIndex = 0;
			this.ListCustPhone.Size = new System.Drawing.Size(144, 520);
			this.ListCustPhone.TabIndex = 33;
			this.ListCustPhone.ItemClick += new ItemsListEventHandler(this.ListCustPhone_ItemClick);
			this.ListCustName.Alignment = ContentAlignment.MiddleLeft;
			this.ListCustName.AutoRefresh = true;
			this.ListCustName.BackAlterColor = Color.FromArgb(192, 255, 255);
			this.ListCustName.BackHeaderColor = Color.FromArgb(255, 192, 128);
			this.ListCustName.BackHeaderSelectedColor = Color.FromArgb(192, 0, 0);
			this.ListCustName.BackNormalColor = Color.White;
			this.ListCustName.BackSelectedColor = Color.Blue;
			this.ListCustName.BindList1 = this.ListCustPhone;
			this.ListCustName.BindList2 = null;
			this.ListCustName.Border = 0;
			this.ListCustName.Font = new System.Drawing.Font("Tahoma", 12f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.ListCustName.ForeAlterColor = Color.Black;
			this.ListCustName.ForeHeaderColor = Color.Black;
			this.ListCustName.ForeHeaderSelectedColor = Color.White;
			this.ListCustName.ForeNormalColor = Color.Black;
			this.ListCustName.ForeSelectedColor = Color.White;
			this.ListCustName.ItemHeight = 40;
			this.ListCustName.ItemWidth = 196;
			this.ListCustName.Location = new Point(816, 168);
			this.ListCustName.Name = "ListCustName";
			this.ListCustName.Row = 13;
			this.ListCustName.SelectedIndex = 0;
			this.ListCustName.Size = new System.Drawing.Size(196, 520);
			this.ListCustName.TabIndex = 40;
			this.ListCustName.ItemClick += new ItemsListEventHandler(this.ListCustPhone_ItemClick);
			this.BtnCustDown.BackColor = Color.Transparent;
			this.BtnCustDown.Blue = 1f;
			this.BtnCustDown.Cursor = Cursors.Hand;
			this.BtnCustDown.Green = 1f;
			this.BtnCustDown.ImageClick = (Image)resourceManager.GetObject("BtnCustDown.ImageClick");
			this.BtnCustDown.ImageClickIndex = 5;
			this.BtnCustDown.ImageIndex = 4;
			this.BtnCustDown.ImageList = this.ButtonImgList;
			this.BtnCustDown.IsLock = false;
			this.BtnCustDown.IsMark = false;
			this.BtnCustDown.Location = new Point(904, 692);
			this.BtnCustDown.Name = "BtnCustDown";
			this.BtnCustDown.ObjectValue = null;
			this.BtnCustDown.Red = 2f;
			this.BtnCustDown.Size = new System.Drawing.Size(110, 60);
			this.BtnCustDown.TabIndex = 35;
			this.BtnCustDown.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnCustDown.Click += new EventHandler(this.BtnCustDown_Click);
			this.BtnCustUp.BackColor = Color.Transparent;
			this.BtnCustUp.Blue = 1f;
			this.BtnCustUp.Cursor = Cursors.Hand;
			this.BtnCustUp.Green = 1f;
			this.BtnCustUp.ImageClick = (Image)resourceManager.GetObject("BtnCustUp.ImageClick");
			this.BtnCustUp.ImageClickIndex = 3;
			this.BtnCustUp.ImageIndex = 2;
			this.BtnCustUp.ImageList = this.ButtonImgList;
			this.BtnCustUp.IsLock = false;
			this.BtnCustUp.IsMark = false;
			this.BtnCustUp.Location = new Point(680, 692);
			this.BtnCustUp.Name = "BtnCustUp";
			this.BtnCustUp.ObjectValue = null;
			this.BtnCustUp.Red = 2f;
			this.BtnCustUp.Size = new System.Drawing.Size(110, 60);
			this.BtnCustUp.TabIndex = 34;
			this.BtnCustUp.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnCustUp.Click += new EventHandler(this.BtnCustUp_Click);
			this.BtnCustList.BackColor = Color.Transparent;
			this.BtnCustList.Blue = 1f;
			this.BtnCustList.Cursor = Cursors.Hand;
			this.BtnCustList.Green = 1f;
			this.BtnCustList.ImageClick = (Image)resourceManager.GetObject("BtnCustList.ImageClick");
			this.BtnCustList.ImageClickIndex = 1;
			this.BtnCustList.ImageIndex = 0;
			this.BtnCustList.ImageList = this.ButtonImgList;
			this.BtnCustList.IsLock = false;
			this.BtnCustList.IsMark = false;
			this.BtnCustList.Location = new Point(792, 64);
			this.BtnCustList.Name = "BtnCustList";
			this.BtnCustList.ObjectValue = null;
			this.BtnCustList.Red = 2f;
			this.BtnCustList.Size = new System.Drawing.Size(110, 60);
			this.BtnCustList.TabIndex = 37;
			this.BtnCustList.Text = "List All";
			this.BtnCustList.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnCustList.Click += new EventHandler(this.BtnCustList_Click);
			this.BtnCustSearch.BackColor = Color.Transparent;
			this.BtnCustSearch.Blue = 1f;
			this.BtnCustSearch.Cursor = Cursors.Hand;
			this.BtnCustSearch.Green = 1f;
			this.BtnCustSearch.ImageClick = (Image)resourceManager.GetObject("BtnCustSearch.ImageClick");
			this.BtnCustSearch.ImageClickIndex = 1;
			this.BtnCustSearch.ImageIndex = 0;
			this.BtnCustSearch.ImageList = this.ButtonImgList;
			this.BtnCustSearch.IsLock = false;
			this.BtnCustSearch.IsMark = false;
			this.BtnCustSearch.Location = new Point(680, 64);
			this.BtnCustSearch.Name = "BtnCustSearch";
			this.BtnCustSearch.ObjectValue = null;
			this.BtnCustSearch.Red = 2f;
			this.BtnCustSearch.Size = new System.Drawing.Size(110, 60);
			this.BtnCustSearch.TabIndex = 36;
			this.BtnCustSearch.Text = "Search";
			this.BtnCustSearch.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnCustSearch.Click += new EventHandler(this.BtnCustSearch_Click);
			this.LblHeaderName.BackColor = Color.Black;
			this.LblHeaderName.Font = new System.Drawing.Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.LblHeaderName.ForeColor = Color.White;
			this.LblHeaderName.Location = new Point(816, 128);
			this.LblHeaderName.Name = "LblHeaderName";
			this.LblHeaderName.Size = new System.Drawing.Size(196, 40);
			this.LblHeaderName.TabIndex = 39;
			this.LblHeaderName.Text = "Name";
			this.LblHeaderName.TextAlign = ContentAlignment.MiddleCenter;
			this.LblHeaderPhone.BackColor = Color.Black;
			this.LblHeaderPhone.Font = new System.Drawing.Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.LblHeaderPhone.ForeColor = Color.White;
			this.LblHeaderPhone.Location = new Point(672, 128);
			this.LblHeaderPhone.Name = "LblHeaderPhone";
			this.LblHeaderPhone.Size = new System.Drawing.Size(144, 40);
			this.LblHeaderPhone.TabIndex = 38;
			this.LblHeaderPhone.Text = "Phone#";
			this.LblHeaderPhone.TextAlign = ContentAlignment.MiddleCenter;
			this.LblHeaderOrder.BackColor = Color.Black;
			this.LblHeaderOrder.Font = new System.Drawing.Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.LblHeaderOrder.ForeColor = Color.White;
			this.LblHeaderOrder.Location = new Point(8, 128);
			this.LblHeaderOrder.Name = "LblHeaderOrder";
			this.LblHeaderOrder.Size = new System.Drawing.Size(196, 40);
			this.LblHeaderOrder.TabIndex = 41;
			this.LblHeaderOrder.Text = "Order Customer";
			this.LblHeaderOrder.TextAlign = ContentAlignment.MiddleLeft;
			this.BtnEditOrder.BackColor = Color.Transparent;
			this.BtnEditOrder.Blue = 1.75f;
			this.BtnEditOrder.Cursor = Cursors.Hand;
			this.BtnEditOrder.Green = 1f;
			this.BtnEditOrder.ImageClick = (Image)resourceManager.GetObject("BtnEditOrder.ImageClick");
			this.BtnEditOrder.ImageClickIndex = 1;
			this.BtnEditOrder.ImageIndex = 0;
			this.BtnEditOrder.ImageList = this.ButtonImgList;
			this.BtnEditOrder.IsLock = false;
			this.BtnEditOrder.IsMark = false;
			this.BtnEditOrder.Location = new Point(120, 64);
			this.BtnEditOrder.Name = "BtnEditOrder";
			this.BtnEditOrder.ObjectValue = null;
			this.BtnEditOrder.Red = 1.75f;
			this.BtnEditOrder.Size = new System.Drawing.Size(110, 60);
			this.BtnEditOrder.TabIndex = 43;
			this.BtnEditOrder.Text = "Edit Order";
			this.BtnEditOrder.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnEditOrder.Click += new EventHandler(this.BtnEditOrder_Click);
			this.LblPageID.BackColor = Color.Transparent;
			this.LblPageID.Font = new System.Drawing.Font("Tahoma", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.LblPageID.ForeColor = Color.FromArgb(103, 138, 198);
			this.LblPageID.Location = new Point(784, 752);
			this.LblPageID.Name = "LblPageID";
			this.LblPageID.Size = new System.Drawing.Size(224, 23);
			this.LblPageID.TabIndex = 44;
			this.LblPageID.Text = "STTO020";
			this.LblPageID.TextAlign = ContentAlignment.TopRight;
			this.LblCopyright.BackColor = Color.Transparent;
			this.LblCopyright.Font = new System.Drawing.Font("Tahoma", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.LblCopyright.ForeColor = Color.FromArgb(103, 138, 198);
			this.LblCopyright.Location = new Point(8, 752);
			this.LblCopyright.Name = "LblCopyright";
			this.LblCopyright.Size = new System.Drawing.Size(280, 16);
			this.LblCopyright.TabIndex = 45;
			this.LblCopyright.Text = "Copyright (c) 2004. All rights reserved.";
			this.LblHeaderTime.BackColor = Color.Black;
			this.LblHeaderTime.Cursor = Cursors.Hand;
			this.LblHeaderTime.Font = new System.Drawing.Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.LblHeaderTime.ForeColor = Color.White;
			this.LblHeaderTime.Location = new Point(204, 128);
			this.LblHeaderTime.Name = "LblHeaderTime";
			this.LblHeaderTime.Size = new System.Drawing.Size(124, 40);
			this.LblHeaderTime.TabIndex = 47;
			this.LblHeaderTime.Text = "Time";
			this.LblHeaderTime.TextAlign = ContentAlignment.MiddleCenter;
			this.LblHeaderTime.Click += new EventHandler(this.LblHeaderTime_Click);
			this.LblHeaderTime.DoubleClick += new EventHandler(this.LblHeaderTime_Click);
			this.BtnManager.BackColor = Color.Transparent;
			this.BtnManager.Blue = 1f;
			this.BtnManager.Cursor = Cursors.Hand;
			this.BtnManager.Green = 2f;
			this.BtnManager.ImageClick = (Image)resourceManager.GetObject("BtnManager.ImageClick");
			this.BtnManager.ImageClickIndex = 1;
			this.BtnManager.ImageIndex = 0;
			this.BtnManager.ImageList = this.ButtonImgList;
			this.BtnManager.IsLock = false;
			this.BtnManager.IsMark = false;
			this.BtnManager.Location = new Point(424, 64);
			this.BtnManager.Name = "BtnManager";
			this.BtnManager.ObjectValue = null;
			this.BtnManager.Red = 1f;
			this.BtnManager.Size = new System.Drawing.Size(110, 60);
			this.BtnManager.TabIndex = 48;
			this.BtnManager.Text = "Manager";
			this.BtnManager.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnManager.Click += new EventHandler(this.BtnManager_Click);
			this.BtnExit.BackColor = Color.Transparent;
			this.BtnExit.Blue = 2f;
			this.BtnExit.Cursor = Cursors.Hand;
			this.BtnExit.Green = 2f;
			this.BtnExit.ImageClick = (Image)resourceManager.GetObject("BtnExit.ImageClick");
			this.BtnExit.ImageClickIndex = 1;
			this.BtnExit.ImageIndex = 0;
			this.BtnExit.ImageList = this.ButtonImgList;
			this.BtnExit.IsLock = false;
			this.BtnExit.IsMark = false;
			this.BtnExit.Location = new Point(536, 64);
			this.BtnExit.Name = "BtnExit";
			this.BtnExit.ObjectValue = null;
			this.BtnExit.Red = 1f;
			this.BtnExit.Size = new System.Drawing.Size(110, 60);
			this.BtnExit.TabIndex = 49;
			this.BtnExit.Text = "Logout";
			this.BtnExit.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnExit.Click += new EventHandler(this.BtnExit_Click);
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
			base.ClientSize = new System.Drawing.Size(1020, 764);
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
				this.selectedCust = (smartRestaurant.CustomerService.CustomerInformation)e.Item.Value;
				this.UpdateCustomerField();
			}
		}

		private void ListOrderQueue_ItemClick(object sender, ItemsListEventArgs e)
		{
			if (e.Item.Value is TakeOutInformation)
			{
				this.selectedTakeOut = (TakeOutInformation)e.Item.Value;
			}
			this.UpdateOrderButton();
		}

		private void LstRoad_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode != Keys.Return)
			{
				this.roadLstIgnoreFlag = true;
				return;
			}
			this.LstRoad_SelectedIndexChanged(sender, null);
		}

		private void LstRoad_SelectedIndexChanged(object sender, EventArgs e)
		{
			Road obj = ((RoadItem)this.LstRoad.SelectedItem).Object;
			if (this.roadLstIgnoreFlag)
			{
				this.roadIgnoreFlag = true;
				this.FieldRoad.Text = obj.RoadName;
				this.FieldRoad.SelectAll();
				this.roadIgnoreFlag = false;
				this.roadLstIgnoreFlag = false;
				return;
			}
			this.roadIgnoreFlag = true;
			this.FieldRoad.Text = obj.RoadName;
			this.FieldOtherRoad.Text = string.Empty;
			this.FieldArea.Text = obj.AreaName;
			this.selectedRoad = obj;
			this.roadIgnoreFlag = false;
			this.LstRoad.Visible = false;
		}

		private void NumberKeyPad_PadClick(object sender, NumberPadEventArgs e)
		{
			this.FieldPhone.Focus();
			if (e.IsNumeric)
			{
				TextBox fieldPhone = this.FieldPhone;
				string text = fieldPhone.Text;
				int number = e.Number;
				fieldPhone.Text = string.Concat(text, number.ToString());
			}
			else if (e.IsCancel)
			{
				if (this.FieldPhone.Text.Length <= 1)
				{
					this.FieldPhone.Text = "";
				}
				else
				{
					this.FieldPhone.Text = this.FieldPhone.Text.Substring(0, this.FieldPhone.Text.Length - 1);
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
				return;
			}
			string upper = this.FieldRoad.Text.ToUpper();
			for (int i = 0; i < (int)Customer.Roads.Length; i++)
			{
				if (Customer.Roads[i].RoadName.ToUpper().IndexOf(upper) == 0)
				{
					RoadItem roadItem = new RoadItem()
					{
						RoadID = Customer.Roads[i].RoadID,
						RoadName = Customer.Roads[i].RoadName,
						AreaName = Customer.Roads[i].AreaName,
						Object = Customer.Roads[i]
					};
					this.LstRoad.Items.Add(roadItem);
				}
			}
			if (this.LstRoad.Items.Count <= 0)
			{
				this.FieldArea.Text = "N/A";
				this.LstRoad.Visible = false;
				this.selectedRoad = null;
				return;
			}
			this.LstRoad.Width = this.FieldRoad.Width;
			this.LstRoad.Height = 160;
			this.LstRoad.Top = this.FieldRoad.Top + this.FieldRoad.Height;
			this.LstRoad.Left = this.FieldRoad.Left;
			this.LstRoad.SelectedItem = null;
			this.LstRoad.Visible = true;
		}

		private void UpdateCustomerButton()
		{
			this.BtnTakeOrder.Enabled = this.FieldFName.Text != "";
			this.BtnSave.Enabled = this.FieldFName.Text != "";
			this.BtnDelete.Enabled = this.selectedCust != null;
			this.BtnCustSearch.Enabled = (this.FieldPhone.Text != "" || this.FieldFName.Text != "" || this.FieldMName.Text != "" ? true : this.FieldLName.Text != "");
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
					string str = firstName;
					string[] middleName = new string[] { str, " ", this.selectedCust.MiddleName, " ", this.selectedCust.LastName };
					firstName = string.Concat(middleName);
				}
				else if (this.selectedCust.LastName != string.Empty)
				{
					firstName = string.Concat(firstName, " ", this.selectedCust.LastName);
				}
				this.FieldPhone.Text = this.selectedCust.Telephone;
				this.FieldFName.Text = firstName;
				this.FieldMName.Text = this.selectedCust.MiddleName;
				this.FieldLName.Text = this.selectedCust.LastName;
				if (this.selectedCust.Address == null)
				{
					this.FieldAddress.Text = string.Empty;
				}
				else
				{
					this.FieldAddress.Text = this.selectedCust.Address.Replace("\n", "\r\n");
				}
				this.memo = this.selectedCust.Description;
				this.UpdateMemoButton();
				this.roadIgnoreFlag = true;
				if (this.selectedCust.RoadID != 0)
				{
					this.selectedRoad = Customer.GetRoad(this.selectedCust.RoadID);
					if (this.selectedRoad == null)
					{
						this.FieldRoad.Text = string.Empty;
						this.FieldArea.Text = string.Empty;
					}
					else
					{
						this.FieldRoad.Text = this.selectedRoad.RoadName;
						this.FieldArea.Text = this.selectedRoad.AreaName;
					}
					this.FieldOtherRoad.Text = string.Empty;
				}
				else
				{
					this.FieldRoad.Text = string.Empty;
					this.FieldOtherRoad.Text = this.selectedCust.OtherRoadName;
					this.FieldArea.Text = "N/A";
				}
				this.FieldFName.Focus();
				this.roadIgnoreFlag = false;
			}
			this.UpdateCustomerButton();
		}

		private void UpdateCustomerList()
		{
			StringBuilder stringBuilder = new StringBuilder();
			this.ListCustPhone.AutoRefresh = false;
			this.ListCustName.AutoRefresh = false;
			this.ListCustPhone.Clear();
			this.ListCustName.Clear();
			if (this.custList == null)
			{
				this.ListCustPhone.AutoRefresh = true;
				this.ListCustName.AutoRefresh = true;
				return;
			}
			for (int i = 0; i < (int)this.custList.Length; i++)
			{
				stringBuilder.Length = 0;
				stringBuilder.Append(this.custList[i].FirstName);
				stringBuilder.Append(" ");
				stringBuilder.Append(this.custList[i].MiddleName);
				stringBuilder.Append(" ");
				stringBuilder.Append(this.custList[i].LastName);
				DataItem dataItem = new DataItem(this.custList[i].Telephone, this.custList[i], false);
				this.ListCustPhone.Items.Add(dataItem);
				dataItem = new DataItem(stringBuilder.ToString(), this.custList[i], false);
				this.ListCustName.Items.Add(dataItem);
				if (this.selectedCust == this.custList[i])
				{
					this.ListCustPhone.SelectedIndex = this.ListCustPhone.Items.Count - 1;
				}
			}
			this.ListCustPhone.AutoRefresh = true;
			this.ListCustName.AutoRefresh = true;
			this.UpdateCustomerButton();
		}

		public override void UpdateForm()
		{
			if (!AppParameter.DeliveryModeOnly)
			{
				this.BtnMain.Visible = true;
				this.BtnManager.Visible = false;
				this.BtnExit.Visible = false;
			}
			else
			{
				this.BtnMain.Visible = false;
				this.BtnManager.Visible = (((MainForm)base.MdiParent).User.IsManager() ? true : ((MainForm)base.MdiParent).User.IsAuditor());
				this.BtnExit.Visible = true;
			}
			if (this.tableInfo == null)
			{
				smartRestaurant.TableService.TableService tableService = new smartRestaurant.TableService.TableService();
				TableInformation[] tableList = tableService.GetTableList();
				while (tableList == null)
				{
					if (MessageBox.Show("Can't load table information.", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Hand) != System.Windows.Forms.DialogResult.Cancel)
					{
						tableList = tableService.GetTableList();
					}
					else
					{
						((MainForm)base.MdiParent).Exit();
					}
				}
				int num = 0;
				while (num < (int)tableList.Length)
				{
					if (tableList[num].TableID != 0)
					{
						num++;
					}
					else
					{
						this.tableInfo = tableList[num];
						break;
					}
				}
				if (this.tableInfo == null)
				{
					((MainForm)base.MdiParent).Exit();
				}
			}
			this.selectedTakeOut = null;
			this.selectedRoad = null;
			this.roadIgnoreFlag = false;
			this.roadLstIgnoreFlag = false;
			this.LblPageID.Text = string.Concat("Employee ID:", ((MainForm)base.MdiParent).UserID, " | STTO020");
			this.UpdateTakeOutList();
			if (this.FieldFName.Text != "")
			{
				this.BtnCustSearch_Click(null, null);
			}
			else
			{
				this.BtnCustList_Click(null, null);
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
			if (this.selectedTakeOut != null)
			{
				this.BtnPay.Enabled = true;
				this.BtnCancel.Enabled = true;
				this.BtnEditOrder.Enabled = true;
			}
			else
			{
				this.BtnPay.Enabled = false;
				this.BtnCancel.Enabled = false;
				this.BtnEditOrder.Enabled = false;
			}
			this.BtnUp.Enabled = this.ListOrderQueue.CanUp;
			this.BtnDown.Enabled = this.ListOrderQueue.CanDown;
		}

		private void UpdateTakeOutList()
		{
			int num;
			string empty;
			StringBuilder stringBuilder = new StringBuilder();
			this.ListOrderQueue.AutoRefresh = false;
			this.ListOrderTime.AutoRefresh = false;
			this.ListOrderQueue.Clear();
			this.ListOrderTime.Clear();
			this.ListOrderQueue.SelectedIndex = -1;
			this.takeOutList = (new smartRestaurant.OrderService.OrderService()).GetTakeOutList();
			if (this.takeOutList == null)
			{
				this.ListOrderQueue.AutoRefresh = true;
				this.ListOrderTime.AutoRefresh = true;
				return;
			}
			for (int i = 0; i < (int)this.takeOutList.Length; i++)
			{
				num = (!this.orderListSortFlag ? (int)this.takeOutList.Length - i - 1 : i);
				stringBuilder.Length = 0;
				stringBuilder.Append(this.takeOutList[num].CustInfo.FirstName);
				if (this.takeOutList[num].CustInfo.MiddleName != "")
				{
					stringBuilder.Append(" ");
					stringBuilder.Append(this.takeOutList[num].CustInfo.MiddleName);
				}
				if (this.takeOutList[num].CustInfo.LastName != "")
				{
					stringBuilder.Append(" ");
					stringBuilder.Append(this.takeOutList[num].CustInfo.LastName);
				}
				DateTime orderDate = this.takeOutList[num].OrderDate;
				if (orderDate == AppParameter.MinDateTime)
				{
					empty = string.Empty;
				}
				else
				{
					empty = (orderDate.DayOfYear != DateTime.Today.DayOfYear || orderDate.Year != DateTime.Today.Year ? this.takeOutList[num].OrderDate.ToString("dd/MM HH:mm") : this.takeOutList[num].OrderDate.ToString("HH:mm:ss"));
				}
				DataItem dataItem = new DataItem(stringBuilder.ToString(), this.takeOutList[num], false);
				this.ListOrderQueue.Items.Add(dataItem);
				dataItem = new DataItem(empty, this.takeOutList[num], false);
				this.ListOrderTime.Items.Add(dataItem);
				if (this.selectedTakeOut == this.takeOutList[num])
				{
					this.ListOrderQueue.SelectedIndex = this.ListOrderQueue.Items.Count - 1;
				}
			}
			this.ListOrderQueue.AutoRefresh = true;
			this.ListOrderTime.AutoRefresh = true;
			this.UpdateOrderButton();
		}
	}
}