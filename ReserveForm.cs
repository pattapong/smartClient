using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using smartRestaurant.Controls;
using smartRestaurant.ReserveService;
using smartRestaurant.Utils;
using System.Resources;
using smartRestaurant.TableService;

namespace smartRestaurant
{
	/// <summary>
	/// Summary description for ReserveForm.
	/// </summary>
	public class ReserveForm : SmartForm
	{
		// Fields
		private ImageButton BtnCancel;
		private ImageButton BtnDayDown;
		private ImageButton BtnDayUp;
		private ImageButton BtnDinIn;
		private ImageButton BtnDown;
		private ImageButton BtnGotoToday;
		private ImageButton BtnMain;
		private ImageButton BtnReserve;
		private ImageButton BtnSearch;
		private ImageButton BtnUp;
		private ImageList ButtonImgList;
		private ImageList ButtonLiteImgList;
		private IContainer components;
		private ButtonListPad DatePad;
		private int employeeID;
		private static string FIELD_CUST_TEXT = "- Please input name -";
		private Label FieldCustName;
		private Label FieldInputType;
		private Label FieldSeat;
		private GroupPanel GroupDate;
		private GroupPanel groupPanel2;
		private ButtonListPad HourPad;
		private Label LblCopyright;
		private Label LblCustName;
		private Label LblHeaderCustomer;
		private Label LblHeaderHour;
		private Label LblHeaderNumber;
		private Label LblHeaderSeat;
		private Label LblHeaderTime;
		private Label LblMinute;
		private Label LblPageID;
		private Label LblReserveInfo;
		private Label LblSelectedDate;
		private ItemsList ListReserveID;
		private ItemsList ListReserveQueue;
		private ItemsList ListReserveSeat;
		private ItemsList ListReserveTime;
		private ButtonListPad MinutePad;
		private ImageList NumberImgList;
		private NumberPad NumberKeyPad;
		private GroupPanel PanCustName;
		private GroupPanel PanSelectTime;
		private TableReserve[] reserveList;
		private DateTime selectedDate;
		private TableReserve selectedReserve;
		private DateTime startDate;

		// Methods
		public ReserveForm()
		{
			this.InitializeComponent();
			this.UpdateTimeButton();
		}

		private void BtnCancel_Click(object sender, EventArgs e)
		{
			new smartRestaurant.ReserveService.ReserveService().SetReserveCancel(this.selectedReserve.reserveTableID.ToString());
			this.UpdateReserveQueue();
		}

		private void BtnDayDown_Click(object sender, EventArgs e)
		{
			this.startDate = this.startDate.AddDays(7.0);
			this.UpdateDateButton();
			this.UpdateSelectedTime();
		}

		private void BtnDayUp_Click(object sender, EventArgs e)
		{
			this.startDate = this.startDate.AddDays(-7.0);
			this.UpdateDateButton();
			this.UpdateSelectedTime();
		}

		private void BtnDinIn_Click(object sender, EventArgs e)
		{
			if (this.selectedReserve != null)
			{
				TableInformation table = TableForm.Show("Reserve");
				if (table != null)
				{
					new smartRestaurant.ReserveService.ReserveService().SetReserveDinIn(this.selectedReserve.reserveTableID.ToString(), table.TableID.ToString());
					table.NumberOfSeat = this.selectedReserve.seat;
					((MainForm) base.MdiParent).ShowTakeOrderForm(table);
				}
			}
		}

		private void BtnGotoToday_Click(object sender, EventArgs e)
		{
			this.selectedDate = DateTime.Today.Add(new TimeSpan(this.selectedDate.Hour, this.selectedDate.Minute, 0));
			this.startDate = this.SearchStartOfWeek(this.selectedDate);
			this.UpdateDateButton();
			this.UpdateSelectedTime();
		}

		private void BtnMain_Click(object sender, EventArgs e)
		{
			((MainForm) base.MdiParent).ShowMainMenuForm();
		}

		private void BtnReserve_Click(object sender, EventArgs e)
		{
			TableReserve reserve = new TableReserve();
			reserve.reserveTableID = 0;
			reserve.tableID = 0;
			reserve.customerID = 0;
			reserve.seat = int.Parse(this.FieldSeat.Text);
			reserve.reserveDate = this.selectedDate;
			string text = new smartRestaurant.ReserveService.ReserveService().SetTableReserve(reserve, this.FieldCustName.Text);
			if (text != null)
			{
				MessageBox.Show(text);
			}
			else
			{
				this.FieldCustName.Text = FIELD_CUST_TEXT;
				this.FieldSeat.Text = "";
				this.UpdateReserveQueue();
				this.UpdateReserveButton();
			}
		}

		private void BtnSearch_Click(object sender, EventArgs e)
		{
			string text;
			if (this.FieldCustName.Text == FIELD_CUST_TEXT)
			{
				text = "";
			}
			else
			{
				text = this.FieldCustName.Text;
			}
			string str2 = SearchCustomerForm.Show(text);
			if (str2 != null)
			{
				this.FieldCustName.Text = str2;
				this.UpdateReserveButton();
			}
		}

		private void DatePad_PadClick(object sender, ButtonListPadEventArgs e)
		{
			int num = int.Parse(e.Value);
			this.selectedDate = this.startDate.AddDays((double) num).Add(new TimeSpan(this.selectedDate.Hour, this.selectedDate.Minute, 0));
			this.UpdateDateButton();
			this.UpdateSelectedTime();
			this.UpdateReserveButton();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && (this.components != null))
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void FieldCustName_Click(object sender, EventArgs e)
		{
			string text;
			if (this.FieldCustName.Text == FIELD_CUST_TEXT)
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
				this.FieldCustName.Text = str2;
				this.UpdateReserveButton();
			}
		}

		private void HourPad_PadClick(object sender, ButtonListPadEventArgs e)
		{
			int num = int.Parse(e.Value);
			this.selectedDate = this.selectedDate.AddHours((double) (num - this.selectedDate.Hour));
			this.UpdateSelectedTime();
		}

