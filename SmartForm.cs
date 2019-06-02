using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Resources;
using System.Windows.Forms;

namespace smartRestaurant
{
	public class SmartForm : Form
	{
		private const int HEADER_HEIGHT = 60;

		private const int FORM_WIDTH = 1020;

		private const int FORM_HEIGHT = 764;

		private static System.Drawing.Font titleFont;

		private static System.Drawing.Font smallFont;

		private static System.Drawing.Font titleBoldFont;

		private static System.Drawing.Font smallBoldFont;

		private IContainer components;

		private ImageList ImgLogo;

		public SmartForm()
		{
			if (SmartForm.titleFont == null)
			{
				SmartForm.titleFont = new System.Drawing.Font("Tahoma", 30f, FontStyle.Italic, GraphicsUnit.Pixel);
				SmartForm.smallFont = new System.Drawing.Font("Tahoma", 12f, FontStyle.Italic, GraphicsUnit.Pixel);
				SmartForm.titleBoldFont = new System.Drawing.Font("Tahoma", 30f, FontStyle.Bold, GraphicsUnit.Pixel);
				SmartForm.smallBoldFont = new System.Drawing.Font("Tahoma", 12f, FontStyle.Bold, GraphicsUnit.Pixel);
			}
			ResourceManager resourceManager = new ResourceManager(typeof(SmartForm));
			this.ImgLogo = new ImageList()
			{
				ColorDepth = ColorDepth.Depth32Bit,
				ImageSize = new System.Drawing.Size(55, 59),
				ImageStream = (ImageListStreamer)resourceManager.GetObject("ImgLogo.ImageStream"),
				TransparentColor = Color.Transparent
			};
			this.BackColor = Color.White;
			this.AutoScroll = false;
			base.Size = new System.Drawing.Size(1020, 764);
			base.StartPosition = FormStartPosition.Manual;
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Font = new System.Drawing.Font("Tahoma", 12f, GraphicsUnit.Pixel);
			base.Paint += new PaintEventHandler(this.SmartForm_Paint);
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
			ResourceManager resourceManager = new ResourceManager(typeof(SmartForm));
			this.ImgLogo = new ImageList(this.components)
			{
				ColorDepth = ColorDepth.Depth32Bit,
				ImageSize = new System.Drawing.Size(55, 59),
				ImageStream = (ImageListStreamer)resourceManager.GetObject("ImgLogo.ImageStream"),
				TransparentColor = Color.Transparent
			};
			base.AutoScale = false;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			base.ClientSize = new System.Drawing.Size(292, 273);
			base.Name = "SmartForm";
		}

		private void SmartForm_Paint(object sender, PaintEventArgs e)
		{
			Rectangle rectangle = new Rectangle(0, 0, 1020, 59);
			LinearGradientBrush linearGradientBrush = new LinearGradientBrush(rectangle, Color.FromArgb(103, 138, 198), Color.White, 0f);
			e.Graphics.FillRectangle(linearGradientBrush, rectangle);
			e.Graphics.DrawLine(new Pen(Color.FromArgb(180, 180, 180)), 0, 59, 1019, 59);
			rectangle = new Rectangle(0, 60, 1020, 704);
			linearGradientBrush = new LinearGradientBrush(rectangle, Color.FromArgb(230, 230, 230), Color.White, 180f);
			e.Graphics.FillRectangle(linearGradientBrush, rectangle);
			if (this.ImgLogo != null && this.ImgLogo.Images != null && this.ImgLogo.Images.Count > 0)
			{
				e.Graphics.DrawImage(this.ImgLogo.Images[0], 0, 0);
			}
			e.Graphics.DrawString("smart", SmartForm.titleFont, Brushes.White, 55f, 5f);
			e.Graphics.DrawString("Touch", SmartForm.titleBoldFont, Brushes.White, 135f, 5f);
			e.Graphics.DrawString("for smart", SmartForm.smallFont, Brushes.White, 124f, 36f);
			e.Graphics.DrawString("Restaurant", SmartForm.smallBoldFont, Brushes.White, 175f, 36f);
		}

		public virtual void UpdateForm()
		{
		}
	}
}