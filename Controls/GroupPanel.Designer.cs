using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace smartRestaurant.Controls
{
	public class GroupPanel : Panel
	{
		private bool showHeader;

		private string text;

		public string Caption
		{
			get
			{
				return this.text;
			}
			set
			{
				this.text = value;
			}
		}

		public bool ShowHeader
		{
			get
			{
				return this.showHeader;
			}
			set
			{
				this.showHeader = value;
			}
		}

		public GroupPanel()
		{
			this.Font = new System.Drawing.Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.BackColor = Color.Transparent;
			this.showHeader = true;
		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			Rectangle rectangle;
			Graphics graphics = pe.Graphics;
			Pen pen = new Pen(Color.FromArgb(180, 180, 180));
			if (!this.showHeader)
			{
				rectangle = new Rectangle(0, 0, base.Width, base.Height);
				graphics.FillRectangle(Brushes.White, rectangle);
			}
			else
			{
				rectangle = new Rectangle(0, 0, base.Width, 29);
				LinearGradientBrush linearGradientBrush = new LinearGradientBrush(rectangle, Color.FromArgb(200, 200, 200), Color.White, 90f);
				graphics.FillRectangle(linearGradientBrush, rectangle);
				rectangle = new Rectangle(0, 30, base.Width, base.Height - 30);
				graphics.FillRectangle(Brushes.White, rectangle);
				graphics.DrawLine(pen, 0, 29, base.Width - 1, 29);
				graphics.DrawString(this.text, this.Font, Brushes.Black, 15f, 5f);
			}
			graphics.DrawRectangle(pen, 0, 0, base.Width - 1, base.Height - 1);
			base.OnPaint(pe);
		}
	}
}