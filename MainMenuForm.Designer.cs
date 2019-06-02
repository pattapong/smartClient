using smartRestaurant.Controls;
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
using System.Timers;
using System.Windows.Forms;

namespace smartRestaurant
{
	public class MainMenuForm : SmartForm
	{
		private TableInformation[] tableInfo;

		private TableStatus[] tableStatus;

		private TableInformation takeOutTable;

		private OrderWaiting[] orderWaiting;

		private int waitingTop;

		private int waitingLeft;

		private int waitingRow;

		private int waitingColumn;

		private bool showDate;

		private bool showWaitingList;

		private ImageButton BtnExit;

		private GroupPanel TablePanel;

		private ImageButton BtnTakeOut;

		private ImageButton BtnReserve;

		private ImageButton BtnTakeOutList;

		private IContainer components;

		private ImageList ButtonImgList;

		private Label LblPageID;

		private Label LblCopyRight;

		private ButtonListPad TablePad;

		private ImageList ButtonLiteImgList;

		private ImageList NumberImgList;

		private ImageButton BtnDown;

		private ImageButton BtnUp;

		private ImageButton BtnRight;

		private ImageButton BtnLeft;

		private ButtonListPad BillItemPad;

		private ImageButton BtnManager;

		private System.Timers.Timer TimerClock;

		private Label LblClock;

		private ImageList CalculatorImgList;

		private ImageButton BtnCalculator;

		private ImageButton BtnWaitingList;

		private ImageButton BtnRefresh;

		private ImageList RefreshImgList;

		private GroupPanel WaitingListPanel;

		public MainMenuForm()
		{
			this.InitializeComponent();
			this.showDate = false;
			this.showWaitingList = AppParameter.WaitingListEnabled;
			this.LoadTableInformation();
			this.InitWaitingList();
		}

		private void AddTableButton()
		{
			try
			{
				try
				{
					this.TablePad.AutoRefresh = false;
					this.TablePad.Red = 1f;
					this.TablePad.Green = 1f;
					this.TablePad.Blue = 1f;
					this.TablePad.Items.Clear();
					this.tableStatus = (new smartRestaurant.TableService.TableService()).GetTableStatus();
					if (this.tableStatus != null && (int)this.tableStatus.Length > 1)
					{
						for (int i = 0; i < (int)this.tableStatus.Length - 1; i++)
						{
							ButtonItem buttonItem = new ButtonItem(this.tableStatus[i + 1].TableName, this.tableStatus[i + 1]);
							if (this.tableStatus[i + 1].LockInUse)
							{
								buttonItem.IsLock = true;
							}
							this.TablePad.Items.Add(buttonItem);
						}
					}
				}
				catch (Exception exception)
				{
					MessageBox.Show(exception.ToString());
				}
			}
			finally
			{
				this.UpdateTableStatus();
				this.TablePad.AutoRefresh = true;
			}
		}

		private void BillItemPad_PadClick(object sender, ButtonListPadEventArgs e)
		{
			if (e.ObjectValue == null)
			{
				return;
			}
			int objectValue = (int)e.ObjectValue;
			smartRestaurant.OrderService.OrderService orderService = new smartRestaurant.OrderService.OrderService();
			if (objectValue >= 0)
			{
				this.orderWaiting = orderService.ServeWaitingOrder(0, objectValue);
			}
			else
			{
				objectValue = -objectValue;
				this.orderWaiting = orderService.ServeWaitingOrder(objectValue, 0);
			}
			this.AddTableButton();
			this.UpdateWaitingList();
		}

		private void BtnCalculator_Click(object sender, EventArgs e)
		{
			CalculatorForm.Show(true);
		}

		private void BtnDown_Click(object sender, EventArgs e)
		{
			if (this.waitingTop + this.BillItemPad.Row - 1 >= this.waitingRow)
			{
				return;
			}
			MainMenuForm row = this;
			row.waitingTop = row.waitingTop + (this.BillItemPad.Row - 1);
			this.UpdateWaitingList();
		}

		private void BtnExit_Click(object sender, EventArgs e)
		{
			UserProfile.CheckLogout(((MainForm)base.MdiParent).User.UserID);
			((MainForm)base.MdiParent).User = null;
			((MainForm)base.MdiParent).ShowLoginForm();
		}

		private void BtnLeft_Click(object sender, EventArgs e)
		{
			if (this.waitingLeft == 0)
			{
				return;
			}
			this.waitingLeft -= this.BillItemPad.Column;
			if (this.waitingLeft < 0)
			{
				this.waitingLeft = 0;
			}
			this.UpdateWaitingList();
		}

		private void BtnManager_Click(object sender, EventArgs e)
		{
			((MainForm)base.MdiParent).ShowSalesForm();
		}

		private void BtnRefresh_Click(object sender, EventArgs e)
		{
			this.UpdateForm();
		}

		private void BtnReserve_Click(object sender, EventArgs e)
		{
			((MainForm)base.MdiParent).ShowReserveForm();
		}

