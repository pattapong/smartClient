using smartRestaurant.Controls;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Resources;
using System.Windows.Forms;

namespace smartRestaurant.Utils
{
	public class MessageForm : Form
	{
		private static MessageForm instance;

		private Label LblText;

		private ImageButton BtnOk;

		private System.ComponentModel.Container components = null;

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

		static MessageForm()
		{
			MessageForm.instance = null;
		}

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
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			ResourceManager resourceManager = new ResourceManager(typeof(MessageForm));
			this.LblText = new Label();
			this.BtnOk = new ImageButton();
			base.SuspendLayout();
			this.LblText.BackColor = Color.Transparent;
			this.LblText.Location = new Point(8, 40);
			this.LblText.Name = "LblText";
			this.LblText.Size = new System.Drawing.Size(288, 56);
			this.LblText.TabIndex = 0;
			this.LblText.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnOk.BackColor = Color.Transparent;
			this.BtnOk.Blue = 1f;
			this.BtnOk.Cursor = Cursors.Hand;
			this.BtnOk.Font = new System.Drawing.Font("Tahoma", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 222);
			this.BtnOk.Green = 1f;
			this.BtnOk.Image = (Bitmap)resourceManager.GetObject("BtnOk.Image");
			this.BtnOk.ImageClick = (Bitmap)resourceManager.GetObject("BtnOk.ImageClick");
			this.BtnOk.ImageClickIndex = 0;
			this.BtnOk.Location = new Point(184, 96);
			this.BtnOk.Name = "BtnOk";
			this.BtnOk.ObjectValue = null;
			this.BtnOk.Red = 1f;
			this.BtnOk.Size = new System.Drawing.Size(110, 60);
			this.BtnOk.TabIndex = 1;
			this.BtnOk.Text = "Ok";
			this.BtnOk.TextAlign = ContentAlignment.MiddleCenter;
			this.BtnOk.Click += new EventHandler(this.BtnOk_Click);
			this.AutoScaleBaseSize = new System.Drawing.Size(9, 23);
			this.BackColor = Color.White;
			base.ClientSize = new System.Drawing.Size(304, 160);
			Control.ControlCollection controls = base.Controls;
			Control[] btnOk = new Control[] { this.BtnOk, this.LblText };
			controls.AddRange(btnOk);
			this.Font = new System.Drawing.Font("Tahoma", 14.25f, FontStyle.Regular, GraphicsUnit.Point, 222);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			base.Name = "MessageForm";
			base.ShowInTaskbar = false;
			base.StartPosition = FormStartPosition.CenterParent;
			this.Text = "MessageForm";
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

		public static void Show(string caption, string text)
		{
			if (MessageForm.instance == null)
			{
				MessageForm.instance = new MessageForm();
			}
			MessageForm.instance.Text = caption;
			MessageForm.instance.MessageText = text;
			MessageForm.instance.ShowDialog();
		}
	}
}