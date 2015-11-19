using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Windows.Forms;

namespace smartRestaurant.Controls
{
	/// <summary>
	/// Summary description for GroupPanel.
	/// </summary>
	public class GroupPanel : Panel
	{
		// Fields
		private bool showHeader;
		private string text;

		// Methods
		public GroupPanel()
		{
			this.Font = new Font("Tahoma", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
			this.BackColor = Color.Transparent;
			this.showHeader = true;
		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			Rectangle rectangle;
			Graphics graphics = pe.Graphics;
			Pen pen = new Pen(Color.FromArgb(180, 180, 180));
			if (this.showHeader)
			{
				rectangle = new Rectangle(0, 0, base.Width, 0x1d);
				LinearGradientBrush brush = new LinearGradientBrush(rectangle, Color.FromArgb(200, 200, 200), Color.White, 90f);
				graphics.FillRectangle(brush, rectangle);
				rectangle = new Rectangle(0, 30, base.Width, base.Height - 30);
				graphics.FillRectangle(Brushes.White, rectangle);
				graphics.DrawLine(pen, 0, 0x1d, base.Width - 1, 0x1d);
				graphics.DrawString(this.text, this.Font, Brushes.Black, (float) 15f, (float) 5f);
			}
			else
			{
				rectangle = new Rectangle(0, 0, base.Width, base.Height);
				graphics.FillRectangle(Brushes.White, rectangle);
			}
			graphics.DrawRectangle(pen, 0, 0, base.Width - 1, base.Height - 1);
			base.OnPaint(pe);
		}

		// Properties
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
	}

 

}
