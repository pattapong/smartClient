using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using smartRestaurant.Data;
using smartRestaurant.Utils;
using System.Resources;
using smartRestaurant.Controls;

namespace smartRestaurant.Business
{
	/// <summary>
	/// Summary description for InvoiceViewerForm.
	/// </summary>
	public class InvoiceViewerForm : Form
	{
		// Fields
		private smartRestaurant.Controls.ImageButton BtnOk;
		private smartRestaurant.Controls.ImageButton BtnReprint;
		private ImageList ButtonImgList;
		private IContainer components;
		private Label FieldAmountDue;
		private Label FieldChange;
		private Label FieldTax1;
		private Label FieldTax2;
		private Label FieldTotalDiscount;
		private Label FieldTotalDue;
		private Label FieldTotalReceive;
		private smartRestaurant.Controls.GroupPanel groupPanel1;
		private static InvoiceViewerForm instance = null;
		private Label LblAmountDue;
		private Label LblTax1;
		private Label LblTax2;
		private Label LblTotalChange;
		private Label LblTotalDiscount;
		private Label LblTotalDue;
		private Label LblTotalReceive;
		private Receipt receipt;

		// Methods
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
			if (disposing && (this.components != null))
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			this.components = new Container();
			ResourceManager manager = new ResourceManager(typeof(InvoiceViewerForm));
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
			this.ButtonImgList.ImageSize = new Size(110, 60);
			this.ButtonImgList.ImageStream = (ImageListStreamer) manager.GetObject("ButtonImgList.ImageStream");
			this.ButtonImgList.TransparentColor = Color.Transparent;
			this.BtnOk.BackColor = Color.Transparent;
			this.BtnOk.Blue = 1f;
			this.BtnOk.Cursor = Cursors.Hand;
			this.BtnOk.Font = new Font("Tahoma", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0xde);
			this.BtnOk.Green = 1f;
			this.BtnOk.Image = (Bitmap) manager.GetObject("BtnOk.Image");
			this.BtnOk.ImageClick = (Bitmap) manager.GetObject("BtnOk.ImageClick");
			this.BtnOk.ImageClickIndex = 0;
			this.BtnOk.Location = new Point(0xd8, 0x158);
			this.BtnOk.Name = "BtnOk";
			this.BtnOk.ObjectValue = null;
			this.BtnOk.Red = 1f;
			this.BtnOk.Size = new Size(110, 60);
			this.BtnOk.TabIndex = 5;
			this.BtnOk.Text = "Ok";
			this.BtnOk.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnOk.Click += new EventHandler(this.BtnOk_Click);
			this.groupPanel1.BackColor = Color.Transparent;
			this.groupPanel1.Caption = null;
			this.groupPanel1.Controls.AddRange(new Control[] { this.FieldChange, this.FieldTotalReceive, this.FieldTotalDue, this.FieldTotalDiscount, this.FieldTax2, this.FieldTax1, this.FieldAmountDue, this.LblTotalChange, this.LblTotalReceive, this.LblTotalDue, this.LblTotalDiscount, this.LblTax2, this.LblTax1, this.LblAmountDue });
			this.groupPanel1.Font = new Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.groupPanel1.Location = new Point(8, 40);
			this.groupPanel1.Name = "groupPanel1";
			this.groupPanel1.ShowHeader = false;
			this.groupPanel1.Size = new Size(320, 0x128);
			this.groupPanel1.TabIndex = 0x26;
			this.FieldChange.Location = new Point(0x9c, 0xf8);
			this.FieldChange.Name = "FieldChange";
			this.FieldChange.Size = new Size(160, 40);
			this.FieldChange.TabIndex = 0x33;
			this.FieldChange.Text = "0.00";
			this.FieldChange.TextAlign = ContentAlignment.MiddleRight;
			this.FieldTotalReceive.Location = new Point(0x9c, 0xd0);
			this.FieldTotalReceive.Name = "FieldTotalReceive";
			this.FieldTotalReceive.Size = new Size(160, 40);
			this.FieldTotalReceive.TabIndex = 50;
			this.FieldTotalReceive.Text = "0.00";
			this.FieldTotalReceive.TextAlign = ContentAlignment.MiddleRight;
			this.FieldTotalDue.Font = new Font("Tahoma", 15.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.FieldTotalDue.ForeColor = Color.Green;
			this.FieldTotalDue.Location = new Point(0x9c, 0xa8);
			this.FieldTotalDue.Name = "FieldTotalDue";
			this.FieldTotalDue.Size = new Size(160, 40);
			this.FieldTotalDue.TabIndex = 0x31;
			this.FieldTotalDue.Text = "0.00";
			this.FieldTotalDue.TextAlign = ContentAlignment.MiddleRight;
			this.FieldTotalDiscount.Location = new Point(0x9c, 0x30);
			this.FieldTotalDiscount.Name = "FieldTotalDiscount";
			this.FieldTotalDiscount.Size = new Size(160, 40);
			this.FieldTotalDiscount.TabIndex = 0x30;
			this.FieldTotalDiscount.Text = "0.00";
			this.FieldTotalDiscount.TextAlign = ContentAlignment.MiddleRight;
			this.FieldTax2.Location = new Point(0x9c, 0x80);
			this.FieldTax2.Name = "FieldTax2";
			this.FieldTax2.Size = new Size(160, 40);
			this.FieldTax2.TabIndex = 0x2f;
			this.FieldTax2.Text = "0.00";
			this.FieldTax2.TextAlign = ContentAlignment.MiddleRight;
			this.FieldTax1.Location = new Point(0x9c, 0x58);
			this.FieldTax1.Name = "FieldTax1";
			this.FieldTax1.Size = new Size(160, 40);
			this.FieldTax1.TabIndex = 0x2e;
			this.FieldTax1.Text = "0.00";
			this.FieldTax1.TextAlign = ContentAlignment.MiddleRight;
			this.FieldAmountDue.Location = new Point(0x9c, 8);
			this.FieldAmountDue.Name = "FieldAmountDue";
			this.FieldAmountDue.Size = new Size(160, 40);
			this.FieldAmountDue.TabIndex = 0x2d;
			this.FieldAmountDue.Text = "0.00";
			this.FieldAmountDue.TextAlign = ContentAlignment.MiddleRight;
			this.LblTotalChange.Location = new Point(4, 0xf8);
			this.LblTotalChange.Name = "LblTotalChange";
			this.LblTotalChange.Size = new Size(0x90, 40);
			this.LblTotalChange.TabIndex = 0x2c;
			this.LblTotalChange.Text = "Change";
			this.LblTotalChange.TextAlign = ContentAlignment.MiddleLeft;
			this.LblTotalReceive.Location = new Point(4, 0xd0);
			this.LblTotalReceive.Name = "LblTotalReceive";
			this.LblTotalReceive.Size = new Size(0x90, 40);
			this.LblTotalReceive.TabIndex = 0x2b;
			this.LblTotalReceive.Text = "Total Receive";
			this.LblTotalReceive.TextAlign = ContentAlignment.MiddleLeft;
			this.LblTotalDue.Font = new Font("Tahoma", 15.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.LblTotalDue.ForeColor = Color.Green;
			this.LblTotalDue.Location = new Point(4, 0xa8);
			this.LblTotalDue.Name = "LblTotalDue";
			this.LblTotalDue.Size = new Size(0x90, 40);
			this.LblTotalDue.TabIndex = 0x2a;
			this.LblTotalDue.Text = "Total Due";
			this.LblTotalDue.TextAlign = ContentAlignment.MiddleLeft;
			this.LblTotalDiscount.Location = new Point(4, 0x30);
			this.LblTotalDiscount.Name = "LblTotalDiscount";
			this.LblTotalDiscount.Size = new Size(0x90, 40);
			this.LblTotalDiscount.TabIndex = 0x29;
			this.LblTotalDiscount.Text = "Total Discount";
			this.LblTotalDiscount.TextAlign = ContentAlignment.MiddleLeft;
			this.LblTax2.Location = new Point(4, 0x80);
			this.LblTax2.Name = "LblTax2";
			this.LblTax2.Size = new Size(0x90, 40);
			this.LblTax2.TabIndex = 40;
			this.LblTax2.Text = "Tax2";
			this.LblTax2.TextAlign = ContentAlignment.MiddleLeft;
			this.LblTax1.Location = new Point(4, 0x58);
			this.LblTax1.Name = "LblTax1";
			this.LblTax1.Size = new Size(0x90, 40);
			this.LblTax1.TabIndex = 0x27;
			this.LblTax1.Text = "Tax1";
			this.LblTax1.TextAlign = ContentAlignment.MiddleLeft;
			this.LblAmountDue.Location = new Point(4, 8);
			this.LblAmountDue.Name = "LblAmountDue";
			this.LblAmountDue.Size = new Size(0x90, 40);
			this.LblAmountDue.TabIndex = 0x26;
			this.LblAmountDue.Text = "Amount Due";
			this.LblAmountDue.TextAlign = ContentAlignment.MiddleLeft;
			this.BtnReprint.BackColor = Color.Transparent;
			this.BtnReprint.Blue = 2f;
			this.BtnReprint.Cursor = Cursors.Hand;
			this.BtnReprint.Font = new Font("Tahoma", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0xde);
			this.BtnReprint.Green = 1f;
			this.BtnReprint.Image = (Bitmap) manager.GetObject("BtnReprint.Image");
			this.BtnReprint.ImageClick = (Bitmap) manager.GetObject("BtnReprint.ImageClick");
			this.BtnReprint.ImageClickIndex = 0;
			this.BtnReprint.Location = new Point(8, 0x158);
			this.BtnReprint.Name = "BtnReprint";
			this.BtnReprint.ObjectValue = null;
			this.BtnReprint.Red = 2f;
			this.BtnReprint.Size = new Size(110, 60);
			this.BtnReprint.TabIndex = 0x27;
			this.BtnReprint.Text = "Reprint";
			this.BtnReprint.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnReprint.Click += new EventHandler(this.BtnReprint_Click);
			this.AutoScaleBaseSize = new Size(9, 20);
			this.BackColor = Color.White;
			base.ClientSize = new Size(0x150, 0x198);
			base.Controls.AddRange(new Control[] { this.BtnReprint, this.groupPanel1, this.BtnOk });
			this.Font = new Font("Tahoma", 12f, FontStyle.Bold, GraphicsUnit.Point, 0xde);
			base.FormBorderStyle = FormBorderStyle.None;
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

		public static void Show(int invoiceID)
		{
			if (instance == null)
			{
				instance = new InvoiceViewerForm();
			}
			if (AppParameter.IsDemo())
			{
				instance.LblTotalChange.Text = "Change";
			}
			else
			{
				instance.LblTotalChange.Text = "Tip";
			}
			instance.receipt = new Receipt(invoiceID);
			instance.UpdateSummary();
			instance.ShowDialog();
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
		}
	}
 

}
