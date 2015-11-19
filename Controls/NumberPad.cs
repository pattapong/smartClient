using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace smartRestaurant.Controls
{
	public class NumberPad : Control
	{
		// Fields
		public static int BUTTON_CANCEL = 10;
		public static int BUTTON_ENTER = 11;
		private ImageButton[] buttons;
		private Image image;
		private Image imageClick;
		private int imageClickIndex;
		private int imageIndex;
		private ImageList imageList;
		public delegate void NumberPadEventHandler(object sender, NumberPadEventArgs e);

 

		// Events
		public event NumberPadEventHandler PadClick;

		// Methods
		public NumberPad()
		{
			int num2;
			base.Width = 0xe2;
			base.Height = 0xff;
			this.buttons = new ImageButton[12];
			int x = num2 = 0;
			for (int i = 0; i < 12; i++)
			{
				this.buttons[i] = new ImageButton();
				if (i < 10)
				{
					this.buttons[i].Text = i.ToString();
				}
				else if (i == BUTTON_CANCEL)
				{
					this.buttons[i].Text = "Cor.";
				}
				else if (i == BUTTON_ENTER)
				{
					this.buttons[i].Text = "Enter";
				}
				if (i == 0)
				{
					x = 0x4d;
					num2 = 0xc3;
				}
				else if ((i >= 1) && (i <= 3))
				{
					x = (i - 1) * 0x4d;
					num2 = 130;
				}
				else if ((i >= 4) && (i <= 6))
				{
					x = (i - 4) * 0x4d;
					num2 = 0x41;
				}
				else if ((i >= 7) && (i <= 9))
				{
					x = (i - 7) * 0x4d;
					num2 = 0;
				}
				else if (i == BUTTON_CANCEL)
				{
					x = 0;
					num2 = 0xc3;
				}
				else if (i == BUTTON_ENTER)
				{
					x = 0x9a;
					num2 = 0xc3;
				}
				this.buttons[i].Location = new Point(x, num2);
				this.buttons[i].Width = 0x48;
				this.buttons[i].Blue = 0.5f;
				this.buttons[i].ObjectValue = i;
				this.buttons[i].Click += new EventHandler(this.Button_Click);
				this.buttons[i].DoubleClick += new EventHandler(this.Button_Click);
			}
			base.Controls.AddRange(this.buttons);
		}

		private void Button_Click(object sender, EventArgs e)
		{
			if ((this.buttons != null) && (this.buttons.Length > 0))
			{
				int objectValue = (int) ((ImageButton) sender).ObjectValue;
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

		// Properties
		public Image Image
		{
			get
			{
				return this.image;
			}
			set
			{
				this.image = value;
				for (int i = 0; i < this.buttons.Length; i++)
				{
					this.buttons[i].Image = this.image;
				}
			}
		}

		public Image ImageClick
		{
			get
			{
				return this.imageClick;
			}
			set
			{
				this.imageClick = value;
				for (int i = 0; i < this.buttons.Length; i++)
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

		public ImageList ImageList
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
	}
	public delegate void NumberPadEventHandler(object sender, NumberPadEventArgs e);

	public class NumberPadEventArgs : EventArgs
	{
		// Fields
		private ImageButton button;
		private int number;

		// Methods
		public NumberPadEventArgs(ImageButton button, int number)
		{
			this.button = button;
			this.number = number;
		}

		// Properties
		public ImageButton Button
		{
			get
			{
				return this.button;
			}
		}

		public bool IsCancel
		{
			get
			{
				return (this.number == NumberPad.BUTTON_CANCEL);
			}
		}

		public bool IsEnter
		{
			get
			{
				return (this.number == NumberPad.BUTTON_ENTER);
			}
		}

		public bool IsNumeric
		{
			get
			{
				return (this.number < 10);
			}
		}

		public int Number
		{
			get
			{
				return this.number;
			}
		}
	}
}
