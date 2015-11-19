using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using smartRestaurant.Controls;
using smartRestaurant.Utils;
using System.Resources;
using smartRestaurant.CustomerService;

namespace smartRestaurant
{
	/// <summary>
	/// <b>SearchCustomerForm</b> use for select customer name or create new customer.
	/// </summary>
	public class SearchCustomerForm : Form
	{
		// Fields
		private ImageButton BtnCancel;
		private ImageButton BtnClear;
		private ImageButton BtnCustDown;
		private ImageButton BtnCustList;
		private ImageButton BtnCustSearch;
		private ImageButton BtnCustUp;
		private ImageButton BtnDelete;
		private ImageButton BtnOk;
		private ImageButton BtnSave;
		private ImageList ButtonImgList;
		private ImageList ButtonLiteImgList;
		private IContainer components;
		private CustomerService.CustomerInformation[] custList;
		private bool dialogResult;
		private Label FieldAddress;
		private Label FieldFName;
		private Label FieldLName;
		private Label FieldMemo;
		private Label FieldMName;
		private Label FieldPhone;
		private const int INPUT_ADDRESS = 3;
		private const int INPUT_FIRSTNAME = 0;
		private const int INPUT_LASTNAME = 2;
		private const int INPUT_MEMO = 4;
		private const int INPUT_MIDDLENAME = 1;
		private int inputState;
		private static SearchCustomerForm instance = null;
		private Label LblAddress;
		private Label LblFName;
		private Label LblHeaderName;
		private Label LblHeaderPhone;
		private Label LblLName;
		private Label LblMemo;
		private Label LblMName;
		private Label LblPhone;
		private ItemsList ListCustName;
		private ItemsList ListCustPhone;
		private ImageList NumberImgList;
		private NumberPad NumberKeyPad;
		private GroupPanel PanCustField;
		private CustomerService.CustomerInformation selectedCust;

		// Methods
		public SearchCustomerForm()
		{
			this.InitializeComponent();
			this.selectedCust = null;
		}

