using System;
using System.Drawing;
using System.Data;
using System.Collections;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using smartRestaurant.Controls;
using smartRestaurant.Data;
using smartRestaurant.MenuService;
using smartRestaurant.OrderService;
using smartRestaurant.PaymentService;
using smartRestaurant.TableService;
using smartRestaurant.Utils;
using System.Resources;
namespace smartRestaurant
{
	/// <summary>
	/// <b>PrintReceiptForm</b> is form for print receipt and check bill form.
	/// </summary>
	public class PrintReceiptForm : SmartForm
	{
		// Fields
		private int billNumber;
		private ImageButton BtnBack;
		private ImageButton BtnCalculator;
		private ImageButton BtnCancel;
		private ImageButton BtnDown;
		private ImageButton BtnFillPay;
		private ImageButton BtnKBInvoiceNote;
		private ImageButton BtnPay;
		private ImageButton BtnPayClear;
		private ImageButton BtnPayClearAll;
		private ImageButton BtnPrintReceipt;
		private ImageButton BtnUndo;
		private ImageButton BtnUp;
		private ImageList ButtonImgList;
		private ImageList ButtonLiteImgList;
		private ImageList CalculatorImgList;
		private IContainer components;
		private ButtonListPad DiscountPad;
		private ArrayList discountSelected;
		private int employeeID;
		private Label FieldAmountDue;
		private Label FieldBill;
		private Label FieldChange;
		private Label FieldCurrentInput;
		private Label FieldGuest;
		private Label FieldInputType;
		private TextBox FieldInvoiceNote;
		private Label FieldTable;
		private Label FieldTax1;
		private Label FieldTax2;
		private Label FieldTotalDiscount;
		private Label FieldTotalDue;
		private Label FieldTotalReceive;
		private GroupPanel groupPanel2;
		private GroupPanel groupPanel3;
		private int guestNumber;
		private const int INPUT_COUPON = 4;
		private const int INPUT_GIFT = 5;
		private const int INPUT_NONE = 0;
		private const int INPUT_PAYMENT = 1;
		private const int INPUT_PAYVALUE = 2;
		private const int INPUT_POINTAMOUNT = 3;
		private int inputState;
		private string inputValue;
		private Label LblAmountDue;
		private Label LblBill;
		private Label LblCopyright;
		private Label LblDiscount;
		private Label LblGuest;
		private Label LblPageID;
		private Label LblPayment;
		private Label LblTable;
		private Label LblTax1;
		private Label LblTax2;
		private Label LblTotalChange;
		private Label LblTotalDiscount;
		private Label LblTotalDue;
		private Label LblTotalReceive;
		private ItemsList ListOrderCount;
		private ItemsList ListOrderItem;
		private ItemsList ListOrderItemPrice;
		private MenuOption[] menuOptions;
		private MenuType[] menuTypes;
		private ImageList NumberImgList;
		private NumberPad NumberKeyPad;
		private OrderInformation orderInfo;
		private GroupPanel OrderPanel;
		private PaymentMethod[] paymentMethods;
		private ArrayList paymentSelected;
		private ButtonListPad PaymentTypePad;
		private Receipt receipt;
		private OrderBill selectedBill;
		private OrderBillItem selectedItem;
		private TableInformation tableInfo;

		// Methods
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
			if (this.orderInfo.TableID != 0)
			{
				((MainForm) base.MdiParent).ShowTakeOrderForm(null);
			}
			else
			{
				((MainForm) base.MdiParent).ShowMainMenuForm();
			}
		}

		private void BtnCalculator_Click(object sender, EventArgs e)
		{
			CalculatorForm.Show(true);
		}

		private void BtnCancel_Click(object sender, EventArgs e)
		{
			if ((this.selectedItem != null) && OrderManagement.CancelOrderBillItem(this.selectedBill, this.selectedItem, this.employeeID))
			{
				this.UpdateOrderGrid();
				string text = new smartRestaurant.OrderService.OrderService().SendOrderBill(this.selectedBill);
				if (text != null)
				{
					MessageBox.Show(this, text);
				}
				else
				{
					this.UpdateSummary();
				}
			}
		}

		private void BtnDown_Click(object sender, EventArgs e)
		{
			this.ListOrderItem.Down(5);
			this.UpdateOrderButton();
		}

		private void BtnFillPay_Click(object sender, EventArgs e)
		{
			double num;
			try
			{
				int index = this.receipt.PaymentMethodList.IndexOf(this.receipt.PaymentMethod);
				num = (double) this.receipt.PayValueList[index];
			}
			catch (Exception)
			{
				num = 0.0;
			}
			this.receipt.PayValue = (this.receipt.TotalDue - this.receipt.TotalReceive) + num;
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
			smartRestaurant.OrderService.OrderService service = new smartRestaurant.OrderService.OrderService();
			WaitingForm.Show("Print Bill");
			base.Enabled = false;
			string text = service.SendOrderBill(this.selectedBill);
			if (text != null)
			{
				base.Enabled = true;
				WaitingForm.HideForm();
				MessageBox.Show(this, text);
			}
			else
			{
				bool flag = this.receipt.SendInvoice(true, true);
				base.Enabled = true;
				WaitingForm.HideForm();
				if (flag)
				{
					((MainForm) base.MdiParent).ShowTakeOrderForm(null);
				}
				else
				{
					if (this.orderInfo.TableID != 0)
					{
						new smartRestaurant.TableService.TableService().UpdateTableLockInuse(this.orderInfo.TableID, false);
					}
					((MainForm) base.MdiParent).ShowMainMenuForm();
				}
			}
		}

		private void BtnPayClear_Click(object sender, EventArgs e)
		{
			this.receipt.PayValue = 0.0;
			this.receipt.SetPaymentMethod(this.receipt.PaymentMethod, this.receipt.PayValue);
			this.receipt.PaymentMethod = null;
			this.StartInputNone();
			this.UpdatePaymentTypeList();
			this.UpdateSummary();
		}

		private void BtnPayClearAll_Click(object sender, EventArgs e)
		{
			this.receipt.SetPaymentMethod(null, 0.0);
			this.receipt.PaymentMethod = null;
			this.StartInputNone();
			this.UpdatePaymentTypeList();
			this.UpdateSummary();
		}

		private void BtnPrintReceipt_Click(object sender, EventArgs e)
		{
			smartRestaurant.OrderService.OrderService service = new smartRestaurant.OrderService.OrderService();
			WaitingForm.Show("Print Receipt");
			base.Enabled = false;
			string text = service.SendOrderBill(this.selectedBill);
			if (text != null)
			{
				base.Enabled = true;
				WaitingForm.HideForm();
				MessageBox.Show(this, text);
			}
			else
			{
				bool flag = this.receipt.SendInvoice(false, true);
				base.Enabled = true;
				WaitingForm.HideForm();
				if (flag && (this.orderInfo.TableID != 0))
				{
					((MainForm) base.MdiParent).ShowTakeOrderForm(null);
				}
				else
				{
					((MainForm) base.MdiParent).ShowMainMenuForm();
				}
			}
		}

