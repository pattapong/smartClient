using smartRestaurant.Controls;
using smartRestaurant.Data;
using smartRestaurant.MenuService;
using smartRestaurant.OrderService;
using smartRestaurant.PaymentService;
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
	public class PrintReceiptForm : SmartForm
	{
		private const int INPUT_NONE = 0;

		private const int INPUT_PAYMENT = 1;

		private const int INPUT_PAYVALUE = 2;

		private const int INPUT_POINTAMOUNT = 3;

		private const int INPUT_COUPON = 4;

		private const int INPUT_GIFT = 5;

		private int employeeID;

		private int inputState;

		private string inputValue;

		private int guestNumber;

		private int billNumber;

		private TableInformation tableInfo;

		private MenuOption[] menuOptions;

		private MenuType[] menuTypes;

		private OrderInformation orderInfo;

		private smartRestaurant.OrderService.OrderBill selectedBill;

		private OrderBillItem selectedItem;

		private PaymentMethod[] paymentMethods;

		private Receipt receipt;

		private ArrayList discountSelected;

		private ArrayList paymentSelected;

		private GroupPanel OrderPanel;

		private Label FieldBill;

		private Label LblBill;

		private Label FieldGuest;

		private Label LblGuest;

		private Label FieldTable;

		private Label LblTable;

		private ImageButton BtnDown;

		private ImageButton BtnUp;

		private ImageButton BtnUndo;

		private ImageButton BtnCancel;

		private ImageList NumberImgList;

		private ImageList ButtonImgList;

		private Label FieldCurrentInput;

		private Label FieldInputType;

		private NumberPad NumberKeyPad;

		private GroupPanel groupPanel2;

		private ImageButton BtnPay;

		private ImageButton BtnPrintReceipt;

		private Label LblAmountDue;

		private Label LblTax1;

		private Label LblTax2;

		private Label LblTotalDiscount;

		private Label LblTotalDue;

		private Label LblTotalReceive;

		private Label LblTotalChange;

		private Label FieldAmountDue;

		private Label FieldTax1;

		private Label FieldTotalDiscount;

		private Label FieldTax2;

		private Label FieldTotalReceive;

		private Label FieldTotalDue;

		private Label FieldChange;

		private GroupPanel groupPanel3;

		private ImageButton BtnBack;

		private IContainer components;

		private Label LblDiscount;

		private Label LblPayment;

		private ImageList ButtonLiteImgList;

		private ItemsList ListOrderItem;

		private ButtonListPad DiscountPad;

		private Label LblPageID;

		private ItemsList ListOrderItemPrice;

		private Label LblCopyright;

		private ButtonListPad PaymentTypePad;

		private ImageButton BtnFillPay;

		private ItemsList ListOrderCount;

		private ImageButton BtnPayClear;

		private ImageList CalculatorImgList;

		private ImageButton BtnCalculator;

		private ImageButton BtnKBInvoiceNote;

		private TextBox FieldInvoiceNote;

		private ImageButton BtnPayClearAll;

		public int EmployeeID
		{
			set
			{
				this.employeeID = value;
			}
		}

		public OrderInformation Order
		{
			set
			{
				this.orderInfo = value;
				this.guestNumber = this.orderInfo.NumberOfGuest;
				this.billNumber = (int)this.orderInfo.Bills.Length;
			}
		}

		public smartRestaurant.OrderService.OrderBill OrderBill
		{
			set
			{
				this.selectedBill = value;
			}
		}

		public TableInformation Table
		{
			set
			{
				this.tableInfo = value;
			}
		}

		public PrintReceiptForm()
		{
			this.InitializeComponent();
			this.LblTax1.Text = AppParameter.Tax1;
			this.LblTax2.Text = AppParameter.Tax2;
			this.paymentMethods = Receipt.PaymentMethods;
			this.discountSelected = new ArrayList();
			this.paymentSelected = new ArrayList();
		}

		private void BtnBack_Click(object sender, EventArgs e)
		{
			if (this.orderInfo.TableID == 0)
			{
				((MainForm)base.MdiParent).ShowMainMenuForm();
				return;
			}
			((MainForm)base.MdiParent).ShowTakeOrderForm(null);
		}

		private void BtnCalculator_Click(object sender, EventArgs e)
		{
			CalculatorForm.Show(true);
		}

		private void BtnCancel_Click(object sender, EventArgs e)
		{
			if (this.selectedItem != null)
			{
				if (!OrderManagement.CancelOrderBillItem(this.selectedBill, this.selectedItem, this.employeeID))
				{
					return;
				}
				this.UpdateOrderGrid();
				string str = (new smartRestaurant.OrderService.OrderService()).SendOrderBill(this.selectedBill);
				if (str != null)
				{
					MessageBox.Show(this, str);
					return;
				}
				this.UpdateSummary();
			}
		}

		private void BtnDown_Click(object sender, EventArgs e)
		{
			this.ListOrderItem.Down(5);
			this.UpdateOrderButton();
		}

		private void BtnFillPay_Click(object sender, EventArgs e)
		{
			double item;
			try
			{
				int num = this.receipt.PaymentMethodList.IndexOf(this.receipt.PaymentMethod);
				item = (double)this.receipt.PayValueList[num];
			}
			catch (Exception exception)
			{
				item = 0;
			}
			this.receipt.PayValue = this.receipt.TotalDue - this.receipt.TotalReceive + item;
			this.receipt.SetPaymentMethod(this.receipt.PaymentMethod, this.receipt.PayValue);
			this.receipt.PaymentMethod = null;
			this.StartInputNone();
			this.UpdatePaymentTypeList();
			this.UpdateSummary();
		}

		private void BtnKBInvoiceNote_Click(object sender, EventArgs e)
		{
			this.FieldInvoiceNote.Focus();
			string str = KeyboardForm.Show("Invoice Note", this.FieldInvoiceNote.Text);
			if (str != null)
			{
				this.FieldInvoiceNote.Text = str;
			}
		}

		private void BtnPay_Click(object sender, EventArgs e)
		{
			smartRestaurant.OrderService.OrderService orderService = new smartRestaurant.OrderService.OrderService();
			WaitingForm.Show("Print Bill");
			base.Enabled = false;
			string str = orderService.SendOrderBill(this.selectedBill);
			if (str != null)
			{
				base.Enabled = true;
				WaitingForm.HideForm();
				MessageBox.Show(this, str);
				return;
			}
			bool flag = this.receipt.SendInvoice(true, true);
			base.Enabled = true;
			WaitingForm.HideForm();
			if (flag)
			{
				((MainForm)base.MdiParent).ShowTakeOrderForm(null);
				return;
			}
			if (this.orderInfo.TableID != 0)
			{
				(new smartRestaurant.TableService.TableService()).UpdateTableLockInuse(this.orderInfo.TableID, false);
			}
			((MainForm)base.MdiParent).ShowMainMenuForm();
		}

		private void BtnPayClear_Click(object sender, EventArgs e)
		{
			this.receipt.PayValue = 0;
			this.receipt.SetPaymentMethod(this.receipt.PaymentMethod, this.receipt.PayValue);
			this.receipt.PaymentMethod = null;
			this.StartInputNone();
			this.UpdatePaymentTypeList();
			this.UpdateSummary();
		}

		private void BtnPayClearAll_Click(object sender, EventArgs e)
		{
			this.receipt.SetPaymentMethod(null, 0);
			this.receipt.PaymentMethod = null;
			this.StartInputNone();
			this.UpdatePaymentTypeList();
			this.UpdateSummary();
		}

		private void BtnPrintReceipt_Click(object sender, EventArgs e)
		{
			smartRestaurant.OrderService.OrderService orderService = new smartRestaurant.OrderService.OrderService();
			WaitingForm.Show("Print Receipt");
			base.Enabled = false;
			string str = orderService.SendOrderBill(this.selectedBill);
			if (str != null)
			{
				base.Enabled = true;
				WaitingForm.HideForm();
				MessageBox.Show(this, str);
				return;
			}
			bool flag = this.receipt.SendInvoice(false, true);
			base.Enabled = true;
			WaitingForm.HideForm();
			if (!flag || this.orderInfo.TableID == 0)
			{
				((MainForm)base.MdiParent).ShowMainMenuForm();
				return;
			}
			((MainForm)base.MdiParent).ShowTakeOrderForm(null);
		}

		private void BtnUndo_Click(object sender, EventArgs e)
		{
			if (this.selectedItem != null)
			{
				OrderManagement.UndoCancelOrderBillItem(this.selectedItem, this.employeeID);
				this.UpdateOrderGrid();
				string str = (new smartRestaurant.OrderService.OrderService()).SendOrderBill(this.selectedBill);
				if (str != null)
				{
					MessageBox.Show(this, str);
					return;
				}
				this.UpdateSummary();
			}
		}

		private void BtnUp_Click(object sender, EventArgs e)
		{
			this.ListOrderItem.Up(5);
			this.UpdateOrderButton();
		}

		private void DiscountPad_PadClick(object sender, ButtonListPadEventArgs e)
		{
			if (e.Value == null)
			{
				return;
			}
			Discount discount = Receipt.SearchDiscountByID(int.Parse(e.Value));
			if (!this.discountSelected.Contains(e.Index))
			{
				this.discountSelected.Add(e.Index);
				this.receipt.UseDiscountAdd(discount);
			}
			else
			{
				this.discountSelected.Remove(e.Index);
				this.receipt.UseDiscountRemove(discount);
			}
			this.UpdateDiscountList();
			this.UpdateSummary();
		}

		private void DiscountPad_PageChange(object sender, ButtonListPadEventArgs e)
		{
			this.UpdateDiscountList();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void FieldInvoiceNote_Enter(object sender, EventArgs e)
		{
			((TextBox)sender).BackColor = Color.FromArgb(255, 255, 192);
		}

		private void FieldInvoiceNote_Leave(object sender, EventArgs e)
		{
			((TextBox)sender).BackColor = Color.White;
		}

		private void FieldInvoiceNote_TextChanged(object sender, EventArgs e)
		{
			this.receipt.InvoiceNote = this.FieldInvoiceNote.Text;
		}

		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			ResourceManager resourceManager = new ResourceManager(typeof(PrintReceiptForm));
			this.OrderPanel = new GroupPanel();
			this.FieldBill = new Label();
			this.LblBill = new Label();
			this.FieldGuest = new Label();
			this.LblGuest = new Label();
			this.FieldTable = new Label();
			this.LblTable = new Label();
			this.BtnDown = new ImageButton();
			this.ButtonImgList = new ImageList(this.components);
			this.BtnUp = new ImageButton();
			this.BtnUndo = new ImageButton();
			this.BtnCancel = new ImageButton();
			this.NumberImgList = new ImageList(this.components);
			this.FieldCurrentInput = new Label();
			this.FieldInputType = new Label();
			this.NumberKeyPad = new NumberPad();
			this.groupPanel2 = new GroupPanel();
			this.FieldChange = new Label();
			this.FieldTotalReceive = new Label();
			this.FieldTotalDue = new Label();
			this.FieldTotalDiscount = new Label();
			this.FieldTax2 = new Label();
			this.FieldTax1 = new Label();
			this.FieldAmountDue = new Label();
			this.LblTotalChange = new Label();
			this.LblTotalReceive = new Label();
			this.LblTotalDue = new Label();
			this.LblTotalDiscount = new Label();
			this.LblTax2 = new Label();
			this.LblTax1 = new Label();
			this.LblAmountDue = new Label();
			this.BtnCalculator = new ImageButton();
			this.CalculatorImgList = new ImageList(this.components);
			this.BtnPay = new ImageButton();
			this.BtnPrintReceipt = new ImageButton();
			this.groupPanel3 = new GroupPanel();
			this.BtnPayClearAll = new ImageButton();
			this.ButtonLiteImgList = new ImageList(this.components);
			this.BtnPayClear = new ImageButton();
			this.BtnFillPay = new ImageButton();
			this.PaymentTypePad = new ButtonListPad();
			this.LblPayment = new Label();
			this.LblDiscount = new Label();
			this.DiscountPad = new ButtonListPad();
			this.BtnBack = new ImageButton();
			this.ListOrderItem = new ItemsList();
			this.ListOrderCount = new ItemsList();
			this.ListOrderItemPrice = new ItemsList();
			this.LblPageID = new Label();
			this.LblCopyright = new Label();
			this.BtnKBInvoiceNote = new ImageButton();
			this.FieldInvoiceNote = new TextBox();
			this.OrderPanel.SuspendLayout();
			this.groupPanel2.SuspendLayout();
			this.groupPanel3.SuspendLayout();
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
			this.OrderPanel.ShowHeader = false;
			this.OrderPanel.Size = new System.Drawing.Size(449, 58);
			this.OrderPanel.TabIndex = 2;
			this.FieldBill.BackColor = Color.White;
			this.FieldBill.Location = new Point(384, 1);
			this.FieldBill.Name = "FieldBill";
			this.FieldBill.Size = new System.Drawing.Size(64, 56);
			this.FieldBill.TabIndex = 11;
			this.FieldBill.Text = "1";
			this.FieldBill.TextAlign = ContentAlignment.MiddleCenter;
			this.LblBill.BackColor = Color.Orange;
			this.LblBill.Location = new Point(312, 1);
			this.LblBill.Name = "LblBill";
			this.LblBill.Size = new System.Drawing.Size(72, 56);
			this.LblBill.TabIndex = 10;
			this.LblBill.Text = "Bill:";
			this.LblBill.TextAlign = ContentAlignment.MiddleCenter;
			this.FieldGuest.BackColor = Color.White;
			this.FieldGuest.Location = new Point(248, 1);
			this.FieldGuest.Name = "FieldGuest";
			this.FieldGuest.Size = new System.Drawing.Size(64, 56);
			this.FieldGuest.TabIndex = 9;
			this.FieldGuest.Text = "1";
			this.FieldGuest.TextAlign = ContentAlignment.MiddleCenter;
			this.LblGuest.BackColor = Color.Orange;
			this.LblGuest.Location = new Point(176, 1);
			this.LblGuest.Name = "LblGuest";
			this.LblGuest.Size = new System.Drawing.Size(72, 56);
			this.LblGuest.TabIndex = 8;
			this.LblGuest.Text = "Seat:";
			this.LblGuest.TextAlign = ContentAlignment.MiddleCenter;
			this.FieldTable.BackColor = Color.White;
			this.FieldTable.Cursor = Cursors.Default;
			this.FieldTable.Location = new Point(73, 1);
			this.FieldTable.Name = "FieldTable";
			this.FieldTable.Size = new System.Drawing.Size(103, 56);
			this.FieldTable.TabIndex = 7;
			this.FieldTable.Text = "1";
			this.FieldTable.TextAlign = ContentAlignment.MiddleCenter;
			this.LblTable.BackColor = Color.Orange;
			this.LblTable.Location = new Point(1, 1);
			this.LblTable.Name = "LblTable";
			this.LblTable.Size = new System.Drawing.Size(72, 56);
			this.LblTable.TabIndex = 6;
			this.LblTable.Text = "Table:";
			this.LblTable.TextAlign = ContentAlignment.MiddleCenter;
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
			this.BtnDown.TabIndex = 24;
			this.BtnDown.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnDown.Click += new EventHandler(this.BtnDown_Click);
			this.ButtonImgList.ColorDepth = ColorDepth.Depth32Bit;
			this.ButtonImgList.ImageSize = new System.Drawing.Size(110, 60);
			this.ButtonImgList.ImageStream = (ImageListStreamer)resourceManager.GetObject("ButtonImgList.ImageStream");
			this.ButtonImgList.TransparentColor = Color.Transparent;
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
			this.BtnUp.TabIndex = 23;
			this.BtnUp.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnUp.Click += new EventHandler(this.BtnUp_Click);
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
			this.BtnUndo.TabIndex = 21;
			this.BtnUndo.Text = "Undo";
			this.BtnUndo.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnUndo.Click += new EventHandler(this.BtnUndo_Click);
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
			this.BtnCancel.TabIndex = 20;
			this.BtnCancel.Text = "Cancel";
			this.BtnCancel.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnCancel.Click += new EventHandler(this.BtnCancel_Click);
			this.NumberImgList.ColorDepth = ColorDepth.Depth32Bit;
			this.NumberImgList.ImageSize = new System.Drawing.Size(72, 60);
			this.NumberImgList.ImageStream = (ImageListStreamer)resourceManager.GetObject("NumberImgList.ImageStream");
			this.NumberImgList.TransparentColor = Color.Transparent;
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
			this.FieldInputType.Text = "none";
			this.FieldInputType.TextAlign = ContentAlignment.MiddleCenter;
			this.NumberKeyPad.BackColor = Color.White;
			this.NumberKeyPad.Font = new System.Drawing.Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.NumberKeyPad.Image = (Image)resourceManager.GetObject("NumberKeyPad.Image");
			this.NumberKeyPad.ImageClick = (Image)resourceManager.GetObject("NumberKeyPad.ImageClick");
			this.NumberKeyPad.ImageClickIndex = 1;
			this.NumberKeyPad.ImageIndex = 0;
			this.NumberKeyPad.ImageList = this.NumberImgList;
			this.NumberKeyPad.Location = new Point(56, 360);
			this.NumberKeyPad.Name = "NumberKeyPad";
			this.NumberKeyPad.Size = new System.Drawing.Size(226, 255);
			this.NumberKeyPad.TabIndex = 7;
			this.NumberKeyPad.Text = "numberPad1";
			this.NumberKeyPad.PadClick += new NumberPadEventHandler(this.NumberKeyPad_PadClick);
			this.groupPanel2.BackColor = Color.White;
			this.groupPanel2.Caption = null;
			this.groupPanel2.Controls.Add(this.FieldChange);
			this.groupPanel2.Controls.Add(this.FieldTotalReceive);
			this.groupPanel2.Controls.Add(this.FieldTotalDue);
			this.groupPanel2.Controls.Add(this.FieldTotalDiscount);
			this.groupPanel2.Controls.Add(this.FieldTax2);
			this.groupPanel2.Controls.Add(this.FieldTax1);
			this.groupPanel2.Controls.Add(this.FieldAmountDue);
			this.groupPanel2.Controls.Add(this.LblTotalChange);
			this.groupPanel2.Controls.Add(this.LblTotalReceive);
			this.groupPanel2.Controls.Add(this.LblTotalDue);
			this.groupPanel2.Controls.Add(this.LblTotalDiscount);
			this.groupPanel2.Controls.Add(this.LblTax2);
			this.groupPanel2.Controls.Add(this.LblTax1);
			this.groupPanel2.Controls.Add(this.LblAmountDue);
			this.groupPanel2.Controls.Add(this.FieldCurrentInput);
			this.groupPanel2.Controls.Add(this.FieldInputType);
			this.groupPanel2.Controls.Add(this.NumberKeyPad);
			this.groupPanel2.Font = new System.Drawing.Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.groupPanel2.Location = new Point(328, 128);
			this.groupPanel2.Name = "groupPanel2";
			this.groupPanel2.ShowHeader = false;
			this.groupPanel2.Size = new System.Drawing.Size(344, 624);
			this.groupPanel2.TabIndex = 25;
			this.FieldChange.Location = new Point(168, 256);
			this.FieldChange.Name = "FieldChange";
			this.FieldChange.Size = new System.Drawing.Size(160, 40);
			this.FieldChange.TabIndex = 23;
			this.FieldChange.Text = "0.00";
			this.FieldChange.TextAlign = ContentAlignment.MiddleRight;
			this.FieldTotalReceive.Location = new Point(168, 216);
			this.FieldTotalReceive.Name = "FieldTotalReceive";
			this.FieldTotalReceive.Size = new System.Drawing.Size(160, 40);
			this.FieldTotalReceive.TabIndex = 22;
			this.FieldTotalReceive.Text = "0.00";
			this.FieldTotalReceive.TextAlign = ContentAlignment.MiddleRight;
			this.FieldTotalDue.Font = new System.Drawing.Font("Tahoma", 15.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.FieldTotalDue.ForeColor = Color.Green;
			this.FieldTotalDue.Location = new Point(168, 176);
			this.FieldTotalDue.Name = "FieldTotalDue";
			this.FieldTotalDue.Size = new System.Drawing.Size(160, 40);
			this.FieldTotalDue.TabIndex = 21;
			this.FieldTotalDue.Text = "0.00";
			this.FieldTotalDue.TextAlign = ContentAlignment.MiddleRight;
			this.FieldTotalDiscount.Location = new Point(168, 56);
			this.FieldTotalDiscount.Name = "FieldTotalDiscount";
			this.FieldTotalDiscount.Size = new System.Drawing.Size(160, 40);
			this.FieldTotalDiscount.TabIndex = 20;
			this.FieldTotalDiscount.Text = "0.00";
			this.FieldTotalDiscount.TextAlign = ContentAlignment.MiddleRight;
			this.FieldTax2.Location = new Point(168, 136);
			this.FieldTax2.Name = "FieldTax2";
			this.FieldTax2.Size = new System.Drawing.Size(160, 40);
			this.FieldTax2.TabIndex = 19;
			this.FieldTax2.Text = "0.00";
			this.FieldTax2.TextAlign = ContentAlignment.MiddleRight;
			this.FieldTax1.Location = new Point(168, 96);
			this.FieldTax1.Name = "FieldTax1";
			this.FieldTax1.Size = new System.Drawing.Size(160, 40);
			this.FieldTax1.TabIndex = 18;
			this.FieldTax1.Text = "0.00";
			this.FieldTax1.TextAlign = ContentAlignment.MiddleRight;
			this.FieldAmountDue.Location = new Point(168, 16);
			this.FieldAmountDue.Name = "FieldAmountDue";
			this.FieldAmountDue.Size = new System.Drawing.Size(160, 40);
			this.FieldAmountDue.TabIndex = 17;
			this.FieldAmountDue.Text = "0.00";
			this.FieldAmountDue.TextAlign = ContentAlignment.MiddleRight;
			this.LblTotalChange.Location = new Point(16, 256);
			this.LblTotalChange.Name = "LblTotalChange";
			this.LblTotalChange.Size = new System.Drawing.Size(144, 40);
			this.LblTotalChange.TabIndex = 16;
			this.LblTotalChange.Text = "Change";
			this.LblTotalChange.TextAlign = ContentAlignment.MiddleLeft;
			this.LblTotalReceive.Location = new Point(16, 216);
			this.LblTotalReceive.Name = "LblTotalReceive";
			this.LblTotalReceive.Size = new System.Drawing.Size(144, 40);
			this.LblTotalReceive.TabIndex = 15;
			this.LblTotalReceive.Text = "Total Receive";
			this.LblTotalReceive.TextAlign = ContentAlignment.MiddleLeft;
			this.LblTotalDue.Font = new System.Drawing.Font("Tahoma", 15.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.LblTotalDue.ForeColor = Color.Green;
			this.LblTotalDue.Location = new Point(16, 176);
			this.LblTotalDue.Name = "LblTotalDue";
			this.LblTotalDue.Size = new System.Drawing.Size(144, 40);
			this.LblTotalDue.TabIndex = 14;
			this.LblTotalDue.Text = "Total Due";
			this.LblTotalDue.TextAlign = ContentAlignment.MiddleLeft;
			this.LblTotalDiscount.Location = new Point(16, 56);
			this.LblTotalDiscount.Name = "LblTotalDiscount";
			this.LblTotalDiscount.Size = new System.Drawing.Size(144, 40);
			this.LblTotalDiscount.TabIndex = 13;
			this.LblTotalDiscount.Text = "Total Discount";
			this.LblTotalDiscount.TextAlign = ContentAlignment.MiddleLeft;
			this.LblTax2.Location = new Point(16, 136);
			this.LblTax2.Name = "LblTax2";
			this.LblTax2.Size = new System.Drawing.Size(144, 40);
			this.LblTax2.TabIndex = 12;
			this.LblTax2.Text = "Tax2";
			this.LblTax2.TextAlign = ContentAlignment.MiddleLeft;
			this.LblTax1.Location = new Point(16, 96);
			this.LblTax1.Name = "LblTax1";
			this.LblTax1.Size = new System.Drawing.Size(144, 40);
			this.LblTax1.TabIndex = 11;
			this.LblTax1.Text = "Tax1";
			this.LblTax1.TextAlign = ContentAlignment.MiddleLeft;
			this.LblAmountDue.Location = new Point(16, 16);
			this.LblAmountDue.Name = "LblAmountDue";
			this.LblAmountDue.Size = new System.Drawing.Size(144, 40);
			this.LblAmountDue.TabIndex = 10;
			this.LblAmountDue.Text = "Amount Due";
			this.LblAmountDue.TextAlign = ContentAlignment.MiddleLeft;
			this.BtnCalculator.BackColor = Color.Transparent;
			this.BtnCalculator.Blue = 1f;
			this.BtnCalculator.Cursor = Cursors.Hand;
			this.BtnCalculator.Green = 1f;
			this.BtnCalculator.ImageClick = (Image)resourceManager.GetObject("BtnCalculator.ImageClick");
			this.BtnCalculator.ImageClickIndex = 1;
			this.BtnCalculator.ImageIndex = 0;
			this.BtnCalculator.ImageList = this.CalculatorImgList;
			this.BtnCalculator.IsLock = false;
			this.BtnCalculator.Location = new Point(744, 64);
			this.BtnCalculator.Name = "BtnCalculator";
			this.BtnCalculator.ObjectValue = null;
			this.BtnCalculator.Red = 1f;
			this.BtnCalculator.Size = new System.Drawing.Size(40, 40);
			this.BtnCalculator.TabIndex = 40;
			this.BtnCalculator.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnCalculator.Click += new EventHandler(this.BtnCalculator_Click);
			this.CalculatorImgList.ColorDepth = ColorDepth.Depth32Bit;
			this.CalculatorImgList.ImageSize = new System.Drawing.Size(40, 40);
			this.CalculatorImgList.ImageStream = (ImageListStreamer)resourceManager.GetObject("CalculatorImgList.ImageStream");
			this.CalculatorImgList.TransparentColor = Color.Transparent;
			this.BtnPay.BackColor = Color.Transparent;
			this.BtnPay.Blue = 1f;
			this.BtnPay.Cursor = Cursors.Hand;
			this.BtnPay.Green = 1f;
			this.BtnPay.ImageClick = (Image)resourceManager.GetObject("BtnPay.ImageClick");
			this.BtnPay.ImageClickIndex = 1;
			this.BtnPay.ImageIndex = 0;
			this.BtnPay.ImageList = this.ButtonImgList;
			this.BtnPay.IsLock = false;
			this.BtnPay.Location = new Point(904, 64);
			this.BtnPay.Name = "BtnPay";
			this.BtnPay.ObjectValue = null;
			this.BtnPay.Red = 1.75f;
			this.BtnPay.Size = new System.Drawing.Size(110, 60);
			this.BtnPay.TabIndex = 28;
			this.BtnPay.Text = "Close-Bill";
			this.BtnPay.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnPay.Click += new EventHandler(this.BtnPay_Click);
			this.BtnPrintReceipt.BackColor = Color.Transparent;
			this.BtnPrintReceipt.Blue = 1.75f;
			this.BtnPrintReceipt.Cursor = Cursors.Hand;
			this.BtnPrintReceipt.Green = 1f;
			this.BtnPrintReceipt.ImageClick = (Image)resourceManager.GetObject("BtnPrintReceipt.ImageClick");
			this.BtnPrintReceipt.ImageClickIndex = 1;
			this.BtnPrintReceipt.ImageIndex = 0;
			this.BtnPrintReceipt.ImageList = this.ButtonImgList;
			this.BtnPrintReceipt.IsLock = false;
			this.BtnPrintReceipt.Location = new Point(792, 64);
			this.BtnPrintReceipt.Name = "BtnPrintReceipt";
			this.BtnPrintReceipt.ObjectValue = null;
			this.BtnPrintReceipt.Red = 1.75f;
			this.BtnPrintReceipt.Size = new System.Drawing.Size(110, 60);
			this.BtnPrintReceipt.TabIndex = 27;
			this.BtnPrintReceipt.Text = "Print-Receipt";
			this.BtnPrintReceipt.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnPrintReceipt.Click += new EventHandler(this.BtnPrintReceipt_Click);
			this.groupPanel3.BackColor = Color.Transparent;
			this.groupPanel3.Caption = null;
			this.groupPanel3.Controls.Add(this.BtnKBInvoiceNote);
			this.groupPanel3.Controls.Add(this.FieldInvoiceNote);
			this.groupPanel3.Controls.Add(this.BtnPayClearAll);
			this.groupPanel3.Controls.Add(this.BtnPayClear);
			this.groupPanel3.Controls.Add(this.BtnFillPay);
			this.groupPanel3.Controls.Add(this.PaymentTypePad);
			this.groupPanel3.Controls.Add(this.LblPayment);
			this.groupPanel3.Controls.Add(this.LblDiscount);
			this.groupPanel3.Controls.Add(this.DiscountPad);
			this.groupPanel3.Font = new System.Drawing.Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.groupPanel3.Location = new Point(672, 128);
			this.groupPanel3.Name = "groupPanel3";
			this.groupPanel3.ShowHeader = false;
			this.groupPanel3.Size = new System.Drawing.Size(344, 624);
			this.groupPanel3.TabIndex = 29;
			this.BtnPayClearAll.BackColor = Color.Transparent;
			this.BtnPayClearAll.Blue = 2f;
			this.BtnPayClearAll.Cursor = Cursors.Hand;
			this.BtnPayClearAll.Font = new System.Drawing.Font("Tahoma", 12f, FontStyle.Regular, GraphicsUnit.Pixel);
			this.BtnPayClearAll.Green = 1f;
			this.BtnPayClearAll.ImageClick = (Image)resourceManager.GetObject("BtnPayClearAll.ImageClick");
			this.BtnPayClearAll.ImageClickIndex = 1;
			this.BtnPayClearAll.ImageIndex = 0;
			this.BtnPayClearAll.ImageList = this.ButtonLiteImgList;
			this.BtnPayClearAll.IsLock = false;
			this.BtnPayClearAll.Location = new Point(8, 576);
			this.BtnPayClearAll.Name = "BtnPayClearAll";
			this.BtnPayClearAll.ObjectValue = null;
			this.BtnPayClearAll.Red = 1f;
			this.BtnPayClearAll.Size = new System.Drawing.Size(110, 40);
			this.BtnPayClearAll.TabIndex = 44;
			this.BtnPayClearAll.Text = "Clear All";
			this.BtnPayClearAll.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnPayClearAll.Click += new EventHandler(this.BtnPayClearAll_Click);
			this.ButtonLiteImgList.ColorDepth = ColorDepth.Depth32Bit;
			this.ButtonLiteImgList.ImageSize = new System.Drawing.Size(110, 40);
			this.ButtonLiteImgList.ImageStream = (ImageListStreamer)resourceManager.GetObject("ButtonLiteImgList.ImageStream");
			this.ButtonLiteImgList.TransparentColor = Color.Transparent;
			this.BtnPayClear.BackColor = Color.Transparent;
			this.BtnPayClear.Blue = 2f;
			this.BtnPayClear.Cursor = Cursors.Hand;
			this.BtnPayClear.Font = new System.Drawing.Font("Tahoma", 12f, FontStyle.Regular, GraphicsUnit.Pixel);
			this.BtnPayClear.Green = 1f;
			this.BtnPayClear.ImageClick = (Image)resourceManager.GetObject("BtnPayClear.ImageClick");
			this.BtnPayClear.ImageClickIndex = 1;
			this.BtnPayClear.ImageIndex = 0;
			this.BtnPayClear.ImageList = this.ButtonLiteImgList;
			this.BtnPayClear.IsLock = false;
			this.BtnPayClear.Location = new Point(119, 576);
			this.BtnPayClear.Name = "BtnPayClear";
			this.BtnPayClear.ObjectValue = null;
			this.BtnPayClear.Red = 1f;
			this.BtnPayClear.Size = new System.Drawing.Size(110, 40);
			this.BtnPayClear.TabIndex = 43;
			this.BtnPayClear.Text = "Clear";
			this.BtnPayClear.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnPayClear.Click += new EventHandler(this.BtnPayClear_Click);
			this.BtnFillPay.BackColor = Color.Transparent;
			this.BtnFillPay.Blue = 2f;
			this.BtnFillPay.Cursor = Cursors.Hand;
			this.BtnFillPay.Font = new System.Drawing.Font("Tahoma", 12f, FontStyle.Regular, GraphicsUnit.Pixel);
			this.BtnFillPay.Green = 1f;
			this.BtnFillPay.ImageClick = (Image)resourceManager.GetObject("BtnFillPay.ImageClick");
			this.BtnFillPay.ImageClickIndex = 1;
			this.BtnFillPay.ImageIndex = 0;
			this.BtnFillPay.ImageList = this.ButtonLiteImgList;
			this.BtnFillPay.IsLock = false;
			this.BtnFillPay.Location = new Point(230, 576);
			this.BtnFillPay.Name = "BtnFillPay";
			this.BtnFillPay.ObjectValue = null;
			this.BtnFillPay.Red = 1f;
			this.BtnFillPay.Size = new System.Drawing.Size(110, 40);
			this.BtnFillPay.TabIndex = 42;
			this.BtnFillPay.Text = "Fill Pay";
			this.BtnFillPay.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnFillPay.Click += new EventHandler(this.BtnFillPay_Click);
			this.PaymentTypePad.AutoRefresh = true;
			this.PaymentTypePad.BackColor = Color.White;
			this.PaymentTypePad.Blue = 1f;
			this.PaymentTypePad.Column = 3;
			this.PaymentTypePad.Font = new System.Drawing.Font("Tahoma", 12f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.PaymentTypePad.Green = 1f;
			this.PaymentTypePad.Image = (Image)resourceManager.GetObject("PaymentTypePad.Image");
			this.PaymentTypePad.ImageClick = (Image)resourceManager.GetObject("PaymentTypePad.ImageClick");
			this.PaymentTypePad.ImageClickIndex = 1;
			this.PaymentTypePad.ImageIndex = 0;
			this.PaymentTypePad.ImageList = this.ButtonImgList;
			this.PaymentTypePad.ItemStart = 0;
			this.PaymentTypePad.Location = new Point(8, 388);
			this.PaymentTypePad.Name = "PaymentTypePad";
			this.PaymentTypePad.Padding = 1;
			this.PaymentTypePad.Red = 1f;
			this.PaymentTypePad.Row = 3;
			this.PaymentTypePad.Size = new System.Drawing.Size(332, 182);
			this.PaymentTypePad.TabIndex = 41;
			this.PaymentTypePad.PadClick += new ButtonListPadEventHandler(this.PaymentTypePad_PadClick);
			this.PaymentTypePad.PageChange += new ButtonListPadEventHandler(this.PaymentTypePad_PageChange);
			this.LblPayment.BackColor = Color.Black;
			this.LblPayment.ForeColor = Color.White;
			this.LblPayment.Location = new Point(0, 296);
			this.LblPayment.Name = "LblPayment";
			this.LblPayment.Size = new System.Drawing.Size(344, 40);
			this.LblPayment.TabIndex = 1;
			this.LblPayment.Text = "Payment Receive";
			this.LblPayment.TextAlign = ContentAlignment.MiddleLeft;
			this.LblDiscount.BackColor = Color.Black;
			this.LblDiscount.ForeColor = Color.White;
			this.LblDiscount.Location = new Point(0, 0);
			this.LblDiscount.Name = "LblDiscount";
			this.LblDiscount.Size = new System.Drawing.Size(344, 40);
			this.LblDiscount.TabIndex = 0;
			this.LblDiscount.Text = "Discount";
			this.LblDiscount.TextAlign = ContentAlignment.MiddleLeft;
			this.DiscountPad.AutoRefresh = true;
			this.DiscountPad.BackColor = Color.White;
			this.DiscountPad.Blue = 1f;
			this.DiscountPad.Column = 3;
			this.DiscountPad.Font = new System.Drawing.Font("Tahoma", 12f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.DiscountPad.Green = 1f;
			this.DiscountPad.Image = (Image)resourceManager.GetObject("DiscountPad.Image");
			this.DiscountPad.ImageClick = (Image)resourceManager.GetObject("DiscountPad.ImageClick");
			this.DiscountPad.ImageClickIndex = 1;
			this.DiscountPad.ImageIndex = 0;
			this.DiscountPad.ImageList = this.ButtonImgList;
			this.DiscountPad.ItemStart = 0;
			this.DiscountPad.Location = new Point(6, 48);
			this.DiscountPad.Name = "DiscountPad";
			this.DiscountPad.Padding = 1;
			this.DiscountPad.Red = 1f;
			this.DiscountPad.Row = 4;
			this.DiscountPad.Size = new System.Drawing.Size(332, 243);
			this.DiscountPad.TabIndex = 40;
			this.DiscountPad.PadClick += new ButtonListPadEventHandler(this.DiscountPad_PadClick);
			this.DiscountPad.PageChange += new ButtonListPadEventHandler(this.DiscountPad_PageChange);
			this.BtnBack.BackColor = Color.Transparent;
			this.BtnBack.Blue = 2f;
			this.BtnBack.Cursor = Cursors.Hand;
			this.BtnBack.Green = 2f;
			this.BtnBack.ImageClick = (Image)resourceManager.GetObject("BtnBack.ImageClick");
			this.BtnBack.ImageClickIndex = 1;
			this.BtnBack.ImageIndex = 0;
			this.BtnBack.ImageList = this.ButtonImgList;
			this.BtnBack.IsLock = false;
			this.BtnBack.Location = new Point(424, 64);
			this.BtnBack.Name = "BtnBack";
			this.BtnBack.ObjectValue = null;
			this.BtnBack.Red = 1f;
			this.BtnBack.Size = new System.Drawing.Size(110, 60);
			this.BtnBack.TabIndex = 30;
			this.BtnBack.Text = "Back";
			this.BtnBack.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnBack.Click += new EventHandler(this.BtnBack_Click);
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
			this.ListOrderItem.ItemWidth = 200;
			this.ListOrderItem.Location = new Point(8, 128);
			this.ListOrderItem.Name = "ListOrderItem";
			this.ListOrderItem.Row = 14;
			this.ListOrderItem.SelectedIndex = 0;
			this.ListOrderItem.Size = new System.Drawing.Size(200, 560);
			this.ListOrderItem.TabIndex = 31;
			this.ListOrderItem.ItemClick += new ItemsListEventHandler(this.ListOrderItem_ItemClick);
			this.ListOrderCount.Alignment = ContentAlignment.MiddleCenter;
			this.ListOrderCount.AutoRefresh = true;
			this.ListOrderCount.BackAlterColor = Color.FromArgb(192, 255, 255);
			this.ListOrderCount.BackHeaderColor = Color.FromArgb(255, 192, 128);
			this.ListOrderCount.BackHeaderSelectedColor = Color.FromArgb(192, 0, 0);
			this.ListOrderCount.BackNormalColor = Color.White;
			this.ListOrderCount.BackSelectedColor = Color.Blue;
			this.ListOrderCount.BindList1 = this.ListOrderItem;
			this.ListOrderCount.BindList2 = this.ListOrderItemPrice;
			this.ListOrderCount.Border = 0;
			this.ListOrderCount.Font = new System.Drawing.Font("Tahoma", 12f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.ListOrderCount.ForeAlterColor = Color.Black;
			this.ListOrderCount.ForeHeaderColor = Color.Black;
			this.ListOrderCount.ForeHeaderSelectedColor = Color.White;
			this.ListOrderCount.ForeNormalColor = Color.Black;
			this.ListOrderCount.ForeSelectedColor = Color.White;
			this.ListOrderCount.ItemHeight = 40;
			this.ListOrderCount.ItemWidth = 40;
			this.ListOrderCount.Location = new Point(208, 128);
			this.ListOrderCount.Name = "ListOrderCount";
			this.ListOrderCount.Row = 14;
			this.ListOrderCount.SelectedIndex = 0;
			this.ListOrderCount.Size = new System.Drawing.Size(40, 560);
			this.ListOrderCount.TabIndex = 37;
			this.ListOrderCount.ItemClick += new ItemsListEventHandler(this.ListOrderItem_ItemClick);
			this.ListOrderItemPrice.Alignment = ContentAlignment.MiddleRight;
			this.ListOrderItemPrice.AutoRefresh = true;
			this.ListOrderItemPrice.BackAlterColor = Color.FromArgb(192, 255, 255);
			this.ListOrderItemPrice.BackHeaderColor = Color.FromArgb(255, 192, 128);
			this.ListOrderItemPrice.BackHeaderSelectedColor = Color.FromArgb(192, 0, 0);
			this.ListOrderItemPrice.BackNormalColor = Color.White;
			this.ListOrderItemPrice.BackSelectedColor = Color.Blue;
			this.ListOrderItemPrice.BindList1 = this.ListOrderCount;
			this.ListOrderItemPrice.BindList2 = null;
			this.ListOrderItemPrice.Border = 0;
			this.ListOrderItemPrice.Font = new System.Drawing.Font("Tahoma", 12f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.ListOrderItemPrice.ForeAlterColor = Color.Black;
			this.ListOrderItemPrice.ForeHeaderColor = Color.Black;
			this.ListOrderItemPrice.ForeHeaderSelectedColor = Color.White;
			this.ListOrderItemPrice.ForeNormalColor = Color.Black;
			this.ListOrderItemPrice.ForeSelectedColor = Color.White;
			this.ListOrderItemPrice.ItemHeight = 40;
			this.ListOrderItemPrice.ItemWidth = 80;
			this.ListOrderItemPrice.Location = new Point(248, 128);
			this.ListOrderItemPrice.Name = "ListOrderItemPrice";
			this.ListOrderItemPrice.Row = 14;
			this.ListOrderItemPrice.SelectedIndex = 0;
			this.ListOrderItemPrice.Size = new System.Drawing.Size(80, 560);
			this.ListOrderItemPrice.TabIndex = 33;
			this.ListOrderItemPrice.ItemClick += new ItemsListEventHandler(this.ListOrderItem_ItemClick);
			this.LblPageID.BackColor = Color.Transparent;
			this.LblPageID.Font = new System.Drawing.Font("Tahoma", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.LblPageID.ForeColor = Color.FromArgb(103, 138, 198);
			this.LblPageID.Location = new Point(792, 752);
			this.LblPageID.Name = "LblPageID";
			this.LblPageID.Size = new System.Drawing.Size(216, 23);
			this.LblPageID.TabIndex = 32;
			this.LblPageID.Text = "STCB011";
			this.LblPageID.TextAlign = ContentAlignment.TopRight;
			this.LblCopyright.BackColor = Color.Transparent;
			this.LblCopyright.Font = new System.Drawing.Font("Tahoma", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.LblCopyright.ForeColor = Color.FromArgb(103, 138, 198);
			this.LblCopyright.Location = new Point(8, 752);
			this.LblCopyright.Name = "LblCopyright";
			this.LblCopyright.Size = new System.Drawing.Size(280, 16);
			this.LblCopyright.TabIndex = 36;
			this.LblCopyright.Text = "Copyright (c) 2004. All rights reserved.";
			this.BtnKBInvoiceNote.BackColor = Color.Transparent;
			this.BtnKBInvoiceNote.Blue = 1f;
			this.BtnKBInvoiceNote.Cursor = Cursors.Hand;
			this.BtnKBInvoiceNote.Font = new System.Drawing.Font("Tahoma", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.BtnKBInvoiceNote.Green = 1f;
			this.BtnKBInvoiceNote.ImageClick = (Image)resourceManager.GetObject("BtnKBInvoiceNote.ImageClick");
			this.BtnKBInvoiceNote.ImageClickIndex = 3;
			this.BtnKBInvoiceNote.ImageIndex = 2;
			this.BtnKBInvoiceNote.ImageList = this.CalculatorImgList;
			this.BtnKBInvoiceNote.IsLock = false;
			this.BtnKBInvoiceNote.Location = new Point(296, 344);
			this.BtnKBInvoiceNote.Name = "BtnKBInvoiceNote";
			this.BtnKBInvoiceNote.ObjectValue = null;
			this.BtnKBInvoiceNote.Red = 1f;
			this.BtnKBInvoiceNote.Size = new System.Drawing.Size(40, 40);
			this.BtnKBInvoiceNote.TabIndex = 46;
			this.BtnKBInvoiceNote.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnKBInvoiceNote.Click += new EventHandler(this.BtnKBInvoiceNote_Click);
			this.FieldInvoiceNote.Anchor = AnchorStyles.Left;
			this.FieldInvoiceNote.BackColor = Color.White;
			this.FieldInvoiceNote.BorderStyle = BorderStyle.FixedSingle;
			this.FieldInvoiceNote.Font = new System.Drawing.Font("Tahoma", 27f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.FieldInvoiceNote.Location = new Point(8, 344);
			this.FieldInvoiceNote.Name = "FieldInvoiceNote";
			this.FieldInvoiceNote.Size = new System.Drawing.Size(288, 40);
			this.FieldInvoiceNote.TabIndex = 45;
			this.FieldInvoiceNote.Text = "";
			this.FieldInvoiceNote.TextChanged += new EventHandler(this.FieldInvoiceNote_TextChanged);
			this.FieldInvoiceNote.Leave += new EventHandler(this.FieldInvoiceNote_Leave);
			this.FieldInvoiceNote.Enter += new EventHandler(this.FieldInvoiceNote_Enter);
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
			base.ClientSize = new System.Drawing.Size(1020, 764);
			base.Controls.Add(this.ListOrderCount);
			base.Controls.Add(this.LblCopyright);
			base.Controls.Add(this.ListOrderItemPrice);
			base.Controls.Add(this.LblPageID);
			base.Controls.Add(this.ListOrderItem);
			base.Controls.Add(this.BtnBack);
			base.Controls.Add(this.groupPanel3);
			base.Controls.Add(this.BtnPay);
			base.Controls.Add(this.BtnPrintReceipt);
			base.Controls.Add(this.groupPanel2);
			base.Controls.Add(this.BtnDown);
			base.Controls.Add(this.BtnUp);
			base.Controls.Add(this.BtnUndo);
			base.Controls.Add(this.BtnCancel);
			base.Controls.Add(this.OrderPanel);
			base.Controls.Add(this.BtnCalculator);
			base.Name = "PrintReceiptForm";
			this.Text = "Check Bill";
			this.OrderPanel.ResumeLayout(false);
			this.groupPanel2.ResumeLayout(false);
			this.groupPanel3.ResumeLayout(false);
			base.ResumeLayout(false);
		}

		private void ListOrderItem_ItemClick(object sender, ItemsListEventArgs e)
		{
			this.StartInputNone();
			if (e.Item.Value is smartRestaurant.OrderService.OrderBill)
			{
				this.selectedBill = (smartRestaurant.OrderService.OrderBill)e.Item.Value;
				this.selectedItem = null;
				this.UpdateOrderGrid();
				return;
			}
			if (e.Item.Value is OrderBillItem)
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
				this.UpdateOrderGrid();
			}
		}

		private void LoadDiscountSelected()
		{
			if (this.receipt.UseDiscount.Count > 0)
			{
				for (int i = 0; i < this.receipt.UseDiscount.Count; i++)
				{
					int num = 0;
					while (num < (int)Receipt.Discounts.Length)
					{
						if (((Discount)this.receipt.UseDiscount[i]).promotionID != Receipt.Discounts[num].promotionID)
						{
							num++;
						}
						else
						{
							this.discountSelected.Add(num);
							break;
						}
					}
				}
			}
		}

		private void NumberKeyPad_PadClick(object sender, NumberPadEventArgs e)
		{
			int num;
			double num1;
			int number;
			if (this.inputState == 0)
			{
				return;
			}
			if (e.IsNumeric)
			{
				if (this.inputState != 2)
				{
					PrintReceiptForm printReceiptForm = this;
					string str = printReceiptForm.inputValue;
					number = e.Number;
					printReceiptForm.inputValue = string.Concat(str, number.ToString());
				}
				else
				{
					try
					{
						double number1 = double.Parse(this.inputValue);
						number1 = number1 * 10 + 0.01 * (double)e.Number;
						this.inputValue = number1.ToString("N");
					}
					catch (Exception exception)
					{
						number = e.Number;
						this.inputValue = string.Concat("0.0", number.ToString());
					}
				}
				this.UpdateMonitor();
				return;
			}
			if (!e.IsCancel)
			{
				if (e.IsEnter)
				{
					this.inputValue = this.inputValue.Replace(",", "");
					if (this.inputValue != "")
					{
						try
						{
							num = int.Parse(this.inputValue);
						}
						catch (Exception exception1)
						{
							num = 0;
						}
						try
						{
							num1 = double.Parse(this.inputValue);
						}
						catch (Exception exception2)
						{
							num1 = 0;
						}
						switch (this.inputState)
						{
							case 2:
							{
								this.receipt.PayValue = num1;
								this.receipt.SetPaymentMethod(this.receipt.PaymentMethod, this.receipt.PayValue);
								break;
							}
							case 3:
							{
								this.receipt.PointAmount = num;
								break;
							}
						}
					}
					this.receipt.PaymentMethod = null;
					this.UpdatePaymentTypeList();
					this.StartInputNone();
					this.UpdateSummary();
				}
				return;
			}
			if (this.inputState != 2)
			{
				if (this.inputValue.Length <= 1)
				{
					this.StartInputNone();
					return;
				}
				this.inputValue = this.inputValue.Substring(0, this.inputValue.Length - 1);
				this.UpdateMonitor();
				return;
			}
			try
			{
				double num2 = double.Parse(this.inputValue);
				num2 = Math.Floor(num2 * 10) / 100;
				this.inputValue = num2.ToString("N");
				if (num2 != 0)
				{
					this.UpdateMonitor();
				}
				else
				{
					this.StartInputNone();
					this.receipt.PaymentMethod = null;
					this.UpdatePaymentTypeList();
				}
			}
			catch (Exception exception3)
			{
				this.StartInputNone();
				this.receipt.PaymentMethod = null;
				this.UpdatePaymentTypeList();
			}
			this.receipt.PaymentMethod = null;
		}

		private void PaymentTypePad_PadClick(object sender, ButtonListPadEventArgs e)
		{
			if (e.Value == null)
			{
				return;
			}
			this.receipt.PaymentMethod = Receipt.SearchPaymentMethodByID(int.Parse(e.Value));
			int num = this.receipt.PaymentMethodList.IndexOf(this.receipt.PaymentMethod);
			if (num < 0)
			{
				this.receipt.PayValue = 0;
			}
			else
			{
				this.receipt.PayValue = (double)this.receipt.PayValueList[num];
			}
			this.StartInputPayValue();
			this.UpdatePaymentTypeList();
			this.UpdateSummary();
		}

		private void PaymentTypePad_PageChange(object sender, ButtonListPadEventArgs e)
		{
			this.UpdatePaymentTypeList();
		}

		private void StartInputNone()
		{
			this.inputState = 0;
			this.inputValue = "";
			this.NumberKeyPad.Enabled = false;
			this.receipt.PaymentMethod = null;
			this.UpdateMonitor();
		}

		private void StartInputPayValue()
		{
			this.inputState = 2;
			this.inputValue = "";
			this.NumberKeyPad.Enabled = true;
			this.UpdateMonitor();
		}

		private void StartInputPointAmount()
		{
			this.inputState = 3;
			this.inputValue = "";
			this.NumberKeyPad.Enabled = true;
			this.UpdateMonitor();
		}

		private void UpdateDiscountList()
		{
			Discount[] discounts = Receipt.Discounts;
			if (discounts == null)
			{
				return;
			}
			this.DiscountPad.AutoRefresh = false;
			if (this.DiscountPad.Items.Count == 0)
			{
				for (int i = 0; i < (int)discounts.Length; i++)
				{
					ButtonItem buttonItem = new ButtonItem(discounts[i].description, discounts[i].promotionID.ToString());
					this.DiscountPad.Items.Add(buttonItem);
				}
			}
			this.DiscountPad.Red = 1f;
			this.DiscountPad.Green = 1f;
			this.DiscountPad.Blue = 1f;
			for (int j = 0; j < this.discountSelected.Count; j++)
			{
				int item = (int)this.discountSelected[j];
				int position = this.DiscountPad.GetPosition(item);
				if (position > -1)
				{
					this.DiscountPad.SetMatrix(position, 1f, 2f, 1f);
				}
			}
			this.DiscountPad.AutoRefresh = true;
		}

		private void UpdateFlowButton()
		{
			if (this.receipt.IsCompleted)
			{
				this.BtnPay.Enabled = true;
				return;
			}
			this.BtnPay.Enabled = false;
		}

		public override void UpdateForm()
		{
			int num;
			this.selectedItem = null;
			this.menuTypes = MenuManagement.MenuTypes;
			this.menuOptions = MenuManagement.MenuOptions;
			num = (((MainForm)base.MdiParent).User == null ? this.selectedBill.EmployeeID : ((MainForm)base.MdiParent).User.UserID);
			this.receipt = new Receipt(this.selectedBill, num);
			this.DiscountPad.Items.Clear();
			this.discountSelected.Clear();
			this.paymentSelected.Clear();
			this.LoadDiscountSelected();
			this.LblPageID.Text = string.Concat("Employee ID:", num.ToString(), " | STCB011");
			if (!AppParameter.IsDemo())
			{
				this.LblTotalChange.Text = "Tip";
				this.LblGuest.Text = "Seat";
			}
			else
			{
				this.LblTotalChange.Text = "Change";
				this.LblGuest.Text = "Guest";
			}
			this.StartInputNone();
			this.UpdateTableInformation();
			this.UpdateSummary();
			this.UpdateOrderGrid();
			this.UpdateDiscountList();
			this.UpdatePaymentTypeList();
		}

		private void UpdateMonitor()
		{
			bool flag = false;
			switch (this.inputState)
			{
				case 0:
				{
					this.inputState = 0;
					this.FieldInputType.Text = "";
					break;
				}
				case 1:
				{
					this.FieldInputType.Text = "Payment";
					break;
				}
				case 2:
				{
					this.FieldInputType.Text = "Pay";
					flag = true;
					break;
				}
				case 3:
				{
					this.FieldInputType.Text = "Point";
					flag = true;
					break;
				}
				case 4:
				{
					this.FieldInputType.Text = "Coupon";
					break;
				}
				case 5:
				{
					this.FieldInputType.Text = "Gift Voucher";
					break;
				}
				default:
				{
					goto case 0;
				}
			}
			if (!flag || !(this.inputValue == ""))
			{
				this.FieldCurrentInput.ForeColor = Color.Cyan;
				this.FieldCurrentInput.Text = this.inputValue;
			}
			else
			{
				this.FieldCurrentInput.ForeColor = Color.Yellow;
				if (this.inputState == 2)
				{
					this.FieldCurrentInput.Text = this.receipt.PayValue.ToString("N");
					return;
				}
				if (this.inputState == 3)
				{
					this.FieldCurrentInput.Text = this.receipt.PointAmount.ToString();
					return;
				}
			}
		}

		private void UpdateOrderButton()
		{
			if (this.selectedItem == null || !(this.selectedBill.CloseBillDate == AppParameter.MinDateTime))
			{
				this.BtnCancel.Enabled = false;
				this.BtnUndo.Enabled = false;
			}
			else
			{
				this.BtnCancel.Enabled = !OrderManagement.IsCancel(this.selectedItem);
				this.BtnUndo.Enabled = OrderManagement.IsCancel(this.selectedItem);
			}
			this.BtnUp.Enabled = this.ListOrderItem.CanUp;
			this.BtnDown.Enabled = this.ListOrderItem.CanDown;
		}

		private void UpdateOrderGrid()
		{
			StringBuilder stringBuilder = new StringBuilder();
			this.ListOrderItem.AutoRefresh = false;
			this.ListOrderCount.AutoRefresh = false;
			this.ListOrderItemPrice.AutoRefresh = false;
			this.ListOrderItem.Items.Clear();
			this.ListOrderCount.Items.Clear();
			this.ListOrderItemPrice.Items.Clear();
			stringBuilder.Length = 0;
			if (!AppParameter.IsDemo())
			{
				stringBuilder.Append("Seat #");
			}
			else
			{
				stringBuilder.Append("Bill #");
			}
			stringBuilder.Append(this.selectedBill.BillID);
			DataItem dataItem = new DataItem(stringBuilder.ToString(), this.selectedBill, true);
			this.ListOrderItem.Items.Add(dataItem);
			dataItem = new DataItem("", this.selectedBill, true);
			this.ListOrderCount.Items.Add(dataItem);
			dataItem = new DataItem("", this.selectedBill, true);
			this.ListOrderItemPrice.Items.Add(dataItem);
			if (this.selectedItem == null)
			{
				this.ListOrderItem.SelectedIndex = this.ListOrderItem.Items.Count - 1;
				this.ListOrderCount.SelectedIndex = this.ListOrderCount.Items.Count - 1;
				this.ListOrderItemPrice.SelectedIndex = this.ListOrderItemPrice.Items.Count - 1;
			}
			OrderBillItem[] items = this.selectedBill.Items;
			if (items != null)
			{
				for (int i = 0; i < (int)items.Length; i++)
				{
					dataItem = new DataItem(OrderManagement.OrderBillItemDisplayString(items[i]), items[i], false);
					if (OrderManagement.IsCancel(items[i]))
					{
						dataItem.Strike = true;
					}
					this.ListOrderItem.Items.Add(dataItem);
					dataItem = new DataItem(items[i].Unit.ToString(), items[i], false);
					if (OrderManagement.IsCancel(items[i]))
					{
						dataItem.Strike = true;
					}
					this.ListOrderCount.Items.Add(dataItem);
					dataItem = new DataItem(MenuManagement.GetMenuItemFromID(items[i].MenuID).Price.ToString("N"), items[i], false);
					if (OrderManagement.IsCancel(items[i]))
					{
						dataItem.Strike = true;
					}
					this.ListOrderItemPrice.Items.Add(dataItem);
					if (this.selectedItem == items[i])
					{
						this.ListOrderItem.SelectedIndex = this.ListOrderItem.Items.Count - 1;
						this.ListOrderCount.SelectedIndex = this.ListOrderCount.Items.Count - 1;
						this.ListOrderItemPrice.SelectedIndex = this.ListOrderItemPrice.Items.Count - 1;
					}
				}
			}
			this.ListOrderItem.AutoRefresh = true;
			this.ListOrderCount.AutoRefresh = true;
			this.ListOrderItemPrice.AutoRefresh = true;
			this.UpdateOrderButton();
			this.UpdateFlowButton();
		}

		private void UpdatePaymentButtion()
		{
			if (this.inputState == 2)
			{
				this.BtnPayClear.Enabled = true;
				this.BtnFillPay.Enabled = true;
				return;
			}
			this.BtnPayClear.Enabled = false;
			this.BtnFillPay.Enabled = false;
		}

		private void UpdatePaymentTypeList()
		{
			ButtonItem buttonItem;
			double item;
			if (this.paymentMethods == null)
			{
				return;
			}
			this.PaymentTypePad.AutoRefresh = false;
			if (this.PaymentTypePad.Items.Count == 0)
			{
				for (int i = 0; i < (int)this.paymentMethods.Length; i++)
				{
					buttonItem = new ButtonItem(this.paymentMethods[i].paymentMethodName, this.paymentMethods[i].paymentMethodID.ToString());
					this.PaymentTypePad.Items.Add(buttonItem);
				}
			}
			for (int j = 0; j < (int)this.paymentMethods.Length; j++)
			{
				buttonItem = (ButtonItem)this.PaymentTypePad.Items[j];
				int num = this.receipt.PaymentMethodList.IndexOf(this.paymentMethods[j]);
				if (this.paymentMethods[j] == this.receipt.PaymentMethod)
				{
					this.PaymentTypePad.SetMatrix(j, 1f, 1.75f, 1.75f);
					if (num >= 0)
					{
						string str = this.paymentMethods[j].paymentMethodName;
						item = (double)this.receipt.PayValueList[num];
						buttonItem.Text = string.Concat(str, "\n", item.ToString("N"));
					}
				}
				else if (num < 0)
				{
					this.PaymentTypePad.SetMatrix(j, 1f, 1f, 1f);
					buttonItem.Text = this.paymentMethods[j].paymentMethodName;
				}
				else
				{
					this.PaymentTypePad.SetMatrix(j, 1f, 2f, 2f);
					string str1 = this.paymentMethods[j].paymentMethodName;
					item = (double)this.receipt.PayValueList[num];
					buttonItem.Text = string.Concat(str1, "\n", item.ToString("N"));
				}
			}
			this.PaymentTypePad.AutoRefresh = true;
			this.UpdatePaymentButtion();
		}

		private void UpdateSummary()
		{
			this.receipt.Compute();
			Label fieldAmountDue = this.FieldAmountDue;
			double amountDue = this.receipt.AmountDue;
			fieldAmountDue.Text = amountDue.ToString("N");
			this.FieldTax1.Visible = (this.LblTax1.Text != "" ? true : this.receipt.Tax1 > 0);
			this.FieldTax2.Visible = (this.LblTax2.Text != "" ? true : this.receipt.Tax2 > 0);
			Label fieldTax1 = this.FieldTax1;
			amountDue = this.receipt.Tax1;
			fieldTax1.Text = amountDue.ToString("N");
			Label fieldTax2 = this.FieldTax2;
			amountDue = this.receipt.Tax2;
			fieldTax2.Text = amountDue.ToString("N");
			Label fieldTotalDiscount = this.FieldTotalDiscount;
			amountDue = this.receipt.TotalDiscount;
			fieldTotalDiscount.Text = amountDue.ToString("N");
			Label fieldTotalDue = this.FieldTotalDue;
			amountDue = this.receipt.TotalDue;
			fieldTotalDue.Text = amountDue.ToString("N");
			Label fieldTotalReceive = this.FieldTotalReceive;
			amountDue = this.receipt.TotalReceive;
			fieldTotalReceive.Text = amountDue.ToString("N");
			Label fieldChange = this.FieldChange;
			amountDue = this.receipt.Change;
			fieldChange.Text = amountDue.ToString("N");
			if (this.receipt.TotalReceive >= this.receipt.TotalDue)
			{
				this.FieldTotalReceive.ForeColor = Color.Black;
			}
			else
			{
				this.FieldTotalReceive.ForeColor = Color.Red;
			}
			if (this.receipt.Change <= 0)
			{
				this.FieldChange.ForeColor = Color.Black;
			}
			else
			{
				this.FieldChange.ForeColor = Color.Blue;
			}
			this.FieldInvoiceNote.Text = this.receipt.InvoiceNote;
			this.UpdateFlowButton();
		}

		private void UpdateTableInformation()
		{
			this.FieldTable.Text = this.tableInfo.TableName;
			this.FieldGuest.Text = this.guestNumber.ToString();
			this.FieldBill.Text = this.billNumber.ToString();
		}
	}
}