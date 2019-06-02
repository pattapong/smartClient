using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace smartRestaurant.Controls
{
	public class NumberPad : Control
	{
		public static int BUTTON_CANCEL;

		public static int BUTTON_ENTER;

		private System.Windows.Forms.ImageList imageList;

		private System.Drawing.Image image;

		private System.Drawing.Image imageClick;

		private int imageIndex;

		private int imageClickIndex;

		private ImageButton[] buttons;

		public System.Drawing.Image Image
		{
			get
			{
				return this.image;
			}
			set
			{
				this.image = value;
				for (int i = 0; i < (int)this.buttons.Length; i++)
				{
					this.buttons[i].Image = this.image;
				}
			}
		}

		public System.Drawing.Image ImageClick
		{
			get
			{
				return this.imageClick;
			}
			set
			{
				this.imageClick = value;
				for (int i = 0; i < (int)this.buttons.Length; i++)
				{
					this.buttons[i].ImageClick = this.imageClick;
				}
			}
		}

		public int ImageClickIndex
		{
			get
			{
				return this.imageClickIndex;
			}
			set
			{
				this.imageClickIndex = value;
				if (this.imageList != null)
				{
					this.ImageClick = this.imageList.Images[this.imageClickIndex];
				}
			}
		}

		public int ImageIndex
		{
			get
			{
				return this.imageIndex;
			}
			set
			{
				this.imageIndex = value;
				if (this.imageList != null)
				{
					this.Image = this.imageList.Images[this.imageIndex];
				}
			}
		}

		public System.Windows.Forms.ImageList ImageList
		{
			get
			{
				return this.imageList;
			}
			set
			{
				this.imageList = value;
			}
		}

		static NumberPad()
		{
			NumberPad.BUTTON_CANCEL = 10;
			NumberPad.BUTTON_ENTER = 11;
		}

		public NumberPad()
		{
			base.Width = 226;
			base.Height = 255;
			this.buttons = new ImageButton[12];
			int num = 0;
			int num1 = num;
			int num2 = num;
			for (int i = 0; i < 12; i++)
			{
				this.buttons[i] = new ImageButton();
				if (i < 10)
				{
					this.buttons[i].Text = i.ToString();
				}
				else if (i == NumberPad.BUTTON_CANCEL)
				{
					this.buttons[i].Text = "Cor.";
				}
				else if (i == NumberPad.BUTTON_ENTER)
				{
					this.buttons[i].Text = "Enter";
				}
				if (i == 0)
				{
					num2 = 77;
					num1 = 195;
				}
				else if (i >= 1 && i <= 3)
				{
					num2 = (i - 1) * 77;
					num1 = 130;
				}
				else if (i >= 4 && i <= 6)
				{
					num2 = (i - 4) * 77;
					num1 = 65;
				}
				else if (i >= 7 && i <= 9)
				{
					num2 = (i - 7) * 77;
					num1 = 0;
				}
				else if (i == NumberPad.BUTTON_CANCEL)
				{
					num2 = 0;
					num1 = 195;
				}
				else if (i == NumberPad.BUTTON_ENTER)
				{
					num2 = 154;
					num1 = 195;
				}
				this.buttons[i].Location = new Point(num2, num1);
				this.buttons[i].Width = 72;
				this.buttons[i].Blue = 0.5f;
				this.buttons[i].ObjectValue = i;
				this.buttons[i].Click += new EventHandler(this.Button_Click);
				this.buttons[i].DoubleClick += new EventHandler(this.Button_Click);
			}
			base.Controls.AddRange(this.buttons);
		}

		private void Button_Click(object sender, EventArgs e)
		{
			if (this.buttons != null && (int)this.buttons.Length > 0)
			{
				int objectValue = (int)((ImageButton)sender).ObjectValue;
				this.OnPadClick(new NumberPadEventArgs(this.buttons[objectValue], objectValue));
			}
		}

		protected virtual void OnPadClick(NumberPadEventArgs e)
		{
			if (this.PadClick != null)
			{
				this.PadClick(this, e);
			}
		}

		public event NumberPadEventHandler PadClick;
	}
}