		private void BtnRight_Click(object sender, EventArgs e)
		{
			if (this.waitingLeft + this.BillItemPad.Column >= this.waitingColumn)
			{
				return;
			}
			this.waitingLeft += this.BillItemPad.Column;
			this.UpdateWaitingList();
		}

		private void BtnTakeOut_Click(object sender, EventArgs e)
		{
			((MainForm)base.MdiParent).ShowTakeOrderForm(this.takeOutTable, 0, 0, "");
		}

		private void BtnTakeOutList_Click(object sender, EventArgs e)
		{
			((MainForm)base.MdiParent).ShowTakeOutForm(this.takeOutTable, 0, null, false);
		}

		private void BtnUp_Click(object sender, EventArgs e)
		{
			if (this.waitingTop == 0)
			{
				return;
			}
			MainMenuForm row = this;
			row.waitingTop = row.waitingTop - (this.BillItemPad.Row - 1);
			if (this.waitingTop < 0)
			{
				this.waitingTop = 0;
			}
			this.UpdateWaitingList();
		}

		private void BtnWaitingList_Click(object sender, EventArgs e)
		{
			this.showWaitingList = !this.showWaitingList;
			this.UpdateForm();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			ResourceManager resourceManager = new ResourceManager(typeof(MainMenuForm));
			this.BtnExit = new ImageButton();
			this.ButtonImgList = new ImageList(this.components);
			this.TablePanel = new GroupPanel();
			this.BtnRefresh = new ImageButton();
			this.RefreshImgList = new ImageList(this.components);
			this.TablePad = new ButtonListPad();
			this.NumberImgList = new ImageList(this.components);
			this.BtnTakeOut = new ImageButton();
			this.BtnReserve = new ImageButton();
			this.BtnTakeOutList = new ImageButton();
			this.BtnManager = new ImageButton();
			this.LblPageID = new Label();
			this.LblCopyRight = new Label();
			this.WaitingListPanel = new GroupPanel();
			this.BtnDown = new ImageButton();
			this.BtnUp = new ImageButton();
			this.BtnRight = new ImageButton();
			this.ButtonLiteImgList = new ImageList(this.components);
			this.BtnLeft = new ImageButton();
			this.BillItemPad = new ButtonListPad();
			this.TimerClock = new System.Timers.Timer();
			this.LblClock = new Label();
			this.CalculatorImgList = new ImageList(this.components);
			this.BtnCalculator = new ImageButton();
			this.BtnWaitingList = new ImageButton();
			this.TablePanel.SuspendLayout();
			this.WaitingListPanel.SuspendLayout();
			((ISupportInitialize)this.TimerClock).BeginInit();
			base.SuspendLayout();
			this.BtnExit.BackColor = Color.Transparent;
			this.BtnExit.Blue = 2f;
			this.BtnExit.Cursor = Cursors.Hand;
			this.BtnExit.Green = 2f;
			this.BtnExit.ImageClick = (Image)resourceManager.GetObject("BtnExit.ImageClick");
			this.BtnExit.ImageClickIndex = 1;
			this.BtnExit.ImageIndex = 0;
			this.BtnExit.ImageList = this.ButtonImgList;
			this.BtnExit.IsLock = false;
			this.BtnExit.Location = new Point(880, 692);
			this.BtnExit.Name = "BtnExit";
			this.BtnExit.ObjectValue = null;
			this.BtnExit.Red = 1f;
			this.BtnExit.Size = new System.Drawing.Size(110, 60);
			this.BtnExit.TabIndex = 0;
			this.BtnExit.Text = "Logout";
			this.BtnExit.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnExit.Click += new EventHandler(this.BtnExit_Click);
			this.ButtonImgList.ColorDepth = ColorDepth.Depth32Bit;
			this.ButtonImgList.ImageSize = new System.Drawing.Size(110, 60);
			this.ButtonImgList.ImageStream = (ImageListStreamer)resourceManager.GetObject("ButtonImgList.ImageStream");
			this.ButtonImgList.TransparentColor = Color.Transparent;
			this.TablePanel.BackColor = Color.Transparent;
			this.TablePanel.Caption = "Select Table";
			this.TablePanel.Controls.Add(this.BtnRefresh);
			this.TablePanel.Controls.Add(this.TablePad);
			this.TablePanel.Font = new System.Drawing.Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.TablePanel.Location = new Point(24, 144);
			this.TablePanel.Name = "TablePanel";
			this.TablePanel.ShowHeader = true;
			this.TablePanel.Size = new System.Drawing.Size(976, 264);
			this.TablePanel.TabIndex = 1;
			this.BtnRefresh.BackColor = Color.Transparent;
			this.BtnRefresh.Blue = 1f;
			this.BtnRefresh.Cursor = Cursors.Hand;
			this.BtnRefresh.Green = 1f;
			this.BtnRefresh.ImageClick = (Image)resourceManager.GetObject("BtnRefresh.ImageClick");
			this.BtnRefresh.ImageClickIndex = 1;
			this.BtnRefresh.ImageIndex = 0;
			this.BtnRefresh.ImageList = this.RefreshImgList;
			this.BtnRefresh.IsLock = false;
			this.BtnRefresh.Location = new Point(944, 0);
			this.BtnRefresh.Name = "BtnRefresh";
			this.BtnRefresh.ObjectValue = null;
			this.BtnRefresh.Red = 1f;
			this.BtnRefresh.Size = new System.Drawing.Size(30, 30);
			this.BtnRefresh.TabIndex = 8;
			this.BtnRefresh.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnRefresh.Click += new EventHandler(this.BtnRefresh_Click);
			this.RefreshImgList.ImageSize = new System.Drawing.Size(30, 30);
			this.RefreshImgList.ImageStream = (ImageListStreamer)resourceManager.GetObject("RefreshImgList.ImageStream");
			this.RefreshImgList.TransparentColor = Color.Transparent;
			this.TablePad.AutoRefresh = true;
			this.TablePad.BackColor = Color.White;
			this.TablePad.Blue = 1f;
			this.TablePad.Column = 8;
			this.TablePad.Font = new System.Drawing.Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.TablePad.Green = 1f;
			this.TablePad.Image = (Image)resourceManager.GetObject("TablePad.Image");
			this.TablePad.ImageClick = (Image)resourceManager.GetObject("TablePad.ImageClick");
			this.TablePad.ImageClickIndex = 1;
			this.TablePad.ImageIndex = 0;
			this.TablePad.ImageList = this.ButtonImgList;
			this.TablePad.ItemStart = 0;
			this.TablePad.Location = new Point(13, 48);
			this.TablePad.Name = "TablePad";
			this.TablePad.Padding = 10;
			this.TablePad.Red = 1f;
			this.TablePad.Row = 3;
			this.TablePad.Size = new System.Drawing.Size(950, 200);
			this.TablePad.TabIndex = 7;
			this.TablePad.PadClick += new ButtonListPadEventHandler(this.TablePad_PadClick);
			this.TablePad.PageChange += new ButtonListPadEventHandler(this.TablePad_PageChange);
			this.NumberImgList.ColorDepth = ColorDepth.Depth32Bit;
			this.NumberImgList.ImageSize = new System.Drawing.Size(72, 60);
			this.NumberImgList.ImageStream = (ImageListStreamer)resourceManager.GetObject("NumberImgList.ImageStream");
			this.NumberImgList.TransparentColor = Color.Transparent;
			this.BtnTakeOut.BackColor = Color.Transparent;
			this.BtnTakeOut.Blue = 2f;
			this.BtnTakeOut.Cursor = Cursors.Hand;
			this.BtnTakeOut.Green = 1f;
			this.BtnTakeOut.ImageClick = (Image)resourceManager.GetObject("BtnTakeOut.ImageClick");
			this.BtnTakeOut.ImageClickIndex = 1;
			this.BtnTakeOut.ImageIndex = 0;
			this.BtnTakeOut.ImageList = this.ButtonImgList;
			this.BtnTakeOut.IsLock = false;
			this.BtnTakeOut.Location = new Point(608, 72);
			this.BtnTakeOut.Name = "BtnTakeOut";
			this.BtnTakeOut.ObjectValue = null;
			this.BtnTakeOut.Red = 1f;
			this.BtnTakeOut.Size = new System.Drawing.Size(110, 60);
			this.BtnTakeOut.TabIndex = 3;
			this.BtnTakeOut.Text = "Take Out";
			this.BtnTakeOut.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnTakeOut.Click += new EventHandler(this.BtnTakeOut_Click);
			this.BtnReserve.BackColor = Color.Transparent;
			this.BtnReserve.Blue = 1f;
			this.BtnReserve.Cursor = Cursors.Hand;
			this.BtnReserve.Green = 1f;
			this.BtnReserve.ImageClick = (Image)resourceManager.GetObject("BtnReserve.ImageClick");
			this.BtnReserve.ImageClickIndex = 1;
			this.BtnReserve.ImageIndex = 0;
			this.BtnReserve.ImageList = this.ButtonImgList;
			this.BtnReserve.IsLock = false;
			this.BtnReserve.Location = new Point(880, 72);
			this.BtnReserve.Name = "BtnReserve";
			this.BtnReserve.ObjectValue = null;
			this.BtnReserve.Red = 2f;
			this.BtnReserve.Size = new System.Drawing.Size(110, 60);
			this.BtnReserve.TabIndex = 4;
			this.BtnReserve.Text = "Reserve";
			this.BtnReserve.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnReserve.Click += new EventHandler(this.BtnReserve_Click);
			this.BtnTakeOutList.BackColor = Color.Transparent;
			this.BtnTakeOutList.Blue = 2f;
			this.BtnTakeOutList.Cursor = Cursors.Hand;
			this.BtnTakeOutList.Green = 1f;
			this.BtnTakeOutList.ImageClick = (Image)resourceManager.GetObject("BtnTakeOutList.ImageClick");
			this.BtnTakeOutList.ImageClickIndex = 1;
			this.BtnTakeOutList.ImageIndex = 0;
			this.BtnTakeOutList.ImageList = this.ButtonImgList;
			this.BtnTakeOutList.IsLock = false;
			this.BtnTakeOutList.Location = new Point(744, 72);
			this.BtnTakeOutList.Name = "BtnTakeOutList";
			this.BtnTakeOutList.ObjectValue = null;
			this.BtnTakeOutList.Red = 2f;
			this.BtnTakeOutList.Size = new System.Drawing.Size(110, 60);
			this.BtnTakeOutList.TabIndex = 5;
			this.BtnTakeOutList.Text = "Take Out List";
			this.BtnTakeOutList.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnTakeOutList.Click += new EventHandler(this.BtnTakeOutList_Click);
			this.BtnManager.BackColor = Color.Transparent;
			this.BtnManager.Blue = 1f;
			this.BtnManager.Cursor = Cursors.Hand;
			this.BtnManager.Green = 2f;
			this.BtnManager.ImageClick = (Image)resourceManager.GetObject("BtnManager.ImageClick");
			this.BtnManager.ImageClickIndex = 1;
			this.BtnManager.ImageIndex = 0;
			this.BtnManager.ImageList = this.ButtonImgList;
			this.BtnManager.IsLock = false;
			this.BtnManager.Location = new Point(32, 692);
			this.BtnManager.Name = "BtnManager";
			this.BtnManager.ObjectValue = null;
			this.BtnManager.Red = 1f;
			this.BtnManager.Size = new System.Drawing.Size(110, 60);
			this.BtnManager.TabIndex = 6;
			this.BtnManager.Text = "Manager";
			this.BtnManager.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnManager.Click += new EventHandler(this.BtnManager_Click);
			this.LblPageID.BackColor = Color.Transparent;
			this.LblPageID.Font = new System.Drawing.Font("Tahoma", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.LblPageID.ForeColor = Color.FromArgb(103, 138, 198);
			this.LblPageID.Location = new Point(784, 752);
			this.LblPageID.Name = "LblPageID";
			this.LblPageID.Size = new System.Drawing.Size(224, 23);
			this.LblPageID.TabIndex = 33;
			this.LblPageID.Text = "STST010";
			this.LblPageID.TextAlign = ContentAlignment.TopRight;
			this.LblCopyRight.BackColor = Color.Transparent;
			this.LblCopyRight.Font = new System.Drawing.Font("Tahoma", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.LblCopyRight.ForeColor = Color.FromArgb(103, 138, 198);
			this.LblCopyRight.Location = new Point(8, 752);
			this.LblCopyRight.Name = "LblCopyRight";
			this.LblCopyRight.Size = new System.Drawing.Size(280, 16);
			this.LblCopyRight.TabIndex = 36;
			this.LblCopyRight.Text = "Copyright (c) 2004. All rights reserved.";
			this.WaitingListPanel.BackColor = Color.Transparent;
			this.WaitingListPanel.Caption = null;
			this.WaitingListPanel.Controls.Add(this.BtnDown);
			this.WaitingListPanel.Controls.Add(this.BtnUp);
			this.WaitingListPanel.Controls.Add(this.BtnRight);
			this.WaitingListPanel.Controls.Add(this.BtnLeft);
			this.WaitingListPanel.Controls.Add(this.BillItemPad);
			this.WaitingListPanel.Font = new System.Drawing.Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.WaitingListPanel.Location = new Point(24, 408);
			this.WaitingListPanel.Name = "WaitingListPanel";
			this.WaitingListPanel.ShowHeader = false;
			this.WaitingListPanel.Size = new System.Drawing.Size(976, 280);
			this.WaitingListPanel.TabIndex = 37;
			this.BtnDown.BackColor = Color.Transparent;
			this.BtnDown.Blue = 1f;
			this.BtnDown.Cursor = Cursors.Hand;
			this.BtnDown.Green = 2f;
			this.BtnDown.ImageClick = (Image)resourceManager.GetObject("BtnDown.ImageClick");
			this.BtnDown.ImageClickIndex = 1;
			this.BtnDown.ImageIndex = 0;
			this.BtnDown.ImageList = this.NumberImgList;
			this.BtnDown.IsLock = false;
			this.BtnDown.Location = new Point(896, 176);
			this.BtnDown.Name = "BtnDown";
			this.BtnDown.ObjectValue = null;
			this.BtnDown.Red = 2f;
			this.BtnDown.Size = new System.Drawing.Size(72, 60);
			this.BtnDown.TabIndex = 11;
			this.BtnDown.Text = "Down";
			this.BtnDown.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnDown.Click += new EventHandler(this.BtnDown_Click);
			this.BtnDown.DoubleClick += new EventHandler(this.BtnDown_Click);
			this.BtnUp.BackColor = Color.Transparent;
			this.BtnUp.Blue = 1f;
			this.BtnUp.Cursor = Cursors.Hand;
			this.BtnUp.Green = 2f;
			this.BtnUp.ImageClick = (Image)resourceManager.GetObject("BtnUp.ImageClick");
			this.BtnUp.ImageClickIndex = 1;
			this.BtnUp.ImageIndex = 0;
			this.BtnUp.ImageList = this.NumberImgList;
			this.BtnUp.IsLock = false;
			this.BtnUp.Location = new Point(896, 8);
			this.BtnUp.Name = "BtnUp";
			this.BtnUp.ObjectValue = null;
			this.BtnUp.Red = 2f;
			this.BtnUp.Size = new System.Drawing.Size(72, 60);
			this.BtnUp.TabIndex = 10;
			this.BtnUp.Text = "Up";
			this.BtnUp.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnUp.Click += new EventHandler(this.BtnUp_Click);
			this.BtnUp.DoubleClick += new EventHandler(this.BtnUp_Click);
			this.BtnRight.BackColor = Color.Transparent;
			this.BtnRight.Blue = 1f;
			this.BtnRight.Cursor = Cursors.Hand;
			this.BtnRight.Green = 2f;
			this.BtnRight.ImageClick = (Image)resourceManager.GetObject("BtnRight.ImageClick");
			this.BtnRight.ImageClickIndex = 1;
			this.BtnRight.ImageIndex = 0;
			this.BtnRight.ImageList = this.ButtonLiteImgList;
			this.BtnRight.IsLock = false;
			this.BtnRight.Location = new Point(778, 240);
			this.BtnRight.Name = "BtnRight";
			this.BtnRight.ObjectValue = null;
			this.BtnRight.Red = 2f;
			this.BtnRight.Size = new System.Drawing.Size(110, 40);
			this.BtnRight.TabIndex = 9;
			this.BtnRight.Text = ">>";
			this.BtnRight.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnRight.Click += new EventHandler(this.BtnRight_Click);
			this.BtnRight.DoubleClick += new EventHandler(this.BtnRight_Click);
			this.ButtonLiteImgList.ColorDepth = ColorDepth.Depth32Bit;
			this.ButtonLiteImgList.ImageSize = new System.Drawing.Size(110, 40);
			this.ButtonLiteImgList.ImageStream = (ImageListStreamer)resourceManager.GetObject("ButtonLiteImgList.ImageStream");
			this.ButtonLiteImgList.TransparentColor = Color.Transparent;
			this.BtnLeft.BackColor = Color.Transparent;
			this.BtnLeft.Blue = 1f;
			this.BtnLeft.Cursor = Cursors.Hand;
			this.BtnLeft.Green = 2f;
			this.BtnLeft.ImageClick = (Image)resourceManager.GetObject("BtnLeft.ImageClick");
			this.BtnLeft.ImageClickIndex = 1;
			this.BtnLeft.ImageIndex = 0;
			this.BtnLeft.ImageList = this.ButtonLiteImgList;
			this.BtnLeft.IsLock = false;
			this.BtnLeft.Location = new Point(8, 240);
			this.BtnLeft.Name = "BtnLeft";
			this.BtnLeft.ObjectValue = null;
			this.BtnLeft.Red = 2f;
			this.BtnLeft.Size = new System.Drawing.Size(110, 40);
			this.BtnLeft.TabIndex = 8;
			this.BtnLeft.Text = "<<";
			this.BtnLeft.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnLeft.Click += new EventHandler(this.BtnLeft_Click);
			this.BtnLeft.DoubleClick += new EventHandler(this.BtnLeft_Click);
			this.BillItemPad.AutoRefresh = true;
			this.BillItemPad.BackColor = Color.White;
			this.BillItemPad.Blue = 1f;
			this.BillItemPad.Column = 8;
			this.BillItemPad.Font = new System.Drawing.Font("Tahoma", 12f, FontStyle.Regular, GraphicsUnit.Pixel);
			this.BillItemPad.Green = 1f;
			this.BillItemPad.Image = (Image)resourceManager.GetObject("BillItemPad.Image");
			this.BillItemPad.ImageClick = (Image)resourceManager.GetObject("BillItemPad.ImageClick");
			this.BillItemPad.ImageClickIndex = 1;
			this.BillItemPad.ImageIndex = 0;
			this.BillItemPad.ImageList = this.ButtonLiteImgList;
			this.BillItemPad.ItemStart = 0;
			this.BillItemPad.Location = new Point(8, 1);
			this.BillItemPad.Name = "BillItemPad";
			this.BillItemPad.Padding = 0;
			this.BillItemPad.Red = 1f;
			this.BillItemPad.Row = 6;
			this.BillItemPad.Size = new System.Drawing.Size(880, 240);
			this.BillItemPad.TabIndex = 7;
			this.BillItemPad.Text = "BillItemPad";
			this.BillItemPad.PadClick += new ButtonListPadEventHandler(this.BillItemPad_PadClick);
			this.TimerClock.Enabled = true;
			this.TimerClock.Interval = 1000;
			this.TimerClock.SynchronizingObject = this;
			this.TimerClock.Elapsed += new ElapsedEventHandler(this.TimerClock_Elapsed);
			this.LblClock.BackColor = Color.Transparent;
			this.LblClock.Cursor = Cursors.Hand;
			this.LblClock.Font = new System.Drawing.Font("Tahoma", 20.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.LblClock.Location = new Point(736, 8);
			this.LblClock.Name = "LblClock";
			this.LblClock.Size = new System.Drawing.Size(280, 40);
			this.LblClock.TabIndex = 38;
			this.LblClock.Text = "Sun 99:99:99";
			this.LblClock.TextAlign = ContentAlignment.MiddleRight;
			this.LblClock.Click += new EventHandler(this.LblClock_Click);
			this.CalculatorImgList.ColorDepth = ColorDepth.Depth32Bit;
			this.CalculatorImgList.ImageSize = new System.Drawing.Size(40, 40);
			this.CalculatorImgList.ImageStream = (ImageListStreamer)resourceManager.GetObject("CalculatorImgList.ImageStream");
			this.CalculatorImgList.TransparentColor = Color.Transparent;
			this.BtnCalculator.BackColor = Color.Transparent;
			this.BtnCalculator.Blue = 1f;
			this.BtnCalculator.Cursor = Cursors.Hand;
			this.BtnCalculator.Green = 1f;
			this.BtnCalculator.ImageClick = (Image)resourceManager.GetObject("BtnCalculator.ImageClick");
			this.BtnCalculator.ImageClickIndex = 1;
			this.BtnCalculator.ImageIndex = 0;
			this.BtnCalculator.ImageList = this.CalculatorImgList;
			this.BtnCalculator.IsLock = false;
			this.BtnCalculator.Location = new Point(8, 64);
			this.BtnCalculator.Name = "BtnCalculator";
			this.BtnCalculator.ObjectValue = null;
			this.BtnCalculator.Red = 1f;
			this.BtnCalculator.Size = new System.Drawing.Size(40, 40);
			this.BtnCalculator.TabIndex = 39;
			this.BtnCalculator.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnCalculator.Click += new EventHandler(this.BtnCalculator_Click);
			this.BtnWaitingList.BackColor = Color.Transparent;
			this.BtnWaitingList.Blue = 2f;
			this.BtnWaitingList.Cursor = Cursors.Hand;
			this.BtnWaitingList.Green = 1f;
			this.BtnWaitingList.ImageClick = (Image)resourceManager.GetObject("BtnWaitingList.ImageClick");
			this.BtnWaitingList.ImageClickIndex = 1;
			this.BtnWaitingList.ImageIndex = 0;
			this.BtnWaitingList.ImageList = this.ButtonImgList;
			this.BtnWaitingList.IsLock = false;
			this.BtnWaitingList.Location = new Point(744, 692);
			this.BtnWaitingList.Name = "BtnWaitingList";
			this.BtnWaitingList.ObjectValue = null;
			this.BtnWaitingList.Red = 1f;
			this.BtnWaitingList.Size = new System.Drawing.Size(110, 60);
			this.BtnWaitingList.TabIndex = 40;
			this.BtnWaitingList.Text = "Waiting List";
			this.BtnWaitingList.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnWaitingList.Click += new EventHandler(this.BtnWaitingList_Click);
			this.BtnWaitingList.DoubleClick += new EventHandler(this.BtnWaitingList_Click);
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
			base.ClientSize = new System.Drawing.Size(1020, 764);
			base.Controls.Add(this.BtnWaitingList);
			base.Controls.Add(this.BtnCalculator);
			base.Controls.Add(this.LblClock);
			base.Controls.Add(this.WaitingListPanel);
			base.Controls.Add(this.LblCopyRight);
			base.Controls.Add(this.LblPageID);
			base.Controls.Add(this.BtnManager);
			base.Controls.Add(this.BtnTakeOutList);
			base.Controls.Add(this.BtnReserve);
			base.Controls.Add(this.BtnTakeOut);
			base.Controls.Add(this.TablePanel);
			base.Controls.Add(this.BtnExit);
			base.Name = "MainMenuForm";
			this.Text = "Select Table";
			this.TablePanel.ResumeLayout(false);
			this.WaitingListPanel.ResumeLayout(false);
			((ISupportInitialize)this.TimerClock).EndInit();
			base.ResumeLayout(false);
		}

		private void InitWaitingList()
		{
			this.WaitingListPanel.Visible = this.showWaitingList;
			try
			{
				try
				{
					this.BillItemPad.AutoRefresh = false;
					for (int i = 0; i < this.BillItemPad.Row; i++)
					{
						for (int j = 0; j < this.BillItemPad.Column; j++)
						{
							ButtonItem buttonItem = new ButtonItem(null, null);
							this.BillItemPad.Items.Add(buttonItem);
							if (i == 0)
							{
								this.BillItemPad.SetMatrix(j, 1f, 1f, 2f);
							}
						}
					}
				}
				catch (Exception exception)
				{
					MessageBox.Show(exception.ToString());
				}
			}
			finally
			{
				this.BillItemPad.AutoRefresh = true;
			}
		}

		private void LblClock_Click(object sender, EventArgs e)
		{
			this.showDate = !this.showDate;
		}

		private void LoadTableInformation()
		{
			smartRestaurant.TableService.TableService tableService = new smartRestaurant.TableService.TableService();
			this.tableInfo = tableService.GetTableList();
			while (this.tableInfo == null)
			{
				if (MessageBox.Show("Can't load table information.", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Hand) != System.Windows.Forms.DialogResult.Cancel)
				{
					this.tableInfo = tableService.GetTableList();
				}
				else
				{
					((MainForm)base.MdiParent).Exit();
				}
			}
			for (int i = 0; i < (int)this.tableInfo.Length; i++)
			{
				if (this.tableInfo[i].TableID == 0)
				{
					this.takeOutTable = this.tableInfo[i];
					return;
				}
			}
		}

		private void LoadTableListStyle()
		{
			string tableListStyle = AppParameter.TableListStyle;
			if (tableListStyle == "1")
			{
				this.TablePad.ImageList = this.ButtonImgList;
				this.TablePad.ImageIndex = 0;
				this.TablePad.ImageClickIndex = 1;
				this.TablePad.Padding = 10;
				this.TablePad.Top = 48;
				this.TablePad.Left = 13;
				this.TablePad.Column = 8;
				this.TablePad.Row = (this.showWaitingList ? 3 : 7);
				return;
			}
			if (tableListStyle == "2")
			{
				this.TablePad.ImageList = this.NumberImgList;
				this.TablePad.ImageIndex = 0;
				this.TablePad.ImageClickIndex = 1;
				this.TablePad.Padding = 8;
				this.TablePad.Top = 48;
				this.TablePad.Left = 13;
				this.TablePad.Column = 12;
				this.TablePad.Row = (this.showWaitingList ? 3 : 7);
				return;
			}
			if (tableListStyle == "3")
			{
				this.TablePad.ImageList = this.ButtonLiteImgList;
				this.TablePad.ImageIndex = 0;
				this.TablePad.ImageClickIndex = 1;
				this.TablePad.Padding = 5;
				this.TablePad.Top = 36;
				this.TablePad.Left = 31;
				this.TablePad.Column = 8;
				this.TablePad.Row = (this.showWaitingList ? 5 : 11);
			}
		}

		private void TablePad_PadClick(object sender, ButtonListPadEventArgs e)
		{
			if (e.ObjectValue == null)
			{
				return;
			}
			TableStatus objectValue = (TableStatus)e.ObjectValue;
			for (int i = 0; i < (int)this.tableInfo.Length; i++)
			{
				if (this.tableInfo[i].TableID == objectValue.TableID)
				{
					((MainForm)base.MdiParent).ShowTakeOrderForm(this.tableInfo[i]);
					return;
				}
			}
			MessageForm.Show("Select Table", "Can't find table information.");
		}

		private void TablePad_PageChange(object sender, ButtonListPadEventArgs e)
		{
			this.UpdateTableStatus();
		}

		private void TimerClock_Elapsed(object sender, ElapsedEventArgs e)
		{
			DateTime now;
			if (this.showDate)
			{
				Label lblClock = this.LblClock;
				now = DateTime.Now;
				lblClock.Text = now.ToString("ddd d MMM, hh:mm", AppParameter.Culture);
				return;
			}
			Label str = this.LblClock;
			now = DateTime.Now;
			str.Text = now.ToString("ddd HH:mm:ss", AppParameter.Culture);
		}

		public override void UpdateForm()
		{
			this.LblPageID.Text = string.Concat("Employee ID:", ((MainForm)base.MdiParent).UserID, " | STST010");
			if (!AppParameter.IsDemo())
			{
				this.BtnManager.Visible = (((MainForm)base.MdiParent).User.IsManager() ? true : ((MainForm)base.MdiParent).User.IsAuditor());
				this.BtnReserve.Visible = false;
				this.BtnWaitingList.Visible = AppParameter.ShowWaitingListButton;
				this.WaitingListPanel.Visible = this.showWaitingList;
				if (!this.showWaitingList)
				{
					this.TablePanel.Height = 544;
					this.LoadTableListStyle();
				}
				else
				{
					this.TablePanel.Height = 264;
					this.LoadTableListStyle();
					int num = 0;
					int num1 = num;
					this.waitingLeft = num;
					this.waitingTop = num1;
					this.orderWaiting = (new smartRestaurant.OrderService.OrderService()).GetBillDetailWaitingList();
					this.UpdateWaitingList();
				}
			}
			else
			{
				this.BtnManager.Visible = false;
				this.BtnReserve.Visible = true;
				this.TablePanel.Height = 544;
				this.TablePad.Row = 7;
			}
			this.AddTableButton();
		}

		private void UpdateTableStatus()
		{
			try
			{
				try
				{
					this.TablePad.AutoRefresh = true;
					this.TablePad.AutoRefresh = false;
					this.TablePad.Red = 1f;
					this.TablePad.Green = 1f;
					this.TablePad.Blue = 1f;
					if (this.tableStatus != null && (int)this.tableStatus.Length > 1)
					{
						for (int i = 0; i < (int)this.tableStatus.Length - 1; i++)
						{
							bool flag = false;
							int num = 0;
							while (num < (int)this.TablePad.Buttons.Length)
							{
								if (this.TablePad.Buttons[num].ObjectValue == null || (TableStatus)this.TablePad.Buttons[num].ObjectValue != this.tableStatus[i + 1])
								{
									num++;
								}
								else
								{
									flag = true;
									break;
								}
							}
							if (flag)
							{
								this.UpdateTableStatusColor(num, this.tableStatus[i + 1]);
							}
						}
					}
				}
				catch (Exception exception)
				{
					MessageBox.Show(exception.ToString());
				}
			}
			finally
			{
				this.TablePad.AutoRefresh = true;
			}
		}

		private void UpdateTableStatusColor(int padPos, TableStatus tStatus)
		{
			if (tStatus.InUse)
			{
				if (tStatus.IsWaitingItem)
				{
					this.TablePad.SetMatrix(padPos, 1f, 2f, 2f);
				}
				else if (!tStatus.IsPrintBill)
				{
					this.TablePad.SetMatrix(padPos, 2f, 2f, 1f);
				}
				else
				{
					this.TablePad.SetMatrix(padPos, 2f, 1f, 2f);
				}
			}
			this.TablePad.SetLock(padPos, tStatus.LockInUse);
		}

		private void UpdateWaitingList()
		{
			ButtonItem item;
			if (!this.showWaitingList)
			{
				return;
			}
			try
			{
				try
				{
					OrderWaiting[] orderWaitingArray = this.orderWaiting;
					StringBuilder stringBuilder = new StringBuilder();
					this.BillItemPad.AutoRefresh = false;
					if (orderWaitingArray != null)
					{
						this.waitingColumn = (int)orderWaitingArray.Length;
						this.waitingRow = 0;
						int num = this.waitingLeft;
						for (int i = 0; i < this.BillItemPad.Column; i++)
						{
							item = (ButtonItem)this.BillItemPad.Items[i];
							if ((int)orderWaitingArray.Length > num)
							{
								if (this.waitingRow < (int)orderWaitingArray[num].Items.Length)
								{
									this.waitingRow = (int)orderWaitingArray[num].Items.Length;
								}
								stringBuilder.Length = 0;
								stringBuilder.Append("Serve all\n");
								stringBuilder.Append(orderWaitingArray[num].TableName);
								item.Text = stringBuilder.ToString();
								item.ObjectValue = -orderWaitingArray[num].OrderID;
								int num1 = this.waitingTop;
								for (int j = 1; j < this.BillItemPad.Row; j++)
								{
									item = (ButtonItem)this.BillItemPad.Items[i + j * this.BillItemPad.Column];
									if ((int)orderWaitingArray[num].Items.Length > num1)
									{
										OrderBillItemWaiting items = orderWaitingArray[num].Items[num1];
										stringBuilder.Length = 0;
										stringBuilder.Append(items.MenuKeyID);
										if (items.Unit > 1)
										{
											stringBuilder.Append(" (");
											stringBuilder.Append(items.Unit);
											stringBuilder.Append(")");
										}
										if (items.Choice != null && items.Choice != "")
										{
											stringBuilder.Append("\n");
											if (items.Choice.Length <= 12)
											{
												stringBuilder.Append(items.Choice);
											}
											else
											{
												stringBuilder.Append(items.Choice.Substring(0, 12));
												stringBuilder.Append("...");
											}
										}
										item.Text = stringBuilder.ToString();
										item.ObjectValue = orderWaitingArray[num].Items[num1].BillDetailID;
									}
									else
									{
										item.Text = null;
										item.ObjectValue = null;
									}
									num1++;
								}
								num++;
							}
							else
							{
								item.Text = null;
								item.ObjectValue = null;
								for (int k = 1; k < this.BillItemPad.Row; k++)
								{
									item = (ButtonItem)this.BillItemPad.Items[i + k * this.BillItemPad.Column];
									item.Text = null;
									item.ObjectValue = null;
								}
							}
						}
					}
					else
					{
						for (int l = 0; l < this.BillItemPad.Items.Count; l++)
						{
							item = (ButtonItem)this.BillItemPad.Items[l];
							item.Text = null;
							item.ObjectValue = null;
						}
						return;
					}
				}
				catch (Exception exception)
				{
					MessageBox.Show(exception.ToString());
				}
			}
			finally
			{
				this.BillItemPad.AutoRefresh = true;
			}
		}
	}
}