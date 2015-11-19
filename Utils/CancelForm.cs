using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace smartRestaurant.Utils
{
	/// <summary>
	/// Summary description for CancelForm.
	/// </summary>
	public class CancelForm : Form
	{
		// Fields
		private smartRestaurant.Controls.ImageButton BtnCancel;
		private smartRestaurant.Controls.ImageButton BtnOk;
		private Container components = null;
		private static CancelForm instance = null;
		private Label LblCancalReason;
		private ComboBox LstCancelReason;
		private static OrderService.CancelReason[] reasons = null;
		private static bool status = false;

		// Methods
		public CancelForm()
		{
			this.InitializeComponent();
			if (reasons == null)
			{
				reasons = new smartRestaurant.OrderService.OrderService().GetCancelReason();
			}
		}

		private void BtnCancel_Click(object sender, EventArgs e)
		{
			status = false;
			base.Close();
		}

		private void BtnOk_Click(object sender, EventArgs e)
		{
			status = true;
			base.Close();
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
			this.LstCancelReason = new System.Windows.Forms.ComboBox();
			this.LblCancalReason = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// LstCancelReason
			// 
			this.LstCancelReason.Location = new System.Drawing.Point(160, 48);
			this.LstCancelReason.Name = "LstCancelReason";
			this.LstCancelReason.Size = new System.Drawing.Size(256, 31);
			this.LstCancelReason.TabIndex = 0;
			// 
			// LblCancalReason
			// 
			this.LblCancalReason.Location = new System.Drawing.Point(16, 51);
			this.LblCancalReason.Name = "LblCancalReason";
			this.LblCancalReason.Size = new System.Drawing.Size(136, 23);
			this.LblCancalReason.TabIndex = 1;
			this.LblCancalReason.Text = "Cancel Reason";
			// 
			// CancelForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(9, 23);
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(432, 168);
			this.Controls.Add(this.LblCancalReason);
			this.Controls.Add(this.LstCancelReason);
			this.Font = new System.Drawing.Font("Tahoma", 14.25F);
			this.Name = "CancelForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "CancelForm";
			this.ResumeLayout(false);

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

		public static int Show(string caption)
		{
			if (instance == null)
			{
				instance = new CancelForm();
				if (reasons != null)
				{
					instance.LstCancelReason.Items.Add("Other");
					for (int i = 0; i < reasons.Length; i++)
					{
						instance.LstCancelReason.Items.Add(reasons[i].Reason);
					}
				}
			}
			if (reasons == null)
			{
				return 0;
			}
			instance.Text = caption;
			instance.LstCancelReason.SelectedIndex = 0;
			instance.LstCancelReason.Text = instance.LstCancelReason.Items[0].ToString();
			instance.ShowDialog();
			if (!status)
			{
				return -1;
			}
			if (instance.LstCancelReason.SelectedIndex == 0)
			{
				return 0;
			}
			return reasons[instance.LstCancelReason.SelectedIndex - 1].CancelReasonID;
		}
	}


}
