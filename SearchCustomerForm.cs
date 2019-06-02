using smartRestaurant.Controls;
using smartRestaurant.CustomerService;
using smartRestaurant.Utils;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Resources;
using System.Text;
using System.Windows.Forms;

namespace smartRestaurant
{
	public class SearchCustomerForm : Form
	{
		private const int INPUT_FIRSTNAME = 0;

		private const int INPUT_MIDDLENAME = 1;

		private const int INPUT_LASTNAME = 2;

		private const int INPUT_ADDRESS = 3;

		private const int INPUT_MEMO = 4;

		private static SearchCustomerForm instance;

		private int inputState;

		private bool dialogResult;

		private CustomerInformation[] custList;

		private CustomerInformation selectedCust;

		private ImageList NumberImgList;

		private ImageList ButtonImgList;

		private ImageList ButtonLiteImgList;

		private ItemsList ListCustName;

		private ItemsList ListCustPhone;

		private Label LblHeaderName;

		private Label LblHeaderPhone;

		private ImageButton BtnCustList;

		private ImageButton BtnCustSearch;

		private ImageButton BtnCustDown;

		private ImageButton BtnCustUp;

		private GroupPanel PanCustField;

		private ImageButton BtnDelete;

		private ImageButton BtnSave;

		private ImageButton BtnClear;

		private Label FieldMemo;

		private Label FieldAddress;

		private Label FieldLName;

		private Label FieldMName;

		private Label FieldFName;

		private Label FieldPhone;

		private Label LblAddress;

		private Label LblMemo;

		private Label LblLName;

		private Label LblMName;

		private Label LblFName;

		private Label LblPhone;

		private NumberPad NumberKeyPad;

		private ImageButton BtnOk;

		private ImageButton BtnCancel;

		private IContainer components;

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

		static SearchCustomerForm()
		{
			SearchCustomerForm.instance = null;
		}

		public SearchCustomerForm()
		{
			this.InitializeComponent();
			this.selectedCust = null;
		}

		private void BtnCancel_Click(object sender, EventArgs e)
		{
			this.dialogResult = false;
			SearchCustomerForm.instance.Close();
		}

		private void BtnClear_Click(object sender, EventArgs e)
		{
			this.ClearCustomer();
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

		private void BtnOk_Click(object sender, EventArgs e)
		{
			this.dialogResult = true;
			SearchCustomerForm.instance.Close();
		}

		private void BtnSave_Click(object sender, EventArgs e)
		{
			CustomerInformation text;
			if (this.FieldFName.Text == "")
			{
				MessageBox.Show("Please fill first name.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				return;
			}
			text = (this.selectedCust == null ? new CustomerInformation()
			{
				CustID = 0
			} : this.selectedCust);
			text.Telephone = this.FieldPhone.Text;
			text.FirstName = this.FieldFName.Text;
			text.MiddleName = this.FieldMName.Text;
			text.LastName = this.FieldLName.Text;
			text.Address = this.FieldAddress.Text;
			text.Description = this.FieldMemo.Text;
			string str = (new smartRestaurant.CustomerService.CustomerService()).SetCustomer(text);
			if (str != null)
			{
				MessageBox.Show(str);
				return;
			}
			this.BtnCustSearch_Click(null, null);
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
						this.FieldMemo.Text = result;
						break;
					}
				}
				this.UpdateCustomerButton();
			}
		}

