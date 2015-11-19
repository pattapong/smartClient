using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace smartRestaurant.Utils
{
	/// <summary>
	/// Summary description for MessageForm.
	/// </summary>
	public class MessageForm : Form
	{
		// Fields
		private smartRestaurant.Controls.ImageButton BtnOk;
		private Container components = null;
		private static MessageForm instance = null;
		private Label LblText;

		// Methods
		public MessageForm()
		{
			this.InitializeComponent();
		}

		private void BtnOk_Click(object sender, EventArgs e)
		{
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
			this.LblText = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// LblText
			// 
			this.LblText.BackColor = System.Drawing.Color.Transparent;
			this.LblText.Location = new System.Drawing.Point(8, 40);
			this.LblText.Name = "LblText";
			this.LblText.Size = new System.Drawing.Size(288, 56);
			this.LblText.TabIndex = 0;
			this.LblText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// MessageForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(9, 23);
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(304, 160);
			this.Controls.Add(this.LblText);
			this.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(222)));
			this.Name = "MessageForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "MessageForm";
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

		public static void Show(string caption, string text)
		{
			if (instance == null)
			{
				instance = new MessageForm();
			}
			instance.Text = caption;
			instance.MessageText = text;
			instance.ShowDialog();
		}

		// Properties
		public string MessageText
		{
			get
			{
				return this.LblText.Text;
			}
			set
			{
				this.LblText.Text = value;
			}
		}
	}


}
