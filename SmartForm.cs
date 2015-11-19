using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Resources;

namespace smartRestaurant
{
	/// <summary>
	/// Summary description for SmartForm.
	/// </summary>
	public class SmartForm : Form
	{
		// Fields
		private IContainer components;
		private const int FORM_HEIGHT = 0x2fc;
		private const int FORM_WIDTH = 0x3fc;
		private const int HEADER_HEIGHT = 60;
		private ImageList ImgLogo;
		private static Font smallBoldFont;
		private static Font smallFont;
		private static Font titleBoldFont;
		private static Font titleFont;

		// Methods
		public SmartForm()
		{
			if (titleFont == null)
			{
				titleFont = new Font("Tahoma", 30f, FontStyle.Italic, GraphicsUnit.Pixel);
				smallFont = new Font("Tahoma", 12f, FontStyle.Italic, GraphicsUnit.Pixel);
				titleBoldFont = new Font("Tahoma", 30f, FontStyle.Bold, GraphicsUnit.Pixel);
				smallBoldFont = new Font("Tahoma", 12f, FontStyle.Bold, GraphicsUnit.Pixel);
			}
			ResourceManager manager = new ResourceManager(typeof(SmartForm));
			this.ImgLogo = new ImageList();
			this.ImgLogo.ColorDepth = ColorDepth.Depth32Bit;
			this.ImgLogo.ImageSize = new Size(0x37, 0x3b);
			this.ImgLogo.ImageStream = (ImageListStreamer) manager.GetObject("ImgLogo.ImageStream");
			this.ImgLogo.TransparentColor = Color.Transparent;
			this.BackColor = Color.White;
			this.AutoScroll = false;
			base.Size = new Size(0x3fc, 0x2fc);
			base.StartPosition = FormStartPosition.Manual;
			base.FormBorderStyle = FormBorderStyle.None;
			this.Font = new Font("Tahoma", 12f, GraphicsUnit.Pixel);
			base.Paint += new PaintEventHandler(this.SmartForm_Paint);
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
			ResourceManager manager = new ResourceManager(typeof(SmartForm));
			this.ImgLogo = new ImageList(this.components);
			this.ImgLogo.ColorDepth = ColorDepth.Depth32Bit;
			this.ImgLogo.ImageSize = new Size(0x37, 0x3b);
			this.ImgLogo.ImageStream = (ImageListStreamer) manager.GetObject("ImgLogo.ImageStream");
			this.ImgLogo.TransparentColor = Color.Transparent;
			base.AutoScale = false;
			this.AutoScaleBaseSize = new Size(5, 13);
			base.ClientSize = new Size(0x124, 0x111);
			base.Name = "SmartForm";
		}

		private void SmartForm_Paint(object sender, PaintEventArgs e)
		{
			Rectangle rect = new Rectangle(0, 0, 0x3fc, 0x3b);
			LinearGradientBrush brush = new LinearGradientBrush(rect, Color.FromArgb(0x67, 0x8a, 0xc6), Color.White, 0f);
			e.Graphics.FillRectangle(brush, rect);
			e.Graphics.DrawLine(new Pen(Color.FromArgb(180, 180, 180)), 0, 0x3b, 0x3fb, 0x3b);
			rect = new Rectangle(0, 60, 0x3fc, 0x2c0);
			brush = new LinearGradientBrush(rect, Color.FromArgb(230, 230, 230), Color.White, 180f);
			e.Graphics.FillRectangle(brush, rect);
			if (((this.ImgLogo != null) && (this.ImgLogo.Images != null)) && (this.ImgLogo.Images.Count > 0))
			{
				e.Graphics.DrawImage(this.ImgLogo.Images[0], 0, 0);
			}
			e.Graphics.DrawString("smart", titleFont, Brushes.White, (float) 55f, (float) 5f);
			e.Graphics.DrawString("Touch", titleBoldFont, Brushes.White, (float) 135f, (float) 5f);
			e.Graphics.DrawString("for smart", smallFont, Brushes.White, (float) 124f, (float) 36f);
			e.Graphics.DrawString("Restaurant", smallBoldFont, Brushes.White, (float) 175f, (float) 36f);
		}

		public virtual void UpdateForm()
		{
		}
	}

 

}
