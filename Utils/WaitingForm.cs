using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Resources;
using System.Windows.Forms;

namespace smartRestaurant.Utils
{
	public class WaitingForm : Form
	{
		private Label label1;

		private ImageList PrintImgList;

		private PictureBox ImgPrinter;

		private Timer IconChangeTimer;

		private IContainer components;

		private static WaitingForm instance;

		private static int iconCount;

		static WaitingForm()
		{
			WaitingForm.instance = null;
		}

		public WaitingForm()
		{
			this.InitializeComponent();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		public static void HideForm()
		{
			if (WaitingForm.instance == null)
			{
				return;
			}
			WaitingForm.instance.Hide();
			WaitingForm.instance.IconChangeTimer.Stop();
		}

		private void IconChangeTimer_Tick(object sender, EventArgs e)
		{
			PictureBox imgPrinter = this.ImgPrinter;
			ImageList.ImageCollection images = this.PrintImgList.Images;
			int num = WaitingForm.iconCount;
			WaitingForm.iconCount = num + 1;
			imgPrinter.Image = images[num];
			if (WaitingForm.iconCount >= this.PrintImgList.Images.Count)
			{
				WaitingForm.iconCount = 0;
			}
		}

		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			ResourceManager resourceManager = new ResourceManager(typeof(WaitingForm));
			this.label1 = new Label();
			this.PrintImgList = new ImageList(this.components);
			this.IconChangeTimer = new Timer(this.components);
			this.ImgPrinter = new PictureBox();
			base.SuspendLayout();
			this.label1.BackColor = Color.Transparent;
			this.label1.Location = new Point(96, 80);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(208, 23);
			this.label1.TabIndex = 0;
			this.label1.Text = "Please wait . . .";
			this.label1.TextAlign = ContentAlignment.MiddleCenter;
			this.PrintImgList.ColorDepth = ColorDepth.Depth32Bit;
			this.PrintImgList.ImageSize = new System.Drawing.Size(44, 49);
			this.PrintImgList.ImageStream = (ImageListStreamer)resourceManager.GetObject("PrintImgList.ImageStream");
			this.PrintImgList.TransparentColor = Color.Transparent;
			this.IconChangeTimer.Interval = 25;
			this.IconChangeTimer.Tick += new EventHandler(this.IconChangeTimer_Tick);
			this.ImgPrinter.BackColor = Color.Transparent;
			this.ImgPrinter.Location = new Point(32, 64);
			this.ImgPrinter.Name = "ImgPrinter";
			this.ImgPrinter.Size = new System.Drawing.Size(44, 49);
			this.ImgPrinter.TabIndex = 1;
			this.ImgPrinter.TabStop = false;
			this.AutoScaleBaseSize = new System.Drawing.Size(9, 23);
			this.BackColor = Color.White;
			base.ClientSize = new System.Drawing.Size(344, 144);
			Control.ControlCollection controls = base.Controls;
			Control[] imgPrinter = new Control[] { this.ImgPrinter, this.label1 };
			controls.AddRange(imgPrinter);
			this.Cursor = Cursors.WaitCursor;
			this.Font = new System.Drawing.Font("Tahoma", 14.25f, FontStyle.Regular, GraphicsUnit.Point, 222);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
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

		public static void Show(string caption)
		{
			if (WaitingForm.instance == null)
			{
				WaitingForm.instance = new WaitingForm();
			}
			WaitingForm.iconCount = 0;
			PictureBox imgPrinter = WaitingForm.instance.ImgPrinter;
			ImageList.ImageCollection images = WaitingForm.instance.PrintImgList.Images;
			int num = WaitingForm.iconCount;
			WaitingForm.iconCount = num + 1;
			imgPrinter.Image = images[num];
			WaitingForm.instance.Text = caption;
			WaitingForm.instance.Show();
			WaitingForm.instance.Refresh();
			WaitingForm.instance.IconChangeTimer.Start();
		}
	}
}