		private void BtnCancel_Click(object sender, EventArgs e)
		{
			this.dialogResult = false;
			instance.Close();
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
				string text = new smartRestaurant.CustomerService.CustomerService().DeleteCustomer (this.selectedCust.CustID);
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

		private void BtnOk_Click(object sender, EventArgs e)
		{
			this.dialogResult = true;
			instance.Close();
		}

		private void BtnSave_Click(object sender, EventArgs e)
		{
			if (this.FieldFName.Text == "")
			{
				MessageBox.Show("Please fill first name.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
			else
			{
				CustomerInformation selectedCust;
				if (this.selectedCust != null)
				{
					selectedCust = this.selectedCust;
				}
				else
				{
					selectedCust = new CustomerInformation();
					selectedCust.CustID = 0;
				}
				selectedCust.Telephone = this.FieldPhone.Text;
				selectedCust.FirstName = this.FieldFName.Text;
				selectedCust.MiddleName = this.FieldMName.Text;
				selectedCust.LastName = this.FieldLName.Text;
				selectedCust.Address = this.FieldAddress.Text;
				selectedCust.Description = this.FieldMemo.Text;
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
						this.FieldMemo.Text = result;
						break;
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
			if (disposing && (this.components != null))
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
			this.components = new Container();
			ResourceManager manager = new ResourceManager(typeof(SearchCustomerForm));
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
			this.NumberImgList.ImageSize = new Size(0x48, 60);
			this.NumberImgList.ImageStream = (ImageListStreamer) manager.GetObject("NumberImgList.ImageStream");
			this.NumberImgList.TransparentColor = Color.Transparent;
			this.ButtonImgList.ColorDepth = ColorDepth.Depth32Bit;
			this.ButtonImgList.ImageSize = new Size(110, 60);
			this.ButtonImgList.ImageStream = (ImageListStreamer) manager.GetObject("ButtonImgList.ImageStream");
			this.ButtonImgList.TransparentColor = Color.Transparent;
			this.ButtonLiteImgList.ColorDepth = ColorDepth.Depth32Bit;
			this.ButtonLiteImgList.ImageSize = new Size(110, 40);
			this.ButtonLiteImgList.ImageStream = (ImageListStreamer) manager.GetObject("ButtonLiteImgList.ImageStream");
			this.ButtonLiteImgList.TransparentColor = Color.Transparent;
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
			this.ListCustName.Location = new Point(0x1f0, 0x70);
			this.ListCustName.Name = "ListCustName";
			this.ListCustName.Row = 9;
			this.ListCustName.SelectedIndex = 0;
			this.ListCustName.Size = new Size(0xc4, 360);
			this.ListCustName.TabIndex = 0x33;
			this.ListCustName.ItemClick += new smartRestaurant.Controls.ItemsList.ItemsListEventHandler(this.ListCustPhone_ItemClick);
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
			this.ListCustPhone.Location = new Point(0x160, 0x70);
			this.ListCustPhone.Name = "ListCustPhone";
			this.ListCustPhone.Row = 9;
			this.ListCustPhone.SelectedIndex = 0;
			this.ListCustPhone.Size = new Size(0x90, 360);
			this.ListCustPhone.TabIndex = 0x2c;
			this.ListCustPhone.ItemClick += new smartRestaurant.Controls.ItemsList.ItemsListEventHandler(this.ListCustPhone_ItemClick);
			this.LblHeaderName.BackColor = Color.Black;
			this.LblHeaderName.Font = new Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.LblHeaderName.ForeColor = Color.White;
			this.LblHeaderName.Location = new Point(0x1f0, 0x48);
			this.LblHeaderName.Name = "LblHeaderName";
			this.LblHeaderName.Size = new Size(0xc4, 40);
			this.LblHeaderName.TabIndex = 50;
			this.LblHeaderName.Text = "Name";
			this.LblHeaderName.TextAlign = ContentAlignment.MiddleCenter;
			this.LblHeaderPhone.BackColor = Color.Black;
			this.LblHeaderPhone.Font = new Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.LblHeaderPhone.ForeColor = Color.White;
			this.LblHeaderPhone.Location = new Point(0x160, 0x48);
			this.LblHeaderPhone.Name = "LblHeaderPhone";
			this.LblHeaderPhone.Size = new Size(0x90, 40);
			this.LblHeaderPhone.TabIndex = 0x31;
			this.LblHeaderPhone.Text = "Phone#";
			this.LblHeaderPhone.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnCustList.BackColor = Color.Transparent;
			this.BtnCustList.Blue = 1f;
			this.BtnCustList.Cursor = Cursors.Hand;
			this.BtnCustList.Font = new Font("Tahoma", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0xde);
			this.BtnCustList.Green = 1f;
			this.BtnCustList.Image = (Bitmap) manager.GetObject("BtnCustList.Image");
			this.BtnCustList.ImageClick = (Bitmap) manager.GetObject("BtnCustList.ImageClick");
			this.BtnCustList.ImageClickIndex = 1;
			this.BtnCustList.ImageIndex = 0;
			this.BtnCustList.ImageList = this.ButtonLiteImgList;
			this.BtnCustList.Location = new Point(0x1d2, 0x20);
			this.BtnCustList.Name = "BtnCustList";
			this.BtnCustList.ObjectValue = null;
			this.BtnCustList.Red = 2f;
			this.BtnCustList.Size = new Size(110, 40);
			this.BtnCustList.TabIndex = 0x30;
			this.BtnCustList.Text = "List All";
			this.BtnCustList.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnCustList.Click += new EventHandler(this.BtnCustList_Click);
			this.BtnCustSearch.BackColor = Color.Transparent;
			this.BtnCustSearch.Blue = 1f;
			this.BtnCustSearch.Cursor = Cursors.Hand;
			this.BtnCustSearch.Font = new Font("Tahoma", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0xde);
			this.BtnCustSearch.Green = 1f;
			this.BtnCustSearch.Image = (Bitmap) manager.GetObject("BtnCustSearch.Image");
			this.BtnCustSearch.ImageClick = (Bitmap) manager.GetObject("BtnCustSearch.ImageClick");
			this.BtnCustSearch.ImageClickIndex = 1;
			this.BtnCustSearch.ImageIndex = 0;
			this.BtnCustSearch.ImageList = this.ButtonLiteImgList;
			this.BtnCustSearch.Location = new Point(0x162, 0x20);
			this.BtnCustSearch.Name = "BtnCustSearch";
			this.BtnCustSearch.ObjectValue = null;
			this.BtnCustSearch.Red = 2f;
			this.BtnCustSearch.Size = new Size(110, 40);
			this.BtnCustSearch.TabIndex = 0x2f;
			this.BtnCustSearch.Text = "Search";
			this.BtnCustSearch.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnCustSearch.Click += new EventHandler(this.BtnCustSearch_Click);
			this.BtnCustDown.BackColor = Color.Transparent;
			this.BtnCustDown.Blue = 1f;
			this.BtnCustDown.Cursor = Cursors.Hand;
			this.BtnCustDown.Green = 1f;
			this.BtnCustDown.Image = (Bitmap) manager.GetObject("BtnCustDown.Image");
			this.BtnCustDown.ImageClick = (Bitmap) manager.GetObject("BtnCustDown.ImageClick");
			this.BtnCustDown.ImageClickIndex = 5;
			this.BtnCustDown.ImageIndex = 4;
			this.BtnCustDown.ImageList = this.ButtonImgList;
			this.BtnCustDown.Location = new Point(0x240, 480);
			this.BtnCustDown.Name = "BtnCustDown";
			this.BtnCustDown.ObjectValue = null;
			this.BtnCustDown.Red = 2f;
			this.BtnCustDown.Size = new Size(110, 60);
			this.BtnCustDown.TabIndex = 0x2e;
			this.BtnCustDown.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnCustDown.Click += new EventHandler(this.BtnCustDown_Click);
			this.BtnCustUp.BackColor = Color.Transparent;
			this.BtnCustUp.Blue = 1f;
			this.BtnCustUp.Cursor = Cursors.Hand;
			this.BtnCustUp.Green = 1f;
			this.BtnCustUp.Image = (Bitmap) manager.GetObject("BtnCustUp.Image");
			this.BtnCustUp.ImageClick = (Bitmap) manager.GetObject("BtnCustUp.ImageClick");
			this.BtnCustUp.ImageClickIndex = 3;
			this.BtnCustUp.ImageIndex = 2;
			this.BtnCustUp.ImageList = this.ButtonImgList;
			this.BtnCustUp.Location = new Point(360, 480);
			this.BtnCustUp.Name = "BtnCustUp";
			this.BtnCustUp.ObjectValue = null;
			this.BtnCustUp.Red = 2f;
			this.BtnCustUp.Size = new Size(110, 60);
			this.BtnCustUp.TabIndex = 0x2d;
			this.BtnCustUp.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnCustUp.Click += new EventHandler(this.BtnCustUp_Click);
			this.PanCustField.BackColor = Color.Transparent;
			this.PanCustField.Caption = null;
			this.PanCustField.Controls.AddRange(new Control[] { this.FieldMemo, this.FieldAddress, this.FieldLName, this.FieldMName, this.FieldFName, this.FieldPhone, this.LblAddress, this.LblMemo, this.LblLName, this.LblMName, this.LblFName, this.LblPhone, this.NumberKeyPad, this.BtnSave, this.BtnClear });
			this.PanCustField.Font = new Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.PanCustField.Location = new Point(8, 0x20);
			this.PanCustField.Name = "PanCustField";
			this.PanCustField.ShowHeader = false;
			this.PanCustField.Size = new Size(0x158, 0x240);
			this.PanCustField.TabIndex = 0x2b;
			this.FieldMemo.BackColor = Color.FromArgb(0xff, 0xff, 0xc0);
			this.FieldMemo.BorderStyle = BorderStyle.FixedSingle;
			this.FieldMemo.Cursor = Cursors.Hand;
			this.FieldMemo.Location = new Point(0x60, 0xd8);
			this.FieldMemo.Name = "FieldMemo";
			this.FieldMemo.Size = new Size(0xe8, 40);
			this.FieldMemo.TabIndex = 0x22;
			this.FieldMemo.TextAlign = ContentAlignment.MiddleLeft;
			this.FieldMemo.Click += new EventHandler(this.FieldMemo_Click);
			this.FieldAddress.BackColor = Color.FromArgb(0xff, 0xff, 0xc0);
			this.FieldAddress.BorderStyle = BorderStyle.FixedSingle;
			this.FieldAddress.Cursor = Cursors.Hand;
			this.FieldAddress.Location = new Point(0x60, 0xa8);
			this.FieldAddress.Name = "FieldAddress";
			this.FieldAddress.Size = new Size(0xe8, 0x30);
			this.FieldAddress.TabIndex = 0x21;
			this.FieldAddress.TextAlign = ContentAlignment.MiddleLeft;
			this.FieldAddress.Click += new EventHandler(this.FieldAddress_Click);
			this.FieldLName.BackColor = Color.FromArgb(0xff, 0xff, 0xc0);
			this.FieldLName.BorderStyle = BorderStyle.FixedSingle;
			this.FieldLName.Cursor = Cursors.Hand;
			this.FieldLName.Location = new Point(0x60, 0x80);
			this.FieldLName.Name = "FieldLName";
			this.FieldLName.Size = new Size(0xe8, 40);
			this.FieldLName.TabIndex = 0x20;
			this.FieldLName.TextAlign = ContentAlignment.MiddleLeft;
			this.FieldLName.Click += new EventHandler(this.FieldLName_Click);
			this.FieldMName.BackColor = Color.FromArgb(0xff, 0xff, 0xc0);
			this.FieldMName.BorderStyle = BorderStyle.FixedSingle;
			this.FieldMName.Cursor = Cursors.Hand;
			this.FieldMName.Location = new Point(0x60, 0x58);
			this.FieldMName.Name = "FieldMName";
			this.FieldMName.Size = new Size(0xe8, 40);
			this.FieldMName.TabIndex = 0x1f;
			this.FieldMName.TextAlign = ContentAlignment.MiddleLeft;
			this.FieldMName.Click += new EventHandler(this.FieldMName_Click);
			this.FieldFName.BackColor = Color.FromArgb(0xff, 0xff, 0xc0);
			this.FieldFName.BorderStyle = BorderStyle.FixedSingle;
			this.FieldFName.Cursor = Cursors.Hand;
			this.FieldFName.Location = new Point(0x60, 0x30);
			this.FieldFName.Name = "FieldFName";
			this.FieldFName.Size = new Size(0xe8, 40);
			this.FieldFName.TabIndex = 30;
			this.FieldFName.TextAlign = ContentAlignment.MiddleLeft;
			this.FieldFName.Click += new EventHandler(this.FieldFName_Click);
			this.FieldPhone.BackColor = Color.FromArgb(0xff, 0xff, 0xc0);
			this.FieldPhone.BorderStyle = BorderStyle.FixedSingle;
			this.FieldPhone.Cursor = Cursors.Hand;
			this.FieldPhone.Location = new Point(0x60, 8);
			this.FieldPhone.Name = "FieldPhone";
			this.FieldPhone.Size = new Size(0xe8, 40);
			this.FieldPhone.TabIndex = 0x1d;
			this.FieldPhone.TextAlign = ContentAlignment.MiddleLeft;
			this.LblAddress.Location = new Point(0x10, 0xa8);
			this.LblAddress.Name = "LblAddress";
			this.LblAddress.Size = new Size(80, 40);
			this.LblAddress.TabIndex = 0x18;
			this.LblAddress.Text = "Address";
			this.LblAddress.TextAlign = ContentAlignment.MiddleLeft;
			this.LblMemo.Location = new Point(0x10, 0xd8);
			this.LblMemo.Name = "LblMemo";
			this.LblMemo.Size = new Size(80, 40);
			this.LblMemo.TabIndex = 0x16;
			this.LblMemo.Text = "Memo";
			this.LblMemo.TextAlign = ContentAlignment.MiddleLeft;
			this.LblLName.Location = new Point(0x10, 0x80);
			this.LblLName.Name = "LblLName";
			this.LblLName.Size = new Size(80, 40);
			this.LblLName.TabIndex = 20;
			this.LblLName.Text = "L.Name";
			this.LblLName.TextAlign = ContentAlignment.MiddleLeft;
			this.LblMName.Location = new Point(0x10, 0x58);
			this.LblMName.Name = "LblMName";
			this.LblMName.Size = new Size(80, 40);
			this.LblMName.TabIndex = 0x13;
			this.LblMName.Text = "M.Name";
			this.LblMName.TextAlign = ContentAlignment.MiddleLeft;
			this.LblFName.Location = new Point(0x10, 0x30);
			this.LblFName.Name = "LblFName";
			this.LblFName.Size = new Size(80, 40);
			this.LblFName.TabIndex = 0x12;
			this.LblFName.Text = "F.Name";
			this.LblFName.TextAlign = ContentAlignment.MiddleLeft;
			this.LblPhone.Location = new Point(0x10, 8);
			this.LblPhone.Name = "LblPhone";
			this.LblPhone.Size = new Size(80, 40);
			this.LblPhone.TabIndex = 0x11;
			this.LblPhone.Text = "Phone#";
			this.LblPhone.TextAlign = ContentAlignment.MiddleLeft;
			this.NumberKeyPad.BackColor = Color.White;
			this.NumberKeyPad.Image = (Bitmap) manager.GetObject("NumberKeyPad.Image");
			this.NumberKeyPad.ImageClick = (Bitmap) manager.GetObject("NumberKeyPad.ImageClick");
			this.NumberKeyPad.ImageClickIndex = 1;
			this.NumberKeyPad.ImageIndex = 0;
			this.NumberKeyPad.ImageList = this.NumberImgList;
			this.NumberKeyPad.Location = new Point(0x40, 0x138);
			this.NumberKeyPad.Name = "NumberKeyPad";
			this.NumberKeyPad.Size = new Size(0xe2, 0xff);
			this.NumberKeyPad.TabIndex = 7;
			this.NumberKeyPad.Text = "numberPad1";
			this.NumberKeyPad.PadClick += new smartRestaurant.Controls.NumberPad.NumberPadEventHandler(this.NumberKeyPad_PadClick);
			this.BtnSave.BackColor = Color.Transparent;
			this.BtnSave.Blue = 2f;
			this.BtnSave.Cursor = Cursors.Hand;
			this.BtnSave.Font = new Font("Tahoma", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.BtnSave.Green = 1f;
			this.BtnSave.Image = (Bitmap) manager.GetObject("BtnSave.Image");
			this.BtnSave.ImageClick = (Bitmap) manager.GetObject("BtnSave.ImageClick");
			this.BtnSave.ImageClickIndex = 1;
			this.BtnSave.ImageIndex = 0;
			this.BtnSave.ImageList = this.ButtonLiteImgList;
			this.BtnSave.Location = new Point(0xd8, 0x108);
			this.BtnSave.Name = "BtnSave";
			this.BtnSave.ObjectValue = null;
			this.BtnSave.Red = 2f;
			this.BtnSave.Size = new Size(0x70, 40);
			this.BtnSave.TabIndex = 0x24;
			this.BtnSave.Text = "Save";
			this.BtnSave.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnSave.Click += new EventHandler(this.BtnSave_Click);
			this.BtnClear.BackColor = Color.Transparent;
			this.BtnClear.Blue = 2f;
			this.BtnClear.Cursor = Cursors.Hand;
			this.BtnClear.Font = new Font("Tahoma", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.BtnClear.Green = 1f;
			this.BtnClear.Image = (Bitmap) manager.GetObject("BtnClear.Image");
			this.BtnClear.ImageClick = (Bitmap) manager.GetObject("BtnClear.ImageClick");
			this.BtnClear.ImageClickIndex = 1;
			this.BtnClear.ImageIndex = 0;
			this.BtnClear.ImageList = this.ButtonLiteImgList;
			this.BtnClear.Location = new Point(0x10, 0x108);
			this.BtnClear.Name = "BtnClear";
			this.BtnClear.ObjectValue = null;
			this.BtnClear.Red = 1f;
			this.BtnClear.Size = new Size(110, 40);
			this.BtnClear.TabIndex = 0x23;
			this.BtnClear.Text = "Clear";
			this.BtnClear.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnClear.Click += new EventHandler(this.BtnClear_Click);
			this.BtnDelete.BackColor = Color.Transparent;
			this.BtnDelete.Blue = 2f;
			this.BtnDelete.Cursor = Cursors.Hand;
			this.BtnDelete.Font = new Font("Tahoma", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.BtnDelete.Green = 2f;
			this.BtnDelete.Image = (Bitmap) manager.GetObject("BtnDelete.Image");
			this.BtnDelete.ImageClick = (Bitmap) manager.GetObject("BtnDelete.ImageClick");
			this.BtnDelete.ImageClickIndex = 1;
			this.BtnDelete.ImageIndex = 0;
			this.BtnDelete.ImageList = this.ButtonLiteImgList;
			this.BtnDelete.Location = new Point(0x242, 0x20);
			this.BtnDelete.Name = "BtnDelete";
			this.BtnDelete.ObjectValue = null;
			this.BtnDelete.Red = 1f;
			this.BtnDelete.Size = new Size(110, 40);
			this.BtnDelete.TabIndex = 0x25;
			this.BtnDelete.Text = "Delete";
			this.BtnDelete.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnDelete.Visible = false;
			this.BtnDelete.Click += new EventHandler(this.BtnDelete_Click);
			this.BtnOk.BackColor = Color.Transparent;
			this.BtnOk.Blue = 1.75f;
			this.BtnOk.Cursor = Cursors.Hand;
			this.BtnOk.Font = new Font("Tahoma", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0xde);
			this.BtnOk.Green = 1f;
			this.BtnOk.Image = (Bitmap) manager.GetObject("BtnOk.Image");
			this.BtnOk.ImageClick = (Bitmap) manager.GetObject("BtnOk.ImageClick");
			this.BtnOk.ImageClickIndex = 1;
			this.BtnOk.ImageIndex = 0;
			this.BtnOk.ImageList = this.ButtonImgList;
			this.BtnOk.Location = new Point(0x1d0, 0x220);
			this.BtnOk.Name = "BtnOk";
			this.BtnOk.ObjectValue = null;
			this.BtnOk.Red = 1.75f;
			this.BtnOk.Size = new Size(0x70, 60);
			this.BtnOk.TabIndex = 0x2a;
			this.BtnOk.Text = "OK";
			this.BtnOk.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnOk.Click += new EventHandler(this.BtnOk_Click);
			this.BtnCancel.BackColor = Color.Transparent;
			this.BtnCancel.Blue = 2f;
			this.BtnCancel.Cursor = Cursors.Hand;
			this.BtnCancel.Font = new Font("Tahoma", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0xde);
			this.BtnCancel.Green = 2f;
			this.BtnCancel.Image = (Bitmap) manager.GetObject("BtnCancel.Image");
			this.BtnCancel.ImageClick = (Bitmap) manager.GetObject("BtnCancel.ImageClick");
			this.BtnCancel.ImageClickIndex = 1;
			this.BtnCancel.ImageIndex = 0;
			this.BtnCancel.ImageList = this.ButtonImgList;
			this.BtnCancel.Location = new Point(0x240, 0x220);
			this.BtnCancel.Name = "BtnCancel";
			this.BtnCancel.ObjectValue = null;
			this.BtnCancel.Red = 1f;
			this.BtnCancel.Size = new Size(110, 60);
			this.BtnCancel.TabIndex = 0x29;
			this.BtnCancel.Text = "Cancel";
			this.BtnCancel.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnCancel.Click += new EventHandler(this.BtnCancel_Click);
			this.AutoScaleBaseSize = new Size(9, 0x17);
			this.BackColor = Color.White;
			base.ClientSize = new Size(0x2c0, 0x268);
			base.Controls.AddRange(new Control[] { this.ListCustName, this.ListCustPhone, this.LblHeaderName, this.LblHeaderPhone, this.BtnCustList, this.BtnCustSearch, this.BtnCustDown, this.BtnCustUp, this.PanCustField, this.BtnOk, this.BtnCancel, this.BtnDelete });
			this.Font = new Font("Tahoma", 14.25f, FontStyle.Regular, GraphicsUnit.Point, 0xde);
			base.FormBorderStyle = FormBorderStyle.None;
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
				this.selectedCust = (CustomerInformation) e.Item.Value;
				this.UpdateCustomerField();
			}
		}

		private void NumberKeyPad_PadClick(object sender, NumberPadEventArgs e)
		{
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

		protected override void OnPaint(PaintEventArgs pe)
		{
			Graphics graphics = pe.Graphics;
			Rectangle rect = new Rectangle(0, 0, base.Width, 0x1d);
			LinearGradientBrush brush = new LinearGradientBrush(rect, Color.FromArgb(0x67, 0x8a, 0xc6), Color.White, 90f);
			graphics.FillRectangle(brush, rect);
			rect = new Rectangle(0, 30, base.Width, base.Height - 30);
			brush = new LinearGradientBrush(rect, Color.FromArgb(230, 230, 230), Color.White, 180f);
			graphics.FillRectangle(brush, rect);
			Pen pen = new Pen(Color.FromArgb(180, 180, 180));
			graphics.DrawLine(pen, 0, 0x1d, base.Width - 1, 0x1d);
			graphics.DrawRectangle(pen, 0, 0, base.Width - 1, base.Height - 1);
			graphics.DrawString(this.Text, this.Font, Brushes.Black, (float) 15f, (float) 4f);
			base.OnPaint(pe);
		}

		public static string Show(string customer)
		{
			if (instance == null)
			{
				instance = new SearchCustomerForm();
			}
			instance.CustomerName = customer;
			instance.UpdateForm();
			instance.ShowDialog();
			if (instance.dialogResult)
			{
				return instance.CustomerName.Trim();
			}
			return null;
		}

		private void UpdateCustomerButton()
		{
			this.BtnOk.Enabled = this.FieldFName.Text != "";
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
			StringBuilder builder = new StringBuilder();
			this.ListCustPhone.AutoRefresh = false;
			this.ListCustName.AutoRefresh = false;
			this.ListCustPhone.Items.Clear();
			this.ListCustName.Items.Clear();
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

		public void UpdateForm()
		{
			if (this.FieldFName.Text == "")
			{
				this.BtnCustList_Click(null, null);
			}
			else
			{
				this.BtnCustSearch_Click(null, null);
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
	}

 

}