		private void BtnUndo_Click(object sender, EventArgs e)
		{
			if (this.selectedItem != null)
			{
				OrderManagement.UndoCancelOrderBillItem(this.selectedItem, this.employeeID);
				this.UpdateOrderGrid();
				string text = new smartRestaurant.OrderService.OrderService().SendOrderBill(this.selectedBill);
				if (text != null)
				{
					MessageBox.Show(this, text);
				}
				else
				{
					this.UpdateSummary();
				}
			}
		}

		private void BtnUp_Click(object sender, EventArgs e)
		{
			this.ListOrderItem.Up(5);
			this.UpdateOrderButton();
		}

		private void DiscountPad_PadClick(object sender, ButtonListPadEventArgs e)
		{
			if (e.Value != null)
			{
				Discount dis = Receipt.SearchDiscountByID(int.Parse(e.Value));
				if (this.discountSelected.Contains(e.Index))
				{
					this.discountSelected.Remove(e.Index);
					this.receipt.UseDiscountRemove(dis);
				}
				else
				{
					this.discountSelected.Add(e.Index);
					this.receipt.UseDiscountAdd(dis);
				}
				this.UpdateDiscountList();
				this.UpdateSummary();
			}
		}

		private void DiscountPad_PageChange(object sender, ButtonListPadEventArgs e)
		{
			this.UpdateDiscountList();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && (this.components != null))
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void FieldInvoiceNote_Enter(object sender, EventArgs e)
		{
			TextBox box = (TextBox) sender;
			box.BackColor = Color.FromArgb(0xff, 0xff, 0xc0);
		}

		private void FieldInvoiceNote_Leave(object sender, EventArgs e)
		{
			TextBox box = (TextBox) sender;
			box.BackColor = Color.White;
		}

		private void FieldInvoiceNote_TextChanged(object sender, EventArgs e)
		{
			this.receipt.InvoiceNote = this.FieldInvoiceNote.Text;
		}

