using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Text;
using System.Windows.Forms;
using smartRestaurant.Controls;
using smartRestaurant.Data;
using smartRestaurant.TableService;
using smartRestaurant.OrderService;
using smartRestaurant.Utils;

namespace smartRestaurant
{
	/// <summary>
	/// <b>MainMenuForm</b> is main form for user to select thing to do.
	/// </summary>
	public class MainMenuForm : SmartForm
	{
		private ButtonListPad BillItemPad;
		private ImageButton BtnCalculator;
		private ImageButton BtnDown;
		private ImageButton BtnExit;
		private ImageButton BtnLeft;
		private ImageButton BtnManager;
		private ImageButton BtnRefresh;
		private ImageButton BtnReserve;
		private ImageButton BtnRight;
		private ImageButton BtnTakeOut;
		private ImageButton BtnTakeOutList;
		private ImageButton BtnUp;
		private ImageButton BtnWaitingList;
		private ImageList ButtonImgList;
		private ImageList ButtonLiteImgList;
		private ImageList CalculatorImgList;
		private IContainer components;
		private Label LblClock;
		private Label LblCopyRight;
		private Label LblPageID;
		private ImageList NumberImgList;
		private OrderWaiting[] orderWaiting;
		private ImageList RefreshImgList;
		private bool showDate;
		private bool showWaitingList;
		private TableInformation[] tableInfo;
		private ButtonListPad TablePad;
		private GroupPanel TablePanel;
		private TableStatus[] tableStatus;
		private TableInformation takeOutTable;
		private Timer TimerClock;
		private int waitingColumn;
		private int waitingLeft;
		private GroupPanel WaitingListPanel;
		private int waitingRow;
		private int waitingTop;

