using smartRestaurant.Controls;
using smartRestaurant.ReserveService;
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
	public class ReserveForm : SmartForm
	{
		private static string FIELD_CUST_TEXT;

		private ImageButton BtnSearch;

		private GroupPanel PanCustName;

		private Label FieldCustName;

		private ImageList ButtonImgList;

		private ImageList NumberImgList;

		private Label LblCopyright;

		private ImageButton BtnDown;

		private ImageButton BtnUp;

		private ImageButton BtnMain;

		private Label LblHeaderCustomer;

		private ItemsList ListReserveQueue;

		private ImageButton BtnDinIn;

		private ImageButton BtnReserve;

		private ImageButton BtnCancel;

		private Label LblHeaderSeat;

		private ItemsList ListReserveSeat;

		private Label LblHeaderTime;

		private ItemsList ListReserveTime;

		private Label LblPageID;

		private GroupPanel groupPanel2;

		private Label FieldInputType;

		private NumberPad NumberKeyPad;

		private ButtonListPad HourPad;

		private GroupPanel PanSelectTime;

		private ButtonListPad MinutePad;

		private Label LblHeaderHour;

		private Label LblMinute;

		private ImageButton BtnDayDown;

		private ImageButton BtnDayUp;

		private IContainer components;

		private Label LblCustName;

		private Label LblReserveInfo;

		private Label LblSelectedDate;

		private ButtonListPad DatePad;

		private GroupPanel GroupDate;

		private ImageList ButtonLiteImgList;

		private ImageButton BtnGotoToday;

		private Label FieldSeat;

		private int employeeID;

		private DateTime startDate;

		private DateTime selectedDate;

		private TableReserve[] reserveList;

		private ItemsList ListReserveID;

		private Label LblHeaderNumber;

		private TableReserve selectedReserve;

		public int EmployeeID
		{
			set
			{
				this.employeeID = value;
			}
		}

		static ReserveForm()
		{
			ReserveForm.FIELD_CUST_TEXT = "- Please input name -";
		}

		public ReserveForm()
		{
			this.InitializeComponent();
			this.UpdateTimeButton();
		}

		private void BtnCancel_Click(object sender, EventArgs e)
		{
			smartRestaurant.ReserveService.ReserveService reserveService = new smartRestaurant.ReserveService.ReserveService();
			reserveService.SetReserveCancel(this.selectedReserve.reserveTableID.ToString());
			this.UpdateReserveQueue();
		}

		private void BtnDayDown_Click(object sender, EventArgs e)
		{
			this.startDate = this.startDate.AddDays(7);
			this.UpdateDateButton();
			this.UpdateSelectedTime();
		}

		private void BtnDayUp_Click(object sender, EventArgs e)
		{
			this.startDate = this.startDate.AddDays(-7);
			this.UpdateDateButton();
			this.UpdateSelectedTime();
		}

		private void BtnDinIn_Click(object sender, EventArgs e)
		{
			if (this.selectedReserve != null)
			{
				TableInformation tableInformation = TableForm.Show("Reserve");
				if (tableInformation == null)
				{
					return;
				}
				smartRestaurant.ReserveService.ReserveService reserveService = new smartRestaurant.ReserveService.ReserveService();
				reserveService.SetReserveDinIn(this.selectedReserve.reserveTableID.ToString(), tableInformation.TableID.ToString());
				tableInformation.NumberOfSeat = this.selectedReserve.seat;
				((MainForm)base.MdiParent).ShowTakeOrderForm(tableInformation);
			}
		}

		private void BtnGotoToday_Click(object sender, EventArgs e)
		{
			DateTime today = DateTime.Today;
			this.selectedDate = today.Add(new TimeSpan(this.selectedDate.Hour, this.selectedDate.Minute, 0));
			this.startDate = this.SearchStartOfWeek(this.selectedDate);
			this.UpdateDateButton();
			this.UpdateSelectedTime();
		}

		private void BtnMain_Click(object sender, EventArgs e)
		{
			((MainForm)base.MdiParent).ShowMainMenuForm();
		}

		private void BtnReserve_Click(object sender, EventArgs e)
		{
			TableReserve tableReserve = new TableReserve()
			{
				reserveTableID = 0,
				tableID = 0,
				customerID = 0,
				seat = int.Parse(this.FieldSeat.Text),
				reserveDate = this.selectedDate
			};
			smartRestaurant.ReserveService.ReserveService reserveService = new smartRestaurant.ReserveService.ReserveService();
			string str = reserveService.SetTableReserve(tableReserve, this.FieldCustName.Text);
			if (str != null)
			{
				MessageBox.Show(str);
				return;
			}
			this.FieldCustName.Text = ReserveForm.FIELD_CUST_TEXT;
			this.FieldSeat.Text = "";
			this.UpdateReserveQueue();
			this.UpdateReserveButton();
		}

		private void BtnSearch_Click(object sender, EventArgs e)
		{
			string str;
			str = (this.FieldCustName.Text != ReserveForm.FIELD_CUST_TEXT ? this.FieldCustName.Text : "");
			string str1 = SearchCustomerForm.Show(str);
			if (str1 != null)
			{
				this.FieldCustName.Text = str1;
				this.UpdateReserveButton();
			}
		}

		private void DatePad_PadClick(object sender, ButtonListPadEventArgs e)
		{
			int num = int.Parse(e.Value);
			DateTime dateTime = this.startDate.AddDays((double)num);
			this.selectedDate = dateTime.Add(new TimeSpan(this.selectedDate.Hour, this.selectedDate.Minute, 0));
			this.UpdateDateButton();
			this.UpdateSelectedTime();
			this.UpdateReserveButton();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void FieldCustName_Click(object sender, EventArgs e)
		{
			string str;
			str = (this.FieldCustName.Text != ReserveForm.FIELD_CUST_TEXT ? this.FieldCustName.Text : "");
			string str1 = KeyboardForm.Show("Customer Name", str);
			if (str1 != null)
			{
				this.FieldCustName.Text = str1;
				this.UpdateReserveButton();
			}
		}

		private void HourPad_PadClick(object sender, ButtonListPadEventArgs e)
		{
			int num = int.Parse(e.Value);
			this.selectedDate = this.selectedDate.AddHours((double)(num - this.selectedDate.Hour));
			this.UpdateSelectedTime();
		}

		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			ResourceManager resourceManager = new ResourceManager(typeof(ReserveForm));
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
			this.BtnSearch.Font = new System.Drawing.Font("Tahoma", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.BtnSearch.Green = 1f;
			this.BtnSearch.Image = (Bitmap)resourceManager.GetObject("BtnSearch.Image");
			this.BtnSearch.ImageClick = (Bitmap)resourceManager.GetObject("BtnSearch.ImageClick");
			this.BtnSearch.ImageClickIndex = 1;
			this.BtnSearch.ImageIndex = 0;
			this.BtnSearch.ImageList = this.ButtonImgList;
			this.BtnSearch.Location = new Point(216, 176);
			this.BtnSearch.Name = "BtnSearch";
			this.BtnSearch.ObjectValue = null;
			this.BtnSearch.Red = 1f;
			this.BtnSearch.Size = new System.Drawing.Size(110, 60);
			this.BtnSearch.TabIndex = 26;
			this.BtnSearch.Text = "Search";
			this.BtnSearch.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnSearch.Click += new EventHandler(this.BtnSearch_Click);
			this.ButtonImgList.ColorDepth = ColorDepth.Depth32Bit;
			this.ButtonImgList.ImageSize = new System.Drawing.Size(110, 60);
			this.ButtonImgList.ImageStream = (ImageListStreamer)resourceManager.GetObject("ButtonImgList.ImageStream");
			this.ButtonImgList.TransparentColor = Color.Transparent;
			this.PanCustName.BackColor = Color.Transparent;
			this.PanCustName.Caption = null;
			Control.ControlCollection controls = this.PanCustName.Controls;
			Control[] fieldCustName = new Control[] { this.FieldCustName };
			controls.AddRange(fieldCustName);
			this.PanCustName.Font = new System.Drawing.Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.PanCustName.Location = new Point(16, 112);
			this.PanCustName.Name = "PanCustName";
			this.PanCustName.ShowHeader = false;
			this.PanCustName.Size = new System.Drawing.Size(312, 58);
			this.PanCustName.TabIndex = 25;
			this.FieldCustName.Cursor = Cursors.Hand;
			this.FieldCustName.Location = new Point(1, 1);
			this.FieldCustName.Name = "FieldCustName";
			this.FieldCustName.Size = new System.Drawing.Size(311, 56);
			this.FieldCustName.TabIndex = 0;
			this.FieldCustName.TextAlign = ContentAlignment.MiddleLeft;
			this.FieldCustName.Click += new EventHandler(this.FieldCustName_Click);
			this.NumberImgList.ColorDepth = ColorDepth.Depth32Bit;
			this.NumberImgList.ImageSize = new System.Drawing.Size(72, 60);
			this.NumberImgList.ImageStream = (ImageListStreamer)resourceManager.GetObject("NumberImgList.ImageStream");
			this.NumberImgList.TransparentColor = Color.Transparent;
			this.LblCopyright.BackColor = Color.Transparent;
			this.LblCopyright.Font = new System.Drawing.Font("Tahoma", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.LblCopyright.ForeColor = Color.FromArgb(103, 138, 198);
			this.LblCopyright.Location = new Point(8, 752);
			this.LblCopyright.Name = "LblCopyright";
			this.LblCopyright.Size = new System.Drawing.Size(280, 16);
			this.LblCopyright.TabIndex = 51;
			this.LblCopyright.Text = "Copyright (c) 2004. All rights reserved.";
			this.LblHeaderCustomer.BackColor = Color.Black;
			this.LblHeaderCustomer.Font = new System.Drawing.Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.LblHeaderCustomer.ForeColor = Color.White;
			this.LblHeaderCustomer.Location = new Point(64, 192);
			this.LblHeaderCustomer.Name = "LblHeaderCustomer";
			this.LblHeaderCustomer.Size = new System.Drawing.Size(168, 40);
			this.LblHeaderCustomer.TabIndex = 50;
			this.LblHeaderCustomer.Text = "Customer";
			this.LblHeaderCustomer.TextAlign = ContentAlignment.MiddleLeft;
			this.BtnDown.BackColor = Color.Transparent;
			this.BtnDown.Blue = 2f;
			this.BtnDown.Cursor = Cursors.Hand;
			this.BtnDown.Green = 1f;
			this.BtnDown.Image = (Bitmap)resourceManager.GetObject("BtnDown.Image");
			this.BtnDown.ImageClick = (Bitmap)resourceManager.GetObject("BtnDown.ImageClick");
			this.BtnDown.ImageClickIndex = 5;
			this.BtnDown.ImageIndex = 4;
			this.BtnDown.ImageList = this.ButtonImgList;
			this.BtnDown.Location = new Point(208, 692);
			this.BtnDown.Name = "BtnDown";
			this.BtnDown.ObjectValue = null;
			this.BtnDown.Red = 2f;
			this.BtnDown.Size = new System.Drawing.Size(110, 60);
			this.BtnDown.TabIndex = 49;
			this.BtnDown.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnUp.BackColor = Color.Transparent;
			this.BtnUp.Blue = 2f;
			this.BtnUp.Cursor = Cursors.Hand;
			this.BtnUp.Green = 1f;
			this.BtnUp.Image = (Bitmap)resourceManager.GetObject("BtnUp.Image");
			this.BtnUp.ImageClick = (Bitmap)resourceManager.GetObject("BtnUp.ImageClick");
			this.BtnUp.ImageClickIndex = 3;
			this.BtnUp.ImageIndex = 2;
			this.BtnUp.ImageList = this.ButtonImgList;
			this.BtnUp.Location = new Point(16, 692);
			this.BtnUp.Name = "BtnUp";
			this.BtnUp.ObjectValue = null;
			this.BtnUp.Red = 2f;
			this.BtnUp.Size = new System.Drawing.Size(110, 60);
			this.BtnUp.TabIndex = 48;
			this.BtnUp.TextAlign = ContentAlignment.MiddleCenter;
			this.ListReserveQueue.Alignment = ContentAlignment.MiddleLeft;
			this.ListReserveQueue.AutoRefresh = true;
			this.ListReserveQueue.BackAlterColor = Color.FromArgb(192, 255, 255);
			this.ListReserveQueue.BackHeaderColor = Color.FromArgb(255, 192, 128);
			this.ListReserveQueue.BackHeaderSelectedColor = Color.FromArgb(192, 0, 0);
			this.ListReserveQueue.BackNormalColor = Color.White;
			this.ListReserveQueue.BackSelectedColor = Color.Blue;
			this.ListReserveQueue.BindList1 = this.ListReserveTime;
			this.ListReserveQueue.BindList2 = this.ListReserveID;
			this.ListReserveQueue.Border = 0;
			this.ListReserveQueue.Font = new System.Drawing.Font("Tahoma", 12f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.ListReserveQueue.ForeAlterColor = Color.Black;
			this.ListReserveQueue.ForeHeaderColor = Color.Black;
			this.ListReserveQueue.ForeHeaderSelectedColor = Color.White;
			this.ListReserveQueue.ForeNormalColor = Color.Black;
			this.ListReserveQueue.ForeSelectedColor = Color.White;
			this.ListReserveQueue.ItemHeight = 40;
			this.ListReserveQueue.ItemWidth = 168;
			this.ListReserveQueue.Location = new Point(64, 232);
			this.ListReserveQueue.Name = "ListReserveQueue";
			this.ListReserveQueue.Row = 11;
			this.ListReserveQueue.SelectedIndex = 0;
			this.ListReserveQueue.Size = new System.Drawing.Size(168, 440);
			this.ListReserveQueue.TabIndex = 47;
			this.ListReserveQueue.ItemClick += new ItemsListEventHandler(this.ListReserveTime_ItemClick);
			this.ListReserveTime.Alignment = ContentAlignment.MiddleCenter;
			this.ListReserveTime.AutoRefresh = true;
			this.ListReserveTime.BackAlterColor = Color.FromArgb(192, 255, 255);
			this.ListReserveTime.BackHeaderColor = Color.FromArgb(255, 192, 128);
			this.ListReserveTime.BackHeaderSelectedColor = Color.FromArgb(192, 0, 0);
			this.ListReserveTime.BackNormalColor = Color.White;
			this.ListReserveTime.BackSelectedColor = Color.Blue;
			this.ListReserveTime.BindList1 = null;
			this.ListReserveTime.BindList2 = this.ListReserveQueue;
			this.ListReserveTime.Border = 0;
			this.ListReserveTime.Font = new System.Drawing.Font("Tahoma", 12f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.ListReserveTime.ForeAlterColor = Color.Black;
			this.ListReserveTime.ForeHeaderColor = Color.Black;
			this.ListReserveTime.ForeHeaderSelectedColor = Color.White;
			this.ListReserveTime.ForeNormalColor = Color.Black;
			this.ListReserveTime.ForeSelectedColor = Color.White;
			this.ListReserveTime.ItemHeight = 40;
			this.ListReserveTime.ItemWidth = 56;
			this.ListReserveTime.Location = new Point(8, 232);
			this.ListReserveTime.Name = "ListReserveTime";
			this.ListReserveTime.Row = 11;
			this.ListReserveTime.SelectedIndex = 0;
			this.ListReserveTime.Size = new System.Drawing.Size(56, 440);
			this.ListReserveTime.TabIndex = 57;
			this.ListReserveTime.ItemClick += new ItemsListEventHandler(this.ListReserveTime_ItemClick);
			this.ListReserveID.Alignment = ContentAlignment.MiddleCenter;
			this.ListReserveID.AutoRefresh = true;
			this.ListReserveID.BackAlterColor = Color.FromArgb(192, 255, 255);
			this.ListReserveID.BackHeaderColor = Color.FromArgb(255, 192, 128);
			this.ListReserveID.BackHeaderSelectedColor = Color.FromArgb(192, 0, 0);
			this.ListReserveID.BackNormalColor = Color.White;
			this.ListReserveID.BackSelectedColor = Color.Blue;
			this.ListReserveID.BindList1 = this.ListReserveQueue;
			this.ListReserveID.BindList2 = this.ListReserveSeat;
			this.ListReserveID.Border = 0;
			this.ListReserveID.Font = new System.Drawing.Font("Tahoma", 12f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.ListReserveID.ForeAlterColor = Color.Black;
			this.ListReserveID.ForeHeaderColor = Color.Black;
			this.ListReserveID.ForeHeaderSelectedColor = Color.White;
			this.ListReserveID.ForeNormalColor = Color.Black;
			this.ListReserveID.ForeSelectedColor = Color.White;
			this.ListReserveID.ItemHeight = 40;
			this.ListReserveID.ItemWidth = 48;
			this.ListReserveID.Location = new Point(232, 232);
			this.ListReserveID.Name = "ListReserveID";
			this.ListReserveID.Row = 11;
			this.ListReserveID.SelectedIndex = 0;
			this.ListReserveID.Size = new System.Drawing.Size(48, 440);
			this.ListReserveID.TabIndex = 63;
			this.ListReserveID.ItemClick += new ItemsListEventHandler(this.ListReserveTime_ItemClick);
			this.ListReserveSeat.Alignment = ContentAlignment.MiddleCenter;
			this.ListReserveSeat.AutoRefresh = true;
			this.ListReserveSeat.BackAlterColor = Color.FromArgb(192, 255, 255);
			this.ListReserveSeat.BackHeaderColor = Color.FromArgb(255, 192, 128);
			this.ListReserveSeat.BackHeaderSelectedColor = Color.FromArgb(192, 0, 0);
			this.ListReserveSeat.BackNormalColor = Color.White;
			this.ListReserveSeat.BackSelectedColor = Color.Blue;
			this.ListReserveSeat.BindList1 = this.ListReserveID;
			this.ListReserveSeat.BindList2 = null;
			this.ListReserveSeat.Border = 0;
			this.ListReserveSeat.Font = new System.Drawing.Font("Tahoma", 12f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.ListReserveSeat.ForeAlterColor = Color.Black;
			this.ListReserveSeat.ForeHeaderColor = Color.Black;
			this.ListReserveSeat.ForeHeaderSelectedColor = Color.White;
			this.ListReserveSeat.ForeNormalColor = Color.Black;
			this.ListReserveSeat.ForeSelectedColor = Color.White;
			this.ListReserveSeat.ItemHeight = 40;
			this.ListReserveSeat.ItemWidth = 48;
			this.ListReserveSeat.Location = new Point(280, 232);
			this.ListReserveSeat.Name = "ListReserveSeat";
			this.ListReserveSeat.Row = 11;
			this.ListReserveSeat.SelectedIndex = 0;
			this.ListReserveSeat.Size = new System.Drawing.Size(48, 440);
			this.ListReserveSeat.TabIndex = 55;
			this.ListReserveSeat.ItemClick += new ItemsListEventHandler(this.ListReserveTime_ItemClick);
			this.BtnDinIn.BackColor = Color.Transparent;
			this.BtnDinIn.Blue = 2f;
			this.BtnDinIn.Cursor = Cursors.Hand;
			this.BtnDinIn.Green = 1f;
			this.BtnDinIn.Image = (Bitmap)resourceManager.GetObject("BtnDinIn.Image");
			this.BtnDinIn.ImageClick = (Bitmap)resourceManager.GetObject("BtnDinIn.ImageClick");
			this.BtnDinIn.ImageClickIndex = 1;
			this.BtnDinIn.ImageIndex = 0;
			this.BtnDinIn.ImageList = this.ButtonImgList;
			this.BtnDinIn.Location = new Point(8, 64);
			this.BtnDinIn.Name = "BtnDinIn";
			this.BtnDinIn.ObjectValue = null;
			this.BtnDinIn.Red = 2f;
			this.BtnDinIn.Size = new System.Drawing.Size(110, 60);
			this.BtnDinIn.TabIndex = 46;
			this.BtnDinIn.Text = "Din-in";
			this.BtnDinIn.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnDinIn.Click += new EventHandler(this.BtnDinIn_Click);
			this.BtnMain.BackColor = Color.Transparent;
			this.BtnMain.Blue = 2f;
			this.BtnMain.Cursor = Cursors.Hand;
			this.BtnMain.Green = 2f;
			this.BtnMain.Image = (Bitmap)resourceManager.GetObject("BtnMain.Image");
			this.BtnMain.ImageClick = null;
			this.BtnMain.ImageClickIndex = 0;
			this.BtnMain.ImageIndex = 0;
			this.BtnMain.ImageList = this.ButtonImgList;
			this.BtnMain.Location = new Point(456, 64);
			this.BtnMain.Name = "BtnMain";
			this.BtnMain.ObjectValue = null;
			this.BtnMain.Red = 1f;
			this.BtnMain.Size = new System.Drawing.Size(110, 60);
			this.BtnMain.TabIndex = 52;
			this.BtnMain.Text = "Main";
			this.BtnMain.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnMain.Click += new EventHandler(this.BtnMain_Click);
			this.BtnReserve.BackColor = Color.Transparent;
			this.BtnReserve.Blue = 1f;
			this.BtnReserve.Cursor = Cursors.Hand;
			this.BtnReserve.Green = 1f;
			this.BtnReserve.Image = (Bitmap)resourceManager.GetObject("BtnReserve.Image");
			this.BtnReserve.ImageClick = null;
			this.BtnReserve.ImageClickIndex = 0;
			this.BtnReserve.ImageIndex = 0;
			this.BtnReserve.ImageList = this.ButtonImgList;
			this.BtnReserve.Location = new Point(904, 64);
			this.BtnReserve.Name = "BtnReserve";
			this.BtnReserve.ObjectValue = null;
			this.BtnReserve.Red = 0.75f;
			this.BtnReserve.Size = new System.Drawing.Size(110, 60);
			this.BtnReserve.TabIndex = 53;
			this.BtnReserve.Text = "Reserve";
			this.BtnReserve.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnReserve.Click += new EventHandler(this.BtnReserve_Click);
			this.BtnCancel.BackColor = Color.Transparent;
			this.BtnCancel.Blue = 2f;
			this.BtnCancel.Cursor = Cursors.Hand;
			this.BtnCancel.Green = 1f;
			this.BtnCancel.Image = (Bitmap)resourceManager.GetObject("BtnCancel.Image");
			this.BtnCancel.ImageClick = (Bitmap)resourceManager.GetObject("BtnCancel.ImageClick");
			this.BtnCancel.ImageClickIndex = 1;
			this.BtnCancel.ImageIndex = 0;
			this.BtnCancel.ImageList = this.ButtonImgList;
			this.BtnCancel.Location = new Point(120, 64);
			this.BtnCancel.Name = "BtnCancel";
			this.BtnCancel.ObjectValue = null;
			this.BtnCancel.Red = 2f;
			this.BtnCancel.Size = new System.Drawing.Size(110, 60);
			this.BtnCancel.TabIndex = 54;
			this.BtnCancel.Text = "Cancel";
			this.BtnCancel.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnCancel.Click += new EventHandler(this.BtnCancel_Click);
			this.LblHeaderSeat.BackColor = Color.Black;
			this.LblHeaderSeat.Font = new System.Drawing.Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.LblHeaderSeat.ForeColor = Color.White;
			this.LblHeaderSeat.Location = new Point(280, 192);
			this.LblHeaderSeat.Name = "LblHeaderSeat";
			this.LblHeaderSeat.Size = new System.Drawing.Size(48, 40);
			this.LblHeaderSeat.TabIndex = 56;
			this.LblHeaderSeat.Text = "Seat";
			this.LblHeaderSeat.TextAlign = ContentAlignment.MiddleCenter;
			this.LblHeaderTime.BackColor = Color.Black;
			this.LblHeaderTime.Font = new System.Drawing.Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.LblHeaderTime.ForeColor = Color.White;
			this.LblHeaderTime.Location = new Point(8, 192);
			this.LblHeaderTime.Name = "LblHeaderTime";
			this.LblHeaderTime.Size = new System.Drawing.Size(56, 40);
			this.LblHeaderTime.TabIndex = 58;
			this.LblHeaderTime.Text = "Time";
			this.LblHeaderTime.TextAlign = ContentAlignment.MiddleCenter;
			this.LblPageID.BackColor = Color.Transparent;
			this.LblPageID.Font = new System.Drawing.Font("Tahoma", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.LblPageID.ForeColor = Color.FromArgb(103, 138, 198);
			this.LblPageID.Location = new Point(760, 752);
			this.LblPageID.Name = "LblPageID";
			this.LblPageID.Size = new System.Drawing.Size(248, 23);
			this.LblPageID.TabIndex = 61;
			this.LblPageID.Text = "STRT010";
			this.LblPageID.TextAlign = ContentAlignment.TopRight;
			this.PanSelectTime.BackColor = Color.Transparent;
			this.PanSelectTime.Caption = null;
			Control.ControlCollection controlCollections = this.PanSelectTime.Controls;
			fieldCustName = new Control[] { this.LblMinute, this.LblHeaderHour, this.MinutePad, this.HourPad };
			controlCollections.AddRange(fieldCustName);
			this.PanSelectTime.Font = new System.Drawing.Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.PanSelectTime.Location = new Point(672, 192);
			this.PanSelectTime.Name = "PanSelectTime";
			this.PanSelectTime.ShowHeader = false;
			this.PanSelectTime.Size = new System.Drawing.Size(344, 560);
			this.PanSelectTime.TabIndex = 60;
			this.LblMinute.BackColor = Color.Black;
			this.LblMinute.Font = new System.Drawing.Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.LblMinute.ForeColor = Color.White;
			this.LblMinute.Location = new Point(0, 424);
			this.LblMinute.Name = "LblMinute";
			this.LblMinute.Size = new System.Drawing.Size(344, 40);
			this.LblMinute.TabIndex = 52;
			this.LblMinute.Text = "Minute";
			this.LblMinute.TextAlign = ContentAlignment.MiddleCenter;
			this.LblHeaderHour.BackColor = Color.Black;
			this.LblHeaderHour.Font = new System.Drawing.Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.LblHeaderHour.ForeColor = Color.White;
			this.LblHeaderHour.Name = "LblHeaderHour";
			this.LblHeaderHour.Size = new System.Drawing.Size(344, 40);
			this.LblHeaderHour.TabIndex = 51;
			this.LblHeaderHour.Text = "Hour";
			this.LblHeaderHour.TextAlign = ContentAlignment.MiddleCenter;
			this.MinutePad.AutoRefresh = true;
			this.MinutePad.BackColor = Color.White;
			this.MinutePad.Blue = 1f;
			this.MinutePad.Column = 4;
			this.MinutePad.Font = new System.Drawing.Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.MinutePad.Green = 1f;
			this.MinutePad.Image = (Bitmap)resourceManager.GetObject("MinutePad.Image");
			this.MinutePad.ImageClick = (Bitmap)resourceManager.GetObject("MinutePad.ImageClick");
			this.MinutePad.ImageClickIndex = 1;
			this.MinutePad.ImageIndex = 0;
			this.MinutePad.ImageList = this.NumberImgList;
			this.MinutePad.Location = new Point(28, 472);
			this.MinutePad.Name = "MinutePad";
			this.MinutePad.Padding = 1;
			this.MinutePad.Red = 1f;
			this.MinutePad.Row = 1;
			this.MinutePad.Size = new System.Drawing.Size(291, 60);
			this.MinutePad.TabIndex = 6;
			this.MinutePad.PadClick += new ButtonListPadEventHandler(this.MinutePad_PadClick);
			this.HourPad.AutoRefresh = true;
			this.HourPad.BackColor = Color.White;
			this.HourPad.Blue = 1f;
			this.HourPad.Column = 4;
			this.HourPad.Font = new System.Drawing.Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.HourPad.Green = 1f;
			this.HourPad.Image = (Bitmap)resourceManager.GetObject("HourPad.Image");
			this.HourPad.ImageClick = (Bitmap)resourceManager.GetObject("HourPad.ImageClick");
			this.HourPad.ImageClickIndex = 1;
			this.HourPad.ImageIndex = 0;
			this.HourPad.ImageList = this.NumberImgList;
			this.HourPad.Location = new Point(28, 48);
			this.HourPad.Name = "HourPad";
			this.HourPad.Padding = 1;
			this.HourPad.Red = 1f;
			this.HourPad.Row = 6;
			this.HourPad.Size = new System.Drawing.Size(291, 365);
			this.HourPad.TabIndex = 5;
			this.HourPad.PadClick += new ButtonListPadEventHandler(this.HourPad_PadClick);
			this.groupPanel2.BackColor = Color.Transparent;
			this.groupPanel2.Caption = null;
			Control.ControlCollection controls1 = this.groupPanel2.Controls;
			fieldCustName = new Control[] { this.BtnGotoToday, this.LblSelectedDate, this.LblCustName, this.FieldSeat, this.FieldInputType, this.NumberKeyPad, this.BtnSearch, this.PanCustName, this.LblReserveInfo };
			controls1.AddRange(fieldCustName);
			this.groupPanel2.Font = new System.Drawing.Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.groupPanel2.Location = new Point(328, 192);
			this.groupPanel2.Name = "groupPanel2";
			this.groupPanel2.ShowHeader = false;
			this.groupPanel2.Size = new System.Drawing.Size(344, 560);
			this.groupPanel2.TabIndex = 59;
			this.BtnGotoToday.BackColor = Color.Transparent;
			this.BtnGotoToday.Blue = 2f;
			this.BtnGotoToday.Cursor = Cursors.Hand;
			this.BtnGotoToday.Font = new System.Drawing.Font("Tahoma", 12f, FontStyle.Regular, GraphicsUnit.Pixel);
			this.BtnGotoToday.Green = 1f;
			this.BtnGotoToday.Image = (Bitmap)resourceManager.GetObject("BtnGotoToday.Image");
			this.BtnGotoToday.ImageClick = (Bitmap)resourceManager.GetObject("BtnGotoToday.ImageClick");
			this.BtnGotoToday.ImageClickIndex = 1;
			this.BtnGotoToday.ImageIndex = 0;
			this.BtnGotoToday.ImageList = this.ButtonLiteImgList;
			this.BtnGotoToday.Location = new Point(224, 48);
			this.BtnGotoToday.Name = "BtnGotoToday";
			this.BtnGotoToday.ObjectValue = null;
			this.BtnGotoToday.Red = 1f;
			this.BtnGotoToday.Size = new System.Drawing.Size(110, 40);
			this.BtnGotoToday.TabIndex = 54;
			this.BtnGotoToday.Text = "Today";
			this.BtnGotoToday.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnGotoToday.Click += new EventHandler(this.BtnGotoToday_Click);
			this.ButtonLiteImgList.ColorDepth = ColorDepth.Depth32Bit;
			this.ButtonLiteImgList.ImageSize = new System.Drawing.Size(110, 40);
			this.ButtonLiteImgList.ImageStream = (ImageListStreamer)resourceManager.GetObject("ButtonLiteImgList.ImageStream");
			this.ButtonLiteImgList.TransparentColor = Color.Transparent;
			this.LblSelectedDate.BackColor = Color.Transparent;
			this.LblSelectedDate.BorderStyle = BorderStyle.FixedSingle;
			this.LblSelectedDate.Cursor = Cursors.Hand;
			this.LblSelectedDate.Font = new System.Drawing.Font("Tahoma", 12f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.LblSelectedDate.ForeColor = Color.Black;
			this.LblSelectedDate.Location = new Point(8, 48);
			this.LblSelectedDate.Name = "LblSelectedDate";
			this.LblSelectedDate.Size = new System.Drawing.Size(208, 40);
			this.LblSelectedDate.TabIndex = 53;
			this.LblSelectedDate.Text = "dd MMMM yyyy HH:mm";
			this.LblSelectedDate.TextAlign = ContentAlignment.MiddleCenter;
			this.LblSelectedDate.Click += new EventHandler(this.LblSelectedDate_Click);
			this.LblCustName.BackColor = Color.Transparent;
			this.LblCustName.Font = new System.Drawing.Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.LblCustName.ForeColor = Color.Black;
			this.LblCustName.Location = new Point(16, 88);
			this.LblCustName.Name = "LblCustName";
			this.LblCustName.Size = new System.Drawing.Size(144, 24);
			this.LblCustName.TabIndex = 51;
			this.LblCustName.Text = "Customer Name";
			this.LblCustName.TextAlign = ContentAlignment.MiddleLeft;
			this.FieldSeat.BackColor = Color.Black;
			this.FieldSeat.Font = new System.Drawing.Font("Tahoma", 14.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.FieldSeat.ForeColor = Color.Cyan;
			this.FieldSeat.Location = new Point(136, 248);
			this.FieldSeat.Name = "FieldSeat";
			this.FieldSeat.Size = new System.Drawing.Size(200, 40);
			this.FieldSeat.TabIndex = 9;
			this.FieldSeat.TextAlign = ContentAlignment.MiddleCenter;
			this.FieldInputType.BackColor = Color.Black;
			this.FieldInputType.Font = new System.Drawing.Font("Tahoma", 14.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.FieldInputType.ForeColor = Color.Cyan;
			this.FieldInputType.Location = new Point(8, 248);
			this.FieldInputType.Name = "FieldInputType";
			this.FieldInputType.Size = new System.Drawing.Size(128, 40);
			this.FieldInputType.TabIndex = 8;
			this.FieldInputType.Text = "Seat";
			this.FieldInputType.TextAlign = ContentAlignment.MiddleCenter;
			this.NumberKeyPad.BackColor = Color.White;
			this.NumberKeyPad.Image = (Bitmap)resourceManager.GetObject("NumberKeyPad.Image");
			this.NumberKeyPad.ImageClick = (Bitmap)resourceManager.GetObject("NumberKeyPad.ImageClick");
			this.NumberKeyPad.ImageClickIndex = 1;
			this.NumberKeyPad.ImageIndex = 0;
			this.NumberKeyPad.ImageList = this.NumberImgList;
			this.NumberKeyPad.Location = new Point(64, 296);
			this.NumberKeyPad.Name = "NumberKeyPad";
			this.NumberKeyPad.Size = new System.Drawing.Size(226, 255);
			this.NumberKeyPad.TabIndex = 7;
			this.NumberKeyPad.Text = "numberPad1";
			this.NumberKeyPad.PadClick += new NumberPadEventHandler(this.NumberKeyPad_PadClick);
			this.LblReserveInfo.BackColor = Color.Black;
			this.LblReserveInfo.Font = new System.Drawing.Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.LblReserveInfo.ForeColor = Color.White;
			this.LblReserveInfo.Name = "LblReserveInfo";
			this.LblReserveInfo.Size = new System.Drawing.Size(344, 40);
			this.LblReserveInfo.TabIndex = 52;
			this.LblReserveInfo.Text = "Reserve Information";
			this.LblReserveInfo.TextAlign = ContentAlignment.MiddleCenter;
			this.DatePad.AutoRefresh = true;
			this.DatePad.BackColor = Color.White;
			this.DatePad.Blue = 1f;
			this.DatePad.Column = 7;
			this.DatePad.Font = new System.Drawing.Font("Tahoma", 12f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.DatePad.Green = 1f;
			this.DatePad.Image = (Bitmap)resourceManager.GetObject("DatePad.Image");
			this.DatePad.ImageClick = (Bitmap)resourceManager.GetObject("DatePad.ImageClick");
			this.DatePad.ImageClickIndex = 1;
			this.DatePad.ImageIndex = 0;
			this.DatePad.ImageList = this.ButtonImgList;
			this.DatePad.Location = new Point(116, 2);
			this.DatePad.Name = "DatePad";
			this.DatePad.Padding = 1;
			this.DatePad.Red = 1f;
			this.DatePad.Row = 1;
			this.DatePad.Size = new System.Drawing.Size(776, 60);
			this.DatePad.TabIndex = 6;
			this.DatePad.PadClick += new ButtonListPadEventHandler(this.DatePad_PadClick);
			this.BtnDayDown.BackColor = Color.Transparent;
			this.BtnDayDown.Blue = 2f;
			this.BtnDayDown.Cursor = Cursors.Hand;
			this.BtnDayDown.Green = 1f;
			this.BtnDayDown.Image = (Bitmap)resourceManager.GetObject("BtnDayDown.Image");
			this.BtnDayDown.ImageClick = (Bitmap)resourceManager.GetObject("BtnDayDown.ImageClick");
			this.BtnDayDown.ImageClickIndex = 5;
			this.BtnDayDown.ImageIndex = 4;
			this.BtnDayDown.ImageList = this.ButtonImgList;
			this.BtnDayDown.Location = new Point(893, 2);
			this.BtnDayDown.Name = "BtnDayDown";
			this.BtnDayDown.ObjectValue = null;
			this.BtnDayDown.Red = 1f;
			this.BtnDayDown.Size = new System.Drawing.Size(110, 60);
			this.BtnDayDown.TabIndex = 51;
			this.BtnDayDown.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnDayDown.Click += new EventHandler(this.BtnDayDown_Click);
			this.BtnDayUp.BackColor = Color.Transparent;
			this.BtnDayUp.Blue = 2f;
			this.BtnDayUp.Cursor = Cursors.Hand;
			this.BtnDayUp.Green = 1f;
			this.BtnDayUp.Image = (Bitmap)resourceManager.GetObject("BtnDayUp.Image");
			this.BtnDayUp.ImageClick = (Bitmap)resourceManager.GetObject("BtnDayUp.ImageClick");
			this.BtnDayUp.ImageClickIndex = 3;
			this.BtnDayUp.ImageIndex = 2;
			this.BtnDayUp.ImageList = this.ButtonImgList;
			this.BtnDayUp.Location = new Point(5, 2);
			this.BtnDayUp.Name = "BtnDayUp";
			this.BtnDayUp.ObjectValue = null;
			this.BtnDayUp.Red = 1f;
			this.BtnDayUp.Size = new System.Drawing.Size(110, 60);
			this.BtnDayUp.TabIndex = 50;
			this.BtnDayUp.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnDayUp.Click += new EventHandler(this.BtnDayUp_Click);
			this.GroupDate.BackColor = Color.Transparent;
			this.GroupDate.Caption = null;
			Control.ControlCollection controlCollections1 = this.GroupDate.Controls;
			fieldCustName = new Control[] { this.BtnDayUp, this.DatePad, this.BtnDayDown };
			controlCollections1.AddRange(fieldCustName);
			this.GroupDate.Font = new System.Drawing.Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.GroupDate.Location = new Point(8, 128);
			this.GroupDate.Name = "GroupDate";
			this.GroupDate.ShowHeader = false;
			this.GroupDate.Size = new System.Drawing.Size(1008, 64);
			this.GroupDate.TabIndex = 62;
			this.LblHeaderNumber.BackColor = Color.Black;
			this.LblHeaderNumber.Font = new System.Drawing.Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.LblHeaderNumber.ForeColor = Color.White;
			this.LblHeaderNumber.Location = new Point(232, 192);
			this.LblHeaderNumber.Name = "LblHeaderNumber";
			this.LblHeaderNumber.Size = new System.Drawing.Size(48, 40);
			this.LblHeaderNumber.TabIndex = 64;
			this.LblHeaderNumber.Text = "#";
			this.LblHeaderNumber.TextAlign = ContentAlignment.MiddleCenter;
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
			base.ClientSize = new System.Drawing.Size(1020, 764);
			Control.ControlCollection controls2 = base.Controls;
			fieldCustName = new Control[] { this.LblHeaderNumber, this.ListReserveID, this.GroupDate, this.LblPageID, this.PanSelectTime, this.groupPanel2, this.LblHeaderTime, this.ListReserveTime, this.LblHeaderSeat, this.ListReserveSeat, this.BtnCancel, this.BtnReserve, this.BtnMain, this.LblCopyright, this.LblHeaderCustomer, this.BtnDown, this.BtnUp, this.ListReserveQueue, this.BtnDinIn };
			controls2.AddRange(fieldCustName);
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
			this.selectedReserve = (TableReserve)e.Item.Value;
			this.UpdateReserveButton();
		}

		private void MinutePad_PadClick(object sender, ButtonListPadEventArgs e)
		{
			int num = int.Parse(e.Value);
			this.selectedDate = this.selectedDate.AddMinutes((double)(num - this.selectedDate.Minute));
			this.UpdateSelectedTime();
		}

		private void NumberKeyPad_PadClick(object sender, NumberPadEventArgs e)
		{
			if (e.IsNumeric)
			{
				Label fieldSeat = this.FieldSeat;
				string text = fieldSeat.Text;
				int number = e.Number;
				fieldSeat.Text = string.Concat(text, number.ToString());
			}
			else if (e.IsCancel)
			{
				if (this.FieldSeat.Text.Length <= 1)
				{
					this.FieldSeat.Text = "";
				}
				else
				{
					this.FieldSeat.Text = this.FieldSeat.Text.Substring(0, this.FieldSeat.Text.Length - 1);
				}
			}
			this.UpdateReserveButton();
		}

		private DateTime SearchStartOfWeek(DateTime date)
		{
			if (date.DayOfWeek <= DayOfWeek.Sunday)
			{
				return date.Add(new TimeSpan(-date.Hour, -date.Minute, 0));
			}
			DateTime dateTime = date.AddDays((double)(-(int)date.DayOfWeek));
			return dateTime.Add(new TimeSpan(-date.Hour, -date.Minute, 0));
		}

		private void UpdateDateButton()
		{
			DateTime dateTime = this.startDate;
			this.DatePad.AutoRefresh = false;
			this.DatePad.Items.Clear();
			this.DatePad.Red = 1f;
			this.DatePad.Green = 1f;
			this.DatePad.Blue = 1f;
			for (int i = 0; i < 7; i++)
			{
				ButtonItem buttonItem = new ButtonItem(dateTime.ToString("dd MMM yyyy", AppParameter.Culture), i.ToString());
				this.DatePad.Items.Add(buttonItem);
				dateTime = dateTime.AddDays(1);
			}
			this.DatePad.AutoRefresh = true;
			this.UpdateReserveQueue();
		}

		public override void UpdateForm()
		{
			DateTime today = DateTime.Today;
			this.selectedDate = today.Add(new TimeSpan(8, 0, 0));
			this.startDate = this.SearchStartOfWeek(this.selectedDate);
			this.selectedReserve = null;
			this.FieldCustName.Text = ReserveForm.FIELD_CUST_TEXT;
			this.FieldSeat.Text = "";
			if (!AppParameter.IsDemo())
			{
				this.FieldInputType.Text = "Seat";
			}
			else
			{
				this.FieldInputType.Text = "Guest";
			}
			this.LblPageID.Text = string.Concat("Employee ID:", ((MainForm)base.MdiParent).UserID.ToString(), " | STRT010");
			this.UpdateDateButton();
			this.UpdateSelectedTime();
		}

		private void UpdateReserveButton()
		{
			this.BtnReserve.Enabled = (!(this.FieldCustName.Text != "") || !(this.FieldCustName.Text != ReserveForm.FIELD_CUST_TEXT) ? false : this.FieldSeat.Text != "");
			this.BtnUp.Enabled = this.ListReserveTime.CanUp;
			this.BtnDown.Enabled = this.ListReserveTime.CanDown;
			if (this.selectedReserve == null)
			{
				this.BtnDinIn.Enabled = false;
				this.BtnCancel.Enabled = false;
				return;
			}
			this.BtnDinIn.Enabled = true;
			this.BtnCancel.Enabled = true;
		}

		private void UpdateReserveQueue()
		{
			this.reserveList = (new smartRestaurant.ReserveService.ReserveService()).GetTableReserve(this.selectedDate);
			StringBuilder stringBuilder = new StringBuilder();
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
				return;
			}
			this.ListReserveTime.SelectedIndex = -1;
			for (int i = 0; i < (int)this.reserveList.Length; i++)
			{
				stringBuilder.Length = 0;
				stringBuilder.Append(this.reserveList[i].custFirstName);
				stringBuilder.Append(" ");
				stringBuilder.Append(this.reserveList[i].custMiddleName);
				stringBuilder.Append(" ");
				stringBuilder.Append(this.reserveList[i].custLastName);
				DataItem dataItem = new DataItem(this.reserveList[i].reserveDate.ToString("HH:mm"), this.reserveList[i], false);
				this.ListReserveTime.Items.Add(dataItem);
				dataItem = new DataItem(stringBuilder.ToString(), this.reserveList[i], false);
				this.ListReserveQueue.Items.Add(dataItem);
				dataItem = new DataItem(this.reserveList[i].reserveTableID.ToString(), this.reserveList[i], false);
				this.ListReserveID.Items.Add(dataItem);
				dataItem = new DataItem(this.reserveList[i].seat.ToString(), this.reserveList[i], false);
				this.ListReserveSeat.Items.Add(dataItem);
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

		private void UpdateSelectedTime()
		{
			int minute = this.selectedDate.Minute / 15;
			if (minute > 3)
			{
				this.selectedDate = this.selectedDate.Add(new TimeSpan(-1, -this.selectedDate.Minute, 0));
				minute = 0;
			}
			this.DatePad.AutoRefresh = false;
			this.HourPad.AutoRefresh = false;
			this.MinutePad.AutoRefresh = false;
			ButtonListPad hourPad = this.HourPad;
			ButtonListPad buttonListPad = this.HourPad;
			float single = 1f;
			float single1 = single;
			this.HourPad.Blue = single;
			float single2 = single1;
			single1 = single2;
			buttonListPad.Green = single2;
			hourPad.Red = single1;
			this.HourPad.SetMatrix(this.selectedDate.Hour, 1f, 2f, 1f);
			ButtonListPad minutePad = this.MinutePad;
			ButtonListPad minutePad1 = this.MinutePad;
			float single3 = 1f;
			single1 = single3;
			this.MinutePad.Blue = single3;
			float single4 = single1;
			single1 = single4;
			minutePad1.Green = single4;
			minutePad.Red = single1;
			this.MinutePad.SetMatrix(minute, 1f, 2f, 1f);
			DateTime dateTime = this.startDate;
			ButtonListPad datePad = this.DatePad;
			ButtonListPad datePad1 = this.DatePad;
			float single5 = 1f;
			single1 = single5;
			this.DatePad.Blue = single5;
			float single6 = single1;
			single1 = single6;
			datePad1.Green = single6;
			datePad.Red = single1;
			for (int i = 0; i < 7; i++)
			{
				if (dateTime.Date == this.selectedDate.Date)
				{
					this.DatePad.SetMatrix(i, 0.5f, 1f, 1f);
				}
				dateTime = dateTime.AddDays(1);
			}
			this.DatePad.AutoRefresh = true;
			this.HourPad.AutoRefresh = true;
			this.MinutePad.AutoRefresh = true;
			this.LblSelectedDate.Text = this.selectedDate.ToString("dd MMMM yyyy HH:mm", AppParameter.Culture);
			this.UpdateReserveButton();
		}

		private void UpdateTimeButton()
		{
			for (int i = 0; i < 24; i++)
			{
				this.HourPad.Items.Add(new ButtonItem(i.ToString("D2"), i.ToString()));
			}
			this.MinutePad.Items.Add(new ButtonItem("00", "0"));
			this.MinutePad.Items.Add(new ButtonItem("15", "15"));
			this.MinutePad.Items.Add(new ButtonItem("30", "30"));
			this.MinutePad.Items.Add(new ButtonItem("45", "45"));
		}
	}
}