		private void InitializeComponent()
		{
			this.components = new Container();
			ResourceManager manager = new ResourceManager(typeof(ReserveForm));
			this.BtnSearch = new ImageButton();
			this.ButtonImgList = new ImageList(this.components);
			this.PanCustName = new GroupPanel();
			this.FieldCustName = new Label();
			this.NumberImgList = new ImageList(this.components);
			this.LblCopyright = new Label();
			this.LblHeaderCustomer = new Label();
			this.BtnDown = new ImageButton();
			this.BtnUp = new ImageButton();
			this.ListReserveQueue = new ItemsList();
			this.ListReserveTime = new ItemsList();
			this.ListReserveID = new ItemsList();
			this.ListReserveSeat = new ItemsList();
			this.BtnDinIn = new ImageButton();
			this.BtnMain = new ImageButton();
			this.BtnReserve = new ImageButton();
			this.BtnCancel = new ImageButton();
			this.LblHeaderSeat = new Label();
			this.LblHeaderTime = new Label();
			this.LblPageID = new Label();
			this.PanSelectTime = new GroupPanel();
			this.LblMinute = new Label();
			this.LblHeaderHour = new Label();
			this.MinutePad = new ButtonListPad();
			this.HourPad = new ButtonListPad();
			this.groupPanel2 = new GroupPanel();
			this.BtnGotoToday = new ImageButton();
			this.ButtonLiteImgList = new ImageList(this.components);
			this.LblSelectedDate = new Label();
			this.LblCustName = new Label();
			this.FieldSeat = new Label();
			this.FieldInputType = new Label();
			this.NumberKeyPad = new NumberPad();
			this.LblReserveInfo = new Label();
			this.DatePad = new ButtonListPad();
			this.BtnDayDown = new ImageButton();
			this.BtnDayUp = new ImageButton();
			this.GroupDate = new GroupPanel();
			this.LblHeaderNumber = new Label();
			this.PanCustName.SuspendLayout();
			this.PanSelectTime.SuspendLayout();
			this.groupPanel2.SuspendLayout();
			this.GroupDate.SuspendLayout();
			base.SuspendLayout();
			this.BtnSearch.BackColor = Color.Transparent;
			this.BtnSearch.Blue = 0.5f;
			this.BtnSearch.Cursor = Cursors.Hand;
			this.BtnSearch.Font = new Font("Tahoma", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.BtnSearch.Green = 1f;
			this.BtnSearch.Image = (Bitmap) manager.GetObject("BtnSearch.Image");
			this.BtnSearch.ImageClick = (Bitmap) manager.GetObject("BtnSearch.ImageClick");
			this.BtnSearch.ImageClickIndex = 1;
			this.BtnSearch.ImageIndex = 0;
			this.BtnSearch.ImageList = this.ButtonImgList;
			this.BtnSearch.Location = new Point(0xd8, 0xb0);
			this.BtnSearch.Name = "BtnSearch";
			this.BtnSearch.ObjectValue = null;
			this.BtnSearch.Red = 1f;
			this.BtnSearch.Size = new Size(110, 60);
			this.BtnSearch.TabIndex = 0x1a;
			this.BtnSearch.Text = "Search";
			this.BtnSearch.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnSearch.Click += new EventHandler(this.BtnSearch_Click);
			this.ButtonImgList.ColorDepth = ColorDepth.Depth32Bit;
			this.ButtonImgList.ImageSize = new Size(110, 60);
			this.ButtonImgList.ImageStream = (ImageListStreamer) manager.GetObject("ButtonImgList.ImageStream");
			this.ButtonImgList.TransparentColor = Color.Transparent;
			this.PanCustName.BackColor = Color.Transparent;
			this.PanCustName.Caption = null;
			this.PanCustName.Controls.AddRange(new Control[] { this.FieldCustName });
			this.PanCustName.Font = new Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.PanCustName.Location = new Point(0x10, 0x70);
			this.PanCustName.Name = "PanCustName";
			this.PanCustName.ShowHeader = false;
			this.PanCustName.Size = new Size(0x138, 0x3a);
			this.PanCustName.TabIndex = 0x19;
			this.FieldCustName.Cursor = Cursors.Hand;
			this.FieldCustName.Location = new Point(1, 1);
			this.FieldCustName.Name = "FieldCustName";
			this.FieldCustName.Size = new Size(0x137, 0x38);
			this.FieldCustName.TabIndex = 0;
			this.FieldCustName.TextAlign = ContentAlignment.MiddleLeft;
			this.FieldCustName.Click += new EventHandler(this.FieldCustName_Click);
			this.NumberImgList.ColorDepth = ColorDepth.Depth32Bit;
			this.NumberImgList.ImageSize = new Size(0x48, 60);
			this.NumberImgList.ImageStream = (ImageListStreamer) manager.GetObject("NumberImgList.ImageStream");
			this.NumberImgList.TransparentColor = Color.Transparent;
			this.LblCopyright.BackColor = Color.Transparent;
			this.LblCopyright.Font = new Font("Tahoma", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.LblCopyright.ForeColor = Color.FromArgb(0x67, 0x8a, 0xc6);
			this.LblCopyright.Location = new Point(8, 0x2f0);
			this.LblCopyright.Name = "LblCopyright";
			this.LblCopyright.Size = new Size(280, 0x10);
			this.LblCopyright.TabIndex = 0x33;
			this.LblCopyright.Text = "Copyright (c) 2004. All rights reserved.";
			this.LblHeaderCustomer.BackColor = Color.Black;
			this.LblHeaderCustomer.Font = new Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.LblHeaderCustomer.ForeColor = Color.White;
			this.LblHeaderCustomer.Location = new Point(0x40, 0xc0);
			this.LblHeaderCustomer.Name = "LblHeaderCustomer";
			this.LblHeaderCustomer.Size = new Size(0xa8, 40);
			this.LblHeaderCustomer.TabIndex = 50;
			this.LblHeaderCustomer.Text = "Customer";
			this.LblHeaderCustomer.TextAlign = ContentAlignment.MiddleLeft;
			this.BtnDown.BackColor = Color.Transparent;
			this.BtnDown.Blue = 2f;
			this.BtnDown.Cursor = Cursors.Hand;
			this.BtnDown.Green = 1f;
			this.BtnDown.Image = (Bitmap) manager.GetObject("BtnDown.Image");
			this.BtnDown.ImageClick = (Bitmap) manager.GetObject("BtnDown.ImageClick");
			this.BtnDown.ImageClickIndex = 5;
			this.BtnDown.ImageIndex = 4;
			this.BtnDown.ImageList = this.ButtonImgList;
			this.BtnDown.Location = new Point(0xd0, 0x2b4);
			this.BtnDown.Name = "BtnDown";
			this.BtnDown.ObjectValue = null;
			this.BtnDown.Red = 2f;
			this.BtnDown.Size = new Size(110, 60);
			this.BtnDown.TabIndex = 0x31;
			this.BtnDown.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnUp.BackColor = Color.Transparent;
			this.BtnUp.Blue = 2f;
			this.BtnUp.Cursor = Cursors.Hand;
			this.BtnUp.Green = 1f;
			this.BtnUp.Image = (Bitmap) manager.GetObject("BtnUp.Image");
			this.BtnUp.ImageClick = (Bitmap) manager.GetObject("BtnUp.ImageClick");
			this.BtnUp.ImageClickIndex = 3;
			this.BtnUp.ImageIndex = 2;
			this.BtnUp.ImageList = this.ButtonImgList;
			this.BtnUp.Location = new Point(0x10, 0x2b4);
			this.BtnUp.Name = "BtnUp";
			this.BtnUp.ObjectValue = null;
			this.BtnUp.Red = 2f;
			this.BtnUp.Size = new Size(110, 60);
			this.BtnUp.TabIndex = 0x30;
			this.BtnUp.TextAlign = ContentAlignment.MiddleCenter;
			this.ListReserveQueue.Alignment = ContentAlignment.MiddleLeft;
			this.ListReserveQueue.AutoRefresh = true;
			this.ListReserveQueue.BackAlterColor = Color.FromArgb(0xc0, 0xff, 0xff);
			this.ListReserveQueue.BackHeaderColor = Color.FromArgb(0xff, 0xc0, 0x80);
			this.ListReserveQueue.BackHeaderSelectedColor = Color.FromArgb(0xc0, 0, 0);
			this.ListReserveQueue.BackNormalColor = Color.White;
			this.ListReserveQueue.BackSelectedColor = Color.Blue;
			this.ListReserveQueue.BindList1 = this.ListReserveTime;
			this.ListReserveQueue.BindList2 = this.ListReserveID;
			this.ListReserveQueue.Border = 0;
			this.ListReserveQueue.Font = new Font("Tahoma", 12f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.ListReserveQueue.ForeAlterColor = Color.Black;
			this.ListReserveQueue.ForeHeaderColor = Color.Black;
			this.ListReserveQueue.ForeHeaderSelectedColor = Color.White;
			this.ListReserveQueue.ForeNormalColor = Color.Black;
			this.ListReserveQueue.ForeSelectedColor = Color.White;
			this.ListReserveQueue.ItemHeight = 40;
			this.ListReserveQueue.ItemWidth = 0xa8;
			this.ListReserveQueue.Location = new Point(0x40, 0xe8);
			this.ListReserveQueue.Name = "ListReserveQueue";
			this.ListReserveQueue.Row = 11;
			this.ListReserveQueue.SelectedIndex = 0;
			this.ListReserveQueue.Size = new Size(0xa8, 440);
			this.ListReserveQueue.TabIndex = 0x2f;
			this.ListReserveQueue.ItemClick += new smartRestaurant.Controls.ItemsList.ItemsListEventHandler(this.ListReserveTime_ItemClick);
			this.ListReserveTime.Alignment = ContentAlignment.MiddleCenter;
			this.ListReserveTime.AutoRefresh = true;
			this.ListReserveTime.BackAlterColor = Color.FromArgb(0xc0, 0xff, 0xff);
			this.ListReserveTime.BackHeaderColor = Color.FromArgb(0xff, 0xc0, 0x80);
			this.ListReserveTime.BackHeaderSelectedColor = Color.FromArgb(0xc0, 0, 0);
			this.ListReserveTime.BackNormalColor = Color.White;
			this.ListReserveTime.BackSelectedColor = Color.Blue;
			this.ListReserveTime.BindList1 = null;
			this.ListReserveTime.BindList2 = this.ListReserveQueue;
			this.ListReserveTime.Border = 0;
			this.ListReserveTime.Font = new Font("Tahoma", 12f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.ListReserveTime.ForeAlterColor = Color.Black;
			this.ListReserveTime.ForeHeaderColor = Color.Black;
			this.ListReserveTime.ForeHeaderSelectedColor = Color.White;
			this.ListReserveTime.ForeNormalColor = Color.Black;
			this.ListReserveTime.ForeSelectedColor = Color.White;
			this.ListReserveTime.ItemHeight = 40;
			this.ListReserveTime.ItemWidth = 0x38;
			this.ListReserveTime.Location = new Point(8, 0xe8);
			this.ListReserveTime.Name = "ListReserveTime";
			this.ListReserveTime.Row = 11;
			this.ListReserveTime.SelectedIndex = 0;
			this.ListReserveTime.Size = new Size(0x38, 440);
			this.ListReserveTime.TabIndex = 0x39;
			this.ListReserveTime.ItemClick += new smartRestaurant.Controls.ItemsList.ItemsListEventHandler(this.ListReserveTime_ItemClick);
			this.ListReserveID.Alignment = ContentAlignment.MiddleCenter;
			this.ListReserveID.AutoRefresh = true;
			this.ListReserveID.BackAlterColor = Color.FromArgb(0xc0, 0xff, 0xff);
			this.ListReserveID.BackHeaderColor = Color.FromArgb(0xff, 0xc0, 0x80);
			this.ListReserveID.BackHeaderSelectedColor = Color.FromArgb(0xc0, 0, 0);
			this.ListReserveID.BackNormalColor = Color.White;
			this.ListReserveID.BackSelectedColor = Color.Blue;
			this.ListReserveID.BindList1 = this.ListReserveQueue;
			this.ListReserveID.BindList2 = this.ListReserveSeat;
			this.ListReserveID.Border = 0;
			this.ListReserveID.Font = new Font("Tahoma", 12f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.ListReserveID.ForeAlterColor = Color.Black;
			this.ListReserveID.ForeHeaderColor = Color.Black;
			this.ListReserveID.ForeHeaderSelectedColor = Color.White;
			this.ListReserveID.ForeNormalColor = Color.Black;
			this.ListReserveID.ForeSelectedColor = Color.White;
			this.ListReserveID.ItemHeight = 40;
			this.ListReserveID.ItemWidth = 0x30;
			this.ListReserveID.Location = new Point(0xe8, 0xe8);
			this.ListReserveID.Name = "ListReserveID";
			this.ListReserveID.Row = 11;
			this.ListReserveID.SelectedIndex = 0;
			this.ListReserveID.Size = new Size(0x30, 440);
			this.ListReserveID.TabIndex = 0x3f;
			this.ListReserveID.ItemClick += new smartRestaurant.Controls.ItemsList.ItemsListEventHandler(this.ListReserveTime_ItemClick);
			this.ListReserveSeat.Alignment = ContentAlignment.MiddleCenter;
			this.ListReserveSeat.AutoRefresh = true;
			this.ListReserveSeat.BackAlterColor = Color.FromArgb(0xc0, 0xff, 0xff);
			this.ListReserveSeat.BackHeaderColor = Color.FromArgb(0xff, 0xc0, 0x80);
			this.ListReserveSeat.BackHeaderSelectedColor = Color.FromArgb(0xc0, 0, 0);
			this.ListReserveSeat.BackNormalColor = Color.White;
			this.ListReserveSeat.BackSelectedColor = Color.Blue;
			this.ListReserveSeat.BindList1 = this.ListReserveID;
			this.ListReserveSeat.BindList2 = null;
			this.ListReserveSeat.Border = 0;
			this.ListReserveSeat.Font = new Font("Tahoma", 12f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.ListReserveSeat.ForeAlterColor = Color.Black;
			this.ListReserveSeat.ForeHeaderColor = Color.Black;
			this.ListReserveSeat.ForeHeaderSelectedColor = Color.White;
			this.ListReserveSeat.ForeNormalColor = Color.Black;
			this.ListReserveSeat.ForeSelectedColor = Color.White;
			this.ListReserveSeat.ItemHeight = 40;
			this.ListReserveSeat.ItemWidth = 0x30;
			this.ListReserveSeat.Location = new Point(280, 0xe8);
			this.ListReserveSeat.Name = "ListReserveSeat";
			this.ListReserveSeat.Row = 11;
			this.ListReserveSeat.SelectedIndex = 0;
			this.ListReserveSeat.Size = new Size(0x30, 440);
			this.ListReserveSeat.TabIndex = 0x37;
			this.ListReserveSeat.ItemClick += new smartRestaurant.Controls.ItemsList.ItemsListEventHandler(this.ListReserveTime_ItemClick);
			this.BtnDinIn.BackColor = Color.Transparent;
			this.BtnDinIn.Blue = 2f;
			this.BtnDinIn.Cursor = Cursors.Hand;
			this.BtnDinIn.Green = 1f;
			this.BtnDinIn.Image = (Bitmap) manager.GetObject("BtnDinIn.Image");
			this.BtnDinIn.ImageClick = (Bitmap) manager.GetObject("BtnDinIn.ImageClick");
			this.BtnDinIn.ImageClickIndex = 1;
			this.BtnDinIn.ImageIndex = 0;
			this.BtnDinIn.ImageList = this.ButtonImgList;
			this.BtnDinIn.Location = new Point(8, 0x40);
			this.BtnDinIn.Name = "BtnDinIn";
			this.BtnDinIn.ObjectValue = null;
			this.BtnDinIn.Red = 2f;
			this.BtnDinIn.Size = new Size(110, 60);
			this.BtnDinIn.TabIndex = 0x2e;
			this.BtnDinIn.Text = "Din-in";
			this.BtnDinIn.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnDinIn.Click += new EventHandler(this.BtnDinIn_Click);
			this.BtnMain.BackColor = Color.Transparent;
			this.BtnMain.Blue = 2f;
			this.BtnMain.Cursor = Cursors.Hand;
			this.BtnMain.Green = 2f;
			this.BtnMain.Image = (Bitmap) manager.GetObject("BtnMain.Image");
			this.BtnMain.ImageClick = null;
			this.BtnMain.ImageClickIndex = 0;
			this.BtnMain.ImageIndex = 0;
			this.BtnMain.ImageList = this.ButtonImgList;
			this.BtnMain.Location = new Point(0x1c8, 0x40);
			this.BtnMain.Name = "BtnMain";
			this.BtnMain.ObjectValue = null;
			this.BtnMain.Red = 1f;
			this.BtnMain.Size = new Size(110, 60);
			this.BtnMain.TabIndex = 0x34;
			this.BtnMain.Text = "Main";
			this.BtnMain.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnMain.Click += new EventHandler(this.BtnMain_Click);
			this.BtnReserve.BackColor = Color.Transparent;
			this.BtnReserve.Blue = 1f;
			this.BtnReserve.Cursor = Cursors.Hand;
			this.BtnReserve.Green = 1f;
			this.BtnReserve.Image = (Bitmap) manager.GetObject("BtnReserve.Image");
			this.BtnReserve.ImageClick = null;
			this.BtnReserve.ImageClickIndex = 0;
			this.BtnReserve.ImageIndex = 0;
			this.BtnReserve.ImageList = this.ButtonImgList;
			this.BtnReserve.Location = new Point(0x388, 0x40);
			this.BtnReserve.Name = "BtnReserve";
			this.BtnReserve.ObjectValue = null;
			this.BtnReserve.Red = 0.75f;
			this.BtnReserve.Size = new Size(110, 60);
			this.BtnReserve.TabIndex = 0x35;
			this.BtnReserve.Text = "Reserve";
			this.BtnReserve.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnReserve.Click += new EventHandler(this.BtnReserve_Click);
			this.BtnCancel.BackColor = Color.Transparent;
			this.BtnCancel.Blue = 2f;
			this.BtnCancel.Cursor = Cursors.Hand;
			this.BtnCancel.Green = 1f;
			this.BtnCancel.Image = (Bitmap) manager.GetObject("BtnCancel.Image");
			this.BtnCancel.ImageClick = (Bitmap) manager.GetObject("BtnCancel.ImageClick");
			this.BtnCancel.ImageClickIndex = 1;
			this.BtnCancel.ImageIndex = 0;
			this.BtnCancel.ImageList = this.ButtonImgList;
			this.BtnCancel.Location = new Point(120, 0x40);
			this.BtnCancel.Name = "BtnCancel";
			this.BtnCancel.ObjectValue = null;
			this.BtnCancel.Red = 2f;
			this.BtnCancel.Size = new Size(110, 60);
			this.BtnCancel.TabIndex = 0x36;
			this.BtnCancel.Text = "Cancel";
			this.BtnCancel.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnCancel.Click += new EventHandler(this.BtnCancel_Click);
			this.LblHeaderSeat.BackColor = Color.Black;
			this.LblHeaderSeat.Font = new Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.LblHeaderSeat.ForeColor = Color.White;
			this.LblHeaderSeat.Location = new Point(280, 0xc0);
			this.LblHeaderSeat.Name = "LblHeaderSeat";
			this.LblHeaderSeat.Size = new Size(0x30, 40);
			this.LblHeaderSeat.TabIndex = 0x38;
			this.LblHeaderSeat.Text = "Seat";
			this.LblHeaderSeat.TextAlign = ContentAlignment.MiddleCenter;
			this.LblHeaderTime.BackColor = Color.Black;
			this.LblHeaderTime.Font = new Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.LblHeaderTime.ForeColor = Color.White;
			this.LblHeaderTime.Location = new Point(8, 0xc0);
			this.LblHeaderTime.Name = "LblHeaderTime";
			this.LblHeaderTime.Size = new Size(0x38, 40);
			this.LblHeaderTime.TabIndex = 0x3a;
			this.LblHeaderTime.Text = "Time";
			this.LblHeaderTime.TextAlign = ContentAlignment.MiddleCenter;
			this.LblPageID.BackColor = Color.Transparent;
			this.LblPageID.Font = new Font("Tahoma", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.LblPageID.ForeColor = Color.FromArgb(0x67, 0x8a, 0xc6);
			this.LblPageID.Location = new Point(760, 0x2f0);
			this.LblPageID.Name = "LblPageID";
			this.LblPageID.Size = new Size(0xf8, 0x17);
			this.LblPageID.TabIndex = 0x3d;
			this.LblPageID.Text = "STRT010";
			this.LblPageID.TextAlign = ContentAlignment.TopRight;
			this.PanSelectTime.BackColor = Color.Transparent;
			this.PanSelectTime.Caption = null;
			this.PanSelectTime.Controls.AddRange(new Control[] { this.LblMinute, this.LblHeaderHour, this.MinutePad, this.HourPad });
			this.PanSelectTime.Font = new Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.PanSelectTime.Location = new Point(0x2a0, 0xc0);
			this.PanSelectTime.Name = "PanSelectTime";
			this.PanSelectTime.ShowHeader = false;
			this.PanSelectTime.Size = new Size(0x158, 560);
			this.PanSelectTime.TabIndex = 60;
			this.LblMinute.BackColor = Color.Black;
			this.LblMinute.Font = new Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.LblMinute.ForeColor = Color.White;
			this.LblMinute.Location = new Point(0, 0x1a8);
			this.LblMinute.Name = "LblMinute";
			this.LblMinute.Size = new Size(0x158, 40);
			this.LblMinute.TabIndex = 0x34;
			this.LblMinute.Text = "Minute";
			this.LblMinute.TextAlign = ContentAlignment.MiddleCenter;
			this.LblHeaderHour.BackColor = Color.Black;
			this.LblHeaderHour.Font = new Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.LblHeaderHour.ForeColor = Color.White;
			this.LblHeaderHour.Name = "LblHeaderHour";
			this.LblHeaderHour.Size = new Size(0x158, 40);
			this.LblHeaderHour.TabIndex = 0x33;
			this.LblHeaderHour.Text = "Hour";
			this.LblHeaderHour.TextAlign = ContentAlignment.MiddleCenter;
			this.MinutePad.AutoRefresh = true;
			this.MinutePad.BackColor = Color.White;
			this.MinutePad.Blue = 1f;
			this.MinutePad.Column = 4;
			this.MinutePad.Font = new Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.MinutePad.Green = 1f;
			this.MinutePad.Image = (Bitmap) manager.GetObject("MinutePad.Image");
			this.MinutePad.ImageClick = (Bitmap) manager.GetObject("MinutePad.ImageClick");
			this.MinutePad.ImageClickIndex = 1;
			this.MinutePad.ImageIndex = 0;
			this.MinutePad.ImageList = this.NumberImgList;
			this.MinutePad.Location = new Point(0x1c, 0x1d8);
			this.MinutePad.Name = "MinutePad";
			this.MinutePad.Padding = 1;
			this.MinutePad.Red = 1f;
			this.MinutePad.Row = 1;
			this.MinutePad.Size = new Size(0x123, 60);
			this.MinutePad.TabIndex = 6;
			this.MinutePad.PadClick += new ButtonListPadEventHandler(this.MinutePad_PadClick);
			this.HourPad.AutoRefresh = true;
			this.HourPad.BackColor = Color.White;
			this.HourPad.Blue = 1f;
			this.HourPad.Column = 4;
			this.HourPad.Font = new Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.HourPad.Green = 1f;
			this.HourPad.Image = (Bitmap) manager.GetObject("HourPad.Image");
			this.HourPad.ImageClick = (Bitmap) manager.GetObject("HourPad.ImageClick");
			this.HourPad.ImageClickIndex = 1;
			this.HourPad.ImageIndex = 0;
			this.HourPad.ImageList = this.NumberImgList;
			this.HourPad.Location = new Point(0x1c, 0x30);
			this.HourPad.Name = "HourPad";
			this.HourPad.Padding = 1;
			this.HourPad.Red = 1f;
			this.HourPad.Row = 6;
			this.HourPad.Size = new Size(0x123, 0x16d);
			this.HourPad.TabIndex = 5;
			this.HourPad.PadClick += new ButtonListPadEventHandler(this.HourPad_PadClick);
			this.groupPanel2.BackColor = Color.Transparent;
			this.groupPanel2.Caption = null;
			this.groupPanel2.Controls.AddRange(new Control[] { this.BtnGotoToday, this.LblSelectedDate, this.LblCustName, this.FieldSeat, this.FieldInputType, this.NumberKeyPad, this.BtnSearch, this.PanCustName, this.LblReserveInfo });
			this.groupPanel2.Font = new Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.groupPanel2.Location = new Point(0x148, 0xc0);
			this.groupPanel2.Name = "groupPanel2";
			this.groupPanel2.ShowHeader = false;
			this.groupPanel2.Size = new Size(0x158, 560);
			this.groupPanel2.TabIndex = 0x3b;
			this.BtnGotoToday.BackColor = Color.Transparent;
			this.BtnGotoToday.Blue = 2f;
			this.BtnGotoToday.Cursor = Cursors.Hand;
			this.BtnGotoToday.Font = new Font("Tahoma", 12f, FontStyle.Regular, GraphicsUnit.Pixel);
			this.BtnGotoToday.Green = 1f;
			this.BtnGotoToday.Image = (Bitmap) manager.GetObject("BtnGotoToday.Image");
			this.BtnGotoToday.ImageClick = (Bitmap) manager.GetObject("BtnGotoToday.ImageClick");
			this.BtnGotoToday.ImageClickIndex = 1;
			this.BtnGotoToday.ImageIndex = 0;
			this.BtnGotoToday.ImageList = this.ButtonLiteImgList;
			this.BtnGotoToday.Location = new Point(0xe0, 0x30);
			this.BtnGotoToday.Name = "BtnGotoToday";
			this.BtnGotoToday.ObjectValue = null;
			this.BtnGotoToday.Red = 1f;
			this.BtnGotoToday.Size = new Size(110, 40);
			this.BtnGotoToday.TabIndex = 0x36;
			this.BtnGotoToday.Text = "Today";
			this.BtnGotoToday.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnGotoToday.Click += new EventHandler(this.BtnGotoToday_Click);
			this.ButtonLiteImgList.ColorDepth = ColorDepth.Depth32Bit;
			this.ButtonLiteImgList.ImageSize = new Size(110, 40);
			this.ButtonLiteImgList.ImageStream = (ImageListStreamer) manager.GetObject("ButtonLiteImgList.ImageStream");
			this.ButtonLiteImgList.TransparentColor = Color.Transparent;
			this.LblSelectedDate.BackColor = Color.Transparent;
			this.LblSelectedDate.BorderStyle = BorderStyle.FixedSingle;
			this.LblSelectedDate.Cursor = Cursors.Hand;
			this.LblSelectedDate.Font = new Font("Tahoma", 12f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.LblSelectedDate.ForeColor = Color.Black;
			this.LblSelectedDate.Location = new Point(8, 0x30);
			this.LblSelectedDate.Name = "LblSelectedDate";
			this.LblSelectedDate.Size = new Size(0xd0, 40);
			this.LblSelectedDate.TabIndex = 0x35;
			this.LblSelectedDate.Text = "dd MMMM yyyy HH:mm";
			this.LblSelectedDate.TextAlign = ContentAlignment.MiddleCenter;
			this.LblSelectedDate.Click += new EventHandler(this.LblSelectedDate_Click);
			this.LblCustName.BackColor = Color.Transparent;
			this.LblCustName.Font = new Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.LblCustName.ForeColor = Color.Black;
			this.LblCustName.Location = new Point(0x10, 0x58);
			this.LblCustName.Name = "LblCustName";
			this.LblCustName.Size = new Size(0x90, 0x18);
			this.LblCustName.TabIndex = 0x33;
			this.LblCustName.Text = "Customer Name";
			this.LblCustName.TextAlign = ContentAlignment.MiddleLeft;
			this.FieldSeat.BackColor = Color.Black;
			this.FieldSeat.Font = new Font("Tahoma", 14.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.FieldSeat.ForeColor = Color.Cyan;
			this.FieldSeat.Location = new Point(0x88, 0xf8);
			this.FieldSeat.Name = "FieldSeat";
			this.FieldSeat.Size = new Size(200, 40);
			this.FieldSeat.TabIndex = 9;
			this.FieldSeat.TextAlign = ContentAlignment.MiddleCenter;
			this.FieldInputType.BackColor = Color.Black;
			this.FieldInputType.Font = new Font("Tahoma", 14.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.FieldInputType.ForeColor = Color.Cyan;
			this.FieldInputType.Location = new Point(8, 0xf8);
			this.FieldInputType.Name = "FieldInputType";
			this.FieldInputType.Size = new Size(0x80, 40);
			this.FieldInputType.TabIndex = 8;
			this.FieldInputType.Text = "Seat";
			this.FieldInputType.TextAlign = ContentAlignment.MiddleCenter;
			this.NumberKeyPad.BackColor = Color.White;
			this.NumberKeyPad.Image = (Bitmap) manager.GetObject("NumberKeyPad.Image");
			this.NumberKeyPad.ImageClick = (Bitmap) manager.GetObject("NumberKeyPad.ImageClick");
			this.NumberKeyPad.ImageClickIndex = 1;
			this.NumberKeyPad.ImageIndex = 0;
			this.NumberKeyPad.ImageList = this.NumberImgList;
			this.NumberKeyPad.Location = new Point(0x40, 0x128);
			this.NumberKeyPad.Name = "NumberKeyPad";
			this.NumberKeyPad.Size = new Size(0xe2, 0xff);
			this.NumberKeyPad.TabIndex = 7;
			this.NumberKeyPad.Text = "numberPad1";
			this.NumberKeyPad.PadClick += new smartRestaurant.Controls.NumberPad.NumberPadEventHandler(this.NumberKeyPad_PadClick);
			this.LblReserveInfo.BackColor = Color.Black;
			this.LblReserveInfo.Font = new Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.LblReserveInfo.ForeColor = Color.White;
			this.LblReserveInfo.Name = "LblReserveInfo";
			this.LblReserveInfo.Size = new Size(0x158, 40);
			this.LblReserveInfo.TabIndex = 0x34;
			this.LblReserveInfo.Text = "Reserve Information";
			this.LblReserveInfo.TextAlign = ContentAlignment.MiddleCenter;
			this.DatePad.AutoRefresh = true;
			this.DatePad.BackColor = Color.White;
			this.DatePad.Blue = 1f;
			this.DatePad.Column = 7;
			this.DatePad.Font = new Font("Tahoma", 12f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.DatePad.Green = 1f;
			this.DatePad.Image = (Bitmap) manager.GetObject("DatePad.Image");
			this.DatePad.ImageClick = (Bitmap) manager.GetObject("DatePad.ImageClick");
			this.DatePad.ImageClickIndex = 1;
			this.DatePad.ImageIndex = 0;
			this.DatePad.ImageList = this.ButtonImgList;
			this.DatePad.Location = new Point(0x74, 2);
			this.DatePad.Name = "DatePad";
			this.DatePad.Padding = 1;
			this.DatePad.Red = 1f;
			this.DatePad.Row = 1;
			this.DatePad.Size = new Size(0x308, 60);
			this.DatePad.TabIndex = 6;
			this.DatePad.PadClick += new ButtonListPadEventHandler(this.DatePad_PadClick);
			this.BtnDayDown.BackColor = Color.Transparent;
			this.BtnDayDown.Blue = 2f;
			this.BtnDayDown.Cursor = Cursors.Hand;
			this.BtnDayDown.Green = 1f;
			this.BtnDayDown.Image = (Bitmap) manager.GetObject("BtnDayDown.Image");
			this.BtnDayDown.ImageClick = (Bitmap) manager.GetObject("BtnDayDown.ImageClick");
			this.BtnDayDown.ImageClickIndex = 5;
			this.BtnDayDown.ImageIndex = 4;
			this.BtnDayDown.ImageList = this.ButtonImgList;
			this.BtnDayDown.Location = new Point(0x37d, 2);
			this.BtnDayDown.Name = "BtnDayDown";
			this.BtnDayDown.ObjectValue = null;
			this.BtnDayDown.Red = 1f;
			this.BtnDayDown.Size = new Size(110, 60);
			this.BtnDayDown.TabIndex = 0x33;
			this.BtnDayDown.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnDayDown.Click += new EventHandler(this.BtnDayDown_Click);
			this.BtnDayUp.BackColor = Color.Transparent;
			this.BtnDayUp.Blue = 2f;
			this.BtnDayUp.Cursor = Cursors.Hand;
			this.BtnDayUp.Green = 1f;
			this.BtnDayUp.Image = (Bitmap) manager.GetObject("BtnDayUp.Image");
			this.BtnDayUp.ImageClick = (Bitmap) manager.GetObject("BtnDayUp.ImageClick");
			this.BtnDayUp.ImageClickIndex = 3;
			this.BtnDayUp.ImageIndex = 2;
			this.BtnDayUp.ImageList = this.ButtonImgList;
			this.BtnDayUp.Location = new Point(5, 2);
			this.BtnDayUp.Name = "BtnDayUp";
			this.BtnDayUp.ObjectValue = null;
			this.BtnDayUp.Red = 1f;
			this.BtnDayUp.Size = new Size(110, 60);
			this.BtnDayUp.TabIndex = 50;
			this.BtnDayUp.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnDayUp.Click += new EventHandler(this.BtnDayUp_Click);
			this.GroupDate.BackColor = Color.Transparent;
			this.GroupDate.Caption = null;
			this.GroupDate.Controls.AddRange(new Control[] { this.BtnDayUp, this.DatePad, this.BtnDayDown });
			this.GroupDate.Font = new Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.GroupDate.Location = new Point(8, 0x80);
			this.GroupDate.Name = "GroupDate";
			this.GroupDate.ShowHeader = false;
			this.GroupDate.Size = new Size(0x3f0, 0x40);
			this.GroupDate.TabIndex = 0x3e;
			this.LblHeaderNumber.BackColor = Color.Black;
			this.LblHeaderNumber.Font = new Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.LblHeaderNumber.ForeColor = Color.White;
			this.LblHeaderNumber.Location = new Point(0xe8, 0xc0);
			this.LblHeaderNumber.Name = "LblHeaderNumber";
			this.LblHeaderNumber.Size = new Size(0x30, 40);
			this.LblHeaderNumber.TabIndex = 0x40;
			this.LblHeaderNumber.Text = "#";
			this.LblHeaderNumber.TextAlign = ContentAlignment.MiddleCenter;
			this.AutoScaleBaseSize = new Size(6, 15);
			base.ClientSize = new Size(0x3fc, 0x2fc);
			base.Controls.AddRange(new Control[] { 
													 this.LblHeaderNumber, this.ListReserveID, this.GroupDate, this.LblPageID, this.PanSelectTime, this.groupPanel2, this.LblHeaderTime, this.ListReserveTime, this.LblHeaderSeat, this.ListReserveSeat, this.BtnCancel, this.BtnReserve, this.BtnMain, this.LblCopyright, this.LblHeaderCustomer, this.BtnDown, 
													 this.BtnUp, this.ListReserveQueue, this.BtnDinIn
												 });
			base.Name = "ReserveForm";
			this.Text = "Reserve Table";
			this.PanCustName.ResumeLayout(false);
			this.PanSelectTime.ResumeLayout(false);
			this.groupPanel2.ResumeLayout(false);
			this.GroupDate.ResumeLayout(false);
			base.ResumeLayout(false);
		}

		private void LblSelectedDate_Click(object sender, EventArgs e)
		{
			this.startDate = this.SearchStartOfWeek(this.selectedDate);
			this.UpdateDateButton();
			this.UpdateSelectedTime();
		}

		private void ListReserveTime_ItemClick(object sender, ItemsListEventArgs e)
		{
			this.selectedReserve = (TableReserve) e.Item.Value;
			this.UpdateReserveButton();
		}

		private void MinutePad_PadClick(object sender, ButtonListPadEventArgs e)
		{
			int num = int.Parse(e.Value);
			this.selectedDate = this.selectedDate.AddMinutes((double) (num - this.selectedDate.Minute));
			this.UpdateSelectedTime();
		}

		private void NumberKeyPad_PadClick(object sender, NumberPadEventArgs e)
		{
			if (e.IsNumeric)
			{
				this.FieldSeat.Text = this.FieldSeat.Text + e.Number.ToString();
			}
			else if (e.IsCancel)
			{
				if (this.FieldSeat.Text.Length > 1)
				{
					this.FieldSeat.Text = this.FieldSeat.Text.Substring(0, this.FieldSeat.Text.Length - 1);
				}
				else
				{
					this.FieldSeat.Text = "";
				}
			}
			this.UpdateReserveButton();
		}

		private DateTime SearchStartOfWeek(DateTime date)
		{
			if (date.DayOfWeek > DayOfWeek.Sunday)
			{
				return date.AddDays(0 - date.DayOfWeek).Add(new TimeSpan(-date.Hour, -date.Minute, 0));
			}
			return date.Add(new TimeSpan(-date.Hour, -date.Minute, 0));
		}

		private void UpdateDateButton()
		{
			DateTime startDate = this.startDate;
			this.DatePad.AutoRefresh = false;
			this.DatePad.Items.Clear();
			this.DatePad.Red = 1f;
			this.DatePad.Green = 1f;
			this.DatePad.Blue = 1f;
			for (int i = 0; i < 7; i++)
			{
				ButtonItem item = new ButtonItem(startDate.ToString("dd MMM yyyy", AppParameter.Culture), i.ToString());
				this.DatePad.Items.Add(item);
				startDate = startDate.AddDays(1.0);
			}
			this.DatePad.AutoRefresh = true;
			this.UpdateReserveQueue();
		}

		public override void UpdateForm()
		{
			this.selectedDate = DateTime.Today.Add(new TimeSpan(8, 0, 0));
			this.startDate = this.SearchStartOfWeek(this.selectedDate);
			this.selectedReserve = null;
			this.FieldCustName.Text = FIELD_CUST_TEXT;
			this.FieldSeat.Text = "";
			if (AppParameter.IsDemo())
			{
				this.FieldInputType.Text = "Guest";
			}
			else
			{
				this.FieldInputType.Text = "Seat";
			}
			this.LblPageID.Text = "Employee ID:" + ((MainForm) base.MdiParent).UserID.ToString() + " | STRT010";
			this.UpdateDateButton();
			this.UpdateSelectedTime();
		}

		private void UpdateReserveButton()
		{
			this.BtnReserve.Enabled = ((this.FieldCustName.Text != "") && (this.FieldCustName.Text != FIELD_CUST_TEXT)) && (this.FieldSeat.Text != "");
			this.BtnUp.Enabled = this.ListReserveTime.CanUp;
			this.BtnDown.Enabled = this.ListReserveTime.CanDown;
			if (this.selectedReserve == null)
			{
				this.BtnDinIn.Enabled = false;
				this.BtnCancel.Enabled = false;
			}
			else
			{
				this.BtnDinIn.Enabled = true;
				this.BtnCancel.Enabled = true;
			}
		}

		private void UpdateReserveQueue()
		{
			this.reserveList = new smartRestaurant.ReserveService.ReserveService().GetTableReserve(this.selectedDate);
			StringBuilder builder = new StringBuilder();
			this.ListReserveTime.AutoRefresh = false;
			this.ListReserveQueue.AutoRefresh = false;
			this.ListReserveID.AutoRefresh = false;
			this.ListReserveSeat.AutoRefresh = false;
			this.ListReserveTime.Items.Clear();
			this.ListReserveQueue.Items.Clear();
			this.ListReserveID.Items.Clear();
			this.ListReserveSeat.Items.Clear();
			if (this.reserveList == null)
			{
				this.ListReserveTime.AutoRefresh = true;
				this.ListReserveQueue.AutoRefresh = true;
				this.ListReserveID.AutoRefresh = true;
				this.ListReserveSeat.AutoRefresh = true;
			}
			else
			{
				this.ListReserveTime.SelectedIndex = -1;
				for (int i = 0; i < this.reserveList.Length; i++)
				{
					builder.Length = 0;
					builder.Append(this.reserveList[i].custFirstName);
					builder.Append(" ");
					builder.Append(this.reserveList[i].custMiddleName);
					builder.Append(" ");
					builder.Append(this.reserveList[i].custLastName);
					DataItem item = new DataItem(this.reserveList[i].reserveDate.ToString("HH:mm"), this.reserveList[i], false);
					this.ListReserveTime.Items.Add(item);
					item = new DataItem(builder.ToString(), this.reserveList[i], false);
					this.ListReserveQueue.Items.Add(item);
					item = new DataItem(this.reserveList[i].reserveTableID.ToString(), this.reserveList[i], false);
					this.ListReserveID.Items.Add(item);
					item = new DataItem(this.reserveList[i].seat.ToString(), this.reserveList[i], false);
					this.ListReserveSeat.Items.Add(item);
					if (this.selectedReserve == this.reserveList[i])
					{
						this.ListReserveTime.SelectedIndex = this.ListReserveTime.Items.Count - 1;
					}
				}
				this.ListReserveTime.AutoRefresh = true;
				this.ListReserveQueue.AutoRefresh = true;
				this.ListReserveID.AutoRefresh = true;
				this.ListReserveSeat.AutoRefresh = true;
				this.UpdateReserveButton();
			}
		}

		private void UpdateSelectedTime()
		{
			int pos = this.selectedDate.Minute / 15;
			if (pos > 3)
			{
				this.selectedDate = this.selectedDate.Add(new TimeSpan(-1, -this.selectedDate.Minute, 0));
				pos = 0;
			}
			this.DatePad.AutoRefresh = false;
			this.HourPad.AutoRefresh = false;
			this.MinutePad.AutoRefresh = false;
			this.HourPad.Red = this.HourPad.Green = this.HourPad.Blue = 1f;
			this.HourPad.SetMatrix(this.selectedDate.Hour, 1f, 2f, 1f);
			this.MinutePad.Red = this.MinutePad.Green = this.MinutePad.Blue = 1f;
			this.MinutePad.SetMatrix(pos, 1f, 2f, 1f);
			DateTime startDate = this.startDate;
			this.DatePad.Red = this.DatePad.Green = this.DatePad.Blue = 1f;
			for (int i = 0; i < 7; i++)
			{
				if (startDate.Date == this.selectedDate.Date)
				{
					this.DatePad.SetMatrix(i, 0.5f, 1f, 1f);
				}
				startDate = startDate.AddDays(1.0);
			}
			this.DatePad.AutoRefresh = true;
			this.HourPad.AutoRefresh = true;
			this.MinutePad.AutoRefresh = true;
			this.LblSelectedDate.Text = this.selectedDate.ToString("dd MMMM yyyy HH:mm", AppParameter.Culture);
			this.UpdateReserveButton();
		}

		private void UpdateTimeButton()
		{
			for (int i = 0; i < 0x18; i++)
			{
				this.HourPad.Items.Add(new ButtonItem(i.ToString("D2"), i.ToString()));
			}
			this.MinutePad.Items.Add(new ButtonItem("00", "0"));
			this.MinutePad.Items.Add(new ButtonItem("15", "15"));
			this.MinutePad.Items.Add(new ButtonItem("30", "30"));
			this.MinutePad.Items.Add(new ButtonItem("45", "45"));
		}

		// Properties
		public int EmployeeID
		{
			set
			{
				this.employeeID = value;
			}
		}
	}

 

}