		private void InitializeComponent()
		{
			this.components = new Container();
			ResourceManager manager = new ResourceManager(typeof(PrintReceiptForm));
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
			this.OrderPanel.Font = new Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.OrderPanel.Location = new Point(560, 0);
			this.OrderPanel.Name = "OrderPanel";
			this.OrderPanel.ShowHeader = false;
			this.OrderPanel.Size = new Size(0x1c1, 0x3a);
			this.OrderPanel.TabIndex = 2;
			this.FieldBill.BackColor = Color.White;
			this.FieldBill.Location = new Point(0x180, 1);
			this.FieldBill.Name = "FieldBill";
			this.FieldBill.Size = new Size(0x40, 0x38);
			this.FieldBill.TabIndex = 11;
			this.FieldBill.Text = "1";
			this.FieldBill.TextAlign = ContentAlignment.MiddleCenter;
			this.LblBill.BackColor = Color.Orange;
			this.LblBill.Location = new Point(0x138, 1);
			this.LblBill.Name = "LblBill";
			this.LblBill.Size = new Size(0x48, 0x38);
			this.LblBill.TabIndex = 10;
			this.LblBill.Text = "Bill:";
			this.LblBill.TextAlign = ContentAlignment.MiddleCenter;
			this.FieldGuest.BackColor = Color.White;
			this.FieldGuest.Location = new Point(0xf8, 1);
			this.FieldGuest.Name = "FieldGuest";
			this.FieldGuest.Size = new Size(0x40, 0x38);
			this.FieldGuest.TabIndex = 9;
			this.FieldGuest.Text = "1";
			this.FieldGuest.TextAlign = ContentAlignment.MiddleCenter;
			this.LblGuest.BackColor = Color.Orange;
			this.LblGuest.Location = new Point(0xb0, 1);
			this.LblGuest.Name = "LblGuest";
			this.LblGuest.Size = new Size(0x48, 0x38);
			this.LblGuest.TabIndex = 8;
			this.LblGuest.Text = "Seat:";
			this.LblGuest.TextAlign = ContentAlignment.MiddleCenter;
			this.FieldTable.BackColor = Color.White;
			this.FieldTable.Cursor = Cursors.Default;
			this.FieldTable.Location = new Point(0x49, 1);
			this.FieldTable.Name = "FieldTable";
			this.FieldTable.Size = new Size(0x67, 0x38);
			this.FieldTable.TabIndex = 7;
			this.FieldTable.Text = "1";
			this.FieldTable.TextAlign = ContentAlignment.MiddleCenter;
			this.LblTable.BackColor = Color.Orange;
			this.LblTable.Location = new Point(1, 1);
			this.LblTable.Name = "LblTable";
			this.LblTable.Size = new Size(0x48, 0x38);
			this.LblTable.TabIndex = 6;
			this.LblTable.Text = "Table:";
			this.LblTable.TextAlign = ContentAlignment.MiddleCenter;
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
			this.BtnDown.TabIndex = 0x18;
			this.BtnDown.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnDown.Click += new EventHandler(this.BtnDown_Click);
			this.ButtonImgList.ColorDepth = ColorDepth.Depth32Bit;
			this.ButtonImgList.ImageSize = new Size(110, 60);
			this.ButtonImgList.ImageStream = (ImageListStreamer) manager.GetObject("ButtonImgList.ImageStream");
			this.ButtonImgList.TransparentColor = Color.Transparent;
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
			this.BtnUp.TabIndex = 0x17;
			this.BtnUp.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnUp.Click += new EventHandler(this.BtnUp_Click);
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
			this.BtnUndo.TabIndex = 0x15;
			this.BtnUndo.Text = "Undo";
			this.BtnUndo.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnUndo.Click += new EventHandler(this.BtnUndo_Click);
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
			this.BtnCancel.TabIndex = 20;
			this.BtnCancel.Text = "Cancel";
			this.BtnCancel.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnCancel.Click += new EventHandler(this.BtnCancel_Click);
			this.NumberImgList.ColorDepth = ColorDepth.Depth32Bit;
			this.NumberImgList.ImageSize = new Size(0x48, 60);
			this.NumberImgList.ImageStream = (ImageListStreamer) manager.GetObject("NumberImgList.ImageStream");
			this.NumberImgList.TransparentColor = Color.Transparent;
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
			this.FieldInputType.Text = "none";
			this.FieldInputType.TextAlign = ContentAlignment.MiddleCenter;
			this.NumberKeyPad.BackColor = Color.White;
			this.NumberKeyPad.Font = new Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.NumberKeyPad.Image = (Image) manager.GetObject("NumberKeyPad.Image");
			this.NumberKeyPad.ImageClick = (Image) manager.GetObject("NumberKeyPad.ImageClick");
			this.NumberKeyPad.ImageClickIndex = 1;
			this.NumberKeyPad.ImageIndex = 0;
			this.NumberKeyPad.ImageList = this.NumberImgList;
			this.NumberKeyPad.Location = new Point(0x38, 360);
			this.NumberKeyPad.Name = "NumberKeyPad";
			this.NumberKeyPad.Size = new Size(0xe2, 0xff);
			this.NumberKeyPad.TabIndex = 7;
			this.NumberKeyPad.Text = "numberPad1";
			this.NumberKeyPad.PadClick += new smartRestaurant.Controls.NumberPad.NumberPadEventHandler(this.NumberKeyPad_PadClick);
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
			this.groupPanel2.Font = new Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.groupPanel2.Location = new Point(0x148, 0x80);
			this.groupPanel2.Name = "groupPanel2";
			this.groupPanel2.ShowHeader = false;
			this.groupPanel2.Size = new Size(0x158, 0x270);
			this.groupPanel2.TabIndex = 0x19;
			this.FieldChange.Location = new Point(0xa8, 0x100);
			this.FieldChange.Name = "FieldChange";
			this.FieldChange.Size = new Size(160, 40);
			this.FieldChange.TabIndex = 0x17;
			this.FieldChange.Text = "0.00";
			this.FieldChange.TextAlign = ContentAlignment.MiddleRight;
			this.FieldTotalReceive.Location = new Point(0xa8, 0xd8);
			this.FieldTotalReceive.Name = "FieldTotalReceive";
			this.FieldTotalReceive.Size = new Size(160, 40);
			this.FieldTotalReceive.TabIndex = 0x16;
			this.FieldTotalReceive.Text = "0.00";
			this.FieldTotalReceive.TextAlign = ContentAlignment.MiddleRight;
			this.FieldTotalDue.Font = new Font("Tahoma", 15.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.FieldTotalDue.ForeColor = Color.Green;
			this.FieldTotalDue.Location = new Point(0xa8, 0xb0);
			this.FieldTotalDue.Name = "FieldTotalDue";
			this.FieldTotalDue.Size = new Size(160, 40);
			this.FieldTotalDue.TabIndex = 0x15;
			this.FieldTotalDue.Text = "0.00";
			this.FieldTotalDue.TextAlign = ContentAlignment.MiddleRight;
			this.FieldTotalDiscount.Location = new Point(0xa8, 0x38);
			this.FieldTotalDiscount.Name = "FieldTotalDiscount";
			this.FieldTotalDiscount.Size = new Size(160, 40);
			this.FieldTotalDiscount.TabIndex = 20;
			this.FieldTotalDiscount.Text = "0.00";
			this.FieldTotalDiscount.TextAlign = ContentAlignment.MiddleRight;
			this.FieldTax2.Location = new Point(0xa8, 0x88);
			this.FieldTax2.Name = "FieldTax2";
			this.FieldTax2.Size = new Size(160, 40);
			this.FieldTax2.TabIndex = 0x13;
			this.FieldTax2.Text = "0.00";
			this.FieldTax2.TextAlign = ContentAlignment.MiddleRight;
			this.FieldTax1.Location = new Point(0xa8, 0x60);
			this.FieldTax1.Name = "FieldTax1";
			this.FieldTax1.Size = new Size(160, 40);
			this.FieldTax1.TabIndex = 0x12;
			this.FieldTax1.Text = "0.00";
			this.FieldTax1.TextAlign = ContentAlignment.MiddleRight;
			this.FieldAmountDue.Location = new Point(0xa8, 0x10);
			this.FieldAmountDue.Name = "FieldAmountDue";
			this.FieldAmountDue.Size = new Size(160, 40);
			this.FieldAmountDue.TabIndex = 0x11;
			this.FieldAmountDue.Text = "0.00";
			this.FieldAmountDue.TextAlign = ContentAlignment.MiddleRight;
			this.LblTotalChange.Location = new Point(0x10, 0x100);
			this.LblTotalChange.Name = "LblTotalChange";
			this.LblTotalChange.Size = new Size(0x90, 40);
			this.LblTotalChange.TabIndex = 0x10;
			this.LblTotalChange.Text = "Change";
			this.LblTotalChange.TextAlign = ContentAlignment.MiddleLeft;
			this.LblTotalReceive.Location = new Point(0x10, 0xd8);
			this.LblTotalReceive.Name = "LblTotalReceive";
			this.LblTotalReceive.Size = new Size(0x90, 40);
			this.LblTotalReceive.TabIndex = 15;
			this.LblTotalReceive.Text = "Total Receive";
			this.LblTotalReceive.TextAlign = ContentAlignment.MiddleLeft;
			this.LblTotalDue.Font = new Font("Tahoma", 15.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.LblTotalDue.ForeColor = Color.Green;
			this.LblTotalDue.Location = new Point(0x10, 0xb0);
			this.LblTotalDue.Name = "LblTotalDue";
			this.LblTotalDue.Size = new Size(0x90, 40);
			this.LblTotalDue.TabIndex = 14;
			this.LblTotalDue.Text = "Total Due";
			this.LblTotalDue.TextAlign = ContentAlignment.MiddleLeft;
			this.LblTotalDiscount.Location = new Point(0x10, 0x38);
			this.LblTotalDiscount.Name = "LblTotalDiscount";
			this.LblTotalDiscount.Size = new Size(0x90, 40);
			this.LblTotalDiscount.TabIndex = 13;
			this.LblTotalDiscount.Text = "Total Discount";
			this.LblTotalDiscount.TextAlign = ContentAlignment.MiddleLeft;
			this.LblTax2.Location = new Point(0x10, 0x88);
			this.LblTax2.Name = "LblTax2";
			this.LblTax2.Size = new Size(0x90, 40);
			this.LblTax2.TabIndex = 12;
			this.LblTax2.Text = "Tax2";
			this.LblTax2.TextAlign = ContentAlignment.MiddleLeft;
			this.LblTax1.Location = new Point(0x10, 0x60);
			this.LblTax1.Name = "LblTax1";
			this.LblTax1.Size = new Size(0x90, 40);
			this.LblTax1.TabIndex = 11;
			this.LblTax1.Text = "Tax1";
			this.LblTax1.TextAlign = ContentAlignment.MiddleLeft;
			this.LblAmountDue.Location = new Point(0x10, 0x10);
			this.LblAmountDue.Name = "LblAmountDue";
			this.LblAmountDue.Size = new Size(0x90, 40);
			this.LblAmountDue.TabIndex = 10;
			this.LblAmountDue.Text = "Amount Due";
			this.LblAmountDue.TextAlign = ContentAlignment.MiddleLeft;
			this.BtnCalculator.BackColor = Color.Transparent;
			this.BtnCalculator.Blue = 1f;
			this.BtnCalculator.Cursor = Cursors.Hand;
			this.BtnCalculator.Green = 1f;
			this.BtnCalculator.ImageClick = (Image) manager.GetObject("BtnCalculator.ImageClick");
			this.BtnCalculator.ImageClickIndex = 1;
			this.BtnCalculator.ImageIndex = 0;
			this.BtnCalculator.ImageList = this.CalculatorImgList;
			this.BtnCalculator.IsLock = false;
			this.BtnCalculator.Location = new Point(0x2e8, 0x40);
			this.BtnCalculator.Name = "BtnCalculator";
			this.BtnCalculator.ObjectValue = null;
			this.BtnCalculator.Red = 1f;
			this.BtnCalculator.Size = new Size(40, 40);
			this.BtnCalculator.TabIndex = 40;
			this.BtnCalculator.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnCalculator.Click += new EventHandler(this.BtnCalculator_Click);
			this.CalculatorImgList.ColorDepth = ColorDepth.Depth32Bit;
			this.CalculatorImgList.ImageSize = new Size(40, 40);
			this.CalculatorImgList.ImageStream = (ImageListStreamer) manager.GetObject("CalculatorImgList.ImageStream");
			this.CalculatorImgList.TransparentColor = Color.Transparent;
			this.BtnPay.BackColor = Color.Transparent;
			this.BtnPay.Blue = 1f;
			this.BtnPay.Cursor = Cursors.Hand;
			this.BtnPay.Green = 1f;
			this.BtnPay.ImageClick = (Image) manager.GetObject("BtnPay.ImageClick");
			this.BtnPay.ImageClickIndex = 1;
			this.BtnPay.ImageIndex = 0;
			this.BtnPay.ImageList = this.ButtonImgList;
			this.BtnPay.IsLock = false;
			this.BtnPay.Location = new Point(0x388, 0x40);
			this.BtnPay.Name = "BtnPay";
			this.BtnPay.ObjectValue = null;
			this.BtnPay.Red = 1.75f;
			this.BtnPay.Size = new Size(110, 60);
			this.BtnPay.TabIndex = 0x1c;
			this.BtnPay.Text = "Close-Bill";
			this.BtnPay.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnPay.Click += new EventHandler(this.BtnPay_Click);
			this.BtnPrintReceipt.BackColor = Color.Transparent;
			this.BtnPrintReceipt.Blue = 1.75f;
			this.BtnPrintReceipt.Cursor = Cursors.Hand;
			this.BtnPrintReceipt.Green = 1f;
			this.BtnPrintReceipt.ImageClick = (Image) manager.GetObject("BtnPrintReceipt.ImageClick");
			this.BtnPrintReceipt.ImageClickIndex = 1;
			this.BtnPrintReceipt.ImageIndex = 0;
			this.BtnPrintReceipt.ImageList = this.ButtonImgList;
			this.BtnPrintReceipt.IsLock = false;
			this.BtnPrintReceipt.Location = new Point(0x318, 0x40);
			this.BtnPrintReceipt.Name = "BtnPrintReceipt";
			this.BtnPrintReceipt.ObjectValue = null;
			this.BtnPrintReceipt.Red = 1.75f;
			this.BtnPrintReceipt.Size = new Size(110, 60);
			this.BtnPrintReceipt.TabIndex = 0x1b;
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
			this.groupPanel3.Font = new Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.groupPanel3.Location = new Point(0x2a0, 0x80);
			this.groupPanel3.Name = "groupPanel3";
			this.groupPanel3.ShowHeader = false;
			this.groupPanel3.Size = new Size(0x158, 0x270);
			this.groupPanel3.TabIndex = 0x1d;
			this.BtnPayClearAll.BackColor = Color.Transparent;
			this.BtnPayClearAll.Blue = 2f;
			this.BtnPayClearAll.Cursor = Cursors.Hand;
			this.BtnPayClearAll.Font = new Font("Tahoma", 12f, FontStyle.Regular, GraphicsUnit.Pixel);
			this.BtnPayClearAll.Green = 1f;
			this.BtnPayClearAll.ImageClick = (Image) manager.GetObject("BtnPayClearAll.ImageClick");
			this.BtnPayClearAll.ImageClickIndex = 1;
			this.BtnPayClearAll.ImageIndex = 0;
			this.BtnPayClearAll.ImageList = this.ButtonLiteImgList;
			this.BtnPayClearAll.IsLock = false;
			this.BtnPayClearAll.Location = new Point(8, 0x240);
			this.BtnPayClearAll.Name = "BtnPayClearAll";
			this.BtnPayClearAll.ObjectValue = null;
			this.BtnPayClearAll.Red = 1f;
			this.BtnPayClearAll.Size = new Size(110, 40);
			this.BtnPayClearAll.TabIndex = 0x2c;
			this.BtnPayClearAll.Text = "Clear All";
			this.BtnPayClearAll.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnPayClearAll.Click += new EventHandler(this.BtnPayClearAll_Click);
			this.ButtonLiteImgList.ColorDepth = ColorDepth.Depth32Bit;
			this.ButtonLiteImgList.ImageSize = new Size(110, 40);
			this.ButtonLiteImgList.ImageStream = (ImageListStreamer) manager.GetObject("ButtonLiteImgList.ImageStream");
			this.ButtonLiteImgList.TransparentColor = Color.Transparent;
			this.BtnPayClear.BackColor = Color.Transparent;
			this.BtnPayClear.Blue = 2f;
			this.BtnPayClear.Cursor = Cursors.Hand;
			this.BtnPayClear.Font = new Font("Tahoma", 12f, FontStyle.Regular, GraphicsUnit.Pixel);
			this.BtnPayClear.Green = 1f;
			this.BtnPayClear.ImageClick = (Image) manager.GetObject("BtnPayClear.ImageClick");
			this.BtnPayClear.ImageClickIndex = 1;
			this.BtnPayClear.ImageIndex = 0;
			this.BtnPayClear.ImageList = this.ButtonLiteImgList;
			this.BtnPayClear.IsLock = false;
			this.BtnPayClear.Location = new Point(0x77, 0x240);
			this.BtnPayClear.Name = "BtnPayClear";
			this.BtnPayClear.ObjectValue = null;
			this.BtnPayClear.Red = 1f;
			this.BtnPayClear.Size = new Size(110, 40);
			this.BtnPayClear.TabIndex = 0x2b;
			this.BtnPayClear.Text = "Clear";
			this.BtnPayClear.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnPayClear.Click += new EventHandler(this.BtnPayClear_Click);
			this.BtnFillPay.BackColor = Color.Transparent;
			this.BtnFillPay.Blue = 2f;
			this.BtnFillPay.Cursor = Cursors.Hand;
			this.BtnFillPay.Font = new Font("Tahoma", 12f, FontStyle.Regular, GraphicsUnit.Pixel);
			this.BtnFillPay.Green = 1f;
			this.BtnFillPay.ImageClick = (Image) manager.GetObject("BtnFillPay.ImageClick");
			this.BtnFillPay.ImageClickIndex = 1;
			this.BtnFillPay.ImageIndex = 0;
			this.BtnFillPay.ImageList = this.ButtonLiteImgList;
			this.BtnFillPay.IsLock = false;
			this.BtnFillPay.Location = new Point(230, 0x240);
			this.BtnFillPay.Name = "BtnFillPay";
			this.BtnFillPay.ObjectValue = null;
			this.BtnFillPay.Red = 1f;
			this.BtnFillPay.Size = new Size(110, 40);
			this.BtnFillPay.TabIndex = 0x2a;
			this.BtnFillPay.Text = "Fill Pay";
			this.BtnFillPay.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnFillPay.Click += new EventHandler(this.BtnFillPay_Click);
			this.PaymentTypePad.AutoRefresh = true;
			this.PaymentTypePad.BackColor = Color.White;
			this.PaymentTypePad.Blue = 1f;
			this.PaymentTypePad.Column = 3;
			this.PaymentTypePad.Font = new Font("Tahoma", 12f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.PaymentTypePad.Green = 1f;
			this.PaymentTypePad.Image = (Image) manager.GetObject("PaymentTypePad.Image");
			this.PaymentTypePad.ImageClick = (Image) manager.GetObject("PaymentTypePad.ImageClick");
			this.PaymentTypePad.ImageClickIndex = 1;
			this.PaymentTypePad.ImageIndex = 0;
			this.PaymentTypePad.ImageList = this.ButtonImgList;
			this.PaymentTypePad.ItemStart = 0;
			this.PaymentTypePad.Location = new Point(8, 0x184);
			this.PaymentTypePad.Name = "PaymentTypePad";
			this.PaymentTypePad.Padding = 1;
			this.PaymentTypePad.Red = 1f;
			this.PaymentTypePad.Row = 3;
			this.PaymentTypePad.Size = new Size(0x14c, 0xb6);
			this.PaymentTypePad.TabIndex = 0x29;
			this.PaymentTypePad.PadClick += new ButtonListPadEventHandler(this.PaymentTypePad_PadClick);
			this.PaymentTypePad.PageChange += new ButtonListPadEventHandler(this.PaymentTypePad_PageChange);
			this.LblPayment.BackColor = Color.Black;
			this.LblPayment.ForeColor = Color.White;
			this.LblPayment.Location = new Point(0, 0x128);
			this.LblPayment.Name = "LblPayment";
			this.LblPayment.Size = new Size(0x158, 40);
			this.LblPayment.TabIndex = 1;
			this.LblPayment.Text = "Payment Receive";
			this.LblPayment.TextAlign = ContentAlignment.MiddleLeft;
			this.LblDiscount.BackColor = Color.Black;
			this.LblDiscount.ForeColor = Color.White;
			this.LblDiscount.Location = new Point(0, 0);
			this.LblDiscount.Name = "LblDiscount";
			this.LblDiscount.Size = new Size(0x158, 40);
			this.LblDiscount.TabIndex = 0;
			this.LblDiscount.Text = "Discount";
			this.LblDiscount.TextAlign = ContentAlignment.MiddleLeft;
			this.DiscountPad.AutoRefresh = true;
			this.DiscountPad.BackColor = Color.White;
			this.DiscountPad.Blue = 1f;
			this.DiscountPad.Column = 3;
			this.DiscountPad.Font = new Font("Tahoma", 12f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.DiscountPad.Green = 1f;
			this.DiscountPad.Image = (Image) manager.GetObject("DiscountPad.Image");
			this.DiscountPad.ImageClick = (Image) manager.GetObject("DiscountPad.ImageClick");
			this.DiscountPad.ImageClickIndex = 1;
			this.DiscountPad.ImageIndex = 0;
			this.DiscountPad.ImageList = this.ButtonImgList;
			this.DiscountPad.ItemStart = 0;
			this.DiscountPad.Location = new Point(6, 0x30);
			this.DiscountPad.Name = "DiscountPad";
			this.DiscountPad.Padding = 1;
			this.DiscountPad.Red = 1f;
			this.DiscountPad.Row = 4;
			this.DiscountPad.Size = new Size(0x14c, 0xf3);
			this.DiscountPad.TabIndex = 40;
			this.DiscountPad.PadClick += new ButtonListPadEventHandler(this.DiscountPad_PadClick);
			this.DiscountPad.PageChange += new ButtonListPadEventHandler(this.DiscountPad_PageChange);
			this.BtnBack.BackColor = Color.Transparent;
			this.BtnBack.Blue = 2f;
			this.BtnBack.Cursor = Cursors.Hand;
			this.BtnBack.Green = 2f;
			this.BtnBack.ImageClick = (Image) manager.GetObject("BtnBack.ImageClick");
			this.BtnBack.ImageClickIndex = 1;
			this.BtnBack.ImageIndex = 0;
			this.BtnBack.ImageList = this.ButtonImgList;
			this.BtnBack.IsLock = false;
			this.BtnBack.Location = new Point(0x1a8, 0x40);
			this.BtnBack.Name = "BtnBack";
			this.BtnBack.ObjectValue = null;
			this.BtnBack.Red = 1f;
			this.BtnBack.Size = new Size(110, 60);
			this.BtnBack.TabIndex = 30;
			this.BtnBack.Text = "Back";
			this.BtnBack.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnBack.Click += new EventHandler(this.BtnBack_Click);
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
			this.ListOrderItem.ItemWidth = 200;
			this.ListOrderItem.Location = new Point(8, 0x80);
			this.ListOrderItem.Name = "ListOrderItem";
			this.ListOrderItem.Row = 14;
			this.ListOrderItem.SelectedIndex = 0;
			this.ListOrderItem.Size = new Size(200, 560);
			this.ListOrderItem.TabIndex = 0x1f;
			this.ListOrderItem.ItemClick += new smartRestaurant.Controls.ItemsList.ItemsListEventHandler(this.ListOrderItem_ItemClick);
			this.ListOrderCount.Alignment = ContentAlignment.MiddleCenter;
			this.ListOrderCount.AutoRefresh = true;
			this.ListOrderCount.BackAlterColor = Color.FromArgb(0xc0, 0xff, 0xff);
			this.ListOrderCount.BackHeaderColor = Color.FromArgb(0xff, 0xc0, 0x80);
			this.ListOrderCount.BackHeaderSelectedColor = Color.FromArgb(0xc0, 0, 0);
			this.ListOrderCount.BackNormalColor = Color.White;
			this.ListOrderCount.BackSelectedColor = Color.Blue;
			this.ListOrderCount.BindList1 = this.ListOrderItem;
			this.ListOrderCount.BindList2 = this.ListOrderItemPrice;
			this.ListOrderCount.Border = 0;
			this.ListOrderCount.Font = new Font("Tahoma", 12f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.ListOrderCount.ForeAlterColor = Color.Black;
			this.ListOrderCount.ForeHeaderColor = Color.Black;
			this.ListOrderCount.ForeHeaderSelectedColor = Color.White;
			this.ListOrderCount.ForeNormalColor = Color.Black;
			this.ListOrderCount.ForeSelectedColor = Color.White;
			this.ListOrderCount.ItemHeight = 40;
			this.ListOrderCount.ItemWidth = 40;
			this.ListOrderCount.Location = new Point(0xd0, 0x80);
			this.ListOrderCount.Name = "ListOrderCount";
			this.ListOrderCount.Row = 14;
			this.ListOrderCount.SelectedIndex = 0;
			this.ListOrderCount.Size = new Size(40, 560);
			this.ListOrderCount.TabIndex = 0x25;
			this.ListOrderCount.ItemClick += new smartRestaurant.Controls.ItemsList.ItemsListEventHandler(this.ListOrderItem_ItemClick);
			this.ListOrderItemPrice.Alignment = ContentAlignment.MiddleRight;
			this.ListOrderItemPrice.AutoRefresh = true;
			this.ListOrderItemPrice.BackAlterColor = Color.FromArgb(0xc0, 0xff, 0xff);
			this.ListOrderItemPrice.BackHeaderColor = Color.FromArgb(0xff, 0xc0, 0x80);
			this.ListOrderItemPrice.BackHeaderSelectedColor = Color.FromArgb(0xc0, 0, 0);
			this.ListOrderItemPrice.BackNormalColor = Color.White;
			this.ListOrderItemPrice.BackSelectedColor = Color.Blue;
			this.ListOrderItemPrice.BindList1 = this.ListOrderCount;
			this.ListOrderItemPrice.BindList2 = null;
			this.ListOrderItemPrice.Border = 0;
			this.ListOrderItemPrice.Font = new Font("Tahoma", 12f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.ListOrderItemPrice.ForeAlterColor = Color.Black;
			this.ListOrderItemPrice.ForeHeaderColor = Color.Black;
			this.ListOrderItemPrice.ForeHeaderSelectedColor = Color.White;
			this.ListOrderItemPrice.ForeNormalColor = Color.Black;
			this.ListOrderItemPrice.ForeSelectedColor = Color.White;
			this.ListOrderItemPrice.ItemHeight = 40;
			this.ListOrderItemPrice.ItemWidth = 80;
			this.ListOrderItemPrice.Location = new Point(0xf8, 0x80);
			this.ListOrderItemPrice.Name = "ListOrderItemPrice";
			this.ListOrderItemPrice.Row = 14;
			this.ListOrderItemPrice.SelectedIndex = 0;
			this.ListOrderItemPrice.Size = new Size(80, 560);
			this.ListOrderItemPrice.TabIndex = 0x21;
			this.ListOrderItemPrice.ItemClick += new smartRestaurant.Controls.ItemsList.ItemsListEventHandler(this.ListOrderItem_ItemClick);
			this.LblPageID.BackColor = Color.Transparent;
			this.LblPageID.Font = new Font("Tahoma", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.LblPageID.ForeColor = Color.FromArgb(0x67, 0x8a, 0xc6);
			this.LblPageID.Location = new Point(0x318, 0x2f0);
			this.LblPageID.Name = "LblPageID";
			this.LblPageID.Size = new Size(0xd8, 0x17);
			this.LblPageID.TabIndex = 0x20;
			this.LblPageID.Text = "STCB011";
			this.LblPageID.TextAlign = ContentAlignment.TopRight;
			this.LblCopyright.BackColor = Color.Transparent;
			this.LblCopyright.Font = new Font("Tahoma", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.LblCopyright.ForeColor = Color.FromArgb(0x67, 0x8a, 0xc6);
			this.LblCopyright.Location = new Point(8, 0x2f0);
			this.LblCopyright.Name = "LblCopyright";
			this.LblCopyright.Size = new Size(280, 0x10);
			this.LblCopyright.TabIndex = 0x24;
			this.LblCopyright.Text = "Copyright (c) 2004. All rights reserved.";
			this.BtnKBInvoiceNote.BackColor = Color.Transparent;
			this.BtnKBInvoiceNote.Blue = 1f;
			this.BtnKBInvoiceNote.Cursor = Cursors.Hand;
			this.BtnKBInvoiceNote.Font = new Font("Tahoma", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.BtnKBInvoiceNote.Green = 1f;
			this.BtnKBInvoiceNote.ImageClick = (Image) manager.GetObject("BtnKBInvoiceNote.ImageClick");
			this.BtnKBInvoiceNote.ImageClickIndex = 3;
			this.BtnKBInvoiceNote.ImageIndex = 2;
			this.BtnKBInvoiceNote.ImageList = this.CalculatorImgList;
			this.BtnKBInvoiceNote.IsLock = false;
			this.BtnKBInvoiceNote.Location = new Point(0x128, 0x158);
			this.BtnKBInvoiceNote.Name = "BtnKBInvoiceNote";
			this.BtnKBInvoiceNote.ObjectValue = null;
			this.BtnKBInvoiceNote.Red = 1f;
			this.BtnKBInvoiceNote.Size = new Size(40, 40);
			this.BtnKBInvoiceNote.TabIndex = 0x2e;
			this.BtnKBInvoiceNote.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnKBInvoiceNote.Click += new EventHandler(this.BtnKBInvoiceNote_Click);
			this.FieldInvoiceNote.Anchor = AnchorStyles.Left;
			this.FieldInvoiceNote.BackColor = Color.White;
			this.FieldInvoiceNote.BorderStyle = BorderStyle.FixedSingle;
			this.FieldInvoiceNote.Font = new Font("Tahoma", 27f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.FieldInvoiceNote.Location = new Point(8, 0x158);
			this.FieldInvoiceNote.Name = "FieldInvoiceNote";
			this.FieldInvoiceNote.Size = new Size(0x120, 40);
			this.FieldInvoiceNote.TabIndex = 0x2d;
			this.FieldInvoiceNote.Text = "";
			this.FieldInvoiceNote.TextChanged += new EventHandler(this.FieldInvoiceNote_TextChanged);
			this.FieldInvoiceNote.Leave += new EventHandler(this.FieldInvoiceNote_Leave);
			this.FieldInvoiceNote.Enter += new EventHandler(this.FieldInvoiceNote_Enter);
			this.AutoScaleBaseSize = new Size(6, 15);
			base.ClientSize = new Size(0x3fc, 0x2fc);
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
			if (e.Item.Value is OrderBill)
			{
				this.selectedBill = (OrderBill) e.Item.Value;
				this.selectedItem = null;
				this.UpdateOrderGrid();
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
				this.UpdateOrderGrid();
			}
		}

		private void LoadDiscountSelected()
		{
			if (this.receipt.UseDiscount.Count > 0)
			{
				for (int i = 0; i < this.receipt.UseDiscount.Count; i++)
				{
					for (int j = 0; j < Receipt.Discounts.Length; j++)
					{
						if (((Discount) this.receipt.UseDiscount[i]).promotionID == Receipt.Discounts[j].promotionID)
						{
							this.discountSelected.Add(j);
							break;
						}
					}
				}
			}
		}

		private void NumberKeyPad_PadClick(object sender, NumberPadEventArgs e)
		{
			if (this.inputState != 0)
			{
				if (e.IsNumeric)
				{
					if (this.inputState == 2)
					{
						try
						{
							this.inputValue = ((double.Parse(this.inputValue) * 10.0) + (0.01 * e.Number)).ToString("N");
						}
						catch (Exception)
						{
							this.inputValue = "0.0" + e.Number.ToString();
						}
					}
					else
					{
						this.inputValue = this.inputValue + e.Number.ToString();
					}
					this.UpdateMonitor();
				}
				else if (e.IsCancel)
				{
					if (this.inputState == 2)
					{
						try
						{
							double num2 = Math.Floor(double.Parse(this.inputValue) * 10.0) / 100.0;
							this.inputValue = num2.ToString("N");
							if (num2 == 0.0)
							{
								this.StartInputNone();
								this.receipt.PaymentMethod = null;
								this.UpdatePaymentTypeList();
							}
							else
							{
								this.UpdateMonitor();
							}
						}
						catch (Exception)
						{
							this.StartInputNone();
							this.receipt.PaymentMethod = null;
							this.UpdatePaymentTypeList();
						}
						this.receipt.PaymentMethod = null;
					}
					else if (this.inputValue.Length > 1)
					{
						this.inputValue = this.inputValue.Substring(0, this.inputValue.Length - 1);
						this.UpdateMonitor();
					}
					else
					{
						this.StartInputNone();
					}
				}
				else if (e.IsEnter)
				{
					this.inputValue = this.inputValue.Replace(",", "");
					if (this.inputValue != "")
					{
						int num3;
						double num4;
						try
						{
							num3 = int.Parse(this.inputValue);
						}
						catch (Exception)
						{
							num3 = 0;
						}
						try
						{
							num4 = double.Parse(this.inputValue);
						}
						catch (Exception)
						{
							num4 = 0.0;
						}
						switch (this.inputState)
						{
							case 2:
								this.receipt.PayValue = num4;
								this.receipt.SetPaymentMethod(this.receipt.PaymentMethod, this.receipt.PayValue);
								break;

							case 3:
								this.receipt.PointAmount = num3;
								break;
						}
					}
					this.receipt.PaymentMethod = null;
					this.UpdatePaymentTypeList();
					this.StartInputNone();
					this.UpdateSummary();
				}
			}
		}

		private void PaymentTypePad_PadClick(object sender, ButtonListPadEventArgs e)
		{
			if (e.Value != null)
			{
				this.receipt.PaymentMethod = Receipt.SearchPaymentMethodByID(int.Parse(e.Value));
				int index = this.receipt.PaymentMethodList.IndexOf(this.receipt.PaymentMethod);
				if (index >= 0)
				{
					this.receipt.PayValue = (double) this.receipt.PayValueList[index];
				}
				else
				{
					this.receipt.PayValue = 0.0;
				}
				this.StartInputPayValue();
				this.UpdatePaymentTypeList();
				this.UpdateSummary();
			}
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
			if (discounts != null)
			{
				this.DiscountPad.AutoRefresh = false;
				if (this.DiscountPad.Items.Count == 0)
				{
					for (int j = 0; j < discounts.Length; j++)
					{
						ButtonItem item = new ButtonItem(discounts[j].description, discounts[j].promotionID.ToString());
						this.DiscountPad.Items.Add(item);
					}
				}
				this.DiscountPad.Red = 1f;
				this.DiscountPad.Green = 1f;
				this.DiscountPad.Blue = 1f;
				for (int i = 0; i < this.discountSelected.Count; i++)
				{
					int index = (int) this.discountSelected[i];
					int position = this.DiscountPad.GetPosition(index);
					if (position > -1)
					{
						this.DiscountPad.SetMatrix(position, 1f, 2f, 1f);
					}
				}
				this.DiscountPad.AutoRefresh = true;
			}
		}

		private void UpdateFlowButton()
		{
			if (this.receipt.IsCompleted)
			{
				this.BtnPay.Enabled = true;
			}
			else
			{
				this.BtnPay.Enabled = false;
			}
		}

		public override void UpdateForm()
		{
			int userID;
			this.selectedItem = null;
			this.menuTypes = MenuManagement.MenuTypes;
			this.menuOptions = MenuManagement.MenuOptions;
			if (((MainForm) base.MdiParent).User != null)
			{
				userID = ((MainForm) base.MdiParent).User.UserID;
			}
			else
			{
				userID = this.selectedBill.EmployeeID;
			}
			this.receipt = new Receipt(this.selectedBill, userID);
			this.DiscountPad.Items.Clear();
			this.discountSelected.Clear();
			this.paymentSelected.Clear();
			this.LoadDiscountSelected();
			this.LblPageID.Text = "Employee ID:" + userID.ToString() + " | STCB011";
			if (AppParameter.IsDemo())
			{
				this.LblTotalChange.Text = "Change";
				this.LblGuest.Text = "Guest";
			}
			else
			{
				this.LblTotalChange.Text = "Tip";
				this.LblGuest.Text = "Seat";
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
				case 1:
					this.FieldInputType.Text = "Payment";
					break;

				case 2:
					this.FieldInputType.Text = "Pay";
					flag = true;
					break;

				case 3:
					this.FieldInputType.Text = "Point";
					flag = true;
					break;

				case 4:
					this.FieldInputType.Text = "Coupon";
					break;

				case 5:
					this.FieldInputType.Text = "Gift Voucher";
					break;

				default:
					this.inputState = 0;
					this.FieldInputType.Text = "";
					break;
			}
			if (flag && (this.inputValue == ""))
			{
				this.FieldCurrentInput.ForeColor = Color.Yellow;
				if (this.inputState == 2)
				{
					this.FieldCurrentInput.Text = this.receipt.PayValue.ToString("N");
				}
				else if (this.inputState == 3)
				{
					this.FieldCurrentInput.Text = this.receipt.PointAmount.ToString();
				}
			}
			else
			{
				this.FieldCurrentInput.ForeColor = Color.Cyan;
				this.FieldCurrentInput.Text = this.inputValue;
			}
		}

		private void UpdateOrderButton()
		{
			if ((this.selectedItem != null) && (this.selectedBill.CloseBillDate == AppParameter.MinDateTime))
			{
				this.BtnCancel.Enabled = !OrderManagement.IsCancel(this.selectedItem);
				this.BtnUndo.Enabled = OrderManagement.IsCancel(this.selectedItem);
			}
			else
			{
				this.BtnCancel.Enabled = false;
				this.BtnUndo.Enabled = false;
			}
			this.BtnUp.Enabled = this.ListOrderItem.CanUp;
			this.BtnDown.Enabled = this.ListOrderItem.CanDown;
		}

		private void UpdateOrderGrid()
		{
			StringBuilder builder = new StringBuilder();
			this.ListOrderItem.AutoRefresh = false;
			this.ListOrderCount.AutoRefresh = false;
			this.ListOrderItemPrice.AutoRefresh = false;
			this.ListOrderItem.Items.Clear();
			this.ListOrderCount.Items.Clear();
			this.ListOrderItemPrice.Items.Clear();
			builder.Length = 0;
			if (AppParameter.IsDemo())
			{
				builder.Append("Bill #");
			}
			else
			{
				builder.Append("Seat #");
			}
			builder.Append(this.selectedBill.BillID);
			DataItem item = new DataItem(builder.ToString(), this.selectedBill, true);
			this.ListOrderItem.Items.Add(item);
			item = new DataItem("", this.selectedBill, true);
			this.ListOrderCount.Items.Add(item);
			item = new DataItem("", this.selectedBill, true);
			this.ListOrderItemPrice.Items.Add(item);
			if (this.selectedItem == null)
			{
				this.ListOrderItem.SelectedIndex = this.ListOrderItem.Items.Count - 1;
				this.ListOrderCount.SelectedIndex = this.ListOrderCount.Items.Count - 1;
				this.ListOrderItemPrice.SelectedIndex = this.ListOrderItemPrice.Items.Count - 1;
			}
			OrderBillItem[] items = this.selectedBill.Items;
			if (items != null)
			{
				for (int i = 0; i < items.Length; i++)
				{
					item = new DataItem(OrderManagement.OrderBillItemDisplayString(items[i]), items[i], false);
					if (OrderManagement.IsCancel(items[i]))
					{
						item.Strike = true;
					}
					this.ListOrderItem.Items.Add(item);
					item = new DataItem(items[i].Unit.ToString(), items[i], false);
					if (OrderManagement.IsCancel(items[i]))
					{
						item.Strike = true;
					}
					this.ListOrderCount.Items.Add(item);
					item = new DataItem(MenuManagement.GetMenuItemFromID(items[i].MenuID).Price.ToString("N"), items[i], false);
					if (OrderManagement.IsCancel(items[i]))
					{
						item.Strike = true;
					}
					this.ListOrderItemPrice.Items.Add(item);
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
			}
			else
			{
				this.BtnPayClear.Enabled = false;
				this.BtnFillPay.Enabled = false;
			}
		}

		private void UpdatePaymentTypeList()
		{
			if (this.paymentMethods != null)
			{
				ButtonItem item;
				this.PaymentTypePad.AutoRefresh = false;
				if (this.PaymentTypePad.Items.Count == 0)
				{
					for (int j = 0; j < this.paymentMethods.Length; j++)
					{
						item = new ButtonItem(this.paymentMethods[j].paymentMethodName, this.paymentMethods[j].paymentMethodID.ToString());
						this.PaymentTypePad.Items.Add(item);
					}
				}
				for (int i = 0; i < this.paymentMethods.Length; i++)
				{
					item = (ButtonItem) this.PaymentTypePad.Items[i];
					int index = this.receipt.PaymentMethodList.IndexOf(this.paymentMethods[i]);
					if (this.paymentMethods[i] == this.receipt.PaymentMethod)
					{
						this.PaymentTypePad.SetMatrix(i, 1f, 1.75f, 1.75f);
						if (index >= 0)
						{
							double num4 = (double) this.receipt.PayValueList[index];
							item.Text = this.paymentMethods[i].paymentMethodName + "\n" + num4.ToString("N");
						}
					}
					else if (index >= 0)
					{
						this.PaymentTypePad.SetMatrix(i, 1f, 2f, 2f);
						item.Text = this.paymentMethods[i].paymentMethodName + "\n" + ((double) this.receipt.PayValueList[index]).ToString("N");
					}
					else
					{
						this.PaymentTypePad.SetMatrix(i, 1f, 1f, 1f);
						item.Text = this.paymentMethods[i].paymentMethodName;
					}
				}
				this.PaymentTypePad.AutoRefresh = true;
				this.UpdatePaymentButtion();
			}
		}

		private void UpdateSummary()
		{
			this.receipt.Compute();
			this.FieldAmountDue.Text = this.receipt.AmountDue.ToString("N");
			this.FieldTax1.Visible = (this.LblTax1.Text != "") || (this.receipt.Tax1 > 0.0);
			this.FieldTax2.Visible = (this.LblTax2.Text != "") || (this.receipt.Tax2 > 0.0);
			this.FieldTax1.Text = this.receipt.Tax1.ToString("N");
			this.FieldTax2.Text = this.receipt.Tax2.ToString("N");
			this.FieldTotalDiscount.Text = this.receipt.TotalDiscount.ToString("N");
			this.FieldTotalDue.Text = this.receipt.TotalDue.ToString("N");
			this.FieldTotalReceive.Text = this.receipt.TotalReceive.ToString("N");
			this.FieldChange.Text = this.receipt.Change.ToString("N");
			if (this.receipt.TotalReceive < this.receipt.TotalDue)
			{
				this.FieldTotalReceive.ForeColor = Color.Red;
			}
			else
			{
				this.FieldTotalReceive.ForeColor = Color.Black;
			}
			if (this.receipt.Change > 0.0)
			{
				this.FieldChange.ForeColor = Color.Blue;
			}
			else
			{
				this.FieldChange.ForeColor = Color.Black;
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

		// Properties
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
				this.billNumber = this.orderInfo.Bills.Length;
			}
		}

		public OrderBill OrderBill
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
	}

 

}
