using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Resources;

namespace smartRestaurant.Utils
{
	/// <summary>
	/// Summary description for WaitingForm.
	/// </summary>
	public class WaitingForm : Form
	{
		// Fields
		private IContainer components;
		private Timer IconChangeTimer;
		private static int iconCount;
		private PictureBox ImgPrinter;
		private static WaitingForm instance = null;
		private Label label1;
		private ImageList PrintImgList;

		// Methods
		public WaitingForm()
		{
			this.InitializeComponent();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && (this.components != null))
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		public static void HideForm()
		{
			if (instance != null)
			{
				instance.Hide();
				instance.IconChangeTimer.Stop();
			}
		}

		private void IconChangeTimer_Tick(object sender, EventArgs e)
		{
			this.ImgPrinter.Image = this.PrintImgList.Images[iconCount++];
			if (iconCount >= this.PrintImgList.Images.Count)
			{
				iconCount = 0;
			}
		}

		private void InitializeComponent()
		{
			this.components = new Container();
			ResourceManager manager = new ResourceManager(typeof(WaitingForm));
			this.label1 = new Label();
			this.PrintImgList = new ImageList(this.components);
			this.IconChangeTimer = new Timer(this.components);
			this.ImgPrinter = new PictureBox();
			base.SuspendLayout();
			this.label1.BackColor = Color.Transparent;
			this.label1.Location = new Point(0x60, 80);
			this.label1.Name = "label1";
			this.label1.Size = new Size(0xd0, 0x17);
			this.label1.TabIndex = 0;
			this.label1.Text = "Please wait . . .";
			this.label1.TextAlign = ContentAlignment.MiddleCenter;
			this.PrintImgList.ColorDepth = ColorDepth.Depth32Bit;
			this.PrintImgList.ImageSize = new Size(0x2c, 0x31);
			this.PrintImgList.ImageStream = (ImageListStreamer) manager.GetObject("PrintImgList.ImageStream");
			this.PrintImgList.TransparentColor = Color.Transparent;
			this.IconChangeTimer.Interval = 0x19;
			this.IconChangeTimer.Tick += new EventHandler(this.IconChangeTimer_Tick);
			this.ImgPrinter.BackColor = Color.Transparent;
			this.ImgPrinter.Location = new Point(0x20, 0x40);
			this.ImgPrinter.Name = "ImgPrinter";
			this.ImgPrinter.Size = new Size(0x2c, 0x31);
			this.ImgPrinter.TabIndex = 1;
			this.ImgPrinter.TabStop = false;
			this.AutoScaleBaseSize = new Size(9, 0x17);
			this.BackColor = Color.White;
			base.ClientSize = new Size(0x158, 0x90);
			base.Controls.AddRange(new Control[] { this.ImgPrinter, this.label1 });
			this.Cursor = Cursors.WaitCursor;
			this.Font = new Font("Tahoma", 14.25f, FontStyle.Regular, GraphicsUnit.Point, 0xde);
			base.FormBorderStyle = FormBorderStyle.None;
			base.Name = "WaitingForm";
			base.ShowInTaskbar = false;
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "Waiting";
			base.TopMost = true;
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

		public static void Show(string caption)
		{
			if (instance == null)
			{
				instance = new WaitingForm();
			}
			iconCount = 0;
			instance.ImgPrinter.Image = instance.PrintImgList.Images[iconCount++];
			instance.Text = caption;
			instance.Show();
			instance.Refresh();
			instance.IconChangeTimer.Start();
		}
	}


}
