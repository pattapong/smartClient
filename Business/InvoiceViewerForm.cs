using smartRestaurant.Controls;
using smartRestaurant.Data;
using smartRestaurant.Utils;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Resources;
using System.Windows.Forms;

namespace smartRestaurant.Business
{
	public class InvoiceViewerForm : Form
	{
		private static InvoiceViewerForm instance;

		private Receipt receipt;

		private ImageList ButtonImgList;

		private ImageButton BtnOk;

		private GroupPanel groupPanel1;

		private Label FieldChange;

		private Label FieldTotalReceive;

		private Label FieldTotalDue;

		private Label FieldTotalDiscount;

		private Label FieldTax2;

		private Label FieldTax1;

		private Label FieldAmountDue;

		private Label LblTotalChange;

		private Label LblTotalReceive;

		private Label LblTotalDue;

		private Label LblTotalDiscount;

		private Label LblTax2;

		private Label LblTax1;

		private Label LblAmountDue;

		private IContainer components;

		private ImageButton BtnReprint;

		static InvoiceViewerForm()
		{
			InvoiceViewerForm.instance = null;
		}

		public InvoiceViewerForm()
		{
			this.InitializeComponent();
		}

		private void BtnOk_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		private void BtnReprint_Click(object sender, EventArgs e)
		{
			WaitingForm.Show("Print Receipt");
			base.Enabled = false;
			this.receipt.PrintInvoice();
			base.Enabled = true;
			WaitingForm.HideForm();
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
			ResourceManager resourceManager = new ResourceManager(typeof(InvoiceViewerForm));
			this.ButtonImgList = new ImageList(this.components);
			this.BtnOk = new ImageButton();
			this.groupPanel1 = new GroupPanel();
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
			this.BtnReprint = new ImageButton();
			this.groupPanel1.SuspendLayout();
			base.SuspendLayout();
			this.ButtonImgList.ColorDepth = ColorDepth.Depth32Bit;
			this.ButtonImgList.ImageSize = new System.Drawing.Size(110, 60);
			this.ButtonImgList.ImageStream = (ImageListStreamer)resourceManager.GetObject("ButtonImgList.ImageStream");
			this.ButtonImgList.TransparentColor = Color.Transparent;
			this.BtnOk.BackColor = Color.Transparent;
			this.BtnOk.Blue = 1f;
			this.BtnOk.Cursor = Cursors.Hand;
			this.BtnOk.Font = new System.Drawing.Font("Tahoma", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 222);
			this.BtnOk.Green = 1f;
			this.BtnOk.Image = (Bitmap)resourceManager.GetObject("BtnOk.Image");
			this.BtnOk.ImageClick = (Bitmap)resourceManager.GetObject("BtnOk.ImageClick");
			this.BtnOk.ImageClickIndex = 0;
			this.BtnOk.Location = new Point(216, 344);
			this.BtnOk.Name = "BtnOk";
			this.BtnOk.ObjectValue = null;
			this.BtnOk.Red = 1f;
			this.BtnOk.Size = new System.Drawing.Size(110, 60);
			this.BtnOk.TabIndex = 5;
			this.BtnOk.Text = "Ok";
			this.BtnOk.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnOk.Click += new EventHandler(this.BtnOk_Click);
			this.groupPanel1.BackColor = Color.Transparent;
			this.groupPanel1.Caption = null;
			Control.ControlCollection controls = this.groupPanel1.Controls;
			Control[] fieldChange = new Control[] { this.FieldChange, this.FieldTotalReceive, this.FieldTotalDue, this.FieldTotalDiscount, this.FieldTax2, this.FieldTax1, this.FieldAmountDue, this.LblTotalChange, this.LblTotalReceive, this.LblTotalDue, this.LblTotalDiscount, this.LblTax2, this.LblTax1, this.LblAmountDue };
			controls.AddRange(fieldChange);
			this.groupPanel1.Font = new System.Drawing.Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.groupPanel1.Location = new Point(8, 40);
			this.groupPanel1.Name = "groupPanel1";
			this.groupPanel1.ShowHeader = false;
			this.groupPanel1.Size = new System.Drawing.Size(320, 296);
			this.groupPanel1.TabIndex = 38;
			this.FieldChange.Location = new Point(156, 248);
			this.FieldChange.Name = "FieldChange";
			this.FieldChange.Size = new System.Drawing.Size(160, 40);
			this.FieldChange.TabIndex = 51;
			this.FieldChange.Text = "0.00";
			this.FieldChange.TextAlign = ContentAlignment.MiddleRight;
			this.FieldTotalReceive.Location = new Point(156, 208);
			this.FieldTotalReceive.Name = "FieldTotalReceive";
			this.FieldTotalReceive.Size = new System.Drawing.Size(160, 40);
			this.FieldTotalReceive.TabIndex = 50;
			this.FieldTotalReceive.Text = "0.00";
			this.FieldTotalReceive.TextAlign = ContentAlignment.MiddleRight;
			this.FieldTotalDue.Font = new System.Drawing.Font("Tahoma", 15.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.FieldTotalDue.ForeColor = Color.Green;
			this.FieldTotalDue.Location = new Point(156, 168);
			this.FieldTotalDue.Name = "FieldTotalDue";
			this.FieldTotalDue.Size = new System.Drawing.Size(160, 40);
			this.FieldTotalDue.TabIndex = 49;
			this.FieldTotalDue.Text = "0.00";
			this.FieldTotalDue.TextAlign = ContentAlignment.MiddleRight;
			this.FieldTotalDiscount.Location = new Point(156, 48);
			this.FieldTotalDiscount.Name = "FieldTotalDiscount";
			this.FieldTotalDiscount.Size = new System.Drawing.Size(160, 40);
			this.FieldTotalDiscount.TabIndex = 48;
			this.FieldTotalDiscount.Text = "0.00";
			this.FieldTotalDiscount.TextAlign = ContentAlignment.MiddleRight;
			this.FieldTax2.Location = new Point(156, 128);
			this.FieldTax2.Name = "FieldTax2";
			this.FieldTax2.Size = new System.Drawing.Size(160, 40);
			this.FieldTax2.TabIndex = 47;
			this.FieldTax2.Text = "0.00";
			this.FieldTax2.TextAlign = ContentAlignment.MiddleRight;
			this.FieldTax1.Location = new Point(156, 88);
			this.FieldTax1.Name = "FieldTax1";
			this.FieldTax1.Size = new System.Drawing.Size(160, 40);
			this.FieldTax1.TabIndex = 46;
			this.FieldTax1.Text = "0.00";
			this.FieldTax1.TextAlign = ContentAlignment.MiddleRight;
			this.FieldAmountDue.Location = new Point(156, 8);
			this.FieldAmountDue.Name = "FieldAmountDue";
			this.FieldAmountDue.Size = new System.Drawing.Size(160, 40);
			this.FieldAmountDue.TabIndex = 45;
			this.FieldAmountDue.Text = "0.00";
			this.FieldAmountDue.TextAlign = ContentAlignment.MiddleRight;
			this.LblTotalChange.Location = new Point(4, 248);
			this.LblTotalChange.Name = "LblTotalChange";
			this.LblTotalChange.Size = new System.Drawing.Size(144, 40);
			this.LblTotalChange.TabIndex = 44;
			this.LblTotalChange.Text = "Change";
			this.LblTotalChange.TextAlign = ContentAlignment.MiddleLeft;
			this.LblTotalReceive.Location = new Point(4, 208);
			this.LblTotalReceive.Name = "LblTotalReceive";
			this.LblTotalReceive.Size = new System.Drawing.Size(144, 40);
			this.LblTotalReceive.TabIndex = 43;
			this.LblTotalReceive.Text = "Total Receive";
			this.LblTotalReceive.TextAlign = ContentAlignment.MiddleLeft;
			this.LblTotalDue.Font = new System.Drawing.Font("Tahoma", 15.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.LblTotalDue.ForeColor = Color.Green;
			this.LblTotalDue.Location = new Point(4, 168);
			this.LblTotalDue.Name = "LblTotalDue";
			this.LblTotalDue.Size = new System.Drawing.Size(144, 40);
			this.LblTotalDue.TabIndex = 42;
			this.LblTotalDue.Text = "Total Due";
			this.LblTotalDue.TextAlign = ContentAlignment.MiddleLeft;
			this.LblTotalDiscount.Location = new Point(4, 48);
			this.LblTotalDiscount.Name = "LblTotalDiscount";
			this.LblTotalDiscount.Size = new System.Drawing.Size(144, 40);
			this.LblTotalDiscount.TabIndex = 41;
			this.LblTotalDiscount.Text = "Total Discount";
			this.LblTotalDiscount.TextAlign = ContentAlignment.MiddleLeft;
			this.LblTax2.Location = new Point(4, 128);
			this.LblTax2.Name = "LblTax2";
			this.LblTax2.Size = new System.Drawing.Size(144, 40);
			this.LblTax2.TabIndex = 40;
			this.LblTax2.Text = "Tax2";
			this.LblTax2.TextAlign = ContentAlignment.MiddleLeft;
			this.LblTax1.Location = new Point(4, 88);
			this.LblTax1.Name = "LblTax1";
			this.LblTax1.Size = new System.Drawing.Size(144, 40);
			this.LblTax1.TabIndex = 39;
			this.LblTax1.Text = "Tax1";
			this.LblTax1.TextAlign = ContentAlignment.MiddleLeft;
			this.LblAmountDue.Location = new Point(4, 8);
			this.LblAmountDue.Name = "LblAmountDue";
			this.LblAmountDue.Size = new System.Drawing.Size(144, 40);
			this.LblAmountDue.TabIndex = 38;
			this.LblAmountDue.Text = "Amount Due";
			this.LblAmountDue.TextAlign = ContentAlignment.MiddleLeft;
			this.BtnReprint.BackColor = Color.Transparent;
			this.BtnReprint.Blue = 2f;
			this.BtnReprint.Cursor = Cursors.Hand;
			this.BtnReprint.Font = new System.Drawing.Font("Tahoma", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 222);
			this.BtnReprint.Green = 1f;
			this.BtnReprint.Image = (Bitmap)resourceManager.GetObject("BtnReprint.Image");
			this.BtnReprint.ImageClick = (Bitmap)resourceManager.GetObject("BtnReprint.ImageClick");
			this.BtnReprint.ImageClickIndex = 0;
			this.BtnReprint.Location = new Point(8, 344);
			this.BtnReprint.Name = "BtnReprint";
			this.BtnReprint.ObjectValue = null;
			this.BtnReprint.Red = 2f;
			this.BtnReprint.Size = new System.Drawing.Size(110, 60);
			this.BtnReprint.TabIndex = 39;
			this.BtnReprint.Text = "Reprint";
			this.BtnReprint.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnReprint.Click += new EventHandler(this.BtnReprint_Click);
			this.AutoScaleBaseSize = new System.Drawing.Size(9, 20);
			this.BackColor = Color.White;
			base.ClientSize = new System.Drawing.Size(336, 408);
			Control.ControlCollection controlCollections = base.Controls;
			fieldChange = new Control[] { this.BtnReprint, this.groupPanel1, this.BtnOk };
			controlCollections.AddRange(fieldChange);
			this.Font = new System.Drawing.Font("Tahoma", 12f, FontStyle.Bold, GraphicsUnit.Point, 222);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			base.Name = "InvoiceViewerForm";
			base.ShowInTaskbar = false;
			base.StartPosition = FormStartPosition.CenterParent;
			this.Text = "Invoice Viewer";
			this.groupPanel1.ResumeLayout(false);
			base.ResumeLayout(false);
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

		public static void Show(int invoiceID)
		{
			if (InvoiceViewerForm.instance == null)
			{
				InvoiceViewerForm.instance = new InvoiceViewerForm();
			}
			if (!AppParameter.IsDemo())
			{
				InvoiceViewerForm.instance.LblTotalChange.Text = "Tip";
			}
			else
			{
				InvoiceViewerForm.instance.LblTotalChange.Text = "Change";
			}
			InvoiceViewerForm.instance.receipt = new Receipt(invoiceID);
			InvoiceViewerForm.instance.UpdateSummary();
			InvoiceViewerForm.instance.ShowDialog();
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
			if (this.receipt.Change > 0)
			{
				this.FieldChange.ForeColor = Color.Blue;
				return;
			}
			this.FieldChange.ForeColor = Color.Black;
		}
	}
}