		// Methods
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
				this.TablePad.AutoRefresh = false;
				this.TablePad.Red = 1f;
				this.TablePad.Green = 1f;
				this.TablePad.Blue = 1f;
				this.TablePad.Items.Clear();
				this.tableStatus = new smartRestaurant.TableService.TableService().GetTableStatus();
				if ((this.tableStatus != null) && (this.tableStatus.Length > 1))
				{
					for (int i = 0; i < (this.tableStatus.Length - 1); i++)
					{
						ButtonItem item = new ButtonItem(this.tableStatus[i + 1].TableName, this.tableStatus[i + 1]);
						if (this.tableStatus[i + 1].LockInUse)
						{
							item.IsLock = true;
						}
						this.TablePad.Items.Add(item);
					}
				}
			}
			catch (Exception exception)
			{
				MessageBox.Show(exception.ToString());
			}
			finally
			{
				this.UpdateTableStatus();
				this.TablePad.AutoRefresh = true;
			}
		}

		private void BillItemPad_PadClick(object sender, ButtonListPadEventArgs e)
		{
			if (e.ObjectValue != null)
			{
				int objectValue = (int) e.ObjectValue;
				smartRestaurant.OrderService.OrderService service = new smartRestaurant.OrderService.OrderService();
				if (objectValue < 0)
				{
					objectValue = -objectValue;
					this.orderWaiting = service.ServeWaitingOrder(objectValue, 0);
				}
				else
				{
					this.orderWaiting = service.ServeWaitingOrder(0, objectValue);
				}
				this.AddTableButton();
				this.UpdateWaitingList();
			}
		}

		private void BtnCalculator_Click(object sender, EventArgs e)
		{
			CalculatorForm.Show(true);
		}

		private void BtnDown_Click(object sender, EventArgs e)
		{
			if (((this.waitingTop + this.BillItemPad.Row) - 1) < this.waitingRow)
			{
				this.waitingTop += this.BillItemPad.Row - 1;
				this.UpdateWaitingList();
			}
		}

		private void BtnExit_Click(object sender, EventArgs e)
		{
			UserProfile.CheckLogout(((MainForm) base.MdiParent).User.UserID);
			((MainForm) base.MdiParent).User = null;
			((MainForm) base.MdiParent).ShowLoginForm();
		}

		private void BtnLeft_Click(object sender, EventArgs e)
		{
			if (this.waitingLeft != 0)
			{
				this.waitingLeft -= this.BillItemPad.Column;
				if (this.waitingLeft < 0)
				{
					this.waitingLeft = 0;
				}
				this.UpdateWaitingList();
			}
		}

		private void BtnManager_Click(object sender, EventArgs e)
		{
			((MainForm) base.MdiParent).ShowSalesForm();
		}

		private void BtnRefresh_Click(object sender, EventArgs e)
		{
			this.UpdateForm();
		}

		private void BtnReserve_Click(object sender, EventArgs e)
		{
			((MainForm) base.MdiParent).ShowReserveForm();
		}

		private void BtnRight_Click(object sender, EventArgs e)
		{
			if ((this.waitingLeft + this.BillItemPad.Column) < this.waitingColumn)
			{
				this.waitingLeft += this.BillItemPad.Column;
				this.UpdateWaitingList();
			}
		}

		private void BtnTakeOut_Click(object sender, EventArgs e)
		{
			((MainForm) base.MdiParent).ShowTakeOrderForm(this.takeOutTable, 0, 0, "");
		}

		private void BtnTakeOutList_Click(object sender, EventArgs e)
		{
			((MainForm) base.MdiParent).ShowTakeOutForm(this.takeOutTable, 0, null, false);
		}

		private void BtnUp_Click(object sender, EventArgs e)
		{
			if (this.waitingTop != 0)
			{
				this.waitingTop -= this.BillItemPad.Row - 1;
				if (this.waitingTop < 0)
				{
					this.waitingTop = 0;
				}
				this.UpdateWaitingList();
			}
		}

		private void BtnWaitingList_Click(object sender, EventArgs e)
		{
			this.showWaitingList = !this.showWaitingList;
			this.UpdateForm();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && (this.components != null))
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.BtnExit = new smartRestaurant.Controls.ImageButton();
			this.ButtonImgList = new System.Windows.Forms.ImageList(this.components);
			this.TablePanel = new smartRestaurant.Controls.GroupPanel();
			this.BtnRefresh = new smartRestaurant.Controls.ImageButton();
			this.RefreshImgList = new System.Windows.Forms.ImageList(this.components);
			this.TablePad = new smartRestaurant.Controls.ButtonListPad();
			this.NumberImgList = new System.Windows.Forms.ImageList(this.components);
			this.BtnTakeOut = new smartRestaurant.Controls.ImageButton();
			this.BtnReserve = new smartRestaurant.Controls.ImageButton();
			this.BtnTakeOutList = new smartRestaurant.Controls.ImageButton();
			this.BtnManager = new smartRestaurant.Controls.ImageButton();
			this.LblPageID = new System.Windows.Forms.Label();
			this.LblCopyRight = new System.Windows.Forms.Label();
			this.WaitingListPanel = new smartRestaurant.Controls.GroupPanel();
			this.BtnDown = new smartRestaurant.Controls.ImageButton();
			this.BtnUp = new smartRestaurant.Controls.ImageButton();
			this.BtnRight = new smartRestaurant.Controls.ImageButton();
			this.ButtonLiteImgList = new System.Windows.Forms.ImageList(this.components);
			this.BtnLeft = new smartRestaurant.Controls.ImageButton();
			this.BillItemPad = new smartRestaurant.Controls.ButtonListPad();
			this.TimerClock = new System.Windows.Forms.Timer(this.components);
			this.LblClock = new System.Windows.Forms.Label();
			this.CalculatorImgList = new System.Windows.Forms.ImageList(this.components);
			this.BtnCalculator = new smartRestaurant.Controls.ImageButton();
			this.BtnWaitingList = new smartRestaurant.Controls.ImageButton();
			this.TablePanel.SuspendLayout();
			this.WaitingListPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// BtnExit
			// 
			this.BtnExit.BackColor = System.Drawing.Color.Transparent;
			this.BtnExit.Blue = 2F;
			this.BtnExit.Cursor = System.Windows.Forms.Cursors.Hand;
			this.BtnExit.Green = 2F;
			this.BtnExit.ImageClick = null;
			this.BtnExit.ImageClickIndex = 1;
			this.BtnExit.ImageList = this.ButtonImgList;
			this.BtnExit.Location = new System.Drawing.Point(880, 692);
			this.BtnExit.Name = "BtnExit";
			this.BtnExit.ObjectValue = null;
			this.BtnExit.Red = 1F;
			this.BtnExit.Size = new System.Drawing.Size(110, 60);
			this.BtnExit.TabIndex = 0;
			this.BtnExit.Text = "Logout";
			this.BtnExit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.BtnExit.Click += new System.EventHandler(this.BtnExit_Click);
			// 
			// ButtonImgList
			// 
			this.ButtonImgList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			this.ButtonImgList.ImageSize = new System.Drawing.Size(110, 60);
			this.ButtonImgList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// TablePanel
			// 
			this.TablePanel.BackColor = System.Drawing.Color.Transparent;
			this.TablePanel.Caption = "Select Table";
			this.TablePanel.Controls.Add(this.TablePad);
			this.TablePanel.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
			this.TablePanel.Location = new System.Drawing.Point(24, 144);
			this.TablePanel.Name = "TablePanel";
			this.TablePanel.ShowHeader = true;
			this.TablePanel.Size = new System.Drawing.Size(976, 264);
			this.TablePanel.TabIndex = 1;
			// 
			// BtnRefresh
			// 
			this.BtnRefresh.BackColor = System.Drawing.Color.Transparent;
			this.BtnRefresh.Blue = 1F;
			this.BtnRefresh.Cursor = System.Windows.Forms.Cursors.Hand;
			this.BtnRefresh.Green = 1F;
			this.BtnRefresh.ImageClick = null;
			this.BtnRefresh.ImageClickIndex = 1;
			this.BtnRefresh.ImageList = this.RefreshImgList;
			this.BtnRefresh.Location = new System.Drawing.Point(944, 0);
			this.BtnRefresh.Name = "BtnRefresh";
			this.BtnRefresh.ObjectValue = null;
			this.BtnRefresh.Red = 1F;
			this.BtnRefresh.Size = new System.Drawing.Size(30, 30);
			this.BtnRefresh.TabIndex = 8;
			this.BtnRefresh.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.BtnRefresh.Click += new System.EventHandler(this.BtnRefresh_Click);
			// 
			// RefreshImgList
			// 
			this.RefreshImgList.ImageSize = new System.Drawing.Size(30, 30);
			this.RefreshImgList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// TablePad
			// 
			this.TablePad.AutoRefresh = true;
			this.TablePad.BackColor = System.Drawing.Color.White;
			this.TablePad.Blue = 1F;
			this.TablePad.Column = 8;
			this.TablePad.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
			this.TablePad.Green = 1F;
			this.TablePad.Image = null;
			this.TablePad.ImageClick = null;
			this.TablePad.ImageClickIndex = 1;
			this.TablePad.ImageIndex = 0;
			this.TablePad.ImageList = this.ButtonImgList;
			this.TablePad.ItemStart = 0;
			this.TablePad.Location = new System.Drawing.Point(13, 48);
			this.TablePad.Name = "TablePad";
			this.TablePad.Padding = 10;
			this.TablePad.Red = 1F;
			this.TablePad.Row = 3;
			this.TablePad.Size = new System.Drawing.Size(950, 200);
			this.TablePad.TabIndex = 7;
			this.TablePad.PadClick += new smartRestaurant.Controls.ButtonListPadEventHandler(this.TablePad_PadClick);
			this.TablePad.PageChange += new smartRestaurant.Controls.ButtonListPadEventHandler(this.TablePad_PageChange);
			// 
			// NumberImgList
			// 
			this.NumberImgList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			this.NumberImgList.ImageSize = new System.Drawing.Size(72, 60);
			this.NumberImgList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// BtnTakeOut
			// 
			this.BtnTakeOut.BackColor = System.Drawing.Color.Transparent;
			this.BtnTakeOut.Blue = 2F;
			this.BtnTakeOut.Cursor = System.Windows.Forms.Cursors.Hand;
			this.BtnTakeOut.Green = 1F;
			this.BtnTakeOut.ImageClick = null;
			this.BtnTakeOut.ImageClickIndex = 1;
			this.BtnTakeOut.ImageList = this.ButtonImgList;
			this.BtnTakeOut.Location = new System.Drawing.Point(608, 72);
			this.BtnTakeOut.Name = "BtnTakeOut";
			this.BtnTakeOut.ObjectValue = null;
			this.BtnTakeOut.Red = 1F;
			this.BtnTakeOut.Size = new System.Drawing.Size(110, 60);
			this.BtnTakeOut.TabIndex = 3;
			this.BtnTakeOut.Text = "Take Out";
			this.BtnTakeOut.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.BtnTakeOut.Click += new System.EventHandler(this.BtnTakeOut_Click);
			// 
			// BtnReserve
			// 
			this.BtnReserve.BackColor = System.Drawing.Color.Transparent;
			this.BtnReserve.Blue = 1F;
			this.BtnReserve.Cursor = System.Windows.Forms.Cursors.Hand;
			this.BtnReserve.Green = 1F;
			this.BtnReserve.ImageClick = null;
			this.BtnReserve.ImageClickIndex = 1;
			this.BtnReserve.ImageList = this.ButtonImgList;
			this.BtnReserve.Location = new System.Drawing.Point(880, 72);
			this.BtnReserve.Name = "BtnReserve";
			this.BtnReserve.ObjectValue = null;
			this.BtnReserve.Red = 2F;
			this.BtnReserve.Size = new System.Drawing.Size(110, 60);
			this.BtnReserve.TabIndex = 4;
			this.BtnReserve.Text = "Reserve";
			this.BtnReserve.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.BtnReserve.Click += new System.EventHandler(this.BtnReserve_Click);
			// 
			// BtnTakeOutList
			// 
			this.BtnTakeOutList.BackColor = System.Drawing.Color.Transparent;
			this.BtnTakeOutList.Blue = 2F;
			this.BtnTakeOutList.Cursor = System.Windows.Forms.Cursors.Hand;
			this.BtnTakeOutList.Green = 1F;
			this.BtnTakeOutList.ImageClick = null;
			this.BtnTakeOutList.ImageClickIndex = 1;
			this.BtnTakeOutList.ImageList = this.ButtonImgList;
			this.BtnTakeOutList.Location = new System.Drawing.Point(744, 72);
			this.BtnTakeOutList.Name = "BtnTakeOutList";
			this.BtnTakeOutList.ObjectValue = null;
			this.BtnTakeOutList.Red = 2F;
			this.BtnTakeOutList.Size = new System.Drawing.Size(110, 60);
			this.BtnTakeOutList.TabIndex = 5;
			this.BtnTakeOutList.Text = "Take Out List";
			this.BtnTakeOutList.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.BtnTakeOutList.Click += new System.EventHandler(this.BtnTakeOutList_Click);
			// 
			// BtnManager
			// 
			this.BtnManager.BackColor = System.Drawing.Color.Transparent;
			this.BtnManager.Blue = 1F;
			this.BtnManager.Cursor = System.Windows.Forms.Cursors.Hand;
			this.BtnManager.Green = 2F;
			this.BtnManager.ImageClick = null;
			this.BtnManager.ImageClickIndex = 1;
			this.BtnManager.ImageList = this.ButtonImgList;
			this.BtnManager.Location = new System.Drawing.Point(32, 692);
			this.BtnManager.Name = "BtnManager";
			this.BtnManager.ObjectValue = null;
			this.BtnManager.Red = 1F;
			this.BtnManager.Size = new System.Drawing.Size(110, 60);
			this.BtnManager.TabIndex = 6;
			this.BtnManager.Text = "Manager";
			this.BtnManager.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.BtnManager.Click += new System.EventHandler(this.BtnManager_Click);
			// 
			// LblPageID
			// 
			this.LblPageID.BackColor = System.Drawing.Color.Transparent;
			this.LblPageID.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.LblPageID.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(103)), ((System.Byte)(138)), ((System.Byte)(198)));
			this.LblPageID.Location = new System.Drawing.Point(784, 752);
			this.LblPageID.Name = "LblPageID";
			this.LblPageID.Size = new System.Drawing.Size(224, 23);
			this.LblPageID.TabIndex = 33;
			this.LblPageID.Text = "STST010";
			this.LblPageID.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// LblCopyRight
			// 
			this.LblCopyRight.BackColor = System.Drawing.Color.Transparent;
			this.LblCopyRight.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.LblCopyRight.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(103)), ((System.Byte)(138)), ((System.Byte)(198)));
			this.LblCopyRight.Location = new System.Drawing.Point(8, 752);
			this.LblCopyRight.Name = "LblCopyRight";
			this.LblCopyRight.Size = new System.Drawing.Size(280, 16);
			this.LblCopyRight.TabIndex = 36;
			this.LblCopyRight.Text = "Copyright (c) 2004. All rights reserved.";
			// 
			// WaitingListPanel
			// 
			this.WaitingListPanel.BackColor = System.Drawing.Color.Transparent;
			this.WaitingListPanel.Caption = null;
			this.WaitingListPanel.Controls.Add(this.BillItemPad);
			this.WaitingListPanel.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
			this.WaitingListPanel.Location = new System.Drawing.Point(24, 408);
			this.WaitingListPanel.Name = "WaitingListPanel";
			this.WaitingListPanel.ShowHeader = false;
			this.WaitingListPanel.Size = new System.Drawing.Size(976, 280);
			this.WaitingListPanel.TabIndex = 37;
			// 
			// BtnDown
			// 
			this.BtnDown.BackColor = System.Drawing.Color.Transparent;
			this.BtnDown.Blue = 1F;
			this.BtnDown.Cursor = System.Windows.Forms.Cursors.Hand;
			this.BtnDown.Green = 2F;
			this.BtnDown.ImageClick = null;
			this.BtnDown.ImageClickIndex = 1;
			this.BtnDown.ImageList = this.NumberImgList;
			this.BtnDown.Location = new System.Drawing.Point(896, 176);
			this.BtnDown.Name = "BtnDown";
			this.BtnDown.ObjectValue = null;
			this.BtnDown.Red = 2F;
			this.BtnDown.Size = new System.Drawing.Size(72, 60);
			this.BtnDown.TabIndex = 11;
			this.BtnDown.Text = "Down";
			this.BtnDown.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.BtnDown.Click += new System.EventHandler(this.BtnDown_Click);
			this.BtnDown.DoubleClick += new System.EventHandler(this.BtnDown_Click);
			// 
			// BtnUp
			// 
			this.BtnUp.BackColor = System.Drawing.Color.Transparent;
			this.BtnUp.Blue = 1F;
			this.BtnUp.Cursor = System.Windows.Forms.Cursors.Hand;
			this.BtnUp.Green = 2F;
			this.BtnUp.ImageClick = null;
			this.BtnUp.ImageClickIndex = 1;
			this.BtnUp.ImageList = this.NumberImgList;
			this.BtnUp.Location = new System.Drawing.Point(896, 8);
			this.BtnUp.Name = "BtnUp";
			this.BtnUp.ObjectValue = null;
			this.BtnUp.Red = 2F;
			this.BtnUp.Size = new System.Drawing.Size(72, 60);
			this.BtnUp.TabIndex = 10;
			this.BtnUp.Text = "Up";
			this.BtnUp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.BtnUp.Click += new System.EventHandler(this.BtnUp_Click);
			this.BtnUp.DoubleClick += new System.EventHandler(this.BtnUp_Click);
			// 
			// BtnRight
			// 
			this.BtnRight.BackColor = System.Drawing.Color.Transparent;
			this.BtnRight.Blue = 1F;
			this.BtnRight.Cursor = System.Windows.Forms.Cursors.Hand;
			this.BtnRight.Green = 2F;
			this.BtnRight.ImageClick = null;
			this.BtnRight.ImageClickIndex = 1;
			this.BtnRight.ImageList = this.ButtonLiteImgList;
			this.BtnRight.Location = new System.Drawing.Point(778, 240);
			this.BtnRight.Name = "BtnRight";
			this.BtnRight.ObjectValue = null;
			this.BtnRight.Red = 2F;
			this.BtnRight.Size = new System.Drawing.Size(110, 40);
			this.BtnRight.TabIndex = 9;
			this.BtnRight.Text = ">>";
			this.BtnRight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.BtnRight.Click += new System.EventHandler(this.BtnRight_Click);
			this.BtnRight.DoubleClick += new System.EventHandler(this.BtnRight_Click);
			// 
			// ButtonLiteImgList
			// 
			this.ButtonLiteImgList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			this.ButtonLiteImgList.ImageSize = new System.Drawing.Size(110, 40);
			this.ButtonLiteImgList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// BtnLeft
			// 
			this.BtnLeft.BackColor = System.Drawing.Color.Transparent;
			this.BtnLeft.Blue = 1F;
			this.BtnLeft.Cursor = System.Windows.Forms.Cursors.Hand;
			this.BtnLeft.Green = 2F;
			this.BtnLeft.ImageClick = null;
			this.BtnLeft.ImageClickIndex = 1;
			this.BtnLeft.ImageList = this.ButtonLiteImgList;
			this.BtnLeft.Location = new System.Drawing.Point(8, 240);
			this.BtnLeft.Name = "BtnLeft";
			this.BtnLeft.ObjectValue = null;
			this.BtnLeft.Red = 2F;
			this.BtnLeft.Size = new System.Drawing.Size(110, 40);
			this.BtnLeft.TabIndex = 8;
			this.BtnLeft.Text = "<<";
			this.BtnLeft.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.BtnLeft.Click += new System.EventHandler(this.BtnLeft_Click);
			this.BtnLeft.DoubleClick += new System.EventHandler(this.BtnLeft_Click);
			// 
			// BillItemPad
			// 
			this.BillItemPad.AutoRefresh = true;
			this.BillItemPad.BackColor = System.Drawing.Color.White;
			this.BillItemPad.Blue = 1F;
			this.BillItemPad.Column = 8;
			this.BillItemPad.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.BillItemPad.Green = 1F;
			this.BillItemPad.Image = null;
			this.BillItemPad.ImageClick = null;
			this.BillItemPad.ImageClickIndex = 1;
			this.BillItemPad.ImageIndex = 0;
			this.BillItemPad.ImageList = this.ButtonLiteImgList;
			this.BillItemPad.ItemStart = 0;
			this.BillItemPad.Location = new System.Drawing.Point(8, 1);
			this.BillItemPad.Name = "BillItemPad";
			this.BillItemPad.Padding = 0;
			this.BillItemPad.Red = 1F;
			this.BillItemPad.Row = 6;
			this.BillItemPad.Size = new System.Drawing.Size(880, 240);
			this.BillItemPad.TabIndex = 7;
			this.BillItemPad.Text = "BillItemPad";
			this.BillItemPad.PadClick += new smartRestaurant.Controls.ButtonListPadEventHandler(this.BillItemPad_PadClick);
			// 
			// TimerClock
			// 
			this.TimerClock.Enabled = true;
			this.TimerClock.Interval = 1000;
			// 
			// LblClock
			// 
			this.LblClock.BackColor = System.Drawing.Color.Transparent;
			this.LblClock.Cursor = System.Windows.Forms.Cursors.Hand;
			this.LblClock.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.LblClock.Location = new System.Drawing.Point(736, 8);
			this.LblClock.Name = "LblClock";
			this.LblClock.Size = new System.Drawing.Size(280, 40);
			this.LblClock.TabIndex = 38;
			this.LblClock.Text = "Sun 99:99:99";
			this.LblClock.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.LblClock.Click += new System.EventHandler(this.LblClock_Click);
			// 
			// CalculatorImgList
			// 
			this.CalculatorImgList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			this.CalculatorImgList.ImageSize = new System.Drawing.Size(40, 40);
			this.CalculatorImgList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// BtnCalculator
			// 
			this.BtnCalculator.BackColor = System.Drawing.Color.Transparent;
			this.BtnCalculator.Blue = 1F;
			this.BtnCalculator.Cursor = System.Windows.Forms.Cursors.Hand;
			this.BtnCalculator.Green = 1F;
			this.BtnCalculator.ImageClick = null;
			this.BtnCalculator.ImageClickIndex = 1;
			this.BtnCalculator.ImageList = this.CalculatorImgList;
			this.BtnCalculator.Location = new System.Drawing.Point(8, 64);
			this.BtnCalculator.Name = "BtnCalculator";
			this.BtnCalculator.ObjectValue = null;
			this.BtnCalculator.Red = 1F;
			this.BtnCalculator.Size = new System.Drawing.Size(40, 40);
			this.BtnCalculator.TabIndex = 39;
			this.BtnCalculator.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.BtnCalculator.Click += new System.EventHandler(this.BtnCalculator_Click);
			// 
			// BtnWaitingList
			// 
			this.BtnWaitingList.BackColor = System.Drawing.Color.Transparent;
			this.BtnWaitingList.Blue = 2F;
			this.BtnWaitingList.Cursor = System.Windows.Forms.Cursors.Hand;
			this.BtnWaitingList.Green = 1F;
			this.BtnWaitingList.ImageClick = null;
			this.BtnWaitingList.ImageClickIndex = 1;
			this.BtnWaitingList.ImageList = this.ButtonImgList;
			this.BtnWaitingList.Location = new System.Drawing.Point(744, 692);
			this.BtnWaitingList.Name = "BtnWaitingList";
			this.BtnWaitingList.ObjectValue = null;
			this.BtnWaitingList.Red = 1F;
			this.BtnWaitingList.Size = new System.Drawing.Size(110, 60);
			this.BtnWaitingList.TabIndex = 40;
			this.BtnWaitingList.Text = "Waiting List";
			this.BtnWaitingList.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.BtnWaitingList.Click += new System.EventHandler(this.BtnWaitingList_Click);
			this.BtnWaitingList.DoubleClick += new System.EventHandler(this.BtnWaitingList_Click);
			// 
			// MainMenuForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
			this.ClientSize = new System.Drawing.Size(1020, 764);
			this.Controls.Add(this.BtnWaitingList);
			this.Controls.Add(this.BtnCalculator);
			this.Controls.Add(this.LblClock);
			this.Controls.Add(this.WaitingListPanel);
			this.Controls.Add(this.LblCopyRight);
			this.Controls.Add(this.LblPageID);
			this.Controls.Add(this.BtnManager);
			this.Controls.Add(this.BtnTakeOutList);
			this.Controls.Add(this.BtnReserve);
			this.Controls.Add(this.BtnTakeOut);
			this.Controls.Add(this.TablePanel);
			this.Controls.Add(this.BtnExit);
			this.Name = "MainMenuForm";
			this.Text = "Select Table";
			this.TablePanel.ResumeLayout(false);
			this.WaitingListPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		private void InitWaitingList()
		{
			this.WaitingListPanel.Visible = this.showWaitingList;
			try
			{
				this.BillItemPad.AutoRefresh = false;
				for (int i = 0; i < this.BillItemPad.Row; i++)
				{
					for (int j = 0; j < this.BillItemPad.Column; j++)
					{
						ButtonItem item = new ButtonItem(null, null);
						this.BillItemPad.Items.Add(item);
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
			smartRestaurant.TableService.TableService service = new smartRestaurant.TableService.TableService();
			this.tableInfo = service.GetTableList();
			while (this.tableInfo == null)
			{
				if (MessageBox.Show("Can't load table information.", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Hand) == DialogResult.Cancel)
				{
					((MainForm) base.MdiParent).Exit();
				}
				else
				{
					this.tableInfo = service.GetTableList();
				}
			}
			for (int i = 0; i < this.tableInfo.Length; i++)
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
            // To load table style
            // Read from app.config
            //TableListStyle: Table List style to show on screen.
	        //Value:	1 - Dimension  8x3 (22,24) (diabled waiting list -  8x 7)
			//          2 - Dimension 12x3 (34,36) (diabled waiting list - 12x 7)
			//          3 - Dimension  8x5 (38,40) (diabled waiting list -  8x11)
			switch (AppParameter.TableListStyle)
			{
				case "1":
					this.TablePad.ImageList = this.ButtonImgList;
					this.TablePad.ImageIndex = 0;
					this.TablePad.ImageClickIndex = 1;
					this.TablePad.Padding = 10;
					this.TablePad.Top = 0x30;
					this.TablePad.Left = 13;
					this.TablePad.Column = 8;
					this.TablePad.Row = this.showWaitingList ? 3 : 7;
					return;

				case "2":
					this.TablePad.ImageList = this.NumberImgList;
					this.TablePad.ImageIndex = 0;
					this.TablePad.ImageClickIndex = 1;
					this.TablePad.Padding = 8;
					this.TablePad.Top = 0x30;
					this.TablePad.Left = 13;
					this.TablePad.Column = 12;
					this.TablePad.Row = this.showWaitingList ? 3 : 7;
					break;

				case "3":
					this.TablePad.ImageList = this.ButtonLiteImgList;
					this.TablePad.ImageIndex = 0;
					this.TablePad.ImageClickIndex = 1;
					this.TablePad.Padding = 5;
					this.TablePad.Top = 0x24;
					this.TablePad.Left = 0x1f;
					this.TablePad.Column = 8;
					this.TablePad.Row = this.showWaitingList ? 5 : 11;
					break;
			}
		}

		private void TablePad_PadClick(object sender, ButtonListPadEventArgs e)
		{
			if (e.ObjectValue != null)
			{
				TableStatus objectValue = (TableStatus) e.ObjectValue;
				for (int i = 0; i < this.tableInfo.Length; i++)
				{
					if (this.tableInfo[i].TableID == objectValue.TableID)
					{
						((MainForm) base.MdiParent).ShowTakeOrderForm(this.tableInfo[i]);
						return;
					}
				}
				MessageForm.Show("Select Table", "Can't find table information.");
			}
		}

		private void TablePad_PageChange(object sender, ButtonListPadEventArgs e)
		{
			this.UpdateTableStatus();
		}

		private void TimerClock_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			if (this.showDate)
			{
				this.LblClock.Text = DateTime.Now.ToString("ddd d MMM, hh:mm", AppParameter.Culture);
			}
			else
			{
				this.LblClock.Text = DateTime.Now.ToString("ddd HH:mm:ss", AppParameter.Culture);
			}
		}

		public override void UpdateForm()
		{
			this.LblPageID.Text = "Employee ID:" + ((MainForm) base.MdiParent).UserID + " | STST010";
			if (AppParameter.IsDemo())
			{
				this.BtnManager.Visible = false;
				this.BtnReserve.Visible = true;
				this.TablePanel.Height = 0x220;
				this.TablePad.Row = 7;
			}
			else
			{
				this.BtnManager.Visible = ((MainForm) base.MdiParent).User.IsManager() || ((MainForm) base.MdiParent).User.IsAuditor();
				this.BtnReserve.Visible = false;
				this.BtnWaitingList.Visible = AppParameter.ShowWaitingListButton;
				this.WaitingListPanel.Visible = this.showWaitingList;
				if (this.showWaitingList)
				{
					this.TablePanel.Height = 0x108;
					this.LoadTableListStyle();
					this.waitingTop = this.waitingLeft = 0;
					this.orderWaiting = new smartRestaurant.OrderService.OrderService().GetBillDetailWaitingList();
					this.UpdateWaitingList();
				}
				else
				{
					this.TablePanel.Height = 0x220;
					this.LoadTableListStyle();
				}
			}
			this.AddTableButton();
		}

		private void UpdateTableStatus()
		{
			try
			{
				this.TablePad.AutoRefresh = true;
				this.TablePad.AutoRefresh = false;
				this.TablePad.Red = 1f;
				this.TablePad.Green = 1f;
				this.TablePad.Blue = 1f;
				if ((this.tableStatus != null) && (this.tableStatus.Length > 1))
				{
					for (int i = 0; i < (this.tableStatus.Length - 1); i++)
					{
						bool flag = false;
						int index = 0;
						while (index < this.TablePad.Buttons.Length)
						{
							if ((this.TablePad.Buttons[index].ObjectValue != null) && (((TableStatus) this.TablePad.Buttons[index].ObjectValue) == this.tableStatus[i + 1]))
							{
								flag = true;
								break;
							}
							index++;
						}
						if (flag)
						{
							this.UpdateTableStatusColor(index, this.tableStatus[i + 1]);
						}
					}
				}
			}
			catch (Exception exception)
			{
				MessageBox.Show(exception.ToString());
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
				else if (tStatus.IsPrintBill)
				{
					this.TablePad.SetMatrix(padPos, 2f, 1f, 2f);
				}
				else
				{
					this.TablePad.SetMatrix(padPos, 2f, 2f, 1f);
				}
			}
			this.TablePad.SetLock(padPos, tStatus.LockInUse);
		}

		private void UpdateWaitingList()
		{
			if (this.showWaitingList)
			{
				try
				{
					ButtonItem item;
					OrderWaiting[] orderWaiting = this.orderWaiting;
					StringBuilder builder = new StringBuilder();
					this.BillItemPad.AutoRefresh = false;
					if (orderWaiting == null)
					{
						for (int i = 0; i < this.BillItemPad.Items.Count; i++)
						{
							item = (ButtonItem) this.BillItemPad.Items[i];
							item.Text = null;
							item.ObjectValue = null;
						}
					}
					else
					{
						this.waitingColumn = orderWaiting.Length;
						this.waitingRow = 0;
						int waitingLeft = this.waitingLeft;
						for (int j = 0; j < this.BillItemPad.Column; j++)
						{
							item = (ButtonItem) this.BillItemPad.Items[j];
							if (orderWaiting.Length <= waitingLeft)
							{
								item.Text = null;
								item.ObjectValue = null;
								for (int k = 1; k < this.BillItemPad.Row; k++)
								{
									item = (ButtonItem) this.BillItemPad.Items[j + (k * this.BillItemPad.Column)];
									item.Text = null;
									item.ObjectValue = null;
								}
							}
							else
							{
								if (this.waitingRow < orderWaiting[waitingLeft].Items.Length)
								{
									this.waitingRow = orderWaiting[waitingLeft].Items.Length;
								}
								builder.Length = 0;
								builder.Append("Serve all\n");
								builder.Append(orderWaiting[waitingLeft].TableName);
								item.Text = builder.ToString();
								item.ObjectValue = -orderWaiting[waitingLeft].OrderID;
								int waitingTop = this.waitingTop;
								for (int m = 1; m < this.BillItemPad.Row; m++)
								{
									item = (ButtonItem) this.BillItemPad.Items[j + (m * this.BillItemPad.Column)];
									if (orderWaiting[waitingLeft].Items.Length <= waitingTop)
									{
										item.Text = null;
										item.ObjectValue = null;
									}
									else
									{
										OrderBillItemWaiting waiting = orderWaiting[waitingLeft].Items[waitingTop];
										builder.Length = 0;
										builder.Append(waiting.MenuKeyID);
										if (waiting.Unit > 1)
										{
											builder.Append(" (");
											builder.Append(waiting.Unit);
											builder.Append(")");
										}
										if ((waiting.Choice != null) && (waiting.Choice != ""))
										{
											builder.Append("\n");
											if (waiting.Choice.Length > 12)
											{
												builder.Append(waiting.Choice.Substring(0, 12));
												builder.Append("...");
											}
											else
											{
												builder.Append(waiting.Choice);
											}
										}
										item.Text = builder.ToString();
										item.ObjectValue = orderWaiting[waitingLeft].Items[waitingTop].BillDetailID;
									}
									waitingTop++;
								}
								waitingLeft++;
							}
						}
					}
				}
				catch (Exception exception)
				{
					MessageBox.Show(exception.ToString());
				}
				finally
				{
					this.BillItemPad.AutoRefresh = true;
				}
			}
		}
	}
}