		private void ClearCustomer()
		{
			this.selectedCust = null;
			this.FieldPhone.Text = "";
			this.FieldFName.Text = "";
			this.FieldMName.Text = "";
			this.FieldLName.Text = "";
			this.FieldAddress.Text = "";
			this.FieldMemo.Text = "";
			this.ListCustPhone.SelectedIndex = -1;
			this.ListCustPhone.AutoRefresh = true;
			this.ListCustName.AutoRefresh = true;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void FieldAddress_Click(object sender, EventArgs e)
		{
			this.inputState = 3;
			this.CheckKeyboardOutput(KeyboardForm.Show("Address", this.FieldAddress.Text));
		}

		private void FieldFName_Click(object sender, EventArgs e)
		{
			this.inputState = 0;
			this.CheckKeyboardOutput(KeyboardForm.Show("First Name", this.FieldFName.Text));
		}

		private void FieldLName_Click(object sender, EventArgs e)
		{
			this.inputState = 2;
			this.CheckKeyboardOutput(KeyboardForm.Show("Last Name", this.FieldLName.Text));
		}

		private void FieldMemo_Click(object sender, EventArgs e)
		{
			this.inputState = 4;
			this.CheckKeyboardOutput(KeyboardForm.Show("Memo", this.FieldMemo.Text));
		}

		private void FieldMName_Click(object sender, EventArgs e)
		{
			this.inputState = 1;
			this.CheckKeyboardOutput(KeyboardForm.Show("Middle Name", this.FieldMName.Text));
		}

		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			ResourceManager resourceManager = new ResourceManager(typeof(SearchCustomerForm));
			this.NumberImgList = new ImageList(this.components);
			this.ButtonImgList = new ImageList(this.components);
			this.ButtonLiteImgList = new ImageList(this.components);
			this.ListCustName = new ItemsList();
			this.ListCustPhone = new ItemsList();
			this.LblHeaderName = new Label();
			this.LblHeaderPhone = new Label();
			this.BtnCustList = new ImageButton();
			this.BtnCustSearch = new ImageButton();
			this.BtnCustDown = new ImageButton();
			this.BtnCustUp = new ImageButton();
			this.PanCustField = new GroupPanel();
			this.FieldMemo = new Label();
			this.FieldAddress = new Label();
			this.FieldLName = new Label();
			this.FieldMName = new Label();
			this.FieldFName = new Label();
			this.FieldPhone = new Label();
			this.LblAddress = new Label();
			this.LblMemo = new Label();
			this.LblLName = new Label();
			this.LblMName = new Label();
			this.LblFName = new Label();
			this.LblPhone = new Label();
			this.NumberKeyPad = new NumberPad();
			this.BtnSave = new ImageButton();
			this.BtnClear = new ImageButton();
			this.BtnDelete = new ImageButton();
			this.BtnOk = new ImageButton();
			this.BtnCancel = new ImageButton();
			this.PanCustField.SuspendLayout();
			base.SuspendLayout();
			this.NumberImgList.ColorDepth = ColorDepth.Depth32Bit;
			this.NumberImgList.ImageSize = new System.Drawing.Size(72, 60);
			this.NumberImgList.ImageStream = (ImageListStreamer)resourceManager.GetObject("NumberImgList.ImageStream");
			this.NumberImgList.TransparentColor = Color.Transparent;
			this.ButtonImgList.ColorDepth = ColorDepth.Depth32Bit;
			this.ButtonImgList.ImageSize = new System.Drawing.Size(110, 60);
			this.ButtonImgList.ImageStream = (ImageListStreamer)resourceManager.GetObject("ButtonImgList.ImageStream");
			this.ButtonImgList.TransparentColor = Color.Transparent;
			this.ButtonLiteImgList.ColorDepth = ColorDepth.Depth32Bit;
			this.ButtonLiteImgList.ImageSize = new System.Drawing.Size(110, 40);
			this.ButtonLiteImgList.ImageStream = (ImageListStreamer)resourceManager.GetObject("ButtonLiteImgList.ImageStream");
			this.ButtonLiteImgList.TransparentColor = Color.Transparent;
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
			this.ListCustName.Location = new Point(496, 112);
			this.ListCustName.Name = "ListCustName";
			this.ListCustName.Row = 9;
			this.ListCustName.SelectedIndex = 0;
			this.ListCustName.Size = new System.Drawing.Size(196, 360);
			this.ListCustName.TabIndex = 51;
			this.ListCustName.ItemClick += new ItemsListEventHandler(this.ListCustPhone_ItemClick);
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
			this.ListCustPhone.Location = new Point(352, 112);
			this.ListCustPhone.Name = "ListCustPhone";
			this.ListCustPhone.Row = 9;
			this.ListCustPhone.SelectedIndex = 0;
			this.ListCustPhone.Size = new System.Drawing.Size(144, 360);
			this.ListCustPhone.TabIndex = 44;
			this.ListCustPhone.ItemClick += new ItemsListEventHandler(this.ListCustPhone_ItemClick);
			this.LblHeaderName.BackColor = Color.Black;
			this.LblHeaderName.Font = new System.Drawing.Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.LblHeaderName.ForeColor = Color.White;
			this.LblHeaderName.Location = new Point(496, 72);
			this.LblHeaderName.Name = "LblHeaderName";
			this.LblHeaderName.Size = new System.Drawing.Size(196, 40);
			this.LblHeaderName.TabIndex = 50;
			this.LblHeaderName.Text = "Name";
			this.LblHeaderName.TextAlign = ContentAlignment.MiddleCenter;
			this.LblHeaderPhone.BackColor = Color.Black;
			this.LblHeaderPhone.Font = new System.Drawing.Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.LblHeaderPhone.ForeColor = Color.White;
			this.LblHeaderPhone.Location = new Point(352, 72);
			this.LblHeaderPhone.Name = "LblHeaderPhone";
			this.LblHeaderPhone.Size = new System.Drawing.Size(144, 40);
			this.LblHeaderPhone.TabIndex = 49;
			this.LblHeaderPhone.Text = "Phone#";
			this.LblHeaderPhone.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnCustList.BackColor = Color.Transparent;
			this.BtnCustList.Blue = 1f;
			this.BtnCustList.Cursor = Cursors.Hand;
			this.BtnCustList.Font = new System.Drawing.Font("Tahoma", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 222);
			this.BtnCustList.Green = 1f;
			this.BtnCustList.Image = (Bitmap)resourceManager.GetObject("BtnCustList.Image");
			this.BtnCustList.ImageClick = (Bitmap)resourceManager.GetObject("BtnCustList.ImageClick");
			this.BtnCustList.ImageClickIndex = 1;
			this.BtnCustList.ImageIndex = 0;
			this.BtnCustList.ImageList = this.ButtonLiteImgList;
			this.BtnCustList.Location = new Point(466, 32);
			this.BtnCustList.Name = "BtnCustList";
			this.BtnCustList.ObjectValue = null;
			this.BtnCustList.Red = 2f;
			this.BtnCustList.Size = new System.Drawing.Size(110, 40);
			this.BtnCustList.TabIndex = 48;
			this.BtnCustList.Text = "List All";
			this.BtnCustList.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnCustList.Click += new EventHandler(this.BtnCustList_Click);
			this.BtnCustSearch.BackColor = Color.Transparent;
			this.BtnCustSearch.Blue = 1f;
			this.BtnCustSearch.Cursor = Cursors.Hand;
			this.BtnCustSearch.Font = new System.Drawing.Font("Tahoma", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 222);
			this.BtnCustSearch.Green = 1f;
			this.BtnCustSearch.Image = (Bitmap)resourceManager.GetObject("BtnCustSearch.Image");
			this.BtnCustSearch.ImageClick = (Bitmap)resourceManager.GetObject("BtnCustSearch.ImageClick");
			this.BtnCustSearch.ImageClickIndex = 1;
			this.BtnCustSearch.ImageIndex = 0;
			this.BtnCustSearch.ImageList = this.ButtonLiteImgList;
			this.BtnCustSearch.Location = new Point(354, 32);
			this.BtnCustSearch.Name = "BtnCustSearch";
			this.BtnCustSearch.ObjectValue = null;
			this.BtnCustSearch.Red = 2f;
			this.BtnCustSearch.Size = new System.Drawing.Size(110, 40);
			this.BtnCustSearch.TabIndex = 47;
			this.BtnCustSearch.Text = "Search";
			this.BtnCustSearch.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnCustSearch.Click += new EventHandler(this.BtnCustSearch_Click);
			this.BtnCustDown.BackColor = Color.Transparent;
			this.BtnCustDown.Blue = 1f;
			this.BtnCustDown.Cursor = Cursors.Hand;
			this.BtnCustDown.Green = 1f;
			this.BtnCustDown.Image = (Bitmap)resourceManager.GetObject("BtnCustDown.Image");
			this.BtnCustDown.ImageClick = (Bitmap)resourceManager.GetObject("BtnCustDown.ImageClick");
			this.BtnCustDown.ImageClickIndex = 5;
			this.BtnCustDown.ImageIndex = 4;
			this.BtnCustDown.ImageList = this.ButtonImgList;
			this.BtnCustDown.Location = new Point(576, 480);
			this.BtnCustDown.Name = "BtnCustDown";
			this.BtnCustDown.ObjectValue = null;
			this.BtnCustDown.Red = 2f;
			this.BtnCustDown.Size = new System.Drawing.Size(110, 60);
			this.BtnCustDown.TabIndex = 46;
			this.BtnCustDown.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnCustDown.Click += new EventHandler(this.BtnCustDown_Click);
			this.BtnCustUp.BackColor = Color.Transparent;
			this.BtnCustUp.Blue = 1f;
			this.BtnCustUp.Cursor = Cursors.Hand;
			this.BtnCustUp.Green = 1f;
			this.BtnCustUp.Image = (Bitmap)resourceManager.GetObject("BtnCustUp.Image");
			this.BtnCustUp.ImageClick = (Bitmap)resourceManager.GetObject("BtnCustUp.ImageClick");
			this.BtnCustUp.ImageClickIndex = 3;
			this.BtnCustUp.ImageIndex = 2;
			this.BtnCustUp.ImageList = this.ButtonImgList;
			this.BtnCustUp.Location = new Point(360, 480);
			this.BtnCustUp.Name = "BtnCustUp";
			this.BtnCustUp.ObjectValue = null;
			this.BtnCustUp.Red = 2f;
			this.BtnCustUp.Size = new System.Drawing.Size(110, 60);
			this.BtnCustUp.TabIndex = 45;
			this.BtnCustUp.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnCustUp.Click += new EventHandler(this.BtnCustUp_Click);
			this.PanCustField.BackColor = Color.Transparent;
			this.PanCustField.Caption = null;
			Control.ControlCollection controls = this.PanCustField.Controls;
			Control[] fieldMemo = new Control[] { this.FieldMemo, this.FieldAddress, this.FieldLName, this.FieldMName, this.FieldFName, this.FieldPhone, this.LblAddress, this.LblMemo, this.LblLName, this.LblMName, this.LblFName, this.LblPhone, this.NumberKeyPad, this.BtnSave, this.BtnClear };
			controls.AddRange(fieldMemo);
			this.PanCustField.Font = new System.Drawing.Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.PanCustField.Location = new Point(8, 32);
			this.PanCustField.Name = "PanCustField";
			this.PanCustField.ShowHeader = false;
			this.PanCustField.Size = new System.Drawing.Size(344, 576);
			this.PanCustField.TabIndex = 43;
			this.FieldMemo.BackColor = Color.FromArgb(255, 255, 192);
			this.FieldMemo.BorderStyle = BorderStyle.FixedSingle;
			this.FieldMemo.Cursor = Cursors.Hand;
			this.FieldMemo.Location = new Point(96, 216);
			this.FieldMemo.Name = "FieldMemo";
			this.FieldMemo.Size = new System.Drawing.Size(232, 40);
			this.FieldMemo.TabIndex = 34;
			this.FieldMemo.TextAlign = ContentAlignment.MiddleLeft;
			this.FieldMemo.Click += new EventHandler(this.FieldMemo_Click);
			this.FieldAddress.BackColor = Color.FromArgb(255, 255, 192);
			this.FieldAddress.BorderStyle = BorderStyle.FixedSingle;
			this.FieldAddress.Cursor = Cursors.Hand;
			this.FieldAddress.Location = new Point(96, 168);
			this.FieldAddress.Name = "FieldAddress";
			this.FieldAddress.Size = new System.Drawing.Size(232, 48);
			this.FieldAddress.TabIndex = 33;
			this.FieldAddress.TextAlign = ContentAlignment.MiddleLeft;
			this.FieldAddress.Click += new EventHandler(this.FieldAddress_Click);
			this.FieldLName.BackColor = Color.FromArgb(255, 255, 192);
			this.FieldLName.BorderStyle = BorderStyle.FixedSingle;
			this.FieldLName.Cursor = Cursors.Hand;
			this.FieldLName.Location = new Point(96, 128);
			this.FieldLName.Name = "FieldLName";
			this.FieldLName.Size = new System.Drawing.Size(232, 40);
			this.FieldLName.TabIndex = 32;
			this.FieldLName.TextAlign = ContentAlignment.MiddleLeft;
			this.FieldLName.Click += new EventHandler(this.FieldLName_Click);
			this.FieldMName.BackColor = Color.FromArgb(255, 255, 192);
			this.FieldMName.BorderStyle = BorderStyle.FixedSingle;
			this.FieldMName.Cursor = Cursors.Hand;
			this.FieldMName.Location = new Point(96, 88);
			this.FieldMName.Name = "FieldMName";
			this.FieldMName.Size = new System.Drawing.Size(232, 40);
			this.FieldMName.TabIndex = 31;
			this.FieldMName.TextAlign = ContentAlignment.MiddleLeft;
			this.FieldMName.Click += new EventHandler(this.FieldMName_Click);
			this.FieldFName.BackColor = Color.FromArgb(255, 255, 192);
			this.FieldFName.BorderStyle = BorderStyle.FixedSingle;
			this.FieldFName.Cursor = Cursors.Hand;
			this.FieldFName.Location = new Point(96, 48);
			this.FieldFName.Name = "FieldFName";
			this.FieldFName.Size = new System.Drawing.Size(232, 40);
			this.FieldFName.TabIndex = 30;
			this.FieldFName.TextAlign = ContentAlignment.MiddleLeft;
			this.FieldFName.Click += new EventHandler(this.FieldFName_Click);
			this.FieldPhone.BackColor = Color.FromArgb(255, 255, 192);
			this.FieldPhone.BorderStyle = BorderStyle.FixedSingle;
			this.FieldPhone.Cursor = Cursors.Hand;
			this.FieldPhone.Location = new Point(96, 8);
			this.FieldPhone.Name = "FieldPhone";
			this.FieldPhone.Size = new System.Drawing.Size(232, 40);
			this.FieldPhone.TabIndex = 29;
			this.FieldPhone.TextAlign = ContentAlignment.MiddleLeft;
			this.LblAddress.Location = new Point(16, 168);
			this.LblAddress.Name = "LblAddress";
			this.LblAddress.Size = new System.Drawing.Size(80, 40);
			this.LblAddress.TabIndex = 24;
			this.LblAddress.Text = "Address";
			this.LblAddress.TextAlign = ContentAlignment.MiddleLeft;
			this.LblMemo.Location = new Point(16, 216);
			this.LblMemo.Name = "LblMemo";
			this.LblMemo.Size = new System.Drawing.Size(80, 40);
			this.LblMemo.TabIndex = 22;
			this.LblMemo.Text = "Memo";
			this.LblMemo.TextAlign = ContentAlignment.MiddleLeft;
			this.LblLName.Location = new Point(16, 128);
			this.LblLName.Name = "LblLName";
			this.LblLName.Size = new System.Drawing.Size(80, 40);
			this.LblLName.TabIndex = 20;
			this.LblLName.Text = "L.Name";
			this.LblLName.TextAlign = ContentAlignment.MiddleLeft;
			this.LblMName.Location = new Point(16, 88);
			this.LblMName.Name = "LblMName";
			this.LblMName.Size = new System.Drawing.Size(80, 40);
			this.LblMName.TabIndex = 19;
			this.LblMName.Text = "M.Name";
			this.LblMName.TextAlign = ContentAlignment.MiddleLeft;
			this.LblFName.Location = new Point(16, 48);
			this.LblFName.Name = "LblFName";
			this.LblFName.Size = new System.Drawing.Size(80, 40);
			this.LblFName.TabIndex = 18;
			this.LblFName.Text = "F.Name";
			this.LblFName.TextAlign = ContentAlignment.MiddleLeft;
			this.LblPhone.Location = new Point(16, 8);
			this.LblPhone.Name = "LblPhone";
			this.LblPhone.Size = new System.Drawing.Size(80, 40);
			this.LblPhone.TabIndex = 17;
			this.LblPhone.Text = "Phone#";
			this.LblPhone.TextAlign = ContentAlignment.MiddleLeft;
			this.NumberKeyPad.BackColor = Color.White;
			this.NumberKeyPad.Image = (Bitmap)resourceManager.GetObject("NumberKeyPad.Image");
			this.NumberKeyPad.ImageClick = (Bitmap)resourceManager.GetObject("NumberKeyPad.ImageClick");
			this.NumberKeyPad.ImageClickIndex = 1;
			this.NumberKeyPad.ImageIndex = 0;
			this.NumberKeyPad.ImageList = this.NumberImgList;
			this.NumberKeyPad.Location = new Point(64, 312);
			this.NumberKeyPad.Name = "NumberKeyPad";
			this.NumberKeyPad.Size = new System.Drawing.Size(226, 255);
			this.NumberKeyPad.TabIndex = 7;
			this.NumberKeyPad.Text = "numberPad1";
			this.NumberKeyPad.PadClick += new NumberPadEventHandler(this.NumberKeyPad_PadClick);
			this.BtnSave.BackColor = Color.Transparent;
			this.BtnSave.Blue = 2f;
			this.BtnSave.Cursor = Cursors.Hand;
			this.BtnSave.Font = new System.Drawing.Font("Tahoma", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.BtnSave.Green = 1f;
			this.BtnSave.Image = (Bitmap)resourceManager.GetObject("BtnSave.Image");
			this.BtnSave.ImageClick = (Bitmap)resourceManager.GetObject("BtnSave.ImageClick");
			this.BtnSave.ImageClickIndex = 1;
			this.BtnSave.ImageIndex = 0;
			this.BtnSave.ImageList = this.ButtonLiteImgList;
			this.BtnSave.Location = new Point(216, 264);
			this.BtnSave.Name = "BtnSave";
			this.BtnSave.ObjectValue = null;
			this.BtnSave.Red = 2f;
			this.BtnSave.Size = new System.Drawing.Size(112, 40);
			this.BtnSave.TabIndex = 36;
			this.BtnSave.Text = "Save";
			this.BtnSave.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnSave.Click += new EventHandler(this.BtnSave_Click);
			this.BtnClear.BackColor = Color.Transparent;
			this.BtnClear.Blue = 2f;
			this.BtnClear.Cursor = Cursors.Hand;
			this.BtnClear.Font = new System.Drawing.Font("Tahoma", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.BtnClear.Green = 1f;
			this.BtnClear.Image = (Bitmap)resourceManager.GetObject("BtnClear.Image");
			this.BtnClear.ImageClick = (Bitmap)resourceManager.GetObject("BtnClear.ImageClick");
			this.BtnClear.ImageClickIndex = 1;
			this.BtnClear.ImageIndex = 0;
			this.BtnClear.ImageList = this.ButtonLiteImgList;
			this.BtnClear.Location = new Point(16, 264);
			this.BtnClear.Name = "BtnClear";
			this.BtnClear.ObjectValue = null;
			this.BtnClear.Red = 1f;
			this.BtnClear.Size = new System.Drawing.Size(110, 40);
			this.BtnClear.TabIndex = 35;
			this.BtnClear.Text = "Clear";
			this.BtnClear.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnClear.Click += new EventHandler(this.BtnClear_Click);
			this.BtnDelete.BackColor = Color.Transparent;
			this.BtnDelete.Blue = 2f;
			this.BtnDelete.Cursor = Cursors.Hand;
			this.BtnDelete.Font = new System.Drawing.Font("Tahoma", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.BtnDelete.Green = 2f;
			this.BtnDelete.Image = (Bitmap)resourceManager.GetObject("BtnDelete.Image");
			this.BtnDelete.ImageClick = (Bitmap)resourceManager.GetObject("BtnDelete.ImageClick");
			this.BtnDelete.ImageClickIndex = 1;
			this.BtnDelete.ImageIndex = 0;
			this.BtnDelete.ImageList = this.ButtonLiteImgList;
			this.BtnDelete.Location = new Point(578, 32);
			this.BtnDelete.Name = "BtnDelete";
			this.BtnDelete.ObjectValue = null;
			this.BtnDelete.Red = 1f;
			this.BtnDelete.Size = new System.Drawing.Size(110, 40);
			this.BtnDelete.TabIndex = 37;
			this.BtnDelete.Text = "Delete";
			this.BtnDelete.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnDelete.Visible = false;
			this.BtnDelete.Click += new EventHandler(this.BtnDelete_Click);
			this.BtnOk.BackColor = Color.Transparent;
			this.BtnOk.Blue = 1.75f;
			this.BtnOk.Cursor = Cursors.Hand;
			this.BtnOk.Font = new System.Drawing.Font("Tahoma", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 222);
			this.BtnOk.Green = 1f;
			this.BtnOk.Image = (Bitmap)resourceManager.GetObject("BtnOk.Image");
			this.BtnOk.ImageClick = (Bitmap)resourceManager.GetObject("BtnOk.ImageClick");
			this.BtnOk.ImageClickIndex = 1;
			this.BtnOk.ImageIndex = 0;
			this.BtnOk.ImageList = this.ButtonImgList;
			this.BtnOk.Location = new Point(464, 544);
			this.BtnOk.Name = "BtnOk";
			this.BtnOk.ObjectValue = null;
			this.BtnOk.Red = 1.75f;
			this.BtnOk.Size = new System.Drawing.Size(112, 60);
			this.BtnOk.TabIndex = 42;
			this.BtnOk.Text = "OK";
			this.BtnOk.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnOk.Click += new EventHandler(this.BtnOk_Click);
			this.BtnCancel.BackColor = Color.Transparent;
			this.BtnCancel.Blue = 2f;
			this.BtnCancel.Cursor = Cursors.Hand;
			this.BtnCancel.Font = new System.Drawing.Font("Tahoma", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 222);
			this.BtnCancel.Green = 2f;
			this.BtnCancel.Image = (Bitmap)resourceManager.GetObject("BtnCancel.Image");
			this.BtnCancel.ImageClick = (Bitmap)resourceManager.GetObject("BtnCancel.ImageClick");
			this.BtnCancel.ImageClickIndex = 1;
			this.BtnCancel.ImageIndex = 0;
			this.BtnCancel.ImageList = this.ButtonImgList;
			this.BtnCancel.Location = new Point(576, 544);
			this.BtnCancel.Name = "BtnCancel";
			this.BtnCancel.ObjectValue = null;
			this.BtnCancel.Red = 1f;
			this.BtnCancel.Size = new System.Drawing.Size(110, 60);
			this.BtnCancel.TabIndex = 41;
			this.BtnCancel.Text = "Cancel";
			this.BtnCancel.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnCancel.Click += new EventHandler(this.BtnCancel_Click);
			this.AutoScaleBaseSize = new System.Drawing.Size(9, 23);
			this.BackColor = Color.White;
			base.ClientSize = new System.Drawing.Size(704, 616);
			Control.ControlCollection controlCollections = base.Controls;
			fieldMemo = new Control[] { this.ListCustName, this.ListCustPhone, this.LblHeaderName, this.LblHeaderPhone, this.BtnCustList, this.BtnCustSearch, this.BtnCustDown, this.BtnCustUp, this.PanCustField, this.BtnOk, this.BtnCancel, this.BtnDelete };
			controlCollections.AddRange(fieldMemo);
			this.Font = new System.Drawing.Font("Tahoma", 14.25f, FontStyle.Regular, GraphicsUnit.Point, 222);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			base.Name = "SearchCustomerForm";
			base.ShowInTaskbar = false;
			base.StartPosition = FormStartPosition.CenterParent;
			this.Text = "Search Customer";
			this.PanCustField.ResumeLayout(false);
			base.ResumeLayout(false);
		}

		private void ListCustPhone_ItemClick(object sender, ItemsListEventArgs e)
		{
			if (e.Item.Value is CustomerInformation)
			{
				this.selectedCust = (CustomerInformation)e.Item.Value;
				this.UpdateCustomerField();
			}
		}

		private void NumberKeyPad_PadClick(object sender, NumberPadEventArgs e)
		{
			if (e.IsNumeric)
			{
				Label fieldPhone = this.FieldPhone;
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

		protected override void OnPaint(PaintEventArgs pe)
		{
			Graphics graphics = pe.Graphics;
			Rectangle rectangle = new Rectangle(0, 0, base.Width, 29);
			LinearGradientBrush linearGradientBrush = new LinearGradientBrush(rectangle, Color.FromArgb(103, 138, 198), Color.White, 90f);
			graphics.FillRectangle(linearGradientBrush, rectangle);
			rectangle = new Rectangle(0, 30, base.Width, base.Height - 30);
			linearGradientBrush = new LinearGradientBrush(rectangle, Color.FromArgb(230, 230, 230), Color.White, 180f);
			graphics.FillRectangle(linearGradientBrush, rectangle);
			Pen pen = new Pen(Color.FromArgb(180, 180, 180));
			graphics.DrawLine(pen, 0, 29, base.Width - 1, 29);
			graphics.DrawRectangle(pen, 0, 0, base.Width - 1, base.Height - 1);
			graphics.DrawString(this.Text, this.Font, Brushes.Black, 15f, 4f);
			base.OnPaint(pe);
		}

		public static string Show(string customer)
		{
			if (SearchCustomerForm.instance == null)
			{
				SearchCustomerForm.instance = new SearchCustomerForm();
			}
			SearchCustomerForm.instance.CustomerName = customer;
			SearchCustomerForm.instance.UpdateForm();
			SearchCustomerForm.instance.ShowDialog();
			if (!SearchCustomerForm.instance.dialogResult)
			{
				return null;
			}
			return SearchCustomerForm.instance.CustomerName.Trim();
		}

		private void UpdateCustomerButton()
		{
			this.BtnOk.Enabled = this.FieldFName.Text != "";
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
				this.FieldPhone.Text = this.selectedCust.Telephone;
				this.FieldFName.Text = this.selectedCust.FirstName;
				this.FieldMName.Text = this.selectedCust.MiddleName;
				this.FieldLName.Text = this.selectedCust.LastName;
				this.FieldAddress.Text = this.selectedCust.Address;
				this.FieldMemo.Text = this.selectedCust.Description;
			}
			this.UpdateCustomerButton();
		}

		private void UpdateCustomerList()
		{
			StringBuilder stringBuilder = new StringBuilder();
			this.ListCustPhone.AutoRefresh = false;
			this.ListCustName.AutoRefresh = false;
			this.ListCustPhone.Items.Clear();
			this.ListCustName.Items.Clear();
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

		public void UpdateForm()
		{
			if (this.FieldFName.Text == "")
			{
				this.BtnCustList_Click(null, null);
				return;
			}
			this.BtnCustSearch_Click(null, null);
		}
	}
}