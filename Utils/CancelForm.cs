using smartRestaurant.Controls;
using smartRestaurant.OrderService;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Resources;
using System.Windows.Forms;

namespace smartRestaurant.Utils
{
	public class CancelForm : Form
	{
		private static CancelForm instance;

		private static CancelReason[] reasons;

		private static bool status;

		private ImageButton BtnOk;

		private ComboBox LstCancelReason;

		private Label LblCancalReason;

		private ImageButton BtnCancel;

		private System.ComponentModel.Container components = null;

		static CancelForm()
		{
			CancelForm.instance = null;
			CancelForm.reasons = null;
			CancelForm.status = false;
		}

		public CancelForm()
		{
			this.InitializeComponent();
			if (CancelForm.reasons == null)
			{
				CancelForm.reasons = (new smartRestaurant.OrderService.OrderService()).GetCancelReason();
			}
		}

		private void BtnCancel_Click(object sender, EventArgs e)
		{
			CancelForm.status = false;
			base.Close();
		}

		private void BtnOk_Click(object sender, EventArgs e)
		{
			CancelForm.status = true;
			base.Close();
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
			ResourceManager resourceManager = new ResourceManager(typeof(CancelForm));
			this.LstCancelReason = new ComboBox();
			this.LblCancalReason = new Label();
			this.BtnOk = new ImageButton();
			this.BtnCancel = new ImageButton();
			base.SuspendLayout();
			this.LstCancelReason.Location = new Point(160, 48);
			this.LstCancelReason.Name = "LstCancelReason";
			this.LstCancelReason.Size = new System.Drawing.Size(256, 31);
			this.LstCancelReason.TabIndex = 0;
			this.LblCancalReason.Location = new Point(16, 51);
			this.LblCancalReason.Name = "LblCancalReason";
			this.LblCancalReason.Size = new System.Drawing.Size(136, 23);
			this.LblCancalReason.TabIndex = 1;
			this.LblCancalReason.Text = "Cancel Reason";
			this.BtnOk.BackColor = Color.Transparent;
			this.BtnOk.Blue = 1f;
			this.BtnOk.Cursor = Cursors.Hand;
			this.BtnOk.Font = new System.Drawing.Font("Tahoma", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 222);
			this.BtnOk.Green = 1f;
			this.BtnOk.Image = (Image)resourceManager.GetObject("BtnOk.Image");
			this.BtnOk.ImageClick = (Image)resourceManager.GetObject("BtnOk.ImageClick");
			this.BtnOk.ImageClickIndex = 0;
			this.BtnOk.IsLock = false;
			this.BtnOk.Location = new Point(184, 96);
			this.BtnOk.Name = "BtnOk";
			this.BtnOk.ObjectValue = null;
			this.BtnOk.Red = 1f;
			this.BtnOk.Size = new System.Drawing.Size(110, 60);
			this.BtnOk.TabIndex = 2;
			this.BtnOk.Text = "Ok";
			this.BtnOk.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnOk.Click += new EventHandler(this.BtnOk_Click);
			this.BtnCancel.BackColor = Color.Transparent;
			this.BtnCancel.Blue = 1f;
			this.BtnCancel.Cursor = Cursors.Hand;
			this.BtnCancel.Font = new System.Drawing.Font("Tahoma", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 222);
			this.BtnCancel.Green = 1f;
			this.BtnCancel.Image = (Image)resourceManager.GetObject("BtnCancel.Image");
			this.BtnCancel.ImageClick = (Image)resourceManager.GetObject("BtnCancel.ImageClick");
			this.BtnCancel.ImageClickIndex = 0;
			this.BtnCancel.IsLock = false;
			this.BtnCancel.Location = new Point(304, 96);
			this.BtnCancel.Name = "BtnCancel";
			this.BtnCancel.ObjectValue = null;
			this.BtnCancel.Red = 1f;
			this.BtnCancel.Size = new System.Drawing.Size(110, 60);
			this.BtnCancel.TabIndex = 3;
			this.BtnCancel.Text = "Cancel";
			this.BtnCancel.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnCancel.Click += new EventHandler(this.BtnCancel_Click);
			this.AutoScaleBaseSize = new System.Drawing.Size(9, 23);
			this.BackColor = Color.White;
			base.ClientSize = new System.Drawing.Size(432, 168);
			base.Controls.Add(this.BtnCancel);
			base.Controls.Add(this.BtnOk);
			base.Controls.Add(this.LblCancalReason);
			base.Controls.Add(this.LstCancelReason);
			this.Font = new System.Drawing.Font("Tahoma", 14.25f);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			base.Name = "CancelForm";
			base.ShowInTaskbar = false;
			base.StartPosition = FormStartPosition.CenterParent;
			this.Text = "CancelForm";
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

		public static int Show(string caption)
		{
			if (CancelForm.instance == null)
			{
				CancelForm.instance = new CancelForm();
				if (CancelForm.reasons != null)
				{
					CancelForm.instance.LstCancelReason.Items.Add("Other");
					for (int i = 0; i < (int)CancelForm.reasons.Length; i++)
					{
						CancelForm.instance.LstCancelReason.Items.Add(CancelForm.reasons[i].Reason);
					}
				}
			}
			if (CancelForm.reasons == null)
			{
				return 0;
			}
			CancelForm.instance.Text = caption;
			CancelForm.instance.LstCancelReason.SelectedIndex = 0;
			CancelForm.instance.LstCancelReason.Text = CancelForm.instance.LstCancelReason.Items[0].ToString();
			CancelForm.instance.ShowDialog();
			if (!CancelForm.status)
			{
				return -1;
			}
			if (CancelForm.instance.LstCancelReason.SelectedIndex == 0)
			{
				return 0;
			}
			return CancelForm.reasons[CancelForm.instance.LstCancelReason.SelectedIndex - 1].CancelReasonID;
		}
	}
}